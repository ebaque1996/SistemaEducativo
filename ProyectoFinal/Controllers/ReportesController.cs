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

        [HttpGet]
        public JsonResult GetOfertasLPC(string q)
        {

            string sValue = q;

            if (string.IsNullOrEmpty(q))
            {
                sValue = string.Empty;
            }

            var Ofertas = (from ofer in db.Oferta.AsNoTracking()
                           join per in db.PeriodoLectivo.AsNoTracking() on ofer.IdPeriodoLectivo equals per.IdPeriodoLectivo
                           join cur in db.Curso.AsNoTracking() on ofer.IdCurso equals cur.IdCurso
                           join par in db.Paralelo.AsNoTracking() on ofer.IdParalelo equals par.IdParalelo
                           where (cur.Descripcion.Contains(sValue) || par.Descripcion.Contains(sValue)) && ofer.Estado.Equals("A")
                                  && per.Estado.Equals("A") && cur.Estado.Equals("A") && par.Estado.Equals("A")
                           select new
                           {
                               id = ofer.IdOferta,
                               text = cur.Descripcion + " " + par.Descripcion + " | " + (ofer.Jornada == "MAT" ? "MATUTINA" : "VESPERTINA")
                           }).ToList();
            Ofertas.RemoveAll(item => item == null);

            return Json(new { items = Ofertas }, JsonRequestBehavior.AllowGet);
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
                           //join per in db.PeriodoLectivo.AsNoTracking() on ofer.IdPeriodoLectivo equals per.IdPeriodoLectivo
                           //join cur in db.Curso.AsNoTracking() on ofer.IdCurso equals cur.IdCurso
                           //join par in db.Paralelo.AsNoTracking() on ofer.IdParalelo equals par.IdParalelo
                           where (alum.Cedula.Contains(sValue) || alum.Nombres.Contains(sValue) || alum.Apellidos.Contains(sValue))
                           select new
                           {
                               id = alum.IdAlumno,
                               text = alum.Cedula + " | " + alum.Apellidos + " " + alum.Nombres
                           }).ToList();
            Alumnos.RemoveAll(item => item == null);

            return Json(new { items = Alumnos }, JsonRequestBehavior.AllowGet);
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


        public ActionResult ExportDataProduccion(string psNomRep, string NomArc, string psQuery)
        {
            strConnSQL = ConfigurationManager.ConnectionStrings["ProyectoFinalCN"].ConnectionString;
            ServerSql = ConfigurationManager.AppSettings["Server"];
            dbName = ConfigurationManager.AppSettings["DataBase_SQL"];
            dbUser = ConfigurationManager.AppSettings["User_SQL"];
            dbPassword = ConfigurationManager.AppSettings["Password_SQL"];

            ReportDocument rd = new ReportDocument();
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