using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace ActivationKeyGenerator
{
    public partial class NewClient : System.Web.UI.Page
    {

        public string path = "C:\\ActivationKeyGenerator\\_Clients"; // The folder that stores the client files
        protected void Page_Load(object sender, EventArgs e)
        {
            // Creates a dialogue shown when the save client details button is pressed. If cancel is pressed it doesn't continue execution.
            SaveSingleBttn.Attributes.Add("onclick", "return confirm('Warning: This will create a new Client.\\r\\nSave Client?');");
            // Creates a dialogue shown when the save client details button is pressed. If cancel is pressed it doesn't continue execution.
            SaveMultipleBttn.Attributes.Add("onclick", "return confirm('Warning: Have you saved your last entity?\\r\\nYou will lose the information if not.\\r\\nContinue?');"); 
            

            // Series of conditions which govern the visibility of certain buttons and textboxes
            if (NewClientTB.Text.Trim() == "")
            {
                NewClientTB.Focus();
            }
            else
            {
                GoBack.Attributes.Add("onclick", "return confirm('Warning: All unsaved information will be lost\\r\\nContinue?');"); //Creates a dialogue shown when the Go back button is pressed. If cancel is pressed it doesn't continue execution.
                SaveSingleBttn.Enabled = true;
                ResetBttn.Enabled = true;
                if (EntityBL.SelectedIndex == (-1))
                {
                    EntityBL.Focus();                 
                }
                else if (EntityBL.SelectedIndex == 1 && NewEntityNameTB.Text.Trim() == "")
                {
                    NewEntityNameTB.Focus();
                }
                else if (EntityBL.SelectedIndex == 0)
                {
                    NewVersionTB.Focus();
                }

                if (NewEntityNameTB.Text.Trim() != "" && EntityBL.SelectedIndex == 1)
                {
                    AddNewBttn.Enabled = true;
                    SaveMultipleBttn.Enabled = true;
                    NewVersionTB.Focus();
                }
            }
        }
        protected void GoBack_Click(object sender, EventArgs e)
        {
            // Opens the generate form
            Response.Redirect("~/Generate.aspx");
        }
        protected void HideAllNewLabels()
        {
            // Hides all the labels
            NewClientLabel.Visible = false;
            EntitiesLabel.Visible = false;
            NewEntityNameLabel.Visible = false;
            NewVersionLabel.Visible = false;
            NewLastSPLabel.Visible = false;
            NewSystemCodeLabel.Visible = false;
            NewActivationTypeLabel.Visible = false;
            NewInteractiveLicensesLabel.Visible = false;
            NewServiceLicensesLabel.Visible = false;
            NewExpiryMonthLabel.Visible = false;
            NewExpiryYearLabel.Visible = false;
        }
        protected void SaveSingleBttn_Click(object sender, EventArgs e)
        {
            HideAllNewLabels();

            // Stores all the information entered in the form
            string newClient, newEntityName, newVersion, newLastSP, newEmailTo, newSystemCode, newInteractiveLicenses, newServiceLicenses, newExpiryMonth, newExpiryYear;
            int newActivationType;

            ObtainEnteredInfo(out newClient, out newEntityName, out newVersion, out newLastSP, out newEmailTo, out newSystemCode, out newActivationType, out newInteractiveLicenses, out newServiceLicenses, out newExpiryMonth, out newExpiryYear);

            if (SaveNewClientValidation(newClient, newEntityName, newActivationType, newInteractiveLicenses, newServiceLicenses, newExpiryMonth, newExpiryYear, newSystemCode, newVersion, newLastSP))
            {
                // If it passes validation                
                SaveNewClient(newClient, "singleEntity", newVersion, newLastSP, newEmailTo, newSystemCode, (newActivationType == 0 ? "Concurrent" : "Named users"), newInteractiveLicenses, newServiceLicenses, newExpiryMonth, newExpiryYear);
            }
        }
        protected void AddNewBttn_Click(object sender, EventArgs e)
        {
            HideAllNewLabels(); // Hides all warnings at this point

            // Stores all the information entered in the form
            string newClient, newEntityName, newVersion, newLastSP, newEmailTo, newSystemCode, newInteractiveLicenses, newServiceLicenses, newExpiryMonth, newExpiryYear;
            int newActivationType;

            ObtainEnteredInfo(out newClient, out newEntityName, out newVersion, out newLastSP, out newEmailTo, out newSystemCode, out newActivationType, out newInteractiveLicenses, out newServiceLicenses, out newExpiryMonth, out newExpiryYear);

            if (SaveNewClientValidation(newClient, newEntityName, newActivationType, newInteractiveLicenses, newServiceLicenses, newExpiryMonth, newExpiryYear, newSystemCode, newVersion, newLastSP))
            {
                // If it passes validation                
                SaveNewClient(newClient, newEntityName, newVersion, newLastSP, newEmailTo, newSystemCode, (newActivationType == 0 ? "Concurrent" : "Named users"), newInteractiveLicenses, newServiceLicenses, newExpiryMonth, newExpiryYear);
            }

            NewEntityNameTB.Focus(); // Focus page on new entity name field
        }
        protected void SaveMultipleBttn_Click(object sender, EventArgs e)
        {
            try
            {                
                HideAllNewLabels(); // First hide all labels

                string newClientName = NewClientTB.Text.Trim();

                // Determine number of entities for this Client            
                int rowNumber = File.ReadAllLines(path + "\\" + newClientName + ".txt").Length;
                int entityNumber = rowNumber / 11;

                // Copy file contents into a string
                string[] thisFile = File.ReadAllLines(path + "\\" + newClientName + ".txt");

                // Write out entity number and append file contents
                StreamWriter sw = new StreamWriter(path + "\\" + newClientName + ".txt");
                sw.WriteLine("Entity Number:" + entityNumber);
                for (int i = 0; i < thisFile.Length; i++)
                {
                    sw.WriteLine(thisFile[i]);
                }

                sw.Close();
                sw.Dispose();

                NewClientLabel.Text = "New Client successfully saved";
                NewClientLabel.Visible = true;
            }
            catch(Exception ex)
            {
                // Normally when the Client list has been moved
                Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('Could not get Client List.');", true);
                NewClientLabel.Text = ex.Message;
                NewClientLabel.Visible = true;
            }            
        }
        protected bool SaveNewClientValidation(string newClient, string newEntityName, int newActivationType, string newInteractiveLicenses, string newServiceLicenses, string newExpiryMonth, string newExpiryYear, string newSystemCode, string newVersion, string newLastSP)
        {
            // If there is an error, returns false and displays the relevant label displaying the validation error
            bool valid = true;
            double Num;
            bool isNum;
            bool isRightDigits;

            // New Client name
            if (newClient == "")
            {
                valid = false;
                NewClientLabel.Text = "Client's Name is required";
                NewClientLabel.Visible = true;
            }
            else if (!ValidateClient(newClient, newEntityName))
            {
                valid = false;
            }

            // New Entity Name
            if (EntityBL.SelectedValue == "Multiple")
            {
                if (newEntityName == "")
                {
                    valid = false;
                    NewEntityNameLabel.Text = "Entity Name is required";
                    NewEntityNameLabel.Visible = true;
                }
                else if (!ValidateClient(newClient, newEntityName))
                {
                    valid = false;
                }
            }

            // Activation Type
            if (newActivationType.Equals(-1))
            {
                valid = false;
                NewActivationTypeLabel.Text = "Select an Activation type";
                NewActivationTypeLabel.Visible = true;
            }

            // InteractiveLicenses
            if (!(newInteractiveLicenses == ""))
            {
                isNum = double.TryParse(newInteractiveLicenses, out Num);
                if (!isNum)
                {
                    valid = false;
                    NewInteractiveLicensesLabel.Text = "Interactive Licenses must be a number";
                    NewInteractiveLicensesLabel.Visible = true;
                }
            }
            else
            {
                valid = false;
                NewInteractiveLicensesLabel.Text = "Interactive Licenses is required";
                NewInteractiveLicensesLabel.Visible = true;

            }

            // ServiceLicenses
            if (!(newServiceLicenses == ""))
            {
                isNum = double.TryParse(newServiceLicenses, out Num);
                if (!isNum)
                {
                    valid = false;
                    NewServiceLicensesLabel.Text = "Service Licenses must be a number";
                    NewServiceLicensesLabel.Visible = true;
                }
            }
            else
            {
                valid = false;
                NewServiceLicensesLabel.Text = "Service Licenses is required";
                NewServiceLicensesLabel.Visible = true;
            }

            // Expiry Month
            if (!(newExpiryMonth == ""))
            {
                isNum = double.TryParse(newExpiryMonth, out Num);
                isRightDigits = (newExpiryMonth.Length == 2);
                if (!isNum)
                {
                    valid = false;
                    NewExpiryMonthLabel.Text = "Expiry Month must be a number and of the form MM";
                    NewExpiryMonthLabel.Visible = true;
                }
                else if (Num > 12 | Num < 1)
                {
                    valid = false;
                    NewExpiryMonthLabel.Text = "Expiry Month must be between 01 and 12";
                    NewExpiryMonthLabel.Visible = true;
                }

                if (!isRightDigits)
                {
                    valid = false;
                    NewExpiryMonthLabel.Text = "Expiry Month must be a number and of the form MM";
                    NewExpiryMonthLabel.Visible = true;
                }
            }
            else
            {
                valid = false;
                NewExpiryMonthLabel.Text = "Expiry Month is required";
                NewExpiryMonthLabel.Visible = true;
            }

            // Expiry Year
            if (!(newExpiryYear == ""))
            {
                isNum = double.TryParse(newExpiryYear, out Num);
                isRightDigits = (newExpiryYear.Length == 4);
                if (!isNum)
                {
                    valid = false;
                    NewExpiryYearLabel.Text = "Expiry Year must be a number and of the form YYYY";
                    NewExpiryYearLabel.Visible = true;
                }
                if (!isRightDigits)
                {
                    valid = false;
                    NewExpiryYearLabel.Text = "Expiry Year must be a number and of the form YYYY";
                    NewExpiryYearLabel.Visible = true;
                }
            }
            else
            {
                valid = false;
                NewExpiryYearLabel.Text = "Expiry Year is required";
                NewExpiryYearLabel.Visible = true;
            }

            // Client code
            if (newSystemCode.Equals(""))
            {
                valid = false;
                NewSystemCodeLabel.Text = ("System Code is Required");
                NewSystemCodeLabel.Visible = true;
            }

            // Version
            isNum = double.TryParse(newVersion, out Num);
            if (!isNum || Num < 2010)
            {
                valid = false;
                NewVersionLabel.Text = "Trinity Version is not correct";
                NewVersionLabel.Visible = true;
            }
            if (newVersion == "")
            {
                valid = false;
                NewVersionLabel.Text = "Trinity Version is required";
                NewVersionLabel.Visible = true;
            }

            // Service pack
            isNum = double.TryParse(newLastSP, out Num);
            if (!isNum || Num < 0)
            {
                valid = false;
                NewLastSPLabel.Text = "Last Service Pack is not correct";
                NewLastSPLabel.Visible = true;
            }
            if (newLastSP == "")
            {
                valid = false;
                NewLastSPLabel.Text = "Last Service Pack is required";
                NewLastSPLabel.Visible = true;
            }

            return valid;
        }
        protected void SaveNewClient(string newClient, string newEntityName, string newVersion, string newLastSP, string newEmailTo, string newSystemCode, string newActivationType, string newInteractiveLicenses, string newServiceLicenses, string newExpiryMonth, string newExpiryYear)
        {
            // Once validation is passed, this writes entered information to the Client's text file.  
            // First check if Client is single or multiple entitied

            // If single
            if (EntityBL.SelectedValue == "Single")
            {
                try
                {
                    // Enter client details with "Entity Number:1" on first line
                    StreamWriter sr = new StreamWriter(path + "\\" + newClient + ".txt");
                    sr.WriteLine("Entity Number:1");
                    sr.WriteLine(newVersion);
                    sr.WriteLine(newEmailTo);
                    sr.WriteLine(newLastSP);
                    sr.WriteLine(newSystemCode);
                    sr.WriteLine(newActivationType);
                    sr.WriteLine(newInteractiveLicenses);
                    sr.WriteLine(newServiceLicenses);
                    sr.WriteLine(newExpiryMonth);
                    sr.WriteLine(newExpiryYear);

                    sr.Close();
                    sr.Dispose();

                    NewClientLabel.Text = "New Client sucessfully saved: " + newClient + ".";
                    NewClientLabel.Visible = true;
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('Could not save new Client\\'s details.');", true);
                    NewClientLabel.Text = ex.Message;
                    NewClientLabel.Visible = true;
                }
            }

            // If multiple
            else if (EntityBL.SelectedValue == "Multiple")
            {
                try
                {    
                    // Enter client details with '*' on first line to differentiate from single entity clients
                    using (StreamWriter sr = File.AppendText(path + "\\" + newClient + ".txt"))
                    {
                        sr.WriteLine("*");
                        sr.WriteLine(newEntityName);
                        sr.WriteLine(newVersion);
                        sr.WriteLine(newEmailTo);
                        sr.WriteLine(newLastSP);
                        sr.WriteLine(newSystemCode);
                        sr.WriteLine(newActivationType);
                        sr.WriteLine(newInteractiveLicenses);
                        sr.WriteLine(newServiceLicenses);
                        sr.WriteLine(newExpiryMonth);
                        sr.WriteLine(newExpiryYear);

                        sr.Close();
                        sr.Dispose();
                    }

                    NewEntityNameLabel.Text = "New Entity sucessfully saved: " + newEntityName + ".";
                    NewEntityNameLabel.Visible = true;
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('Could not save new Client\\'s details.');", true);
                    NewEntityNameLabel.Text = ex.Message;
                    NewEntityNameLabel.Visible = true;
                }
            }
        }
        protected void EntityBL_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Show or hide multiple entity fields on change of selection, retains info in TB incase change is made erroneously
            if (EntityBL.SelectedValue == "Multiple")
            {

                NewEntityID.Visible = true;
                MEBLRow.Visible = true;
                SaveSingleBttn.Visible = false;
            }
            else
            {

                NewEntityID.Visible = false;
                MEBLRow.Visible = false;
                SaveSingleBttn.Visible = true;
            }
        }
        protected void ClearAllFields()
        {
            // Deletes va;ues stored in textboxes
            NewEntityNameTB.Text = "";
            NewEntityNameLabel.Text = "";
            NewVersionTB.Text = "";
            NewLastSPTB.Text = "";
            NewEmailToTB.Text = "";
            NewSystemCodeTB.Text = "";
            NewActivationTypeBL.ClearSelection();
            NewInteractiveLicensesTB.Text = "";
            NewServiceLicensesTB.Text = "";
            NewExpiryMonthTB.Text = "";
            NewExpiryYearTB.Text = "";
        }
        protected void ResetBttn_Click(object sender, EventArgs e)
        {
            HideAllNewLabels();
            ClearAllFields();
        }
        protected bool ValidateClient(string newClientName, string newEntityName )
        {
            // Validate Client name
            bool valid = true;
            if (EntityBL.SelectedValue == "Single")
            {
                try
                {
                    // Find the names of the clients from the directory
                    string[] clientsIncPath = Directory.GetFiles(path, "*.txt");

                    foreach (string ClientName in clientsIncPath)
                    {
                        string Client = Path.GetFileNameWithoutExtension(ClientName);
                        if (!Client.StartsWith("_") & !Client.StartsWith("$") & !Client.StartsWith("%"))
                        {
                            if (newClientName.ToLower() == Client.ToLower())
                            {
                                NewClientLabel.Text = "This client name already exists";
                                NewClientLabel.Visible = true;
                                valid = false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Normally when the Client list has been moved
                    Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('Could not get Client List.');", true);
                    NewClientLabel.Text = ex.Message;
                    NewClientLabel.Visible = true;
                }
            }

            // Validate Multiple Entities
            if (EntityBL.SelectedValue == "Multiple")
            {
                try
                {
                    if (!(File.Exists(path + "\\" + newClientName + ".txt")))
                    {
                        // Any Entity name is allowed if Client name does not yet exist
                    }
                    else
                    {
                        StreamReader sr = new StreamReader(path + "\\" + newClientName + ".txt");

                        // Check if Client name already exists for multiple entities
                        if (!(sr.ReadLine() == "*"))
                        {
                            NewClientLabel.Text = "This client name already exists";
                            NewClientLabel.Visible = true;
                            valid = false;
                        }
                        else
                        {
                            int rowNumber = File.ReadAllLines(path + "\\" + newClientName + ".txt").Length;

                            // Same file reading algorithm as used in Generate.aspx.cs
                            for (int i = 0; i < rowNumber; i++)
                            {                               
                                string thisLine = sr.ReadLine();

                                if (thisLine != null && newEntityName != null && i % 11 == 0 ) // Remainder is 0 due to already used Readline() in preceeding if statement
                                {                                   
                                    if (newEntityName.ToLower() == thisLine.ToLower())
                                    {
                                        NewEntityNameLabel.Text = "This Entity name already exists";
                                        NewEntityNameLabel.Visible = true;
                                        valid = false;
                                        break;
                                    }                                    
                                }
                            }
                        }

                        sr.Close();
                        sr.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('Could not validate client and entity name');", true);
                    NewClientLabel.Text = ex.Message;
                    NewClientLabel.Visible = true;
                }
            }
            
            return valid;
        }
        protected void ObtainEnteredInfo(out string newClient, out string newEntityNameInfo, out string newVersionInfo, out string newLastSPInfo, out string newEmailToInfo, out string newSystemCodeInfo, out int newActivationTypeInfo, out string newInteractiveLicensesInfo, out string newServiceLicensesInfo, out string newExpiryMonthInfo, out string newExpiryYearInfo)
        {

            // Function to store all the information stored in the form
            newClient = NewClientTB.Text.Trim();
            newEntityNameInfo = NewEntityNameTB.Text.Trim();
            newVersionInfo = NewVersionTB.Text.Trim();
            newLastSPInfo = NewLastSPTB.Text.Trim();
            newEmailToInfo = NewEmailToTB.Text.Trim();
            newSystemCodeInfo = NewSystemCodeTB.Text.Trim();
            newActivationTypeInfo = NewActivationTypeBL.SelectedIndex;
            newInteractiveLicensesInfo = NewInteractiveLicensesTB.Text.Trim();
            newServiceLicensesInfo = NewServiceLicensesTB.Text.Trim();
            newExpiryMonthInfo = NewExpiryMonthTB.Text.Trim();
            newExpiryYearInfo = NewExpiryYearTB.Text.Trim();
        }

    }
}
