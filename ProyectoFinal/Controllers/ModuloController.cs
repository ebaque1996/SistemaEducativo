using ProyectoFinalModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class ModuloController : Controller
    {

        ProyectoFinalEntities db = new ProyectoFinalEntities();
        String error = "";

        // GET: Modulo
        public ActionResult Index()
        {

            List<Modulo> modulos = db.Modulo.AsNoTracking().ToList();
            return View(modulos);
        }

        public ActionResult Edit(int id)
        {
            Modulo objModulo = new Modulo();
            objModulo = db.Modulo.AsNoTracking().Where(x => x.IdModulo == id).FirstOrDefault();
            return View(objModulo);
        }

        public ActionResult Create()
        {
            Modulo objModulo = new Modulo();
            return View(objModulo);
        }

        //[HttpPost]
        //public JsonResult Create(string Descripcion, string Estado)
        //{
        //    return Json(new { Message = "a", bResultado = true }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult ConsultarModulos()
        {
            ProyectoFinalEntities db = new ProyectoFinalEntities();
            List<Modulo> listModulo = new List<Modulo>();

            listModulo = db.Modulo.ToList();

            return Json(new { data = listModulo }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(int IdModulo, string Descripcion, string Estado)
        {
            string strResult = string.Empty;
            bool bResult = false;

            ProyectoFinalEntities dbGrabar = new ProyectoFinalEntities();

            if (esValido(Descripcion, Estado))
            {
                Modulo objModulo = new Modulo();
                objModulo.Descripcion = Descripcion;
                objModulo.Estado = Estado;

                if (IdModulo == 0)
                {
                    List<Modulo> modulos = new List<Modulo>();
                    modulos = db.Modulo.AsNoTracking().ToList();
                    objModulo.IdModulo = modulos.Count() == 0 ? 1 : modulos.Max(x => x.IdModulo) + 1;

                    objModulo.UsuarioCreacion = 1;
                    objModulo.FechaCreacion = DateTime.Now;
                    dbGrabar.Entry(objModulo).State = EntityState.Added;
                }
                else
                {
                    Modulo consModulo = db.Modulo.AsNoTracking().Where(x => x.IdModulo == IdModulo).FirstOrDefault();
                    objModulo.IdModulo = IdModulo;
                    objModulo.UsuarioCreacion = consModulo.UsuarioCreacion;
                    objModulo.FechaCreacion = consModulo.FechaCreacion;
                    objModulo.UsuarioActualizacion = 1;
                    objModulo.FechaActualizacion = DateTime.Now;
                    dbGrabar.Entry(objModulo).State = EntityState.Modified;
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