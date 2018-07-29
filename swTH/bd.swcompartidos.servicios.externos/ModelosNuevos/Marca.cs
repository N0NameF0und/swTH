﻿using System;
using System.Collections.Generic;

namespace bd.sw.externos.ModelosNuevos
{
    public partial class Marca
    {
        public Marca()
        {
            Modelo = new HashSet<Modelo>();
        }

        public int IdMarca { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Modelo> Modelo { get; set; }
    }
}
