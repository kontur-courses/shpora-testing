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

        // Перепишите код на использование Fluent Assertions.
        // ClassicAssert.AreEqual(actualTsar.Name, expectedTsar.Name);
        // ClassicAssert.AreEqual(actualTsar.Age, expectedTsar.Age);
        // ClassicAssert.AreEqual(actualTsar.Height, expectedTsar.Height);
        // ClassicAssert.AreEqual(actualTsar.Weight, expectedTsar.Weight);
        //
        // ClassicAssert.AreEqual(expectedTsar.Parent!.Name, actualTsar.Parent!.Name);
        // ClassicAssert.AreEqual(expectedTsar.Parent.Age, actualTsar.Parent.Age);
        // ClassicAssert.AreEqual(expectedTsar.Parent.Height, actualTsar.Parent.Height);
        // ClassicAssert.AreEqual(expectedTsar.Parent.Parent, actualTsar.Parent.Parent);

        actualTsar.Should().BeEquivalentTo(expectedTsar, options =>
            options
                .Excluding(p => p.Id)
                .Excluding(p => p.Parent.Id)
        );
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода? 
        ClassicAssert.True(AreEqual(actualTsar, expectedTsar));

        // 1. При падении теста мы просто получаем информацию о том, что объекты не совпали, без указания на конкретные поля
        // 2. Если вдруг класс расширяется, необходимо вручную добавлять проверки каждого нового поля в методе AreEqual
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