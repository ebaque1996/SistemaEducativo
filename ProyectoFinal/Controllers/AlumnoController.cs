using ProyectoFinalModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class AlumnoController : Controller
    {

        ProyectoFinalEntities db = new ProyectoFinalEntities();
        String error = "";

        // GET: Alumno
        public ActionResult Index()
        {

            List<Alumno> alumnos = db.Alumno.AsNoTracking().ToList();
            return View(alumnos);
        }

        public ActionResult Edit(int id)
        {
            Alumno objAlumno = new Alumno();
            objAlumno = db.Alumno.AsNoTracking().Where(x => x.IdAlumno == id).FirstOrDefault();
            return View(objAlumno);
        }

        public ActionResult Create()
        {
            Alumno objAlumno = new Alumno();
            return View(objAlumno);
        }

        //[HttpPost]
        //public JsonResult Create(string Descripcion, string Estado)
        //{
        //    return Json(new { Message = "a", bResultado = true }, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetCursosIngreso(string q)
        {
            var FatherItems = (from maq in db.Curso.AsNoTracking()
                               where maq.Estado == "A"
                               select new { id = maq.IdCurso, text = maq.Descripcion }
                   ).ToList();

            FatherItems.Add(new { id = 0, text = "Primer Ingreso a una Institución Educativa" });
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

                FatherItems.Add(new { id = 0, text = "Primer Ingreso a una Institución Educativa" });
                FatherItem.RemoveAll(item => item == null);
                return Json(new { items = FatherItem }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetCursosEdicion(string q)
        {
            var FatherItems = (from maq in db.Curso.AsNoTracking()
                               where maq.Estado == "A"
                               select new { id = maq.IdCurso, text = maq.Descripcion }
                   ).ToList();

            FatherItems.Add(new { id = 0, text = "Primer Ingreso a una Institución Educativa" });
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

                FatherItems.Add(new { id = 0, text = "Primer Ingreso a una Institución Educativa" });
                FatherItem.RemoveAll(item => item == null);
                return Json(new { items = FatherItem }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetDetalleCurso(int idCurso)
        {
            Curso objCurso = new Curso();
            objCurso = db.Curso.AsNoTracking().Where(x => x.IdCurso == idCurso).FirstOrDefault();

            return Json(new { data = objCurso }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult ConsultarAlumnos()
        {
            ProyectoFinalEntities db = new ProyectoFinalEntities();
            List<Alumno> listAlumno = new List<Alumno>();

            listAlumno = db.Alumno.ToList();

            return Json(new { data = listAlumno }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAlumnos(int? idperiodo, int? idoferta, int? idmateria, int? idalumno, int? idestado)
        {
            List<Alumno> listAlumno = new List<Alumno>();

            string est = "";

            if (idestado != null)
            {
                est = idestado == 1 ? "A" : "I";
            }

            var model = (from deta in db.Alumno.AsNoTracking()
                         //join ofer in db.Oferta.AsNoTracking() on deta.IdOferta equals ofer.IdOferta
                         //where deta.NNuIdOF == NNuIdOF
                         select new
                         {
                             deta.IdAlumno,
                             deta.Cedula,
                             deta.Nombres,
                             deta.Apellidos,
                             deta.Estado
                         });

            if (idestado != null)
            {
                model = model.Where(x => x.Estado == est);
            }

            //if (idperiodo != null)
            //{
            //    model = model.Where(x => x.IdPeriodoLectivo == idperiodo);
            //}

            //if (idoferta != null)
            //{
            //    model = model.Where(x => x.IdOferta == idoferta);
            //}

            //if (idmateria != null)
            //{
            //    model = model.Where(x => x.IdMateria == idmateria);
            //}

            if (idalumno != null)
            {
                model = model.Where(x => x.IdAlumno == idalumno);
            }

            var model2 = model.ToList();

            foreach (var item in model2)
            {
                Alumno objAlumno = new Alumno();
                objAlumno.IdAlumno = item.IdAlumno;
                objAlumno.Cedula = item.Cedula;
                objAlumno.Nombres = item.Nombres;
                objAlumno.Apellidos = item.Apellidos;
                objAlumno.Estado = item.Estado;
                listAlumno.Add(objAlumno);
            }

            //return View(listAlumno);
            return Json(new { detsCargasLectivas = listAlumno }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(int IdAlumno, string Cedula, string Nombres, string Apellidos, DateTime FechaNac, 
                                string Sexo, string Direccion, string Telefono, int UltimoNivel, string CedulaRepresentante, 
                                string NombresRepresentante, string ApellidosRepresentante, string TelefonoRepresentante, 
                                string DireccionRepresentante, string ColegioAnterior, string Estado)
        {
            string strResult = string.Empty;
            bool bResult = false;

            ProyectoFinalEntities dbGrabar = new ProyectoFinalEntities();

            if (esValido(IdAlumno, Cedula, Nombres, Apellidos, FechaNac, Sexo, Direccion, Telefono, UltimoNivel, CedulaRepresentante, 
                NombresRepresentante, ApellidosRepresentante, TelefonoRepresentante, DireccionRepresentante, ColegioAnterior, Estado))
            {
                Alumno objAlumno = new Alumno();
                objAlumno.Cedula = Cedula;
                objAlumno.Nombres = Nombres;
                objAlumno.Apellidos = Apellidos;
                objAlumno.FechaNac = FechaNac;
                objAlumno.Sexo = Sexo;
                objAlumno.Direccion = Direccion;
                objAlumno.Telefono = Telefono;
                objAlumno.UltimoNivel = UltimoNivel;
                objAlumno.CedulaRepresentante = CedulaRepresentante;
                objAlumno.NombresRepresentante = NombresRepresentante;
                objAlumno.ApellidosRepresentante = ApellidosRepresentante;
                objAlumno.TelefonoRepresentante = TelefonoRepresentante;
                objAlumno.DireccionRepresentante = DireccionRepresentante;
                objAlumno.ColegioAnterior = ColegioAnterior;
                objAlumno.Estado = Estado;

                if (IdAlumno == 0)
                {
                    List<Alumno> alumnos = new List<Alumno>();
                    alumnos = db.Alumno.AsNoTracking().ToList();
                    objAlumno.IdAlumno = alumnos.Count() == 0 ? 1 : alumnos.Max(x => x.IdAlumno) + 1;

                    objAlumno.UsuarioCreacion = 1;
                    objAlumno.FechaCreacion = DateTime.Now;
                    dbGrabar.Entry(objAlumno).State = EntityState.Added;
                }
                else
                {
                    Alumno consAlumno = db.Alumno.AsNoTracking().Where(x => x.IdAlumno == IdAlumno).FirstOrDefault();
                    objAlumno.IdAlumno = IdAlumno;
                    objAlumno.UsuarioCreacion = consAlumno.UsuarioCreacion;
                    objAlumno.FechaCreacion = consAlumno.FechaCreacion;
                    objAlumno.UsuarioActualizacion = 1;
                    objAlumno.FechaActualizacion = DateTime.Now;
                    dbGrabar.Entry(objAlumno).State = EntityState.Modified;
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

        public bool esValido(int IdAlumno, string Cedula, string Nombres, string Apellidos, DateTime FechaNac,
                            string Sexo, string Direccion, string Telefono, int UltimoNivel, string CedulaRepresentante,
                            string NombresRepresentante, string ApellidosRepresentante, string TelefonoRepresentante,
                            string DireccionRepresentante, string ColegioAnterior, string Estado)
        {

            if (String.IsNullOrEmpty(Cedula))
            {
                error = "Debe ingresar la Cédula";
                return false;
            }

            //Si es creacion siempre va a verificar la cedula y si ya existe en otro alumno, si es edicion consulto la cedula grabada actualmente y si es la
            //misma que la que ingresa en la pantalla no ejecuta metodo (xq no la ha cambiado), si es que no es la misma (la cambio) ahi va a verificar si la
            //cedula ya pertenece a otro alumno y si es valida

            if (IdAlumno == 0)
            {                

                if (cedulaRegistrada(Cedula))
                {
                    error = "Ya Existe un Alumno registrado con esta Cédula";
                    return false;
                }

                if (!VerificaIdentificacion(Cedula))
                {
                    error = "Cédula Inválida";
                    return false;
                }
            }
            else
            {
                Alumno objAlumno = db.Alumno.AsNoTracking().Where(x => x.IdAlumno == IdAlumno).FirstOrDefault();
                if (objAlumno != null)
                {
                    if (Cedula != objAlumno.Cedula)
                    {
                        if (cedulaRegistrada(Cedula))
                        {
                            error = "Ya Existe un Alumno registrado con esta Cédula";
                            return false;
                        }

                        if (!VerificaIdentificacion(Cedula))
                        {
                            error = "Cédula Inválida";
                            return false;
                        }
                    }
                }
                else
                {
                    error = "El Alumno no existe";
                    return false;
                }
            }            

            //if (!VerificaIdentificacion(Cedula)) 
            //{
            //    error = "Cédula Inválida";
            //    return false;
            //}

            //if (cedulaRegistrada(Cedula))
            //{
            //    error = "Ya Existe un Alumno registrado con esta Cédula";
            //    return false;
            //}


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

            if (FechaNac == Convert.ToDateTime("01/01/1900"))
            {
                error = "Debe ingresar la Fecha de Nacimiento";
                return false;
            }

            if (String.IsNullOrEmpty(Sexo))
            {
                error = "Debe ingresar el Sexo";
                return false;
            }

            if (String.IsNullOrEmpty(Direccion))
            {
                error = "Debe ingresar la Dirección";
                return false;
            }

            if (String.IsNullOrEmpty(Telefono))
            {
                error = "Debe ingresar el Teléfono";
                return false;
            }

            /*if (UltimoNivel==0)
            {
                error = "Debe ingresar el Último Nivel";
                return false;
            }*/

            if (String.IsNullOrEmpty(CedulaRepresentante))
            {
                error = "Debe ingresar la Cédula del Representante";
                return false;
            }

            if (String.IsNullOrEmpty(NombresRepresentante))
            {
                error = "Debe ingresar los Nombres del Representante";
                return false;
            }

            if (String.IsNullOrEmpty(ApellidosRepresentante))
            {
                error = "Debe ingresar los Apellidos del Representante";
                return false;
            }

            if (String.IsNullOrEmpty(TelefonoRepresentante))
            {
                error = "Debe ingresar el Teléfono del Representante";
                return false;
            }

            if (String.IsNullOrEmpty(DireccionRepresentante))
            {
                error = "Debe ingresar la Dirección del Representante";
                return false;
            }

            if (String.IsNullOrEmpty(ColegioAnterior))
            {
                error = "Debe ingresar el Colegio Anterior";
                return false;
            }
            
            return true;
        }

        public bool cedulaRegistrada(string Cedula)
        {
            var model = (from alu in db.Alumno.AsNoTracking()
                         where alu.Cedula.Contains(Cedula)
                         select new { id = alu.IdAlumno }
                        ).ToList().FirstOrDefault();

            if (model == null)
            {
                return false;
            }

            return true;
        }

        internal static bool VerificaIdentificacion(string identificacion)
        {
            bool estado = false;
            char[] valced = new char[13];
            int provincia;
            if (identificacion.Length >= 10)
            {
                valced = identificacion.Trim().ToCharArray();
                provincia = int.Parse((valced[0].ToString() + valced[1].ToString()));
                if (provincia > 0 && provincia < 25)
                {
                    if (int.Parse(valced[2].ToString()) < 6)
                    {
                        estado = VerificaCedula(valced);
                    }
                    /*else if (int.Parse(valced[2].ToString()) == 6)
                    {
                        estado = VerificaSectorPublico(valced);
                    }
                    else if (int.Parse(valced[2].ToString()) == 9)
                    {
                        estado = VerificaPersonaJuridica(valced);
                    }*/
                }
            }
            return estado;
        }

        public static bool VerificaCedula(char[] validarCedula)
        {
            int aux = 0, par = 0, impar = 0, verifi;
            for (int i = 0; i < 9; i += 2)
            {
                aux = 2 * int.Parse(validarCedula[i].ToString());
                if (aux > 9)
                    aux -= 9;
                par += aux;
            }
            for (int i = 1; i < 9; i += 2)
            {
                impar += int.Parse(validarCedula[i].ToString());
            }

            aux = par + impar;
            if (aux % 10 != 0)
            {
                verifi = 10 - (aux % 10);
            }
            else
                verifi = 0;
            if (verifi == int.Parse(validarCedula[9].ToString()))
                return true;
            else
                return false;
        }



        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }
    }
}