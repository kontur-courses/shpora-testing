using NUnit.Framework;
using NUnit.Framework.Legacy;
using FluentAssertions;

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
            .BeEquivalentTo(expectedTsar, options => options
                .Excluding(ctx => ctx.Path.EndsWith("Id")));

        /*
         * Достоинства подхода по сравнению с CheckCurrentTsar_WithCustomEquality
         * 1. В случае падения теста в сообщении прописывается место несовпадения, ожидаемый результат и полученный результат.
         * 2. При добавлении в класс нового поля тест нужно будет изменять только в случае добавления особых проверок, то есть он лучше расширяем.
         * 3. Проверки полей не блокируют друг друга, при несовпадении нескольких полей будет выведено отдельное сообщение для каждого.
         * 4. Тест стал более читаемым
         */
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
