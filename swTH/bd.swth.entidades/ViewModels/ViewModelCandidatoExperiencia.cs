﻿using bd.swth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.swth.entidades.ViewModels
{
    public class ViewModelCandidatoExperiencia
    {
        public int Idcandidato { get; set; }
        public int Estado { get; set; }
        public string Nombres { get; set; }
        public string Cedula { get; set; }
        public string ExperienciaDias { get; set; }
        public string ExperienciaMesAno { get; set; }
        public string grupoOcupacional { get; set; }
        public DateTime FechaInico { get; set; }
        public DateTime FechaFin { get; set; }

    }
}
