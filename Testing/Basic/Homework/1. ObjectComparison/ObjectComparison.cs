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
        
        // Исправленный вариант
        // FluentAssertions позволяет удобно сравнивать обьекты, регулируя само сравнение (в данном случае исключаем для сравнения уникальные поля Id), благодаря чему
        // код становится кратким и лаконичным, а также понятным.
        actualTsar.Should().BeEquivalentTo(expectedTsar, options => options
            .Excluding(person => person.Id)
            .Excluding(person => person.Parent.Id));
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода? 
        // У данного подхода существенный недостаток - это хардкод при сравнении полей обьекта. При изменении/добавлении/удалении полей, данный метод придется
        // дописывать/переписывать. Оригинальное решение лучше во многом: проще читается, меньше мест где можно допустить ошибку, а также не потребует
        // переписывания при изменении сравниваемых обьектов.
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
