using ProyectoFinalModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class CargaLectivaExt:CargaLectiva
    {
        public string DescProfesor { get; set; }
        public string DescOferta { get; set; }
        public string DescPerLec { get; set; }
        public string DescMateria { get; set; }
    }
}