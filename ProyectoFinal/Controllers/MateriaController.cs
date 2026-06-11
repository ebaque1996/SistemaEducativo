//using ProyectoFinal.Clases;
using ProyectoFinalModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class MateriaController : Controller
    {

        ProyectoFinalEntities db = new ProyectoFinalEntities();
        String error = "";

        // GET: Materia
        public ActionResult Index()
        {

            List<Materia> materias = db.Materia.AsNoTracking().ToList();
            List<Nota> notas = db.Nota.AsNoTracking().ToList();
            return View(materias);
        }

        public ActionResult Edit(int id)
        {
            Materia objMateria = new Materia();
            objMateria = db.Materia.AsNoTracking().Where(x => x.IdMateria == id).FirstOrDefault();
            return View(objMateria);
        }

        public ActionResult Create()
        {
            Materia objMateria = new Materia();
            return View(objMateria);
        }

        //[HttpPost]
        //public JsonResult Create(string Descripcion, string Estado)
        //{
        //    return Json(new { Message = "a", bResultado = true }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult ConsultarMaterias()
        {
            ProyectoFinalEntities db = new ProyectoFinalEntities();
            List<Materia> listMateria = new List<Materia>();

            listMateria = db.Materia.ToList();

            return Json(new { data = listMateria }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(int IdMateria, string Descripcion, string Estado)
        {
            string strResult = string.Empty;
            bool bResult = false;

            ProyectoFinalEntities dbGrabar = new ProyectoFinalEntities();
            
            string userName = User.Identity.Name;            

            if (esValido(Descripcion, Estado))
            {
                Materia objMateria = new Materia();
                objMateria.Descripcion = Descripcion;
                objMateria.Estado = Estado;

                if (IdMateria == 0)
                {
                    List<Materia> materias = new List<Materia>();
                    materias = db.Materia.AsNoTracking().ToList();
                    objMateria.IdMateria = materias.Count() == 0 ? 1 : materias.Max(x => x.IdMateria) + 1;

                    //objMateria.UsuarioCreacion = 1;
                    objMateria.UsuarioCreacion = userName;
                    objMateria.FechaCreacion = DateTime.Now;
                    dbGrabar.Entry(objMateria).State = EntityState.Added;
                }
                else
                {
                    Materia consMateria = db.Materia.AsNoTracking().Where(x => x.IdMateria == IdMateria).FirstOrDefault();
                    objMateria.IdMateria = IdMateria;
                    objMateria.UsuarioCreacion = consMateria.UsuarioCreacion;
                    objMateria.FechaCreacion = consMateria.FechaCreacion;
                    //objMateria.UsuarioActualizacion = 1;
                    objMateria.UsuarioActualizacion = userName;
                    objMateria.FechaActualizacion = DateTime.Now;
                    dbGrabar.Entry(objMateria).State = EntityState.Modified;
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

        public bool esValido(string Descripcion, string Estado)
        {
            if (String.IsNullOrEmpty(Descripcion))
            {
                error = "Debe ingresar la descripcion";
                return false;
            }

            return true;
        }
    }
}