using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace GymSeleniumTests
{
    public class LoginTests
    {
        // 1. Cambiamos IWebDriver por ChromeDriver para quitar la primera advertencia
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
            // ¡OJO! Recuerda cambiar el 7090 por tu puerto real
            driver.Navigate().GoToUrl("https://localhost:7090/");

            Thread.Sleep(2000);

            driver.FindElement(By.Id("txtUsuario")).SendKeys("admin");
            driver.FindElement(By.Id("txtClave")).SendKeys("12345");

            Thread.Sleep(1000);

            driver.FindElement(By.Id("btnLogin")).Click();

            Thread.Sleep(2000);

            Assert.That(driver.Url.Contains("Home"), Is.True);
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