using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Advanced.Samples.Interface;

[TestFixture]
[Explicit]
internal class Silenium
{
    private ChromeDriver webDriver;

    [SetUp]
    public void SetUp()
    {
        webDriver = new ChromeDriver();
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
    public void Google()
    {
        webDriver.Url = "https://www.google.com";

        /*
         * В HTML у инпута поиска такая верстка:
         * <textarea ... title="Search" name="q"></textarea>
         */
        var searchControl = webDriver.FindElement(By.Name("q"));

        searchControl.SendKeys("Контур");
        searchControl.SendKeys(Keys.Enter);

        webDriver.Title.Should().Be("Контур - Поиск в Google");
    }

    [Test]
    public void Wikipedia_KonturCreatedDate_ShouldBe1988()
    {
        webDriver.Url = "https://www.wikipedia.org/";

        var searchControl = webDriver.FindElement(By.Name("search"));
        searchControl.SendKeys("СКБ Контур");
        searchControl.SendKeys(Keys.Enter);

        // Как получился такой локатор? Стоит ли использовать такие локаторы для тестирования?
        var locator = By.CssSelector("#mw-content-text > div.mw-content-ltr.mw-parser-output > table.infobox.infobox-3578c39699877354 > tbody > tr:nth-child(5)");
        var createdYearCell = webDriver.FindElement(locator);
        createdYearCell.Text.Should().Contain("Основание 1988");
    }
}