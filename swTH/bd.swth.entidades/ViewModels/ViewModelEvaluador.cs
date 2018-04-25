﻿using bd.swth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.swth.entidades.ViewModels
{
    public class ViewModelEvaluador
    {
        public int IdEmpleado { get; set; }
        public string NombreApellido { get; set; }
        public string Puesto { get; set; }
        public string Titulo { get; set; }
        public string DatosJefe { get; set; }
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
        public string NombreUsuario { get; set; }
        public List<ActividadesEsenciales> ListaActividad { get; set; }
    }
}
