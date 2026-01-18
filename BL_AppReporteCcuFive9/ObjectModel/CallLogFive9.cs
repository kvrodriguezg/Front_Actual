using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_AppReporteCcuFive9.ObjectModel
{
    public class CallLogFive9
    {
        public int? IdCliente { get; set; }
        public short? Ofi { get; set; }
        public int? RutEjecutivo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? CallId { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
        public string DispositionGroupA { get; set; }
        public string DispositionGroupB { get; set; }
        public string DispositionGroupC { get; set; }
        public string Disposition { get; set; }
        public long? Dnis { get; set; }
        public long? Number1 { get; set; }
        public long? Number2 { get; set; }
        public long? Number3 { get; set; }
        public string CallType { get; set; }
        public string Campaign { get; set; }

    }
}
