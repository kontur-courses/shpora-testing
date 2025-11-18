using NUnit.Framework;
using NUnit.Framework.Legacy;
using FluentAssertions;
using FluentAssertions.Equivalency;

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

        // Перепишите код на использование Fluent Assertions.
        actualTsar.Should()
            .BeEquivalentTo(expectedTsar, o => o
                .Excluding((IMemberInfo t) => t.Name == nameof(Person.Id)));
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода? 
        /*
         * 1. В тестовом методе выше есть возможность отследить на каком из этапов сравнения двух экземпляров класса
         * упал тест. Здесь же, используя кастомный метод сравнения AreEqual мы можем проверить только факт полного
         * совпадения всех полей, и не сможем отследить какое именно поле отличается.
         * 2. При добавлении новых полей в класс Person необходимо каждый раз переписывать метод AreEqual
         * Ну и читаемость намного возрастает в сравнении с решением представленным выше, там просто
         * нужно знать что делает BeEquivalentTo и понимать что его поведение не изменится, а здесь нужно разбираться
         * в методе AreEqual который может время от времени меняться в зависимости от полей класса Person
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
