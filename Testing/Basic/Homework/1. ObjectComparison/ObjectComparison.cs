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
        var expectedParentTsar = TestPersonBuilder.Create()
            .WithName("Vasili III of Russia")
            .WithAge(28)
            .WithHeight(170)
            .WithWeight(60)
            .Build();

        var expectedTsar = TestPersonBuilder.Create()
            .WithName("Ivan IV The Terrible")
            .WithAge(54)
            .WithHeight(170)
            .WithWeight(70)
            .WithParent(expectedParentTsar)
            .Build();

        actualTsar.Should().BeEquivalentTo(expectedTsar, options => options
            .Excluding(ctx => ctx.Path.EndsWith("Id")));
    }

    // 1. Код менее читаемый и менее декларативный.
    // 2. Требуется понимание того, что происходит в AreEqual.
    // 3. Потребуется вручную обновлять логику сравнения в AreEqual, что может привести к ошибкам.
    // 4. Не информативное сообщение при упавшем тесте.
    // 5. При добавлении новых полей в Person потребуется обновлять все места создания Person.
    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

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