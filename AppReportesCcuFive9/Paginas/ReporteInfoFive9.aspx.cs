using AppReportesCcuFive9.Models;
using BL_AppReporteCcuFive9.ObjectModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Runtime.CompilerServices;
using System.Web.Services.Description;

using Ajax = Telerik.Web.Spreadsheet;
using DplSpreadsheet = Telerik.Windows.Documents.Spreadsheet;
using System.Web.DynamicData;
using Newtonsoft.Json;
using System.Diagnostics.Eventing.Reader;

namespace AppReportesCcuFive9.Paginas
{
    public partial class ReporteInfoFive9 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //ObtenerDatosReporte();
                CargarDatosReporte();
                CargarEjecutivosFiltro();
                CargarFiltroEjecutivo();
            }
        }
        private void CargarDatosReporte()
        {
            List<FiltroReporteCcuFive9> lstFiltro = _ctrl.GetFiltroReporteCcuFive9(IdUsuario);
            DataTable datos = _ctrl.GetReporteCcuFive9();

            if (lstFiltro != null && lstFiltro.Any())
            {
                datos = FiltrarPorNombreEjecutivo(datos, lstFiltro);
            }

            SetFEchaReporte();
            rptDatosReporteCcuFive9.DataSource = datos;
            rptDatosReporteCcuFive9.DataBind();
        }
        protected void btnReload_Click(object sender, EventArgs e)
        {
            _ctrl.ActualizaDatosReporte();
            CargarDatosReporte();
            SetFEchaReporte();
        }
        private void SetFEchaReporte()
        {
            DateTime fechaReporte = _ctrl.GetFechaEjecucionReporteInfoCcuFive9();
            lblFechaReporte.Text = fechaReporte.ToString("dd-MM-yyyy HH:mm");
        }
        private void CargarEjecutivosFiltro()
        {
            ddlNombreEjecutivo.DataTextField = "NombreEjecutivo";
            ddlNombreEjecutivo.DataValueField = "NombreEjecutivo";
            ddlNombreEjecutivo.DataSource = _ctrl.GetReporteCcuFive9();//GetTmpDatosReporte();
            ddlNombreEjecutivo.DataBind();
        }

        #region exportar a excel
        [WebMethod]
        public static string DescargarExcelInfoCcuFive9()
        {
            int idUsuario = 0;
            int.TryParse(HttpContext.Current.User.Identity.Name, out idUsuario);

            string sRol = "";
            t24_ConfiguracionesKey ConfKey = new Controlador().GetConfiguracionByKey("rol");
            if (ConfKey != null)
            {
                string[] Roles = ConfKey.t24_valorkey.Split(",".ToCharArray());
                foreach (string r in Roles)
                {
                    if (HttpContext.Current.User.IsInRole(r)) sRol += (sRol.Equals("") ? r : "," + r);
                }
            }

            byte[] excelResultByte = null;
            string strInicio = DateTime.Now.ToString("yyyyMMdd");
            string strHora = DateTime.Now.ToString("HH:mm:ss");
            DataTable dt = new Controlador().GetReporteCcuFive9();

            if (dt != null && dt.Rows.Count > 0)
            {
                string fileName = string.Concat(strInicio, "_", strHora, "_ReporteInformaciónFive9.xlsx");
                Ajax.Workbook ajaxWorkbook = new Ajax.Workbook();
                // populate the ajax workbook with dummy data
                ajaxWorkbook.Sheets = GetSheets(dt);
                // convert ajax workbook to DPL workbook
                DplSpreadsheet.Model.Workbook dplWorkbook = ajaxWorkbook.ToDocument();
                DplSpreadsheet.FormatProviders.IWorkbookFormatProvider formatProvider = new DplSpreadsheet.FormatProviders.OpenXml.Xlsx.XlsxFormatProvider();
                using (MemoryStream output = new MemoryStream())
                {
                    // export DPL workbook to MemoryStream
                    formatProvider.Export(dplWorkbook, output);
                    excelResultByte = output.ToArray();
                }
                return Convert.ToBase64String(excelResultByte, 0, excelResultByte.Length);
            }
            else
            {
                return null;
            }
        }
        private static List<Ajax.Worksheet> GetSheets(DataTable dt)
        {
            var result = new List<Ajax.Worksheet>();
            var sheet = new Ajax.Worksheet();
            sheet.ShowGridLines = true;
            sheet.Name = "Hoja1";
            int rowIndex = 0;

            #region genera nombre de Columnas
            int colIndex = 0;
            var rowColNames = new Ajax.Row() { Index = rowIndex++ };
            foreach (DataColumn col in dt.Columns)
            {
                var cellValue = col.ColumnName;
                var cell = new Ajax.Cell() { Index = colIndex++, Value = cellValue };
                rowColNames.AddCell(cell);
            }
            sheet.AddRow(rowColNames);
            #endregion

            #region Genera filas con datos
            foreach (DataRow dataRow in dt.Rows)
            {
                var row = new Ajax.Row() { Index = rowIndex++ };
                int columnIndex = 0;
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    //if (dataColumn.ColumnName == "ID") continue; // Skip the ID column
                    var cellValue = dataRow[dataColumn.ColumnName];
                    var cell = new Ajax.Cell() { Index = columnIndex++, Value = cellValue };
                    row.AddCell(cell);
                }
                sheet.AddRow(row);
            }
            result.Add(sheet);
            #endregion
            return result;
        }

        #endregion

        protected void btnAgregarFiltro_Click(object sender, EventArgs e)
        {
            try
            {
                string nombreSeleccionado = ddlNombreEjecutivo.SelectedValue;
                _ctrl.SaveFiltroReporteCcuFive9(IdUsuario, nombreSeleccionado);
            }
            catch
            {

            }
            finally
            {
                CargarFiltroEjecutivo();
                upnlFiltroEjecutivo.Update();
            }
        }
        private void CargarFiltroEjecutivo()
        {
            rptFiltroEjecutivo.DataSource = _ctrl.GetFiltroReporteCcuFive9(IdUsuario);
            rptFiltroEjecutivo.DataBind();
        }

        public DataTable FiltrarPorNombreEjecutivo(DataTable datos, List<FiltroReporteCcuFive9> lstFiltro)
        {
            // 1) Normalizar y convertir la lista de filtros a HashSet (rápido y sin duplicados)
            var filtros = new HashSet<string>(
                lstFiltro
                    .Where(f => !string.IsNullOrWhiteSpace(f.Filtro))
                    .Select(f => f.Filtro.Trim().ToUpperInvariant())
            );

            // Si no hay filtros, devuelve DataTable vacío (o decide retornar original)
            if (filtros.Count == 0)
                return datos.Clone(); // mismo esquema pero sin filas

            // 2) Filtrar filas de datos
            var filasFiltradas = datos.AsEnumerable()
                .Where(row =>
                {
                    // Manejo de nulos y espacios
                    var nombre = row.Field<string>("NombreEjecutivo");
                    if (string.IsNullOrWhiteSpace(nombre)) return false;

                    var key = nombre.Trim().ToUpperInvariant();
                    return filtros.Contains(key);
                });

            // 3) Materializar a DataTable (copiar esquema y cargar filas)
            if (!filasFiltradas.Any())
                return datos.Clone();

            return filasFiltradas.CopyToDataTable();
        }

        protected void btnQuitarFiltro_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                int idFiltro = int.Parse(btn.CommandArgument);
                _ctrl.DeleteFiltroReporteCcuFive9(idFiltro);
            }
            catch
            {

            }
            finally
            {
                CargarFiltroEjecutivo();
                upnlFiltroEjecutivo.Update();
            }
        }

        protected void btnCerrarModal_Click(object sender, EventArgs e)
        {
            CargarDatosReporte();
        }
    }
}