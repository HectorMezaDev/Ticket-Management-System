using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Services;

namespace BackendSoporte
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class SoporteService : WebService
    {
        private readonly string cadenaConexion = ConfigurationManager.ConnectionStrings["CadenaSoporte"].ConnectionString;

        [WebMethod]
        public List<Ticket> ObtenerTickets()
        {
            List<Ticket> lista = new List<Ticket>();
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                string query = "SELECT IdTicket, Solicitante, Departamento, Asunto, Descripcion, Prioridad, Estado, FechaRegistro FROM Tickets";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearTicket(dr));
                        }
                    }
                }
            }
            return lista;
        }

        [WebMethod]
        public Ticket ObtenerTicketPorId(int idTicket)
        {
            Ticket ticket = null;
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                string query = "SELECT IdTicket, Solicitante, Departamento, Asunto, Descripcion, Prioridad, Estado, FechaRegistro FROM Tickets WHERE IdTicket = @IdTicket";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@IdTicket", idTicket);
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            ticket = MapearTicket(dr);
                        }
                    }
                }
            }
            return ticket;
        }

        [WebMethod]
        public bool InsertarTicket(string solicitante, string departamento, string asunto, string descripcion, string prioridad, string estado)
        {
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                string query = @"INSERT INTO Tickets (Solicitante, Departamento, Asunto, Descripcion, Prioridad, Estado, FechaRegistro) 
                                VALUES (@Solicitante, @Departamento, @Asunto, @Descripcion, @Prioridad, @Estado, GETDATE())";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Solicitante", solicitante);
                    cmd.Parameters.AddWithValue("@Departamento", departamento);
                    cmd.Parameters.AddWithValue("@Asunto", asunto);
                    cmd.Parameters.AddWithValue("@Descripcion", descripcion);
                    cmd.Parameters.AddWithValue("@Prioridad", prioridad);
                    cmd.Parameters.AddWithValue("@Estado", estado);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        [WebMethod]
        public bool ActualizarTicket(int idTicket, string solicitante, string departamento, string asunto, string descripcion, string prioridad, string estado)
        {
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                string query = @"UPDATE Tickets 
                                SET Solicitante=@Solicitante, Departamento=@Departamento, Asunto=@Asunto, 
                                    Descripcion=@Descripcion, Prioridad=@Prioridad, Estado=@Estado 
                                WHERE IdTicket=@IdTicket";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@IdTicket", idTicket);
                    cmd.Parameters.AddWithValue("@Solicitante", solicitante);
                    cmd.Parameters.AddWithValue("@Departamento", departamento);
                    cmd.Parameters.AddWithValue("@Asunto", asunto);
                    cmd.Parameters.AddWithValue("@Descripcion", descripcion);
                    cmd.Parameters.AddWithValue("@Prioridad", prioridad);
                    cmd.Parameters.AddWithValue("@Estado", estado);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        [WebMethod]
        public bool EliminarTicket(int idTicket)
        {
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                string query = "DELETE FROM Tickets WHERE IdTicket = @IdTicket";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@IdTicket", idTicket);
                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        [WebMethod]
        public List<Ticket> ObtenerTicketsPorEstado(string estado)
        {
            List<Ticket> lista = new List<Ticket>();
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                string query = "SELECT IdTicket, Solicitante, Departamento, Asunto, Descripcion, Prioridad, Estado, FechaRegistro FROM Tickets WHERE Estado = @Estado";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Estado", estado);
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearTicket(dr));
                        }
                    }
                }
            }
            return lista;
        }

        private Ticket MapearTicket(SqlDataReader dr)
        {
            return new Ticket
            {
                IdTicket = Convert.ToInt32(dr["IdTicket"]),
                Solicitante = dr["Solicitante"].ToString(),
                Departamento = dr["Departamento"].ToString(),
                Asunto = dr["Asunto"].ToString(),
                Descripcion = dr["Descripcion"].ToString(),
                Prioridad = dr["Prioridad"].ToString(),
                Estado = dr["Estado"].ToString(),
                FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"])
            };
        }
    }
}