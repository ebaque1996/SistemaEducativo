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

            List<Matricula> listMatriculaExt = new List<Matricula>();
            var model = (from deta in db.Matricula.AsNoTracking()
                         join alu in db.Alumno.AsNoTracking() on deta.IdAlumno equals alu.IdAlumno
                         join ofer in db.Oferta.AsNoTracking() on deta.IdOferta equals ofer.IdOferta 
                         join per in db.PeriodoLectivo.AsNoTracking() on ofer.IdPeriodoLectivo equals per.IdPeriodoLectivo
                         join cur in db.Curso.AsNoTracking() on ofer.IdCurso equals cur.IdCurso
                         join par in db.Paralelo.AsNoTracking() on ofer.IdParalelo equals par.IdParalelo
                         //where deta.NNuIdOF == NNuIdOF
                         select new
                         {
                             deta.IdMatricula,
                             deta.IdAlumno,
                             deta.IdOferta,
                             deta.Estado,
                             DescAlumno = alu.Nombres + " " + alu.Apellidos,
                             DescOferta = per.Descripcion + " | " + cur.Descripcion + " | " + par.Descripcion
                         }).ToList();

            foreach (var item in model)
            {
                MatriculaExt objMatricula = new MatriculaExt();
                objMatricula.IdMatricula = item.IdMatricula;
                objMatricula.IdAlumno = item.IdAlumno;
                objMatricula.IdOferta = item.IdOferta;
                objMatricula.Estado = item.Estado;
                objMatricula.DescAlumno = item.DescAlumno;
                objMatricula.DescOferta = item.DescOferta;
                listMatriculaExt.Add(objMatricula);
            }
            
            return View(listMatriculaExt);
            
        }

        [HttpGet]
        public JsonResult GetAlumnos(string q)
        {

            string sValue = q;

            if (string.IsNullOrEmpty(q))
            {
                sValue = string.Empty;
            }

            var Alumnos = (from alu in db.Alumno.AsNoTracking()
                           //join mat in db.Matricula.AsNoTracking() on alu.IdAlumno equals mat.IdAlumno 
                           where (alu.Cedula.Contains(sValue) || alu.Nombres.Contains(sValue) || alu.Apellidos.Contains(sValue)) &&
                                 !(from mat in db.Matricula where mat.IdAlumno > 0 select mat.IdAlumno).Contains(alu.IdAlumno) 
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
        public JsonResult GetOfertas(string q)
        {

            string sValue = q;

            if (string.IsNullOrEmpty(q))
            {
                sValue = string.Empty;
            }

            var Ofertas = (from ofer in db.Oferta.AsNoTracking() 
                           join per in db.PeriodoLectivo.AsNoTracking() on ofer.IdPeriodoLectivo equals per.IdPeriodoLectivo 
                           join cur in db.Curso.AsNoTracking() on ofer.IdCurso equals cur.IdCurso 
                           join par in db.Paralelo.AsNoTracking() on ofer.IdParalelo equals par.IdParalelo 
                           where (per.Descripcion.Contains(sValue) || cur.Descripcion.Contains(sValue) || par.Descripcion.Contains(sValue)) 
                           select new { id = ofer.IdOferta, text = per.Descripcion + " | " + cur.Descripcion + " | " + par.Descripcion ,
                                        disabled = ofer.Ocupado < ofer.Capacidad ? false : true
                           }).ToList();
            Ofertas.RemoveAll(item => item == null);

            return Json(new { items = Ofertas }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDetalleOferta(int idOfer)
        {
            //List<Matricula> detsOpciones = new List<Matricula>();
            //detsOpciones = db.Matricula.AsNoTracking().Where(x => x.IdOferta == idOfer).ToList();

            //return Json(new { detsOpciones }, JsonRequestBehavior.AllowGet);

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
                                 DescProfesor = profe.Nombres + " " + profe.Apellidos
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

        public ActionResult Create()
        {
            Matricula objMatricula = new Matricula();
            return View(objMatricula);
        }

        //[HttpPost]
        //public JsonResult Create(string Descripcion, string Estado)
        //{
        //    return Json(new { Message = "a", bResultado = true }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult ConsultarMatriculas()
        {
            ProyectoFinalEntities db = new ProyectoFinalEntities();
            List<Matricula> listMatricula = new List<Matricula>();

            listMatricula = db.Matricula.ToList();

            return Json(new { data = listMatricula }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(int IdMatricula, int IdAlumno, int IdOferta, string Estado)
        {
            string strResult = string.Empty;
            bool bResult = false;

            ProyectoFinalEntities dbGrabar = new ProyectoFinalEntities();

            if (esValido(IdAlumno, IdOferta, Estado))
            {
                Matricula objMatricula = new Matricula();
                objMatricula.IdAlumno = IdAlumno;
                objMatricula.IdOferta = IdOferta;
                objMatricula.Estado = Estado;

                if (IdMatricula == 0)
                {
                    List<Matricula> Matriculas = new List<Matricula>();
                    Matriculas = db.Matricula.AsNoTracking().ToList();
                    objMatricula.IdMatricula = Matriculas.Count() == 0 ? 1 : Matriculas.Max(x => x.IdMatricula) + 1;

                    objMatricula.UsuarioCreacion = 1;
                    objMatricula.FechaCreacion = DateTime.Now;
                    dbGrabar.Entry(objMatricula).State = EntityState.Added;
                }
                else
                {
                    Matricula consMatricula = db.Matricula.AsNoTracking().Where(x => x.IdMatricula == IdMatricula).FirstOrDefault();
                    objMatricula.IdMatricula = IdMatricula;
                    objMatricula.UsuarioCreacion = consMatricula.UsuarioCreacion;
                    objMatricula.FechaCreacion = consMatricula.FechaCreacion;
                    objMatricula.UsuarioActualizacion = 1;
                    objMatricula.FechaActualizacion = DateTime.Now;
                    dbGrabar.Entry(objMatricula).State = EntityState.Modified;
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

            return true;
        }
    }
}