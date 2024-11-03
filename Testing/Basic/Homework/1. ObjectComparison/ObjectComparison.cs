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
            options.Excluding(x => x.Name == nameof(Person.Id) && x.DeclaringType == typeof(Person)));
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
         * 1. Метод AreEqual проверяет только определенные свойства класса, поэтому если
         * в будущем придется добавить новые или удалить текущие, то их также придется
         * добавить/удалить в этом методе.
         * 2. Написание собственного метода для проверки равенства теоретически усложнит
         * понимание теста, так как нужно разобраться в работе написанного метода. При
         * этом можно было спокойно воспользоваться стандартными методами.
         * 3. Так как метод возвращает bool, то и сравнивать результат этого метода
         * приходится с True/False, поэтому при провале теста мы не получаем
         * конкретной информации в чем была проблема. В отличие от первого теста с
         * применением AreEqual для каждого поля в отдельности. (если используем
         * FluentAssertions, то вообще сказка)
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
