using ProyectoFinalModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class UsuarioController : Controller
    {
        ProyectoFinalEntities db = new ProyectoFinalEntities();
        string error = "";

        // GET: Usuario
        public ActionResult Index()
        {
            List<Usuario> c = db.Usuario.AsNoTracking().ToList();
            return View(c);
        }

        public ActionResult Create()
        {
            Usuario objUsuario = new Usuario();
            return View(objUsuario);
        }

        public ActionResult Edit(string id)
        {

            Usuario objUsuario = new Usuario();
            objUsuario = db.Usuario.AsNoTracking().Where(x => x.IdUsuario == id).FirstOrDefault();            
            objUsuario.Password = "";
            ViewBag.DescRol = db.Rol.AsNoTracking().Where(x => x.IdRol == objUsuario.IdRol).FirstOrDefault().Descripcion;
            return View(objUsuario);

        }

        [HttpPost]
        public JsonResult Create(string IdUsuario, string Cedula, string Nombres, string Apellidos, string Email,
                                string Password, string ConfPassword, int Rol, string Estado, string Transaccion)
        {
            string strResult = string.Empty;
            bool bResult = false;

            ProyectoFinalEntities dbGrabar = new ProyectoFinalEntities();
            //string userName = Request.Cookies["UserCode"].Value;
            string userName = User.Identity.Name;

            if (esValido(IdUsuario, Cedula, Nombres, Apellidos, Email, Password, ConfPassword, Rol, Estado, Transaccion))
            {
                Usuario objUsuario = new Usuario();
                objUsuario.IdUsuario = IdUsuario;
                objUsuario.Cedula = Cedula;
                objUsuario.Nombres = Nombres;
                objUsuario.Apellidos = Apellidos;
                objUsuario.Email = Email;
                objUsuario.Password = Password;
                objUsuario.IdRol = Rol;
                objUsuario.Estado = Estado;
                objUsuario.EstadoReset = "A";                             

                if (Transaccion == "C")
                {
                    //objUsuario.UsuarioCreacion = 1;
                    objUsuario.UsuarioCreacion = userName;
                    objUsuario.FechaCreacion = DateTime.Now;
                    dbGrabar.Entry(objUsuario).State = EntityState.Added;
                }
                else
                {
                    Usuario consUsuario = db.Usuario.AsNoTracking().Where(x => x.IdUsuario == IdUsuario).FirstOrDefault();
                    //objAlumno.IdAlumno = IdAlumno;
                    objUsuario.UsuarioCreacion = consUsuario.UsuarioCreacion;
                    objUsuario.FechaCreacion = consUsuario.FechaCreacion;
                    //objUsuario.UsuarioActualizacion = 1;
                    objUsuario.UsuarioActualizacion = userName;
                    objUsuario.FechaActualizacion = DateTime.Now;
                    dbGrabar.Entry(objUsuario).State = EntityState.Modified;
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

        public bool esValido(string IdUsuario, string Cedula, string Nombres, string Apellidos, string Email,
                             string Password, string ConfPassword, int Rol, string Estado, string Transaccion)
        {

            if (String.IsNullOrEmpty(IdUsuario))
            {
                error = "Debe ingresar el id de usuario";
                return false;
            }
            else
            {
                
                //Si se crea el usuario, se debe verificar que el id no exista en otro usuario
                if (Transaccion == "C")
                {
                    Usuario consUsuario = db.Usuario.AsNoTracking().Where(x => x.IdUsuario == IdUsuario).FirstOrDefault();

                    if (consUsuario != null)
                    {
                        error = "El id de usuario ya existe";
                        return false;
                    }
                }
            }

            if (String.IsNullOrEmpty(Cedula))
            {
                error = "Debe ingresar la Cédula";
                return false;
            }
            /*else if (VerificaIdentificacion(Cedula)) 
            {
                error = "Cédula Inválida";
                return false;
            }*/





            if (String.IsNullOrEmpty(Nombres))
            {
                error = "Debe ingresar los Nombres";
                return false;
            }

            if (String.IsNullOrEmpty(Apellidos))
            {
                error = "Debe ingresar los Apellidos";
                return false;
            }            

            if (String.IsNullOrEmpty(Email))
            {
                error = "Debe ingresar el email";
                return false;
            }

            if (String.IsNullOrEmpty(Password))
            {
                error = "Debe ingresar el password";
                return false;
            }

            if (String.IsNullOrEmpty(ConfPassword))
            {
                error = "Debe ingresar la confirmacion del password";
                return false;
            }

            if (Password != ConfPassword)
            {
                error = "Las contrasenias no coinciden";
                return false;
            }            

            if (Rol == 0)
            {
                error = "Debe ingresar el rol del usuario";
                return false;
            }            

            return true;
        }

        public JsonResult GetRoles(string q)
        {

            var FatherItems = (from maq in db.Rol.AsNoTracking()
                               where maq.Estado == "A"
                               select new { id = maq.IdRol, text = maq.Descripcion }
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

    }
}