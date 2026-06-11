using ProyectoFinalModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace ProyectoFinal.Controllers
{
    public class CursoController : Controller
    {

        ProyectoFinalEntities db = new ProyectoFinalEntities();
        string error = "";

        //public static clsCapaAcceso.AccesoDatos.SQLServer objSQL;

        // GET: Curso
        public ActionResult Index()
        {
            
            List<Curso> c = db.Curso.AsNoTracking().ToList();            
            return View(c);            
        }        

        public ActionResult Edit(int id)
        {
            Curso objCurso = new Curso();
            objCurso = db.Curso.AsNoTracking().Where(x => x.IdCurso == id).FirstOrDefault();
            return View(objCurso);
        }

        public ActionResult Create()
        {
            Curso objCurso = new Curso();            
            return View(objCurso);
        }

        [HttpPost]
        public JsonResult Create(int IdCurso, int Nivel, string Descripcion, string Estado)
        {
            string strResult = string.Empty;
            bool bResult = false;

            ProyectoFinalEntities dbGrabar = new ProyectoFinalEntities();
            //string userName = Request.Cookies["UserCode"].Value;
            string userName = User.Identity.Name;

            if (esValido(Nivel,Descripcion,Estado))
            {
                Curso objCurso = new Curso();
                objCurso.Nivel = Nivel;
                objCurso.Descripcion = Descripcion;
                objCurso.Estado = Estado;

                if (IdCurso == 0)
                {
                    List<Curso> cursos = new List<Curso>();
                    cursos = db.Curso.AsNoTracking().ToList();
                    objCurso.IdCurso = cursos.Count() == 0 ? 1 : cursos.Max(x => x.IdCurso) + 1;

                    //objCurso.UsuarioCreacion = 1;
                    objCurso.UsuarioCreacion = userName;
                    objCurso.FechaCreacion = DateTime.Now;
                    dbGrabar.Entry(objCurso).State = EntityState.Added;
                }
                else
                {
                    Curso consCurso = db.Curso.AsNoTracking().Where(x => x.IdCurso == IdCurso).FirstOrDefault();
                    objCurso.IdCurso = IdCurso;
                    objCurso.UsuarioCreacion = consCurso.UsuarioCreacion;
                    objCurso.FechaCreacion = consCurso.FechaCreacion;
                    //objCurso.UsuarioActualizacion = 1;
                    objCurso.UsuarioActualizacion = userName;
                    objCurso.FechaActualizacion = DateTime.Now;
                    dbGrabar.Entry(objCurso).State = EntityState.Modified;
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

        public bool esValido(int Nivel, string Descripcion, string Estado)
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