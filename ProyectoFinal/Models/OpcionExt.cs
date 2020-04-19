using ProyectoFinalModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class OpcionExt: Opcion
    {
        public string DescModulo { get; set; }

        public string EstadoRolOpcion { get; set; }
    }
}