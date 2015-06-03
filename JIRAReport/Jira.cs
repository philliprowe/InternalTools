using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;

// using System.Windows.Forms;

namespace JiraReport
{
    class Jira : JiraReportConfig
    {
        /* Since it is unlikely to change in the near future, I will
         * hard code the elements which form the JIRA report URL as a class
         * variable */
        private Dictionary<string, string> url_template = new Dictionary<string, string>() {
            {"host", ""},
            {"Extension", "secure/ConfigureReport!pdfView.jspa"},
            {"EndDate", "endDate"},
            {"ReportKey", "reportKey=com.clearvision.jira.reoports.BradyCustomerReport:brady-customer-report"},
            {"priority", "priorityNames"},
            {"Project", "projectNames"},
            {"Customer", "customerNames"},
            {"SProject", "selectedProjectId=10001"},
            {"StartDate", "startDate"}
        };

        /* Fairly typical constructor. Nothing too fancy */
        public Jira(string xml_config):base(xml_config) {
            if (jira_config.data.ContainsKey("host"))
                url_template["host"] = jira_config.data["host"];
        }

        /* Method to login to JIRA - For the sake of this application, this is redundant */
        /*
        public bool Login(string user, string pass, string host) {
            try {
                HttpWebRequest req = WebRequest.Create(host) as HttpWebRequest;

                req.ContentType = "application/json";
                req.Method = "GET";

                string base64Credentials = GetEncodedCredentials(user, pass);
                req.Headers.Add("Authorization", "Basic " + base64Credentials);

                HttpWebResponse response = req.GetResponse() as HttpWebResponse;

                string result = string.Empty;
                using (StreamReader reader = new StreamReader(response.GetResponseStream())) {
                    result = reader.ReadToEnd();
                }
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
         */

        /* Method which forms the report string */
        public bool DownloadReport(ClientConfig client, DateTime s_date, DateTime e_date, string out_dir, string out_name) {
            string url = GenReportURL(client, s_date, e_date);

            try {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.ContentType = "application/json";
                request.Method = "GET";

                string base64Credentials = GetEncodedCredentials(jira_config.data["user"], jira_config.data["pass"]);
                request.Headers.Add("Authorization", "Basic " + base64Credentials);
                
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                Stream sourceStream = response.GetResponseStream();

                if (!Directory.Exists(out_dir))
                    Directory.CreateDirectory(out_dir);

                FileStream targetStream = File.OpenWrite(out_dir + "/" + out_name);
                int bytesRead = 0;
                byte[] buffer = new byte[2048];
                while (true) {
                    bytesRead = sourceStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                        break;
                    targetStream.Write(buffer, 0, bytesRead);
                }
                sourceStream.Close();
                targetStream.Close();

                return true;
            }
            catch (Exception ex) {
                Console.WriteLine("Exception: " + ex.Message);
                // MessageBox.Show("Exception: " + ex.Message);
                return false;
            }
        }

        /* Simple method for sending emails to clients. In future, will expand to cover HTML emails */
        public bool SendEmail(string [] msg_to, string msg_fr, string msg_sub, string msg_body, string [] msg_attach=null) {
            try {
                MailMessage mail = new MailMessage();
                SmtpClient smtp_client = new SmtpClient();

                /* Add all the people the email will go to */
                for (int i = 0; i < msg_to.Length; i++) {
                    mail.To.Add(new MailAddress(msg_to[i]));
                }

                smtp_client.Port = Convert.ToInt32(email_config.email["server_port"]);
                smtp_client.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp_client.UseDefaultCredentials = false;
                smtp_client.Host = email_config.email["server_name"];

                mail.From = new MailAddress(msg_fr);
                mail.Subject = msg_sub;
                mail.Body = msg_body;

                /* Add the attachments to the email */
                if (msg_attach != null) {
                    for (int i = 0; i < msg_attach.Count(); i++) {
                        mail.Attachments.Add( new Attachment(msg_attach[i]) );
                    }
                }

                /* Now send the email */
                smtp_client.Send(mail);
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine("Exception: " + ex.Message);
                // MessageBox.Show("Exception: " + ex.Message);
                return false;
            }
        }

        /// PRIVATE METHODS ///
        /// 
        private string GetEncodedCredentials(string user, string pass) {
            string mergedCredentials = string.Format("{0}:{1}", user, pass);
            byte[] byteCredentials = UTF8Encoding.UTF8.GetBytes(mergedCredentials);
            return Convert.ToBase64String(byteCredentials);
        }

        /* Returns the URL of the report based on the client's preferences, start date and end date */
        private string GenReportURL(ClientConfig client, DateTime s_date, DateTime e_date) {
            try {
                string url="";
                if ( (!url_template.ContainsKey("host"))||(url_template["host"] == "") )
                    throw new KeyNotFoundException("Unable to find host variable. Please make sure this is specified in the config file");
                
                url += string.Format("{0}/{1}?", url_template["host"], url_template["Extension"]);
                url += string.Format("{0}&{1}={2}&", url_template["SProject"], url_template["Customer"], Uri.EscapeDataString(client.name["full_name"]));
                url += string.Format("{0}&{1}={2}&Next=Next&", url_template["ReportKey"], url_template["StartDate"], s_date.ToString("d/MMM/yy"));

                for (int i = 0; i < client.priority.Count; i++) {
                    url += string.Format("{0}={1}&", url_template["priority"], client.priority[i]);
                }

                for (int i = 0; i < client.projects.Count; i++) {
                    url += string.Format("{0}={1}&", url_template["Project"], Uri.EscapeDataString(client.projects[i]));
                }
                // url += string.Format("{0}&{1}={2}&", url_template["SProject"], url_template["Customer"], client.name["full_name"].Replace(" ", "+"));
                url += string.Format("{0}={1}", url_template["EndDate"], e_date.ToString("d/MMM/yy"));

                return url;
            }
            catch (Exception ex) {
                string err_out = "Exception: " + ex.Message;
                Console.WriteLine(err_out);
                // MessageBox.Show(err_out, "Exception occurred");
                return err_out;
            }
        }
    }
}
