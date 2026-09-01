using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontendSoporte.SoporteServiceReference;

namespace FrontendSoporte
{
    public partial class Tickets : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarGrid();
            }
        }

        private void CargarGrid()
        {
            EjecutarAccionSegura(() =>
            {
                using (var client = new SoporteServiceSoapClient())
                {
                    var tickets = client.ObtenerTickets();
                    gvTickets.DataSource = tickets;
                    gvTickets.DataBind();
                }
            });
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            EjecutarAccionSegura(() =>
            {
                using (var client = new SoporteServiceSoapClient())
                {
                    int idTicket = Convert.ToInt32(hfIdTicket.Value);
                    bool exito;

                    if (idTicket == 0)
                    {
                        exito = client.InsertarTicket(
                            txtSolicitante.Text.Trim(),
                            txtDepartamento.Text.Trim(),
                            txtAsunto.Text.Trim(),
                            txtDescripcion.Text.Trim(),
                            ddlPrioridad.SelectedValue,
                            ddlEstado.SelectedValue
                        );
                        MostrarMensaje(exito ? "Ticket registrado correctamente." : "No se pudo registrar el ticket.");
                    }
                    else
                    {
                        exito = client.ActualizarTicket(
                            idTicket,
                            txtSolicitante.Text.Trim(),
                            txtDepartamento.Text.Trim(),
                            txtAsunto.Text.Trim(),
                            txtDescripcion.Text.Trim(),
                            ddlPrioridad.SelectedValue,
                            ddlEstado.SelectedValue
                        );
                        MostrarMensaje(exito ? "Ticket actualizado correctamente." : "No se pudo actualizar el ticket.");
                    }

                    if (exito)
                    {
                        LimpiarFormulario();
                        CargarGrid();
                    }
                }
            });
        }

        protected void gvTickets_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idTicket = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                EjecutarAccionSegura(() =>
                {
                    using (var client = new SoporteServiceSoapClient())
                    {
                        var ticket = client.ObtenerTicketPorId(idTicket);
                        if (ticket != null)
                        {
                            hfIdTicket.Value = ticket.IdTicket.ToString();
                            txtSolicitante.Text = ticket.Solicitante;
                            txtDepartamento.Text = ticket.Departamento;
                            txtAsunto.Text = ticket.Asunto;
                            txtDescripcion.Text = ticket.Descripcion;
                            ddlPrioridad.SelectedValue = ticket.Prioridad;
                            ddlEstado.SelectedValue = ticket.Estado;

                            lblTituloForm.Text = "Editar Ticket #" + ticket.IdTicket;
                            btnCancelar.Visible = true;
                        }
                    }
                });
            }
            else if (e.CommandName == "Eliminar")
            {
                EjecutarAccionSegura(() =>
                {
                    using (var client = new SoporteServiceSoapClient())
                    {
                        bool exito = client.EliminarTicket(idTicket);
                        MostrarMensaje(exito ? "Ticket eliminado correctamente." : "No se pudo eliminar el ticket.");
                        CargarGrid();
                    }
                });
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtBuscarId.Text.Trim(), out int idTicket))
            {
                EjecutarAccionSegura(() =>
                {
                    using (var client = new SoporteServiceSoapClient())
                    {
                        var ticket = client.ObtenerTicketPorId(idTicket);
                        if (ticket != null)
                        {
                            gvTickets.DataSource = new Ticket[] { ticket };
                            gvTickets.DataBind();
                            MostrarMensaje("Ticket encontrado.");
                        }
                        else
                        {
                            gvTickets.DataSource = null;
                            gvTickets.DataBind();
                            MostrarMensaje("No existe un ticket con el ID indicado.");
                        }
                    }
                });
            }
            else
            {
                MostrarMensaje("Ingrese un ID válido.");
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            string estado = ddlFiltroEstado.SelectedValue;
            if (estado == "Todos")
            {
                CargarGrid();
                return;
            }

            EjecutarAccionSegura(() =>
            {
                using (var client = new SoporteServiceSoapClient())
                {
                    var tickets = client.ObtenerTicketsPorEstado(estado);
                    gvTickets.DataSource = tickets;
                    gvTickets.DataBind();
                    MostrarMensaje($"Filtrando por estado: {estado}");
                }
            });
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBuscarId.Text = string.Empty;
            ddlFiltroEstado.SelectedValue = "Todos";
            CargarGrid();
            lblMensaje.Visible = false;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtSolicitante.Text) ||
                string.IsNullOrWhiteSpace(txtDepartamento.Text) ||
                string.IsNullOrWhiteSpace(txtAsunto.Text) ||
                string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MostrarMensaje("Todos los campos del formulario son obligatorios.");
                return false;
            }
            return true;
        }

        private void LimpiarFormulario()
        {
            hfIdTicket.Value = "0";
            txtSolicitante.Text = string.Empty;
            txtDepartamento.Text = string.Empty;
            txtAsunto.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            ddlPrioridad.SelectedIndex = 0;
            ddlEstado.SelectedIndex = 0;
            lblTituloForm.Text = "Registrar Ticket";
            btnCancelar.Visible = false;
        }

        private void MostrarMensaje(string texto)
        {
            lblMensaje.Text = texto;
            lblMensaje.Visible = true;
        }

        private void EjecutarAccionSegura(Action accion)
        {
            try
            {
                accion();
            }
            catch (Exception ex)
            {
                // Muestra la excepción exacta capturada en el servidor
                MostrarMensaje("Error detectado: " + ex.Message);
            }
        }
    }
}