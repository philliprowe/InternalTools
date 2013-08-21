using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using Microsoft.VisualBasic;
using System.Drawing;
using System.Net.Mail;
namespace ActivationKeyGenerator
{
    public partial class Generate : System.Web.UI.Page
    {
        // The source of the Client text files
        string path = "C:\\ActivationKeyGenerator\\_Clients";
                              
        protected void Page_Load(object sender, EventArgs e)
        {
            //Creates a warning box when the amend client details button is pressed
            AmendBttn.Attributes.Add("onclick", "return confirm('Warning: This will amend this Client\\'s details.\\r\\nAmend this Client?');");
            PushsftpBttn.Attributes.Add("onclick", "return confirm('Warning: This will push the Client\\'s Activation Key to their SFTP. \\r\\n Push Activation Key to SFTP?');");


            if (ClientDDL.Items.Count == 0) //The first time the form is loaded, when the client list is unpopulated
            {
                try
                {
                    ClientDDL.Focus();
                    ClearAllActivationStuff();
                    SystemCodeTB.ForeColor = Color.Black;
                    AmendBttn.Enabled = false;
                    ActivationKeyBttn.Enabled = false;
                    //Find the names of the clients from the directory
                    ClientDDL.Items.Add("Select a client.");
                    string[] clientsIncPath = Directory.GetFiles(path, "*.txt");
                    foreach (string ClientName in clientsIncPath)
                    {
                        string Client = Path.GetFileNameWithoutExtension(ClientName);
                        if (!Client.StartsWith("_") & !Client.StartsWith("$") & !Client.StartsWith("%"))
                        {
                            ClientDDL.Items.Add(Client);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Normally when the Client list has been moved
                    Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('Could not get Client List.');", true);
                    ClientLabel.Text = ex.Message;
                    ClientLabel.Visible = true;
                }
            }
        }

               
        
        protected string getClientTextFile()
        {
            //Gets the path of the client selected in the client list
            string thisclient = ClientDDL.SelectedValue;
            string[] clientsIncPath = Directory.GetFiles(path, "*.txt");
            string clientTextFile = string.Empty;
            foreach (string clientPath in clientsIncPath)
            {
                string Client = Path.GetFileNameWithoutExtension(clientPath);
                if ((Client == thisclient) & !Client.StartsWith("_") & !Client.Contains("$") & !Client.Contains("%"))
                {
                    return clientPath;
                }
            }
            return string.Empty;
        }
        
        protected void ClientDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Hide any previous activation key stuff
            ClearAllActivationStuff();
            SystemCodeTB.ForeColor = Color.Black;
            
            if (!(ClientDDL.SelectedItem.ToString() == "Select a client."))
            {
                //Show the activation key stuff and reset the Text Box
                ActivationKeyBttn.Enabled = true;
                AmendBttn.Enabled = true;
                ActivationKeyTB.Text = string.Empty;
                ActivServicePackTB.Text = string.Empty;
                

                try
                {
                    using (StreamReader sr = new StreamReader(getClientTextFile()))
                    {
                        //Takest the information from the text file and fills in the web form
                        VersionTB.Text = sr.ReadLine();
                        sr.ReadLine();
                        EmailToTB.Text = sr.ReadLine();
                        LastSPTB.Text = sr.ReadLine();
                        SystemCodeTB.Text = sr.ReadLine();
                        string activationType = sr.ReadLine();
                        if (activationType == "Concurrent")
                        {
                            ActivationTypeBL.SelectedValue = "Concurrent users";
                        }
                        else if (activationType == "Named")
                        {
                            ActivationTypeBL.SelectedValue = "Named users";
                        }
                        LimitTB.Text = sr.ReadLine();
                        ExtendedLimitTB.Text = sr.ReadLine();
                        ExpiryMonthTB.Text = sr.ReadLine();
                        ExpiryYearTB.Text = sr.ReadLine();
                        ActivTrinityVersionTB.Text = VersionTB.Text;
                    }
                }
                catch(Exception)
                {
                    //The client file has been changed or deleted outside the program after the list has been generated.
                    ClearAllTBoxes();
                    Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('Could not get Client\\'s details.');", true); 
                }
            
                checkForMultipleSystems();
                ClearAllLabels(); //reset the page's labels
            }
            else
            {
                //reset the form as no client has been selected
                ActivationKeyBttn.Enabled = false;
                AmendBttn.Enabled = false;
                
                ClearAllTBoxes();
                ClearAllLabels();
            }
            

        }

        protected void ClearAllActivationStuff()
        {
            
            PushsftpBttn.Visible = false;
            ActivationKeyTB.Visible = false;
            sftpCancel.Visible = false;
            sftpContinue.Visible = false;
            sftpIT.Visible = false;
            ActivationLabel.Visible = false;
        }

        protected void ClearAllLabels()
        {
            //Hides all the labels
            LimitLabel.Visible = false;
            ExtendedLimitLabel.Visible = false;
            ExpiryMonthLabel.Visible = false;
            ExpiryYearLabel.Visible = false;
            VersionLabel.Visible = false;
            ServicePackLabel.Visible = false;
            ClientLabel.Visible = false;
            ActivationLabel.Visible = false;
            ActivationTypeLabel.Visible = false;
        }
        
        protected void ClearAllTBoxes()
        {
            //Makes all the text boxes empty
            VersionTB.Text = "";
            LastSPTB.Text = "";
            EmailToTB.Text = "";
            SystemCodeTB.Text = "";
            SystemCodeTB.ForeColor = Color.Black;
            ActivationTypeBL.ClearSelection();
            LimitTB.Text = "";
            ExtendedLimitTB.Text = "";
            ExpiryMonthTB.Text = "";
            ExpiryYearTB.Text = "";
            ActivTrinityVersionTB.Text = "";
            ActivServicePackTB.Text = "";
            ActivationKeyTB.Text = "";
           


        }

        protected void ActivationKeyBttn_Click(object sender, EventArgs e)
        {
            ClearAllLabels();
            string systemCode;
            if (SystemCodeTB.Text == ("System Code is Required.")) SystemCodeTB.Text = "";
            //if the multiple systems drop down list is visible it means there are several system codes.
            //the user has selected the correct system code from the multiple systems drop down list so make that the system code.
            //otherwise just use the system code in the text box
            if (MultipleSystemsDDL.Visible == true)
            {
                systemCode = MultipleSystemsDDL.SelectedValue;
            }
            else
            {
                systemCode = SystemCodeTB.Text.Trim();
            }
            
           ActivationKeyTB.Text = GenerateKey(ActivationTypeBL.SelectedIndex, LimitTB.Text.Trim(), ExtendedLimitTB.Text.Trim(), ExpiryMonthTB.Text.Trim(), ExpiryYearTB.Text.Trim(), ActivTrinityVersionTB.Text.Trim() + "." + ActivServicePackTB.Text.Trim() ,systemCode.Trim());
           //display the text box containing the activation key
            if (!(ActivationKeyTB.Text == ""))
           {
               ActivationKeyTB.Visible = true;
               PushsftpBttn.Visible = true;
           }
           else
           {
               ClearAllActivationStuff();
           }
        }

        protected string GenerateKey(int activationType, string limit, string extendedLimit, string expiryMonth, string expiryYear, string version, string clientCode)
        {
            if (GenerateKeyValidation(activationType,limit, extendedLimit, expiryMonth, expiryYear, version, clientCode))
            {
                //has passed the validation
                string strKey = string.Empty;
                bool evaluation = false;
                //The day is generated by the last day of the month
                DateTime endOfMonth = new DateTime(Convert.ToInt32(expiryYear), Convert.ToInt32(expiryMonth), DateTime.DaysInMonth(Convert.ToInt32(expiryYear), Convert.ToInt32(expiryMonth)));
                //the string part of the activation code
                strKey = (evaluation ? "EVALUATION" : "") + "," + (activationType==0 ? "CONCURRENT" : "TOTAL") + "," + limit + "," + extendedLimit + "," + endOfMonth.ToString("dd-MMM-yyyy") + "," + version + "," + clientCode;
                return strKey + "," + Strings.Mid(StrCheckSum(StrCheckSum(strKey)), 2);
            }
            else
            {
                return String.Empty;
            }
        }

        bool GenerateKeyValidation(int activationType, string limit, string extendedLimit, string expiryMonth, string expiryYear, string version, string clientCode)
        {
            //the validation
            //if there is an error, returns false and displays the relevant label displaying the validation error
            bool valid = true;
            double Num;
            bool isNum;
            bool isRightDigits;
            //Activation Type
            if (activationType.Equals(-1))
            {
                valid = false;
                ActivationTypeLabel.Text = "Select an  Activation type";
                ActivationTypeLabel.Visible = true;
            }
            //Limitca
            isNum = double.TryParse(limit, out Num);
            if (!isNum)
            {
                valid = false;
                LimitLabel.Text = "Limit must be a number.";
                LimitLabel.Visible = true;
            }
            if (limit == "")
            {
                valid = false;
                LimitLabel.Text = "Limit is required.";
                LimitLabel.Visible = true;
            }
            //Extended Limit
            isNum = double.TryParse(extendedLimit, out Num);
            if (!isNum)
            {
                valid = false;
                ExtendedLimitLabel.Text = "Extended Limit must be a number.";
                ExtendedLimitLabel.Visible = true;
            }
            if (extendedLimit == "")
            {
                valid = false;
                ExtendedLimitLabel.Text = "Extended Limit is required.";
                ExtendedLimitLabel.Visible = true;
            }
            //expiry Month
            isNum = double.TryParse(expiryMonth, out Num);
            isRightDigits = (expiryMonth.Length == 2);
            if (!isNum)
            {
                valid = false;
                ExpiryMonthLabel.Text = "Expiry Month must be a number and of the form MM.";
                ExpiryMonthLabel.Visible = true;
            }
            else
            {
                if (Num > 12 | Num < 1)
                {
                    valid = false;
                    ExpiryMonthLabel.Text = "Expiry Month must be between 01 and 12.";
                    ExpiryMonthLabel.Visible = true;
                }
            }

            if (!isRightDigits)
            {
                valid = false;
                ExpiryMonthLabel.Text = "Expiry Month must be a number and of the form MM.";
                ExpiryMonthLabel.Visible = true;
            }
            if (expiryMonth == "")
            {
                valid = false;
                ExpiryMonthLabel.Text = "Expiry Month is required.";
                ExpiryMonthLabel.Visible = true;
            }
             //expiry Year
            isNum = double.TryParse(expiryYear, out Num);
            isRightDigits = (expiryYear.Length == 4);
            if (!isNum)
            {
                valid = false;
                ExpiryYearLabel.Text = "Expiry Year must be a number and of the form YYYY.";
                ExpiryYearLabel.Visible = true;
            }
            if (!isRightDigits)
            {
                valid = false;
                ExpiryYearLabel.Text = "Expiry Year must be a number and of the form YYYY.";
                ExpiryYearLabel.Visible = true;
            }
            if (expiryYear == "")
            {
                valid = false;
                ExpiryYearLabel.Text = "Expiry Year is required.";
                ExpiryYearLabel.Visible = true;
            }

            //version
            isNum = double.TryParse(ActivTrinityVersionTB.Text, out Num);
            if (!isNum)
            {
                valid = false;
                VersionLabel.Text = "Trinity Version is not correct.";
                VersionLabel.Visible = true;
            }
            if (ActivTrinityVersionTB.Text == "")
            {
                valid = false;
                VersionLabel.Text = "Trinity Version is required.";
                VersionLabel.Visible = true;
            }
            //service pack
            isNum = double.TryParse(ActivServicePackTB.Text, out Num);
            if (!isNum)
            {
                valid = false;
                ServicePackLabel.Text = "Service Pack is not correct.";
                ServicePackLabel.Visible = true;
            }
            if (ActivServicePackTB.Text == "")
            {
                valid = false;
                ServicePackLabel.Text = "Service Pack is required.";
                ServicePackLabel.Visible = true;
            }
            //client code
            if (clientCode.Equals(""))
            {
                valid = false;
                SystemCodeTB.ForeColor = Color.Red;
                SystemCodeTB.Text = ("System Code is Required.");
                
            }
            return valid;

        }


        
        protected string StrCheckSum(string vstrKey)
        {
            //this was inherited from the original vb6 activation key generator, hence the use of visual basic functions. Can not be converted any more into c#.net unfortunately.
            int i;
            long lngCode;
            vstrKey = Strings.UCase(vstrKey);
            lngCode = 0;
            for (i = 1; i <= Strings.Len(vstrKey) - 1; i += 2)
            {
                lngCode = (2 * lngCode + Strings.Asc(Strings.Mid(vstrKey,i,1)) + 256 * Strings.Asc(Strings.Mid(vstrKey,i+1,1))) % 32573;
            }
            if (Strings.Len(vstrKey) % 2 == 1)
            {
                lngCode = lngCode + Strings.Asc(Strings.Right(vstrKey,1));
            }
            lngCode = lngCode + 1;
            return "P" + Convert.ToString(lngCode);
                    

        }

        protected void SystemCodeTB_TextChanged(object sender, EventArgs e)
        {
            checkForMultipleSystems();
        }

        protected void checkForMultipleSystems()
        {
            //checks to see whether there are multiple system codes (separated by comma).
            //creates a drop down list of them so the system code can be selected.
            string[] listOfSystems;
            string Systems = SystemCodeTB.Text.Trim();
            if (Systems.Contains(","))
            {
                listOfSystems = Systems.Split(',');
                MultipleSystemsDDL.Items.Clear();
                foreach (string system in listOfSystems)
                {
                    
                    MultipleSystemsDDL.Items.Add(system);
                    MulipleSystemsLabel.Visible = true;
                    MultipleSystemsDDL.Visible = true;
                }

            }
            else
            {
                MulipleSystemsLabel.Visible = false;
                MultipleSystemsDDL.Visible = false;
            }
        }

        protected void AmendBttn_Click(object sender, EventArgs e)
        {
            //writes the data from the form in the client's text file.
            //name of text file matches the name of the client.
            //
            try
            {
            StreamWriter sw = new StreamWriter(getClientTextFile(), false);
            sw.WriteLine(VersionTB.Text);
            sw.WriteLine();
            sw.WriteLine(EmailToTB.Text);
            sw.WriteLine(LastSPTB.Text);
            sw.WriteLine(SystemCodeTB.Text);
            sw.WriteLine(ActivationTypeBL.SelectedIndex == 0 ? "Concurrent" : "Named users");
            sw.WriteLine(LimitTB.Text);
            sw.WriteLine(ExtendedLimitTB.Text);
            sw.WriteLine(ExpiryMonthTB.Text);
            sw.WriteLine(ExpiryYearTB.Text);

            sw.Close();
            sw.Dispose();
            ClearAllLabels();
            ClientLabel.Text = "Client's details amended.";
            ClientLabel.Visible = true;
            }
            catch(Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('Could not amend this Client\\'s details.');", true);
                ClientLabel.Text = ex.Message;
                ClientLabel.Visible = true;
            }
        }

        protected void NewClientBttn_Click(object sender, EventArgs e)
        {
            //Opens the new client form
            Response.Redirect("~/NewClient.aspx");
        }

        protected string CreateActivPath(bool cont)
        {
            //makes the path that the activation code text file is placed in the sftp.
            string systemCode;
            //finds the system code if there is multiple system codes.
            if (MultipleSystemsDDL.Visible == true)
            {
                systemCode = MultipleSystemsDDL.SelectedValue;
            }
            else
            {
                systemCode = SystemCodeTB.Text.Trim();
            }
            //the sftp directory
            string path = "\\\\CAMVS-SFTP8\\Clients\\" +systemCode + "\\Download\\Trinity";
            if (!(Directory.Exists(path)))
            {
                if (cont) 
                {
                    Directory.CreateDirectory(path);
                }
                else
                {
                    ActivationLabel.Text = "The destination folder was not found. It is likely that the client has not got an SFTP account set up and </br> without discussing this with IT the file will almost certainly be inaccessible. Do you want to:";
                    ActivationLabel.Visible = true;
                    sftpCancel.Visible = true;
                    sftpContinue.Visible  = true;
                    sftpIT.Visible = true;
                    return "false";
                }
            }
            return path;
        }

        protected void PushsftpBttn_Click(object sender, EventArgs e)
        {
            PushSFTP(false);
        }
        protected void PushSFTP(bool cont)
        {
            try
            {
                if (CreateActivPath(cont) != "false")
                {
                    string fullPath = CreateActivPath(cont) + "\\ActivationKey_" + ActivTrinityVersionTB.Text + "_" + ActivServicePackTB.Text + ".txt";
                    StreamWriter sw = new StreamWriter(fullPath, false);
                    sw.WriteLine(ActivationKeyTB.Text);
                    sw.Close();
                    sw.Dispose();
                    ActivationLabel.Text = "Activation Key pushed to: " + fullPath;
                    ActivationLabel.Visible = true;
                }

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('Could not push Activation Key to sftp.');", true);
                ActivationLabel.Text = ex.Message;
                ActivationLabel.Visible = true;

            }
        
        }
        protected void sftpCancel_Click(object sender, EventArgs e)
        {
            ClearAllActivationStuff();
            PushsftpBttn.Visible = true;
            ActivationKeyTB.Visible = true;

        }
        protected void sftpContinue_Click(object sender, EventArgs e)
        {
            ClearAllActivationStuff();
            ActivationKeyTB.Visible = true;
            PushSFTP(true);
        }
        protected void sftpIT_Click(object sender, EventArgs e)
        {
            string systemCode;
            //finds the system code if there is multiple system codes.
            if (MultipleSystemsDDL.Visible == true)
            {
                systemCode = MultipleSystemsDDL.SelectedValue;
            }
            else
            {
                systemCode = SystemCodeTB.Text.Trim();
            }
            string email = "it-cambridge@bradyplc.com";
            ClientScript.RegisterStartupScript(this.GetType(), "mailto",
            "<script type = 'text/javascript'>parent.location='mailto:" + email + 
            "?subject=New%20SFTP%20account%20needed%20for%20" + systemCode  + "&body=Hello%20IT%2C%0A%0A" +
            "I%20need%20this%20account%20setting%20up%20for%20" + systemCode  + ".%20Could%20you%20do%20the%20following%3A%0A%0A" +
            "1)%09Create%20the%20SFTP%20account%20and%20assign%20a%20password.%0A%0A" +
            "2)%09Create%20the%20appropriate%20folders%20(%5C%5CCAMVS-SFTP8%5CClients%5C" + systemCode + "%5CDownload%5CTrinity%5C%20and%20%5CUpload%5CTrinity%5C%20with%20the%20appropriate%20RO%20%2F%20RW%20rights).%0A" +
            "3)%09Create%20a%20file%20called%20%5C%5CCAMVS-SFTP8%5CClients%5C" + systemCode + "%5CDownload%5CTrinity%5CActivationKey_" + ActivTrinityVersionTB.Text + "_" + ActivServicePackTB.Text+ " .txt%20with%20this%20content%3A%0A" +
            "%09%09%09" + ActivationKeyTB.Text + "%0A%0A" +
            "4)%09E-mail%20Support%20with%20the%20username%20%2F%20password%20details%20so%20that%20it%20can%20be%20passed%20on%20to%20the%20client.%0A%0A" +
            "5)%09Have%20A%20Nice%20Day.'</script>");
          
            sftpCancel.Visible = false;
            sftpContinue.Visible = false;
            sftpIT.Visible = false;
            ActivationLabel.Visible = false;

         

        }
    }
}