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
using System.Web.UI.WebControls;
using System.IO;


namespace UpdateServersList
{
    public partial class UpdateServersListService :ServiceBase
    {
        public List<VersionType> Servers = new List<VersionType>();
        public List<VersionType> Databases = new List<VersionType>();
        public List<VersionType> previousServers = new List<VersionType>();
        public List<VersionType> previousDatabases = new List<VersionType>();
        public List<ServerType> Remotes = new List<ServerType>();
        public string[] _demoservers;
        public string[] _developmentservers;
        public string[] _clientservers;
        public string[] _databases;
        public string _ServerTypeTableDemo;
        public string _ServerTypeTableDevelopment;
        public string _ServerTypeTableClient;
        public string _output;
        public int _interval;
        public bool _standalone;
        private string _user;
        private string _password;
        public UpdateServersListService()
        {
            InitializeComponent();
            GetConfig();
            ReadHTML();
            GenerateHTML();
            return;
        }
        private void GetConfig()
        {
            try
            {
                _demoservers = ConfigurationManager.AppSettings["DemoServer"].Split(new string[1] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                _developmentservers = ConfigurationManager.AppSettings["DevelopmentServer"].Split(new string[1] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                _clientservers = ConfigurationManager.AppSettings["ClientServer"].Split(new string[1] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                
                _databases = ConfigurationManager.AppSettings["Databases"].Split(new string[1] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                _ServerTypeTableDemo = ConfigurationManager.AppSettings["ServerTypeDemo"];
                _ServerTypeTableDevelopment = ConfigurationManager.AppSettings["ServerTypeDevelopment"];
                _ServerTypeTableClient = ConfigurationManager.AppSettings["ServerTypeClient"];
                
                for (int i = 0; i <= _demoservers.GetUpperBound(0); i++) _demoservers[i] = _demoservers[i].Replace("\r\n", string.Empty).Trim().ToUpper();
                Array.Sort<string>(_demoservers);
                for (int i = 0; i <= _demoservers.GetUpperBound(0); i++) Remotes.Add(new ServerType(_demoservers[i],"demo"));
                
                for (int i = 0; i <= _developmentservers.GetUpperBound(0); i++) _developmentservers[i] = _developmentservers[i].Replace("\r\n", string.Empty).Trim().ToUpper();
                Array.Sort<string>(_developmentservers);
                for (int i = 0; i <= _developmentservers.GetUpperBound(0); i++) Remotes.Add(new ServerType(_developmentservers[i],"development"));

                for (int i = 0; i <= _clientservers.GetUpperBound(0); i++)_clientservers[i] = _clientservers[i].Replace("\r\n", string.Empty).Trim().ToUpper();
                Array.Sort<string>(_clientservers);
                for (int i = 0; i <= _clientservers.GetUpperBound(0); i++) Remotes.Add(new ServerType(_clientservers[i],"client"));

                for (int i = 0; i <= _databases.GetUpperBound(0); i++) _databases[i] = _databases[i].Replace("\r\n", string.Empty).Trim().ToUpper();
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

      
        private void ReadRemoteRegistryKey(string remoteName)
        {
            RegistryKey rk = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, remoteName).OpenSubKey("Brady").OpenSubKey("Trinity").OpenSubKey("System");
        }
       
       
        private void ReadHTML()
        {
            try
            {
                int pointer, first, last;
                string input = _output;
                string previousid, previousversion, previouscompile, previousitems, previousoracleversion, previouswindowsversion, previousarchitecture, previousprocessorid, previouserrors, previouscomments, previoushtml = null;
                using (StreamReader sr = new StreamReader(input))
                    previoushtml = sr.ReadToEnd().ToString();

                pointer = previoushtml.IndexOf("Server starts here");
                for (; ; )
                {
                    pointer = previoushtml.IndexOf("<td>", pointer);
                    first = pointer + 7;
                    last = previoushtml.IndexOf("</p></td>", pointer);
                    previousid = previoushtml.Substring(first, last - first);
                    if (previousid == "-") previousid = string.Empty;

                    pointer = pointer + 3;

                    pointer = previoushtml.IndexOf("<td>", pointer);
                    first = pointer + 7;
                    last = previoushtml.IndexOf("</p></td>", pointer);
                    previousversion = previoushtml.Substring(first, last - first);
                    if (previousversion == "-") previousversion = string.Empty;

                    pointer = pointer + 3;

                    pointer = previoushtml.IndexOf("<td>", pointer);
                    first = pointer + 7;
                    last = previoushtml.IndexOf("</p></td>", pointer);
                    previouscompile = previoushtml.Substring(first, last - first);
                    if (previouscompile == "-") previouscompile = string.Empty;


                   pointer = pointer + 3;

                   pointer = previoushtml.IndexOf("<td>", pointer);
                   first = pointer + 7;
                   last = previoushtml.IndexOf("</p></td>", pointer);
                   previousoracleversion = previoushtml.Substring(first, last - first);
                   if (previousoracleversion == "-") previousoracleversion = string.Empty;

                   pointer = pointer + 3;

                   pointer = previoushtml.IndexOf("<td>", pointer);
                   first = pointer + 7;
                   last = previoushtml.IndexOf("</p></td>", pointer);
                   previouswindowsversion = previoushtml.Substring(first, last - first);
                   if (previouswindowsversion == "-") previouswindowsversion = string.Empty;

                   pointer = pointer + 3;

                  
                   pointer = previoushtml.IndexOf("<td>", pointer);
                   first = pointer + 7;
                   last = previoushtml.IndexOf("</p></td>", pointer);
                   previousarchitecture = previoushtml.Substring(first, last - first);
                   if (previousarchitecture == "-") previousarchitecture = string.Empty;

                   pointer = pointer + 3;

                   pointer = previoushtml.IndexOf("<td>", pointer);
                   first = pointer + 7;
                   last = previoushtml.IndexOf("</p></td>", pointer);
                  previousprocessorid = previoushtml.Substring(first, last - first);
                  if (previousprocessorid == "-") previousprocessorid = string.Empty;

                   pointer = pointer + 3;

                    pointer = previoushtml.IndexOf("<td>", pointer);
                    first = pointer + 7;
                    last = previoushtml.IndexOf("</p></td>", pointer);
                    previousitems = previoushtml.Substring(first, last - first);
                    if (previousitems == "-") previousitems = string.Empty;

                    pointer = pointer + 3;

                    pointer = previoushtml.IndexOf("<td>", pointer);
                    first = pointer + 7;
                    last = previoushtml.IndexOf("</p></td>", pointer);
                    previouserrors = previoushtml.Substring(first, last - first);
                    if (previouserrors == "-") previouserrors = string.Empty;
                    if (previouserrors == "<img src = 'tick.png' alt = 'On' width = '20' height = '20'><span style = display:none>On<span style = display:inline>") previouserrors = " On ";
                    if (previouserrors == "<img src = 'tick.png' alt = 'On' width = '20' height = '20'><span style = display:none>Off<span style = display:inline>") previouserrors = " Off ";
                    pointer = pointer + 3;

                    pointer = previoushtml.IndexOf("<td>", pointer);
                    first = pointer + 7;
                    last = previoushtml.IndexOf("</p></td>", pointer);
                    previouscomments = previoushtml.Substring(first, last - first);
                    if (previouscomments == "-") previouscomments = string.Empty;

                    previousServers.Add(new VersionType(previousid, previousversion, previouscompile, previousoracleversion, previouswindowsversion, previousarchitecture, previousprocessorid, previouserrors, previouscomments, string.Empty));

                    pointer = previoushtml.IndexOf("</tr>", pointer);
                    Debug.Print(string.Format("Previous: ID : {0}\tVersion : {1}\tCompile : {2}", previousid, previousversion, previouscompile));
                    if (previoushtml.IndexOf("</table>",pointer) == pointer + 5) break;
                }

                pointer = previoushtml.IndexOf("<!Database starts here>");

                for (; ; )
                {
                    pointer = previoushtml.IndexOf("<td>", pointer);
                    first = pointer + 7;
                    last = previoushtml.IndexOf("</p></td>", pointer);
                    previousid = previoushtml.Substring(first, last - first);
                    if (previousid == "-") previousid = string.Empty;

                    pointer = pointer + 3;

                    pointer = previoushtml.IndexOf("<td>", pointer);
                    first = pointer + 7;
                    last = previoushtml.IndexOf("</p></td>", pointer);
                    previousversion = previoushtml.Substring(first, last - first);
                    if (previousversion == "-") previousversion = string.Empty;

                    pointer = pointer + 3;

                    pointer = previoushtml.IndexOf("<td>", pointer);
                    first = pointer + 7;
                    last = previoushtml.IndexOf("</p></td>", pointer);
                    previouscompile = previoushtml.Substring(first, last - first);
                    if (previouscompile == "-") previouscompile = string.Empty;

                    pointer = pointer + 3;

                    pointer = previoushtml.IndexOf("<td>", pointer);
                    first = pointer + 7;
                    last = previoushtml.IndexOf("</p></td>", pointer);
                    previousoracleversion = previoushtml.Substring(first, last - first);
                    if (previousoracleversion == "-") previousoracleversion = string.Empty;

                    pointer = pointer + 3;

                    pointer = previoushtml.IndexOf("<td>", pointer);
                    first = pointer + 7;
                    last = previoushtml.IndexOf("</p></td>", pointer);
                    previousitems = previoushtml.Substring(first, last - first);
                    if (previousitems == "-") previousitems = string.Empty;

                    pointer = pointer + 3;

                    pointer = previoushtml.IndexOf("<td>", pointer);
                    first = pointer + 7;
                    last = previoushtml.IndexOf("</p></td>", pointer);
                    previouserrors = previoushtml.Substring(first, last - first);
                    if (previouserrors == "<img src = 'tick.png' alt = 'On' width = '20' height = '20'><span style = display:none>On<span style = display:inline>") previouserrors = " On ";
                    if (previouserrors == "<img src = 'cross.png' alt = 'Off' width = '20' height = '20'><span style = display:none>Off<span style = display:inline>") previouserrors = " Off ";
                    if (previouserrors == "-") previouserrors = string.Empty;

                    previousDatabases.Add(new VersionType(previousid, previousversion, previouscompile, previousoracleversion,string.Empty,string.Empty,string.Empty, previouserrors, string.Empty, string.Empty));

                    pointer = previoushtml.IndexOf("</tr>", pointer);
                    Debug.Print(string.Format("Previous: ID : {0}\tVersion : {1}\tCompile : {2}", previousid, previousversion, previouscompile));
                    if (previoushtml.IndexOf("</table>", pointer) == pointer + 5) break;

                }
            }
            catch (Exception e)
            {
                System.IO.File.WriteAllText("ReadingPreviousHtmlFatality.log", e.Message.ToString() + "\r\n" + e.StackTrace.ToString());
            }

        }
       
        private void GenerateHTML()
        {
            try
            {
                string serverOutput = string.Empty;
                string databaseOutput = string.Empty;
                string output = string.Empty;
                string html;
                string lastUpdated = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                const string fileHeader =
                    "<html xmlns=\"http://www.w3.org/TR/REC-html40\">\r\n" +
                    "<head>\r\n" +
                    "<meta http-equiv=Content-Type content=\"text/html; charset=windows-1252\">\r\n" +
                    "<script src=\"sorttable.js\"></script>"+
                    "<title>Current Trinity Terminal Servers</title>" +
                    "</head>\r\n" +
                    "<body lang=EN-GB style='tab-interval:36.0pt'>\r\n";
                const string fileFooter =
                    "\r\n</body>\r\n</html>";
                string serverTypeTable = 
                    "<table border = 1 bordercolor = lightgray cellspacing = 0 cellpadding = 5 style = 'font-family: sans-serif;font-size: small; font-weight: bold; background-color:Gainsboro'>\r\n" +
                    "<tr>" +
                    "<td><p>For</p></td>\r\n" +
                    "<td><p>Upgrades</p></td>\r\n" +
                    "</tr>" +
                    "<tr style = 'font-weight: normal; background-color:#FFD'>\r\n" +
                    "<td><p>Demo</p></td>\r\n" +
                    "<td><p>" + _ServerTypeTableDemo + "</p></td>\r\n" +
                    "</tr>" +
                    "<tr style = 'font-weight: normal; background-color:#E0F0FF'>\r\n" +
                    "<td><p>Development</p></td>\r\n" +
                    "<td><p>" + _ServerTypeTableDevelopment + "</p></td>\r\n" +
                    "</tr>" +
                    "<tr style = 'font-weight: normal; background-color:#FFE7E7'>\r\n" +
                    "<td><p>Client Services</p></td>\r\n" +
                    "<td><p>" + _ServerTypeTableClient + "</p></td>\r\n" +
                    "</tr>" +
                    "</table>";
                string serverTableHeader =
                    "<table>"+
                    "<tr>" +                    
                    "<td><img src = 'BradyLogo.jpg' height  = '55' width = '220' />" +
                    "<td style='font-weight: bold; font-family: sans-serif; font-size: xx-large'> &nbsp Current Trinity Terminal Servers" +
                    "</table>" +
                    "<table width = '100%'>" +
                    "<td align='left' style='font-weight: bold; font-family: sans-serif; font-size: x-large'>Servers</td>" +
                    "<td align='right' style='font-family: sans-serif; font-size: medium; font-weight: bold'> Last Updated: <span style='font-weight:normal'>" + lastUpdated + "</td>"+
                    "</tr>" +
                    "</table>\r\n"+
                    serverTypeTable+
                    "<p></p>"+
                    "<table class = \"sortable\" border=1 bordercolor = gray cellspacing=0 cellpadding=2 style='font-family: sans-serif; font-size: small'>\r\n" +
                    " <tr style='font-weight: bold; text-align:center;background-color:Gainsboro' >\r\n" +
                    "  <td><p>Server</p></td>\r\n" +
                    "  <td><p>Version</p></td>\r\n" +
                    "  <td><p>Compile</p></td>\r\n" +
                    "  <td><p>ODP.NET Version</p></td>\r\n" +
                    "  <td><p>Windows Version</p></td>\r\n" +
                    "  <td><p>Architecture</p></td>\r\n" +
                    "  <td><p>Processor ID</p></td>\r\n" +
                    "  <td><p>Databases</p></td>\r\n" +
                    "  <td><p>On</p></td>\r\n" +
                    "  <td><p>Comments</p></td>\r\n"+
                    "  </tr>\r\n";
                const string databaseTableHeader =
                    "<p></p>"+
                    "<table width='100%'>" +
                    "<tr>" +
                    "<td align='left'><span style='font-weight: bold; font-family: sans-serif; font-size: x-large'>Databases</td>" +
                    "</tr>" +
                    "</table>\r\n" +
                    "<table class = \"sortable\" border=1 bordercolor=gray cellspacing=0 cellpadding=2 style='font-family: sans-serif; font-size: small'>\r\n" +
                    " <tr style='font-weight: bold; text-align:center;background-color:Gainsboro'>\r\n" +
                    "  <td><p>Database</p></td>\r\n" +
                    "  <td><p>Version</p></td>\r\n" +
                    "  <td><p>Compile</p></td>\r\n" +
                    "  <td><p>Oracle Version</p></td>\r\n" +
                    "  <td><p>Servers</p></td>\r\n" +
                    "  <td><p>On</p></td>\r\n" +
                    " </tr>\r\n";
                const string tableFooter =
                    "</table>\r\n";
                UpdateServers();
                UpdateDatabases();
                MatchVersions(ref serverOutput, ref databaseOutput);
                html = fileHeader  + serverTableHeader  + "<!Server starts here>" +serverOutput + tableFooter + databaseTableHeader + "<!Database starts here>" + databaseOutput + tableFooter + fileFooter;
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
            foreach (ServerType server in Remotes)
            {
                ReadRemoteReg(server);
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
            string version, compile, oracleversion, error;
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
                cmd.CommandText = "SELECT MAX(v.version) version FROM   sys.product_component_version v WHERE  upper(v.product) LIKE '%ORACLE%'";
                OracleDataReader rdr2 = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (rdr2.Read())
                {
                    oracleversion = rdr2.GetString(rdr2.GetOrdinal("VERSION"));
                }
                else
                {
                    oracleversion = "-";
                }
                
                rdr.Close();
                rdr.Dispose();
                rdr2.Close();
                rdr2.Dispose();
                cmd.Dispose();
                conn.Close();
                conn.Dispose();

                error = " On ";


                Databases.Add(new VersionType(dsn, version, compile, oracleversion, string.Empty, string.Empty,string.Empty,error, string.Empty, string.Empty));
            }
            catch (OracleException ex)
            {
                var ver = from d in previousDatabases
                          where d._name == dsn
                          select d._version;

                version = String.Join("", ver);

                var comp = from d in previousDatabases
                           where d._name == dsn
                           select d._compile;

                compile = String.Join("", comp);

                var ora = from d in previousDatabases
                          where d._name == dsn
                          select d._oracleversion;

                oracleversion = String.Join("", ora);


                if (ex.Number == 12514)
                {
                    error = " Off ";
                }
                else
                {
                    error = ex.Message.Replace("\r\n", string.Empty);
                }


                Databases.Add(new VersionType(dsn, version, compile, oracleversion, string.Empty, string.Empty, string.Empty,error, string.Empty, string.Empty));

            }
        }
        private void ReadRemoteReg(ServerType server)
        {
            string build, release, compile, trinver, oracleversion, windowsversion, architecture, processorid, error, comments;
            comments = string.Empty;
            try
            {
                GetRegistryValues(server._name, out build, out release, out compile, out oracleversion, out windowsversion, out architecture, out processorid);
                trinver = string.Format("{0}.{1}", build, release);
                if (System.IO.File.Exists("\\\\" + server._name + "\\Startup\\README.TXT"))
                {
                    var sr = System.IO.File.OpenText("\\\\" + server._name + "\\Startup\\README.TXT");
                    comments = sr.ReadToEnd();
                }
                else comments = "-";

               error = " On ";
           
                Servers.Add(new VersionType(server._name, trinver, compile, oracleversion, windowsversion, architecture, processorid,error ,comments, server._type));
            }
            catch (Exception ex)
            {
                var ver = from d in previousServers
                          where d._name == server._name
                          select d._version;

                trinver = String.Join("", ver);

                var comp = from d in previousServers
                           where d._name == server._name
                           select d._compile;

                compile = String.Join("", comp);

                var ora = from d in previousServers
                          where d._name == server._name
                          select d._oracleversion;

                oracleversion = String.Join("", ora);

                var wind = from d in previousServers
                          where d._name == server._name
                          select d._windowsversion;
                
                windowsversion = String.Join("", wind);

                
                var arch = from d in previousServers
                          where d._name == server._name
                          select d._architecture;

                architecture = String.Join("", arch);

                var proid = from d in previousServers
                          where d._name == server._name
                          select d._processorid;

                processorid = String.Join("", proid);



                if (ex.Message == "Requested registry access is not allowed.")
                {
                    error = " On ";
                }
                else
                {
                    error = " Off ";
                }


                var comm = from d in previousServers
                           where d._name == server._name
                           select d._comments;

                comments = String.Join("", comm);


                Servers.Add(new VersionType(server._name, trinver, compile, oracleversion, windowsversion, architecture, processorid,error, comments, server._type));
            }
        }
        static void AddServerTableRow(ref string output, string id, string version, string compile, string oracleversion, string windowsversion,string architecture, string processorid,string items,  string errors, string comments, string type)
        {
                      
            if (version == string.Empty | version == null) version = "-";
            if (compile == string.Empty| compile == null) compile = "-";
            if (items == string.Empty | items == null) items = "-";
            if (oracleversion == string.Empty | oracleversion == null) oracleversion = "-";
            if (windowsversion == string.Empty | windowsversion == null) windowsversion = "-";
            if (architecture == string.Empty | architecture == null) architecture = "-";
            if (processorid == string.Empty | processorid == null) processorid = "-";
            if (errors == string.Empty | errors == null) errors = "-";
            if (errors == " On ") errors = "<img src = 'tick.png' alt = 'On' width = '20' height = '20'><span style = display:none>On<span style = display:inline>";
            if (errors == " Off ") errors = "<img src = 'cross.png' alt = 'Off' width = '20' height = '20'><span style = 'display:none'>Off<span style = 'display:inline'>";
            if (comments == string.Empty | comments == null) comments = "-";

            if (type == "demo")
            {
                output += string.Format("\r\n <tr style =background-color:#FFD>\r\n  <td><p>{0}</p></td>\r\n  <td><p>{1}</p></td>\r\n  <td><p>{2}</p></td>\r\n  <td><p>{3}</p></td>\r\n <td><p>{4}</p></td>\r\n <td><p>{5}</p></td>\r\n <td><p>{6}</p></td>\r\n <td><p>{7}</p></td>\r\n  <td><p>{8}</p></td>\r\n  <td><p>{9}</p></td>\r\n</tr>", id, version, compile, oracleversion, windowsversion, architecture, processorid, items, errors, comments);
            }
            else if (type == "development")
            {
                output += string.Format("\r\n <tr style =background-color:#E0F0FF>\r\n  <td><p>{0}</p></td>\r\n  <td><p>{1}</p></td>\r\n  <td><p>{2}</p></td>\r\n  <td><p>{3}</p></td>\r\n <td><p>{4}</p></td>\r\n <td><p>{5}</p></td>\r\n <td><p>{6}</p></td>\r\n <td><p>{7}</p></td>\r\n  <td><p>{8}</p></td>\r\n  <td><p>{9}</p></td>\r\n</tr>", id, version, compile, oracleversion, windowsversion, architecture, processorid, items, errors, comments);
            }
            else if (type == "client")
            {
                output += string.Format("\r\n <tr style =background-color:#FFE7E7>\r\n  <td><p>{0}</p></td>\r\n  <td><p>{1}</p></td>\r\n  <td><p>{2}</p></td>\r\n  <td><p>{3}</p></td>\r\n <td><p>{4}</p></td>\r\n <td><p>{5}</p></td>\r\n <td><p>{6}</p></td>\r\n <td><p>{7}</p></td>\r\n  <td><p>{8}</p></td>\r\n  <td><p>{9}</p></td>\r\n</tr>", id, version, compile, oracleversion, windowsversion, architecture, processorid, items, errors, comments);

            }
            else
            {
                output += string.Format("\r\n <tr style=font-size: x-small>\r\n  <td><p>{0}</p></td>\r\n  <td><p>{1}</p></td>\r\n  <td><p>{2}</p></td>\r\n  <td><p>{3}</p></td>\r\n <td><p>{4}</p></td>\r\n <td><p>{5}</p></td>\r\n <td><p>{6}</p></td>\r\n <td><p>{7}</p></td>\r\n  <td><p>{8}</p></td>\r\n  <td><p>{9}</p></td>\r\n</tr>", id, version, compile, oracleversion, windowsversion, architecture, processorid, items, errors, comments);
            }
            
           Debug.Print(string.Format("ID : {0}\tVersion : {1}\tCompile : {2}", id, version, compile));
        }
        static void AddDatabaseTableRow(ref string output, string id, string version, string compile, string oracleversion, string items,string errors)
        {
            if (version == string.Empty | version == null) version = "-";
            if (compile == string.Empty | compile == null) compile = "-";
            if (items == string.Empty | items == null) items = "-";
            if (oracleversion == string.Empty | oracleversion == null) oracleversion = "-";
            if (errors == string.Empty | errors == null)
            {
                errors = "-";
            }
            else if (errors == " On ")
            {
                errors = "<img src = 'tick.png' alt = 'On' width = '20' height = '20'><span style = 'display:none'>On<span style = 'display:inline'>";
            }else if (errors == " Off ") 
            {
                errors = "<img src = 'cross.png' alt = 'Off' width = '20' height = '20'><span style = 'display:none'>Off<span style = 'display:inline'>";
            }

            output += string.Format("\r\n <tr font-size: x-small>\r\n  <td><p>{0}</p></td>\r\n  <td><p>{1}</p></td>\r\n  <td><p>{2}</p></td>\r\n  <td><p>{3}</p></td>\r\n <td><p>{4}</p></td>\r\n <td><p>{5}</p></td>\r\n </tr>",id, version, compile, oracleversion,items,errors);
          Debug.Print(string.Format("ID : {0}\tVersion : {1}\tCompile : {2}", id, version, compile));
        }
        private static void GetRegistryValues(string remoteName, out string build, out string release, out string compile, out string OracleVersion, out string WindowsVersion, out string architecture, out string processorid )
        {
           
            RegistryKey rkTrinVer = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, remoteName).OpenSubKey("SOFTWARE\\Brady\\Trinity\\System");
            RegistryKey rkODPVer = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, remoteName).OpenSubKey("SOFTWARE\\ORACLE\\ODP.NET");
            RegistryKey rkWindVer = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, remoteName).OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion");
            RegistryKey rkProcId = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, remoteName).OpenSubKey("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0");
            RegistryKey rkArch = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, remoteName).OpenSubKey("System\\CurrentControlSet\\Control\\Session Manager\\Environment");
            
            string[] odpVersions = rkODPVer.GetSubKeyNames();
            OracleVersion = odpVersions.Max();

            WindowsVersion = rkWindVer.GetValue("ProductName").ToString().Replace("(R)","");
            
            build = rkTrinVer.GetValue("Build").ToString();

            architecture = rkArch.GetValue("PROCESSOR_ARCHITECTURE").ToString();

            processorid = rkProcId.GetValue("ProcessorNameString").ToString().Replace("(R)", "");
            
            release = rkTrinVer.GetValue("Service Pack").ToString();
            
            compile = rkTrinVer.GetValue("Compile").ToString();



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
                AddDatabaseTableRow(ref databaseOutput, d._name, d._version, d._compile, d._oracleversion, d._items(), d._errors);
            }
            foreach (VersionType v in Servers)
            {
                AddServerTableRow(ref serverOutput, v._name, v._version, v._compile, v._oracleversion, v._windowsversion, v._architecture,v._processorid, v._items(), v._errors, v._comments, v._type);
            }
        }
    }
}
