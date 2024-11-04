using FluentAssertions;
using FluentAssertions.Equivalency;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.ObjectComparison;

public class ObjectComparison
{
    private Person actualTsar;

    [SetUp]
    [Description("Получение экземпляра текущего царя")]
    public void SetUp()
    {
        actualTsar = TsarRegistry.GetCurrentTsar();
    }

    [Test]
    [Description("Проверка текущего царя")]
    [Category("ToRefactor")]
    public void CheckCurrentTsar()
    {
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        actualTsar.Should().BeEquivalentTo(expectedTsar,
            ExcludeIdFromPersonClass);
    }

    private EquivalencyAssertionOptions<Person> ExcludeIdFromPersonClass(
        EquivalencyAssertionOptions<Person> congiguration)
        => congiguration
            .Excluding(member =>
                member.DeclaringType == typeof(Person)
                && member.Name.Equals(nameof(Person.Id)));

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        /* Какие недостатки у такого подхода?

        Основным недостатком данного подхода я считаю необходимость
        изменения метода AreEqual при добавлении новых полей класса Person.
        Например, если мы добавим в Person поле Gender(пол),
        то нам нужно будет в AreEqual так же ручками добавить
        проверку совпадения полов:
        actual.Gender == expected.Gender.
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