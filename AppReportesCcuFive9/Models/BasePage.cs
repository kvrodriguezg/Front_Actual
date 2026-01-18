using BL_AppReporteCcuFive9;
using BL_AppReporteCcuFive9.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace AppReportesCcuFive9.Models
{
    public class BasePage : System.Web.UI.Page
    {
        internal Controlador _ctrl;
        public BasePage()
        {
            _ctrl = new Controlador();
        }
        public string Rol
        {
            get
            {
                return ViewState["ListaRoles"].ToString();
            }
            set
            {
                ViewState["ListaRoles"] = value;
            }
        }

        public int IdUsuario
        {
            get
            {
                return int.Parse(ViewState["IdUsuarioBase"].ToString());
            }
            set
            {
                ViewState["IdUsuarioBase"] = value;
            }
        }

        public int PaisUsuario
        {
            get
            {
                if (ViewState["PaisUsuario"] == null)
                {
                    ViewState["PaisUsuario"] = GetPaisUsuario();
                }

                return ViewState["PaisUsuario"] != null ? int.Parse(ViewState["PaisUsuario"].ToString()) : 0;
            }
        }

        private int GetPaisUsuario()
        {
            int idPais = 0;
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity is FormsIdentity)
            {
                FormsAuthenticationTicket ticket = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket;

                if (ticket != null)
                {
                    string[] UserData = ticket.UserData.Split(',');

                    if (UserData.Length > 0)
                    {
                        int.TryParse(UserData[0], out idPais);
                    }
                }
            }

            return idPais;
        }

        /// <summary>
        /// Retorna la URl base "https://Dominio.ExtensionDominio/" siempre termina con el /.
        /// </summary>
        public string GetUrlBase
        {
            get
            {
                return Request.Url.AbsoluteUri.ToLower().Contains("www") ? @"https://www." : @"https://";
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                string path = HttpContext.Current.Request.Url.AbsolutePath;
                int idusuario = 0;
                int.TryParse(User.Identity.Name, out idusuario);
                IdUsuario = idusuario;

                t24_ConfiguracionesKey configValidaAccesoPagina = _ctrl.GetConfiguracionByKey("ValidaAccesoPagina");
                if (configValidaAccesoPagina != null && bool.Parse(configValidaAccesoPagina.t24_valorkey))
                {
                    ValidaAccesoPagina(path, idusuario);
                }

                string sRol = "";
                t24_ConfiguracionesKey ConfKey = _ctrl.GetConfiguracionByKey("rol");
                if (ConfKey != null)
                {
                    string[] Roles = ConfKey.t24_valorkey.Split(",".ToCharArray());
                    foreach (string r in Roles)
                    {
                        if (User.IsInRole(r)) sRol += (sRol.Equals("") ? r : "," + r);
                    }
                }

                Rol = ((String.IsNullOrEmpty(sRol)) ? "" : sRol);
            }
            base.OnLoad(e);
        }
        /// <summary>
        /// Valida si un usario tiene o no acceso a la pagina solicitada.
        /// </summary>
        /// <param name="absolutePath">Se obtiene con HttpContext.Current.Request.Url.AbsolutePath;</param>
        /// <param name="userId">Es el ID del usuario que quiere acceder a  la pagina.</param>
        /// <returns></returns>
        private void ValidaAccesoPagina(string path, int userid)
        {
            try
            {
                if (!new Central.Core.AccesoDatos.CentralManager().ValidaAccesoPagina(path, userid))
                {
                    Response.Redirect("~/ErrorPages/AccesoDenegado.aspx");
                }
            }
            catch
            {
                Response.Redirect("~/ErrorPages/AccesoDenegado.aspx");
            }
        }
        public string EncryptDecrypt(string entrada, EncryptionEnum encryptionEnum)
        {

            t24_ConfiguracionesKey encryptDecryptKey = new Controlador().GetConfiguracionByKey("SECRET_KEY");
            string salida = string.Empty;
            switch (encryptionEnum)
            {
                case EncryptionEnum.Encrypt:
                    salida = EncryptionHelper.Encrypt(entrada, encryptDecryptKey.t24_valorkey);
                    break;
                case EncryptionEnum.Decrypt:
                    salida = EncryptionHelper.Decrypt(entrada, encryptDecryptKey.t24_valorkey);
                    break;
            }
            return salida;
        }
        public enum EncryptionEnum
        {
            Encrypt,
            Decrypt
        }

    }
}