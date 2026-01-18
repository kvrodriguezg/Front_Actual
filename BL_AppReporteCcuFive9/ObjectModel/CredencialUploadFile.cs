using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL_AppReporteCcuFive9.ObjectModel
{
    [Serializable]
    public class CredencialUploadFile
    {
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string Dominio { get; set; }
    }
}
