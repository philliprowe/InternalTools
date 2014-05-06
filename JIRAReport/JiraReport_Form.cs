using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JiraReport {
    public partial class JiraReport_UI : Form {

        /* Instance to the JIRA class and all associated functionality */
        private Jira jira = null;

        public JiraReport_UI() {
            InitializeComponent();
        }

        /* Open the config file */
        private void ms_file_OpenConfig_Click(object sender, EventArgs e) {
            DialogResult ret = open_config_dialog.ShowDialog();
            if (ret == DialogResult.OK) {
                string file = open_config_dialog.FileName;
                jira = new Jira(file);
                
                /* Now populate the JIRA settings */
                JiraConfig j_config = jira.GetJiraConfig();
                if(j_config.data.ContainsKey("host"))
                    jira_server.Text = j_config.data["host"];
                if(j_config.data.ContainsKey("user"))
                    jira_username.Text = j_config.data["user"];
                if (j_config.data.ContainsKey("pass"))
                    jira_password.Text = j_config.data["pass"];

                /* Now populate the client selection list */
                List<ClientConfig> c_config = jira.GetClientConfigL();
                foreach(ClientConfig client in c_config) {
                    if(!client_selection_cb.Items.Contains(client.name["short_name"]))
                        client_selection_cb.Items.Add(client.name["short_name"]);
                }

                for (int i = 0; i < client_selection_cb.Items.Count; i++) {
                    client_selection_cb.SetItemChecked(i, true);
                }

                /* Now populate the output directory info */
                FileConfig f_config = jira.GetFileConfig();
                if (f_config.output.ContainsKey("output_dir"))
                    report_out_dir_tb.Text = f_config.output["output_dir"];
                if (f_config.output.ContainsKey("output_name"))
                    report_out_name_tb.Text = f_config.output["output_name"];
                if (f_config.output.ContainsKey("output_ext"))
                    report_out_name_tb.Text += "." + f_config.output["output_ext"];

                /* Now populate the email contents */
                EmailConfig e_config = jira.GetEmailConfig();
                if (e_config.email.ContainsKey("email_subject"))
                    email_sub_tb.Text = e_config.email["email_subject"];
                if (e_config.email.ContainsKey("email_template")&&(e_config.email["email_template"] != "")) {
                    try {
                        string template = System.IO.File.ReadAllText(e_config.email["email_template"]);
                        email_body_tb.Text = template;
                    }
                    catch (Exception ex) {
                        MessageBox.Show("Exception loading email template file: " + ex.Message);
                    }
                }
                else {
                    if (e_config.email.ContainsKey("email_body"))
                        email_body_tb.Text = e_config.email["email_body"];
                    else
                        email_body_tb.Text = "";
                }

                email_selected.Checked = true;
                no_email.Checked = false;
            }
            else if (ret == DialogResult.Cancel) {
                /* Do something here */
            }
            else {
                MessageBox.Show("Unable to open specified config file");
            }
        }

        /* If the 'select all' check box is selected or deselected, change the status of the clients */
        private void client_select_cb_CheckedChanged(object sender, EventArgs e) {
            for (int i = 0; i < client_selection_cb.Items.Count; i++) {
                client_selection_cb.SetItemChecked(i, (client_select_cb.Checked ? true : false));
            }
        }

        /* If one is selected, deactivate the other selection */
        private void email_selected_CheckedChanged(object sender, EventArgs e) {
            if (email_selected.Checked) {
                no_email.Checked = false;
            }
        }

        private void no_email_CheckedChanged(object sender, EventArgs e) {
            if (no_email.Checked) {
                email_selected.Checked = false;
            }
        }

        /* If the exit button is selected, ask the user if they want to stay or go */
        private void ms_file_Exit_Click(object sender, EventArgs e) {
            DialogResult ret = MessageBox.Show("Are you sure you want to exit?", "Quit Application", MessageBoxButtons.YesNo);
            if (ret == DialogResult.Yes) {
                Application.Exit();
            }
        }

        /* This is where the bulk of the functionality hidden in the lower layers gets called */
        private void jira_report_execute_Click(object sender, EventArgs e) {
            try {
                /* Check to see if we have an instance of the JIRA class */
                if (jira == null)
                    throw new Exception("Config file required - Please load in a config file before continuing");

                List<ClientConfig> selected_clients = new List<ClientConfig>();
                List<ClientConfig> client_list = jira.GetClientConfigL();

                /* Check that there is nothing missing from the JIRA settings */
                if ( (jira_server.Text == "") || (jira_username.Text == "") || (jira_password.Text == "") )
                    throw new Exception("Unable to generate reports without JIRA server and user credentials information");

                /* Set the User/Pass - assume the user changed these from when they loaded up the config file */
                jira.SetJiraSettings(jira_username.Text, jira_password.Text, jira_server.Text);

                for (int i = 0; i < client_selection_cb.Items.Count; i++) {
                    if (client_selection_cb.GetItemChecked(i)) {
                        for (int j = 0; j < client_list.Count; j++) {
                            if (client_list[j].name["short_name"] == client_selection_cb.Items[i].ToString()) {
                                selected_clients.Add(client_list[j]);
                                break;
                            }
                        }
                    }
                }

                EmailConfig e_config = jira.GetEmailConfig();
                // MessageBox.Show("Selected Clients: " + selected_clients.Count);

                /* Next we go down the list and we generate the reports and emails for each client */
                for (int i = 0; i < selected_clients.Count; i++) {
                    string out_dir = report_out_dir_tb.Text;
                    string out_fname = report_out_name_tb.Text;

                    /* Apply markup to the directory name and output filename */
                    if (apply_markup.Checked) {
                        out_dir = jira.ApplyMarkup(report_out_dir_tb.Text, selected_clients[i]);
                        out_fname = jira.ApplyMarkup(report_out_name_tb.Text, selected_clients[i]);
                    }

                    /* Generate the report and save to the hard drive */
                    if (jira.DownloadReport(selected_clients[i], date_config_start.Value.Date, date_config_end.Value.Date, out_dir, out_fname) != true) {
                        throw new Exception("Unexpected error occurred during report generation");
                    }

                    /* Apply mark up to the email */
                    if (email_selected.Checked) {
                        string email_sub = email_sub_tb.Text;
                        string email_body = email_body_tb.Text;
                        if (apply_markup.Checked) {
                            email_sub = jira.ApplyMarkup(email_sub_tb.Text, selected_clients[i]);
                            email_body = jira.ApplyMarkup(email_body_tb.Text, selected_clients[i]);
                        }

                        string[] email_list = selected_clients[i].email.ToArray();
                        string[] attachments = {out_dir + (out_dir.EndsWith("/")||out_dir.EndsWith("\\") ? out_fname : "/" + out_fname) };

                        /* Send the email to the clients */
                        if (!jira.SendEmail(email_list, e_config.email["sender_email"], email_sub, email_body, attachments)) {
                            throw new Exception("Unexpected error occurred during report emailing to clients");
                        }
                    }

                    /* Update the progress bar so the user knows how far along the progress is */
                    if (i+1 != selected_clients.Count)
                        jira_report_progress.Value = (int)(100 * ((float)(i+1) / (float)selected_clients.Count));
                    else if (i+1 == selected_clients.Count)
                        jira_report_progress.Value = 100;
                }
                MessageBox.Show("JIRA report process has completed successfully", "Report process status");
            }
            catch (Exception ex) {
                MessageBox.Show("Exception: " + ex.Message, "Exception Occurred");
            }
        }

        /* Simple messagebox showing information about the program */
        private void ms_info_about_Click(object sender, EventArgs e) {
            MessageBox.Show("JIRA Customer Report Tool v2.0\n\nWritten by Dominic Finch for use by Brady PLC.", "About JIRA Customer Report Tool");
        }



    }
}
