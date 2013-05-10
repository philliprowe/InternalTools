using System;
using System.Data;
using System.Data.SqlClient;

namespace FTempoToFPExporter
{
    class Program
    {
        static void Main(string[] args)
        {

            //---------------------------------if last week then check this-------------------------------------------------------------------------

            bool IsLastWeekTimeRequest = false;

            if (args.Length > 0)
            {
                IsLastWeekTimeRequest = true;
            }

            //----------------------------------------------SORT OUT THE DATES ------------------------------------------------
            DayOfWeek weekStart = DayOfWeek.Monday;
            DateTime nowDate = DateTime.Today;
            DateTime startingDate = DateTime.Today;

            //If we want last week then generate a file as if it were run on Sunday night
            if (IsLastWeekTimeRequest)
            {
                nowDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
                startingDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
            }

            while (startingDate.DayOfWeek != weekStart)
                startingDate = startingDate.AddDays(-1);

            String nowDateStr = nowDate.Date.ToString("yyyy-MM-dd").Substring(0, 10);
            String nowEODStr = nowDate.Date.AddDays(1).ToString("yyyy-MM-dd").Substring(0, 10);
            String startingDateStr = startingDate.Date.ToString("yyyy-MM-dd").Substring(0, 10);


            //-----------------------------------------------SAVE OUT THE DIFFERENCE FILE----------------------------------------------------

            // THIS GIVES A SERVER ERROR 500
            string url = @"https://support.bradyplc.com/plugins/servlet/tempo-getWorklog/?dateFrom=" + startingDateStr + "&dateTo=" + nowDateStr + "&format=xml&diffOnly=false&tempoApiToken=b7201cde-15d4-4e51-8f98-2d65456ce422";
            string xml;
            //XmlSchema thexml;

            using (var webClient = new System.Net.WebClient())
            {
                webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatable; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705");
                xml = webClient.DownloadString(url);
            }

            if (xml != null && xml.Contains("<worklogs "))
            {
                string removeUpToThis = "?>";
                int placeToStart = xml.IndexOf(removeUpToThis);
                xml = xml.Substring(placeToStart + 2, xml.Length - placeToStart - 2);
                string searchForThis = ">";
                int firstCharacter = xml.IndexOf(searchForThis);
                xml = xml.Substring(firstCharacter, xml.Length - firstCharacter);
                xml = "<worklogs " + xml;
                xml = xml.Replace("</work_description>", "</work_description>\n");
            }

            DataSet ds = new DataSet();

            //XML IDEA
            ds.ReadXml(new System.IO.StringReader(xml));

            SaveStatus(ds, startingDateStr);
        }


        //------------------------------------------------UPLOAD THE FILE TO THE JIRA TABLE----------------------------------------------------------

        public static int SaveStatus(DataSet objdatasetdto, String startingDateStr)
        {
            //var keyz = System.Configuration.ConfigurationSettings.AppSettings.Keys;
            //string s = string.Empty;
            //foreach (var key in keyz)
            //{
            //    s += key.ToString() + "\t";
            //}
            String theConnection = System.Configuration.ConfigurationSettings.AppSettings["theFPConnection"];
            //@"Server=AQUASQL\SQL2008;Database=A_JR_AQUADEMO;User id=support;Password=sqlsql";
            SqlConnection objcon = new SqlConnection(theConnection);
            //(System.Configuration.ConfigurationManager.ConnectionStrings[@"Data source=AQUASQL\SQL2008;Initial catalog=A_JR_AQUADEMO;User id=support;Password=sqlsql"].ToString());

            int theRows = objdatasetdto.Tables[0].Rows.Count;
            DataTable thetable = objdatasetdto.Tables[0];

            DataRow therow = thetable.Rows[1];


            for (int i = 0; i < objdatasetdto.Tables[0].Rows.Count; i++)
            {
                try
                {
                    var theFPProject = thetable.Rows[i].ItemArray[20].ToString();
                    var theFPProjecLength = theFPProject.Length;
                    var theFPAccountCode = thetable.Rows[i].ItemArray[21].ToString();
                    var theFPAccountCodeLength = theFPAccountCode.Length;
                    var theFPCustomer = thetable.Rows[i].ItemArray[22].ToString();
                    var theFPAccountCustomerLength = theFPCustomer.Length;
                    var theTempoID = thetable.Rows[i].ItemArray[0];
                    string theRole = "";
                    if (theFPProjecLength != 0 && theFPAccountCodeLength != 0)
                    {
                        var theDate = thetable.Rows[i].ItemArray[5];
                        var theWorkDescription = thetable.Rows[i].ItemArray[12].ToString();
                        if (theWorkDescription.Contains("'"))
                        {
                            theWorkDescription = theWorkDescription.Replace("'", "''");
                        }
                        if (theWorkDescription.Length >= 235)
                        {
                            theWorkDescription = thetable.Rows[i].ItemArray[12].ToString().Substring(0, 234);
                        }
                        if (theFPAccountCode.Contains("- ("))
                        {
                            string searchForThis = "- (";
                            int firstCharacter = theFPAccountCode.IndexOf(searchForThis);
                            theFPAccountCode = theFPAccountCode.Substring(0, firstCharacter);
                        }

                        // Sort out the roles. Possibly no need to do this now that its in the list.
                        if (thetable.Rows[i].ItemArray[9].ToString().Contains("Overtime"))
                        {
                            theRole = "slave";
                        }
                        else if (thetable.Rows[i].ItemArray[9].ToString().Contains("Consulting"))
                        {
                            theRole = "consultant";
                        }
                        else if (thetable.Rows[i].ItemArray[9].ToString().Contains("Developer"))
                        {
                            theRole = "developer";
                        }
                        else if (thetable.Rows[i].ItemArray[9].ToString().Contains("Tester"))
                        {
                            theRole = "tester";
                        }

                        string query = @"insert into JGTest (UserID, WorkDescription, Hours,  DateofWork, FPProject, FPCostCenter, WorkerRole, IDinTempo, Customer) 
                                               values ('" + thetable.Rows[i].ItemArray[7].ToString() + "', '" + thetable.Rows[i].ItemArray[2].ToString()
                                                          + " : " + theWorkDescription + "', " + thetable.Rows[i].ItemArray[3] + ", '"
                                                          + theDate + "', '" + theFPProject + "',  '" + theFPAccountCode + "', '" + theRole + "',"
                                                          + thetable.Rows[i].ItemArray[0] + ",  '" + theFPCustomer + "')";

                        SqlDataAdapter objda = new SqlDataAdapter(query, objcon);
                        objcon.Open();
                        objda.SelectCommand.ExecuteNonQuery();
                        objcon.Close();
                    }
                }
                catch
                {
                    return 0;
                }
            }

            String thedate = startingDateStr;
            // Mark rows as P for previous if the worklog is for the previous week
            string previousquery = @"update JGTest set Status = 'P'  where FPDetailID is not null and dateofwork <  '" + thedate + "'";
            SqlDataAdapter objda0 = new SqlDataAdapter(previousquery, objcon);
            objcon.Open();
            objda0.SelectCommand.ExecuteNonQuery();
            objcon.Close();


            // Mark rows as deleted if the worklog doesnt come through
            string deletequery = @"update JGTest set Status = 'D'  where FPDetailID is not null and status !='P' and IDintempo not in 
                                                                          (select IDintempo from JGTest where FPDetailID is  null)";
            SqlDataAdapter objda2 = new SqlDataAdapter(deletequery, objcon);
            objcon.Open();
            objda2.SelectCommand.ExecuteNonQuery();
            objcon.Close();

            // Mark new rows where the ID is not already in FP
            string newquery = @"update JGTest set Status = 'N'  where FPDetailID is  null and IDintempo not in 
                                                                 (select IDintempo from JGTest where FPDetailID is not null)";
            SqlDataAdapter objda3 = new SqlDataAdapter(newquery, objcon);
            objcon.Open();
            objda3.SelectCommand.ExecuteNonQuery();
            objcon.Close();

            // Mark records as S for same where the record is coming through the same
            string markSamequery = @" update JGTest set Status = 'S' where Status is null and exists
                                                (select * from JGTest b where JGTest.IDintempo = b.IDinTempo
                                                    and JGTest.hours = b.Hours
                                                    and JGTest.DateofWork = b.DateofWork
                                                    and JGTest.FPProject = b.FPProject
                                                    and JGTest.FPCostCenter = b.FPCostCenter
                                                    and JGTest.WorkerRole = b.WorkerRole
                                                    and JGTest.Customer = b.Customer
                                                    and Status is not null)";
            SqlDataAdapter objda4 = new SqlDataAdapter(markSamequery, objcon);
            objcon.Open();
            objda4.SelectCommand.ExecuteNonQuery();
            objcon.Close();

            // Delete the rows where the record is coming through the same
            string deleteSamequery = @" delete from JGTest where Status = 'S' and FPDetailID is null";
            SqlDataAdapter objda5 = new SqlDataAdapter(deleteSamequery, objcon);
            objcon.Open();
            objda5.SelectCommand.ExecuteNonQuery();
            objcon.Close();

            //CHANGED Now mark the changed rows as changed
            string changedquery = @"update JGTest set Status = 'C' where IDinTempo in
                                                     (select IDinTempo where Status is null and FPDetailID is null)";
            SqlDataAdapter objda6 = new SqlDataAdapter(changedquery, objcon);
            objcon.Open();
            objda6.SelectCommand.ExecuteNonQuery();
            objcon.Close();


            return 1;
        }
    }
}


