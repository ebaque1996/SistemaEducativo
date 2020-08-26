using ProyectoFinalModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class PeriodoLectivoController : Controller
    {

        ProyectoFinalEntities db = new ProyectoFinalEntities();
        String error = "";
        public static string strConnSQL = string.Empty;

        // GET: PeriodoLectivo
        public ActionResult Index()
        {

            List<PeriodoLectivo> profesores = db.PeriodoLectivo.AsNoTracking().ToList();
            return View(profesores);
        }

        public ActionResult CerrarPL()
        {

            //List<PeriodoLectivo> profesores = db.PeriodoLectivo.AsNoTracking().ToList();
            return View();
        }

        public ActionResult Edit(int id)
        {
            PeriodoLectivo objPeriodoLectivo = new PeriodoLectivo();
            objPeriodoLectivo = db.PeriodoLectivo.AsNoTracking().Where(x => x.IdPeriodoLectivo == id).FirstOrDefault();
            return View(objPeriodoLectivo);
        }

        public ActionResult Create()
        {
            PeriodoLectivo objPeriodoLectivo = new PeriodoLectivo();
            return View(objPeriodoLectivo);
        }

        //[HttpPost]
        //public JsonResult Create(string Descripcion, string Estado)
        //{
        //    return Json(new { Message = "a", bResultado = true }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult ConsultarPeriodoLectivos()
        {
            ProyectoFinalEntities db = new ProyectoFinalEntities();
            List<PeriodoLectivo> listPeriodoLectivo = new List<PeriodoLectivo>();

            listPeriodoLectivo = db.PeriodoLectivo.ToList();

            return Json(new { data = listPeriodoLectivo }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(int IdPeriodoLectivo, string Descripcion, DateTime FechaInicio, DateTime FechaFin, string Estado)
        {
            string strResult = string.Empty;
            bool bResult = false;

            ProyectoFinalEntities dbGrabar = new ProyectoFinalEntities();

            if (esValido(Descripcion, FechaInicio, FechaFin, Estado))
            {
                PeriodoLectivo objPeriodoLectivo = new PeriodoLectivo();
                objPeriodoLectivo.Descripcion = Descripcion;
                objPeriodoLectivo.FechaInicio = FechaInicio;
                objPeriodoLectivo.FechaFin = FechaFin;
                objPeriodoLectivo.Estado = Estado;

                if (IdPeriodoLectivo == 0)
                {
                    var model = (from per in db.PeriodoLectivo.AsNoTracking()
                                 where per.Estado.Contains("A")
                                 select new
                                 {
                                     per.Estado
                                 }).ToList().FirstOrDefault();

                    if (model != null && Estado.Equals("A"))
                    {
                        bResult = false;
                        strResult = "Ya Existe un Periodo Lectivo Activo";
                    }
                    else
                    {
                        List<PeriodoLectivo> profesores = new List<PeriodoLectivo>();
                        profesores = db.PeriodoLectivo.AsNoTracking().ToList();
                        objPeriodoLectivo.IdPeriodoLectivo = profesores.Count() == 0 ? 1 : profesores.Max(x => x.IdPeriodoLectivo) + 1;

                        objPeriodoLectivo.UsuarioCreacion = 1;
                        objPeriodoLectivo.FechaCreacion = DateTime.Now;
                        dbGrabar.Entry(objPeriodoLectivo).State = EntityState.Added;

                        dbGrabar.SaveChanges();

                        bResult = true;
                        strResult = "Datos Grabados Correctamente";
                    }
                }
                else
                {
                    var model = (from per in db.PeriodoLectivo.AsNoTracking()
                                 where per.Estado.Contains("A") && per.IdPeriodoLectivo != IdPeriodoLectivo
                                 select new
                                 {
                                     per.Estado
                                 }).ToList().FirstOrDefault();

                    if (model != null && Estado.Equals("A"))
                    {
                        bResult = false;
                        strResult = "Ya Existe un Periodo Lectivo Activo";
                    }
                    else
                    {
                        PeriodoLectivo consPeriodoLectivo = db.PeriodoLectivo.AsNoTracking().Where(x => x.IdPeriodoLectivo == IdPeriodoLectivo).FirstOrDefault();
                        objPeriodoLectivo.IdPeriodoLectivo = IdPeriodoLectivo;
                        objPeriodoLectivo.UsuarioCreacion = consPeriodoLectivo.UsuarioCreacion;
                        objPeriodoLectivo.FechaCreacion = consPeriodoLectivo.FechaCreacion;
                        objPeriodoLectivo.UsuarioActualizacion = 1;
                        objPeriodoLectivo.FechaActualizacion = DateTime.Now;
                        dbGrabar.Entry(objPeriodoLectivo).State = EntityState.Modified;

                        dbGrabar.SaveChanges();

                        bResult = true;
                        strResult = "Datos Grabados Correctamente";
                    }
                }
            }
            else
            {
                bResult = false;
                strResult = "Error al grabar el registro: " + error;
            }

            return Json(new { Message = strResult, bResultado = bResult }, JsonRequestBehavior.AllowGet);

        }

        public bool esValido(string Descripcion, DateTime FechaInicio, DateTime FechaFin, string Estado)
        {
            //Console.WriteLine(FechaInicio);
            if (String.IsNullOrEmpty(Descripcion))
            {
                error = "Debe ingresar la descripción";
                return false;
            }

            if (FechaInicio == Convert.ToDateTime("01/01/1900"))
            {
                error = "Debe ingresar la Fecha de Inicio";
                return false;
            }

            if (FechaFin == Convert.ToDateTime("01/01/1900"))
            {
                error = "Debe ingresar la Fecha Fin";
                return false;
            }
            return true;
        }

        public JsonResult GetPeriodosCPL(string q)
        {

            var FatherItems = (from maq in db.PeriodoLectivo.AsNoTracking()
                               where maq.Estado == "A"
                               select new { id = maq.IdPeriodoLectivo, text = maq.Descripcion }
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

        [HttpPost]
        public JsonResult CerrarPeriodoLectivo(string IdPerLec)
        {
            ProyectoFinalEntities db = new ProyectoFinalEntities();
            string strResult = string.Empty;
            bool bResult = false;

            try
            {
                if (String.IsNullOrEmpty(IdPerLec))
                {
                    return Json(new { Message = "Debe ingresar el Periodo Lectivo", bResultado = false }, JsonRequestBehavior.AllowGet);
                }

                strConnSQL = ConfigurationManager.ConnectionStrings["ProyectoFinalCN"].ConnectionString;
                //string psQuery = "EXEC SP_CERRAR_PERIODO_LECTIVO " + IdPerLec.ToString();
                string psQuery = "EXEC SP_CERRAR_PERIODO_LECTIVO " + IdPerLec;

                DataSet dtsConsulta = new DataSet();
                string ConnectionStrinHANA = strConnSQL;
                SqlConnection conn = new SqlConnection(ConnectionStrinHANA);
                conn.Open();

                string sQuery = psQuery;

                SqlCommand cmd = new SqlCommand(sQuery, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtsConsulta);

                conn.Close();

                //DataTable dt = dtsConsulta.Tables[0];
                int count = dtsConsulta.Tables.Count;

                if (count > 0)
                {
                    //DataTable dt = dtsConsulta.Tables[0];
                    DataRow dr = dtsConsulta.Tables[0].Rows[0];
                    bResult = false;
                    strResult = dr["ErrorSp"].ToString();
                }
                else
                {
                    bResult = true;
                    strResult = "Datos grabados correctamente";
                }

            }
            catch (Exception ex)
            {
                bResult = false;
                strResult = ex.Message;
            }            

            return Json(new { Message = strResult, bResultado = bResult }, JsonRequestBehavior.AllowGet);
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

    }
}