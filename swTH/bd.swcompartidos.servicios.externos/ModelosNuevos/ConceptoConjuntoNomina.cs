﻿using System;
using System.Collections.Generic;

namespace bd.sw.externos.ModelosNuevos
{
    public partial class ConceptoConjuntoNomina
    {
        public int IdConceptoConjunto { get; set; }
        public int IdConjunto { get; set; }
        public int IdConcepto { get; set; }
        public bool? Suma { get; set; }
        public bool? Resta { get; set; }

        public virtual ConceptoNomina IdConceptoNavigation { get; set; }
        public virtual ConjuntoNomina IdConjuntoNavigation { get; set; }
    }
}
