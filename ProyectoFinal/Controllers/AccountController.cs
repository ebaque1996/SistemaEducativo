//using ProyectoFinalModel;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.Security;
//using System.Configuration;
//using System.Data;
using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ProyectoFinalModel;
using ProyectoFinal.Clases;
using ProyectoFinal.Models;
//using ProyectoFinal.App_Start;

namespace ProyectoFinal.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        //Globals clsGlobals = new Globals();
        //private PrometeoAdminEntities db = new PrometeoAdminEntities();
        private string mensaje = "";
        //PrometeoAdminEntities db = new PrometeoAdminEntities();
        ProyectoFinalEntities db = new ProyectoFinalEntities();


        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            //Prometeo.PrometeoAdminEntities db = new PrometeoAdminEntities();            

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //tblAdUsuario consusuario = db.tblAdUsuario.Where(x => x.CCiUsuario == model.CCiUsuario && x.CCeUsuario == "A").FirstOrDefault();
            Usuario consusuario = db.Usuario.Where(x => x.IdUsuario == model.CCiUsuario && x.Estado == "A").FirstOrDefault();


            //GlobalSQL.WriteCookie("ServerSQL", ConfigurationManager.AppSettings["ServerSQL"]);
            //GlobalSQL.WriteCookie("DBSQL", ConfigurationManager.AppSettings["DataBase_SQL"]);
            GlobalSQL.WriteCookie("UserCode", model.CCiUsuario);
            //GlobalSQL.WriteCookie("CSnUserSAP", consusuario.CSnUserSAP);

            if (consusuario != null)
            {
                var CCiUsuario = model.CCiUsuario;
                var CTxPassword = model.Password;
                //string CTxPasswordEncryp = Helper.EncodePassword2(CTxPassword).ToString();
                // var CTxPasswordEncryp = Helper.EncodePassword2(CTxPassword.ToString()).ToString();
                var CTxPasswordEncryp = CTxPassword.ToString();
                //var result = Prometeo.General.ValidaUsuario(CCiUsuario, CTxPasswordEncryp);
                var result = ProyectoFinalModel.General.ValidaUsuario(CCiUsuario, CTxPasswordEncryp);

                switch (result)
                {
                    case EstadoAcceso.Success:
                        RecuperaInfoUsuario(CCiUsuario);
                        return RedirectToLocal(returnUrl);
                    case EstadoAcceso.LockedOut:
                        return View("Lockout");
                    case EstadoAcceso.Failure:
                    default:
                        ModelState.AddModelError("", "Intento de inicio de sesión no válido.");
                        return View(model);
                }

                //if (consusuario.CSnUserSAP == "S")
                //{
                //    string loginsap = Globals.SAPB1Conect(model.CCiUsuario, model.Password);
                //    if (loginsap == "Success")
                //    {


                //        RecuperaInfoUsuario(model.CCiUsuario);
                //        return RedirectToLocal(returnUrl);
                //    }
                //    else
                //    {
                //        ModelState.AddModelError("", loginsap);
                //        return View(model);
                //    }
                //}
                //else
                //{
                //    var CCiUsuario = model.CCiUsuario;
                //    var CTxPassword = model.Password;
                //    var CTxPasswordEncryp = Helper.EncodePassword(CTxPassword).ToString();
                //    var result = Prometeo.General.ValidaUsuario(CCiUsuario, CTxPasswordEncryp);

                //    switch (result)
                //    {
                //        case EstadoAcceso.Success:
                //            RecuperaInfoUsuario(CCiUsuario);
                //            return RedirectToLocal(returnUrl);
                //        case EstadoAcceso.LockedOut:
                //            return View("Lockout");
                //        case EstadoAcceso.Failure:
                //        default:
                //            ModelState.AddModelError("", "Intento de inicio de sesión no válido.");
                //            return View(model);
                //    }
                //}
            }
            else
            {
                //ViewBag.Error = "El usuario no existe";
                ModelState.AddModelError("", "El usuario no existe.");
                return View(model);
            }

        }

        private void RecuperaInfoUsuario(string CCiUsuario)
        {
            //PrometeoAdminEntities db = new PrometeoAdminEntities();
            try
            {
                //var objUser = (from user1 in db.tblAdUsuario
                //       .Where(x => x.CCiUsuario.Equals(CCiUsuario))
                //               select user1).FirstOrDefault();

                var objUser = (from user1 in db.Usuario.AsNoTracking()
                       .Where(x => x.IdUsuario.Equals(CCiUsuario))
                               select user1).FirstOrDefault();

                //var objRela = from dpto in db.TblAdDepartamento
                //              join user in db.tblAdUsuario on dpto.CCiDepartamento equals user.CCiDepartamento
                //              where user.CCiUsuario == CCiUsuario
                //              select new { user.CCiDepartamento, dpto.CNoDepartamento };

                //var CTxMail = objUser.CTxEmail.ToString().Trim();
                var CTxMail = objUser.Email.ToString().Trim();

                //var Dpto = objRela.ToList()[0].CNoDepartamento.ToString().Trim();

                //var UserName = string.Concat(objUser.CDsNombre.ToString().Trim(), " ", objUser.CDsApellido.ToString().Trim());
                var UserName = string.Concat(objUser.Nombres.ToString().Trim(), " ", objUser.Apellidos.ToString().Trim());

                if (string.IsNullOrEmpty(CTxMail))
                {
                    CTxMail = "sincorreo@plapasa.com";
                }


                //this.Session["Usuario"] = objUser.CCiUsuario.ToString().Trim();
                this.Session["Usuario"] = objUser.IdUsuario.ToString().Trim();


                // Genero Cookies de Autorización
                FormsAuthentication.SetAuthCookie(CCiUsuario, false);

                /* Graba información en los cookies*/
                //Cookies.StoreInCookie("UserID", "", "UserID", objUser.CCiUsuario.ToString().Trim(), DateTime.Today);
                Cookies.StoreInCookie("UserID", "", "UserID", objUser.IdUsuario.ToString().Trim(), DateTime.Today);

                //Cookies.StoreInCookie("Cargo", "", "Cargo", objUser.CDsCargo.ToString().Trim(), DateTime.Today, true);
                Cookies.StoreInCookie("CTxMail", "", "CTxMail", CTxMail, DateTime.Today, true);
                Cookies.StoreInCookie("UserName", "", "UserName", UserName, DateTime.Today, true);

                Cookies.StoreInCookie("Rol", "", "Rol", objUser.IdRol.ToString(), DateTime.Today, true);

                //Cookies.StoreInCookie("Dpto", "", "Dpto", Dpto, DateTime.Today, true);

                ViewBag.DBName = ConfigurationManager.AppSettings["CompanyDB"];

                //GlobalSQL.WriteCookie("UserID", objUser.CCiUsuario.ToString().Trim());
                GlobalSQL.WriteCookie("UserID", objUser.IdUsuario.ToString().Trim());

                //GlobalSQL.WriteCookie("Cargo", objUser.CDsCargo.ToString().Trim());
                GlobalSQL.WriteCookie("CTxMail", CTxMail);
                GlobalSQL.WriteCookie("UserName", UserName);
                GlobalSQL.WriteCookie("Rol", objUser.IdRol.ToString());
                //GlobalSQL.WriteCookie("Dpto", Dpto);
                GlobalSQL.WriteCookie("DBName", ConfigurationManager.AppSettings["CompanyDB"]);


            }
            catch (Exception)
            {

                throw;
            }

        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Requerir que el usuario haya iniciado sesión con nombre de usuario y contraseña o inicio de sesión externo
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // El código siguiente protege de los ataques por fuerza bruta a los códigos de dos factores. 
            // Si un usuario introduce códigos incorrectos durante un intervalo especificado de tiempo, la cuenta del usuario 
            // se bloqueará durante un período de tiempo especificado. 
            // Puede configurar el bloqueo de la cuenta en IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Código no válido.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        //[HttpPost]       
        //public JsonResult Register(UsuarioModel model)
        //{
        //    PrometeoAdminEntities db = new PrometeoAdminEntities();

        //    if (usuarioValido(model))
        //    {
        //        string claveencrp = Prometeo.Helper.EncodePassword(model.CTxClave);
        //        model.CTxClave = claveencrp;

        //        tblAdUsuario usuario = new tblAdUsuario();

        //        usuario.CCiUsuario = model.CCiUsuario;
        //        usuario.CDsNombre = model.CDsNombre;
        //        usuario.CDsApellido = model.CDsApellido;
        //        usuario.CTxClave = model.CTxClave;
        //        usuario.CTxEmail = model.CTxEmail;
        //        usuario.CCeUsuario = "A";
        //        usuario.DFiIngreso = DateTime.Now;

        //        db.Entry(usuario).State = System.Data.Entity.EntityState.Added;
        //        db.SaveChanges();
        //        ViewBag.Message = "El usuario ha sido creado con éxito";
        //    }
        //    else
        //    {
        //        ViewBag.Message = "Error: " + mensaje;
        //    }

        //    return Json(new JsonResult { });
        //}



        //[HttpGet]
        //public JsonResult GetDepartamentos(string q)
        //{
        //    Prometeo.PrometeoAdminEntities db = new Prometeo.PrometeoAdminEntities();
        //    string sValue = q;

        //    if (string.IsNullOrEmpty(q))
        //    {
        //        sValue = string.Empty;
        //    }

        //    var departamentos = (from maq in db.TblAdDepartamento
        //                         where maq.CNoDepartamento.Contains(sValue)
        //                         select new { id = maq.CCiDepartamento, text = maq.CNoDepartamento }
        //                  ).ToList();
        //    departamentos.RemoveAll(item => item == null);

        //    return Json(new { items = departamentos }, JsonRequestBehavior.AllowGet);
        //}


        //public bool usuarioValido(UsuarioModel model)
        //{
        //    PrometeoAdminEntities db = new PrometeoAdminEntities();

        //    if (String.IsNullOrEmpty(model.CCiUsuario))
        //    {
        //        mensaje = "Debe ingresar username";
        //        return false;
        //    }

        //    if (String.IsNullOrEmpty(model.CDsNombre))
        //    {
        //        mensaje = "Debe ingresar nombre";
        //        return false;
        //    }

        //    if (String.IsNullOrEmpty(model.CDsApellido))
        //    {
        //        mensaje = "Debe ingresar apellido";
        //        return false;
        //    }

        //    if (String.IsNullOrEmpty(model.CTxEmail))
        //    {
        //        mensaje = "Debe ingresar email";
        //        return false;
        //    }

        //    if (String.IsNullOrEmpty(model.CTxClave))
        //    {
        //        mensaje = "Debe ingresar clave";
        //        return false;
        //    }

        //    if (String.IsNullOrEmpty(model.CTxConfirmarClave))
        //    {
        //        mensaje = "Debe ingresar la configuración de clave";
        //        return false;
        //    }

        //    if (model.CTxClave != model.CTxConfirmarClave)
        //    {
        //        mensaje = "Las contraseñas no coinciden";
        //        return false;
        //    }

        //    //Confirmar que el username no exista en otro usuario

        //    tblAdUsuario consUsuario = db.tblAdUsuario.AsNoTracking().Where(x => x.CCiUsuario == model.CCiUsuario).FirstOrDefault();

        //    if (consUsuario != null)
        //    {
        //        mensaje = "Ya existe un usuario con este UserName";
        //        return false;
        //    }

        //    //Confirmar que el correo no exista en otro usuario

        //    tblAdUsuario consEmail = db.tblAdUsuario.AsNoTracking().Where(x => x.CTxEmail == model.CTxEmail).FirstOrDefault();

        //    if (consEmail != null)
        //    {
        //        mensaje = "Ya existe un usuario con este correo";
        //        return false;
        //    }            

        //    return true;
        //}

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // No revelar que el usuario no existe o que no está confirmado
                    return View("ForgotPasswordConfirmation");
                }

                // Para obtener más información sobre cómo habilitar la confirmación de cuentas y el restablecimiento de contraseña, visite https://go.microsoft.com/fwlink/?LinkID=320771
                // Enviar correo electrónico con este vínculo
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Restablecer contraseña", "Para restablecer la contraseña, haga clic <a href=\"" + callbackUrl + "\">aquí</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // No revelar que el usuario no existe
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Solicitar redireccionamiento al proveedor de inicio de sesión externo
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generar el token y enviarlo
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Si el usuario ya tiene un inicio de sesión, iniciar sesión del usuario con este proveedor de inicio de sesión externo
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // Si el usuario no tiene ninguna cuenta, solicitar que cree una
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Obtener datos del usuario del proveedor de inicio de sesión externo
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            FormsAuthentication.SignOut();

            //string CSnUserSAP = GlobalSQL.ReadCookie("CSnUserSAP");

            //if(CSnUserSAP == "S")
            //{
            //    Globals.logOutSession();

            //}

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Asistentes
        // Se usa para la protección XSRF al agregar inicios de sesión externos
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        public ActionResult Perfil()
        {
            string userId = Request.Cookies["UserCode"].Value.ToString();
            Usuario consusuario = db.Usuario.Where(x => x.IdUsuario == userId && x.Estado == "A").FirstOrDefault();            
            return View(consusuario);
        }
    }
}