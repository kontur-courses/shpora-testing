using NUnit.Framework;
using NUnit.Framework.Legacy;
using FluentAssertions;
using NUnit.Framework.Internal;

namespace HomeExercise.Tasks.ObjectComparison;
public class ObjectComparisonTest
{
    private static Person ExpectedTsar() => new Person("Ivan IV The Terrible", 54, 170, 70,
        new Person("Vasili III of Russia", 28, 170, 60, null));

    [Test]
    [Description("Проверка текущего царя")]

    public void CheckCurrentTsar()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();

        actualTsar.Should().BeEquivalentTo(ExpectedTsar(), options => options
            .Excluding(memberInfo =>
                memberInfo.Name == nameof(Person.Id)
                && memberInfo.DeclaringType == typeof(Person)));

    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода? 
        // Если бы за полями класса стояла бы какая-то нетривиальная логика, то хотелось, чтобы тесты
        //отражали проверку каждого по отдельности. Мне кажется, что альтернативное решение не обеспечивает
        //необходимую наглядность и скорее всего оказалось бы сложным для отладки
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
