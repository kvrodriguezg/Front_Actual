using BLToolkit.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_AppReporteCcuFive9.ObjectModel
{

    public class Tracking
    {
        public string Info { get; set; }
        public DateTime Fecha { get; set; }
        public string Modulo { get; set; }
        public string Accion { get; set; }
        public TipoTrack Tipo { get; set; }
        public int IdRegistro { get; set; }
        public int UserId { get; set; }

        public enum TipoTrack
        {
            Ok,
            Info,
            Error
        }
        public static void InsertTracking(Tracking entrada)
        {
            //new ManagerReporte().InsertTracking(entrada);
        }
    }


}
