using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using ProyectoFinalModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class ReportesController : Controller
    {
        ProyectoFinalEntities db = new ProyectoFinalEntities();
        String error = "";
        public static string strConnSQL = string.Empty;
        public static string ServerSql = string.Empty;
        public static string dbName = string.Empty;
        public static string dbUser = string.Empty;
        public static string dbPassword = string.Empty;

        // GET: Reportes
        public ActionResult ListadoPorCurso()
        {
            return View();
        }

        public ActionResult CertificadoPromocion()
        {
            return View();
        }

        public ActionResult ListadoCalificacionesPorCurso()
        {
            return View();
        }

        public ActionResult LibretaAlumno()
        {
            return View();
        }

        public ActionResult MatrizCalificaciones()
        {
            return View();
        }

        public JsonResult GetPeriodosLib(string q, string view)
        {
            var FatherItems = new List<object>().Select(t => new {
                id = default(int),
                text = default(string)
            }).ToList();

            FatherItems = (from maq in db.PeriodoLectivo.AsNoTracking()
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
                                  where maq.text.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) >= 0
                                  select new { id = maq.id, text = maq.text }
                     ).ToList();


                FatherItem.RemoveAll(item => item == null);

                return Json(new { items = FatherItem }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetTiposReportesLib(string q)
        {

            var TiposReportes = new List<object>().Select(t => new {
                id = default(int),
                text = default(string)
            }).ToList();

            TiposReportes.Add(new { id = 1, text = "POR CURSO" });
            TiposReportes.Add(new { id = 2, text = "POR ALUMNO" });

            var FatherItem = TiposReportes.Where(x => x.text.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) >= 0);

            return Json(new { items = FatherItem }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetOfertasLPC(string q, int? IdPeriodoLectivo)
        {

            string sValue = q;            

            if (string.IsNullOrEmpty(q))
            {
                sValue = string.Empty;
            }

            var Ofertas = new List<object>().Select(t => new {
                id = default(int),
                text = default(string)
            }).ToList();

            if (IdPeriodoLectivo != null && IdPeriodoLectivo > 0)
            {
                int idrol = Convert.ToInt32(Request.Cookies["Rol"].Value.ToString());

                //Si es un profesor solo carga los cursos a los que el esta asignado, si no carga todos
                if (idrol == 3)
                {

                    string IdUsuario = Request.Cookies["UserID"].Value.ToString();

                    Ofertas = (from ofer in db.Oferta.AsNoTracking()
                               join per in db.PeriodoLectivo.AsNoTracking() on ofer.IdPeriodoLectivo equals per.IdPeriodoLectivo
                               join cur in db.Curso.AsNoTracking() on ofer.IdCurso equals cur.IdCurso
                               join par in db.Paralelo.AsNoTracking() on ofer.IdParalelo equals par.IdParalelo

                               join clec in db.CargaLectiva.AsNoTracking() on ofer.IdOferta equals clec.IdOferta
                               join prof in db.Profesor.AsNoTracking() on clec.IdProfesor equals prof.IdProfesor
                               join usu in db.Usuario.AsNoTracking() on prof.Cedula equals usu.Cedula

                               where (cur.Descripcion.Contains(sValue) || par.Descripcion.Contains(sValue))
                               //&& ofer.Estado.Equals("A") && per.Estado.Equals("A") && cur.Estado.Equals("A") && par.Estado.Equals("A")
                               && usu.IdUsuario == IdUsuario
                               orderby cur.Nivel ascending, par.Descripcion ascending
                               select new
                               {
                                   id = ofer.IdOferta,
                                   text = cur.Descripcion + " " + par.Descripcion + " | " + (ofer.Jornada == "MAT" ? "MATUTINA" : "VESPERTINA")
                               }).Distinct().ToList();
                    Ofertas.RemoveAll(item => item == null);
                }
                else
                {
                    Ofertas = (from ofer in db.Oferta.AsNoTracking()
                               join per in db.PeriodoLectivo.AsNoTracking() on ofer.IdPeriodoLectivo equals per.IdPeriodoLectivo
                               join cur in db.Curso.AsNoTracking() on ofer.IdCurso equals cur.IdCurso
                               join par in db.Paralelo.AsNoTracking() on ofer.IdParalelo equals par.IdParalelo
                               where (cur.Descripcion.Contains(sValue) || par.Descripcion.Contains(sValue))
                               && ofer.IdPeriodoLectivo == IdPeriodoLectivo
                               //&& ofer.Estado.Equals("A") && per.Estado.Equals("A") && cur.Estado.Equals("A") && par.Estado.Equals("A")
                               orderby cur.Nivel ascending, par.Descripcion ascending
                               select new
                               {
                                   id = ofer.IdOferta,
                                   text = cur.Descripcion + " " + par.Descripcion + " | " + (ofer.Jornada == "MAT" ? "MATUTINA" : "VESPERTINA")
                               }).ToList();
                    Ofertas.RemoveAll(item => item == null);
                }

                //var Ofertas = (from ofer in db.Oferta.AsNoTracking()
                //               join per in db.PeriodoLectivo.AsNoTracking() on ofer.IdPeriodoLectivo equals per.IdPeriodoLectivo
                //               join cur in db.Curso.AsNoTracking() on ofer.IdCurso equals cur.IdCurso
                //               join par in db.Paralelo.AsNoTracking() on ofer.IdParalelo equals par.IdParalelo
                //               where (cur.Descripcion.Contains(sValue) || par.Descripcion.Contains(sValue)) 
                //               && ofer.Estado.Equals("A") && per.Estado.Equals("A") && cur.Estado.Equals("A") && par.Estado.Equals("A")
                //               select new
                //               {
                //                   id = ofer.IdOferta,
                //                   text = cur.Descripcion + " " + par.Descripcion + " | " + (ofer.Jornada == "MAT" ? "MATUTINA" : "VESPERTINA")
                //               }).ToList();
                //Ofertas.RemoveAll(item => item == null);
            }

            return Json(new { items = Ofertas }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetOfertasLib(string q, int? IdPeriodoLectivo)
        {

            var Ofertas = new List<object>().Select(t => new {
                id = default(int),
                text = default(string)
            }).ToList();

            if (IdPeriodoLectivo != null && IdPeriodoLectivo > 0)
            {
                string sValue = q;

                if (string.IsNullOrEmpty(q))
                {
                    sValue = string.Empty;
                }

                Ofertas = (from ofer in db.Oferta.AsNoTracking()
                           join per in db.PeriodoLectivo.AsNoTracking() on ofer.IdPeriodoLectivo equals per.IdPeriodoLectivo
                           join cur in db.Curso.AsNoTracking() on ofer.IdCurso equals cur.IdCurso
                           join par in db.Paralelo.AsNoTracking() on ofer.IdParalelo equals par.IdParalelo
                           where (cur.Descripcion.Contains(sValue) || par.Descripcion.Contains(sValue))
                           && ofer.IdPeriodoLectivo == IdPeriodoLectivo
                           orderby cur.Nivel ascending, par.Descripcion ascending
                           //&& per.Estado.Equals("A") && cur.Estado.Equals("A") && par.Estado.Equals("A")
                           select new
                           {
                               id = ofer.IdOferta,
                               text = cur.Descripcion + " " + par.Descripcion + " | " + (ofer.Jornada == "MAT" ? "MATUTINA" : "VESPERTINA")
                           }).ToList();
                Ofertas.RemoveAll(item => item == null);
            }            

            var FatherItem = Ofertas.Where(x => x.text.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) >= 0);

            return Json(new { items = FatherItem }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAlumnosCP(string q)
        {

            string sValue = q;

            if (string.IsNullOrEmpty(q))
            {
                sValue = string.Empty;
            }

            var Alumnos = (from alum in db.Alumno.AsNoTracking()
                           where (alum.Cedula.Contains(sValue) || alum.Nombres.Contains(sValue) || alum.Apellidos.Contains(sValue))
                           select new
                           {
                               id = alum.IdAlumno,
                               text = alum.Cedula + " | " + alum.Apellidos + " " + alum.Nombres
                           }).ToList();
            Alumnos.RemoveAll(item => item == null);

            return Json(new { items = Alumnos }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAlumnosLib(string q, int? IdPeriodoLectivo)
        {

            var Alumnos = new List<object>().Select(t => new {
                id = default(int),
                text = default(string)
            }).ToList();

            if (IdPeriodoLectivo != null && IdPeriodoLectivo > 0)
            {
                string sValue = q;

                if (string.IsNullOrEmpty(q))
                {
                    sValue = string.Empty;
                }

                Alumnos = (from alum in db.Alumno.AsNoTracking()
                           join mat in db.Matricula.AsNoTracking() on alum.IdAlumno equals mat.IdAlumno
                           join ofer in db.Oferta.AsNoTracking() on mat.IdOferta equals ofer.IdOferta
                           where (alum.Cedula.Contains(sValue) || alum.Nombres.Contains(sValue) || alum.Apellidos.Contains(sValue))
                           && ofer.IdPeriodoLectivo == IdPeriodoLectivo && mat.Anulado == "N"
                           select new
                           {
                               id = alum.IdAlumno,
                               text = alum.Cedula + " | " + alum.Apellidos + " " + alum.Nombres
                           }).ToList();
                Alumnos.RemoveAll(item => item == null);
            }            

            var FatherItem = Alumnos.Where(x => x.text.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) >= 0);

            return Json(new { items = FatherItem }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetParcialesRCPC(string q)
        {

            var Jornadas = new List<object>().Select(t => new {
                id = default(string),
                text = default(string)
            }).ToList();

            Jornadas.Add(new { id = "1P 1Q", text = "1ER PARCIAL - 1ER QUIMESTRE" });
            Jornadas.Add(new { id = "2P 1Q", text = "2DO PARCIAL - 1ER QUIMESTRE" });
            Jornadas.Add(new { id = "3P 1Q", text = "3ER PARCIAL - 1ER QUIMESTRE" });
            Jornadas.Add(new { id = "EX 1Q", text = "EXAMEN - 1ER QUIMESTRE" });
            Jornadas.Add(new { id = "1P 2Q", text = "1ER PARCIAL - 2DO QUIMESTRE" });
            Jornadas.Add(new { id = "2P 2Q", text = "2DO PARCIAL - 2DO QUIMESTRE" });
            Jornadas.Add(new { id = "3P 2Q", text = "3ER PARCIAL - 2DO QUIMESTRE" });
            Jornadas.Add(new { id = "EX 2Q", text = "EXAMEN - 2DO QUIMESTRE" });

            var FatherItem = Jornadas.Where(x => x.text.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) >= 0);

            return Json(new { items = FatherItem }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintCertPromo(string txtIdAlumno)
        {
            string sQuery = "EXEC SP_PROMEDIOS_POR_EST_PERLEC @Alumno = " + txtIdAlumno;
            return ExportDataProduccion("CertPromo", "Certificado Promocion ", sQuery);
        }

        public ActionResult PrintListadoPorCurso(string txtIdOferta)
        {          
            string sQuery = "EXEC SP_LISTADO_POR_CURSO @Oferta = " + txtIdOferta;
            return ExportDataProduccion("ListadoPorCurso", "Listado Por Curso ", sQuery);                      
        }

        public ActionResult PrintListadoCalificacionesPorCurso(string txtIdOferta, string txtIdParcial)
        {
            string sQuery = "EXEC SP_CALIFICACIONES_CURSO_PARCIAL @Oferta = " + txtIdOferta + ", @Parcial = '" + txtIdParcial + "'";
            return ExportDataProduccion("ListadoCalificacionesPorCurso", "Listado de Calificaciones Por Curso ", sQuery);
        }

        public ActionResult PrintLibretaAlumno(string txtIdPeriodo, string txtIdTipoReporte, string txtIdOferta, string txtIdAlumno)
        {
            //string sQuery = "EXEC SP_CALIFICACIONES_CURSO_PARCIAL @Oferta = " + txtIdOferta + ", @Parcial = '" + txtIdParcial + "'";
            string sQuery = "";
            string nomRep = "";

            if (txtIdTipoReporte == "1")
            {
                sQuery = "EXEC SP_LIBRETAS_POR_CURSO @IdOferta = " + txtIdOferta;
                nomRep = "LibretasPorCurso";
            }
            else
            {
                sQuery = "EXEC SP_LIBRETA_POR_ALUMNO @IdAlumno = " + txtIdAlumno;
                nomRep = "LibretaPorAlumno";
            }

            return ExportDataProduccion(nomRep, "Libreta(s) de calificaciones ", sQuery);
        }

        public ActionResult PrintMatrizCalificaciones(string txtIdOferta)
        {
            string sQuery = "EXEC SP_SABANA_NOTAS @IdOferta = " + txtIdOferta;
            return ExportDataProduccion("MatrizCalificaciones", "Matriz de Calificaciones ", sQuery);
        }

        public ActionResult ExportDataProduccion(string psNomRep, string NomArc, string psQuery)
        {
            strConnSQL = ConfigurationManager.ConnectionStrings["ProyectoFinalCN"].ConnectionString;
            ServerSql = ConfigurationManager.AppSettings["Server"];
            dbName = ConfigurationManager.AppSettings["DataBase_SQL"];
            dbUser = ConfigurationManager.AppSettings["User_SQL"];
            dbPassword = ConfigurationManager.AppSettings["Password_SQL"];

            ReportDocument rd = new ReportDocument();
            //rd.Load(Path.Combine(Server.MapPath("~/bin/Reports"), psNomRep + ".rpt"));
            rd.Load(Path.Combine(Server.MapPath("~/bin/Reports"), psNomRep + ".rpt"));

            TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            Tables CrTables;

            crConnectionInfo.ServerName = ServerSql;
            crConnectionInfo.DatabaseName = dbName;
            crConnectionInfo.UserID = dbUser;
            crConnectionInfo.Password = dbPassword;

            CrTables = rd.Database.Tables;

            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            {
                crtableLogoninfo = CrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                CrTable.ApplyLogOnInfo(crtableLogoninfo);
            }
            
            rd.SetDatabaseLogon(dbUser, dbPassword, ServerSql, dbName);

            string parameters = string.Empty;

            DataSet dtsConsulta = new DataSet();            
            string ConnectionStrinHANA = strConnSQL;
            SqlConnection conn = new SqlConnection(ConnectionStrinHANA);
            conn.Open();

            string sQuery = psQuery;

            SqlCommand cmd = new SqlCommand(sQuery, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dtsConsulta);

            conn.Close();

            rd.SetDataSource(dtsConsulta.Tables[0]);
            rd.Refresh();

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream1 = rd.ExportToStream(formatType: ExportFormatType.PortableDocFormat);
            stream1.Seek(0, SeekOrigin.Begin);

            rd.Close();
            stream1.Flush(); //Always catches me out
            stream1.Position = 0; //Not sure if this is required

            string strDateNow = DateTime.Now.ToString("ddMMyyyy HH:mm:ss").ToString();
            strDateNow = strDateNow.Replace(":", "");

            string filName = string.Concat(NomArc, strDateNow, ".pdf");

            return File(stream1, "application/pdf", filName);
        }

    }
}