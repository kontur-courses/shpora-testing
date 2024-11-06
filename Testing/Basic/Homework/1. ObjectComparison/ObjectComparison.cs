using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using FluentAssertions;
using FluentAssertions.Equivalency;
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

        //В отличие от решения в методе CheckCurrentTsar_WithCustomEquality
        //Обладает следующими преимуществами:
        // 
        //1. Лучшая расширширяемость. Необходимость изменения теста
        //при изменении класса Person меньше
        //
        //2. Лучшая читаемость, ясно что делает и проверяет тест
        //
        //3. Сообщение об ошибке информативней
        //

        actualTsar.Should().BeEquivalentTo(expectedTsar, options =>
            options.Excluding(p => p.Path.EndsWith("Id")));
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        //Какие недостатки у такого подхода?
        //
        //1. При добавлении добавлении или изменении полей в
        //классе Person необходимо будет также изменять метод
        //AreEqual для сравнения объектов
        //
        //2. Сообщение об ошибке, не содержит в себе понятной
        //информации по какой причине тесте не пройден.
        //В отличии от аналогичного использования FluentAssertions

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