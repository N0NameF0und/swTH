﻿using System;
using System.Collections.Generic;

namespace bd.sw.externos.ModelosNuevos
{
    public partial class EstadoCivil
    {
        public EstadoCivil()
        {
            Persona = new HashSet<Persona>();
        }

        public int IdEstadoCivil { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Persona> Persona { get; set; }
    }
}
