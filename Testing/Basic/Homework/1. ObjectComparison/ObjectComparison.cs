using NUnit.Framework;
using NUnit.Framework.Legacy;
using FluentAssertions;

namespace HomeExercise.Tasks.ObjectComparison;

public class ObjectComparison
{
    [Test]
    [Description("Проверка текущего царя")]
    public void CheckCurrentTsar()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();

        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        actualTsar
            .Should()
            .BeEquivalentTo(expectedTsar, options =>
                options
                    .AllowingInfiniteRecursion()
                    .Excluding(o => o.Id)
                    .Excluding(o => o.Parent.Id));
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода? 
        // При изменении класса Person придётся изменять метод AreEqual,
        // т.е. если мы добавим поле в Person, то придётся добавить проверку на него в этом методе.
        // При провале теста мы не получаем конкретной информации из-за чего тест не прошёл,
        // т.к. метод AreEqual возвращает bool и скажет нам только Success или Failed.
        // Невозможно сравнить двух Person без определённых полей,
        // придётся создавать различные реализации метода AreEqual.
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