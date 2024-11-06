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

        actualTsar.Should().BeEquivalentTo(expectedTsar,
            options => options
                .IgnoringCyclicReferences()
                .AllowingInfiniteRecursion()
                .Excluding(memberInfo => memberInfo.Name == nameof(Person.Id) && memberInfo.DeclaringType == typeof(Person)));
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        /* Какие недостатки у такого подхода?
             1) В метод можно передать только параметры класса Person, для других классов нужен будет новый метод.
             2) Плохая читаемость и выразительность в сравнении с использованием FluentAssertions
             3) При расширении класса Person необходимо вносить изменения в метод AreEqual
             4) Метод AreEqual возвращает bool, из-за чего не понятно, в чем именно объекты не совпадают
             5) Зачем реализовывать то, что уже есть в FluentAssertions?
             Такой метод тяжело поддерживать.
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
