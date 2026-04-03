using Microsoft.AspNetCore.Mvc;

namespace RegistroMiembros.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Ingresar(string usuario, string clave)
        {
            // Camino feliz fijo para la prueba de Selenium
            if (usuario == "admin" && clave == "12345")
            {
                return RedirectToAction("Index", "Home");
            }

            // Camino negativo (error)
            ViewBag.Error = "Usuario o contraseña incorrectos";
            return View("Index");
        }
    }
}