using BL_AppReporteCcuFive9.DAL;
using BL_AppReporteCcuFive9.ObjectModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_AppReporteCcuFive9
{
    public class ManagerReporte
    {
        private DB_CCU_CONTEXT _context;
        public ManagerReporte()
        {
            _context = new DB_CCU_CONTEXT();
        }
        public DataTable GetReporteCcuFive9()
        {
            try
            {
                return _context.GetReporteCcuFive9();
            }
            catch
            {
                return new DataTable();
            }
        }
        public t24_ConfiguracionesKey GetConfiguracionByKey(string key)
        {
            try
            {
                return _context.GetConfiguracionByKey(key);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Obtiene las credenciales que seran utilizadas 
        /// al guardar archivos adjuntos en el directorio compartido.
        /// </summary>
        /// <returns></returns>
        public CredencialUploadFile ObtenerCredencialesUploadFile()
        {
            //LlaveCredencialADUploadFiles
            //"CredencialesUploadFile"
            t24_ConfiguracionesKey cfk = GetConfiguracionByKey("LlaveCredencialADUploadFiles");
            DataTable dtCredencialesAD = GetCrencialesADApp(cfk.t24_valorkey);

            return new CredencialUploadFile() { Usuario = dtCredencialesAD.Rows[0]["Usuario"].ToString(), Password = dtCredencialesAD.Rows[0]["Clave"].ToString(), Dominio = dtCredencialesAD.Rows[0]["Dominio"].ToString() };
        }
        public DataTable GetCrencialesADApp(string llave)
        {
            return new AtentoFW2008Context().GetCrencialesADApp(llave);
        }
        public ConfigFive9 GetConfigFive9(string llave)
        {
            try
            {
                return _context.GetConfigFive9(llave);
            }
            catch
            {
                return null;
            }
        }
        public void LimpiaCallLogFive9()
        {
            _context.LimpiaCallLogFive9();
        }
        public DateTime GetFechaEjecucionReporteInfoCcuFive9()
        {
            return _context.GetFechaEjecucionReporteInfoCcuFive9();
        }
        public void ActualizaDatosCallLogCcuFive9()
        {
            try
            {
                t24_ConfiguracionesKey MinutosRefrescoRptInfoCcuFive9 = new ManagerReporte().GetConfiguracionByKey("MinutosRefrescoRptInfoCcuFive9");
                DateTime ultimaEjecucion = new ManagerReporte().GetFechaEjecucionReporteInfoCcuFive9();
                ultimaEjecucion = ultimaEjecucion.AddMinutes(int.Parse(MinutosRefrescoRptInfoCcuFive9.t24_valorkey));
                if (DateTime.Now >= ultimaEjecucion)
                {
                    LimpiaCallLogFive9();
                    List<CallLogFive9> lstCallLog = new ManagerFive9().GetDatosReporte();
                    foreach (CallLogFive9 item in lstCallLog)
                    {
                        try
                        {
                            InsertCallLogCcuFive9(item);
                        }
                        catch(Exception ex)
                        {
                            InsertTracking(new Tracking()
                            {
                                Info = ex.ToString(),
                                Accion = "ActualizaDatosCallLogCcuFive9 | InsertCallLogCcuFive9",
                                Fecha = DateTime.Now,
                                Modulo = "ManagerReporte",
                                UserId = 0,
                                IdRegistro = 0
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                InsertTracking(new Tracking()
                {
                    Info = ex.ToString(),
                    Accion = "ActualizaDatosCallLogCcuFive9",
                    Fecha = DateTime.Now,
                    Modulo = "ManagerReporte",
                    UserId = 0,
                    IdRegistro = 0
                });
            }
        }
        public void InsertCallLogCcuFive9(CallLogFive9 entrada)
        {
            _context.InsertCallLogCcuFive9(entrada);
        }
        public List<FiltroReporteCcuFive9> GetFiltroReporteCcuFive9(int userId)
        {
            return _context.GetFiltroReporteCcuFive9(userId);
        }
        public void SaveFiltroReporteCcuFive9(int userId, string filtro)
        {
            _context.SaveFiltroReporteCcuFive9(userId, filtro);
        }
        public void DeleteFiltroReporteCcuFive9(int idFiltro)
        {
            _context.DeleteFiltroReporteCcuFive9(idFiltro);
        }
        public void InsertTracking(Tracking entrada)
        {
            _context.InsertTracking(entrada);
        }
    }
}
