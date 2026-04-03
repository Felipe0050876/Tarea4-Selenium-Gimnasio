using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace GymSeleniumTests
{
    public class LoginTests
    {
        ChromeDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void Login_CaminoFeliz_DeberiaIngresar()
        {
            driver.Navigate().GoToUrl("https://localhost:7090/");

            Thread.Sleep(5000);

            driver.FindElement(By.Id("txtUsuario")).SendKeys("admin");
            driver.FindElement(By.Id("txtClave")).SendKeys("12345");

            Thread.Sleep(5000);

            driver.FindElement(By.Id("btnLogin")).Click();

            Thread.Sleep(5000);

            Assert.That(driver.Url.Contains("Home"), Is.True);
        }

        [Test]
        public void Login_Negativo_CredencialesInvalidas()
        {
            driver.Navigate().GoToUrl("https://localhost:7090/");
            Thread.Sleep(5000);

            // Escribir un usuario correcto pero una contraseña INCORRECTA
            driver.FindElement(By.Id("txtUsuario")).SendKeys("admin");
            driver.FindElement(By.Id("txtClave")).SendKeys("claveEquivocada123");

            Thread.Sleep(5000);

            driver.FindElement(By.Id("btnLogin")).Click();
            Thread.Sleep(5000);

            // Validar que el sistema NO nos dejó entrar (la URL no debe contener "Home")
            Assert.That(driver.Url.Contains("Home"), Is.False, "El sistema permitió el acceso con contraseña incorrecta.");
        }

        [Test]
        public void Login_Limite_CamposVacios()
        {
            // Navegar al Login
            driver.Navigate().GoToUrl("https://localhost:7090/");
            Thread.Sleep(5000);

            // Enviar los campos totalmente VACÍOS (límite inferior de texto = 0)
            driver.FindElement(By.Id("txtUsuario")).SendKeys("");
            driver.FindElement(By.Id("txtClave")).SendKeys("");

            Thread.Sleep(4000);

            driver.FindElement(By.Id("btnLogin")).Click();
            Thread.Sleep(5000);

            // Validar que el sistema nos rebotó y seguimos sin entrar al Home
            Assert.That(driver.Url.Contains("Home"), Is.False, "El sistema permitió el acceso con los campos vacíos.");
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
