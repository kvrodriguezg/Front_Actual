using BLToolkit.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_AppReporteCcuFive9.ObjectModel
{
    public class ConfigFive9
    {
        private ManagerReporte _mngRpt;
        public ConfigFive9()
        {
            _mngRpt = new ManagerReporte();
        }
        public string Llave { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }

        public static ConfigFive9 GetConfigFive9(string llave)
        {
            return new ManagerReporte().GetConfigFive9(llave);
        }
    }
}
