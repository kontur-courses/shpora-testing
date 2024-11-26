﻿using FluentAssertions;
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

        //Вот какой функцией можно заменить выражение Func<IMemberInfo, bool> excludePersonId = tsar => tsar.DeclaringType == typeof(Person) && tsar.Name == nameof(Person.Id);
        actualTsar.Should().BeEquivalentTo(expectedTsar, options =>
                options.Excluding(tsar => tsar.DeclaringType == typeof(Person) && tsar.Name == nameof(Person.Id)));
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода?
        /*
         * 1) Низкая информативность - метод возвращает bool, поэтому при провале теста мы не получим каких-либо подробностей
         * 2) Плохая масштабируемость - метод вызывает пользовательскую функцию AreEqual, которая сравнивает только определенные поля, 
         *    поэтому добавление новых полей также потребует переписывать метод
         * 3) Сложности чтения - нужно изучать пользовательский метод, чтобы понимать какие проверки оно  
         * 
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
