using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoFinalModel;

namespace ProyectoFinal.Controllers
{
    public class OpcionController : Controller
    {
        ProyectoFinalEntities db = new ProyectoFinalEntities();
        string error = "";
        // GET: Opcion
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetModulos(string q)
        {
            
            string sValue = q;

            if (string.IsNullOrEmpty(q))
            {
                sValue = string.Empty;
            }

            var Modulos = (from mod in db.Modulo.AsNoTracking()
                           where mod.Descripcion.Contains(sValue)
                           select new { id = mod.IdModulo, text = mod.Descripcion }
                          ).ToList();
            Modulos.RemoveAll(item => item == null);

            return Json(new { items = Modulos }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDetalle(int idMod)
        {
            List<Opcion> detsOpciones = new List<Opcion>();
            detsOpciones = db.Opcion.AsNoTracking().Where(x => x.IdModulo == idMod).ToList();

            return Json(new { detsOpciones }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult Create(int IdModulo, string Descripcion, string Url)
        {
            string strResult = string.Empty;
            bool bResult = false;
            List<Opcion> detsOpciones = new List<Opcion>();

            ProyectoFinalEntities dbGrabar = new ProyectoFinalEntities();

            if (esValido(IdModulo, Descripcion, Url))
            {
                Opcion objOpcion = new Opcion();
                objOpcion.IdModulo = IdModulo;
                objOpcion.Descripcion = Descripcion;
                objOpcion.Url = Url;
                objOpcion.Estado = "A";

                List<Opcion> opciones = new List<Opcion>();
                opciones = db.Opcion.AsNoTracking().ToList();
                objOpcion.IdOpcion = opciones.Count() == 0 ? 1 : opciones.Max(x => x.IdOpcion) + 1;

                objOpcion.UsuarioCreacion = 1;
                objOpcion.FechaCreacion = DateTime.Now;
                dbGrabar.Entry(objOpcion).State = EntityState.Added;              

                dbGrabar.SaveChanges();

                detsOpciones = db.Opcion.AsNoTracking().Where(x => x.IdModulo == IdModulo).ToList();
                bResult = true;
                strResult = "Datos Grabados Correctamente";
            }
            else
            {
                bResult = false;
                strResult = "Error al grabar el registro: " + error;
            }

            return Json(new { Message = strResult, bResultado = bResult, detsOpciones }, JsonRequestBehavior.AllowGet);
        }

        public bool esValido(int idmod, string descripcion, string url)
        {
            if (idmod == 0)
            {
                error = "Debe ingresar el modulo";
                return false;
            }

            if (String.IsNullOrEmpty(descripcion))
            {
                error = "Debe ingresar la descripcion";
                return false;
            }

            if (String.IsNullOrEmpty(url))
            {
                error = "Debe ingresar la url";
                return false;
            }

            return true;
        }

        [HttpGet]
        public PartialViewResult EditOpcion(int IdOpcion)
        {
            Opcion objOpcion = new Opcion();
            string PartialUrl = "~/Views/Partial/OpcionP/_EditaOp.cshtml";

            try
            {
                objOpcion = db.Opcion.AsNoTracking().Where(x => x.IdOpcion == IdOpcion).FirstOrDefault();                
            }
            catch (Exception)
            {
                objOpcion = new Opcion();
            }

            return PartialView(PartialUrl, objOpcion);
        }

        public JsonResult EditaOpcion(int IdOpcion, string Descripcion, string Url, string Estado)
        {
            string strResult = string.Empty;
            bool bResult = false;
            ProyectoFinalEntities dbGrabar = new ProyectoFinalEntities();

            try
            {
                if (esValidoEditaOpcion(IdOpcion, Descripcion, Url, Estado))
                {
                    Opcion objOpcion = db.Opcion.AsNoTracking().Where(x => x.IdOpcion == IdOpcion).FirstOrDefault();
                    objOpcion.Descripcion = Descripcion;
                    objOpcion.Url = Url;
                    objOpcion.Estado = Estado;

                    objOpcion.UsuarioActualizacion = 1;
                    objOpcion.FechaActualizacion = DateTime.Now;
                    dbGrabar.Entry(objOpcion).State = EntityState.Modified;

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

        public bool esValidoEditaOpcion(int IdOpcion, string Descripcion, string Url, string Estado)
        {
            if (String.IsNullOrEmpty(Descripcion))
            {
                error = "Debe ingresar la descripcion";
                return false;
            }

            if (String.IsNullOrEmpty(Url))
            {
                error = "Debe ingresar la url";
                return false;
            }

            //Si la opcion a inactivar esta asignada a algun rol, no se le permite inactivar
            if (Estado == "I")
            {
                RolOpcion consRolOpcion = db.RolOpcion.AsNoTracking().Where(x => x.IdOpcion == IdOpcion && x.Estado == "A").FirstOrDefault();

                if (consRolOpcion != null)
                {
                    error = "Error al eliminar el registro: No es posible eliminar una opcion que se encuentra asociada a un rol";
                    return false;
                }
            }
            

            return true;
        }

        //public JsonResult DeleteOpcion(int IdOpcion)
        //{
        //    string strResult = string.Empty;
        //    bool bResult = false;
        //    List<Opcion> detsOpciones = new List<Opcion>();

        //    ProyectoFinalEntities dbGrabar = new ProyectoFinalEntities();

        //    try
        //    {
        //        Opcion consOpcion = db.Opcion.AsNoTracking().Where(x => x.IdOpcion == IdOpcion).FirstOrDefault();

        //        if (consOpcion != null)
        //        {

        //            //Si la opcion a eliminar esta asignada a algun rol, no se le permite borrar
        //            RolOpcion consRolOpcion = db.RolOpcion.AsNoTracking().Where(x => x.IdOpcion == IdOpcion && x.Estado == "A").FirstOrDefault();

        //            if (consRolOpcion == null)
        //            {
        //                int idModulo = consOpcion.IdModulo;
        //                dbGrabar.Entry(consOpcion).State = EntityState.Deleted;

        //                dbGrabar.SaveChanges();

        //                detsOpciones = db.Opcion.AsNoTracking().Where(x => x.IdModulo == idModulo).ToList();
        //                bResult = true;
        //                strResult = "Datos Eliminados Correctamente";
        //            }
        //            else
        //            {
        //                bResult = false;
        //                strResult = "Error al eliminar el registro: No es posible eliminar una opcion que se encuentra asociada a un rol";
        //            }

        //        }
        //        else
        //        {
        //            bResult = false;
        //            strResult = "Error al eliminar el registro: La opcion no existe";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bResult = false;
        //        strResult = "Error al eliminar el registro: " + ex.Message;
        //    }

        //    return Json(new { Message = strResult, bResultado = bResult, detsOpciones }, JsonRequestBehavior.AllowGet);
        //}

    }
}