using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace FTempoToFPExporter
{
    class Program
    {
        static void Main(string[] args)
        {

            //---------------------------------if last week then check this-------------------------------------------------------------------------

            var IsLastWeekTimeRequest = false;
            if (args.Length > 0)
            {
                if (args[0] == "lastweek")
                {
                    IsLastWeekTimeRequest = true;
                }
            }

            //----------------------------------------------SORT OUT THE DATES ------------------------------------------------
            DayOfWeek weekStart = DayOfWeek.Monday;
            DateTime nowDate = DateTime.Today;
            DateTime startingDate = DateTime.Today;

            // If we want last week then generate a file as if it were run on Sunday night
            if (IsLastWeekTimeRequest)
            {
                nowDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
                startingDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
            }

            while (startingDate.DayOfWeek != weekStart)
                startingDate = startingDate.AddDays(-1);

            var nowDateStr = DateTime.Today.Date.ToString("yyyy-MM-dd").Substring(0, 10);
            var nowEODStr = nowDate.Date.AddDays(1).ToString("yyyy-MM-dd").Substring(0, 10);
            var startingDateStr = startingDate.Date.ToString("yyyy-MM-dd").Substring(0, 10);


            //-----------------------------------------------GRAB THE DIFFERENCE DATA ----------------------------------------------------

            var theJIRAServer = ConfigurationManager.AppSettings["theJIRAConnection"];
            var theTempoToken = ConfigurationManager.AppSettings["theTempoToken"];
            var url = theJIRAServer + "/plugins/servlet/tempo-getWorklog/?dateFrom=" + startingDateStr + "&dateTo=" + nowDateStr + "&format=xml&diffOnly=false&tempoApiToken=" + theTempoToken;
            string xml;

            Console.WriteLine("startingdate: " + startingDateStr);
            Console.WriteLine("endingdate: " + nowDateStr);

            using (var webClient = new System.Net.WebClient())
            {
                webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatable; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705");
                xml = webClient.DownloadString(url);
            }

            // Tidy up the incomign XML to make it load into a datatable successfully.
            if (xml != null && xml.Contains("<worklogs "))
            {
                var removeUpToThis = "?>";
                int placeToStart = xml.IndexOf(removeUpToThis);
                xml = xml.Substring(placeToStart + 2, xml.Length - placeToStart - 2);
                var searchForThis = ">";
                int firstCharacter = xml.IndexOf(searchForThis);
                xml = xml.Substring(firstCharacter, xml.Length - firstCharacter);
                xml = "<worklogs " + xml;
                xml = xml.Replace("</work_description>", "</work_description>\n");
            }

            DataSet ds = new DataSet();

            ds.ReadXml(new System.IO.StringReader(xml));

            SaveStatus(ds, startingDateStr);
        }
        
        //------------------------------------------------UPLOAD THE FILE TO THE JIRA TABLE----------------------------------------------------------

        public static int SaveStatus(DataSet objdatasetdto, String startingDateStr)
        {

            var theSQLConnection = ConfigurationManager.AppSettings["theFPConnection"];
            SqlConnection objcon = new SqlConnection(theSQLConnection);

            DataTable thetable = objdatasetdto.Tables[0];
            
            // FIRST delete any adhoc runs that have not yet been processed - otherwise we'll end up with duplicates
            var deletePrevUploadQuery = @"delete from UDEF_TS_JIRA_STAGING where FPHeaderID is null and (Status !='U' and Status !='P') or Status is null";

            SqlDataAdapter objdaDelete = new SqlDataAdapter(deletePrevUploadQuery, objcon);
            objcon.Open();
            objdaDelete.SelectCommand.ExecuteNonQuery();
            objcon.Close();

            // Filter out any records from the Datatable that have empty FP1/FP2/FP3 codes
            DataView theDataView = new DataView(thetable);
            theDataView.RowFilter = "customField_12802 is not null AND customField_12803 is not null AND customField_12901 is not null ";
            DataTable theFilteredtable = theDataView.ToTable();

            Console.Out.WriteLine("Filtered to: " + theFilteredtable.Rows.Count + " from " + thetable.Rows.Count);

            foreach (DataRow row in theFilteredtable.Rows)
            {
                try
                {
                    var theFPProject = row.Field<string>("customField_12802");
                    var theFPProjectLength = theFPProject.Length;
                    var theFPAccountCode = row.Field<string>("customField_12803");
                    var theFPAccountCodeLength = theFPAccountCode.Length;
                    var theFPCustomer = row.Field<string>("customField_12901");
                    var theFPAccountCustomerLength = theFPCustomer.Length;
                    var theTempoID = row.Field<string>("worklog_id");
                    var theIssueKey = row.Field<string>("issue_key");
                    var theUserID = row.Field<string>("username");
                    var theHours = row.Field<string>("hours");

                    // This is some debugging code if someone says their records didn't appear....
                    //if (theUserID == "iain.ross@bradyplc.com")
                    //{
                    //    Console.Out.WriteLine("Iain: " + theFPProject +"/" + theFPAccountCode +"/" + theFPCustomer);
                    //}

                    if (theFPProjectLength != 0 && theFPAccountCodeLength != 0)
                    {
                        if (theFPProjectLength > 10)
                        {
                            theFPProject = row.Field<string>("customField_12802").Substring(0, 10);
                        }
                        var theDate = row.Field<string>("work_date");
                        var theWorkDescription = row.Field<string>("work_description");
                        if (theWorkDescription.Contains("'"))
                        {
                            theWorkDescription = theWorkDescription.Replace("'", "''");
                        }
                        if (theWorkDescription.Length >= 180)
                        {
                            theWorkDescription = row.Field<string>("work_description").Substring(0, 180);
                        }
                        if (theUserID.Length >= 64)
                        {
                            theUserID = theUserID.Substring(0, 64);
                        }
                        if (theFPAccountCode.Contains("- ("))
                        {
                            var searchForThis = "- (";
                            int firstCharacter = theFPAccountCode.IndexOf(searchForThis);
                            theFPAccountCode = theFPAccountCode.Substring(0, firstCharacter);
                        }


                        // Now put in the results if this is a new entry for that day, userID, fpProject and costcenter
                        var queryTimeInsert = @"insert into UDEF_TS_JIRA_STAGING (UserID, 
                                                                           WorkDescription, 
                                                                           Hours,  
                                                                           DateofWork, 
                                                                           FPProject, 
                                                                           FPCostCentre, 
                                                                           IDinTempo, 
                                                                           Customer) 
                                               select                '" + theUserID + "', '"
                                                                          + theIssueKey + "', " //work description as JIRA number
                                                                          + theHours + ", '"
                                                                          + theDate + "', '"
                                                                          + theFPProject + "',  '"
                                                                          + theFPAccountCode + "', '"
                                                                          + theTempoID + "', ' "
                                                                          + theFPCustomer + "'  where not exists " +
                                              "(select userID, DateofWork, FPProject, FPCostCentre from UDEF_TS_JIRA_STAGING "
                                                                          + "where userID =  '" + theUserID + "' and "
                                                                          + "DateofWork =  '" + theDate + "' and "
                                                                          + "FPProject =  '" + theFPProject + "' and "
                                                                          + "FPCostCentre =  '" + theFPAccountCode + "' and FPHeaderID is null)";

                        SqlDataAdapter objda = new SqlDataAdapter(queryTimeInsert, objcon);
                        objcon.Open();
                        objda.SelectCommand.ExecuteNonQuery();
                        objcon.Close();

                        // Now update the newly input results if this is a new entry for that day, userID, fpProject and costcenter
                        // concatenate hours and work description if UID, Project and Cost Centre match.
                        // Dont do this if tempo ID happens to be the one used in the previous insert statement
                        var queryUpdateTime = @"update UDEF_TS_JIRA_STAGING set hours = hours + " + theHours +
                                               " , WorkDescription = WorkDescription + '\r\n' + '" + theIssueKey +
                                                "' where  userID = '" + theUserID + "'" +
                                                " and DateofWork = '" + theDate + "'" +
                                                " and FPProject = '" + theFPProject + "'" +
                                                " and FPCostCentre = '" + theFPAccountCode + "' " +
                                                " and IDinTempo != '" + theTempoID + "' " +
                                                " and FPHeaderID is null";

                        SqlDataAdapter objdaUpdate = new SqlDataAdapter(queryUpdateTime, objcon);
                        objcon.Open();
                        objdaUpdate.SelectCommand.ExecuteNonQuery();
                        objcon.Close();
                        var RowCount = theFilteredtable.Rows.IndexOf(row) +1 ;
                        Console.Out.WriteLine("Record: " + RowCount + " of " + theFilteredtable.Rows.Count);
                    }
                }
                catch(SqlException sqlEx)
                {
                    Console.Out.WriteLine("SQLException: " + sqlEx);
                    return 0;
                }
            }

            
            // Get rid of data older than 6 months old
            var thedate = startingDateStr;
            var queryPurgeOlddata = @"delete from UDEF_TS_JIRA_STAGING where dateofwork <  DATEADD(month, -6, '" + thedate + "')";
            SqlDataAdapter objda12 = new SqlDataAdapter(queryPurgeOlddata, objcon);
            objcon.Open();
            objda12.SelectCommand.ExecuteNonQuery();
            objcon.Close();

            // Get rid of absence data - approval process is in FocalPoint
            var querydeleteAbsence = @"delete from UDEF_TS_JIRA_STAGING where FPProject in ('ABSENCE101', 'INT-1', 'INT-2', 'INT-16', 'INT-18', 'INT-23')";
            SqlDataAdapter objda13 = new SqlDataAdapter(querydeleteAbsence, objcon);
            objcon.Open();
            objda13.SelectCommand.ExecuteNonQuery();
            objcon.Close();


            // Mark rows as P for previous if the worklog is for the previous week
            var queryPrevious = @"update UDEF_TS_JIRA_STAGING set Status = 'P'  where FPDetailID is not null and dateofwork <  '" + thedate + "'";
            SqlDataAdapter objda0 = new SqlDataAdapter(queryPrevious, objcon);
            objcon.Open();
            objda0.SelectCommand.ExecuteNonQuery();
            objcon.Close();


            // Mark rows as deleted if the worklog doesnt come through
            // We dont consider rows that are previous.  These are already approved in FP so cannot be deleted even if this has been done in JIRA
            var queryDelete = @"insert into UDEF_TS_JIRA_STAGING (UserID, 
                                    WorkDescription, 
                                    Hours,  
                                    DateofWork, 
                                    FPProject, 
                                    FPCostCentre, 
                                    IDinTempo, 
                                    Customer,
                                    FPDetailID,
                                    Status) 
                                    SELECT DISTINCT UserID, 
                                    WorkDescription, 
                                    Hours,  
                                    DateofWork, 
                                    FPProject, 
                                    FPCostCentre, 
                                    IDinTempo, 
                                    Customer,
                                    'deleted',
                                    'D' 
                                    FROM UDEF_TS_JIRA_STAGING as a
                                    WHERE not exists
                                    (
                                        select * from UDEF_TS_JIRA_STAGING b
                                        where a.userID = b.userID
                                        AND a.DateofWork = b.DateofWork
                                        AND a.FPProject = b.FPProject
                                        AND a.FPCostCentre = b.FPCostCentre
                                        AND b.FPDetailID is null)
                                    AND a.FPDetailID is not null
                                    AND a.status !='P'";
            SqlDataAdapter objda2 = new SqlDataAdapter(queryDelete, objcon);
            objcon.Open();
            objda2.SelectCommand.ExecuteNonQuery();
            objcon.Close();

            // Mark rows as U again as they have been uploaded
            var queryUploadMark = @"update UDEF_TS_JIRA_STAGING set Status = 'U'  where Status = 'P'";
            SqlDataAdapter objdaa = new SqlDataAdapter(queryUploadMark, objcon);
            objcon.Open();
            objdaa.SelectCommand.ExecuteNonQuery();
            objcon.Close();

            // Mark new rows where the ID is not already in FP as NEW
            var queryNewRows = @"UPDATE a 
                                SET a.Status= 'N'
                                FROM UDEF_TS_JIRA_STAGING as a
                                WHERE not exists
                                (
                                    select * from UDEF_TS_JIRA_STAGING b
                                    where a.userID = b.userID
                                    AND a.DateofWork = b.DateofWork
                                    AND a.FPProject = b.FPProject
                                    AND a.FPCostCentre = b.FPCostCentre
                                    AND b.FPDetailID is not null)
                                AND 
                                a.FPDetailID is null";

            SqlDataAdapter objda3 = new SqlDataAdapter(queryNewRows, objcon);
            objcon.Open();
            objda3.SelectCommand.ExecuteNonQuery();
            objcon.Close();

            // Mark records as S for same where the record is coming through the same
            var queryMarkSame = @" update UDEF_TS_JIRA_STAGING set Status = 'S' where Status is null and exists
                                                (select * from UDEF_TS_JIRA_STAGING b where UDEF_TS_JIRA_STAGING.hours = b.Hours
                                                    and UDEF_TS_JIRA_STAGING.DateofWork = b.DateofWork
                                                    and UDEF_TS_JIRA_STAGING.FPProject = b.FPProject
                                                    and UDEF_TS_JIRA_STAGING.FPCostCentre = b.FPCostCentre
                                                    and UDEF_TS_JIRA_STAGING.Customer = b.Customer
                                                    and UDEF_TS_JIRA_STAGING.WorkDescription = b.WorkDescription
                                                    and Status is not null)";
            SqlDataAdapter objda4 = new SqlDataAdapter(queryMarkSame, objcon);
            objcon.Open();
            objda4.SelectCommand.ExecuteNonQuery();
            objcon.Close();

            // Delete the rows where the record is coming through the same
            var queryDeleteSame = @" delete from UDEF_TS_JIRA_STAGING where Status = 'S' and FPDetailID is null";
            SqlDataAdapter objda5 = new SqlDataAdapter(queryDeleteSame, objcon);
            objcon.Open();
            objda5.SelectCommand.ExecuteNonQuery();
            objcon.Close();


            // CHANGED everthing leftover is a change but double redundancy check here to make sure that UID, date, FPproject and costcenter are the same
            // as a previous record.
            var queryChangedRows = @"update a 
                                    SET a.Status= 'C'
                                    FROM UDEF_TS_JIRA_STAGING as a
                                    INNER JOIN UDEF_TS_JIRA_STAGING as b 
                                    ON a.userID = b.userID
                                    AND a.DateofWork = b.DateofWork
                                    AND a.FPProject = b.FPProject
                                    AND a.FPCostCentre = b.FPCostCentre
                                    WHERE 
                                    a.FPDetailID is null
                                    AND b.FPDetailID is not null
                                    AND a.status is null";
            SqlDataAdapter objda6 = new SqlDataAdapter(queryChangedRows, objcon);
            objcon.Open();
            objda6.SelectCommand.ExecuteNonQuery();
            objcon.Close();

            // CHANGED - done another JIRA the same day for the same project and cost center
            // These are already combined in the insert to this table but there might be a row ALREADY in the table
            // So these are also a change.
            // Mark new rows where the ID is not already in FP as NEW
            var queryExistingDupsChanged = @"UPDATE a 
                                SET a.Status= 'N'
                                FROM UDEF_TS_JIRA_STAGING as a
                                WHERE not exists
                                (
                                    select * from UDEF_TS_JIRA_STAGING b
                                    where a.userID = b.userID
                                    AND a.DateofWork = b.DateofWork
                                    AND a.FPProject = b.FPProject
                                    AND a.FPCostCentre = b.FPCostCentre
                                    AND b.FPDetailID is not null)
                                AND 
                                a.FPDetailID is null";

            SqlDataAdapter objda31 = new SqlDataAdapter(queryExistingDupsChanged, objcon);
            objcon.Open();
            objda31.SelectCommand.ExecuteNonQuery();
            objcon.Close();

            return 1;
        }
    }
}


