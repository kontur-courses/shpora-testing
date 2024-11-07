using System;
using System.Reflection;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Homework._1._ObjectComparison;
using NUnit.Framework;
using NUnit.Framework.Internal;
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
        actualTsar.Should()
            .BeEquivalentTo(expectedTsar, options => options.Excluding(ctx => ComparePerson(ctx)));



        //.Using<Person>(ctx => ctx.Subject
        //   .Should().BeEquivalentTo(ctx.Expectation, opt => opt.Excluding(x => x.Id))).WhenTypeIs<Person>());s

        /*
         * Достоинства подхода по сравнению с CheckCurrentTsar_WithCustomEquality
         * 1. В случае падения теста в сообщении прописывается место несовпадения, ожидаемый результат и полученный результат.
         * 2. При добавлении в класс нового поля тест нужно будет изменять только в случае добавления особых проверок, то есть он лучше расширяем.
         * 3. Проверки полей не блокируют друг друга, при несовпадении нескольких полей будет выведено отдельное сообщение для каждого.
         * 4. Тест стал более читаемым
         */
    }

    private bool ComparePerson(IMemberInfo ctx)
    {
        var parentInfo = ctx.GetType()
            ?.GetField("member", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.GetValue(ctx) as Field;

        return parentInfo?.ParentType == typeof(Person) && ctx.Path.EndsWith("Id");
    }


    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода?
        // 1. Не очевидно, где произошло несовпадение в случае падения теста.
        // 2. При добавлении в класс нового поля придётся изменять метод AreEqual
        // 3. Проверки полей блокируют друг друга. Например, при несовпадении имён другие поля проверяться уже не будут.
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