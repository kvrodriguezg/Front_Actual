using AppReportesCcuFive9.Models;
using BL_AppReporteCcuFive9.ObjectModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public class Util
{
    public static string GetEnumDescription(Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());

        DescriptionAttribute[] attributes =
            (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (attributes != null && attributes.Length > 0)
            return attributes[0].Description;
        else
            return value.ToString();
    }

    public string devuelveMes(int valorMes)
    {
        int mes = valorMes;
        string nombreMes = string.Empty;
        switch (mes)
        {
            case 1:
                nombreMes = "enero";
                break;
            case 2:
                nombreMes = "febrero";
                break;
            case 3:
                nombreMes = "marzo";
                break;
            case 4:
                nombreMes = "abril";
                break;
            case 5:
                nombreMes = "mayo";
                break;
            case 6:
                nombreMes = "junio";
                break;
            case 7:
                nombreMes = "julio";
                break;
            case 8:
                nombreMes = "agosto";
                break;
            case 9:
                nombreMes = "septiembre";
                break;
            case 10:
                nombreMes = "octubre";
                break;
            case 11:
                nombreMes = "noviembre";
                break;
            case 12:
                nombreMes = "diciembre";
                break;

            default:
                nombreMes = "";
                break;
        }

        return nombreMes;
    }

    public string devuelveDia(int valor_dia)
    {
        string dia = string.Empty;
        if (valor_dia < 10)
            dia = "0" + valor_dia.ToString();
        else
            dia = valor_dia.ToString();
        return dia;
    }

    #region Llamado scripts bootstrap jquery
    public static void OpenModalPopUp(Page page, string modalName)
    {
        ScriptManager.RegisterStartupScript(page, page.GetType(), "script", string.Concat("$('#", modalName, "').modal({backdrop: 'static', keyboard: false});"), true);
    }

    public static void CloseAndOpenModalPopUp(Page page, string modalNameClose, string modalNameOpen)
    {
        ScriptManager.RegisterStartupScript(page, page.GetType(), "script", string.Concat("$('#", modalNameClose, "').modal('toggle');", "$('#", modalNameOpen, "').modal({backdrop: 'static', keyboard: false});"), true);
    }

    public static void CloseModalPopUp(Page page, string modalName)
    {
        ScriptManager.RegisterStartupScript(page, page.GetType(), "script", string.Concat("$('#", modalName, "').modal('toggle');"), true);
    }

    public static void OpenAlertPopUp(Page page, string modalName)
    {
        ScriptManager.RegisterStartupScript(page, page.GetType(), "script", string.Concat("$('#", modalName, "').show();"), true);
    }

    public static void EjecutarFuncionJavascript(Page page, string key, string nombreFuncion)
    {
        ScriptManager.RegisterStartupScript(page, page.GetType(), key, string.Concat(nombreFuncion, "();"), true);
    }

    public static void EjecutarFuncionJavascriptConParametros(Page page, string key, string nombreFuncion)
    {
        ScriptManager.RegisterStartupScript(page, page.GetType(), key, string.Concat(nombreFuncion, ";"), true);
    }
    #endregion

    public static void DownloadFile(string urlFile, string realFileName, bool permiteDescarga)
    {
        //Create a stream for the file
        Stream stream = null;

        //This controls how many bytes to read at a time and send to the client
        int bytesToRead = 10000;

        // Buffer to read bytes in chunk size specified above
        byte[] buffer = new Byte[bytesToRead];

        // The number of bytes read
        try
        {
            //Create a WebRequest to get the file
            HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(urlFile);

            //Create a response for this request
            HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();

            if (fileReq.ContentLength > 0)
                fileResp.ContentLength = fileReq.ContentLength;

            //Get the Stream returned from the response
            stream = fileResp.GetResponseStream();

            // prepare the response to the client. resp is the client Response
            var resp = HttpContext.Current.Response;

            //Indicate the type of data being sent
            resp.ContentType = "application/octet-stream";

            if (!permiteDescarga)
                resp.AddHeader("X-Download-Options", "nosave");


            //Name the file 
            resp.AddHeader("Content-Disposition", "attachment; filename=\"" + realFileName + "\"");
            resp.AddHeader("Content-Length", fileResp.ContentLength.ToString());

            int length;
            do
            {
                // Verify that the client is connected.
                if (resp.IsClientConnected)
                {
                    // Read data into the buffer.
                    length = stream.Read(buffer, 0, bytesToRead);

                    // and write it out to the response's output stream
                    resp.OutputStream.Write(buffer, 0, length);

                    // Flush the data
                    resp.Flush();

                    //Clear the buffer
                    buffer = new Byte[bytesToRead];
                }
                else
                {
                    // cancel the download if client has disconnected
                    length = -1;
                }
            } while (length > 0); //Repeat until no data is read
        }
        finally
        {
            if (stream != null)
            {
                //Close the input stream
                stream.Close();
            }
        }
    }

    public static void DownloadFile(Page page, string fileName, string fileUrl)
    {
        page.Response.Clear();
        bool success = ResponseFile(page.Request, page.Response, fileName, fileUrl, 1024000);
        //if (!success)
        //    page.Response.Write("Downloading Error!");
        page.Response.End();

    }
    private static bool ResponseFile(HttpRequest _Request, HttpResponse _Response, string _fileName, string _fullPath, long _speed)
    {
        try
        {
            string rutaAdjunto = new Controlador().GetConfiguracionByKey("RutaArchivos").t24_valorkey;
            CredencialUploadFile credencialesUpload = new Controlador().ObtenerCredencialesUploadFile();

            System.Security.Principal.WindowsImpersonationContext impersonationContext;
            impersonationContext = Impersonalizacion.WinLogOn(credencialesUpload.Usuario, credencialesUpload.Password, credencialesUpload.Dominio);

            FileStream myFile = new FileStream(_fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            impersonationContext.Undo();

            BinaryReader br = new BinaryReader(myFile);
            try
            {
                _Response.AddHeader("Accept-Ranges", "bytes");
                _Response.Buffer = false;
                long fileLength = myFile.Length;
                long startBytes = 0;

                int pack = 10240; //10K bytes
                int sleep = (int)Math.Floor((double)(1000 * pack / _speed)) + 1;
                if (_Request.Headers["Range"] != null)
                {
                    _Response.StatusCode = 206;
                    string[] range = _Request.Headers["Range"].Split(new char[] { '=', '-' });
                    startBytes = Convert.ToInt64(range[1]);
                }
                _Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                if (startBytes != 0)
                {
                    _Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                }
                _Response.AddHeader("Connection", "Keep-Alive");
                _Response.ContentType = "application/octet-stream";
                _Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(_fileName, System.Text.Encoding.UTF8));

                br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                int maxCount = (int)Math.Floor((double)((fileLength - startBytes) / pack)) + 1;

                for (int i = 0; i < maxCount; i++)
                {
                    if (_Response.IsClientConnected)
                    {
                        _Response.BinaryWrite(br.ReadBytes(pack));
                        Thread.Sleep(sleep);
                    }
                    else
                    {
                        i = maxCount;
                    }
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                br.Close();
                myFile.Close();
                //Util.EliminArchivo(_fullPath);//esto se ejecuta solo si el llamado esta dentro de impersonalisacion.
            }
        }
        catch (Exception ex)
        {

            return false;
        }
        return true;
    }
    public static string Dv(string r)
    {
        int suma = 0;
        for (int x = r.Length - 1; x >= 0; x--)
            suma += int.Parse(char.IsDigit(r[x]) ? r[x].ToString() : "0") * (((r.Length - (x + 1)) % 6) + 2);
        int numericDigito = (11 - suma % 11);
        string digito = numericDigito == 11 ? "0" : numericDigito == 10 ? "K" : numericDigito.ToString();
        return digito;
    }
    public static string FormatearRut(string rut)
    {
        int cont = 0;
        string format;
        if (rut.Length == 0)
        {
            return "";
        }
        else
        {
            rut = rut.Replace(".", "");
            rut = rut.Replace("-", "");
            format = "-" + rut.Substring(rut.Length - 1);
            for (int i = rut.Length - 2; i >= 0; i--)
            {
                format = rut.Substring(i, 1) + format;
                cont++;
                if (cont == 3 && i != 0)
                {
                    format = "." + format;
                    cont = 0;
                }
            }
            return format;
        }
    }

    /// <summary>
    /// Guarda los archivos adjuntos en la ruta que corresponde y retorna una lista con los archivos que fueron guardados correctamente.
    /// </summary>
    /// <param name="sFilePath">Ruta en donde se guardaran los archivos.</param>
    /// <returns>Retorna una lista de Dictionary, las columnas del diccionario son :NombreArchivoReal,NombreArchivoGenerado(GUID)</returns>
    //public static Collection<AdjuntosVtr> GuardarArchivosConNuevoNombre(UploadedFileCollection uploadedFileCollection, string sFilePath)
    //{
    //    Collection<AdjuntosVtr> archivosGuardados = new Collection<AdjuntosVtr>();
    //    try
    //    {
    //        CredencialUploadFile credencialesUpload = new Manager().ObtenerCredencialesUploadFile();
    //        System.Security.Principal.WindowsImpersonationContext impersonationContext;
    //        impersonationContext = Impersonalizacion.WinLogOn(credencialesUpload.Usuario, credencialesUpload.Password, credencialesUpload.Dominio);

    //        foreach (UploadedFile item in uploadedFileCollection)
    //        {
    //            Guid nuevoNombreArchivo = Guid.NewGuid();
    //            AdjuntosVtr archivo = new AdjuntosVtr();
    //            archivo.NombreArchivoGenerado = string.Concat(nuevoNombreArchivo.ToString(), item.GetExtension());
    //            archivo.NombreArchivoReal = item.FileName;
    //            try
    //            {
    //                item.SaveAs(string.Concat(sFilePath, string.Concat(nuevoNombreArchivo.ToString(), item.GetExtension())));
    //            }
    //            catch (Exception ex)
    //            {
    //                archivo.MensajeError = string.Concat("Ha ocurrido un error al intentar guardar el archivo: ", ex.ToString());
    //            }
    //            archivosGuardados.Add(archivo);
    //        }

    //        impersonationContext.Undo();
    //    }
    //    catch
    //    {
    //        throw;
    //    }

    //    return archivosGuardados;
    //}

    public static void SetDatosQueryStringToControl(string[] parametros, string etiqueta, TypeCode tipoDato, object control)
    {
        if (parametros.Where(x => x.ToUpper().Contains(etiqueta.ToUpper())).Any())
        {
            string dato = parametros.Where(x => x.ToUpper().Contains(etiqueta.ToUpper())).SingleOrDefault().Split(':')[1];
            switch (tipoDato)
            {
                case TypeCode.Int32:
                    int intDato = 0;
                    int.TryParse(dato, out intDato);
                    if (intDato > 0)
                    {
                        SetControlData(control, intDato);
                    }
                    break;
                case TypeCode.String:
                    if (!string.IsNullOrEmpty(dato.Trim()) && dato != "0")
                    {
                        SetControlData(control, dato);
                    }
                    break;
                case TypeCode.DateTime:
                    SetControlData(control, dato);
                    break;
            }
        }
    }
    private static void SetControlData(object control, object data)
    {
        switch (control.GetType().Name)
        {
            case "TextBox":
                ((TextBox)control).Text = data.ToString();//data.GetType().Name == "Int" ? ((int)data).ToString() : (string)data;
                break;
            case "DropDownList":
                ((DropDownList)control).SelectedValue = data.ToString();
                break;
            case "RadComboBox":
                ((RadComboBox)control).SelectedValue = data.ToString();
                break;
            case "RadDatePicker":
                DateTime fecha = DateTime.Now;
                if (!DateTime.TryParse((string)data, out fecha))
                    fecha = DateTime.Now;
                ((RadDatePicker)control).SelectedDate = fecha;
                break;
        }
    }
}

public static class FindControls
{
    #region find control recursivo

    /// <summary>
    /// Similar to Control.FindControl, but recurses through child controls.
    /// </summary>
    public static T FindControl<T>(this Control startingControl, string id) where T : Control
    {
        T found = startingControl.FindControl(id) as T;
        if (found == null)
        {
            found = FindChildControl<T>(startingControl, id);
        }
        return found;
    }

    /// <summary>     
    /// Similar to Control.FindControl, but recurses through child controls.
    /// Assumes that startingControl is NOT the control you are searching for.
    /// </summary>
    public static T FindChildControl<T>(this Control startingControl, string id) where T : Control
    {
        T found = null;

        foreach (Control activeControl in startingControl.Controls)
        {
            found = activeControl as T;

            if (found == null || (string.Compare(id, found.ID, true) != 0))
            {
                found = FindChildControl<T>(activeControl, id);
            }

            if (found != null)
            {
                break;
            }
        }

        return found;
    }

    public static void GetControlList<T>(ControlCollection controlCollection, List<T> resultCollection) where T : Control
    {
        foreach (Control control in controlCollection)
        {
            //if (control.GetType() == typeof(T))
            if (control is T) // This is cleaner
                resultCollection.Add((T)control);

            if (control.HasControls())
                GetControlList(control.Controls, resultCollection);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Root"></param>
    /// <param name="Id"></param>
    /// <returns></returns>
    public static Control FindControlRecursive(Control Root, string Id)
    {
        if (Root.ID == Id)
            return Root;

        foreach (Control Ctl in Root.Controls)
        {
            Control FoundCtl = FindControlRecursive(Ctl, Id);
            if (FoundCtl != null)
                return FoundCtl;
        }

        return null;
    }





    #endregion

}

/// <summary>
/// Finds all controls of type T stores them in FoundControls
/// </summary>
/// <typeparam name="T"></typeparam>
public class ControlFinder<T> where T : Control
{
    private readonly List<T> _foundControls = new List<T>();
    public IEnumerable<T> FoundControls
    {
        get { return _foundControls; }
    }

    public void FindChildControlsRecursive(Control control)
    {
        foreach (Control childControl in control.Controls)
        {
            if (childControl.GetType() == typeof(T))
            {
                _foundControls.Add((T)childControl);
            }
            else
            {
                FindChildControlsRecursive(childControl);
            }
        }
    }
}

public static class MensajeGenericoBootstrap
{
    #region Mensage Generico Bootstrap Dialog
    /// <summary>
    /// 
    /// </summary>
    /// <param name="page">Pagina actual</param>
    /// <param name="mensaje">Mensaje que sera visto en el dialog</param>
    /// <param name="reloadPage">funcion javascript que ejecutara el reload de la pagina (Refresh)</param>
    /// <param name="tipoMsg">El tipo de mensaje </param>
    public static void MensajeGenerico(Page page, string mensaje, string PageFunction, BootstrapDialogMessageType tipoMsg)
    {
        string scriptMensaje = "BootstrapDialog.show({ title: [titulo], type: [tipo], message: '[mensaje]',closable: false, buttons: [{label: 'Cerrar',action: function (dialogItself) {dialogItself.close(); [BtnReloadPage] }}]});";
        string reloadPage = !string.IsNullOrEmpty(PageFunction) ? PageFunction : string.Empty;
        switch (tipoMsg)
        {
            case BootstrapDialogMessageType.TYPE_SUCCESS:
                scriptMensaje = scriptMensaje.Replace("[mensaje]", mensaje.Replace(@"\r\n?|\n", "")).Replace("[titulo]", "'Proceso Exitoso'").Replace("[tipo]", string.Concat("BootstrapDialog.", BootstrapDialogMessageType.TYPE_SUCCESS)).Replace("[BtnReloadPage]", reloadPage);
                break;
            case BootstrapDialogMessageType.TYPE_WARNING:
                scriptMensaje = scriptMensaje.Replace("[mensaje]", mensaje.Replace(@"\r\n?|\n", "")).Replace("[titulo]", "'Advertencia!'").Replace("[tipo]", string.Concat("BootstrapDialog.", BootstrapDialogMessageType.TYPE_WARNING)).Replace("[BtnReloadPage]", reloadPage);
                break;

            case BootstrapDialogMessageType.TYPE_DANGER:
                scriptMensaje = scriptMensaje.Replace("[mensaje]", mensaje.Replace(@"\r\n?|\n", "")).Replace("[titulo]", "'Ha ocurrido un error'").Replace("[tipo]", string.Concat("BootstrapDialog.", BootstrapDialogMessageType.TYPE_DANGER)).Replace("[BtnReloadPage]", reloadPage);
                break;
        }

        ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), scriptMensaje, true);
    }
    /// <summary>
    /// Levanta un aventana modal del tipo Confirmacion (botones aceptar y cancelar)
    /// </summary>
    /// <param name="page">Pagina actual</param>
    /// <param name="titulo">Titulo de la ventana</param>
    /// <param name="mensaje">Mensaje que sera mostrado en la ventana.</param>
    /// <param name="funcionAceptar">funcion javascript a ejecutar al dar click en el boton aceptar. Ej: reload();</param>
    /// <param name="funcionCancelar">funcion javascript a ejecutar al dar click en el boton cancelar. Ej reload();</param>
    /// <param name="tipoMsg">Tipo de mensaje , el cual definira el estilo de la ventana. Ej: Warning, Success, Error</param>
    public static void MensajeGenerico(Page page, string titulo, string mensaje, string funcionAceptar, string funcionCancelar, BootstrapDialogMessageType tipoMsg)
    {
        string scriptMensaje = @"BootstrapDialog.confirm({
                        title: '[titulo]',
                        message: '[mensaje]',
                        type: [tipo], 
                        closable: false, 
                        draggable: false, 
                        btnCancelLabel: 'Cancelar',
                        btnOKLabel: 'Aceptar',                        
                        callback: function(result) {
                            // result will be true if button was click, while it will be false if users close the dialog directly.
                            if(result) {
                                [FuncionAceptar]
                            }else {
                                [FuncionCancelar]                               
                            }
                        }
                    });";


        switch (tipoMsg)
        {
            case BootstrapDialogMessageType.TYPE_SUCCESS:
                scriptMensaje = scriptMensaje.Replace("[mensaje]", mensaje.Replace(@"\r\n?|\n", "")).Replace("[titulo]", titulo).Replace("[tipo]", string.Concat("BootstrapDialog.", BootstrapDialogMessageType.TYPE_SUCCESS)).Replace("[FuncionAceptar]", funcionAceptar).Replace("[FuncionCancelar]", funcionCancelar);
                break;
            case BootstrapDialogMessageType.TYPE_WARNING:
                scriptMensaje = scriptMensaje.Replace("[mensaje]", mensaje.Replace(@"\r\n?|\n", "")).Replace("[titulo]", titulo).Replace("[tipo]", string.Concat("BootstrapDialog.", BootstrapDialogMessageType.TYPE_WARNING)).Replace("[FuncionAceptar]", funcionAceptar).Replace("[FuncionCancelar]", funcionCancelar);
                break;

            case BootstrapDialogMessageType.TYPE_DANGER:
                scriptMensaje = scriptMensaje.Replace("[mensaje]", mensaje.Replace(@"\r\n?|\n", "")).Replace("[titulo]", titulo).Replace("[tipo]", string.Concat("BootstrapDialog.", BootstrapDialogMessageType.TYPE_DANGER)).Replace("[FuncionAceptar]", funcionAceptar).Replace("[FuncionCancelar]", funcionCancelar);
                break;
        }

        ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), scriptMensaje, true);
    }

    public enum BootstrapDialogMessageType
    {
        TYPE_SUCCESS,
        TYPE_WARNING,
        TYPE_DANGER
    }
    #endregion
}


