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
        
        public string path = "C:\\ActivationKeyGenerator\\_Clients"; //the folder that stores the client files
        protected void Page_Load(object sender, EventArgs e)
        {
            SaveNewClientBttn.Attributes.Add("onclick", "return confirm('Warning: This will create a new Client.\\r\\nSave a new Client?');"); //Creates a dialogue shown when the amend client details button is pressed. If cancel is pressed it doesn't continue execution.
            NewClientTB.Focus();
        }

        protected void GoBack_Click(object sender, EventArgs e)
        {
            //Opens the generate form
            Response.Redirect("~/Generate.aspx");
        }
        protected void ClearAllLabels()
        {
            //Hides all the labels
            NewClientLabel.Visible = false;
            NewLimitLabel.Visible = false;
            NewExtendedLimitLabel.Visible = false;
            NewExpiryMonthLabel.Visible = false;
            NewExpiryYearLabel.Visible = false;
            
        }
        protected void SaveNewClientBttn_Click(object sender, EventArgs e)
        {
            ClearAllLabels();
            //Takes all the information stored in the form
            string newClient = NewClientTB.Text.Trim();
            string newVersion  = NewVersionTB.Text.Trim();
            string newLastSP = NewLastSPTB.Text.Trim();
            string newEmailTo = NewEmailToTB.Text.Trim();
            string newSystemCode = NewSystemCodeTB.Text.Trim();
            int newActivationType = NewActivationTypeBL.SelectedIndex;
            string newLimit = NewLimitTB.Text.Trim();
            string newExtendedLimit = NewExtendedLimitTB.Text.Trim();
            string newExpiryMonth = NewExpiryMonthTB.Text.Trim();
            string newExpiryYear = NewExpiryYearTB.Text.Trim();

            if (SaveNewClientValidation(newClient, newActivationType, newLimit, newExtendedLimit, newExpiryMonth, newExpiryYear))
            {
                //if it passes validation
                SaveNewClient(newClient, newVersion, newLastSP, newEmailTo, newSystemCode, (newActivationType == 0 ? "Concurrent" : "Named users"), newLimit, newExtendedLimit, newExpiryMonth, newExpiryYear);
            }
        }
        protected bool SaveNewClientValidation(string newClient, int newActivationType, string newLimit, string newExtendedLimit, string newExpiryMonth, string newExpiryYear)
        {
            //if there is an error, returns false and displays the relevant label displaying the validation error
            bool valid = true;
            double Num;
            bool isNum;
            bool isRightDigits;
            //New Client
            if (newClient == "")
            {
                valid = false;
                NewClientLabel.Text = "Client's Name is required.";
                NewClientLabel.Visible = true;
            }          
            
            //Activation Type
            if (newActivationType.Equals(-1))
            {
                valid = false;
                NewActivationTypeLabel.Text = "Select an Activation type.";
                NewActivationTypeLabel.Visible = true;
            }
            //Limit
            if (!(newLimit == ""))
            {
                isNum = double.TryParse(newLimit, out Num);
                if (!isNum)
                {
                    valid = false;
                    NewLimitLabel.Text = "Limit must be a number.";
                    NewLimitLabel.Visible = true;
                }
            }
            //Extended Limit
            if (!(newExtendedLimit == ""))
            {
                isNum = double.TryParse(newExtendedLimit, out Num);
                if (!isNum)
                {
                    valid = false;
                    NewExtendedLimitLabel.Text = "Extended Limit must be a number.";
                    NewExtendedLimitLabel.Visible = true;
                }
            }
            //Expiry Month
            if (!(newExpiryMonth == ""))
            {
                isNum = double.TryParse(newExpiryMonth, out Num);
                isRightDigits = (newExpiryMonth.Length == 2);
                if (!isNum)
                {
                    valid = false;
                    NewExpiryMonthLabel.Text = "Expiry Month must be a number and of the form MM.";
                    NewExpiryMonthLabel.Visible = true;
                }
                else
                {
                    if (Num > 12 | Num < 1)
                    {
                        valid = false;
                        NewExpiryMonthLabel.Text = "Expiry Month must be between 01 and 12.";
                        NewExpiryMonthLabel.Visible = true;
                    }
                }

                if (!isRightDigits)
                {
                    valid = false;
                    NewExpiryMonthLabel.Text = "Expiry Month must be a number and of the form MM.";
                    NewExpiryMonthLabel.Visible = true;
                }
            }
            //Expiry Year
            if (!(newExpiryYear == ""))
            {
                isNum = double.TryParse(newExpiryYear, out Num);
                isRightDigits = (newExpiryYear.Length == 4);
                if (!isNum)
                {
                    valid = false;
                    NewExpiryYearLabel.Text = "Expiry Year must be a number and of the form YYYY.";
                    NewExpiryYearLabel.Visible = true;
                }
                if (!isRightDigits)
                {
                    valid = false;
                    NewExpiryYearLabel.Text = "Expiry Year must be a number and of the form YYYY.";
                    NewExpiryYearLabel.Visible = true;
                }
            }

            return valid;
        }
        
        protected void SaveNewClient(string newClient, string newVersion, string newLastSP, string newEmailTo, string newSystemCode, string newActivationType, string newLimit, string newExtendedLimit, string newExpiryMonth, string newExpiryYear)
        {
            //Writes all the Client to the Client's text file.  
            try
            {
                StreamWriter sr = new StreamWriter(path + "\\" + newClient + ".txt");
                sr.WriteLine(newVersion);
                sr.WriteLine();
                sr.WriteLine(newEmailTo);
                sr.WriteLine(newLastSP);
                sr.WriteLine(newSystemCode);
                sr.WriteLine(newActivationType);
                sr.WriteLine(newLimit);
                sr.WriteLine(newExtendedLimit);
                sr.WriteLine(newExpiryMonth);
                sr.WriteLine(newExpiryYear);

                sr.Close();
                sr.Dispose();
                NewClientLabel.Text = "New Client sucessfully saved: " + newClient +".";
                NewClientLabel.Visible = true;
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('Could not save new Client\\'s details.');", true);
                NewClientLabel.Text = ex.Message;
                NewClientLabel.Visible = true;
            }
       }
    }
}