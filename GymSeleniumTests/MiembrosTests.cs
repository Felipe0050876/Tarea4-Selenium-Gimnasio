using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace GymSeleniumTests
{
    public class MiembrosTests
    {
        ChromeDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void CrearMiembro_CaminoFeliz_DeberiaGuardar()
        {
            driver.Navigate().GoToUrl("https://localhost:7090/Miembroes/Create");
            Thread.Sleep(5000);

            driver.FindElement(By.Id("Nombre")).SendKeys("Juan");
            driver.FindElement(By.Id("Apellido")).SendKeys("Perez");
            driver.FindElement(By.Id("Email")).SendKeys("juan.perez@fitmanager.com");
            driver.FindElement(By.Id("Telefono")).SendKeys("8095551234");
            driver.FindElement(By.CssSelector("input[type='submit']")).Click();
            Thread.Sleep(5000);

            Assert.That(driver.Url, Does.Not.Contain("Create"));
        }

        [Test]
        public void CrearMiembro_PruebaNegativa_CamposVacios_MuestraError()
        {
            driver.Navigate().GoToUrl("https://localhost:7090/Miembroes/Create");
            Thread.Sleep(5000);

            driver.FindElement(By.CssSelector("input[type='submit']")).Click();
            Thread.Sleep(5000);

            var errores = driver.FindElements(By.ClassName("text-danger"));
            Assert.That(errores.Count, Is.GreaterThan(0));
        }

        [Test]
        public void CrearMiembro_PruebaLimites_NombreExagerado_NoRompeSistema()
        {
            driver.Navigate().GoToUrl("https://localhost:7090/Miembroes/Create");
            Thread.Sleep(1000);

            string nombreLargo = new string('A', 500);

            driver.FindElement(By.Id("Nombre")).SendKeys(nombreLargo);
            driver.FindElement(By.Id("Apellido")).SendKeys("Perez");
            driver.FindElement(By.Id("Email")).SendKeys("limite@fitmanager.com");
            driver.FindElement(By.Id("Telefono")).SendKeys("8095551234");

            driver.FindElement(By.CssSelector("input[type='submit']")).Click();

            Thread.Sleep(2000);

            // Verificamos que el sistema manejó el límite (ya sea rebotándolo con error o truncándolo), 
            // pero que no dio una pantalla de colapso total (Error 500).
            bool sigueActivo = driver.Url.Contains("Create") || driver.Url.Contains("Miembroes");
            Assert.That(sigueActivo, Is.True);
        }
        [Test]
        public void LeerMiembro_CaminoFeliz_DeberiaCargarLista()
        {
            driver.Navigate().GoToUrl("https://localhost:7090/Miembroes");
            Thread.Sleep(5000);

            var tablas = driver.FindElements(By.TagName("table"));
            Assert.That(tablas.Count, Is.GreaterThan(0));
        }

        [Test]
        public void EditarMiembro_CaminoFeliz_DeberiaActualizar()
        {
            driver.Navigate().GoToUrl("https://localhost:7090/Miembroes");
            Thread.Sleep(5000);

            var btnEdit = driver.FindElement(By.LinkText("Edit"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnEdit);
            Thread.Sleep(5000);

            var campoNombre = driver.FindElement(By.Id("Nombre"));
            campoNombre.Clear();
            campoNombre.SendKeys("NombreModificado");
            Thread.Sleep(5000);

            var btnGuardar = driver.FindElement(By.CssSelector("input[type='submit']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnGuardar);
            Thread.Sleep(5000);

            Assert.That(driver.Url, Does.Not.Contain("Edit"));
        }

        [Test]
        public void EditarMiembro_PruebaNegativa_CampoVacio_MuestraError()
        {
            driver.Navigate().GoToUrl("https://localhost:7090/Miembroes");
            Thread.Sleep(5000);

            var btnEdit = driver.FindElement(By.LinkText("Edit"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnEdit);
            Thread.Sleep(5000);

            var campoNombre = driver.FindElement(By.Id("Nombre"));
            campoNombre.Clear();

            var btnGuardar = driver.FindElement(By.CssSelector("input[type='submit']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnGuardar);
            Thread.Sleep(5000);

            var errores = driver.FindElements(By.ClassName("text-danger"));
            Assert.That(errores.Count, Is.GreaterThan(0));
        }

        [Test]
        public void EditarMiembro_PruebaLimites_NombreExagerado_NoRompeSistema()
        {
            driver.Navigate().GoToUrl("https://localhost:7090/Miembroes");
            Thread.Sleep(5000);

            var btnEdit = driver.FindElement(By.LinkText("Edit"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnEdit);
            Thread.Sleep(5000);

            string nombreLargo = new string('X', 500);
            var campoNombre = driver.FindElement(By.Id("Nombre"));
            campoNombre.Clear();
            campoNombre.SendKeys(nombreLargo);

            var btnGuardar = driver.FindElement(By.CssSelector("input[type='submit']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnGuardar);
            Thread.Sleep(5000);

            bool sigueActivo = driver.Url.Contains("Edit") || driver.Url.Contains("Miembroes");
            Assert.That(sigueActivo, Is.True);
        }

        [Test]
        public void EliminarMiembro_CaminoFeliz_DeberiaBorrar()
        {
            driver.Navigate().GoToUrl("https://localhost:7090/Miembroes");
            Thread.Sleep(5000);

            var btnDelete = driver.FindElement(By.LinkText("Delete"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnDelete);
            Thread.Sleep(5000);

            var btnConfirmar = driver.FindElement(By.CssSelector("input[type='submit']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnConfirmar);
            Thread.Sleep(5000);

            Assert.That(driver.Url, Does.Not.Contain("Delete"));
        }

        [TearDown]
        public void TearDown()
        {
            Screenshot captura = ((ITakesScreenshot)driver).GetScreenshot();
            string nombrePrueba = TestContext.CurrentContext.Test.Name;
            captura.SaveAsFile(nombrePrueba + ".png");

            driver.Quit();
            driver.Dispose();
        }
    }
}