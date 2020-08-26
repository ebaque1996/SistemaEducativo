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

        public JsonResult GetCursos(string q, int? idPerLec)
        {

            var FatherItems = new List<object>().Select(t => new {
                id = default(int),
                text = default(string),
                niv = default(int),
                par = default(string),
            }).ToList();
          
            int idrol = Convert.ToInt32(Request.Cookies["Rol"].Value.ToString());

            //Si es un profesor solo carga los cursos a los que el esta asignado, si no carga todos
            if (idrol == 3)
            {
                string IdUsuario = Request.Cookies["UserID"].Value.ToString();

                FatherItems = (from usu in db.Usuario.AsNoTracking()
                               join prof in db.Profesor.AsNoTracking() on usu.Cedula equals prof.Cedula
                               join clec in db.CargaLectiva.AsNoTracking() on prof.IdProfesor equals clec.IdProfesor
                               join ofer in db.Oferta.AsNoTracking() on clec.IdOferta equals ofer.IdOferta
                               join cur in db.Curso.AsNoTracking() on ofer.IdCurso equals cur.IdCurso
                               join par in db.Paralelo.AsNoTracking() on ofer.IdParalelo equals par.IdParalelo
                               where ofer.Estado == "A" && ofer.IdPeriodoLectivo == idPerLec && usu.IdUsuario == IdUsuario
                               //orderby cur.Nivel ascending, par.Descripcion ascending
                               //select new { id = ofer.IdOferta, text = cur.Descripcion + " " + par.Descripcion}
                               select new { id = ofer.IdOferta, text = cur.Descripcion + " " + par.Descripcion + " | " + (ofer.Jornada == "MAT" ? "MATUTINA" : "VESPERTINA"), niv = cur.Nivel, par = par.Descripcion}
                ).Distinct().ToList();

            }
            else
            {
                FatherItems = (from maq in db.Oferta.AsNoTracking()                                       
                               join cur in db.Curso.AsNoTracking() on maq.IdCurso equals cur.IdCurso
                               join par in db.Paralelo.AsNoTracking() on maq.IdParalelo equals par.IdParalelo
                               where maq.Estado == "A" && maq.IdPeriodoLectivo == idPerLec
                               //orderby cur.Nivel ascending, par.Descripcion ascending
                               //select new { id = maq.IdOferta, text = cur.Descripcion + " " + par.Descripcion }
                               select new { id = maq.IdOferta, text = cur.Descripcion + " " + par.Descripcion + " | " + (maq.Jornada == "MAT" ? "MATUTINA" : "VESPERTINA"), niv = cur.Nivel, par = par.Descripcion }
                ).Distinct().ToList();
            }

            FatherItems.RemoveAll(item => item == null);

            if (string.IsNullOrEmpty(q))
            {
                var FatherItem = (from maq in FatherItems
                                  //where maq.text.Contains(q)
                                  orderby maq.niv ascending, maq.par ascending
                                  select new { id = maq.id, text = maq.text }
                     ).ToList();

                return Json(new { items = FatherItem }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var FatherItem = (from maq in FatherItems
                                  where maq.text.Contains(q)
                                  orderby maq.niv ascending, maq.par ascending
                                  select new { id = maq.id, text = maq.text }
                     ).ToList();


                FatherItem.RemoveAll(item => item == null);

                return Json(new { items = FatherItem }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetMateriasPorOferta(string q, int? idOferta, int? idPerLec)
        {

            var FatherItems = new List<object>().Select(t => new {
                id = default(int),
                text = default(string)
            }).ToList();

            int idrol = Convert.ToInt32(Request.Cookies["Rol"].Value.ToString());

            //Si es un profesor solo carga los cursos a los que el esta asignado, si no carga todos
            if (idrol == 3)
            {
                string IdUsuario = Request.Cookies["UserID"].Value.ToString();

                FatherItems = (from usu in db.Usuario.AsNoTracking()
                               join prof in db.Profesor.AsNoTracking() on usu.Cedula equals prof.Cedula
                               join clec in db.CargaLectiva.AsNoTracking() on prof.IdProfesor equals clec.IdProfesor
                               join mat in db.Materia.AsNoTracking() on clec.IdMateria equals mat.IdMateria
                               //join cur in db.Curso.AsNoTracking() on ofer.IdCurso equals cur.IdCurso
                               //join par in db.Paralelo.AsNoTracking() on ofer.IdParalelo equals par.IdParalelo
                               where clec.Estado == "A" && clec.IdOferta == idOferta && clec.IdPeriodoLectivo == idPerLec && usu.IdUsuario == IdUsuario
                               select new { id = clec.IdCargaLectiva, text = mat.Descripcion }
                ).ToList();

            }
            else
            {   
                FatherItems = (from maq in db.CargaLectiva.AsNoTracking()
                               join cur in db.Materia.AsNoTracking() on maq.IdMateria equals cur.IdMateria
                               where maq.Estado == "A" && maq.IdOferta == idOferta && maq.IdPeriodoLectivo == idPerLec
                               select new { id = maq.IdCargaLectiva, text = cur.Descripcion }
                ).ToList();
            }






            //var FatherItems = (from maq in db.CargaLectiva.AsNoTracking()
            //                   join cur in db.Materia.AsNoTracking() on maq.IdMateria equals cur.IdMateria                               
            //                   where maq.Estado == "A" && maq.IdOferta == idOferta && maq.IdPeriodoLectivo == idPerLec
            //                   select new { id = maq.IdCargaLectiva, text = cur.Descripcion}
            //       ).ToList();

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
            //Nota nota = db.Nota.Where(x => x.IdOferta == idoferta && x.IdCargaLectiva == idcl && x.IdPeriodoLectivo == idperiodo).FirstOrDefault();
            decimal notaDef = Convert.ToDecimal(0.00);

            var detsAlumnos = (from matr in db.Matricula.AsNoTracking()
                               join alum in db.Alumno.AsNoTracking() on matr.IdAlumno equals alum.IdAlumno
                               join ofer in db.Oferta.AsNoTracking() on matr.IdOferta equals ofer.IdOferta

                               join clec in db.CargaLectiva.AsNoTracking() on ofer.IdOferta equals clec.IdOferta

                               join notas in db.Nota.AsNoTracking()// on matr.IdAlumno equals notas.IdAlumno 
                               on new { matr.IdAlumno, clec.IdCargaLectiva } equals new { notas.IdAlumno, notas.IdCargaLectiva }
                               //&& clec.IdCargaLectiva equals notas.IdCargaLectiva
                               into g
                               from notas in g.DefaultIfEmpty()
                               where matr.Estado == "A" && alum.Estado == "A" && matr.IdOferta == idoferta && ofer.IdPeriodoLectivo == idperiodo
                               && clec.IdCargaLectiva == idcl
                               select new
                               {
                                   matr.IdOferta,
                                   alum.IdAlumno,
                                   alum.Cedula,
                                   Alumno = alum.Apellidos + " " + alum.Nombres,
                                   PpQ1 = notas.PpQ1 != null ? notas.PpQ1 : notaDef,
                                   SpQ1 = notas.SpQ1 != null ? notas.SpQ1 : notaDef,
                                   TpQ1 = notas.TpQ1 != null ? notas.TpQ1 : notaDef,
                                   SumQ1 = notas.SumQ1 != null ? notas.SumQ1 : notaDef,
                                   PromQ1 = notas.PromQ1 != null ? notas.PromQ1 : notaDef,
                                   OchenPorQ1 = notas.OchenPorQ1 != null ? notas.OchenPorQ1 : notaDef,
                                   ExQ1 = notas.ExQ1 != null ? notas.ExQ1 : notaDef,
                                   VeinQ1 = notas.VeinQ1 != null ? notas.VeinQ1 : notaDef,
                                   PromTotQ1 = notas.PromTotQ1 != null ? notas.PromTotQ1 : notaDef,
                                   PpQ2 = notas.PpQ2 != null ? notas.PpQ2 : notaDef,
                                   SpQ2 = notas.SpQ2 != null ? notas.SpQ2 : notaDef,
                                   TpQ2 = notas.TpQ2 != null ? notas.TpQ2 : notaDef,
                                   SumQ2 = notas.SumQ2 != null ? notas.SumQ2 : notaDef,
                                   PromQ2 = notas.PromQ2 != null ? notas.PromQ2 : notaDef,
                                   OchenPorQ2 = notas.OchenPorQ2 != null ? notas.OchenPorQ2 : notaDef,
                                   ExQ2 = notas.ExQ2 != null ? notas.ExQ2 : notaDef,
                                   VeinQ2 = notas.VeinQ2 != null ? notas.VeinQ2 : notaDef,
                                   PromTotQ2 = notas.PromTotQ2 != null ? notas.PromTotQ2 : notaDef,
                                   Total = notas.Total != null ? notas.Total : notaDef,                                                              
                               }).ToList();

            //detsAlumnos =

            return Json(new { detsAlumnos }, JsonRequestBehavior.AllowGet);

            //if (nota == null)
            //{
            //    var detsAlumnos = (from matr in db.Matricula.AsNoTracking()
            //                       join alum in db.Alumno.AsNoTracking() on matr.IdAlumno equals alum.IdAlumno
            //                       join ofer in db.Oferta.AsNoTracking() on matr.IdOferta equals ofer.IdOferta
            //                       where matr.Estado == "A" && alum.Estado == "A" && matr.IdOferta == idoferta && ofer.IdPeriodoLectivo == idperiodo
            //                       select new
            //                      {
            //                          matr.IdOferta,
            //                          alum.IdAlumno,
            //                          alum.Cedula,
            //                          //alum.Nombres,
            //                          //alum.Apellidos,
            //                          Alumno = alum.Apellidos + " " + alum.Nombres,
            //                          PpQ1 = 0.00,
            //                          SpQ1 = 0.00,
            //                          TpQ1 = 0.00,
            //                          SumQ1 = 0.00,
            //                          PromQ1 = 0.00,
            //                          OchenPorQ1 = 0.00,
            //                          ExQ1 = 0.00,
            //                          VeinQ1 = 0.00,
            //                          PromTotQ1 = 0.00,
            //                          PpQ2 = 0.00,
            //                          SpQ2 = 0.00,
            //                          TpQ2 = 0.00,
            //                          SumQ2 = 0.00,
            //                          PromQ2 = 0.00,
            //                          OchenPorQ2 = 0.00,
            //                          ExQ2 = 0.00,
            //                          VeinQ2 = 0.00,
            //                          PromTotQ2 = 0.00,
            //                          Total = 0.00
            //                      }).ToList();

            //    return Json(new { detsAlumnos }, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    var detsAlumnos = (from matr in db.Matricula.AsNoTracking()
            //                       join alum in db.Alumno.AsNoTracking() on matr.IdAlumno equals alum.IdAlumno
            //                       join notas in db.Nota.AsNoTracking() on matr.IdAlumno equals notas.IdAlumno
            //                       where matr.Estado == "A" && alum.Estado == "A"
            //                       //&& nota.IdOferta == idoferta && nota.IdCargaLectiva == idcl && nota.IdPeriodoLectivo == idperiodo
            //                       && notas.IdOferta == idoferta && notas.IdCargaLectiva == idcl && notas.IdPeriodoLectivo == idperiodo
            //                       select new
            //                       {
            //                           matr.IdOferta,
            //                           alum.IdAlumno,
            //                           alum.Cedula,                                       
            //                           Alumno = alum.Apellidos + " " + alum.Nombres,
            //                           notas.PpQ1,
            //                           notas.SpQ1,
            //                           notas.TpQ1,
            //                           notas.SumQ1,
            //                           notas.PromQ1,
            //                           notas.OchenPorQ1,
            //                           notas.ExQ1,
            //                           notas.VeinQ1,
            //                           notas.PromTotQ1,
            //                           notas.PpQ2,
            //                           notas.SpQ2,
            //                           notas.TpQ2,
            //                           notas.SumQ2,
            //                           notas.PromQ2,
            //                           notas.OchenPorQ2,
            //                           notas.ExQ2,
            //                           notas.VeinQ2,
            //                           notas.PromTotQ2,
            //                           notas.Total
            //                       }).ToList();

            //    return Json(new { detsAlumnos }, JsonRequestBehavior.AllowGet);
            //}

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