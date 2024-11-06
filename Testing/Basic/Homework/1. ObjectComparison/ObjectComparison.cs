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
        
        actualTsar.Should().BeEquivalentTo(expectedTsar, options =>
            options.Excluding(person => person.Id)
                .Excluding(person => person.Parent.Id)
                .AllowingInfiniteRecursion()
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
        // 1) При добавлении нового поля/свойства в класс нужно будет добавлять его проверку в метод AreEqual,
        // если этого не сделать, то тест будет выполнять проверку не верно. В моём решении при добавлении поля/свойства
        // тест менять не нужно (нужно менять только если мы хотим исключить проверку на это поле/свойство)
        // 2) Если поле/свойство будет иметь тип класс, то нужно будет добавлять проверку на этот класс по полям/свойства
        // 3) Если тест сломается, то будет неочевидно на каком именно поле/свойстве расхождение
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
