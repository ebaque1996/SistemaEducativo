using ProyectoFinalModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class NotasController : Controller
    {
        ProyectoFinalEntities db = new ProyectoFinalEntities();
        string error = "";
        // GET: Notas
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetCursos(string q)
        {

            var FatherItems = (from maq in db.Oferta.AsNoTracking()
                               join cur in db.Curso.AsNoTracking() on maq.IdCurso equals cur.IdCurso
                               join par in db.Paralelo.AsNoTracking() on maq.IdParalelo equals par.IdParalelo
                               where maq.Estado == "A"
                               select new { id = maq.IdOferta, text = cur.Descripcion + " " + par.Descripcion }
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

        public JsonResult GetMateriasPorOferta(string q, int idOferta)
        {

            var FatherItems = (from maq in db.CargaLectiva.AsNoTracking()
                               join cur in db.Materia.AsNoTracking() on maq.IdMateria equals cur.IdMateria                               
                               where maq.Estado == "A" && maq.IdOferta == idOferta
                               select new { id = maq.IdCargaLectiva, text = cur.Descripcion}
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
        public JsonResult GetNotas(int idoferta, int idcl, int idperiodo)
        {

            //Consulto si existen notas ya grabadas del curso y materia
            Nota nota = db.Nota.Where(x => x.IdOferta == idoferta && x.IdCargaLectiva == idcl && x.IdPeriodoLectivo == idperiodo).FirstOrDefault();

            if (nota == null)
            {
                var detsAlumnos = (from matr in db.Matricula.AsNoTracking()
                                   join alum in db.Alumno.AsNoTracking() on matr.IdAlumno equals alum.IdAlumno
                                   join ofer in db.Oferta.AsNoTracking() on matr.IdOferta equals ofer.IdOferta
                                   where matr.Estado == "A" && alum.Estado == "A" && matr.IdOferta == idoferta && ofer.IdPeriodoLectivo == idperiodo
                                   select new
                                  {
                                      matr.IdOferta,
                                      alum.IdAlumno,
                                      alum.Cedula,
                                      //alum.Nombres,
                                      //alum.Apellidos,
                                      Alumno = alum.Apellidos + " " + alum.Nombres,
                                      PpQ1 = 0.00,
                                      SpQ1 = 0.00,
                                      TpQ1 = 0.00,
                                      SumQ1 = 0.00,
                                      PromQ1 = 0.00,
                                      OchenPorQ1 = 0.00,
                                      ExQ1 = 0.00,
                                      VeinQ1 = 0.00,
                                      PromTotQ1 = 0.00,
                                      PpQ2 = 0.00,
                                      SpQ2 = 0.00,
                                      TpQ2 = 0.00,
                                      SumQ2 = 0.00,
                                      PromQ2 = 0.00,
                                      OchenPorQ2 = 0.00,
                                      ExQ2 = 0.00,
                                      VeinQ2 = 0.00,
                                      PromTotQ2 = 0.00,
                                      Total = 0.00
                                  }).ToList();

                return Json(new { detsAlumnos }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var detsAlumnos = (from matr in db.Matricula.AsNoTracking()
                                   join alum in db.Alumno.AsNoTracking() on matr.IdAlumno equals alum.IdAlumno
                                   join notas in db.Nota.AsNoTracking() on matr.IdAlumno equals notas.IdAlumno
                                   where matr.Estado == "A" && alum.Estado == "A"
                                   && nota.IdOferta == idoferta && nota.IdCargaLectiva == idcl && nota.IdPeriodoLectivo == idperiodo
                                   select new
                                   {
                                       matr.IdOferta,
                                       alum.IdAlumno,
                                       alum.Cedula,                                       
                                       Alumno = alum.Apellidos + " " + alum.Nombres,
                                       notas.PpQ1,
                                       notas.SpQ1,
                                       notas.TpQ1,
                                       notas.SumQ1,
                                       notas.PromQ1,
                                       notas.OchenPorQ1,
                                       notas.ExQ1,
                                       notas.VeinQ1,
                                       notas.PromTotQ1,
                                       notas.PpQ2,
                                       notas.SpQ2,
                                       notas.TpQ2,
                                       notas.SumQ2,
                                       notas.PromQ2,
                                       notas.OchenPorQ2,
                                       notas.ExQ2,
                                       notas.VeinQ2,
                                       notas.PromTotQ2,
                                       notas.Total
                                   }).ToList();

                return Json(new { detsAlumnos }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult GrabarNotas(List<Nota> dataNotas)
        {

            string strResult = string.Empty;
            bool bResult = false;

            if (esValido(dataNotas))
            {
                try
                {
                    ProyectoFinalEntities dbGrabar = new ProyectoFinalEntities();

                    foreach (var item in dataNotas)
                    {
                        Nota consNota = db.Nota.AsNoTracking().Where(x => x.IdOferta == item.IdOferta && x.IdCargaLectiva == item.IdCargaLectiva && x.IdAlumno == item.IdAlumno && x.IdPeriodoLectivo == item.IdPeriodoLectivo).FirstOrDefault();

                        if (consNota == null)
                        {
                            dbGrabar.Entry(item).State = EntityState.Added;
                        }
                        else
                        {
                            dbGrabar.Entry(item).State = EntityState.Modified;
                        }
                    }

                    dbGrabar.SaveChanges();
                    bResult = true;
                    strResult = "Datos Grabados Correctamente";

                }
                catch (Exception ex)
                {
                    bResult = false;
                    strResult = "Error: " + ex.Message;
                }
            }
            else
            {
                bResult = false;
                strResult = "Error: " + error;
            }            

            return Json(new { Message = strResult, bResultado = bResult }, JsonRequestBehavior.AllowGet);

        }

        public bool esValido(List<Nota> dataNotas)
        {

            if (dataNotas == null)
            {
                error = "Debe ingresar todos los datos";
                return false;
            }

            if (dataNotas.FirstOrDefault().IdOferta == 0)
            {
                error = "Debe ingresar el curso";
                return false;
            }

            if (dataNotas.FirstOrDefault().IdCargaLectiva == 0)
            {
                error = "Debe ingresar la materia";
                return false;
            }

            return true;
        }

    }
}