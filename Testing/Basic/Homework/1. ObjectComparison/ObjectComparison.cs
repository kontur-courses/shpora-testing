using NUnit.Framework;
using NUnit.Framework.Legacy;
using FluentAssertions;

namespace HomeExercise.Tasks.ObjectComparison;
public class ObjectComparison
{
    [Test]
    [Description("Проверка текущего царя")]
    [Category("Refactored")]
    public void CheckCurrentTsar()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();

        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Перепишите код на использование Fluent Assertions.
        actualTsar.Should().BeEquivalentTo(expectedTsar,
            opt => opt.Excluding(p => p.Id)
                      .Excluding(p => p.Parent.Id)); 
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода? 
        //Ответ: не исполняет принцип DRY т.к.
        //повторяет, но при этом является отдельным методом (повторяющим функционал)
        //от общеизвестной (среди других разработчков) библиотеки FluentAssertions,
        //что усложняет читаемость среди других разработчиков
        //При условии actualCsar == actualCsar.Parent метод уйдет в бесконечную рекурсию
        //при изменении модели нужно будет переписывать AreEqual,
        //чтобы он проверял новое свойство, или не проверял то, которое мы убрали из модели.
        //Также, если тест не пройдет, нужно открывать дебагер, чтобы узнать, из-за какого свойства упал тест

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
