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
    public class MatriculaController : Controller
    {

        ProyectoFinalEntities db = new ProyectoFinalEntities();
        String error = "";

        // GET: Matricula
        public ActionResult Index()
        {

            //List<Matricula> listMatriculaExt = new List<Matricula>();
            //var model = (from deta in db.Matricula.AsNoTracking()
            //             join alu in db.Alumno.AsNoTracking() on deta.IdAlumno equals alu.IdAlumno
            //             join ofer in db.Oferta.AsNoTracking() on deta.IdOferta equals ofer.IdOferta 
            //             join per in db.PeriodoLectivo.AsNoTracking() on ofer.IdPeriodoLectivo equals per.IdPeriodoLectivo
            //             join cur in db.Curso.AsNoTracking() on ofer.IdCurso equals cur.IdCurso
            //             join par in db.Paralelo.AsNoTracking() on ofer.IdParalelo equals par.IdParalelo
            //             //where deta.NNuIdOF == NNuIdOF
            //             select new
            //             {
            //                 deta.IdMatricula,
            //                 deta.IdAlumno,
            //                 deta.IdOferta,
            //                 deta.Estado,
            //                 DescAlumno = alu.Nombres + " " + alu.Apellidos,
            //                 DescOferta = per.Descripcion + " | " + cur.Descripcion + " | " + par.Descripcion
            //             }).ToList();

            //foreach (var item in model)
            //{
            //    MatriculaExt objMatricula = new MatriculaExt();
            //    objMatricula.IdMatricula = item.IdMatricula;
            //    objMatricula.IdAlumno = item.IdAlumno;
            //    objMatricula.IdOferta = item.IdOferta;
            //    objMatricula.Estado = item.Estado;
            //    objMatricula.DescAlumno = item.DescAlumno;
            //    objMatricula.DescOferta = item.DescOferta;
            //    listMatriculaExt.Add(objMatricula);
            //}

            //return View(listMatriculaExt);

            return View();

        }

        [HttpGet]
        public JsonResult GetAlumnos(string q)
        {

            string sValue = q;

            if (string.IsNullOrEmpty(q))
            {
                sValue = string.Empty;
            }

            List<int> alumnosMat = new List<int>();
            alumnosMat = db.Matricula.AsNoTracking().Where(x => x.Estado == "A" && x.Anulado == "N").Select(x => x.IdAlumno).ToList();

            var Alumnos = (from alu in db.Alumno.AsNoTracking()
                           //join mat in db.Matricula.AsNoTracking() on alu.IdAlumno equals mat.IdAlumno 
                           where (alu.Cedula.Contains(sValue) || alu.Nombres.Contains(sValue) || alu.Apellidos.Contains(sValue)) 
                           && !alumnosMat.Contains(alu.IdAlumno) && alu.Estado == "A"
                           //where (alu.Cedula.IndexOf(sValue, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                           //       alu.Nombres.IndexOf(sValue, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                           //       alu.Apellidos.IndexOf(sValue, StringComparison.CurrentCultureIgnoreCase) >= 0)
                           //&& alu.Cedula.IndexOf(alu.Cedula, StringComparison.CurrentCultureIgnoreCase) >= 0
                           //&& alu.Nombres.IndexOf(alu.Nombres, StringComparison.CurrentCultureIgnoreCase) >= 0
                           //&& alu.Apellidos.IndexOf(alu.Apellidos, StringComparison.CurrentCultureIgnoreCase) >= 0
                           //&&
                           //      !(from mat in db.Matricula where (mat.IdAlumno > 0 && mat.Estado.Equals("A")) select mat.IdAlumno).Contains(alu.IdAlumno) 
                           select new { id = alu.IdAlumno, text = alu.Cedula + " | " + alu.Nombres + " " + alu.Apellidos }
                          ).ToList();
            Alumnos.RemoveAll(item => item == null);

            return Json(new { items = Alumnos }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDetalleAlumno(int idAlu)
        {
            //List<Alumno> detsAlumnos = new List<Alumno>();
            //detsAlumnos = db.Alumno.AsNoTracking().Where(x => x.IdAlumno == idAlu).ToList();

            Alumno objAlumno = new Alumno();
            objAlumno = db.Alumno.AsNoTracking().Where(x => x.IdAlumno == idAlu).FirstOrDefault();

            return Json(new { data = objAlumno }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetOfertas(string q, int? idAlumno)
        {
            string sValue = q;

            var Ofertas = new List<object>().Select(t => new {
                id = default(int),
                text = default(string),
                disabled = default(bool)
            }).ToList();

            if (idAlumno > 0 || idAlumno != null)
            {
                if (string.IsNullOrEmpty(q))
                {
                    sValue = string.Empty;
                }

                int siguienteNivel = (from alum in db.Alumno.AsNoTracking()
                                      where alum.IdAlumno == idAlumno
                                      select alum.UltimoNivel).FirstOrDefault() + 1;

                Ofertas = (from ofer in db.Oferta.AsNoTracking()
                               join per in db.PeriodoLectivo.AsNoTracking() on ofer.IdPeriodoLectivo equals per.IdPeriodoLectivo
                               join cur in db.Curso.AsNoTracking() on ofer.IdCurso equals cur.IdCurso
                               join par in db.Paralelo.AsNoTracking() on ofer.IdParalelo equals par.IdParalelo
                               where (per.Descripcion.Contains(sValue) || cur.Descripcion.Contains(sValue) || par.Descripcion.Contains(sValue)) &&
                                      (per.Estado.Equals("A")) && (cur.Nivel.Equals(siguienteNivel))
                               select new
                               {
                                   id = ofer.IdOferta,
                                   text = per.Descripcion + " | " + cur.Descripcion + " | " + par.Descripcion + " | " + ofer.Jornada,
                                   disabled = ofer.Ocupado < ofer.Capacidad ? false : true
                               }).ToList();
                Ofertas.RemoveAll(item => item == null);
            }

            

            return Json(new { items = Ofertas }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDetalleOferta(int idOfer)
        {
            //List<Matricula> detsMatriculaes = new List<Matricula>();
            //detsMatriculaes = db.Matricula.AsNoTracking().Where(x => x.IdOferta == idOfer).ToList();

            //return Json(new { detsMatriculaes }, JsonRequestBehavior.AllowGet);

            //Oferta objOferta = new Oferta();
            var objOferta = (from ofer in db.Oferta.AsNoTracking()
                             join per in db.PeriodoLectivo.AsNoTracking() on ofer.IdPeriodoLectivo equals per.IdPeriodoLectivo
                             join cur in db.Curso.AsNoTracking() on ofer.IdCurso equals cur.IdCurso
                             join par in db.Paralelo.AsNoTracking() on ofer.IdParalelo equals par.IdParalelo
                             join profe in db.Profesor.AsNoTracking() on ofer.IdProfesor equals profe.IdProfesor
                             where (ofer.IdOferta == idOfer)
                             select new
                             {
                                 DescPeriodo = per.Descripcion,
                                 DescCurso = cur.Descripcion,
                                 DescParalelo = par.Descripcion,
                                 DescProfesor = profe.Nombres + " " + profe.Apellidos,
                                 DescDisponible = ofer.Capacidad - ofer.Ocupado
                             }).FirstOrDefault();

            return Json(new { data = objOferta }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Edit(int id)
        {

            MatriculaExt objMatricula = new MatriculaExt();
            
            var model = (from deta in db.Matricula.AsNoTracking()
                         join alu in db.Alumno.AsNoTracking() on deta.IdAlumno equals alu.IdAlumno
                         join ofer in db.Oferta.AsNoTracking() on deta.IdOferta equals ofer.IdOferta
                         join per in db.PeriodoLectivo.AsNoTracking() on ofer.IdPeriodoLectivo equals per.IdPeriodoLectivo
                         join cur in db.Curso.AsNoTracking() on ofer.IdCurso equals cur.IdCurso
                         join par in db.Paralelo.AsNoTracking() on ofer.IdParalelo equals par.IdParalelo
                         where deta.IdMatricula == id
                         select new
                         {
                             deta.IdMatricula,
                             deta.IdAlumno,
                             deta.IdOferta,
                             deta.Estado,
                             DescAlumno = alu.Nombres + " " + alu.Apellidos,
                             DescOferta = per.Descripcion + " | " + cur.Descripcion + " | " + par.Descripcion
                         }).ToList().FirstOrDefault();


            if (model == null)
            {
                objMatricula = new MatriculaExt();
                return View(objMatricula);
            }

            objMatricula.IdMatricula = model.IdMatricula;
            objMatricula.IdAlumno = model.IdAlumno;
            objMatricula.IdOferta = model.IdOferta;
            objMatricula.Estado = model.Estado;
            objMatricula.DescAlumno = model.DescAlumno;
            objMatricula.DescOferta = model.DescOferta;
            

            return View(objMatricula);
        }

        public JsonResult GetCursos(string q)
        {

            var FatherItems = new List<object>().Select(t => new {
                id = default(int),
                text = default(string)
            }).ToList();

            FatherItems = (from maq in db.Oferta.AsNoTracking()
                           join cur in db.Curso.AsNoTracking() on maq.IdCurso equals cur.IdCurso
                           join par in db.Paralelo.AsNoTracking() on maq.IdParalelo equals par.IdParalelo
                           where maq.Estado == "A"
                           orderby cur.Nivel
                           select new { id = maq.IdOferta, text = cur.Descripcion + " " + par.Descripcion + " | " + (maq.Jornada == "MAT" ? "MATUTINA" : "VESPERTINA") }
                  ).ToList();

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

        public JsonResult GetCursosIndexCL(string q, int? IdPeriodoLectivo)
        {

            var FatherItems = new List<object>().Select(t => new {
                id = default(int),
                text = default(string)
            }).ToList();

            if (IdPeriodoLectivo != null && IdPeriodoLectivo > 0)
            {
                FatherItems = (from maq in db.Oferta.AsNoTracking()
                               join cur in db.Curso.AsNoTracking() on maq.IdCurso equals cur.IdCurso
                               join par in db.Paralelo.AsNoTracking() on maq.IdParalelo equals par.IdParalelo
                               join per in db.PeriodoLectivo.AsNoTracking() on maq.IdPeriodoLectivo equals per.IdPeriodoLectivo
                               where maq.IdPeriodoLectivo == IdPeriodoLectivo
                               orderby cur.Nivel
                               select new { id = maq.IdOferta, text = cur.Descripcion + " " + par.Descripcion + " | " + (maq.Jornada == "MAT" ? "MATUTINA" : "VESPERTINA") }
                 ).ToList();
            }

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

        //public JsonResult GetMaterias(string q, string view)
        //{

        //    var FatherItems = new List<object>().Select(t => new {
        //        id = default(int),
        //        text = default(string)
        //    }).ToList();

        //    if (view == "Create" || view == "Edit")
        //    {
        //        FatherItems = (from maq in db.Materia.AsNoTracking()
        //                       where maq.Estado == "A"
        //                       select new { id = maq.IdMateria, text = maq.Descripcion }
        //                      ).ToList();
        //    }
        //    else
        //    {
        //        FatherItems = (from maq in db.Materia.AsNoTracking()
        //                       select new { id = maq.IdMateria, text = maq.Descripcion }
        //                      ).ToList();
        //    }

        //    //var FatherItems = (from maq in db.Materia.AsNoTracking()
        //    //                   where maq.Estado == "A"
        //    //                   select new { id = maq.IdMateria, text = maq.Descripcion}
        //    //       ).ToList();

        //    FatherItems.RemoveAll(item => item == null);

        //    if (string.IsNullOrEmpty(q))
        //    {
        //        return Json(new { items = FatherItems }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        var FatherItem = (from maq in FatherItems
        //                          where maq.text.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) >= 0
        //                          select new { id = maq.id, text = maq.text }
        //             ).ToList();


        //        FatherItem.RemoveAll(item => item == null);

        //        return Json(new { items = FatherItem }, JsonRequestBehavior.AllowGet);
        //    }

        //}

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

        public JsonResult GetAnulados(string q)
        {

            var Anulados = new List<object>().Select(t => new {
                id = default(int),
                text = default(string)
            }).ToList();

            Anulados.Add(new { id = 1, text = "SI" });
            Anulados.Add(new { id = 2, text = "NO" });

            var FatherItem = Anulados.Where(x => x.text.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) >= 0);

            return Json(new { items = FatherItem }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            //Matricula objMatricula = new Matricula();
            //return View(objMatricula);
            return View();
        }

        //[HttpPost]
        //public JsonResult Create(string Descripcion, string Estado)
        //{
        //    return Json(new { Message = "a", bResultado = true }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        //public JsonResult ConsultarMatriculas()
        //{
        //    ProyectoFinalEntities db = new ProyectoFinalEntities();
        //    List<Matricula> listMatricula = new List<Matricula>();

        //    listMatricula = db.Matricula.ToList();

        //    return Json(new { data = listMatricula }, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetAlumnos(string q, string view)
        {
            var FatherItems = new List<object>().Select(t => new {
                id = default(int),
                text = default(string)
            }).ToList();

            if (view == "Create" || view == "Edit")
            {
                FatherItems = (from maq in db.Alumno.AsNoTracking()
                               where maq.Estado == "A"
                               select new { id = maq.IdAlumno, text = maq.Nombres + " " + maq.Apellidos }
                ).ToList();
            }
            else
            {
                FatherItems = (from maq in db.Alumno.AsNoTracking()
                               select new { id = maq.IdAlumno, text = maq.Nombres + " " + maq.Apellidos }
                ).ToList();
            }

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

        public JsonResult GetMatriculas (int? idperiodo, int? idoferta/*, int? idmateria*/, int? idalumno, int? idestado, int? idanulado)
        {
            List<MatriculaExt> listCargaLectivaExt = new List<MatriculaExt>();

            string est = "";
            string anu = "";

            if (idestado != null)
            {
                est = idestado == 1 ? "A" : "I";
            }

            if (idanulado != null)
            {
                anu = idanulado == 1 ? "Si" : "No";
            }

            //List<Matricula> listMatriculaExt = new List<Matricula>();
            //var model = (from deta in db.Matricula.AsNoTracking()
            //             join alu in db.Alumno.AsNoTracking() on deta.IdAlumno equals alu.IdAlumno
            //             join ofer in db.Oferta.AsNoTracking() on deta.IdOferta equals ofer.IdOferta 
            //             join per in db.PeriodoLectivo.AsNoTracking() on ofer.IdPeriodoLectivo equals per.IdPeriodoLectivo
            //             join cur in db.Curso.AsNoTracking() on ofer.IdCurso equals cur.IdCurso
            //             join par in db.Paralelo.AsNoTracking() on ofer.IdParalelo equals par.IdParalelo
            //             //where deta.NNuIdOF == NNuIdOF
            //             select new
            //             {
            //                 deta.IdMatricula,
            //                 deta.IdAlumno,
            //                 deta.IdOferta,
            //                 deta.Estado,
            //                 DescAlumno = alu.Nombres + " " + alu.Apellidos,
            //                 DescOferta = per.Descripcion + " | " + cur.Descripcion + " | " + par.Descripcion
            //             }).ToList();

            //foreach (var item in model)
            //{
            //    MatriculaExt objMatricula = new MatriculaExt();
            //    objMatricula.IdMatricula = item.IdMatricula;
            //    objMatricula.IdAlumno = item.IdAlumno;
            //    objMatricula.IdOferta = item.IdOferta;
            //    objMatricula.Estado = item.Estado;
            //    objMatricula.DescAlumno = item.DescAlumno;
            //    objMatricula.DescOferta = item.DescOferta;
            //    listMatriculaExt.Add(objMatricula);
            //}




            var model = (from deta in db.Matricula.AsNoTracking()
                         join alu in db.Alumno.AsNoTracking() on deta.IdAlumno equals alu.IdAlumno
                         join ofer in db.Oferta.AsNoTracking() on deta.IdOferta equals ofer.IdOferta
                         join per in db.PeriodoLectivo.AsNoTracking() on ofer.IdPeriodoLectivo equals per.IdPeriodoLectivo
                         join cur in db.Curso.AsNoTracking() on ofer.IdCurso equals cur.IdCurso
                         join par in db.Paralelo.AsNoTracking() on ofer.IdParalelo equals par.IdParalelo
                         //where deta.NNuIdOF == NNuIdOF
                         select new
                         {
                             ofer.IdPeriodoLectivo,
                             deta.IdMatricula,
                             deta.IdAlumno,
                             deta.IdOferta,
                             Anulado = deta.Anulado == "Y" ? "Si": "No",
                             deta.Estado,                             
                             DescAlumno = alu.Nombres + " " + alu.Apellidos,
                             DescOferta = per.Descripcion + " | " + cur.Descripcion + " | " + par.Descripcion
                         });

            if (idestado != null)
            {
                model = model.Where(x => x.Estado == est);
            }

            if (idanulado != null)
            {
                model = model.Where(x => x.Anulado == anu);
            }

            if (idperiodo != null)
            {
                model = model.Where(x => x.IdPeriodoLectivo == idperiodo);
            }

            if (idoferta != null)
            {
                model = model.Where(x => x.IdOferta == idoferta);
            }

            //if (idmateria != null)
            //{
            //    model = model.Where(x => x.IdMateria == idmateria);
            //}

            if (idalumno != null)
            {
                model = model.Where(x => x.IdAlumno == idalumno);
            }

            var model2 = model.ToList();

            foreach (var item in model2)
            {
                MatriculaExt objMatricula = new MatriculaExt();
                objMatricula.IdMatricula = item.IdMatricula;
                objMatricula.IdAlumno = item.IdAlumno;
                objMatricula.IdOferta = item.IdOferta;
                objMatricula.Estado = item.Estado;
                objMatricula.Anulado = item.Anulado;
                objMatricula.DescAlumno = item.DescAlumno;
                objMatricula.DescOferta = item.DescOferta;
                listCargaLectivaExt.Add(objMatricula);
            }

            //return View(listCargaLectivaExt);
            return Json(new { detsCargasLectivas = listCargaLectivaExt }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(int IdMatricula, int IdAlumno, int IdOferta, string Estado)
        {
            string strResult = string.Empty;
            bool bResult = false;
            //string userName = Request.Cookies["UserCode"].Value;
            string userName = User.Identity.Name;

            using (ProyectoFinalEntities dbGrabar = new ProyectoFinalEntities())
            {
                DbContextTransaction transaction = dbGrabar.Database.BeginTransaction();

                try
                {

                    if (esValido(IdAlumno, IdOferta, Estado))
                    {
                        Matricula objMatricula = new Matricula();

                        objMatricula.IdAlumno = IdAlumno;
                        objMatricula.IdOferta = IdOferta;
                        objMatricula.Estado = Estado;

                        //Oferta objOferta = new Oferta();

                        if (IdMatricula == 0)
                        {
                            List<Matricula> Matriculas = new List<Matricula>();
                            Matriculas = db.Matricula.AsNoTracking().ToList();
                            objMatricula.IdMatricula = Matriculas.Count() == 0 ? 1 : Matriculas.Max(x => x.IdMatricula) + 1;
                            objMatricula.Anulado = "N";

                            //objMatricula.UsuarioCreacion = 1;
                            objMatricula.UsuarioCreacion = userName;
                            objMatricula.FechaCreacion = DateTime.Now;
                            dbGrabar.Entry(objMatricula).State = EntityState.Added;

                            //dbGrabar.SaveChanges();

                            Oferta objOferta = db.Oferta.AsNoTracking().Where(x => x.IdOferta == IdOferta).FirstOrDefault();

                            //objOferta.IdOferta = IdOferta;
                            objOferta.Ocupado = objOferta.Ocupado + 1;
                            //objOferta.UsuarioCreacion = consOferta.UsuarioCreacion;
                            //objOferta.FechaCreacion = consOferta.FechaCreacion;

                            //objOferta.UsuarioActualizacion = 1;
                            objOferta.UsuarioActualizacion = userName;
                            objOferta.FechaActualizacion = DateTime.Now;
                            dbGrabar.Entry(objOferta).State = EntityState.Modified;

                            dbGrabar.SaveChanges();
                        }
                        else
                        {
                            Matricula consMatricula = db.Matricula.AsNoTracking().Where(x => x.IdMatricula == IdMatricula).FirstOrDefault();

                            objMatricula.UsuarioCreacion = consMatricula.UsuarioCreacion;
                            objMatricula.FechaCreacion = consMatricula.FechaCreacion;
                            //objMatricula.UsuarioActualizacion = 1;
                            objMatricula.UsuarioActualizacion = userName;
                            objMatricula.FechaActualizacion = DateTime.Now;

                            dbGrabar.Entry(objMatricula).State = EntityState.Modified;
                        }

                        bResult = true;

                        transaction.Commit();

                        strResult = "Datos Grabados Correctamente";

                    }
                    else
                    {
                        bResult = false;
                        strResult = "Error al grabar el registro: " + error;
                    }
                }
                catch (Exception e)
                {
                    strResult = "Error al grabar el registro: " + e;
                    transaction.Rollback();
                }
            }

            return Json(new { Message = strResult, bResultado = bResult }, JsonRequestBehavior.AllowGet);

        }

        public bool esValido(int IdAlumno, int IdOferta, string Estado)
        {
            if (IdAlumno==0)
            {
                error = "Debe ingresar el Alumno";
                return false;
            }

            if (IdOferta==0)
            {
                error = "Debe ingresar la Oferta";
                return false;
            }

            if (alumnoMatriculado(IdAlumno))
            {
                error = "El Alumno ya fue matriculado";
                return false;
            }

            if (!ofertaDisponible(IdOferta))
            {
                error = "La Oferta no tiene cupos disponibles";
                return false;
            }

            return true;
        }

        public bool alumnoMatriculado(int IdAlumno)
        {
            var model = (from mat in db.Matricula.AsNoTracking()
                        where mat.IdAlumno == IdAlumno && mat.Estado.Equals("A")
                        select new { id = mat.IdAlumno }
                        ).ToList().FirstOrDefault();
            
            if (model == null)
            {
                return false;
            }

            return true;
        }

        public bool ofertaDisponible(int IdOferta)
        {
            var model = (from ofer in db.Oferta.AsNoTracking()
                         where ofer.IdOferta == IdOferta && (ofer.Ocupado < ofer.Capacidad)
                         select new { id = ofer.IdOferta }
                        ).ToList().FirstOrDefault();

            if (model == null)
            {
                return false;
            }

            return true;
        }

        [HttpPost]
        public JsonResult AnularMatricula(int IdMatricula)
        {

            string strResult = string.Empty;
            bool bResult = false;
            ProyectoFinalEntities dbGrabar = new ProyectoFinalEntities();
            //string userName = Request.Cookies["UserCode"].Value;
            string userName = User.Identity.Name;

            try
            {
                //if (esValidoAnulaMatricula(IdMatricula, Estado))
                //{
                Matricula objMatricula = db.Matricula.AsNoTracking().Where(x => x.IdMatricula == IdMatricula).FirstOrDefault();
                objMatricula.Estado = "I";
                objMatricula.Anulado = "Y";

                objMatricula.UsuarioActualizacion = userName;
                objMatricula.FechaActualizacion = DateTime.Now;
                dbGrabar.Entry(objMatricula).State = EntityState.Modified;

                //Aumento cspacidad de oferta
                Oferta objOferta = db.Oferta.AsNoTracking().Where(x => x.IdOferta == objMatricula.IdOferta).FirstOrDefault();
                objOferta.Capacidad = objOferta.Capacidad + 1;
                objOferta.Ocupado = objOferta.Ocupado - 1;
                dbGrabar.Entry(objOferta).State = EntityState.Modified;

                dbGrabar.SaveChanges();

                bResult = true;
                strResult = "La matricula se ha anulado correctamente";
                //}
                //else
                //{
                //    bResult = false;
                //    strResult = "Error al grabar el registro: " + error;
                //}
            }
            catch (Exception ex)
            {
                bResult = false;
                strResult = "Error: " + ex.Message;
            }

            return Json(new { Message = strResult, bResultado = bResult }, JsonRequestBehavior.AllowGet);

            //Matricula objMatricula = new Matricula();
            //string PartialUrl = "~/Views/Partial/MatriculaP/_AnularMatr.cshtml";

            //try
            //{
            //    objMatricula = db.Matricula.AsNoTracking().Where(x => x.IdMatricula == IdMatricula).FirstOrDefault();
            //}
            //catch (Exception)
            //{
            //    objMatricula = new Matricula();
            //}

            //return PartialView(PartialUrl, objMatricula);
        }

        public JsonResult GetDetalle(int idMod)
        {
            List<Matricula> detsMatriculas = new List<Matricula>();
            detsMatriculas = db.Matricula.AsNoTracking().Where(x => x.IdMatricula == idMod).ToList();

            return Json(new { detsMatriculas }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult EditaMatricula(int IdMatricula, string Estado)
        {
            string strResult = string.Empty;
            bool bResult = false;
            ProyectoFinalEntities dbGrabar = new ProyectoFinalEntities();
            //string userName = Request.Cookies["UserCode"].Value;
            string userName = User.Identity.Name;

            try
            {
                if (esValidoAnulaMatricula(IdMatricula, Estado))
                {
                    Matricula objMatricula = db.Matricula.AsNoTracking().Where(x => x.IdMatricula == IdMatricula).FirstOrDefault();
                    objMatricula.Estado = Estado;

                    objMatricula.UsuarioActualizacion = userName;
                    objMatricula.FechaActualizacion = DateTime.Now;
                    dbGrabar.Entry(objMatricula).State = EntityState.Modified;

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

        public bool esValidoAnulaMatricula(int IdMatricula, string Estado)
        {
            //Si la opcion a inactivar esta asignada a algun rol, no se le permite inactivar
            //if (Estado == "I")
            //{
            //    RolMatricula consRolMatricula = db.RolMatricula.AsNoTracking().Where(x => x.IdMatricula == IdMatricula && x.Estado == "A").FirstOrDefault();

            //    if (consRolMatricula != null)
            //    {
            //        error = "Error al eliminar el registro: No es posible eliminar una opcion que se encuentra asociada a un rol";
            //        return false;
            //    }
            //}


            return true;
        }
    }
}