using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

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

        actualTsar.Should().BeEquivalentTo(
            expectedTsar,
            options => options
                .Excluding(member => member.DeclaringType == typeof(Person)
                    && member.Name == nameof(Person.Id)));
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода?
        //
        // Решение с FluentAssertions лучше тем, что оно:
        // 1. позволяет не изменять проверяющую на равенство часть метода при добавлении новых полей и свойств классу Person.
        // Они автоматически будут проверяться.
        // Если проверять их не нужно, их можно исключить из проверки, добавив Excluding.
        // 2. лучше читается.
        // Видно, что объекты сравниваются по всем полям, кроме Id.
        // В альтернативном решении нужно искать, какие поля присутствуют в сравнении, а какие - нет.
        // 3. работает для каждого типа.
        // Альтернативное решение предполагает, что мы должны каждый раз писать новый метод AreEqual.

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
