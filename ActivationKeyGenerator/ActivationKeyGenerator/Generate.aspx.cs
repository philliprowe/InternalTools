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
            //Creates a warning box when the amend client details and Push to sftp button are pressed
            AmendBttn.Attributes.Add("onclick", "return confirm('Warning: This will amend this Client\\'s details.\\r\\nAmend this Client?');");
            PushsftpBttn.Attributes.Add("onclick", "return confirm('Warning: This will push the Client\\'s Activation Key to their SFTP. \\r\\n Push Activation Key to SFTP?');");

            HideAllComments();


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
                    ClientDDL.Items.Add("Select a client");
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
                    ClientComment.Text = ex.Message;
                    ClientComment.Visible = true;
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
            ClearAllActivationStuff(); // Hide any previous activation key entries
            HideAllComments(); // Reset the page's Comments      
            EntityDDL.Items.Clear();
            EntityRow.Visible = false;
            SystemCodeTB.ForeColor = Color.Black;
            int entityNumber;
            
            if (!(ClientDDL.SelectedItem.ToString() == "Select a client"))
            {
                // Show the activation key entries and reset the Text Box
                ActivationKeyBttn.Enabled = true;
                AmendBttn.Enabled = true;
                ActivationKeyTB.Text = string.Empty;
                ActivServicePackTB.Text = string.Empty;
                
                try
                {
                    using (StreamReader sr = new StreamReader(getClientTextFile()))
                    {
                        // Check if the client has multiply entities
                        // If only have one entity, fills in the web form
                        entityNumber = Convert.ToInt32(sr.ReadLine().Substring(14));
                        if (entityNumber != 1)
                        {   
                            ClearAllTBoxes();
                            MultipleEntities(entityNumber);
                            return;
                        }
                            VersionTB.Text = sr.ReadLine();
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
                            InteractiveTB.Text = sr.ReadLine();
                            ServiceTB.Text = sr.ReadLine();
                            ExpiryMonthTB.Text = sr.ReadLine();
                            ExpiryYearTB.Text = sr.ReadLine();
                            ActivTrinityVersionTB.Text = VersionTB.Text;
                        }
                    checkForMultipleSystems();
                                  
                }
                catch(Exception ex)
                {
                    // Usually when the client file has been moved or deleted outside the program 
                    ClearAllTBoxes();
                    Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('Could not get Client\\'s details.');", true);
                    ClientComment.Text = ex.Message;
                    ClientComment.Visible = true;
                }
            }
            else
            {
                // Reset the form as no client has been selected
                ActivationKeyBttn.Enabled = false;
                AmendBttn.Enabled = false;
                
                ClearAllTBoxes();
                HideAllComments();
            }
        }
        protected void EntityDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Hide any previous activation key entries
            ClearAllActivationStuff();
            SystemCodeTB.ForeColor = Color.Black;
            EntityDDL.Focus();

            if (!(EntityDDL.SelectedItem.ToString() == "Select an entity."))
            {
                // Show the activation key fields and reset the Text Boxes
                ActivationKeyBttn.Enabled = true;
                AmendBttn.Enabled = true;
                ActivationKeyTB.Text = string.Empty;
                ActivServicePackTB.Text = string.Empty;

                try
                {
                    using (StreamReader sr = new StreamReader(getClientTextFile()))
                    {
                        // Search for the entity name that matches the selected name
                        // Fills in the web form when the entity name is found

                        while (!(sr.ReadLine() == EntityDDL.SelectedValue))
                        { }
                        VersionTB.Text = sr.ReadLine();
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
                        InteractiveTB.Text = sr.ReadLine();
                        ServiceTB.Text = sr.ReadLine();
                        ExpiryMonthTB.Text = sr.ReadLine();
                        ExpiryYearTB.Text = sr.ReadLine();
                        ActivTrinityVersionTB.Text = VersionTB.Text;
                    }
                    
                }
                catch(Exception)
                {
                    // Usually when the client file has been changed, moved or deleted 
                    ClearAllTBoxes();
                    Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('Could not get Entity\\'s details.');", true); 
                }
                checkForMultipleSystems();
                HideAllComments(); // Reset the page's Comments
            }
            else
            {
                // Reset the form as no entity has been selected
                ActivationKeyBttn.Enabled = false;
                AmendBttn.Enabled = false;
                
                ClearAllTBoxes();
                HideAllComments();
            }
        }
        protected void ClearAllActivationStuff()
        {
            
            PushsftpBttn.Visible = false;
            ActivationKeyTB.Visible = false;
            sftpCancel.Visible = false;
            sftpContinue.Visible = false;
            sftpIT.Visible = false;
            ActivationComment.Visible = false;
        }
        protected void HideAllComments()
        {
            //Hides all the Comments
            InteractiveComment.Visible = false;
            ServiceComment.Visible = false;
            ExpiryMonthComment.Visible = false;
            ExpiryYearComment.Visible = false;
            VersionComment.Visible = false;
            ServicePackComment.Visible = false;
            ClientComment.Visible = false;
            ActivationComment.Visible = false;
            ActivationTypeComment.Visible = false;
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
            InteractiveTB.Text = "";
            ServiceTB.Text = "";
            ExpiryMonthTB.Text = "";
            ExpiryYearTB.Text = "";
            ActivTrinityVersionTB.Text = "";
            ActivServicePackTB.Text = "";
            ActivationKeyTB.Text = "";
        }
        protected void ActivationKeyBttn_Click(object sender, EventArgs e)
        {
            HideAllComments();
            string systemCode;
            if (SystemCodeTB.Text == ("System Code is Required.")) SystemCodeTB.Text = "";
            // If the multiple systems drop down list is visible it means there are several system codes.
            // The user has selected the correct system code from the multiple systems drop down list so make that the system code.
            // Otherwise just use the system code in the text box
            if (MultipleSystemsDDL.Visible == true)
            {
                systemCode = MultipleSystemsDDL.SelectedValue;
            }
            else
            {
                systemCode = SystemCodeTB.Text.Trim();
            }

            ActivationKeyTB.Text = GenerateKey(ActivationTypeBL.SelectedIndex, InteractiveTB.Text.Trim(), ServiceTB.Text.Trim(), ExpiryMonthTB.Text.Trim(), ExpiryYearTB.Text.Trim(), ActivTrinityVersionTB.Text.Trim() + "." + ActivServicePackTB.Text.Trim(), systemCode.Trim());
           // Display the text box containing the activation key
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
        protected string GenerateKey(int activationType, string interactiveLicenses, string serviceLicenses, string expiryMonth, string expiryYear, string version, string clientCode)
        {
            if (GenerateKeyValidation(activationType, interactiveLicenses, serviceLicenses, expiryMonth, expiryYear, version, clientCode))
            {
                //has passed the validation
                string strKey = string.Empty;
                bool evaluation = false;
                //The day is generated by the last day of the month
                DateTime endOfMonth = new DateTime(Convert.ToInt32(expiryYear), Convert.ToInt32(expiryMonth), DateTime.DaysInMonth(Convert.ToInt32(expiryYear), Convert.ToInt32(expiryMonth)));
                //the string part of the activation code
                strKey = (evaluation ? "EVALUATION" : "") + "," + (activationType == 0 ? "CONCURRENT" : "TOTAL") + "," + interactiveLicenses + "," + serviceLicenses + "," + endOfMonth.ToString("dd-MMM-yyyy") + "," + version + "," + clientCode;
                return strKey + "," + Strings.Mid(StrCheckSum(StrCheckSum(strKey)), 2);
            }
            else
            {
                return String.Empty;
            }
        }
        bool GenerateKeyValidation(int activationType, string interactiveLicenses, string serviceLicenses, string expiryMonth, string expiryYear, string version, string clientCode)
        {
            // Validate entries
            // If there is an error, returns false and displays the relevant Comment displaying the validation error
            bool valid = true;
            double Num;
            bool isNum;
            bool isRightDigits;

            // Activation Type
            if (activationType.Equals(-1))
            {
                valid = false;
                ActivationTypeComment.Text = "Select an  Activation type";
                ActivationTypeComment.Visible = true;
            }

            // InteractiveLicenses
            isNum = double.TryParse(interactiveLicenses, out Num);
            if (!isNum)
            {
                valid = false;
                InteractiveComment.Text = "Number of Interactive Licenses must be a number.";
                InteractiveComment.Visible = true;
            }
            if (interactiveLicenses == "")
            {
                valid = false;
                InteractiveComment.Text = "Number of Interactive Licenses is required.";
                InteractiveComment.Visible = true;
            }

            // ServiceLicenses
            isNum = double.TryParse(serviceLicenses, out Num);
            if (!isNum)
            {
                valid = false;
                ServiceComment.Text = "Number of Service Licenses must be a number.";
                ServiceComment.Visible = true;
            }
            if (serviceLicenses == "")
            {
                valid = false;
                ServiceComment.Text = "Number of Service Licenses is required.";
                ServiceComment.Visible = true;
            }

            // Expiry Month
            isNum = double.TryParse(expiryMonth, out Num);
            isRightDigits = (expiryMonth.Length == 2);
            if (!isNum || !isRightDigits)
            {
                valid = false;
                ExpiryMonthComment.Text = "Expiry Month must be a number and of the form MM.";
                ExpiryMonthComment.Visible = true;
            }
            else
            {
                if (Num > 12 | Num < 1)
                {
                    valid = false;
                    ExpiryMonthComment.Text = "Expiry Month must be between 01 and 12.";
                    ExpiryMonthComment.Visible = true;
                }
            }

            if (expiryMonth == "")
            {
                valid = false;
                ExpiryMonthComment.Text = "Expiry Month is required.";
                ExpiryMonthComment.Visible = true;
            }
            // Expiry Year
            isNum = double.TryParse(expiryYear, out Num);
            isRightDigits = (expiryYear.Length == 4);
            if (!isNum || !isRightDigits)
            {
                valid = false;
                ExpiryYearComment.Text = "Expiry Year must be a number and of the form YYYY.";
                ExpiryYearComment.Visible = true;
            }
            if (expiryYear == "")
            {
                valid = false;
                ExpiryYearComment.Text = "Expiry Year is required.";
                ExpiryYearComment.Visible = true;
            }

            // Version
            isNum = double.TryParse(ActivTrinityVersionTB.Text, out Num);
            if (!isNum || Num < 2010)
            {
                valid = false;
                VersionComment.Text = "Trinity Version is not correct.";
                VersionComment.Visible = true;
            }
            if (ActivTrinityVersionTB.Text == "")
            {
                valid = false;
                VersionComment.Text = "Trinity Version is required.";
                VersionComment.Visible = true;
            }

            // Service pack
            isNum = double.TryParse(ActivServicePackTB.Text, out Num);
            if (!isNum || Num < 0)
            {
                valid = false;
                ServicePackComment.Text = "Service Pack is not correct.";
                ServicePackComment.Visible = true;
            }
            if (ActivServicePackTB.Text == "")
            {
                valid = false;
                ServicePackComment.Text = "Service Pack is required.";
                ServicePackComment.Visible = true;
            }

            // Client code
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
            // This was inherited from the original vb6 activation key generator, hence the use of visual basic functions. Can not be converted any more into c#.net unfortunately.
            // Generates unique client key code
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
            // Everytime SystemCode is changed, check for multiple systems
            checkForMultipleSystems();
        }
        protected void MultipleEntities(int entityNumber)
        {
            // For multiple entities of the same client
            if (entityNumber > 1)
            {
                EntityDDL.Items.Add("Select an entity.");
                using(StreamReader sr = new StreamReader(getClientTextFile()))
                {
                    int rowNumber = 11 * entityNumber + 1;
                    for (int i = 0 ; i < rowNumber ; i++)
                    {
                        if(i % 11 == 2)
                        {
                            EntityDDL.Items.Add(sr.ReadLine());
                        }
                        else
                        {
                            sr.ReadLine();
                        }
                    }
                    EntityRow.Visible = true;
                }
            }
            else
            {
                EntityRow.Visible = true;
                EntityDDL.Items.Add("Entity information is not valid.");
            }
        }  
        protected void checkForMultipleSystems()
        {
            // Checks to see whether there are multiple system codes (separated by comma).
            // Creates a drop down list of them so the system code can be selected.
            string[] listOfSystems;
            string Systems = SystemCodeTB.Text.Trim();
            if (Systems.Contains(","))
            {
                listOfSystems = Systems.Split(',');
                MultipleSystemsDDL.Items.Clear();
                foreach (string system in listOfSystems)
                {
                    
                    MultipleSystemsDDL.Items.Add(system);
                    MulipleSystemsComment.Visible = true;
                    MultipleSystemsDDL.Visible = true;
                }

            }
            else
            {
                MulipleSystemsComment.Visible = false;
                MultipleSystemsDDL.Visible = false;
            }
        }
        protected void AmendBttn_Click(object sender, EventArgs e)
        {
            // Writes the data from the form in the client's text file.
            // Name of text file matches the name of the client.
            // Get the information of Entity from the dropdownlist.
            int numberOfEntity = EntityDDL.Items.Count - 1;
            string path = getClientTextFile();
                try
                {
                    if(!(numberOfEntity > 1))
                    {
                        // For single entity, simply rewrite all client information
                        using(StreamWriter sw = new StreamWriter(path, false))
                        {
                            sw.WriteLine("Entity Number:1");
                            sw.WriteLine(VersionTB.Text);
                            sw.WriteLine(EmailToTB.Text);
                            sw.WriteLine(LastSPTB.Text);
                            sw.WriteLine(SystemCodeTB.Text);
                            sw.WriteLine(ActivationTypeBL.SelectedIndex == 0 ? "Concurrent" : "Named");
                            sw.WriteLine(InteractiveTB.Text);
                            sw.WriteLine(ServiceTB.Text);
                            sw.WriteLine(ExpiryMonthTB.Text);
                            sw.WriteLine(ExpiryYearTB.Text);
                        }
                        HideAllComments();
                        ClientComment.Text = "Client's details amended.";
                        ClientComment.Visible = true;
                    }
                    else
                    {
                        // For multiple entities, copy client information to a string, edit this sting then
                        // recopy edited string into client file
                        int i;
                        string[] lines = File.ReadAllLines(path);
                        for (i = 0; ; i++)
                        { 
                            if (lines[i] == "*" && lines[++i] == EntityDDL.SelectedValue)
                            {
                                lines[++i] = VersionTB.Text;
                                lines[++i] = EmailToTB.Text;
                                lines[++i] = LastSPTB.Text;
                                lines[++i] = SystemCodeTB.Text;
                                lines[++i] = ActivationTypeBL.SelectedIndex == 0 ? "Concurrent" : "Named";
                                lines[++i] = InteractiveTB.Text;
                                lines[++i] = ServiceTB.Text;
                                lines[++i] = ExpiryMonthTB.Text;
                                lines[++i] = ExpiryYearTB.Text;
                                break;
                            }
                        }
                        

                        using(StreamWriter sw = new StreamWriter(path, false))
                        {
                            int rows = lines.Length;
                            for (int j = 0; j < rows; j++)
                            {
                                sw.WriteLine(lines[j]);
                            }
                        }
                        HideAllComments();
                        ClientComment.Text = "Entity's details amended.";
                        ClientComment.Visible = true;
                    }
                }
                catch (Exception ex)
                { 
                    Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('Could not amend this Client\\'s details.');", true);
                    ClientComment.Text = ex.Message;
                    ClientComment.Visible = true;
                }
        }        
        protected void NewClientBttn_Click(object sender, EventArgs e)
        {
            // Opens the new client form
            Response.Redirect("~/NewClient.aspx");
        }
        protected string CreateActivPath(bool cont)
        {
            // Makes the path that the activation code text file is placed in the sftp.
            string systemCode;
            // Finds the system code if there is multiple system codes.
            if (MultipleSystemsDDL.Visible == true)
            {
                systemCode = MultipleSystemsDDL.SelectedValue;
            }
            else
            {
                systemCode = SystemCodeTB.Text.Trim();
            }
            // The sftp directory
            string path = "\\\\CAMVS-SFTP8\\Clients\\" +systemCode + "\\Download\\Trinity";
            if (!(Directory.Exists(path)))
            {
                if (cont) 
                {
                    Directory.CreateDirectory(path);
                }
                else
                {
                    ActivationComment.Text = "The destination folder was not found. It is likely that the client has not got an SFTP account set up and </br> without discussing this with IT the file will almost certainly be inaccessible. Do you want to:";
                    ActivationComment.Visible = true;
                    sftpRow.Visible = true;
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
                    using(StreamWriter sw = new StreamWriter(fullPath, false))
                    {
                        sw.WriteLine(ActivationKeyTB.Text);
                    }
                    ActivationComment.Text = "Activation Key pushed to: " + fullPath;
                    ActivationComment.Visible = true;
                }

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('Could not push Activation Key to sftp.');", true);
                ActivationComment.Text = ex.Message;
                ActivationComment.Visible = true;

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
            // Finds the system code if there is multiple system codes.
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
          
            sftpRow.Visible = false;
            ActivationComment.Visible = false;

         

        }
    }
}