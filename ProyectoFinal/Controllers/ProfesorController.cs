using ProyectoFinalModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class ProfesorController : Controller
    {

        ProyectoFinalEntities db = new ProyectoFinalEntities();
        String error = "";

        // GET: Profesor
        public ActionResult Index()
        {

            List<Profesor> profesores = db.Profesor.AsNoTracking().ToList();
            return View(profesores);
        }

        public ActionResult Edit(int id)
        {
            Profesor objProfesor = new Profesor();
            objProfesor = db.Profesor.AsNoTracking().Where(x => x.IdProfesor == id).FirstOrDefault();
            return View(objProfesor);
        }

        public ActionResult Create()
        {
            Profesor objProfesor = new Profesor();
            return View(objProfesor);
        }

        //[HttpPost]
        //public JsonResult Create(string Descripcion, string Estado)
        //{
        //    return Json(new { Message = "a", bResultado = true }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult ConsultarProfesors()
        {
            ProyectoFinalEntities db = new ProyectoFinalEntities();
            List<Profesor> listProfesor = new List<Profesor>();

            listProfesor = db.Profesor.ToList();

            return Json(new { data = listProfesor }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(int IdProfesor, string Cedula, string Nombres, string Apellidos, 
            string Email, string Direccion, string Telefono, string Sexo, string Estado)
        {
            string strResult = string.Empty;
            bool bResult = false;

            ProyectoFinalEntities dbGrabar = new ProyectoFinalEntities();

            if (esValido(Cedula, Nombres, Apellidos, Email, Direccion, Telefono, Sexo, Estado))
            {
                Profesor objProfesor = new Profesor();
                objProfesor.Cedula = Cedula;
                objProfesor.Nombres = Nombres;
                objProfesor.Apellidos = Apellidos;
                objProfesor.Email = Email;
                objProfesor.Direccion = Direccion;
                objProfesor.Telefono = Telefono;
                objProfesor.Sexo = Sexo;
                objProfesor.Estado = Estado;

                if (IdProfesor == 0)
                {
                    List<Profesor> profesores = new List<Profesor>();
                    profesores = db.Profesor.AsNoTracking().ToList();
                    objProfesor.IdProfesor = profesores.Count() == 0 ? 1 : profesores.Max(x => x.IdProfesor) + 1;

                    objProfesor.UsuarioCreacion = 1;
                    objProfesor.FechaCreacion = DateTime.Now;
                    dbGrabar.Entry(objProfesor).State = EntityState.Added;
                }
                else
                {
                    Profesor consProfesor = db.Profesor.AsNoTracking().Where(x => x.IdProfesor == IdProfesor).FirstOrDefault();
                    objProfesor.IdProfesor = IdProfesor;
                    objProfesor.UsuarioCreacion = consProfesor.UsuarioCreacion;
                    objProfesor.FechaCreacion = consProfesor.FechaCreacion;
                    objProfesor.UsuarioActualizacion = 1;
                    objProfesor.FechaActualizacion = DateTime.Now;
                    dbGrabar.Entry(objProfesor).State = EntityState.Modified;
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

        public bool esValido(string Cedula, string Nombres, string Apellidos, string Email, 
            string Direccion, string Telefono, string Sexo, string Estado)
        {
            if (String.IsNullOrEmpty(Cedula))
            {
                error = "Debe ingresar la cédula";
                return false;
            }

            if (String.IsNullOrEmpty(Nombres))
            {
                error = "Debe ingresar los nombres";
                return false;
            }

            if (String.IsNullOrEmpty(Apellidos))
            {
                error = "Debe ingresar los apellidos";
                return false;
            }

            if (String.IsNullOrEmpty(Email))
            {
                error = "Debe ingresar el email";
                return false;
            }else if (!emailValido(Email))
            {
                error = "Formato de email es inválido";
                return false;
            }

            if (String.IsNullOrEmpty(Direccion))
            {
                error = "Debe ingresar la dirección";
                return false;
            }

            if (String.IsNullOrEmpty(Telefono))
            {
                error = "Debe ingresar la teléfono";
                return false;
            }

            if (String.IsNullOrEmpty(Sexo))
            {
                error = "Debe ingresar la sexo";
                return false;
            }

            return true;
        }

        public static bool emailValido(string Email)
        {
            String sFormato;
            sFormato = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(Email, sFormato))
            {
                if (Regex.Replace(Email, sFormato, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }
}