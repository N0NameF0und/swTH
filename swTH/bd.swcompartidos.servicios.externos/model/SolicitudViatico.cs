﻿using System;
using System.Collections.Generic;

namespace bd.sw.externos.model
{
    public partial class SolicitudViatico
    {
        public SolicitudViatico()
        {
            AprobacionViatico = new HashSet<AprobacionViatico>();
            ItinerarioViatico = new HashSet<ItinerarioViatico>();
            SolicitudTipoViatico = new HashSet<SolicitudTipoViatico>();
        }

        public int IdSolicitudViatico { get; set; }
        public int IdEmpleado { get; set; }
        public int IdPais { get; set; }
        public int IdProvincia { get; set; }
        public int IdCiudad { get; set; }
        public int IdConfiguracionViatico { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string Descripcion { get; set; }
        public decimal? ValorEstimado { get; set; }
        public DateTime FechaLlegada { get; set; }
        public DateTime FechaSalida { get; set; }
        public string Observacion { get; set; }
        public int Estado { get; set; }
        public TimeSpan HoraSalida { get; set; }
        public TimeSpan HoraLlegada { get; set; }

        public virtual ICollection<AprobacionViatico> AprobacionViatico { get; set; }
        public virtual ICollection<ItinerarioViatico> ItinerarioViatico { get; set; }
        public virtual ICollection<SolicitudTipoViatico> SolicitudTipoViatico { get; set; }
    }
}