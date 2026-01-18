
using BL_AppReporteCcuFive9;
using BL_AppReporteCcuFive9.ObjectModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AppReportesCcuFive9.Models
{
    public class Controlador
    {
        private ManagerReporte _mngRpt;
        private ManagerFive9 _mngFive9;
        public Controlador()
        {
            _mngRpt = new ManagerReporte();
            _mngFive9 = new ManagerFive9();
        }
        public t24_ConfiguracionesKey GetConfiguracionByKey(string key)
        {
            try
            {
                return _mngRpt.GetConfiguracionByKey(key);
            }
            catch
            {
                return null;
            }
        }
        public CredencialUploadFile ObtenerCredencialesUploadFile()
        {
            try
            {
                return _mngRpt.ObtenerCredencialesUploadFile();
            }
            catch
            {
                return null;
            }
        }
        public DataTable GetReporteCcuFive9()
        {
            try
            {
                return _mngRpt.GetReporteCcuFive9();
            }
            catch
            {
                return new DataTable();
            }
        }
        public DateTime GetFechaEjecucionReporteInfoCcuFive9()
        {
            return _mngRpt.GetFechaEjecucionReporteInfoCcuFive9();
        }
        public void ActualizaDatosReporte()
        {
            _mngRpt.ActualizaDatosCallLogCcuFive9();
        }
        #region filtro reporte 
        public List<FiltroReporteCcuFive9> GetFiltroReporteCcuFive9(int userId)
        {
            return _mngRpt.GetFiltroReporteCcuFive9(userId);
        }
        public void SaveFiltroReporteCcuFive9(int userId, string filtro)
        {
            _mngRpt.SaveFiltroReporteCcuFive9(userId, filtro);
        }
        public void DeleteFiltroReporteCcuFive9(int idFiltro)
        {
            _mngRpt.DeleteFiltroReporteCcuFive9(idFiltro);
        }
        #endregion
    }
}