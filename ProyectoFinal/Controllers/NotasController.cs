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

        [HttpGet]
        public JsonResult GetNotas(int idoferta, int idcl)
        {

            //Consulto si existen notas ya grabadas del curso y materia
            Nota nota = db.Nota.Where(x => x.IdOferta == idoferta && x.IdCargaLectiva == idcl).FirstOrDefault();

            //if (nota == null)
            //{
                var detsAlumnos = (from matr in db.Matricula.AsNoTracking()
                                  join alum in db.Alumno.AsNoTracking() on matr.IdAlumno equals alum.IdAlumno                              
                                  where matr.Estado == "A"
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
            //}

        }

        [HttpPost]
        public JsonResult GrabarNotas(List<Nota> dataNotas)
        {
            Nota consNota = db.Nota.AsNoTracking().Where(x => x.IdOferta == dataNotas.FirstOrDefault().IdOferta && x.IdCargaLectiva == dataNotas.FirstOrDefault().IdCargaLectiva && x.IdPeriodoLectivo == dataNotas.FirstOrDefault().IdPeriodoLectivo).FirstOrDefault();
            ProyectoFinalEntities dbGrabar = new ProyectoFinalEntities();

            if (consNota == null)
            {
                foreach (var item in dataNotas)
                {
                    dbGrabar.Entry(item).State = EntityState.Added;                    
                }
            }
            else
            {
                foreach (var item in dataNotas)
                {
                    dbGrabar.Entry(item).State = EntityState.Modified;
                }
            }

            dbGrabar.SaveChanges();

            return Json(new { dataNotas }, JsonRequestBehavior.AllowGet);
        }

    }
}