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
    public class CargaLectivaController : Controller
    {
        ProyectoFinalEntities db = new ProyectoFinalEntities();
        string error = "";
        // GET: CargaLectiva
        public ActionResult Index()
        {
            //List<CargaLectiva> listCargaLectiva = db.CargaLectiva.AsNoTracking().ToList();
            //List<CargaLectivaExt> listCargaLectivaExt = new List<CargaLectivaExt>();
            //foreach (var item in listCargaLectiva)
            //{
            //    CargaLectivaExt objCarLec = new CargaLectivaExt();
            //    objCarLec.IdCargaLectiva = item.IdCargaLectiva;
            //    //objCarLec.IdCurso = item.IdCurso;
            //    objCarLec.IdOferta = item.IdOferta;
            //    objCarLec.IdMateria = item.IdMateria;
            //    objCarLec.IdPeriodoLectivo = item.IdPeriodoLectivo;
            //    objCarLec.IdProfesor = item.IdProfesor;
            //    objCarLec.Estado = item.Estado;
            //    //objCarLec.DescCurso = db.Curso.AsNoTracking().Where(x => x.IdCurso == item.IdCurso).FirstOrDefault().Descripcion;
            //    objCarLec.DescOferta = db.Oferta.AsNoTracking().Where(x => x.IdOferta == item.IdOferta).FirstOrDefault().Descripcion;
            //    objCarLec.DescMateria = db.Materia.AsNoTracking().Where(x => x.IdMateria == item.IdMateria).FirstOrDefault().Descripcion;
            //    objCarLec.DescPerLec = db.PeriodoLectivo.AsNoTracking().Where(x => x.IdPeriodoLectivo == item.IdPeriodoLectivo).FirstOrDefault().Descripcion;
            //    objCarLec.DescProfesor = db.Profesor.AsNoTracking().Where(x => x.IdProfesor == item.IdProfesor).FirstOrDefault().Apellidos + " " + db.Profesor.AsNoTracking().Where(x => x.IdProfesor == item.IdProfesor).FirstOrDefault().Nombres;
            //    listCargaLectivaExt.Add(objCarLec);
            //}



            //List<CargaLectivaExt> listCargaLectivaExt = new List<CargaLectivaExt>();

            //var model = (from deta in db.CargaLectiva.AsNoTracking()
            //             join ofer in db.Oferta.AsNoTracking() on deta.IdOferta equals ofer.IdOferta
            //             join cur in db.Curso.AsNoTracking() on ofer.IdCurso equals cur.IdCurso
            //             join par in db.Paralelo.AsNoTracking() on ofer.IdParalelo equals par.IdParalelo
            //             join mat in db.Materia.AsNoTracking() on deta.IdMateria equals mat.IdMateria
            //             join perlec in db.PeriodoLectivo.AsNoTracking() on deta.IdPeriodoLectivo equals perlec.IdPeriodoLectivo
            //             join prof in db.Profesor.AsNoTracking() on deta.IdProfesor equals prof.IdProfesor
            //             //where deta.NNuIdOF == NNuIdOF
            //             select new
            //             {
            //                 deta.IdCargaLectiva,
            //                 deta.IdOferta,
            //                 deta.IdMateria,
            //                 deta.IdPeriodoLectivo,
            //                 deta.IdProfesor,
            //                 deta.Estado,
            //                 DescOferta = cur.Descripcion + " " + par.Descripcion + " | " + (ofer.Jornada == "MAT" ? "MATUTINA" : "VESPERTINA"),
            //                 DescMateria = mat.Descripcion,
            //                 DescPerLec = perlec.Descripcion,
            //                 DescProfesor = prof.Nombres + " " + prof.Apellidos
            //             }).ToList();

            //foreach (var item in model)
            //{
            //    CargaLectivaExt objCarLec = new CargaLectivaExt();
            //    objCarLec.IdCargaLectiva = item.IdCargaLectiva;                
            //    objCarLec.IdOferta = item.IdOferta;
            //    objCarLec.IdMateria = item.IdMateria;
            //    objCarLec.IdPeriodoLectivo = item.IdPeriodoLectivo;
            //    objCarLec.IdProfesor = item.IdProfesor;
            //    objCarLec.Estado = item.Estado;
            //    objCarLec.DescOferta = item.DescOferta;
            //    objCarLec.DescMateria = item.DescMateria;
            //    objCarLec.DescPerLec = item.DescPerLec;
            //    objCarLec.DescProfesor = item.DescProfesor;
            //    listCargaLectivaExt.Add(objCarLec);
            //}

            //return View(listCargaLectivaExt);

            return View();
        }

        public ActionResult Create()
        {
            //Curso objCurso = new Curso();
            //return View(objCurso);
            return View();
        }

        public ActionResult Edit(int id)
        {
            //CargaLectiva objCL = new CargaLectiva();
            //objCL = db.CargaLectiva.AsNoTracking().Where(x => x.IdCargaLectiva == id).FirstOrDefault();

            CargaLectivaExt objCarLec = new CargaLectivaExt();
            //objCarLec.IdCargaLectiva = objCL.IdCargaLectiva;
            //objCarLec.IdCurso = objCL.IdCurso;
            //objCarLec.IdMateria = objCL.IdMateria;
            //objCarLec.IdPeriodoLectivo = objCL.IdPeriodoLectivo;
            //objCarLec.IdProfesor = objCL.IdProfesor;
            //objCarLec.Estado = objCL.Estado;
            //objCarLec.DescCurso = db.Curso.AsNoTracking().Where(x => x.IdCurso == objCL.IdCurso).FirstOrDefault().Descripcion;
            //objCarLec.DescMateria = db.Materia.AsNoTracking().Where(x => x.IdMateria == objCL.IdMateria).FirstOrDefault().Descripcion;
            //objCarLec.DescPerLec = db.PeriodoLectivo.AsNoTracking().Where(x => x.IdPeriodoLectivo == objCL.IdPeriodoLectivo).FirstOrDefault().Descripcion;
            //objCarLec.DescProfesor = db.Profesor.AsNoTracking().Where(x => x.IdProfesor == objCL.IdProfesor).FirstOrDefault().Apellidos + " " + db.Profesor.AsNoTracking().Where(x => x.IdProfesor == objCL.IdProfesor).FirstOrDefault().Nombres;

            var model = (from deta in db.CargaLectiva.AsNoTracking()
                         join ofer in db.Oferta.AsNoTracking() on deta.IdOferta equals ofer.IdOferta
                         join cur in db.Curso.AsNoTracking() on ofer.IdCurso equals cur.IdCurso
                         join par in db.Paralelo.AsNoTracking() on ofer.IdParalelo equals par.IdParalelo
                         join mat in db.Materia.AsNoTracking() on deta.IdMateria equals mat.IdMateria
                         join perlec in db.PeriodoLectivo.AsNoTracking() on deta.IdPeriodoLectivo equals perlec.IdPeriodoLectivo
                         join prof in db.Profesor.AsNoTracking() on deta.IdProfesor equals prof.IdProfesor
                         where deta.IdCargaLectiva == id
                         select new
                         {
                             deta.IdCargaLectiva,
                             deta.IdOferta,
                             deta.IdMateria,
                             deta.IdPeriodoLectivo,
                             deta.IdProfesor,
                             deta.Estado,
                             DescOferta = cur.Descripcion + " " + par.Descripcion + " | " + (ofer.Jornada == "MAT" ? "MATUTINA" : "VESPERTINA"),
                             DescMateria = mat.Descripcion,
                             DescPerLec = perlec.Descripcion,
                             DescProfesor = prof.Nombres + " " + prof.Apellidos
                         }).ToList().FirstOrDefault();

            if (model == null)
            {
                objCarLec = new CargaLectivaExt();
                return View(objCarLec);
            }

            objCarLec.IdCargaLectiva = model.IdCargaLectiva;
            objCarLec.IdOferta = model.IdOferta;
            objCarLec.IdMateria = model.IdMateria;
            objCarLec.IdPeriodoLectivo = model.IdPeriodoLectivo;
            objCarLec.IdProfesor = model.IdProfesor;
            objCarLec.Estado = model.Estado;
            objCarLec.DescOferta = model.DescOferta;
            objCarLec.DescMateria = model.DescMateria;
            objCarLec.DescPerLec = model.DescPerLec;
            objCarLec.DescProfesor = model.DescProfesor;            


            return View(objCarLec);
        }

        [HttpGet]
        public JsonResult GetProfesores(string q, string view)
        {
            var FatherItems = new List<object>().Select(t => new {
                id = default(int),
                text = default(string)
            }).ToList();

            if (view == "Create" || view == "Edit")
            {
                FatherItems = (from maq in db.Profesor.AsNoTracking()
                                where maq.Estado == "A"
                                select new { id = maq.IdProfesor, text = maq.Nombres + " " + maq.Apellidos }
                ).ToList();
            }
            else
            {
                FatherItems = (from maq in db.Profesor.AsNoTracking()
                               select new { id = maq.IdProfesor, text = maq.Nombres + " " + maq.Apellidos }
                ).ToList();
            }
            //var FatherItems = (from maq in db.Profesor.AsNoTracking()
            //                   where maq.Estado == "A"                               
            //                   select new { id = maq.IdProfesor, text = maq.Nombres + " " + maq.Apellidos }
            //       ).ToList();

            FatherItems.RemoveAll(item => item == null);

            if (string.IsNullOrEmpty(q))
            {
                return Json(new { items = FatherItems }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var FatherItem = (from maq in FatherItems
                                  //where maq.text.Contains(q)
                                  where maq.text.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) >= 0
                                  select new { id = maq.id, text = maq.text }
                     ).ToList();


                FatherItem.RemoveAll(item => item == null);

                return Json(new { items = FatherItem }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetCursos(string q, string view)
        {

            //var FatherItems = (from maq in db.Curso.AsNoTracking()
            //                   where maq.Estado == "A"
            //                   select new { id = maq.IdCurso, text = maq.Descripcion}
            //       ).ToList();

            var FatherItems = new List<object>().Select(t => new {
                id = default(int),
                text = default(string)
            }).ToList();

            if (view == "Create" || view == "Edit")
            {
                  FatherItems = (from maq in db.Oferta.AsNoTracking()
                                 join cur in db.Curso.AsNoTracking() on maq.IdCurso equals cur.IdCurso
                                 join par in db.Paralelo.AsNoTracking() on maq.IdParalelo equals par.IdParalelo
                                 where maq.Estado == "A"
                                 select new { id = maq.IdOferta, text = cur.Descripcion + " " + par.Descripcion + " | " + (maq.Jornada == "MAT" ? "MATUTINA" : "VESPERTINA") }
                  ).ToList();
            }
            else
            {
                FatherItems = (from maq in db.Oferta.AsNoTracking()
                               join cur in db.Curso.AsNoTracking() on maq.IdCurso equals cur.IdCurso
                               join par in db.Paralelo.AsNoTracking() on maq.IdParalelo equals par.IdParalelo
                               join per in db.PeriodoLectivo.AsNoTracking() on maq.IdPeriodoLectivo equals per.IdPeriodoLectivo
                               where maq.Estado == "A"
                               select new { id = maq.IdOferta, text = per.Descripcion + " | " + cur.Descripcion + " " + par.Descripcion + " | " + (maq.Jornada == "MAT" ? "MATUTINA" : "VESPERTINA") }
                  ).ToList();
            }

            //var FatherItems = (from maq in db.Oferta.AsNoTracking()
            //                   join cur in db.Curso.AsNoTracking() on maq.IdCurso equals cur.IdCurso
            //                   join par in db.Paralelo.AsNoTracking() on maq.IdParalelo equals par.IdParalelo
            //                   where maq.Estado == "A"
            //                   select new { id = maq.IdOferta, text = cur.Descripcion + " " + par.Descripcion + " | " + (maq.Jornada == "MAT" ? "MATUTINA" : "VESPERTINA") }
            //       ).ToList();

            FatherItems.RemoveAll(item => item == null);

            if (string.IsNullOrEmpty(q))
            {
                return Json(new { items = FatherItems }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var FatherItem = (from maq in FatherItems
                                  where maq.text.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) >= 0
                                  select new { id = maq.id, text = maq.text }
                     ).ToList();


                FatherItem.RemoveAll(item => item == null);

                return Json(new { items = FatherItem }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetPeriodos(string q, string view)
        {
            var FatherItems = new List<object>().Select(t => new {
                id = default(int),
                text = default(string)
            }).ToList();

            if (view == "Create" || view == "Edit")
            {
                  FatherItems = (from maq in db.PeriodoLectivo.AsNoTracking()
                                 where maq.Estado == "A"
                                 select new { id = maq.IdPeriodoLectivo, text = maq.Descripcion }
                  ).ToList();
            }
            else
            {
                FatherItems = (from maq in db.PeriodoLectivo.AsNoTracking()                               
                               select new { id = maq.IdPeriodoLectivo, text = maq.Descripcion }
                  ).ToList();
            }

            //var FatherItems = (from maq in db.PeriodoLectivo.AsNoTracking()
            //                   where maq.Estado == "A"
            //                   select new { id = maq.IdPeriodoLectivo, text = maq.Descripcion}
            //       ).ToList();

            FatherItems.RemoveAll(item => item == null);

            if (string.IsNullOrEmpty(q))
            {
                return Json(new { items = FatherItems }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var FatherItem = (from maq in FatherItems
                                  where maq.text.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) >= 0
                                  select new { id = maq.id, text = maq.text }
                     ).ToList();


                FatherItem.RemoveAll(item => item == null);

                return Json(new { items = FatherItem }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetMaterias(string q, string view)
        {

            var FatherItems = new List<object>().Select(t => new {
                id = default(int),
                text = default(string)
            }).ToList();

            if (view == "Create" || view == "Edit")
            {
                FatherItems = (from maq in db.Materia.AsNoTracking()
                               where maq.Estado == "A"
                               select new { id = maq.IdMateria, text = maq.Descripcion }
                              ).ToList();
            }
            else
            {
                FatherItems = (from maq in db.Materia.AsNoTracking()                               
                               select new { id = maq.IdMateria, text = maq.Descripcion }
                              ).ToList();
            }

            //var FatherItems = (from maq in db.Materia.AsNoTracking()
            //                   where maq.Estado == "A"
            //                   select new { id = maq.IdMateria, text = maq.Descripcion}
            //       ).ToList();

            FatherItems.RemoveAll(item => item == null);

            if (string.IsNullOrEmpty(q))
            {
                return Json(new { items = FatherItems }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var FatherItem = (from maq in FatherItems
                                  where maq.text.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) >= 0
                                  select new { id = maq.id, text = maq.text }
                     ).ToList();


                FatherItem.RemoveAll(item => item == null);

                return Json(new { items = FatherItem }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetEstados(string q)
        {

            var Estados = new List<object>().Select(t => new {
                id = default(int),
                text = default(string)
            }).ToList();

            Estados.Add(new { id = 1, text = "ACTIVO" });
            Estados.Add(new { id = 2, text = "INACTIVO" });

            var FatherItem = Estados.Where(x => x.text.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) >= 0);

            return Json(new { items = FatherItem }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCargasLectivas(int? idperiodo, int? idoferta, int? idmateria, int? idprofesor, int? idestado)
        {
            List<CargaLectivaExt> listCargaLectivaExt = new List<CargaLectivaExt>();           

            //bool consper = false;
            //bool consofer = false;
            //bool consmat = false;
            //bool conspro = false;
            //bool consest = false;

            string est = "";

            if (idestado != null)
            {
                est = idestado == 1 ? "A" : "I";
            }

            var model = (from deta in db.CargaLectiva.AsNoTracking()
                         join ofer in db.Oferta.AsNoTracking() on deta.IdOferta equals ofer.IdOferta
                         join cur in db.Curso.AsNoTracking() on ofer.IdCurso equals cur.IdCurso
                         join par in db.Paralelo.AsNoTracking() on ofer.IdParalelo equals par.IdParalelo
                         join mat in db.Materia.AsNoTracking() on deta.IdMateria equals mat.IdMateria
                         join perlec in db.PeriodoLectivo.AsNoTracking() on deta.IdPeriodoLectivo equals perlec.IdPeriodoLectivo
                         join prof in db.Profesor.AsNoTracking() on deta.IdProfesor equals prof.IdProfesor
                         //where (consest && deta.Estado == est)
                         //&& (idperiodo != null && deta.IdPeriodoLectivo == idperiodo)
                         //&& (idoferta != null && deta.IdOferta == idoferta)
                         //&& (idmateria != null && deta.IdMateria == idoferta)
                         //&& (idprofesor != null && deta.IdProfesor == idprofesor)                         
                         select new
                         {
                             deta.IdCargaLectiva,
                             deta.IdOferta,
                             deta.IdMateria,
                             deta.IdPeriodoLectivo,
                             deta.IdProfesor,
                             deta.Estado,
                             DescOferta = cur.Descripcion + " " + par.Descripcion + " | " + (ofer.Jornada == "MAT" ? "MATUTINA" : "VESPERTINA"),
                             DescMateria = mat.Descripcion,
                             DescPerLec = perlec.Descripcion,
                             DescProfesor = prof.Nombres + " " + prof.Apellidos
                         });
            //.ToList();

            if (idestado != null)
            {
                model = model.Where(x => x.Estado == est);
            }

            if (idperiodo != null)
            {
                model = model.Where(x => x.IdPeriodoLectivo == idperiodo);
            }

            if (idoferta != null)
            {
                model = model.Where(x => x.IdOferta == idoferta);
            }

            if (idmateria != null)
            {
                model = model.Where(x => x.IdMateria == idmateria);
            }

            if (idprofesor != null)
            {
                model = model.Where(x => x.IdProfesor == idprofesor);
            }

            var model2 = model.ToList();

            foreach (var item in model2)
            {
                CargaLectivaExt objCarLec = new CargaLectivaExt();
                objCarLec.IdCargaLectiva = item.IdCargaLectiva;
                objCarLec.IdOferta = item.IdOferta;
                objCarLec.IdMateria = item.IdMateria;
                objCarLec.IdPeriodoLectivo = item.IdPeriodoLectivo;
                objCarLec.IdProfesor = item.IdProfesor;
                objCarLec.Estado = item.Estado;
                objCarLec.DescOferta = item.DescOferta;
                objCarLec.DescMateria = item.DescMateria;
                objCarLec.DescPerLec = item.DescPerLec;
                objCarLec.DescProfesor = item.DescProfesor;
                listCargaLectivaExt.Add(objCarLec);
            }

            //return View(listCargaLectivaExt);
            return Json(new { detsCargasLectivas = listCargaLectivaExt }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(CargaLectiva objCargaLectiva)
        {
            string strResult = string.Empty;
            bool bResult = false;

            ProyectoFinalEntities dbGrabar = new ProyectoFinalEntities();

            if (esValido(objCargaLectiva))
            {
                if (objCargaLectiva.IdCargaLectiva == 0)
                {
                    List<CargaLectiva> carLecs = new List<CargaLectiva>();
                    carLecs = db.CargaLectiva.AsNoTracking().ToList();
                    objCargaLectiva.IdCargaLectiva = carLecs.Count() == 0 ? 1 : carLecs.Max(x => x.IdCargaLectiva) + 1;

                    objCargaLectiva.UsuarioCreacion = 1;
                    objCargaLectiva.FechaCreacion = DateTime.Now;
                    dbGrabar.Entry(objCargaLectiva).State = EntityState.Added;
                }
                else
                {
                    CargaLectiva consCL = db.CargaLectiva.AsNoTracking().Where(x => x.IdCargaLectiva == objCargaLectiva.IdCargaLectiva).FirstOrDefault();
                    objCargaLectiva.UsuarioCreacion = consCL.UsuarioCreacion;
                    objCargaLectiva.FechaCreacion = consCL.FechaCreacion;
                    objCargaLectiva.UsuarioActualizacion = 1;
                    objCargaLectiva.FechaActualizacion = DateTime.Now;
                    dbGrabar.Entry(objCargaLectiva).State = EntityState.Modified;
                }

                dbGrabar.SaveChanges();

                bResult = true;
                strResult = "Datos Grabados Correctamente";
            }
            else
            {
                bResult = false;
                strResult = "Error al grabar el registro: " + error;
            }

            return Json(new { Message = strResult, bResultado = bResult }, JsonRequestBehavior.AllowGet);
        }

        public bool esValido(CargaLectiva objCargaLectiva)
        {
            if (objCargaLectiva.IdPeriodoLectivo == 0)
            {
                error = "Debe ingresar el periodo lectivo";
                return false;
            }

            if (objCargaLectiva.IdOferta == 0)
            {
                error = "Debe ingresar el curso";
                return false;
            }

            if (objCargaLectiva.IdMateria == 0)
            {
                error = "Debe ingresar la materia";
                return false;
            }

            if (objCargaLectiva.IdProfesor == 0)
            {
                error = "Debe ingresar el profesor";
                return false;
            }                     

            return true;
        }

    }
}