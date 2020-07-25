using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProyectoFinalModel;

namespace ProyectoFinal.Models
{
    public class OfertaExt: Oferta
    {
        public string DescPerLec { get; set; }
        public string DescCurso { get; set; }       
        public string DescParalelo { get; set; }
        public string DescProfesor { get; set; }
        public string DescJornada { get; set; }
        public int Disponible { get; set; }
    }
}