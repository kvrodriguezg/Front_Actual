using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_AppReporteCcuFive9.DAL
{
    internal class AtentoFW2008Context : DbContext
    {
        public AtentoFW2008Context() : base("name=ModeloCentral")
        {
            
        }
        public DataTable GetCrencialesADApp(string llave)
        {
            List<SqlParameter> parametros = new List<SqlParameter>() {
                    new SqlParameter("@llave", llave),
                };
            return GetDynamicData("asp_GetCrencialesADApp", parametros);
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
