using BL_AppReporteCcuFive9.ObjectModel;
using BLToolkit.Common;
using BLToolkit.TypeBuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BL_AppReporteCcuFive9.DAL
{
    public class DB_CCU_CONTEXT : DbContext
    {
        public DB_CCU_CONTEXT() : base("name=cnnNm")
        {
        }
        public DataTable GetReporteCcuFive9()
        {
            return GetDynamicData("asp_GetReporteCCUFive9", null);
        }
        public t24_ConfiguracionesKey GetConfiguracionByKey(string key)
        {
            List<SqlParameter> parametros = new List<SqlParameter>() {
                    new SqlParameter("@Key", key),
                };

            return this.Database.SqlQuery<t24_ConfiguracionesKey>("asp_GetConfiguracionByKey @Key", parametros.ToArray()).SingleOrDefault();
        }
        public void InsertTracking(Tracking entrada)
        {
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>()
                {
                     new SqlParameter("@Info",entrada.Info),
                     new SqlParameter("@Fecha",entrada.Fecha),
                     new SqlParameter("@IdRegistro",entrada.IdRegistro),
                     new SqlParameter("@Modulo",entrada.Modulo),
                     new SqlParameter("@Accion",entrada.Accion),
                     new SqlParameter("@UserId",entrada.UserId)
                };

                string strParam = string.Join(",", parametros.Select(x => x.ParameterName));
                this.Database.SqlQuery<object>(string.Concat("[dbo].[asp_InsertTracking] ", strParam), parametros.ToArray()).SingleOrDefault();
            }
            catch
            {

            }
        }
        public ConfigFive9 GetConfigFive9(string llave)
        {
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>()
                {
                     new SqlParameter("@llave",llave)
                };

                string strParam = string.Join(",", parametros.Select(x => x.ParameterName));

                return this.Database.SqlQuery<ConfigFive9>(string.Concat("[dbo].[asp_GetConfiguracionFive9] ", strParam), parametros.ToArray()).SingleOrDefault();
            }
            catch
            {
                return null;
            }
        }
        public void LimpiaCallLogFive9()
        {
            try
            {
                this.Database.SqlQuery<object>(string.Concat("[dbo].[asp_LimpiaCallLogFive9]")).SingleOrDefault();
            }
            catch
            {

            }
        }
        public DateTime GetFechaEjecucionReporteInfoCcuFive9()
        {
            try
            {
                return this.Database.SqlQuery<DateTime>(string.Concat("[dbo].[asp_GetFechaEjecucionReporteInfoCcuFive9]")).SingleOrDefault();
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        public void InsertCallLogCcuFive9(CallLogFive9 entrada)
        {
            List<SqlParameter> parametros = new List<SqlParameter>()
                {
                     new SqlParameter("@IdCliente",entrada.IdCliente ?? (object)DBNull.Value),
                     new SqlParameter("@Ofi",entrada.Ofi ?? (object)DBNull.Value),
                     new SqlParameter("@RutEjecutivo",entrada.RutEjecutivo ?? (object)DBNull.Value),
                     new SqlParameter("@first_name",entrada.FirstName ?? (object)DBNull.Value),
                     new SqlParameter("@last_name",entrada.LastName ?? (object)DBNull.Value),
                     new SqlParameter("@CALL_ID",entrada.CallId ?? (object)DBNull.Value),
                     new SqlParameter("@DATE",entrada.Date ?? (object)DBNull.Value),
                     new SqlParameter("@TIME",entrada.Time ??(object) DBNull.Value),
                     new SqlParameter("@DISPOSITION_GROUP_A",entrada.DispositionGroupA ?? (object)DBNull.Value),
                     new SqlParameter("@DISPOSITION_GROUP_B",entrada.DispositionGroupB ?? (object)DBNull.Value),
                     new SqlParameter("@DISPOSITION_GROUP_C",entrada.DispositionGroupC ?? (object)DBNull.Value),
                     new SqlParameter("@DISPOSITION",entrada.Disposition ?? (object)DBNull.Value),
                     new SqlParameter("@DNIS",entrada.Dnis ??(object) DBNull.Value),
                     new SqlParameter("@number1",entrada.Number1 ??(object) DBNull.Value),
                     new SqlParameter("@number2",entrada.Number2 ??(object) DBNull.Value),
                     new SqlParameter("@number3",entrada.Number3 ??(object) DBNull.Value),
                     new SqlParameter("@CALL_TYPE",entrada.CallType ?? (object)DBNull.Value),
                     new SqlParameter("@CAMPAIGN",entrada.Campaign ?? (object)DBNull.Value)
                };

            string strParam = string.Join(",", parametros.Select(x => x.ParameterName));
            this.Database.SqlQuery<object>(string.Concat("[dbo].[asp_InsertCallLogCcuFive9] ", strParam), parametros.ToArray()).SingleOrDefault();
        }
        public List<FiltroReporteCcuFive9> GetFiltroReporteCcuFive9(int userId)
        {
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>()
                {
                     new SqlParameter("@userId",userId)
                };

                string strParam = string.Join(",", parametros.Select(x => x.ParameterName));
                return this.Database.SqlQuery<FiltroReporteCcuFive9>(string.Concat("[dbo].[asp_GetFiltroReporteCcuFive9] ", strParam), parametros.ToArray()).ToList();
            }
            catch
            {
                return null;
            }
        }
        public void SaveFiltroReporteCcuFive9(int userId, string filtro)
        {
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>()
                {
                     new SqlParameter("@userId",userId),
                     new SqlParameter("@filtro",filtro)
                };

                string strParam = string.Join(",", parametros.Select(x => x.ParameterName));
                this.Database.SqlQuery<object>(string.Concat("[dbo].[asp_SaveFiltroReporteCcuFive9] ", strParam), parametros.ToArray()).SingleOrDefault();
            }
            catch
            {

            }
        }
        public void DeleteFiltroReporteCcuFive9(int idFiltro)
        {
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>()
                {
                     new SqlParameter("@idFiltro",idFiltro)
                };

                string strParam = string.Join(",", parametros.Select(x => x.ParameterName));
                this.Database.SqlQuery<object>(string.Concat("[dbo].[asp_DeleteFiltroReporteCcuFive9] ", strParam), parametros.ToArray()).SingleOrDefault();
            }
            catch
            {

            }
        }

        #region utilidades

        /// <summary>
        /// Retorna un DataTable, generado con los datos de respuesta de un StoredProcedure, en el cual las columnas de salida pueden variar.
        /// </summary>
        /// <param name="storedProcedure">Nombre procedimiento almacenado, se agrega con los parametros propios del SP ejemplo:  Sp_XXX @param1, @param2</param>
        /// <param name="parametros">Acepta una lista de objetos del tipo SqlParameter, EJ: new List<SqlParameter>() { new SqlParameter("@param1", variable1), new SqlParameter("@param2", variable2) }</param>
        /// <returns></returns>
        private DataTable GetDynamicData(string storedProcedure, List<SqlParameter> parametros)
        {
            DataSet dsSalida = new DataSet();
            SqlConnection sqlConn = (SqlConnection)this.Database.Connection;
            SqlCommand cmdDynamic = new SqlCommand(storedProcedure, sqlConn);
            SqlDataAdapter daDynamicData = new SqlDataAdapter(cmdDynamic);
            using (cmdDynamic)
            {
                if (parametros != null)
                    cmdDynamic.Parameters.AddRange(parametros.ToArray());
                daDynamicData.Fill(dsSalida);
            }

            if (dsSalida.Tables.Count > 0)
                return dsSalida.Tables[0];
            else
                return null;
        }
        #endregion
    }
}
