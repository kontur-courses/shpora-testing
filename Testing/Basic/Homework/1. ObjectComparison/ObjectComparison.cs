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
            .BeEquivalentTo(expectedTsar, options => options.Using(new PersonComparer()));

        /*
         * Достоинства подхода по сравнению с CheckCurrentTsar_WithCustomEquality
         * 1. В случае падения теста в сообщении прописывается место несовпадения, ожидаемый результат и полученный результат.
         * 2. При добавлении в класс нового поля тест нужно будет изменять только в случае добавления особых проверок, то есть он лучше расширяем.
         * 3. Проверки полей не блокируют друг друга, при несовпадении нескольких полей будет выведено отдельное сообщение для каждого.
         * 4. Тест стал более читаемым
         */
    }
    
    public class PersonComparer : IEquivalencyStep
    {
        public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context, IEquivalencyValidator nestedValidator)
        {
            if (context.CurrentNode.Type == typeof(Person))
            {
                comparands.Subject.Should().BeEquivalentTo(comparands.Expectation, opt => opt.Excluding(x => ExcludeId(x)));

                return EquivalencyResult.AssertionCompleted;
            }

            return EquivalencyResult.ContinueWithNext;
        }
        private bool ExcludeId(IMemberInfo info)
        {
            return info.Name == "Id";
        }
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