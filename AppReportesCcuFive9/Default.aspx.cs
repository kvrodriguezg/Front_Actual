using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppReportesCcuFive9
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                //var cookie = HttpContext.Current?.Request?.Cookies["_CentralCookie_jwt"];
               // return cookie?.Value;

            }
        }
    }
}