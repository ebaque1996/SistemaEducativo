using ProyectoFinalModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class NotaExt : Nota
    {
        public string Cedula { get; set; }        
        public string DescNombres { get; set; }
        public string DescApellidos { get; set; }        
    }
}