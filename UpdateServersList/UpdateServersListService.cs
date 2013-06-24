using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Configuration;
using Oracle.DataAccess.Client;
using System.Collections;

namespace UpdateServersList
{
    public partial class UpdateServersListService : ServiceBase
    {
        public List<VersionType> Servers = new List<VersionType>();
        public List<VersionType> Databases = new List<VersionType>();
        public string[] _remotes;
        public string[] _databases;
        public string _output;
        public int _interval;
        public bool _standalone;
        private string _user;
        private string _password;
        public UpdateServersListService()
        {
            InitializeComponent();
            GetConfig();
            this.timer1.Interval = (_interval);
            GenerateHTML();
            return;
        }
        private void GetConfig()
        {
            try
            {
                _remotes = ConfigurationManager.AppSettings["Servers"].Split(new string[1] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                _databases = ConfigurationManager.AppSettings["Databases"].Split(new string[1] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i <= _remotes.GetUpperBound(0); i++) _remotes[i] = _remotes[i].Replace("\r\n", string.Empty).Trim();
                for (int i = 0; i <= _databases.GetUpperBound(0); i++) _databases[i] = _databases[i].Replace("\r\n", string.Empty).Trim();
                Array.Sort<string>(_remotes);
                Array.Sort<string>(_databases);
                _output = ConfigurationManager.AppSettings["Output"];
                _interval = int.Parse(ConfigurationManager.AppSettings["Interval"]) * 1000;
                _standalone = (ConfigurationManager.AppSettings["Standalone"] == "true");
                _user = ConfigurationManager.AppSettings["User"];
                _password = ConfigurationManager.AppSettings["Password"];
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(string.Format("Malformed Configuration File:\r\n{0}", ex.Message));
            }
        }

        //private void CreateXMLConfig()
        //{
        //    try
        //    {
        //        ConfigXmlDocument xmlDoc = new ConfigXmlDocument();
        //        System.Xml.XmlNode root;
        //        root."test";

        //        xmlDoc.AppendChild(root);
        //        xmlDoc.Save("C:\test.xml");
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Windows.Forms.MessageBox.Show(ex.ToString());
        //    }
        //}

        //private void SaveVersionsToConfig()
        //{
        //    //Need to handle security exceptions correctly
        //    string key;
        //    string value;

            
        //    try
        //    {
        //        foreach (VersionType d in Databases)
        //        {
        //            if (d._version != string.Empty && d._compile != string.Empty)
        //            {
        //                key = string.Format("Database:{0}", d._name);
        //                value = string.Format("{0};{1}", d._version, d._compile);
        //                if (ConfigurationManager.AppSettings[key] == null)
        //                    ConfigurationManager.AppSettings.Add(key, value);
        //                else
        //                    ConfigurationManager.AppSettings.Set(key, value);
        //            }
        //        }
        //        foreach (VersionType s in Servers)
        //        {
        //            if (s._version != string.Empty && s._compile != string.Empty)
        //            {
        //                key = string.Format("Server:{0}", s._name);
        //                value = string.Format("{0};{1}", s._version, s._compile);
        //                if (ConfigurationManager.AppSettings[key] == null)
        //                    ConfigurationManager.AppSettings.Add(key, value);
        //                else
        //                    ConfigurationManager.AppSettings.Set(key, value);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        System.IO.File.WriteAllText("ConfigError.txt", ex.ToString());
        //    }
        //}
        //private void ReadVersionsFromConfig()
        //{
        //    foreach (var item in ConfigurationManager.AppSettings.Keys)
        //    {
                
        //    }
        //}

        protected override void OnStart(string[] args)
        {
            timer1.Start();
        }
        protected override void OnStop()
        {
            timer1.Stop();
        }
        private void ReadRemoteRegistryKey(string remoteName)
        {
            RegistryKey rk = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, remoteName).OpenSubKey("Brady").OpenSubKey("Trinity").OpenSubKey("System");
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            GenerateHTML();
        }
        private void GenerateHTML()
        {
            try
            {
                //CreateXMLConfig();

                string serverOutput = string.Empty;
                string databaseOutput = string.Empty;
                string output = string.Empty;
                string html;
                const string fileHeader =
                    "<html xmlns=\"http://www.w3.org/TR/REC-html40\">\r\n" +
                    "<head>\r\n" +
                    "<meta http-equiv=Content-Type content=\"text/html; charset=windows-1252\">\r\n" +
                    "</head>\r\n" +
                    "<body lang=EN-GB style='tab-interval:36.0pt'>\r\n";
                const string fileFooter =
                    "\r\n</body>\r\n</html>";
                const string serverTableHeader =
                    "<p style='font-weight: bold; font-family: sans-serif; font-size: x-large'>Servers</p>\r\n" +
                    "<table border=1 cellspacing=0 cellpadding=2 style='font-family: sans-serif; font-size: small'>\r\n" +
                    " <tr style='font-weight: bold; text-align:center'>\r\n" +
                    "  <td><p>Server</p></td>\r\n" +
                    "  <td><p>Version</p></td>\r\n" +
                    "  <td><p>Compile</p></td>\r\n" +
                    "  <td><p>Databases</p></td>\r\n" +
                    "  <td><p>Error</p></td>\r\n" +
                    " </tr>\r\n";
                const string databaseTableHeader =
                    "<p style='font-weight: bold; font-family: sans-serif; font-size: x-large'>Databases</p>\r\n" +
                    "<table border=1 cellspacing=0 cellpadding=2 style='font-family: sans-serif; font-size: small'>\r\n" +
                    " <tr style='font-weight: bold; text-align:center'>\r\n" +
                    "  <td><p>Database</p></td>\r\n" +
                    "  <td><p>Version</p></td>\r\n" +
                    "  <td><p>Compile</p></td>\r\n" +
                    "  <td><p>Servers</p></td>\r\n" +
                    "  <td><p>Error</p></td>\r\n" +
                    " </tr>\r\n";
                const string tableFooter =
                    "</table>\r\n";
                UpdateServers();
                UpdateDatabases();
                //SaveVersionsToConfig();
                //ReadVersionsFromConfig();
                MatchVersions(ref serverOutput, ref databaseOutput);
                html = fileHeader + serverTableHeader + serverOutput + tableFooter + databaseTableHeader + databaseOutput + tableFooter + fileFooter;
                System.IO.File.AppendAllText(_output, html);
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText("UpdateTSListFatality.log", ex.Message.ToString() + "\r\n" + ex.StackTrace.ToString());
            }
        }
        private void UpdateServers()
        {
            if (System.IO.File.Exists(_output)) System.IO.File.Delete(_output);
            for (int i = 0; i < _remotes.GetUpperBound(0); i++)
            {
                ReadRemoteReg(_remotes[i]);
            }
        }
        private void UpdateDatabases()
        {
            for (int i = 0; i < _databases.GetUpperBound(0); i++)
            {
                ReadOracleVersion(_databases[i]);
            }
        }
        private void ReadOracleVersion(string dsn)
        {
            string version, compile;
            try
            {
                OracleConnectionStringBuilder csb = new OracleConnectionStringBuilder();
                csb.DataSource = dsn;
                csb.UserID = _user;
                csb.Password = _password;
                OracleConnection conn = new OracleConnection(csb.ConnectionString);
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT build,svcpack,compile FROM sysadm.vversion";
                OracleDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (rdr.Read())
                {
                    version = rdr.GetString(rdr.GetOrdinal("BUILD")) + "." + rdr.GetString(rdr.GetOrdinal("SVCPACK"));
                    compile = rdr.GetString(rdr.GetOrdinal("COMPILE")).ToString();
                }
                else
                {
                    version = "-";
                    compile = "-";
                }
                rdr.Close();
                rdr.Dispose();
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
                Databases.Add(new VersionType(dsn, version, compile, string.Empty));
            }
            catch (Exception ex)
            {
                Databases.Add(new VersionType(dsn, string.Empty, string.Empty, ex.Message.Replace("\r\n", string.Empty)));

            }
        }
        private void ReadRemoteReg(string remoteName)
        {
            string build, release, compile, trinver;
            try
            {
                GetRegistryValues(remoteName, out build, out release, out compile);
                trinver = string.Format("{0}.{1}", build, release);
                Servers.Add(new VersionType(remoteName, trinver, compile, string.Empty));
            }
            catch (Exception ex)
            {
                Servers.Add(new VersionType(remoteName, string.Empty, string.Empty, ex.Message.Replace("\r\n", string.Empty)));
            }
        }
        static void AddTableRow(ref string output, string id, string version, string compile, string items, string errors)
        {
            if (version == string.Empty) version = " ";
            if (compile == string.Empty) compile = " ";
            if (items == string.Empty) items = " ";
            if (errors == string.Empty) errors = " ";
            output += string.Format("\r\n <tr font-size: x-small>\r\n  <td><p>{0}</p></td>\r\n  <td><p>{1}</p></td>\r\n  <td><p>{2}</p></td>\r\n  <td><p>{3}</p></td>\r\n <td><p style=\"color: red\">{4}</p></td>\r\n </tr>", id, version, compile, items, errors);
            Debug.Print(string.Format("ID : {0}\tVersion : {1}\tCompile : {2}", id, version, compile));
        }
        private static void GetRegistryValues(string remoteName, out string build, out string release, out string compile)
        {
            RegistryKey rk = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, remoteName).OpenSubKey("SOFTWARE\\Brady\\Trinity\\System");
            build = rk.GetValue("Build").ToString();
            release = rk.GetValue("Service Pack").ToString();
            compile = rk.GetValue("Compile").ToString();
        }

        private void MatchVersions(ref string serverOutput, ref string databaseOutput)
        {
            foreach (VersionType d in Databases)
            {
                foreach (VersionType s in Servers)
                {
                    if (s._version != string.Empty && s._version == d._version && s._compile == d._compile)
                    {
                        s.addItem(d._name);
                        d.addItem(s._name);
                    }
                }
            }
            foreach (VersionType d in Databases)
            {
                AddTableRow(ref databaseOutput, d._name, d._version, d._compile, d._items(), d._errors);
            }
            foreach (VersionType v in Servers)
            {
                AddTableRow(ref serverOutput, v._name, v._version, v._compile, v._items(), v._errors);
            }
        }
    }
}
