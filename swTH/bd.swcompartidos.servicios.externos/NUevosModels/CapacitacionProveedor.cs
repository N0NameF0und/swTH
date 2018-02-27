﻿using System;
using System.Collections.Generic;

namespace bd.sw.externos.NUevosModels
{
    public partial class CapacitacionProveedor
    {
        public CapacitacionProveedor()
        {
            CapacitacionTemarioProveedor = new HashSet<CapacitacionTemarioProveedor>();
        }

        public int IdCapacitacionProveedor { get; set; }
        public string Telefono { get; set; }
        public string Nombre { get; set; }
        public int? IdPais { get; set; }
        public string Direccion { get; set; }

        public virtual ICollection<CapacitacionTemarioProveedor> CapacitacionTemarioProveedor { get; set; }
        public virtual Pais IdPaisNavigation { get; set; }
    }
}
