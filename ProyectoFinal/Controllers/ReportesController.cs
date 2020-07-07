using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using ProyectoFinalModel;
using System;
using System.Collections.Generic;
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

        // GET: Reportes
        public ActionResult ListadoPorCurso()
        {
            return View();
        }

        public ActionResult CertificadoPromocion()
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
                               text = cur.Descripcion + " " + par.Descripcion
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

        public ActionResult ExportDataProduccion(string txtIdOferta)
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "ListadoPorCurso.rpt"));

            TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            Tables CrTables;

            crConnectionInfo.ServerName = "DESKTOP-0B7JPLB\\SQLEXPRESS";
            crConnectionInfo.DatabaseName = "ProyectoFinal";
            crConnectionInfo.UserID = "sa";
            crConnectionInfo.Password = "Barcelonasc1";

            CrTables = rd.Database.Tables;

            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            {
                crtableLogoninfo = CrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                CrTable.ApplyLogOnInfo(crtableLogoninfo);
            }

            rd.SetDatabaseLogon("sa", "Barcelonasc1", "DESKTOP-0B7JPLB\\SQLEXPRESS", "ProyectoFinal");

            string parameters = string.Empty;

            DataSet dtsConsulta = new DataSet();
            string ConnectionStrinHANA = "Data Source=DESKTOP-0B7JPLB\\SQLEXPRESS;Initial Catalog=ProyectoFinal;User ID=sa;Password=Barcelonasc1;Trusted_Connection=False;";
            SqlConnection conn = new SqlConnection(ConnectionStrinHANA);
            conn.Open();

            string sQuery = "EXEC SP_LISTADO_POR_CURSO @Oferta = " + txtIdOferta;

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

            string filName = string.Concat("Listado ", strDateNow, ".pdf");

            return File(stream1, "application/pdf", filName);
        }

        public ActionResult PrintCertPromo(string txtIdAlumno)
        {
            string sQuery = "EXEC SP_PROMEDIOS_POR_EST_PERLEC @Alumno = " + txtIdAlumno;
            return ExportDataProduccion2("CertPromo", "Certificado Promocion ", sQuery);
        }


        public ActionResult ExportDataProduccion2(string psNomRep, string NomArc, string psQuery)
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), psNomRep + ".rpt"));

            TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            Tables CrTables;

            crConnectionInfo.ServerName = "DESKTOP-0B7JPLB\\SQLEXPRESS";
            crConnectionInfo.DatabaseName = "ProyectoFinal";
            crConnectionInfo.UserID = "sa";
            crConnectionInfo.Password = "Barcelonasc1";

            CrTables = rd.Database.Tables;

            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            {
                crtableLogoninfo = CrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                CrTable.ApplyLogOnInfo(crtableLogoninfo);
            }

            rd.SetDatabaseLogon("sa", "Barcelonasc1", "DESKTOP-0B7JPLB\\SQLEXPRESS", "ProyectoFinal");

            string parameters = string.Empty;

            DataSet dtsConsulta = new DataSet();
            string ConnectionStrinHANA = "Data Source=DESKTOP-0B7JPLB\\SQLEXPRESS;Initial Catalog=ProyectoFinal;User ID=sa;Password=Barcelonasc1;Trusted_Connection=False;";
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