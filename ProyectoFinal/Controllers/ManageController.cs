using ProyectoFinal.Models;
using ProyectoFinalModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class ManageController : Controller
    {

        ProyectoFinalEntities db = new ProyectoFinalEntities();
        string error = "";

        // GET: Manage
        public ActionResult Index()
        {
            string userId = Request.Cookies["UserCode"].Value.ToString();
            Usuario consusuario = db.Usuario.Where(x => x.IdUsuario == userId && x.Estado == "A").FirstOrDefault();
            return View(consusuario);
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string userId = Request.Cookies["UserCode"].Value.ToString();
            Usuario consusuario = db.Usuario.AsNoTracking().Where(x => x.IdUsuario == userId && x.Estado == "A").FirstOrDefault();

            if (consusuario != null)
            {
                var result = ProyectoFinalModel.General.ValidaUsuario(userId, model.OldPassword);

                switch (result)
                {
                    case EstadoAcceso.Success:
                        //RecuperaInfoUsuario(CCiUsuario);
                        if (CambiarClave(consusuario, model.NewPassword))
                        {
                            //return RedirectToLocal(returnUrl);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", error);
                            return View(model);
                        }
                        
                    case EstadoAcceso.LockedOut:
                        return View("Lockout");
                    case EstadoAcceso.Failure:
                    default:
                        ModelState.AddModelError("", "Intento de cambio de clave no válido.");
                        return View(model);
                }

            }

            //var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            //if (result.Succeeded)
            //{
            //    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            //    if (user != null)
            //    {
            //        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            //    }
            //    return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            //}
            //AddErrors(result);
            return View(model);
        }

        public bool CambiarClave(Usuario objUsuario, string password)
        {

            ProyectoFinalEntities dbGrabar = new ProyectoFinalEntities();
            bool result = true;

            try
            {
                objUsuario.Password = password;
                dbGrabar.Entry(objUsuario).State = EntityState.Modified;
                dbGrabar.SaveChanges();                
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = false;
            }

            return result;
                       
        }

    }
}