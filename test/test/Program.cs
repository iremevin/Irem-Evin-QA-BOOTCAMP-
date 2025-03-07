using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;

namespace TestAutomation
{
    public class Program
    {
        public static void Main()
        {
            ExtentHtmlReporter htmlReporter = new ExtentHtmlReporter("extent_report.html");
            ExtentReports extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);

            ITest test = extent.CreateTest("Temu Site Testi");

            ChromeDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.temu.com");

            try
            {
                IWebElement searchBox = driver.FindElement(By.Name("q"));
                searchBox.SendKeys("ayakkabı");
                searchBox.SendKeys(Keys.Enter);

                test.Log(Status.Info, "Sayfa Başlığı: " + driver.Title);
                System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> results = driver.FindElements(By.CssSelector(".product-item"));
                if (results.Count > 0)
                {
                    test.Log(Status.Pass, "Arama başarılı! Ürünler listelendi.");
                }
                else
                {
                    test.Log(Status.Fail, "Arama başarısız, ürünler listelenmedi.");
                }

                driver.Navigate().GoToUrl("https://www.temu.com/product/ayakkabi");
                IWebElement addToCartButton = driver.FindElement(By.CssSelector(".add-to-cart-button"));
                addToCartButton.Click();

                IWebElement cartCountElement = driver.FindElement(By.CssSelector(".cart-count"));
                string cartCount = cartCountElement.Text;
                test.Log(Status.Info, "Sepetteki ürün sayısı: " + cartCount);

                driver.Navigate().GoToUrl("https://www.temu.com/login");
                IWebElement emailField = driver.FindElement(By.Name("email"));
                emailField.SendKeys("ornek@mail.com");
                IWebElement passwordField = driver.FindElement(By.Name("password"));
                passwordField.SendKeys("12345");

                IWebElement loginButton = driver.FindElement(By.CssSelector(".login-button"));
                loginButton.Click();

                Thread.Sleep(2000);
                if (driver.PageSource.Contains("Hoş geldiniz"))
                {
                    test.Log(Status.Pass, "Giriş başarılı!");
                }
                else
                {
                    test.Log(Status.Fail, "Giriş başarısız.");
                }
            }
            catch (Exception e)
            {
                test.Log(Status.Fail, "Test sırasında bir hata oluştu: " + e.Message);
            }
            finally
            {
                extent.Flush();
                driver.Quit();
            }
        }
    }
}