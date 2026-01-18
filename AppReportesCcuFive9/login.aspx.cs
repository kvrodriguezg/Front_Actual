using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppReportesCcuFive9
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtUserName.Focus();
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text.Trim().Replace(".", "");
            int idPais = 2;

            if (username.IndexOf(@"-") > -1)
                username = username.Split(@"-".ToCharArray())[0];

            if (new Central.Core.Membership.CentralMembershipProvider().ValidateUser(username, txtPassword.Text.Trim(), idPais) > 0)
            //if (Membership.ValidateUser(username, txtPassword.Text.Trim()))
            {

                // Crea el authetication ticket
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1,
                                                                                    name: username,
                                                                                    issueDate: DateTime.Now,
                                                                                    expiration: DateTime.Now.AddMinutes(20),
                                                                                    isPersistent: RememberMe.Checked,
                                                                                    userData: String.Concat(idPais),
                                                                                    cookiePath: FormsAuthentication.FormsCookiePath);
                // encriptar el ticket.
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                // Crear la cookie y agregar ticket encriptado como datos de cookie        
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                if (RememberMe.Checked)
                    authCookie.Expires = DateTime.Now.AddYears(1);
                // Agregar la cookie a la colleción saliente de las cookies.
                HttpContext.Current.Response.Cookies.Add(authCookie);

                // Redirecciona el usuario a la página que hizo el request original
                // si es que el usuario tiene bien ingresados los datos sino lo envia a actualización
                bool datosCorrectos = true;
                try
                {
                    Membership.GetUser(txtUserName.Text.Trim(), true);
                }
                catch
                {
                    datosCorrectos = false;
                }

                if (datosCorrectos)
                    Response.Redirect(FormsAuthentication.GetRedirectUrl(username, RememberMe.Checked));
                else
                    FailureText.Text = "Existe un error al carga los datos del usuario, consulte con el administrador del sistema.";

            }
            else
            {
                FailureText.Text = "El usuario no existe.";
            }
        }
    }
}