using FluentAssertions;
using NUnit.Framework;

namespace Basic.Samples._3._Antipatterns;

[TestFixture]
public class Stack1Tests
{
    [Test]
    [Explicit]
    public void Test1()
    {
        var lines = File.ReadAllLines(@"C:\work\edu\testing-course\Patterns\bin\Debug\data.txt")
            .Select(line => line.Split(' '))
            .Select(line => new { command = line[0], value = line[1] });

        var stack = new Stack<string>();
        foreach (var line in lines)
        {
            if (line.command == "push")
                stack.Push(line.value);
            else
                line.value.Should().BeEquivalentTo(stack.Pop());
        }

        #region Почему это плохо?
        /*
        ## Антипаттерн Local Hero

        Тест не будет работать на машине другого человека или на Build-сервере. 
        Да и у того же самого человека после Clean Solution / переустановки ОС / повторного Clone репозитория / ...

        ## Решение

        Тест не должен зависеть от особенностей локальной среды.
        Если нужна работа с файлами, то либо включите файл в проект и настройте в свойствах его копирование в OutputDir,
        либо поместите его в ресурсы.

        var lines = File.ReadAllLines(@"data.txt")
        var lines = Resources.data.Split(new []{"\r\n"}, StringSplitOptions.RemoveEmptyEntries)
        */
        #endregion
    }

    [Test]
    public void TestPushPop()
    {
        var stack = new Stack<int>();
        stack.Push(10);
        stack.Push(20);
        stack.Push(30);
        while (stack.Any())
            Console.WriteLine(stack.Pop());

        #region Почему это плохо?
        /*
        ## Антипаттерн Loudmouth

        Тест не является автоматическим. Если он сломается, никто этого не заметит.

        ## Мораль

        Вместо вывода на консоль, используйте Assert-ы.
        */
        #endregion
    }


    [Test]
    public void Test2()
    {
        var stack = new Stack<int>();
        stack.Any().Should().BeFalse();
        stack.Push(1);
        stack.Pop();
        stack.Any().Should().BeFalse();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);
        3.Should().Be(stack.Count);
        stack.Pop();
        stack.Pop();
        stack.Pop();
        stack.Any().Should().BeFalse();
        for (var i = 0; i < 1000; i++)
            stack.Push(i);
        for (var i = 1000; i > 0; i--)
            (i - 1).Should().Be(stack.Pop());

        #region Почему это плохо?
        /*
        ## Антипаттерн Freeride

        1. Непонятна область его ответственности. Складывается впечатление, что он тестирует все, однако он это делает плохо.
        Он дает ложное чувство, что все протестировано. Хотя, например, этот тест не проверяет много важных случаев.

        2. Таким тестам как-правило невозможно придумать внятное название.

        3. Если что-то упадет в середине теста, будет сложно разобраться что именно пошло не так и сложно отлаживать — нужно жонглировать точками останова.

        4. Такой тест не работает как документация. По этому сценарию непросто восстановить требования к тестируемому объекту.

        ## Мораль

        Каждый тест должен тестировать одно конкретное требование. Это требование должно отражаться в названии теста.
        Если вы не можете придумать название теста, у вас Free Ride!
        */
        #endregion
    }


    [Test]
    public void TestPop()
    {
        var stack = new Stack<int>(new[] { 1, 2, 3, 4, 5 });
        var result = stack.Pop();
        5.Should().Be(result);
        stack.Any().Should().BeTrue();

        4.Should().Be(stack.Count);
        4.Should().Be(stack.Peek());
        new[] { 4, 3, 2, 1 }.Should().BeEquivalentTo(stack.ToArray());

        #region Почему это плохо?
        /*	
        ## Антипаттерн Overspecification

        1. Непонятна область ответственности. Сложно придумать название. Не так плохо, как FreeRide, но плохо.

        2. Изменение API роняет сразу много подобных тестов, создавая много рутинной работы по их починке.

        3. Если все тесты будут такими, то при появлении бага, падают они большой компанией.


        ## Мораль

        Сфокусируйтесь на проверке одного конкретного требования в каждом тесте.
        Не старайтесь проверить "за одно" какое-то требование сразу во всех тестах — это может выйти боком.

        Признак возможной проблемы — более одного Assert на метод.
        */
        #endregion
    }
}
