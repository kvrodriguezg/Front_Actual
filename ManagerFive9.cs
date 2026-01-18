using BL_AppReporteCcuFive9.Five9Services;
using BL_AppReporteCcuFive9.ObjectModel;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace BL_AppReporteCcuFive9
{
    public class ManagerFive9
    {
        public ManagerFive9()
        {
        }
        public List<CallLogFive9> GetDatosReporte()
        {

            // Fuerza TLS 1.2 para todas las llamadas salientes de .NET Framework
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            List<CallLogFive9> lstCallLogFive9 = new List<CallLogFive9>();
            try
            {
                t24_ConfiguracionesKey configReporteInfoCcuFive9 = new ManagerReporte().GetConfiguracionByKey("ReporteInfoCcuFive9");
                string[] configInfoCcuFive9 = configReporteInfoCcuFive9.t24_valorkey.Split('|');

                DateTime fechaIni = DateTime.Today;
                DateTime fechaFin = fechaIni.AddDays(1).AddTicks(-1);

                Five9Services.WsAdminClient five9Client = ConfiguraClientFive9();
                Five9Services.customReportCriteria filtros = new Five9Services.customReportCriteria()
                {
                    //reportObjects = new reportObjectList[] { rObjects },
                    time = new reportTimeCriteria() { start = fechaIni, end = fechaFin, startSpecified = true, endSpecified = true }
                };

                string resultCode = five9Client.runReport(configInfoCcuFive9[0], configInfoCcuFive9[1], filtros);
                while (five9Client.isReportRunning(resultCode, 2))
                {
                    System.Threading.Thread.Sleep(3000);
                }

                string datos = five9Client.getReportResultCsv(resultCode);
                lstCallLogFive9 = ProcesaDatosReporteCallLogDesarrolloCsv(datos);

                five9Client.closeSession();
                five9Client.Close();
            }
            catch (Exception ex)
            {
                new ManagerReporte().InsertTracking(new Tracking()
                {
                    Info = ex.ToString(),
                    Accion = "GetDatosReporte",
                    Fecha = DateTime.Now,
                    Modulo = "ManagerFive9",
                    UserId = 0,
                    IdRegistro = 0
                });
            }
            return lstCallLogFive9;
        }

        private List<CallLogFive9> ProcesaDatosReporteCallLogDesarrolloCsv(string datos)
        {
            List<CallLogFive9> lstSalida = new List<CallLogFive9>();
            var reader = new StringReader(datos);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",", // cambia si tu CSV usa punto y coma, etc.
                BadDataFound = null // evita excepciones por datos 'malos'
            };

            var csvReader = new CsvReader(reader, config);
            csvReader.Context.RegisterClassMap<CallLogFive9Map>();
            lstSalida = csvReader.GetRecords<CallLogFive9>().ToList();
            return lstSalida;
        }

        private Five9Services.WsAdminClient ConfiguraClientFive9()
        {
            Five9Services.WsAdminClient clienteFive9 = new Five9Services.WsAdminClient();
            //Metodo Autorizacion para servicio
            AuthHeaderInserter inserter = new AuthHeaderInserter();
            string llaveConfigApiFive9 = ConfigurationManager.AppSettings["userApiFive9"].ToString();
            ConfigFive9 configCcu = ConfigFive9.GetConfigFive9(llaveConfigApiFive9);

            inserter.Username = configCcu.Usuario;
            inserter.Password = configCcu.Password;

            clienteFive9.Endpoint.Behaviors.Add(new AuthHeaderBehavior(inserter));
            return clienteFive9;
        }
        private List<CallLogFive9> ProcesaDatosReporteCallLogDesarrollo(reportRowData reportData)
        {
            List<CallLogFive9> lstCallLogFive9 = new List<CallLogFive9>();
            if (reportData.records != null)
            {
                foreach (var data in reportData.records)
                {
                    try
                    {
                        CallLogFive9 callLog = new CallLogFive9();
                        callLog.IdCliente = string.IsNullOrEmpty(data.values[0]) ? 0 : int.Parse(data.values[0]);

                        short ofi = 0;

                        if (!string.IsNullOrEmpty(data.values[1]))
                            short.TryParse(data.values[1], out ofi);

                        callLog.Ofi = ofi;
                        callLog.RutEjecutivo = string.IsNullOrEmpty(data.values[2]) ? 0 : int.Parse(data.values[2]);
                        callLog.FirstName = data.values[3];
                        callLog.LastName = data.values[4];
                        callLog.CallId = string.IsNullOrEmpty(data.values[5]) ? 0 : long.Parse(data.values[5]);

                        callLog.DispositionGroupA = data.values[6];
                        callLog.DispositionGroupB = data.values[7];
                        callLog.DispositionGroupC = data.values[8];
                        callLog.Disposition = data.values[9];

                        long dnis = 0;
                        //long.TryParse(data.values[10].Replace("+", ""), out dnis);
                        callLog.Dnis = dnis;

                        long number1 = 0;
                        if (!string.IsNullOrEmpty(data.values[11]))
                            long.TryParse(data.values[11].Replace("+", ""), out number1);
                        callLog.Number1 = number1;

                        long number2 = 0;
                        if (!string.IsNullOrEmpty(data.values[12]))
                            long.TryParse(data.values[12].Replace("+", ""), out number2);
                        callLog.Number2 = number2;

                        long number3 = 0;
                        if (!string.IsNullOrEmpty(data.values[13]))
                            long.TryParse(data.values[13].Replace("+", ""), out number3);
                        callLog.Number3 = number3;

                        callLog.CallType = data.values[14];
                        callLog.Campaign = data.values[15];

                        DateTime fechaCall = DateTime.MinValue;
                        DateTime.TryParse(data.values[16], out fechaCall);
                        callLog.Date = fechaCall.Date;

                        TimeSpan time = TimeSpan.MinValue;
                        TimeSpan.TryParse(data.values[17], out time);
                        callLog.Time = time;

                        lstCallLogFive9.Add(callLog);
                    }
                    catch (Exception ex)
                    {
                        new ManagerReporte().InsertTracking(new Tracking()
                        {
                            Info = ex.ToString(),
                            Accion = "ProcesaDatosReporteCallLogDesarrollo",
                            Fecha = DateTime.Now,
                            Modulo = "ManagerFive9",
                            UserId = 0,
                            IdRegistro = 0
                        });
                    }
                }
            }
            return lstCallLogFive9;
        }

        private sealed class CallLogFive9Map : ClassMap<CallLogFive9>
        {
            public CallLogFive9Map()
            {
                // Coincidencias exactas según tu CSV
                Map(m => m.IdCliente).Name("IdCliente");
                Map(m => m.Ofi).Name("Ofi");
                Map(m => m.RutEjecutivo).Name("RutEjecutivo");

                // Nombre/Apellido mapeados a FirstName/LastName
                Map(m => m.FirstName).Name("Nombre");
                Map(m => m.LastName).Name("Apellido");

                Map(m => m.CallId).Name("ID DE LLAMADA");

                // Grupos de disposiciones
                Map(m => m.DispositionGroupA).Name("GRUPO DE DISPOSICIONES A");
                Map(m => m.DispositionGroupB).Name("GRUPO DE DISPOSICIONES B");
                Map(m => m.DispositionGroupC).Name("GRUPO DE DISPOSICIONES C");

                Map(m => m.Disposition).Name("DISPOSICIÓN");
                Map(m => m.Dnis).Name("DNIS");

                // Teléfonos: Principal y Alternativos → Number1..3
                Map(m => m.Number1).Name("Principal");
                Map(m => m.Number2).Name("Alternativo 1");
                Map(m => m.Number3).Name("Alternativo 2");

                Map(m => m.CallType).Name("TIPO DE LLAMADA");
                Map(m => m.Campaign).Name("CAMPAÑA");

                // Fecha y Hora (se configurarán formatos en el reader)
                Map(m => m.Date).Name("FECHA");
                Map(m => m.Time).Name("HORA");
            }
        }
    }
}
