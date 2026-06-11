using ProyectoFinalModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class ParaleloController : Controller
    {

        ProyectoFinalEntities db = new ProyectoFinalEntities();
        String error = "";

        // GET: Paralelo
        public ActionResult Index()
        {

            List<Paralelo> paralelos = db.Paralelo.AsNoTracking().ToList();
            return View(paralelos);
        }

        public ActionResult Edit(int id)
        {
            Paralelo objParalelo = new Paralelo();
            objParalelo = db.Paralelo.AsNoTracking().Where(x => x.IdParalelo == id).FirstOrDefault();
            return View(objParalelo);
        }

        public ActionResult Create()
        {
            Paralelo objParalelo = new Paralelo();
            return View(objParalelo);
        }

        //[HttpPost]
        //public JsonResult Create(string Descripcion, string Estado)
        //{
        //    return Json(new { Message = "a", bResultado = true }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult ConsultarParalelos()
        {
            ProyectoFinalEntities db = new ProyectoFinalEntities();
            List<Paralelo> listParalelo = new List<Paralelo>();

            listParalelo = db.Paralelo.ToList();

            return Json(new { data = listParalelo }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(int IdParalelo, string Descripcion, string Estado)
        {
            string strResult = string.Empty;
            bool bResult = false;

            ProyectoFinalEntities dbGrabar = new ProyectoFinalEntities();
            //string userName = Request.Cookies["UserCode"].Value;
            string userName = User.Identity.Name;

            if (esValido(Descripcion, Estado))
            {
                Paralelo objParalelo = new Paralelo();
                objParalelo.Descripcion = Descripcion;
                objParalelo.Estado = Estado;

                if (IdParalelo == 0)
                {
                    List<Paralelo> paralelos = new List<Paralelo>();
                    paralelos = db.Paralelo.AsNoTracking().ToList();
                    objParalelo.IdParalelo = paralelos.Count() == 0 ? 1 : paralelos.Max(x => x.IdParalelo) + 1;

                    //objParalelo.UsuarioCreacion = 1;
                    objParalelo.UsuarioCreacion = userName;
                    objParalelo.FechaCreacion = DateTime.Now;
                    dbGrabar.Entry(objParalelo).State = EntityState.Added;
                }
                else
                {
                    Paralelo consParalelo = db.Paralelo.AsNoTracking().Where(x => x.IdParalelo == IdParalelo).FirstOrDefault();
                    objParalelo.IdParalelo = IdParalelo;
                    objParalelo.UsuarioCreacion = consParalelo.UsuarioCreacion;
                    objParalelo.FechaCreacion = consParalelo.FechaCreacion;
                    //objParalelo.UsuarioActualizacion = 1;
                    objParalelo.UsuarioActualizacion = userName;
                    objParalelo.FechaActualizacion = DateTime.Now;
                    dbGrabar.Entry(objParalelo).State = EntityState.Modified;
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