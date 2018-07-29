﻿using System;
using System.Collections.Generic;

namespace bd.sw.externos.ModelosNuevos
{
    public partial class SolicitudVacaciones
    {
        public int IdSolicitudVacaciones { get; set; }
        public int IdEmpleado { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public int Estado { get; set; }
        public string Observaciones { get; set; }
        public bool PlanAnual { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaRespuesta { get; set; }
        public string RazonNoPlanificado { get; set; }
        public bool RequiereReemplazo { get; set; }
        public int? IdEmpleadoReemplazo { get; set; }
    }
}
