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
                    NewSystemCode.Text = NewClientTB.Text.Trim();
                }

                if (NewEntityNameTB.Text.Trim() != "" && EntityBL.SelectedIndex == 1)
                {
                    AddNewBttn.Enabled = true;
                    SaveMultipleBttn.Enabled = true;
                    NewVersionTB.Focus();
                    NewSystemCode.Text = NewClientTB.Text.Trim() + "." + NewEntityNameTB.Text.Trim();
                }
            }
        }
        protected void GoBack_Click(object sender, EventArgs e)
        {
            // Checks for unsaved files and deletes them 
            string[] clientsIncPath = Directory.GetFiles(path, "*.txt");
            string deleteFileName = String.Empty;

            foreach (string ClientName in clientsIncPath)
            {

                string Client = Path.GetFileNameWithoutExtension(ClientName);
                if (!Client.StartsWith("_") & !Client.StartsWith("$") & !Client.StartsWith("%"))
                {
                    using (StreamReader sr = new StreamReader(path + "\\" + Client + ".txt"))
                    {
                        if (sr.ReadLine() == "*")
                        {
                            deleteFileName = Client;
                        }
                    }
                    if (deleteFileName != String.Empty)
                    {
                        File.Delete(path + "\\" + deleteFileName + ".txt");
                    }
                    deleteFileName = String.Empty;
                }
            }
            
            // Opens the generate form
            Response.Redirect("~/Generate.aspx");
        }
        protected void HideAllNewComments()
        {
            // Hides all the Comments
            NewClientComment.Visible = false;
            EntitiesComment.Visible = false;
            NewEntityNameComment.Visible = false;
            NewVersionComment.Visible = false;
            NewLastSPComment.Visible = false;
            NewSystemCodeComment.Visible = false;
            NewActivationTypeComment.Visible = false;
            NewInteractiveLicensesComment.Visible = false;
            NewServiceLicensesComment.Visible = false;
            NewExpiryMonthComment.Visible = false;
            NewExpiryYearComment.Visible = false;
        }
        protected void SaveSingleBttn_Click(object sender, EventArgs e)
        {
            HideAllNewComments();

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
            HideAllNewComments(); // Hides all warnings at this point

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
                HideAllNewComments(); // First hide all Comments

                string newClientName = NewClientTB.Text.Trim();

                // Determine number of entities for this Client            
                int rowNumber = File.ReadAllLines(path + "\\" + newClientName + ".txt").Length;
                int entityNumber = rowNumber / 11;

                // Copy file contents into a string
                string[] thisFile = File.ReadAllLines(path + "\\" + newClientName + ".txt");

                // Write out entity number and append file contents
                using(StreamWriter sw = new StreamWriter(path + "\\" + newClientName + ".txt"))
                {
                    sw.WriteLine("Entity Number:" + entityNumber);
                    for (int i = 0; i < thisFile.Length; i++)
                    {
                        sw.WriteLine(thisFile[i]);
                    }
                }
                NewClientComment.Text = "New Client successfully saved";
                NewClientComment.Visible = true;
                ClearAllFields();
            }
            catch(Exception ex)
            {
                // Normally when the Client list has been moved
                Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('Could not get Client List.');", true);
                NewClientComment.Text = ex.Message;
                NewClientComment.Visible = true;
            }            
        }
        private bool SaveNewClientValidation(string newClient, string newEntityName, int newActivationType, string newInteractiveLicenses, string newServiceLicenses, string newExpiryMonth, string newExpiryYear, string newSystemCode, string newVersion, string newLastSP)
        {
            // If there is an error, returns false and displays the relevant Comment displaying the validation error
            bool valid = true;
            double Num;
            bool isNum;
            bool isRightDigits;

            // New Client name
            if (newClient == "")
            {
                valid = false;
                NewClientComment.Text = "Client's Name is required";
                NewClientComment.Visible = true;
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
                    NewEntityNameComment.Text = "Entity Name is required";
                    NewEntityNameComment.Visible = true;
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
                NewActivationTypeComment.Text = "Select an Activation type";
                NewActivationTypeComment.Visible = true;
            }

            // InteractiveLicenses
            if (!(newInteractiveLicenses == ""))
            {
                isNum = double.TryParse(newInteractiveLicenses, out Num);
                if (!isNum)
                {
                    valid = false;
                    NewInteractiveLicensesComment.Text = "Interactive Licenses must be a number";
                    NewInteractiveLicensesComment.Visible = true;
                }
            }
            else
            {
                valid = false;
                NewInteractiveLicensesComment.Text = "Interactive Licenses is required";
                NewInteractiveLicensesComment.Visible = true;

            }

            // ServiceLicenses
            if (!(newServiceLicenses == ""))
            {
                isNum = double.TryParse(newServiceLicenses, out Num);
                if (!isNum)
                {
                    valid = false;
                    NewServiceLicensesComment.Text = "Service Licenses must be a number";
                    NewServiceLicensesComment.Visible = true;
                }
            }
            else
            {
                valid = false;
                NewServiceLicensesComment.Text = "Service Licenses is required";
                NewServiceLicensesComment.Visible = true;
            }

            // Expiry Month
            if (!(newExpiryMonth == ""))
            {
                isNum = double.TryParse(newExpiryMonth, out Num);
                isRightDigits = (newExpiryMonth.Length == 2);
                if (!isNum)
                {
                    valid = false;
                    NewExpiryMonthComment.Text = "Expiry Month must be a number and of the form MM";
                    NewExpiryMonthComment.Visible = true;
                }
                else if (Num > 12 | Num < 1)
                {
                    valid = false;
                    NewExpiryMonthComment.Text = "Expiry Month must be between 01 and 12";
                    NewExpiryMonthComment.Visible = true;
                }

                if (!isRightDigits)
                {
                    valid = false;
                    NewExpiryMonthComment.Text = "Expiry Month must be a number and of the form MM";
                    NewExpiryMonthComment.Visible = true;
                }
            }
            else
            {
                valid = false;
                NewExpiryMonthComment.Text = "Expiry Month is required";
                NewExpiryMonthComment.Visible = true;
            }

            // Expiry Year
            if (!(newExpiryYear == ""))
            {
                isNum = double.TryParse(newExpiryYear, out Num);
                isRightDigits = (newExpiryYear.Length == 4);
                if (!isNum)
                {
                    valid = false;
                    NewExpiryYearComment.Text = "Expiry Year must be a number and of the form YYYY";
                    NewExpiryYearComment.Visible = true;
                }
                if (!isRightDigits)
                {
                    valid = false;
                    NewExpiryYearComment.Text = "Expiry Year must be a number and of the form YYYY";
                    NewExpiryYearComment.Visible = true;
                }
            }
            else
            {
                valid = false;
                NewExpiryYearComment.Text = "Expiry Year is required";
                NewExpiryYearComment.Visible = true;
            }

            // Client code
            if (newSystemCode.Equals(""))
            {
                valid = false;
                NewSystemCodeComment.Text = ("System Code is Required");
                NewSystemCodeComment.Visible = true;
            }

            // Version
            isNum = double.TryParse(newVersion, out Num);
            if (!isNum || Num < 2010)
            {
                valid = false;
                NewVersionComment.Text = "Trinity Version is not correct";
                NewVersionComment.Visible = true;
            }
            if (newVersion == "")
            {
                valid = false;
                NewVersionComment.Text = "Trinity Version is required";
                NewVersionComment.Visible = true;
            }

            // Service pack
            isNum = double.TryParse(newLastSP, out Num);
            if (!isNum || Num < 0)
            {
                valid = false;
                NewLastSPComment.Text = "Last Service Pack is not correct";
                NewLastSPComment.Visible = true;
            }
            if (newLastSP == "")
            {
                valid = false;
                NewLastSPComment.Text = "Last Service Pack is required";
                NewLastSPComment.Visible = true;
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
                    using(StreamWriter sr = new StreamWriter(path + "\\" + newClient + ".txt"))
                    {
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
                    }
                    NewClientComment.Text = "New Client sucessfully saved: " + newClient + ".";
                    NewClientComment.Visible = true;
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('Could not save new Client\\'s details.');", true);
                    NewClientComment.Text = ex.Message;
                    NewClientComment.Visible = true;
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
                    }

                    NewEntityNameComment.Text = "New Entity sucessfully saved: " + newEntityName + ".";
                    NewEntityNameComment.Visible = true;
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('Could not save new Client\\'s details.');", true);
                    NewEntityNameComment.Text = ex.Message;
                    NewEntityNameComment.Visible = true;
                }
            }
        }
        protected void EntityBL_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool visible = (EntityBL.SelectedValue == "Multiple");

            // Show or hide multiple entity fields on change of selection, retains info in TB incase change is made erroneously
            NewEntityID.Visible = visible;
            MEBLRow.Visible = visible;
            SaveSingleBttn.Visible = (!visible);
        }
        protected void ClearAllFields()
        {
            // Deletes values stored in textboxes
            NewEntityNameTB.Text = "";
            NewEntityNameComment.Text = "";
            NewVersionTB.Text = "";
            NewLastSPTB.Text = "";
            NewEmailToTB.Text = "";
            NewSystemCode.Text = "";
            NewActivationTypeBL.ClearSelection();
            NewInteractiveLicensesTB.Text = "";
            NewServiceLicensesTB.Text = "";
            NewExpiryMonthTB.Text = "";
            NewExpiryYearTB.Text = "";
        }
        protected void ResetBttn_Click(object sender, EventArgs e)
        {
            HideAllNewComments();
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
                                NewClientComment.Text = "This client name already exists";
                                NewClientComment.Visible = true;
                                valid = false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Normally when the Client list has been moved
                    Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('Could not get Client List.');", true);
                    NewClientComment.Text = ex.Message;
                    NewClientComment.Visible = true;
                }
            }

            // Validate Multiple Entities
            else if (EntityBL.SelectedValue == "Multiple")
            {
                try
                {
                    if (!(File.Exists(path + "\\" + newClientName + ".txt")))
                    {
                        // Any Entity name is allowed if Client name does not yet exist
                    }
                    else
                    {
                        using(StreamReader sr = new StreamReader(path + "\\" + newClientName + ".txt"))
                        {

                        // Check if Client name already exists for multiple entities
                            if (!(sr.ReadLine() == "*"))
                            {
                                NewClientComment.Text = "This client name already exists";
                                NewClientComment.Visible = true;
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
                                            NewEntityNameComment.Text = "This Entity name already exists";
                                            NewEntityNameComment.Visible = true;
                                            valid = false;
                                            break;
                                        }                                    
                                    }
                                }
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('Could not validate client and entity name');", true);
                    NewClientComment.Text = ex.Message;
                    NewClientComment.Visible = true;
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
            newSystemCodeInfo = NewSystemCode.Text.Trim();
            newActivationTypeInfo = NewActivationTypeBL.SelectedIndex;
            newInteractiveLicensesInfo = NewInteractiveLicensesTB.Text.Trim();
            newServiceLicensesInfo = NewServiceLicensesTB.Text.Trim();
            newExpiryMonthInfo = NewExpiryMonthTB.Text.Trim();
            newExpiryYearInfo = NewExpiryYearTB.Text.Trim();
        }

    }
} 