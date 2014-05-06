namespace JiraReport {
    partial class JiraReport_UI {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.email_config = new System.Windows.Forms.GroupBox();
            this.email_body_tb = new System.Windows.Forms.RichTextBox();
            this.email_sub_tb = new System.Windows.Forms.TextBox();
            this.jira_report_progress = new System.Windows.Forms.ProgressBar();
            this.jira_settings_gb = new System.Windows.Forms.GroupBox();
            this.jira_pass = new System.Windows.Forms.Label();
            this.jira_password = new System.Windows.Forms.TextBox();
            this.jira_username = new System.Windows.Forms.TextBox();
            this.jira_user = new System.Windows.Forms.Label();
            this.jira_server = new System.Windows.Forms.TextBox();
            this.jira_host = new System.Windows.Forms.Label();
            this.client_select_cb = new System.Windows.Forms.CheckBox();
            this.client_selection_cb = new System.Windows.Forms.CheckedListBox();
            this.date_config_end = new System.Windows.Forms.DateTimePicker();
            this.date_config_start = new System.Windows.Forms.DateTimePicker();
            this.report_out_dir_gb = new System.Windows.Forms.GroupBox();
            this.report_out_name_label = new System.Windows.Forms.Label();
            this.report_out_dir_label = new System.Windows.Forms.Label();
            this.report_out_name_tb = new System.Windows.Forms.TextBox();
            this.report_out_dir_tb = new System.Windows.Forms.TextBox();
            this.jira_report_execute = new System.Windows.Forms.Button();
            this.apply_markup = new System.Windows.Forms.CheckBox();
            this.no_email = new System.Windows.Forms.RadioButton();
            this.client_options_gb = new System.Windows.Forms.GroupBox();
            this.email_selected = new System.Windows.Forms.RadioButton();
            this.client_gb = new System.Windows.Forms.GroupBox();
            this.date_start = new System.Windows.Forms.Label();
            this.jira_report_menu = new System.Windows.Forms.MenuStrip();
            this.ms_file = new System.Windows.Forms.ToolStripMenuItem();
            this.ms_file_OpenConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.ms_file_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.ms_info = new System.Windows.Forms.ToolStripMenuItem();
            this.ms_info_about = new System.Windows.Forms.ToolStripMenuItem();
            this.date_config = new System.Windows.Forms.GroupBox();
            this.date_end = new System.Windows.Forms.Label();
            this.open_config_dialog = new System.Windows.Forms.OpenFileDialog();
            this.email_config.SuspendLayout();
            this.jira_settings_gb.SuspendLayout();
            this.report_out_dir_gb.SuspendLayout();
            this.client_options_gb.SuspendLayout();
            this.client_gb.SuspendLayout();
            this.jira_report_menu.SuspendLayout();
            this.date_config.SuspendLayout();
            this.SuspendLayout();
            // 
            // email_config
            // 
            this.email_config.Controls.Add(this.email_body_tb);
            this.email_config.Controls.Add(this.email_sub_tb);
            this.email_config.Location = new System.Drawing.Point(172, 140);
            this.email_config.Name = "email_config";
            this.email_config.Size = new System.Drawing.Size(300, 156);
            this.email_config.TabIndex = 15;
            this.email_config.TabStop = false;
            this.email_config.Text = "Email Contents";
            // 
            // email_body_tb
            // 
            this.email_body_tb.Location = new System.Drawing.Point(7, 45);
            this.email_body_tb.Name = "email_body_tb";
            this.email_body_tb.Size = new System.Drawing.Size(286, 105);
            this.email_body_tb.TabIndex = 1;
            this.email_body_tb.Text = "This will be the content of the email sent to clients";
            // 
            // email_sub_tb
            // 
            this.email_sub_tb.Location = new System.Drawing.Point(8, 19);
            this.email_sub_tb.Name = "email_sub_tb";
            this.email_sub_tb.Size = new System.Drawing.Size(285, 20);
            this.email_sub_tb.TabIndex = 0;
            this.email_sub_tb.Text = "This will be the subject of the email sent to clients";
            // 
            // jira_report_progress
            // 
            this.jira_report_progress.Location = new System.Drawing.Point(172, 483);
            this.jira_report_progress.Name = "jira_report_progress";
            this.jira_report_progress.Size = new System.Drawing.Size(300, 23);
            this.jira_report_progress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.jira_report_progress.TabIndex = 14;
            this.jira_report_progress.Tag = "";
            // 
            // jira_settings_gb
            // 
            this.jira_settings_gb.Controls.Add(this.jira_pass);
            this.jira_settings_gb.Controls.Add(this.jira_password);
            this.jira_settings_gb.Controls.Add(this.jira_username);
            this.jira_settings_gb.Controls.Add(this.jira_user);
            this.jira_settings_gb.Controls.Add(this.jira_server);
            this.jira_settings_gb.Controls.Add(this.jira_host);
            this.jira_settings_gb.Location = new System.Drawing.Point(12, 33);
            this.jira_settings_gb.Name = "jira_settings_gb";
            this.jira_settings_gb.Size = new System.Drawing.Size(266, 100);
            this.jira_settings_gb.TabIndex = 10;
            this.jira_settings_gb.TabStop = false;
            this.jira_settings_gb.Text = "JIRA Server Config";
            // 
            // jira_pass
            // 
            this.jira_pass.AutoSize = true;
            this.jira_pass.Location = new System.Drawing.Point(6, 75);
            this.jira_pass.Name = "jira_pass";
            this.jira_pass.Size = new System.Drawing.Size(53, 13);
            this.jira_pass.TabIndex = 5;
            this.jira_pass.Text = "Password";
            // 
            // jira_password
            // 
            this.jira_password.Location = new System.Drawing.Point(78, 72);
            this.jira_password.Name = "jira_password";
            this.jira_password.Size = new System.Drawing.Size(182, 20);
            this.jira_password.TabIndex = 4;
            this.jira_password.UseSystemPasswordChar = true;
            // 
            // jira_username
            // 
            this.jira_username.Location = new System.Drawing.Point(78, 46);
            this.jira_username.Name = "jira_username";
            this.jira_username.Size = new System.Drawing.Size(182, 20);
            this.jira_username.TabIndex = 3;
            // 
            // jira_user
            // 
            this.jira_user.AutoSize = true;
            this.jira_user.Location = new System.Drawing.Point(6, 49);
            this.jira_user.Name = "jira_user";
            this.jira_user.Size = new System.Drawing.Size(55, 13);
            this.jira_user.TabIndex = 2;
            this.jira_user.Text = "Username";
            // 
            // jira_server
            // 
            this.jira_server.Location = new System.Drawing.Point(78, 20);
            this.jira_server.Name = "jira_server";
            this.jira_server.Size = new System.Drawing.Size(182, 20);
            this.jira_server.TabIndex = 1;
            // 
            // jira_host
            // 
            this.jira_host.AutoSize = true;
            this.jira_host.Location = new System.Drawing.Point(6, 23);
            this.jira_host.Name = "jira_host";
            this.jira_host.Size = new System.Drawing.Size(64, 13);
            this.jira_host.TabIndex = 0;
            this.jira_host.Text = "JIRA Server";
            // 
            // client_select_cb
            // 
            this.client_select_cb.AutoSize = true;
            this.client_select_cb.Checked = true;
            this.client_select_cb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.client_select_cb.Location = new System.Drawing.Point(8, 343);
            this.client_select_cb.Name = "client_select_cb";
            this.client_select_cb.Size = new System.Drawing.Size(103, 17);
            this.client_select_cb.TabIndex = 2;
            this.client_select_cb.Text = "Select all Clients";
            this.client_select_cb.UseVisualStyleBackColor = true;
            this.client_select_cb.CheckedChanged += new System.EventHandler(this.client_select_cb_CheckedChanged);
            // 
            // client_selection_cb
            // 
            this.client_selection_cb.CheckOnClick = true;
            this.client_selection_cb.FormattingEnabled = true;
            this.client_selection_cb.HorizontalScrollbar = true;
            this.client_selection_cb.Location = new System.Drawing.Point(7, 20);
            this.client_selection_cb.Name = "client_selection_cb";
            this.client_selection_cb.Size = new System.Drawing.Size(140, 319);
            this.client_selection_cb.Sorted = true;
            this.client_selection_cb.TabIndex = 0;
            // 
            // date_config_end
            // 
            this.date_config_end.Location = new System.Drawing.Point(69, 46);
            this.date_config_end.Name = "date_config_end";
            this.date_config_end.Size = new System.Drawing.Size(113, 20);
            this.date_config_end.TabIndex = 3;
            // 
            // date_config_start
            // 
            this.date_config_start.Location = new System.Drawing.Point(68, 20);
            this.date_config_start.Name = "date_config_start";
            this.date_config_start.Size = new System.Drawing.Size(113, 20);
            this.date_config_start.TabIndex = 2;
            // 
            // report_out_dir_gb
            // 
            this.report_out_dir_gb.Controls.Add(this.report_out_name_label);
            this.report_out_dir_gb.Controls.Add(this.report_out_dir_label);
            this.report_out_dir_gb.Controls.Add(this.report_out_name_tb);
            this.report_out_dir_gb.Controls.Add(this.report_out_dir_tb);
            this.report_out_dir_gb.Location = new System.Drawing.Point(172, 303);
            this.report_out_dir_gb.Name = "report_out_dir_gb";
            this.report_out_dir_gb.Size = new System.Drawing.Size(300, 81);
            this.report_out_dir_gb.TabIndex = 18;
            this.report_out_dir_gb.TabStop = false;
            this.report_out_dir_gb.Text = "Report Output Directory";
            // 
            // report_out_name_label
            // 
            this.report_out_name_label.AutoSize = true;
            this.report_out_name_label.Location = new System.Drawing.Point(6, 58);
            this.report_out_name_label.Name = "report_out_name_label";
            this.report_out_name_label.Size = new System.Drawing.Size(84, 13);
            this.report_out_name_label.TabIndex = 3;
            this.report_out_name_label.Text = "Report Filename";
            // 
            // report_out_dir_label
            // 
            this.report_out_dir_label.AutoSize = true;
            this.report_out_dir_label.Location = new System.Drawing.Point(6, 26);
            this.report_out_dir_label.Name = "report_out_dir_label";
            this.report_out_dir_label.Size = new System.Drawing.Size(84, 13);
            this.report_out_dir_label.TabIndex = 2;
            this.report_out_dir_label.Text = "Report Directory";
            // 
            // report_out_name_tb
            // 
            this.report_out_name_tb.Location = new System.Drawing.Point(113, 55);
            this.report_out_name_tb.Name = "report_out_name_tb";
            this.report_out_name_tb.Size = new System.Drawing.Size(181, 20);
            this.report_out_name_tb.TabIndex = 1;
            // 
            // report_out_dir_tb
            // 
            this.report_out_dir_tb.Location = new System.Drawing.Point(113, 23);
            this.report_out_dir_tb.Name = "report_out_dir_tb";
            this.report_out_dir_tb.Size = new System.Drawing.Size(181, 20);
            this.report_out_dir_tb.TabIndex = 0;
            // 
            // jira_report_execute
            // 
            this.jira_report_execute.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jira_report_execute.Location = new System.Drawing.Point(334, 390);
            this.jira_report_execute.Name = "jira_report_execute";
            this.jira_report_execute.Size = new System.Drawing.Size(138, 87);
            this.jira_report_execute.TabIndex = 16;
            this.jira_report_execute.Text = "Run Reports";
            this.jira_report_execute.UseVisualStyleBackColor = true;
            this.jira_report_execute.Click += new System.EventHandler(this.jira_report_execute_Click);
            // 
            // apply_markup
            // 
            this.apply_markup.AutoSize = true;
            this.apply_markup.Checked = true;
            this.apply_markup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.apply_markup.Location = new System.Drawing.Point(6, 65);
            this.apply_markup.Name = "apply_markup";
            this.apply_markup.Size = new System.Drawing.Size(91, 17);
            this.apply_markup.TabIndex = 3;
            this.apply_markup.Text = "Apply Markup";
            this.apply_markup.UseVisualStyleBackColor = true;
            // 
            // no_email
            // 
            this.no_email.AutoSize = true;
            this.no_email.Location = new System.Drawing.Point(6, 42);
            this.no_email.Name = "no_email";
            this.no_email.Size = new System.Drawing.Size(99, 17);
            this.no_email.TabIndex = 2;
            this.no_email.Text = "Email no Clients";
            this.no_email.UseVisualStyleBackColor = true;
            this.no_email.CheckedChanged += new System.EventHandler(this.no_email_CheckedChanged);
            // 
            // client_options_gb
            // 
            this.client_options_gb.Controls.Add(this.apply_markup);
            this.client_options_gb.Controls.Add(this.no_email);
            this.client_options_gb.Controls.Add(this.email_selected);
            this.client_options_gb.Location = new System.Drawing.Point(172, 390);
            this.client_options_gb.Name = "client_options_gb";
            this.client_options_gb.Size = new System.Drawing.Size(156, 87);
            this.client_options_gb.TabIndex = 17;
            this.client_options_gb.TabStop = false;
            this.client_options_gb.Text = "Email Options";
            // 
            // email_selected
            // 
            this.email_selected.AutoSize = true;
            this.email_selected.Location = new System.Drawing.Point(6, 19);
            this.email_selected.Name = "email_selected";
            this.email_selected.Size = new System.Drawing.Size(84, 17);
            this.email_selected.TabIndex = 1;
            this.email_selected.Text = "Email Clients";
            this.email_selected.UseVisualStyleBackColor = true;
            this.email_selected.CheckedChanged += new System.EventHandler(this.email_selected_CheckedChanged);
            // 
            // client_gb
            // 
            this.client_gb.Controls.Add(this.client_select_cb);
            this.client_gb.Controls.Add(this.client_selection_cb);
            this.client_gb.Location = new System.Drawing.Point(13, 140);
            this.client_gb.Name = "client_gb";
            this.client_gb.Size = new System.Drawing.Size(153, 366);
            this.client_gb.TabIndex = 13;
            this.client_gb.TabStop = false;
            this.client_gb.Text = "Client Selection";
            // 
            // date_start
            // 
            this.date_start.AutoSize = true;
            this.date_start.Location = new System.Drawing.Point(7, 23);
            this.date_start.Name = "date_start";
            this.date_start.Size = new System.Drawing.Size(55, 13);
            this.date_start.TabIndex = 0;
            this.date_start.Text = "Start Date";
            // 
            // jira_report_menu
            // 
            this.jira_report_menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ms_file,
            this.ms_info});
            this.jira_report_menu.Location = new System.Drawing.Point(0, 0);
            this.jira_report_menu.Name = "jira_report_menu";
            this.jira_report_menu.Size = new System.Drawing.Size(484, 24);
            this.jira_report_menu.TabIndex = 11;
            this.jira_report_menu.Text = "menu_strip";
            // 
            // ms_file
            // 
            this.ms_file.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ms_file_OpenConfig,
            this.ms_file_Exit});
            this.ms_file.Name = "ms_file";
            this.ms_file.Size = new System.Drawing.Size(37, 20);
            this.ms_file.Text = "File";
            // 
            // ms_file_OpenConfig
            // 
            this.ms_file_OpenConfig.Name = "ms_file_OpenConfig";
            this.ms_file_OpenConfig.Size = new System.Drawing.Size(163, 22);
            this.ms_file_OpenConfig.Text = "Open Config File";
            this.ms_file_OpenConfig.Click += new System.EventHandler(this.ms_file_OpenConfig_Click);
            // 
            // ms_file_Exit
            // 
            this.ms_file_Exit.Name = "ms_file_Exit";
            this.ms_file_Exit.Size = new System.Drawing.Size(163, 22);
            this.ms_file_Exit.Text = "Exit";
            this.ms_file_Exit.Click += new System.EventHandler(this.ms_file_Exit_Click);
            // 
            // ms_info
            // 
            this.ms_info.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ms_info_about});
            this.ms_info.Name = "ms_info";
            this.ms_info.Size = new System.Drawing.Size(82, 20);
            this.ms_info.Text = "Information";
            // 
            // ms_info_about
            // 
            this.ms_info_about.Name = "ms_info_about";
            this.ms_info_about.Size = new System.Drawing.Size(107, 22);
            this.ms_info_about.Text = "About";
            this.ms_info_about.Click += new System.EventHandler(this.ms_info_about_Click);
            // 
            // date_config
            // 
            this.date_config.Controls.Add(this.date_config_end);
            this.date_config.Controls.Add(this.date_config_start);
            this.date_config.Controls.Add(this.date_end);
            this.date_config.Controls.Add(this.date_start);
            this.date_config.Location = new System.Drawing.Point(284, 33);
            this.date_config.Name = "date_config";
            this.date_config.Size = new System.Drawing.Size(188, 100);
            this.date_config.TabIndex = 12;
            this.date_config.TabStop = false;
            this.date_config.Text = "Date Config";
            // 
            // date_end
            // 
            this.date_end.AutoSize = true;
            this.date_end.Location = new System.Drawing.Point(6, 49);
            this.date_end.Name = "date_end";
            this.date_end.Size = new System.Drawing.Size(52, 13);
            this.date_end.TabIndex = 1;
            this.date_end.Text = "End Date";
            // 
            // JiraReport_UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 512);
            this.Controls.Add(this.email_config);
            this.Controls.Add(this.jira_report_progress);
            this.Controls.Add(this.jira_settings_gb);
            this.Controls.Add(this.report_out_dir_gb);
            this.Controls.Add(this.jira_report_execute);
            this.Controls.Add(this.client_options_gb);
            this.Controls.Add(this.client_gb);
            this.Controls.Add(this.jira_report_menu);
            this.Controls.Add(this.date_config);
            this.Name = "JiraReport_UI";
            this.Text = "JIRA Customer Report Tool";
            this.email_config.ResumeLayout(false);
            this.email_config.PerformLayout();
            this.jira_settings_gb.ResumeLayout(false);
            this.jira_settings_gb.PerformLayout();
            this.report_out_dir_gb.ResumeLayout(false);
            this.report_out_dir_gb.PerformLayout();
            this.client_options_gb.ResumeLayout(false);
            this.client_options_gb.PerformLayout();
            this.client_gb.ResumeLayout(false);
            this.client_gb.PerformLayout();
            this.jira_report_menu.ResumeLayout(false);
            this.jira_report_menu.PerformLayout();
            this.date_config.ResumeLayout(false);
            this.date_config.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox email_config;
        private System.Windows.Forms.RichTextBox email_body_tb;
        private System.Windows.Forms.TextBox email_sub_tb;
        private System.Windows.Forms.ProgressBar jira_report_progress;
        private System.Windows.Forms.GroupBox jira_settings_gb;
        private System.Windows.Forms.Label jira_pass;
        private System.Windows.Forms.TextBox jira_password;
        private System.Windows.Forms.TextBox jira_username;
        private System.Windows.Forms.Label jira_user;
        private System.Windows.Forms.TextBox jira_server;
        private System.Windows.Forms.Label jira_host;
        private System.Windows.Forms.CheckBox client_select_cb;
        private System.Windows.Forms.CheckedListBox client_selection_cb;
        private System.Windows.Forms.DateTimePicker date_config_end;
        private System.Windows.Forms.DateTimePicker date_config_start;
        private System.Windows.Forms.GroupBox report_out_dir_gb;
        private System.Windows.Forms.TextBox report_out_dir_tb;
        private System.Windows.Forms.Button jira_report_execute;
        private System.Windows.Forms.CheckBox apply_markup;
        private System.Windows.Forms.RadioButton no_email;
        private System.Windows.Forms.GroupBox client_options_gb;
        private System.Windows.Forms.RadioButton email_selected;
        private System.Windows.Forms.GroupBox client_gb;
        private System.Windows.Forms.Label date_start;
        private System.Windows.Forms.MenuStrip jira_report_menu;
        private System.Windows.Forms.ToolStripMenuItem ms_file;
        private System.Windows.Forms.ToolStripMenuItem ms_file_OpenConfig;
        private System.Windows.Forms.ToolStripMenuItem ms_file_Exit;
        private System.Windows.Forms.ToolStripMenuItem ms_info;
        private System.Windows.Forms.ToolStripMenuItem ms_info_about;
        private System.Windows.Forms.GroupBox date_config;
        private System.Windows.Forms.Label date_end;
        private System.Windows.Forms.OpenFileDialog open_config_dialog;
        private System.Windows.Forms.TextBox report_out_name_tb;
        private System.Windows.Forms.Label report_out_name_label;
        private System.Windows.Forms.Label report_out_dir_label;

    }
}

