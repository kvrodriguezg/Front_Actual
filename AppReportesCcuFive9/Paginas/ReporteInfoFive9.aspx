<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ReporteInfoFive9.aspx.cs" Inherits="AppReportesCcuFive9.Paginas.ReporteInfoFive9" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Csshead" runat="server">
    <!-- DataTables -->
    <link href='<%= Page.ResolveUrl("~/App_Themes/TemaCentral/AdminLte/bower_components/DataTables/datatables.min.css") %>' rel="stylesheet" />

    <style>
        /* Spinner overlay */
        #loading {
            display: none; /* Hidden by default */
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(255, 255, 255, 0.7);
            z-index: 9999;
            text-align: center;
        }

        /* Spinner animation */
        .spinner {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            border: 8px solid #f3f3f3;
            border-top: 8px solid #3498db;
            border-radius: 50%;
            width: 60px;
            height: 60px;
            animation: spin 1s linear infinite;
        }

        @keyframes spin {
            0% {
                transform: translate(-50%, -50%) rotate(0deg);
            }

            100% {
                transform: translate(-50%, -50%) rotate(360deg);
            }
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Jshead" runat="server">
    <!-- DataTables -->
    <script src='<%= Page.ResolveUrl("~/App_Themes/TemaCentral/AdminLte/bower_components/DataTables/datatables.min.js") %>'></script>

    <!-- FileSaver.min.js -->
    <script src='<%= Page.ResolveUrl("~/App_Themes/TemaCentral/js/FileSaver.min.js") %>'></script>


    <script>
        $(function () {
            $('.select2').select2({
                language: "es"
            });

            initPluginTabla();
        })


        // Inicialización reutilizable
        function initPluginTabla() {
            var $tabla = $('.pluginTabla');

            // Evitar doble inicialización
            if ($.fn.DataTable.isDataTable($tabla)) {
                $tabla.DataTable().destroy();
            }

            $tabla.DataTable({
                paging: true,
                lengthChange: true,
                searching: true,
                ordering: true,
                info: true,
                autoWidth: false,
                // Si tu URL es ASP.NET, conviene injectarla desde server.
                language: {
                    url: '<%= Page.ResolveUrl("~/App_Themes/TemaCentral/AdminLte/bower_components/DataTables/es-CL.json") %>'
                }
            });
        }

        function DownloadExcelFile() {
            console.log("entra a DownloadExcelFile");
            try {
                var fileName = "Casos";

                var request = $.ajax({
                    type: "POST",
                    url: "ReporteInfoFive9.aspx/DescargarExcelInfoCcuFive9",
                    //data: JSON.stringify(obj),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (r) {
                        resetLoadingButton();

                        if (r.d != null) {
                            //Convert Base64 string to Byte Array.
                            var bytes = Base64ToBytes(r.d);

                            //Convert Byte Array to BLOB.
                            var blob = new Blob([bytes], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });

                            ////Check the Browser type and download the File.
                            var isIE = false || !!document.documentMode;

                            //se ejecuta FileSaver
                            saveAs(blob, "ReporteInfoCcuFive9.xlsx");
                        }
                        else {
                            BootstrapDialog.show({
                                type: BootstrapDialog.TYPE_WARNING,
                                title: 'Advertencia',
                                message: '<p>La busqueda no ha arrojado resultados.</p>'

                            });
                        }
                    }
                });

                request.fail(function (jqXHR, textStatus) {
                    resetLoadingButton();
                });
            }
            catch (err) {
                //console.log(err);
                resetLoadingButton();
            }
        }
        function Base64ToBytes(base64) {
            var s = window.atob(base64);
            var bytes = new Uint8Array(s.length);
            for (var i = 0; i < s.length; i++) {
                bytes[i] = s.charCodeAt(i);
            }
            return bytes;
        }
        function ShowLoading() {
            $("#loading").fadeIn();
        }
        function hideLoading() {
            $("#loading").fadeOut();
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="divReporteInformaciónFive9" class="box box-solid box-primary panelx">
        <div class="box-header with-border">
            <h2 class="panel-title text-center">Reporte Información Five9</h2>
            <div class="box-tools">
                <asp:LinkButton ID="btnReload" runat="server" CssClass="btn btn-default btn-sm btnLoading" CausesValidation="false" OnClientClick="ShowLoading();" OnClick="btnReload_Click">
                    <i class="fa fa-refresh"></i> 
                </asp:LinkButton>

                <button type="button" id="btnExportarExcel" class="btn btn-success btn-sm btnLoading" onclick="DownloadExcelFile();">
                    <i class="fa fa-file-excel-o"></i>&nbsp&nbsp Exportar a Excel
                </button>
                <!-- Button trigger modal -->
                <button type="button" class="btn bg-navy btn-sm" data-toggle="modal" data-target="#modalFiltro">
                    <i class="fa fa-filter"></i>Filtro
                </button>
            </div>
        </div>
        <div class="box-body">
            <div class="row">
                <div class="col-md-12">
                    <span class="pull-left-container">
                        <span class="label label-primary pull-left" style="font-size: medium;">Datos al :
                            <asp:Label ID="lblFechaReporte" runat="server" />
                        </span>
                    </span>
                </div>
            </div>
            <hr />
            <div class="row">
                <div class="col-md-12 table-responsive">
                    <table class="table table-bordered table-hover pluginTabla">
                        <thead>
                            <tr>
                                <th>Nombre Ejecutivo</th>
                                <th>Recorridos</th>
                                <th>No Recorridos</th>
                                <th>Contactados</th>
                                <th>No Contactados</th>
                                <th>Programados</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptDatosReporteCcuFive9" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("NombreEjecutivo") %></td>
                                        <td><%# Eval("Recorridos") %></td>
                                        <td><%# Eval("NoRecorridos") %></td>
                                        <td><%# Eval("Contactados") %></td>
                                        <td><%# Eval("NoContactados") %></td>
                                        <td><%# Eval("Programados") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modalFiltro" tabindex="-1000000000" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="box box-solid box-primary modal-content">
                <div class="box-header with-border modal-header">
                    <h4 class="modal-title">Filtros</h4>
                </div>
                <div class="box-body modal-body">
                    <div class="row">
                        <div class="col-md-9">
                            <div class="form-group">
                                <label>Nombre Ejecutivo</label>
                                <asp:DropDownList ID="ddlNombreEjecutivo" runat="server" CssClass="form-control select2" Width="100%" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <label>&nbsp&nbsp</label><br />
                                <asp:LinkButton ID="btnAgregarFiltro" runat="server" CssClass="btn btn-primary btn-flat" OnClick="btnAgregarFiltro_Click">
                                    <i class="fa fa-plus"></i>&nbsp;&nbsp;Agregar
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div class="row">
                        <div class="col-md-12">
                            <asp:UpdatePanel ID="upnlFiltroEjecutivo" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table class="table table-bordered table-hover ">
                                        <thead>
                                            <tr>
                                                <th>Nombre Ejecutivo</th>
                                                <th></th>
                                            </tr>
                                        </thead>
                                        <tbody>

                                            <asp:Repeater ID="rptFiltroEjecutivo" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><%# Eval("Filtro") %></td>
                                                         <td>
                                                             <asp:LinkButton ID="btnQuitarFiltro" runat="server" CssClass="btn btn-danger btn-flat btn-sm"
                                                                 CommandArgument='<%# Eval("IdFiltroReporteCcuFive9") %>'
                                                                 ToolTip="Quitar filtro" OnClick="btnQuitarFiltro_Click">
                                                             <i class="fa fa-remove"></i>
                                                             </asp:LinkButton>
                                                         </td>
                                                    </tr>                                                
                                                </ItemTemplate>
                                            </asp:Repeater>

                                        </tbody>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnAgregarFiltro" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="btnCerrarModal" runat="server" CssClass="btn btn-default btn-flat" OnClick="btnCerrarModal_Click">
                        <i class="fa fa-close"></i> &nbsp; &nbsp; Cerrar
                    </asp:LinkButton>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <!-- /.modal -->

    <script type="text/javascript">
        //Para que se ejcuten los scripts cuando se usa UpdatePanel.
        //Events being defined by the PageRequestManager
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //These functions will be handled by the cooresponding events declared above.
        function BeginRequestHandler(sender, args) {


        }
        function EndRequestHandler(sender, args) {
            $(function () {
                $('.select2').select2({
                    language: "es"
                });

                initPluginTabla();
            })
        }

    </script>
    <!-- Loading overlay -->
    <div id="loading">
        <div class="spinner"></div>
    </div>
</asp:Content>
