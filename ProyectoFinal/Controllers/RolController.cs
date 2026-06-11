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
    public class RolController : Controller
    {

        ProyectoFinalEntities db = new ProyectoFinalEntities();
        // GET: Rol
        public ActionResult Index()
        {
            List<Rol> listRoles = db.Rol.AsNoTracking().ToList();
            return View(listRoles);
        }

        [HttpPost]
        public JsonResult Create(Rol objRol, List<RolOpcion> detOpc, string trans)
        {
            string strResult = string.Empty;
            bool bResult = false;

            ProyectoFinalEntities dbGrabarRol = new ProyectoFinalEntities();
            ProyectoFinalEntities dbGrabarOpc = new ProyectoFinalEntities();

            //string userName = Request.Cookies["UserCode"].Value;
            string userName = User.Identity.Name;

            if (String.IsNullOrEmpty(objRol.Descripcion))
            {                
                bResult = false;
                strResult = "Error al grabar el registro: Debe ingresar descripcion";
                return Json(new { Message = strResult, bResultado = bResult }, JsonRequestBehavior.AllowGet);
            }

            //Si se edita un rol y se lo quiere inactivar, se debe verificar que no existan usuarios asociados al rol
            if (objRol.Estado == "I" && trans == "U")
            {
                Usuario consUsu = db.Usuario.AsNoTracking().Where(x => x.Estado == "A" && x.IdRol == objRol.IdRol).FirstOrDefault();

                if (consUsu != null)
                {
                    bResult = false;
                    strResult = "Error al grabar el registro: No es posible inactivar el rol debido a que hay usuarios asociados al mismo";
                    return Json(new { Message = strResult, bResultado = bResult }, JsonRequestBehavior.AllowGet);
                }

            }

            //Grabar Rol

            if (trans == "A")
            {
                List<Rol> roles = new List<Rol>();
                roles = db.Rol.AsNoTracking().ToList();
                objRol.IdRol = roles.Count() == 0 ? 1 : roles.Max(x => x.IdRol) + 1;

                //objRol.UsuarioCreacion = 1;
                objRol.UsuarioCreacion = userName;
                objRol.FechaCreacion = DateTime.Now;                
                dbGrabarRol.Entry(objRol).State = EntityState.Added;
            }
            else
            {
                Rol consRol = db.Rol.AsNoTracking().Where(x => x.IdRol == objRol.IdRol).FirstOrDefault();
                objRol.UsuarioCreacion = consRol.UsuarioCreacion;
                objRol.FechaCreacion = consRol.FechaCreacion;
                //objRol.UsuarioActualizacion = 1;
                objRol.UsuarioActualizacion = userName;
                objRol.FechaActualizacion = DateTime.Now;                
                dbGrabarRol.Entry(objRol).State = EntityState.Modified;
            }

            dbGrabarRol.SaveChanges();

            //Grabar opciones

            if (trans == "A")
            {
                foreach (var item in detOpc)
                {         
                    item.IdRol = objRol.IdRol;
                    //item.UsuarioCreacion = 1;
                    item.UsuarioCreacion = userName;
                    item.FechaCreacion = DateTime.Now;
                    dbGrabarOpc.Entry(item).State = EntityState.Added;
                }
            }
            else
            {
                foreach (var item in detOpc)
                {
                    RolOpcion consRolOp = db.RolOpcion.AsNoTracking().Where(x => x.IdRol == item.IdRol && x.IdOpcion == item.IdOpcion).FirstOrDefault();
                    //Puede que en la edicion, se haya agregado otro permiso y este sea una nueva insercion
                    if (consRolOp == null)
                    {
                        item.IdRol = objRol.IdRol;
                        //item.UsuarioCreacion = 1;
                        item.UsuarioCreacion = userName;
                        item.FechaCreacion = DateTime.Now;
                        dbGrabarOpc.Entry(item).State = EntityState.Added;
                    }
                    else
                    {
                        item.UsuarioCreacion = consRolOp.UsuarioCreacion;
                        item.FechaCreacion = consRolOp.FechaCreacion;
                        //item.UsuarioActualizacion = 1;
                        item.UsuarioActualizacion = userName;
                        item.FechaActualizacion = DateTime.Now;
                        dbGrabarOpc.Entry(item).State = EntityState.Modified;
                    }
                    
                }
            }
               
            dbGrabarOpc.SaveChanges();

            bResult = true;
            strResult = "Datos Grabados Correctamente";

            return Json(new { Message = strResult, bResultado = bResult }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id)
        {            
            Rol objRol = db.Rol.AsNoTracking().Where(x => x.IdRol == id).FirstOrDefault();
            //List<Opcion> listOpc = db.Opcion.AsNoTracking().Where(x => x.Estado == "A").ToList();
            List<Opcion> listOpc = db.Opcion.AsNoTracking().Where(x => x.Estado == "A").OrderBy(y => y.IdModulo).ToList();
            List<OpcionExt> listOpcExt = new List<OpcionExt>();

            foreach (var item in listOpc)
            {
                OpcionExt objOpcExt = new OpcionExt();
                objOpcExt.IdOpcion = item.IdOpcion;
                objOpcExt.IdModulo = item.IdModulo;
                objOpcExt.DescModulo = db.Modulo.AsNoTracking().Where(x => x.IdModulo == item.IdModulo).FirstOrDefault().Descripcion;
                objOpcExt.Descripcion = item.Descripcion;                
                objOpcExt.EstadoRolOpcion = db.RolOpcion.AsNoTracking().Where(x => x.IdRol == id && x.IdOpcion == item.IdOpcion).Count() > 0 ?
                                            db.RolOpcion.AsNoTracking().Where(x => x.IdRol == id && x.IdOpcion == item.IdOpcion).FirstOrDefault().Estado : "I";

                listOpcExt.Add(objOpcExt);
            }

            RolesOpciones objRolesOpciones = new RolesOpciones();
            objRolesOpciones.Rol = objRol;
            objRolesOpciones.OpcionExt = listOpcExt;

            return View(objRolesOpciones);
        }

        public ActionResult Create()
        { 
            Rol objRol = new Rol();
            //List<Opcion> listOpc = db.Opcion.AsNoTracking().Where(x => x.Estado == "A").ToList();
            List<Opcion> listOpc = db.Opcion.AsNoTracking().Where(x => x.Estado == "A").OrderBy(y => y.IdModulo).ToList();
            List<OpcionExt> listOpcExt = new List<OpcionExt>();

            foreach (var item in listOpc)
            {
                OpcionExt objOpcExt = new OpcionExt();
                objOpcExt.IdOpcion = item.IdOpcion;
                objOpcExt.IdModulo = item.IdModulo;
                objOpcExt.DescModulo = db.Modulo.AsNoTracking().Where(x => x.IdModulo == item.IdModulo).FirstOrDefault().Descripcion;
                objOpcExt.Descripcion = item.Descripcion;
                
                listOpcExt.Add(objOpcExt);
            }

            RolesOpciones objRolesOpciones = new RolesOpciones();
            objRolesOpciones.Rol = objRol;
            objRolesOpciones.OpcionExt = listOpcExt;

            return View(objRolesOpciones);
        }


    }
}