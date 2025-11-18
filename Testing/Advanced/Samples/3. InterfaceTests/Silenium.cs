using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Advanced.Samples.Interface;

[TestFixture]
[Explicit]
internal class Silenium
{
    private ChromeDriver webDriver;

    [SetUp]
    public void SetUp()
    {
        var service = ChromeDriverService.CreateDefaultService();
        service.LogPath = "chromedriver.logs";
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");// Запуск в полноэкранном режиме
        options.AddArgument("--no-sandbox");
        webDriver = new ChromeDriver(service, options);
    }

    [TearDown]
    public void TearDown()
    {
        webDriver.Dispose();
    }

    /*
     * Для поиска местоположения веб-элемента из DOM используются локатор. 
     * Дальнейшее взаимодейтействием выполняется относительно найденого элемента.
     * Несколько популярных локаторов в Selenium - ID, Name, Link Text, Partial Link Text, CSS Selectors, XPath, TagName и т.д.
    */
    
    [Test]
    public void Yandex()
    {
        webDriver.Navigate().GoToUrl("https://ya.ru/");

        /*
         * В HTML у инпута поиска такая верстка:
         * <textarea ... title="Search" name="q"></textarea>
         */
        var searchControl = webDriver.FindElement(By.Name("text"));

        searchControl.SendKeys("Контур");
        searchControl.SendKeys(Keys.Enter);

        var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
        wait.Until(x => x.Title.Contains("Контур"));
    }

    [Test]
    public void Wikipedia_KonturCreatedDate_ShouldBe1988()
    {
        var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
        webDriver.Navigate().GoToUrl("https://www.wikipedia.org/");

        wait.Until(x => x.FindElement(By.Name("search")).Displayed);
        var searchControl = webDriver.FindElement(By.Name("search"));
        searchControl.SendKeys("СКБ Контур");
        searchControl.SendKeys(Keys.Enter);

        // Как получился такой локатор? Стоит ли использовать такие локаторы для тестирования?
        var locator = By.CssSelector("#mw-content-text > div.mw-content-ltr.mw-parser-output > table.infobox.infobox-3578c39699877354 > tbody > tr:nth-child(5)");
        
        wait.Until(x => x.FindElement(locator).Text.Contains("Основание 1988"));
    }
}
