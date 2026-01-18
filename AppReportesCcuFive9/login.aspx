<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="AppReportesCcuFive9.login" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link href="App_Themes/TemaCentral/estilos.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .loginBox {
            position: absolute;
            top: 260px;
            left: 40%;
            margin-left: -362px;
        }
    </style>

    <script src="App_Themes/TemaCentral/js/common.js"></script>

    <script type="text/javascript" language="javascript">
        function rutFilter(sender, eventArgs) {
            var c = eventArgs.get_keyCode();
            if (c > 44 && c < 58) return; //0..9-.
            if (c == 75) return; //K
            if (c == 8) return; //Atras
            if (c == 9) return; //Tab
            if (c == 46) return; //Supr            
            if (c == 107) {//k                
                c = c - 32;
                return;
            }
            eventArgs.set_cancel(true);
        }
        function txtRUT_OnBlur(sender, eventArgs) {
            var text = sender.get_value();
            if (ValidarRutAtento(text))
                sender.set_value(FormatoRut(text));
        }
        function cvValidateRut(source, args) {
            args.IsValid = true;
            if (!ValidarRutAtento(args.Value))
                args.IsValid = false;
        }
    </script>
</head>
<body class="fondob">
    <form id="Form1" method="post" runat="server">
        <asp:ScriptManager ID="sm" runat="server"></asp:ScriptManager>
        <div style="width: 100%">
            <table border="0" cellspacing="2" cellpadding="1" width="600px" class="loginBox">
                <tr>
                    <td style="border-right-width: 1px; border-right-style: solid; padding-right: 62px; border-right-color: #567db0;">
                        <table border="0" cellpadding="2" cellspacing="28" width="340">
                            <tr>
                                <td style="width: 40%" align="right">
                                    <img src="images/logotranschico.gif" />
                                </td>
                            </tr>
                            <tr>
                                <td
                                    align="right" class="supertitulo">Central de Aplicaciones v2
                                </td>
                            </tr>
                            <tr>
                                <td
                                    align="right" class="titulo">©2008 Atento Chile
                                </td>
                            </tr>
                            <tr>
                                <td
                                    align="right" class="plainlink">
                                    <asp:HyperLink ID="PasswordRecoveryLink" runat="server" NavigateUrl="~/UserManager/PasswordRecovery.aspx"
                                        Target="main" ForeColor="Gray" Font-Size="Small" EnableViewState="False">Ha olvidado su clave?</asp:HyperLink>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="padding-left: 62px">
                        <table border="0" cellpadding="1" cellspacing="6" width="350px">
                            <tr>
                                <td class="plaintext" style="text-align: justify">El nombre de usuario (RUT) usado 
                es el mismo usado en Atentomático, si nunca has entrado a la central de aplicaciones, usa tu RUT y los ultimos 4 dígitos de éste sin contar dígito verificador para la clave.</td>
                            </tr>
                        </table>

                        <table border="0" cellpadding="0" id="tblLogin" cellspacing="8" width="350px">
                            <tr>
                                <td align="right" class="titulo">RUT:
                                </td>
                                <td align="left">
                                    <telerik:RadTextBox ID="txtUserName" runat="server" Skin="Office2007"
                                        InvalidStyleDuration="100" LabelCssClass="radLabelCss_Hay" Width="125px">
                                        <ClientEvents OnBlur="txtRUT_OnBlur" OnKeyPress="rutFilter" />
                                    </telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="txtUserName"
                                        ErrorMessage="El RUT es requerido." ToolTip="El RUT es requerido.">*</asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvRut" runat="server" ClientValidationFunction="cvValidateRut"
                                        ControlToValidate="txtUserName" ErrorMessage="El rut no es válido">*</asp:CustomValidator>
                                </td>
                            </tr>
                            <tr style="color: #000000">
                                <td align="right" class="titulo">Password:
                                </td>
                                <td align="left">
                                    <telerik:RadTextBox ID="txtPassword" runat="server" Skin="Office2007"
                                        TextMode="Password" SelectionOnFocus="SelectAll" Width="125px">
                                    </telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="txtPassword"
                                        ErrorMessage="El Password es requerido." ToolTip="El Password es requerido.">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1" align="right" valign="middle">&nbsp;</td>
                                <td>
                                    <asp:CheckBox ID="RememberMe" runat="server" Text="Recuérdame la próxima vez." ForeColor="Gray"
                                        Font-Size="Small" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="1"></td>
                                <td>
                                    <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="Log In"
                                        CssClass="button" Width="80px" /></td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="color: red">

                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>

                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server"
            ShowMessageBox="True" ShowSummary="False" />
    </form>
</body>
</html>

