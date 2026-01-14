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
        
        actualTsar.IsEqual(expectedTsar);
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
         * 1) Если у класса изменять поля (удалять или добавлять), то также придется изменять
         * логику проверки на соответствие сущностей. В случае с удалением поля программа вообще не скомпилируется
         * 2) Возможна бесконечная рекурсия
         * 3) При падении теста не поймем в чем была причина. Ожидался True, а получили False, вот и вся инфа(
         * 4) Использование FluentAssertion ускоряет понимание логики секции проверки, за счет использования
         * method chaining. В данном тесте вникнуть в логику проверки теста сложнее, чем в первом.
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

internal static class ObjectComparisonAssertExtensions
{
    public static void IsEqual(this Person actualTsar, Person expectedTsar)
    {
        actualTsar.Should().BeEquivalentTo(
            expectedTsar,
            options => options
                .Excluding(x => x.Name == nameof(Person.Id) && x.DeclaringType == typeof(Person))
                .AllowingInfiniteRecursion()
                .IgnoringCyclicReferences());
    }
}