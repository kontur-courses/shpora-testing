using NUnit.Framework;
using NUnit.Framework.Legacy;
using FluentAssertions;
using NUnit.Framework.Internal;

namespace HomeExercise.Tasks.ObjectComparison;
public class ObjectComparison
{
    private static Person ExpectedTsar() => new Person("Ivan IV The Terrible", 54, 170, 70,
        new Person("Vasili III of Russia", 28, 170, 60, null));
    
    [Test]
    [Description("Проверка имени текущего царя")]
    public void CheckCurrentTsarName()
    {
        var actualTsarName = TsarRegistry.GetCurrentTsar().Name;

        actualTsarName.Should().Be(ExpectedTsar().Name);
    }

    [Test]
    [Description("Проверка возраста текущего царя")]
    public void CheckCurrentTsarAge()
    {
        var actualTsarAge = TsarRegistry.GetCurrentTsar().Age;

        actualTsarAge.Should().Be(ExpectedTsar().Age);
    }

    [Test]
    [Description("Проверка роста текущего царя")]
    public void CheckCurrentTsarHeight()
    {
        var actualTsarHeight = TsarRegistry.GetCurrentTsar().Height;

        actualTsarHeight.Should().Be(ExpectedTsar().Height);
    }

    [Test]
    [Description("Проверка веса текущего царя")]
    public void CheckCurrentTsarWeight()
    {
        var actualTsarWeight = TsarRegistry.GetCurrentTsar().Weight;

        actualTsarWeight.Should().Be(ExpectedTsar().Weight);
    }

    [Test]
    [Description("Проверка имени родителя текущего царя")]

    public void CheckCurrentTsarParentName()
    {
        var actualTsarParentName = TsarRegistry.GetCurrentTsar().Parent.Name;

        actualTsarParentName.Should().Be(ExpectedTsar().Parent.Name);
    }

    [Test]
    [Description("Проверка возраста родителя текущего царя")]

    public void CheckCurrentTsarParentAge()
    {
        var actualTsarParentAge = TsarRegistry.GetCurrentTsar().Parent.Age;

        actualTsarParentAge.Should().Be(ExpectedTsar().Parent.Age);
    }

    [Test]
    [Description("Проверка роста родителя текущего царя")]

    public void CheckCurrentTsarParentHeight()
    {
        var actualTsarParentHeight = TsarRegistry.GetCurrentTsar().Parent.Height;

        actualTsarParentHeight.Should().Be(ExpectedTsar().Parent.Height);
    }

    [Test]
    [Description("Проверка веса родителя текущего царя")]

    public void CheckCurrentTsarParentWeight()
    {
        var actualTsarParentWeight = TsarRegistry.GetCurrentTsar().Parent.Weight;

        actualTsarParentWeight.Should().Be(ExpectedTsar().Parent.Weight);
    }

    [Test]
    [Description("Проверка деда/бабки текущего царя")]

    public void CheckCurrentTsarParentParent()
    {
        var actualTsarParentParent = TsarRegistry.GetCurrentTsar().Parent.Parent;

        actualTsarParentParent.Should().Be(ExpectedTsar().Parent.Parent);
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
