using System;

namespace BackendSoporte
{
    public class Ticket
    {
        public int IdTicket { get; set; }
        public string Solicitante { get; set; }
        public string Departamento { get; set; }
        public string Asunto { get; set; }
        public string Descripcion { get; set; }
        public string Prioridad { get; set; }
        public string Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}