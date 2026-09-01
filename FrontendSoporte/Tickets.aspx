<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tickets.aspx.cs" Inherits="FrontendSoporte.Tickets" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Gestión de Tickets de Soporte Técnico</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 20px; background-color: #f4f6f9; }
        .container { width: 90%; margin: auto; background: white; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0,0,0,0.1); }
        h2 { color: #333; text-align: center; }
        .form-group { margin-bottom: 12px; }
        label { display: inline-block; width: 120px; font-weight: bold; }
        input[type="text"], select, textarea { width: 300px; padding: 6px; border: 1px solid #ccc; border-radius: 4px; }
        .btn { padding: 8px 15px; border: none; border-radius: 4px; cursor: pointer; color: white; background-color: #007bff; }
        .btn-danger { background-color: #dc3545; }
        .btn-secondary { background-color: #6c757d; }
        .grid-view { margin-top: 20px; width: 100%; border-collapse: collapse; }
        .grid-view th, .grid-view td { padding: 10px; border: 1px solid #ddd; text-align: left; }
        .grid-view th { background-color: #007bff; color: white; }
        .alert { padding: 10px; margin-top: 10px; border-radius: 4px; font-weight: bold; }
        .alert-info { background-color: #d1ecf1; color: #0c5460; }
        .section-box { border: 1px solid #ddd; padding: 15px; margin-bottom: 15px; border-radius: 5px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>SISTEMA DE GESTIÓN DE TICKETS</h2>

            <asp:Label ID="lblMensaje" runat="server" CssClass="alert alert-info" Visible="false"></asp:Label>

            <div class="section-box">
                <h3>Filtros y Búsqueda</h3>
                <div class="form-group">
                    <label>Buscar por ID:</label>
                    <asp:TextBox ID="txtBuscarId" runat="server" Width="100px"></asp:TextBox>
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn" OnClick="btnBuscar_Click" />
                </div>
                <div class="form-group">
                    <label>Filtrar Estado:</label>
                    <asp:DropDownList ID="ddlFiltroEstado" runat="server">
                        <asp:ListItem Text="-- Todos --" Value="Todos"></asp:ListItem>
                        <asp:ListItem Text="Pendiente" Value="Pendiente"></asp:ListItem>
                        <asp:ListItem Text="En Proceso" Value="En Proceso"></asp:ListItem>
                        <asp:ListItem Text="Resuelto" Value="Resuelto"></asp:ListItem>
                        <asp:ListItem Text="Cerrado" Value="Cerrado"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-secondary" OnClick="btnFiltrar_Click" />
                    <asp:Button ID="btnLimpiar" runat="server" Text="Mostrar Todos" CssClass="btn btn-secondary" OnClick="btnLimpiar_Click" />
                </div>
            </div>

            <div class="section-box">
                <h3><asp:Label ID="lblTituloForm" runat="server" Text="Registrar Ticket"></asp:Label></h3>
                <asp:HiddenField ID="hfIdTicket" runat="server" Value="0" />
                
                <div class="form-group">
                    <label>Solicitante:</label>
                    <asp:TextBox ID="txtSolicitante" runat="server"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Departamento:</label>
                    <asp:TextBox ID="txtDepartamento" runat="server"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Asunto:</label>
                    <asp:TextBox ID="txtAsunto" runat="server"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Descripción:</label>
                    <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Prioridad:</label>
                    <asp:DropDownList ID="ddlPrioridad" runat="server">
                        <asp:ListItem Text="Baja" Value="Baja"></asp:ListItem>
                        <asp:ListItem Text="Media" Value="Media"></asp:ListItem>
                        <asp:ListItem Text="Alta" Value="Alta"></asp:ListItem>
                        <asp:ListItem Text="Crítica" Value="Crítica"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label>Estado:</label>
                    <asp:DropDownList ID="ddlEstado" runat="server">
                        <asp:ListItem Text="Pendiente" Value="Pendiente"></asp:ListItem>
                        <asp:ListItem Text="En Proceso" Value="En Proceso"></asp:ListItem>
                        <asp:ListItem Text="Resuelto" Value="Resuelto"></asp:ListItem>
                        <asp:ListItem Text="Cerrado" Value="Cerrado"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn" OnClick="btnGuardar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary" OnClick="btnCancelar_Click" Visible="false" />
                </div>
            </div>

            <div class="section-box">
                <h3>Listado de Tickets</h3>
                <asp:GridView ID="gvTickets" runat="server" AutoGenerateColumns="False" CssClass="grid-view" DataKeyNames="IdTicket" OnRowCommand="gvTickets_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="IdTicket" HeaderText="ID" />
                        <asp:BoundField DataField="Solicitante" HeaderText="Solicitante" />
                        <asp:BoundField DataField="Departamento" HeaderText="Departamento" />
                        <asp:BoundField DataField="Asunto" HeaderText="Asunto" />
                        <asp:BoundField DataField="Prioridad" HeaderText="Prioridad" />
                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                        <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha Registro" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:Button ID="btnEditar" runat="server" Text="Editar" CommandName="Editar" CommandArgument='<%# Eval("IdTicket") %>' CssClass="btn" />
                                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CommandName="Eliminar" CommandArgument='<%# Eval("IdTicket") %>' CssClass="btn btn-danger" OnClientClick="return confirm('¿Está seguro que desea eliminar este ticket?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>