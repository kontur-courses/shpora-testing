using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.ObjectComparison;

public class ObjectComparison
{
    private readonly Person actualTsar = TsarRegistry.GetCurrentTsar();

    private readonly Person expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
        new Person("Vasili III of Russia", 28, 170, 60, null));

    [Test]
    [Description("Проверка текущего царя, с родителями")]
    public void CheckCurrentTsar_WithParentRecursive()
    {
        actualTsar.Should().BeEquivalentTo(expectedTsar, options =>
            options
                .Excluding(tsar =>
                    tsar.Name == nameof(Person.Id) &&
                    tsar.DeclaringType == typeof(Person))
        );
    }
    
    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        // Какие недостатки у такого подхода? 
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
/*    [Test]
    [Description("Проверка всей родни текущего царя")]
    public void CheckCurrentTsarParent()
    {
        var actualTsarParent = actualTsar.Parent;
        var expectedParentTsar = expectedTsar.Parent;

        while (actualTsarParent is not null && expectedParentTsar is not null)
        {
            actualTsarParent.Should().BeEquivalentTo(expectedParentTsar, options => options
                .Excluding(parent => parent.Id)
                .Excluding(parent => parent.Parent));
            actualTsarParent = actualTsarParent.Parent;
            expectedParentTsar = expectedParentTsar.Parent;
        }
    }*/
// Моё решение лучше предыдущего, т.к. в случае ошибки точно покажет лог,
// FluentAssertions, даёт возможность узнать в подробностях в каком месте
// проблема, прошлое решение такого не давало.
// Также в прошлом решении CheckCurrentTsar_WithCustomEquality()
// приходилось перечислять все поля, а мы просто исключили не нужные.
// Но самый большой плюс нового решения это расширяемость,
// можно добавить новые поля, удалить старые, т.к. проверка по всем полям,
// но с исключением. Также я разбил на два теста, чтобы повысить читаемость.

