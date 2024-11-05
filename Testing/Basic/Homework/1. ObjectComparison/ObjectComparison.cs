using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.ObjectComparison;
public class ObjectComparison
{
    [Test]
    [Description("Проверка текущего царя")]
    [Category("ToRefactor")]
    public void CheckCurrentTsar()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();

        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        actualTsar.Should().BeEquivalentTo(expectedTsar, properties => properties
            .Excluding(person => person.Id)
            .Excluding(person => person.Parent.Id));
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        /* Данное решение имеет следующие недостатки:
            При добавлении новых свойств в класс Person придётся вручную обновлять метод AreEqual и
            включать новое свойство в сравнение, что сделает код очень громоздким и замедлит разработку.
            В целом, используя собственный метод AreEqual, мы уменьшаем читаемость (особенно если подобных тестов и методов станет много,
            а разбираться в этом будет сторонний разработчик), а также
            отказываемся от более информативной обработки исключений FluentAssertions (можем увидеть, какое конкретное поле не совпало, 
            а не просто получим возврат false) 
            Таким образом моё решение является более устойчивым к изменениям, читабельным и поддерживаемым по сравнению с данным.
        */
        ClassicAssert.True(AreEqual(actualTsar, expectedTsar));
    }

    private bool AreEqual(Person? actual, Person? expected)
    {
        if (actual == expected) return true;
        if (actual == null || expected == null) return false;
        return
            actual.Name == expected.Name
            && actual.Age == expected.Age
            && actual.Height == expected.Height
            && actual.Weight == expected.Weight
            && AreEqual(actual.Parent, expected.Parent);
    }
}
