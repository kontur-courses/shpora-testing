using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Constraints;
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
        
        expectedTsar.Should().BeEquivalentTo(actualTsar, options => options
            .Excluding(p => p.Id)
            .Excluding(p => p.Parent));
        
        expectedTsar.Parent.Should().BeEquivalentTo(actualTsar.Parent, options => options
            .Excluding(p => p.Id)
            .Excluding(p => p.Weight));
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода?
        //При добавлении свойств в класс Person этот тест перестанет корректно работать, его придется дописывать,
        //а тест FluentAssertions придется дописывать, только если добавленное поле не должно учавствовать в сравнении
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
