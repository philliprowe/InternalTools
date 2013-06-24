using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace UpdateServersList
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            UpdateServersListService service = new UpdateServersListService();
            if (service._standalone) return;

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] { service };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
