using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateServersList
{
    public class ServerType
    {
        public string _name;
        public string _type;
        public ServerType(string name, string type)
        {
            _name = name;
            _type = type;
        }
    }
}