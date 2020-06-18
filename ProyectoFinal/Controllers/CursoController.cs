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

        public static clsCapaAcceso.AccesoDatos.SQLServer objSQL;

        // GET: Curso
        public ActionResult Index()
        {
            
            List<Curso> c = db.Curso.AsNoTracking().ToList();            
            return View(c);            
        }

        //[HttpGet]
        public ActionResult ExportDataProduccion()
        {
            ReportDocument rd = new ReportDocument();                        
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "prueba.rpt"));

            TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            Tables CrTables;

            crConnectionInfo.ServerName = "35.222.165.220";
            crConnectionInfo.DatabaseName = "ProyectoFinal";
            crConnectionInfo.UserID = "admin";
            crConnectionInfo.Password = "admin";

            CrTables = rd.Database.Tables;

            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            {
                crtableLogoninfo = CrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                CrTable.ApplyLogOnInfo(crtableLogoninfo);
            }

            rd.SetDatabaseLogon("admin", "admin", "35.222.165.220", "ProyectoFinal");

            string parameters = string.Empty;

            DataSet dtsConsulta = new DataSet();
            string ConnectionStrinHANA = "Data Source=35.222.165.220;Initial Catalog=ProyectoFinal;User ID=admin;Password=admin;Trusted_Connection=False;";
            SqlConnection conn = new SqlConnection(ConnectionStrinHANA);
            conn.Open();

            string sQuery = "select IdUsuario from Usuario";

            SqlCommand cmd = new SqlCommand(sQuery, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dtsConsulta);

            conn.Close();

            //string sQuery = "exec sprProDetalleProduccion ";
            //parameters = string.Concat(parameters, "  @CCiMaquina = '", CCiMaquina, "'");
            //parameters = string.Concat(parameters, ", @CCiMolde = '", CCiMolde, "'");
            //parameters = string.Concat(parameters, ", @NNuIdOF = ", NNuIdOF);

            //sQuery = string.Concat(sQuery, parameters);

            //DataSet dtsConsulta = GlobalSQL.objSQL.TraerDataSetSql(sQuery);

            //DataSet dtsConsulta = objSQL.TraerDataSetSql(sQuery);

            //rd.SetParameterValue("@CCiMaquina", CCiMaquina);
            //rd.SetParameterValue("@CCiMolde", CCiMolde);
            //rd.SetParameterValue("@NNuIdOF", NNuIdOF);

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

            string filName = string.Concat("Prueba ", strDateNow, ".pdf");

            return File(stream1, "application/pdf", filName);
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

                    objCurso.UsuarioCreacion = 1;
                    objCurso.FechaCreacion = DateTime.Now;
                    dbGrabar.Entry(objCurso).State = EntityState.Added;
                }
                else
                {
                    Curso consCurso = db.Curso.AsNoTracking().Where(x => x.IdCurso == IdCurso).FirstOrDefault();
                    objCurso.IdCurso = IdCurso;
                    objCurso.UsuarioCreacion = consCurso.UsuarioCreacion;
                    objCurso.FechaCreacion = consCurso.FechaCreacion;
                    objCurso.UsuarioActualizacion = 1;
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