<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="InscripcionServicioIA.aspx.cs" Inherits="AtentoDataBridge.Paginas.InscripcionServicioIA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content" style="padding: 20px;">
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title"><i class="fa fa-robot"></i> Registro de Servicio IA para RPA</h3>
            </div>
            
            <div class="box-body">
                <div class="row">
                    <div class="col-md-6 form-group">
                        <label>Nombre del Servicio (CONF_CNOMBRE_SERVICIO):</label>
                        <asp:TextBox ID="txtNombreServicio" runat="server" CssClass="form-control" placeholder="Ej: Transcriptor de Audio Azure"></asp:TextBox>
                    </div>
                    <div class="col-md-6 form-group">
                        <label>Plataforma Origen (CONF_NPLAT_NID):</label>
                        <asp:DropDownList ID="ddlPlataforma" runat="server" CssClass="form-control">
                            <asp:ListItem Text="-- Seleccione Plataforma --" Value="0" />
                            <asp:ListItem Text="Five9" Value="1" />
                            <asp:ListItem Text="Genesys" Value="2" />
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-4 form-group">
                        <label>Customer ID (CONF_CCUSTOMER):</label>
                        <asp:TextBox ID="txtCustomer" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-4 form-group">
                        <label>Tipo de Servicio:</label>
                        <asp:DropDownList ID="ddlTipoServicio" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Interno" Value="1" />
                            <asp:ListItem Text="Externo" Value="2" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4 form-group">
                        <label>% Muestreo (CONF_PORCENTAJE_MUESTREO):</label>
                        <asp:TextBox ID="txtMuestreo" runat="server" CssClass="form-control" TextMode="Number" step="0.01"></asp:TextBox>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6 form-group">
                        <label>Inicio Vigencia:</label>
                        <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="col-md-6 form-group">
                        <label>Estado del Registro:</label><br />
                        <asp:CheckBox ID="chkEstado" runat="server" Text=" &nbsp;Activo" Checked="true" />
                    </div>
                </div>

                <hr />
                <h4><i class="fa fa-cogs"></i> Parámetros de Carga (Opcional)</h4>
                <div class="row">
                    <div class="col-md-6 form-group">
                        <label>Ruta FTP / Almacenamiento:</label>
                        <asp:TextBox ID="txtRuta" runat="server" CssClass="form-control" placeholder="C:\RPA\Audios\"></asp:TextBox>
                    </div>
                    <div class="col-md-6 form-group">
                        <label>Password / Token:</label>
                        <asp:TextBox ID="txtToken" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="box-footer">
                <asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-default">
                    <i class="fa fa-close"></i> Cancelar
                </asp:LinkButton>
                <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-primary pull-right">
                    <i class="fa fa-save"></i> Inscribir Servicio
                </asp:LinkButton>
            </div>
        </div>
    </section>
</asp:Content>