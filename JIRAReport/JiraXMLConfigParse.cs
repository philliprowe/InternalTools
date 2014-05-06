using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;


/* Define the classes which are going to play a part in the XML parsing */
namespace JiraReport {
    public class JiraConfig {
        public Dictionary<string, string> data;

        public JiraConfig() {
            data = new Dictionary<string, string>();
        }
    }

    public class ClientConfig {
        public Dictionary<string, string> name;
        public List<String> projects;
        public List<String> priority;
        public List<String> email;

        public ClientConfig() {
            name = new Dictionary<string, string>();
            projects = new List<string>();
            priority = new List<string>();
            email = new List<string>();
        }
    }

    public class FileConfig {
        public Dictionary<string, string> output;

        public FileConfig() {
            output = new Dictionary<string, string>();
        }
    }

    public class EmailConfig {
        public Dictionary<string, string> email;

        public EmailConfig() {
            email = new Dictionary<string, string>();
        }
    }

    public class TemplateMarkup {
        public Dictionary<string, string> markup_delim;

        public TemplateMarkup() {
            markup_delim = new Dictionary<string, string>();
        }
    }
}

/* Main XML parsing class to deal with the config file */
namespace JiraReport
{
    class JiraReportConfig
    {
        /* protected vars taken from the config file */
        protected JiraConfig jira_config;
        protected List<ClientConfig> client_config;
        protected FileConfig file_config;
        protected EmailConfig email_config;
        protected TemplateMarkup template_config;

        /* JiraReportConfig file constructor */
        public JiraReportConfig(string config_file=null) {
            /* Init the variables */
            jira_config = new JiraConfig();
            file_config = new FileConfig();
            email_config = new EmailConfig();
            client_config = new List<ClientConfig>();
            template_config = new TemplateMarkup();

            if (config_file != null) {
                if (!LoadConfig(config_file)) {
                    Console.WriteLine("Unable to read XML config file: " + config_file);
                }
                else {
                    Console.WriteLine("XML config file parsed successfully");
                }
            }
        }

        /* Method to load the XML config file */
        public bool LoadConfig(string config_file) {
            bool ret=true;
            try {
                /* Clean up any data which already exists in this class */
                CleanUp();

                /* Open and parse the XML config file */
                XmlReader reader = XmlReader.Create(config_file);

                while (reader.Read()) {
                    if (reader.IsStartElement()) {
                        switch (reader.Name) {
                            case "jira_config":
                                if (!ExtractAttributes(reader)) {
                                    Console.WriteLine("Failed to extract Jira configuration settings.");
                                    ret = false;
                                }
                                break;
                            case "file_config":
                                if (!ExtractAttributes(reader)) {
                                    Console.WriteLine("Failed to extract file configuration settings.");
                                    ret = false;
                                }
                                break;
                            case "email_config":
                                if (!ExtractAttributes(reader)) {
                                    Console.WriteLine("Failed to extract email configuration settings.");
                                    ret = false;
                                }
                                break;
                            case "client_config":
                                XmlReader c_child = reader.ReadSubtree();
                                if (!ExtractClientConfig(c_child)) {
                                    Console.WriteLine("Failed to extract client configuration settings.");
                                    ret = false;
                                }
                                break;
                            case "template_config":
                                XmlReader t_child = reader.ReadSubtree();
                                while (t_child.Read()) {
                                    if (t_child.Name == "template_str") {
                                        if (!ExtractAttributes(t_child)) {
                                            Console.WriteLine("Failed to extract template configuration settings.");
                                            return false;
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
                return ret;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /* Method used for internal clean up - a bit unnecessary,
         * but makes this class more complete. */
        public void CleanUp() {
            if(jira_config.data.Count != 0)
                jira_config.data.Clear();
            if(file_config.output.Count != 0)
                file_config.output.Clear();
            if(email_config.email.Count != 0)
                email_config.email.Clear();
            if(template_config.markup_delim.Count != 0)
                template_config.markup_delim.Clear();
            for (int i = 0; i < client_config.Count; i++) {
                if (client_config[i].name.Count != 0)
                    client_config[i].name.Clear();
                if (client_config[i].priority.Count != 0)
                    client_config[i].priority.Clear();
                if (client_config[i].projects.Count != 0)
                    client_config[i].projects.Clear();
                if (client_config[i].email.Count != 0)
                    client_config[i].email.Clear();
            }
        }

        /* Simple method which applies the markup */
        public string ApplyMarkup(string msg, ClientConfig client) {
            DateTime c_date = DateTime.Now;
            foreach (KeyValuePair<string, string> pair in template_config.markup_delim) {
                switch(pair.Value) {
                    case "ShortName":
                        msg = msg.Replace(pair.Key, client.name["short_name"]);
                        break;
                    case "FullName":
                        msg = msg.Replace(pair.Key, client.name["full_name"]);
                        break;
                    case "CurrentDay":
                        msg = msg.Replace(pair.Key, c_date.ToString("dd"));
                        break;
                    case "CurrentMonth":
                        msg = msg.Replace(pair.Key, c_date.ToString("MMM"));
                        break;
                    case "CurrentYear":
                        msg = msg.Replace(pair.Key, c_date.ToString("yyyy"));
                        break;
                    case "PreviousMonth":
                        DateTime p_date = DateTime.Now.AddMonths(-1);
                        msg = msg.Replace(pair.Key, p_date.ToString("MMM"));
                        break;
                    case "PreviousMonthFull":
                        DateTime p_date_f = DateTime.Now.AddMonths(-1);
                        msg = msg.Replace(pair.Key, p_date_f.ToString("MMMM"));
                        break;
                    default:
                        msg = msg.Replace(pair.Key, pair.Value);
                        break;
                }
                
            }

            return msg;
        }

        /* Getter method for the JIRA credentials etc */
        public JiraConfig GetJiraConfig() {
            return jira_config;
        }

        public void SetJiraSettings(string user, string pass, string host="") {
            if (host != "")
                jira_config.data["host"] = host;
            jira_config.data["user"] = user;
            jira_config.data["pass"] = pass;
        }

        /* Getter method for the entire list of client configs */
        public List<ClientConfig> GetClientConfigL() {
            return client_config;
        }

        /* Getter method for an individual client */
        public ClientConfig GetClientConfig(uint idx) {
            try {
                return client_config[(int)idx];
            }
            catch (Exception ex) {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        public FileConfig GetFileConfig() {
            return file_config;
        }

        public EmailConfig GetEmailConfig() {
            return email_config;
        }

        /// PRIVATE METHODS ///
        /// 
        /* Method which extracts the attributes from an XML node.
         * All attributes in the XML file are declared here. Each
         * should have a unique name. It would be possible to allow 
         * node scoped attributes by adding extra parameters. Might
         * do this in the future if need this method to do it. */
        private bool ExtractAttributes(XmlReader reader) {
            if (reader.HasAttributes) {
                while (reader.MoveToNextAttribute()) {
                    // Console.WriteLine(reader.Name);
                    switch (reader.Name) {
                        /* Jira Config variables */
                        case "host":
                            jira_config.data.Add("host", reader.Value);
                            break;
                        case "user":
                            jira_config.data.Add("user", reader.Value);
                            break;
                        case "pass":
                            jira_config.data.Add("pass", reader.Value);
                            break;

                        /* File Config variables */
                        case "outputDir":
                            file_config.output.Add("output_dir", reader.Value);
                            break;
                        case "outputName":
                            file_config.output.Add("output_name", reader.Value);
                            break;
                        case "outputExt":
                            file_config.output.Add("output_ext", reader.Value);
                            break;

                        /* Email Config variables */
                        case "serverName":
                            email_config.email.Add("server_name", reader.Value);
                            break;
                        case "serverPort":
                            email_config.email.Add("server_port", reader.Value);
                            break;
                        case "senderEmail":
                            email_config.email.Add("sender_email", reader.Value);
                            break;
                        case "emailSubject":
                            email_config.email.Add("email_subject", reader.Value);
                            break;
                        case "emailBody":
                            email_config.email.Add("email_body", reader.Value);
                            break;
                        case "emailTemplate":
                            email_config.email.Add("email_template", reader.Value);
                            break;
                        case "emailType":
                            email_config.email.Add("email_type", reader.Value);
                            break;
                        
                        /* Template config */
                        case "placeHolder":
                            string ph = reader.Value;
                            if (reader.MoveToNextAttribute()) {
                                if (reader.Name == "content") {
                                    // Console.WriteLine("Template Markup defined: " + ph + " ; " + reader.Value);
                                    template_config.markup_delim.Add(ph, reader.Value);
                                }
                                else
                                    Console.WriteLine("Unknown template configuration attribute: " + reader.Name);
                            }
                            else
                                Console.WriteLine("Unable to process Template Attribute: " + ph);
                            break;

                        default:
                            Console.WriteLine("Unknown report configuration attribute: " + reader.Name);
                            return false;
                    }
                }
            }

            return true;
        }

        /* Method to extract the client specific parts of the config file */
        private bool ExtractClientConfig(XmlReader reader) {
            try {
                while (reader.Read()) {
                    if (reader.IsStartElement()) {
                        if (reader.Name == "client") {
                            client_config.Add(new ClientConfig());
                            while (reader.MoveToNextAttribute()) {
                                if (reader.Name == "name")
                                    client_config.Last().name.Add("short_name", reader.Value);
                                else if (reader.Name == "lname")
                                    client_config.Last().name.Add("full_name", reader.Value);
                                else
                                    Console.WriteLine("Unknown report configuration attribute: " + reader.Name);
                            }
                        }
                        else if (reader.Name == "project")
                            client_config.Last().projects.Add(reader.ReadString());
                        else if (reader.Name == "priority")
                            client_config.Last().priority.Add(reader.ReadString());
                        else if (reader.Name == "email")
                            client_config.Last().email.Add(reader.ReadString());
                    }
                }
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine("Exception when extracting client data from config XML: " + ex.Message);
                return false;
            }
        }
    }
}
