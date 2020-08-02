using ProyectoFinal.Models;
using ProyectoFinalModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class OfertaController : Controller
    {
        ProyectoFinalEntities db = new ProyectoFinalEntities();
        string error = "";
        // GET: Oferta
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetPeriodos(string q)
        {

            var FatherItems = (from maq in db.PeriodoLectivo.AsNoTracking()
                               where maq.Estado == "A"
                               select new { id = maq.IdPeriodoLectivo, text = maq.Descripcion }
                   ).ToList();

            FatherItems.RemoveAll(item => item == null);

            if (string.IsNullOrEmpty(q))
            {
                return Json(new { items = FatherItems }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var FatherItem = (from maq in FatherItems
                                  where maq.text.Contains(q)
                                  select new { id = maq.id, text = maq.text }
                     ).ToList();


                FatherItem.RemoveAll(item => item == null);

                return Json(new { items = FatherItem }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult GetProfesores(string q)
        {

            var FatherItems = (from maq in db.Profesor.AsNoTracking()
                               where maq.Estado == "A"
                               select new { id = maq.IdProfesor, text = maq.Nombres + " " + maq.Apellidos }
                   ).ToList();

            FatherItems.RemoveAll(item => item == null);

            if (string.IsNullOrEmpty(q))
            {
                return Json(new { items = FatherItems }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var FatherItem = (from maq in FatherItems
                                  where maq.text.Contains(q)
                                  select new { id = maq.id, text = maq.text }
                     ).ToList();


                FatherItem.RemoveAll(item => item == null);

                return Json(new { items = FatherItem }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetCursos(string q)
        {

            var FatherItems = (from maq in db.Curso.AsNoTracking()
                               where maq.Estado == "A"
                               select new { id = maq.IdCurso, text = maq.Descripcion }
                   ).ToList();

            FatherItems.RemoveAll(item => item == null);

            if (string.IsNullOrEmpty(q))
            {
                return Json(new { items = FatherItems }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var FatherItem = (from maq in FatherItems
                                  where maq.text.Contains(q)
                                  select new { id = maq.id, text = maq.text }
                     ).ToList();


                FatherItem.RemoveAll(item => item == null);

                return Json(new { items = FatherItem }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetParalelos(string q)
        {

            var FatherItems = (from maq in db.Paralelo.AsNoTracking()
                               where maq.Estado == "A"
                               select new { id = maq.IdParalelo, text = maq.Descripcion }
                   ).ToList();

            FatherItems.RemoveAll(item => item == null);

            if (string.IsNullOrEmpty(q))
            {
                return Json(new { items = FatherItems }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var FatherItem = (from maq in FatherItems
                                  where maq.text.Contains(q)
                                  select new { id = maq.id, text = maq.text }
                     ).ToList();


                FatherItem.RemoveAll(item => item == null);

                return Json(new { items = FatherItem }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetJornadas(string q)
        {

            var Jornadas = new List<object>().Select(t => new {
                id = default(string),
                text = default(string)
            }).ToList();

            Jornadas.Add(new { id = "MAT", text = "MATUTINA" });
            Jornadas.Add(new { id = "VES", text = "VESPERTINA" });

            var FatherItem = Jornadas.Where(x => x.text.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) >= 0);

            return Json(new { items = FatherItem }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDetalle(int idPer)
        {
            List<Oferta> listOfertas = new List<Oferta>();
            listOfertas = (from ofer in db.Oferta.AsNoTracking()
                           join cur in db.Curso.AsNoTracking() on ofer.IdCurso equals cur.IdCurso
                           join par in db.Paralelo.AsNoTracking() on ofer.IdParalelo equals par.IdParalelo
                           orderby cur.Nivel ascending, par.Descripcion ascending
                           select ofer).ToList();

            //List<Oferta> listOfertas = new List<Oferta>();
            //listOfertas = db.Oferta.AsNoTracking().Where(x => x.IdPeriodoLectivo == idPer).ToList();

            List<OfertaExt> listOfertaExt = new List<OfertaExt>();
            foreach (var item in listOfertas)
            {
                OfertaExt objOferExt = new OfertaExt();
                objOferExt.IdOferta = item.IdOferta;
                objOferExt.IdPeriodoLectivo = item.IdPeriodoLectivo;
                objOferExt.IdCurso = item.IdCurso;
                objOferExt.IdParalelo = item.IdParalelo;
                objOferExt.IdProfesor = item.IdProfesor;
                objOferExt.Capacidad = item.Capacidad;
                objOferExt.Ocupado = item.Ocupado;
                objOferExt.Disponible = item.Capacidad - item.Ocupado;
                objOferExt.Estado = item.Estado;
                objOferExt.DescPerLec = db.PeriodoLectivo.AsNoTracking().Where(x => x.IdPeriodoLectivo == item.IdPeriodoLectivo).FirstOrDefault().Descripcion;
                objOferExt.DescCurso = db.Curso.AsNoTracking().Where(x => x.IdCurso == item.IdCurso).FirstOrDefault().Descripcion;
                objOferExt.DescParalelo = db.Paralelo.AsNoTracking().Where(x => x.IdParalelo == item.IdParalelo).FirstOrDefault().Descripcion;
                objOferExt.DescJornada = item.Jornada == "MAT" ? "MATUTINA" : "VESPERTINA";
                objOferExt.DescProfesor = db.Profesor.AsNoTracking().Where(x => x.IdProfesor == item.IdProfesor).FirstOrDefault().Apellidos + " " + db.Profesor.AsNoTracking().Where(x => x.IdProfesor == item.IdProfesor).FirstOrDefault().Nombres;
                listOfertaExt.Add(objOferExt);
            }

            return Json(new { listOfertaExt }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult Create(int IdPeriodo, int IdCurso, int IdParalelo, int IdProfesor, string Jornada, int Capacidad)
        {
            string strResult = string.Empty;
            bool bResult = false;
            List<Oferta> listOfertas = new List<Oferta>();
            List<OfertaExt> listOfertaExt = new List<OfertaExt>();
            ProyectoFinalEntities dbGrabar = new ProyectoFinalEntities();

            try
            {
                if (esValido(IdPeriodo, IdCurso, IdParalelo, IdProfesor, Jornada, Capacidad))
                {
                    Oferta objOferta = new Oferta();
                    objOferta.IdPeriodoLectivo = IdPeriodo;
                    objOferta.IdCurso = IdCurso;
                    objOferta.IdParalelo = IdParalelo;
                    objOferta.IdProfesor = IdProfesor;
                    objOferta.Jornada = Jornada;
                    objOferta.Capacidad = Capacidad;
                    objOferta.Ocupado = 0;
                    objOferta.Estado = "A";

                    List<Oferta> ofertas = new List<Oferta>();
                    ofertas = db.Oferta.AsNoTracking().ToList();
                    objOferta.IdOferta = ofertas.Count() == 0 ? 1 : ofertas.Max(x => x.IdOferta) + 1;

                    objOferta.UsuarioCreacion = 1;
                    objOferta.FechaCreacion = DateTime.Now;
                    dbGrabar.Entry(objOferta).State = EntityState.Added;

                    dbGrabar.SaveChanges();

                    listOfertas = db.Oferta.AsNoTracking().Where(x => x.IdPeriodoLectivo == IdPeriodo).ToList();

                    foreach (var item in listOfertas)
                    {
                        OfertaExt objOferExt = new OfertaExt();
                        objOferExt.IdOferta = item.IdOferta;
                        objOferExt.IdPeriodoLectivo = item.IdPeriodoLectivo;
                        objOferExt.IdCurso = item.IdCurso;
                        objOferExt.IdParalelo = item.IdParalelo;
                        objOferExt.IdProfesor = item.IdProfesor;
                        objOferExt.Capacidad = item.Capacidad;
                        objOferExt.Ocupado = item.Ocupado;
                        objOferExt.Disponible = item.Capacidad - item.Ocupado;
                        objOferExt.Estado = item.Estado;
                        objOferExt.DescPerLec = db.PeriodoLectivo.AsNoTracking().Where(x => x.IdPeriodoLectivo == item.IdPeriodoLectivo).FirstOrDefault().Descripcion;
                        objOferExt.DescCurso = db.Curso.AsNoTracking().Where(x => x.IdCurso == item.IdCurso).FirstOrDefault().Descripcion;
                        objOferExt.DescParalelo = db.Paralelo.AsNoTracking().Where(x => x.IdParalelo == item.IdParalelo).FirstOrDefault().Descripcion;
                        objOferExt.DescJornada = item.Jornada == "MAT" ? "MATUTINA" : "VESPERTINA";
                        objOferExt.DescProfesor = db.Profesor.AsNoTracking().Where(x => x.IdProfesor == item.IdProfesor).FirstOrDefault().Apellidos + " " + db.Profesor.AsNoTracking().Where(x => x.IdProfesor == item.IdProfesor).FirstOrDefault().Nombres;
                        listOfertaExt.Add(objOferExt);
                    }

                    bResult = true;
                    strResult = "Datos Grabados Correctamente";
                }
                else
                {
                    bResult = false;
                    strResult = "Error al grabar el registro: " + error;
                }
            }
            catch (Exception ex)
            {
                bResult = false;
                strResult = "Error: " + ex.Message;
            }

            return Json(new { Message = strResult, bResultado = bResult, listOfertaExt }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        public JsonResult EditaOferta(int IdOferta, int Capacidad)
        {
            string strResult = string.Empty;
            bool bResult = false;
            ProyectoFinalEntities dbGrabar = new ProyectoFinalEntities();

            try
            {
                if (esValidoEditaOferta(IdOferta, Capacidad))
                {
                    Oferta objOferta = db.Oferta.AsNoTracking().Where(x => x.IdOferta == IdOferta).FirstOrDefault();
                    objOferta.Capacidad = Capacidad;

                    objOferta.UsuarioActualizacion = 1;
                    objOferta.FechaActualizacion = DateTime.Now;
                    dbGrabar.Entry(objOferta).State = EntityState.Modified;

                    dbGrabar.SaveChanges();

                    bResult = true;
                    strResult = "Datos Grabados Correctamente";
                }
                else
                {
                    bResult = false;
                    strResult = "Error al grabar el registro: " + error;
                }
            }
            catch (Exception ex)
            {
                bResult = false;
                strResult = "Error: " + ex.Message;
            }

            return Json(new { Message = strResult, bResultado = bResult }, JsonRequestBehavior.AllowGet);
        }

        public bool esValido(int IdPeriodo, int IdCurso, int IdParalelo, int IdProfesor, string Jornada, int Capacidad)
        {
            if (IdPeriodo == 0)
            {
                error = "Debe ingresar el periodo";
                return false;
            }

            if (IdCurso == 0)
            {
                error = "Debe ingresar el curso";
                return false;
            }

            if (IdParalelo == 0)
            {
                error = "Debe ingresar el paralelo";
                return false;
            }

            if (IdProfesor == 0)
            {
                error = "Debe ingresar el profesor";
                return false;
            }

            if (String.IsNullOrEmpty(Jornada))
            {
                error = "Debe ingresar la jornada";
                return false;
            }

            if (Capacidad == 0)
            {
                error = "Debe ingresar la capacidad";
                return false;
            }

            return true;
        }


        public bool esValidoEditaOferta(int IdOferta, int Capacidad)
        {
            if (IdOferta == 0)
            {
                error = "Oferta invalida";
                return false;
            }

            Oferta consOfer = db.Oferta.AsNoTracking().Where(x => x.IdOferta == IdOferta).FirstOrDefault();

            if (consOfer != null)
            {
                if (Capacidad < consOfer.Ocupado)
                {
                    error = "La capacidad es insuficiente para los estudiantes ya matriculados en este curso";
                    return false;
                }
            }
            else
            {
                error = "La oferta que desea editar no existe";
                return false;
            }

            return true;
        }


        [HttpGet]
        public PartialViewResult EditOferta(int IdOferta)
        {

            OfertaExt objOfertaExt = new OfertaExt();
            Oferta objOferta = db.Oferta.AsNoTracking().Where(x => x.IdOferta == IdOferta).FirstOrDefault();

            string PartialUrl = "~/Views/Partial/OfertaP/_EditaOF.cshtml";

            if (objOferta == null)
            {
                return PartialView(PartialUrl, objOfertaExt);
            }

            try
            {

                objOfertaExt.IdOferta = objOferta.IdOferta;
                objOfertaExt.IdPeriodoLectivo = objOferta.IdPeriodoLectivo;
                objOfertaExt.IdCurso = objOferta.IdCurso;
                objOfertaExt.IdParalelo = objOferta.IdParalelo;
                objOfertaExt.IdProfesor = objOferta.IdProfesor;
                objOfertaExt.Capacidad = objOferta.Capacidad;
                objOfertaExt.Ocupado = objOferta.Ocupado;
                objOfertaExt.Disponible = objOferta.Capacidad - objOferta.Ocupado;
                objOfertaExt.Estado = objOferta.Estado;
                objOfertaExt.DescPerLec = db.PeriodoLectivo.AsNoTracking().Where(x => x.IdPeriodoLectivo == objOferta.IdPeriodoLectivo).FirstOrDefault().Descripcion;
                objOfertaExt.DescCurso = db.Curso.AsNoTracking().Where(x => x.IdCurso == objOferta.IdCurso).FirstOrDefault().Descripcion;
                objOfertaExt.DescParalelo = db.Paralelo.AsNoTracking().Where(x => x.IdParalelo == objOferta.IdParalelo).FirstOrDefault().Descripcion;
                objOfertaExt.DescProfesor = db.Profesor.AsNoTracking().Where(x => x.IdProfesor == objOferta.IdProfesor).FirstOrDefault().Apellidos + " " + db.Profesor.AsNoTracking().Where(x => x.IdProfesor == objOferta.IdProfesor).FirstOrDefault().Nombres;

            }
            catch (Exception)
            {
                objOfertaExt = new OfertaExt();
            }

            return PartialView(PartialUrl, objOfertaExt);
        }

        public JsonResult DeleteOferta(int IdOferta)
        {
            string strResult = string.Empty;
            bool bResult = false;
            List<OfertaExt> listOfertaExt = new List<OfertaExt>();

            ProyectoFinalEntities dbGrabar = new ProyectoFinalEntities();

            try
            {
                Oferta consOferta = db.Oferta.AsNoTracking().Where(x => x.IdOferta == IdOferta).FirstOrDefault();

                if (consOferta != null)
                {

                    //Si la oferta ya tiene algun estudiante matriculado, no se le permite borrar
                    if (consOferta.Ocupado == 0)
                    {
                        int idPerLec = consOferta.IdPeriodoLectivo;
                        dbGrabar.Entry(consOferta).State = EntityState.Deleted;

                        dbGrabar.SaveChanges();

                        List<Oferta> listOfertas = new List<Oferta>();
                        listOfertas = db.Oferta.AsNoTracking().Where(x => x.IdPeriodoLectivo == idPerLec).ToList();

                        //List<OfertaExt> listOfertaExt = new List<OfertaExt>();
                        foreach (var item in listOfertas)
                        {
                            OfertaExt objOferExt = new OfertaExt();
                            objOferExt.IdOferta = item.IdOferta;
                            objOferExt.IdPeriodoLectivo = item.IdPeriodoLectivo;
                            objOferExt.IdCurso = item.IdCurso;
                            objOferExt.IdParalelo = item.IdParalelo;
                            objOferExt.IdProfesor = item.IdProfesor;
                            objOferExt.Capacidad = item.Capacidad;
                            objOferExt.Ocupado = item.Ocupado;
                            objOferExt.Disponible = item.Capacidad - item.Ocupado;
                            objOferExt.Estado = item.Estado;
                            objOferExt.DescPerLec = db.PeriodoLectivo.AsNoTracking().Where(x => x.IdPeriodoLectivo == item.IdPeriodoLectivo).FirstOrDefault().Descripcion;
                            objOferExt.DescCurso = db.Curso.AsNoTracking().Where(x => x.IdCurso == item.IdCurso).FirstOrDefault().Descripcion;
                            objOferExt.DescParalelo = db.Paralelo.AsNoTracking().Where(x => x.IdParalelo == item.IdParalelo).FirstOrDefault().Descripcion;
                            objOferExt.DescProfesor = db.Profesor.AsNoTracking().Where(x => x.IdProfesor == item.IdProfesor).FirstOrDefault().Apellidos + " " + db.Profesor.AsNoTracking().Where(x => x.IdProfesor == item.IdProfesor).FirstOrDefault().Nombres;
                            listOfertaExt.Add(objOferExt);
                        }

                        bResult = true;
                        strResult = "Datos Eliminados Correctamente";
                    }
                    else
                    {
                        bResult = false;
                        strResult = "Error al eliminar el registro: No es posible eliminar una oferta que tiene estudiantes matriculados";
                    }

                }
                else
                {
                    bResult = false;
                    strResult = "Error al eliminar el registro: La oferta no existe";
                }
            }
            catch (Exception ex)
            {
                bResult = false;
                strResult = "Error al eliminar el registro: " + ex.Message;
            }

            return Json(new { Message = strResult, bResultado = bResult, listOfertaExt }, JsonRequestBehavior.AllowGet);
        }

    }
}