using ProyectoFinalModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class PeriodoLectivoController : Controller
    {

        ProyectoFinalEntities db = new ProyectoFinalEntities();
        String error = "";

        // GET: PeriodoLectivo
        public ActionResult Index()
        {

            List<PeriodoLectivo> profesores = db.PeriodoLectivo.AsNoTracking().ToList();
            return View(profesores);
        }

        public ActionResult Edit(int id)
        {
            PeriodoLectivo objPeriodoLectivo = new PeriodoLectivo();
            objPeriodoLectivo = db.PeriodoLectivo.AsNoTracking().Where(x => x.IdPeriodoLectivo == id).FirstOrDefault();
            return View(objPeriodoLectivo);
        }

        public ActionResult Create()
        {
            PeriodoLectivo objPeriodoLectivo = new PeriodoLectivo();
            return View(objPeriodoLectivo);
        }

        //[HttpPost]
        //public JsonResult Create(string Descripcion, string Estado)
        //{
        //    return Json(new { Message = "a", bResultado = true }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult ConsultarPeriodoLectivos()
        {
            ProyectoFinalEntities db = new ProyectoFinalEntities();
            List<PeriodoLectivo> listPeriodoLectivo = new List<PeriodoLectivo>();

            listPeriodoLectivo = db.PeriodoLectivo.ToList();

            return Json(new { data = listPeriodoLectivo }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(int IdPeriodoLectivo, string Descripcion, DateTime FechaInicio, DateTime FechaFin, string Estado)
        {
            string strResult = string.Empty;
            bool bResult = false;

            ProyectoFinalEntities dbGrabar = new ProyectoFinalEntities();

            if (esValido(Descripcion, FechaInicio, FechaFin, Estado))
            {
                PeriodoLectivo objPeriodoLectivo = new PeriodoLectivo();
                objPeriodoLectivo.Descripcion = Descripcion;
                objPeriodoLectivo.FechaInicio = FechaInicio;
                objPeriodoLectivo.FechaFin = FechaFin;
                objPeriodoLectivo.Estado = Estado;

                if (IdPeriodoLectivo == 0)
                {
                    var model = (from per in db.PeriodoLectivo.AsNoTracking()
                                 where per.Estado.Contains("A")
                                 select new
                                 {
                                     per.Estado
                                 }).ToList().FirstOrDefault();

                    if (model != null && Estado.Equals("A"))
                    {
                        bResult = false;
                        strResult = "Ya Existe un Periodo Lectivo Activo";
                    }
                    else
                    {
                        List<PeriodoLectivo> profesores = new List<PeriodoLectivo>();
                        profesores = db.PeriodoLectivo.AsNoTracking().ToList();
                        objPeriodoLectivo.IdPeriodoLectivo = profesores.Count() == 0 ? 1 : profesores.Max(x => x.IdPeriodoLectivo) + 1;

                        objPeriodoLectivo.UsuarioCreacion = 1;
                        objPeriodoLectivo.FechaCreacion = DateTime.Now;
                        dbGrabar.Entry(objPeriodoLectivo).State = EntityState.Added;

                        dbGrabar.SaveChanges();

                        bResult = true;
                        strResult = "Datos Grabados Correctamente";
                    }
                }
                else
                {
                    var model = (from per in db.PeriodoLectivo.AsNoTracking()
                                 where per.Estado.Contains("A") && per.IdPeriodoLectivo != IdPeriodoLectivo
                                 select new
                                 {
                                     per.Estado
                                 }).ToList().FirstOrDefault();

                    if (model != null && Estado.Equals("A"))
                    {
                        bResult = false;
                        strResult = "Ya Existe un Periodo Lectivo Activo";
                    }
                    else
                    {
                        PeriodoLectivo consPeriodoLectivo = db.PeriodoLectivo.AsNoTracking().Where(x => x.IdPeriodoLectivo == IdPeriodoLectivo).FirstOrDefault();
                        objPeriodoLectivo.IdPeriodoLectivo = IdPeriodoLectivo;
                        objPeriodoLectivo.UsuarioCreacion = consPeriodoLectivo.UsuarioCreacion;
                        objPeriodoLectivo.FechaCreacion = consPeriodoLectivo.FechaCreacion;
                        objPeriodoLectivo.UsuarioActualizacion = 1;
                        objPeriodoLectivo.FechaActualizacion = DateTime.Now;
                        dbGrabar.Entry(objPeriodoLectivo).State = EntityState.Modified;

                        dbGrabar.SaveChanges();

                        bResult = true;
                        strResult = "Datos Grabados Correctamente";
                    }
                }
            }
            else
            {
                bResult = false;
                strResult = "Error al grabar el registro: " + error;
            }

            return Json(new { Message = strResult, bResultado = bResult }, JsonRequestBehavior.AllowGet);

        }

        public bool esValido(string Descripcion, DateTime FechaInicio, DateTime FechaFin, string Estado)
        {
            //Console.WriteLine(FechaInicio);
            if (String.IsNullOrEmpty(Descripcion))
            {
                error = "Debe ingresar la descripción";
                return false;
            }

            if (FechaInicio == Convert.ToDateTime("01/01/1900"))
            {
                error = "Debe ingresar la Fecha de Inicio";
                return false;
            }

            if (FechaFin == Convert.ToDateTime("01/01/1900"))
            {
                error = "Debe ingresar la Fecha Fin";
                return false;
            }
            return true;
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

    }
}