﻿using System;
using System.Collections.Generic;

namespace bd.sw.externos.ModelosFM
{
    public partial class CapacitacionRespuesta
    {
        public CapacitacionRespuesta()
        {
            CapacitacionPregunta = new HashSet<CapacitacionPregunta>();
        }

        public int IdCapacitacionRespuesta { get; set; }
        public string Descripcion { get; set; }
        public bool EsCorrecta { get; set; }

        public virtual ICollection<CapacitacionPregunta> CapacitacionPregunta { get; set; }
    }
}