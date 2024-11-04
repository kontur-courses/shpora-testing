using Basic.Task.WordsStatistics.WordsStatistics;
using FluentAssertions;
using NUnit.Framework;

namespace Basic.Task.WordsStatistics;

// Документация по FluentAssertions с примерами : https://github.com/fluentassertions/fluentassertions/wiki

[TestFixture]
public class WordsStatisticsTests
{
    private IWordsStatistics wordsStatistics;

    [SetUp]
    public void SetUp()
    {
        wordsStatistics = CreateStatistics();
    }

    public virtual IWordsStatistics CreateStatistics()
    {
        // меняется на разные реализации при запуске exe
        return new WordsStatisticsImpl();
    }


    [Test]
    public void GetStatistics_IsEmpty_AfterCreation()
    {
        wordsStatistics.GetStatistics().Should().BeEmpty();
    }

    [Test]
    public void GetStatistics_ContainsItem_AfterAddition()
    {
        wordsStatistics.AddWord("abc");
        wordsStatistics.GetStatistics().Should().Equal(new WordCount("abc", 1));
    }

    [Test]
    public void GetStatistics_ContainsManyItems_AfterAdditionOfDifferentWords()
    {
        wordsStatistics.AddWord("abc");
        wordsStatistics.AddWord("def");
        wordsStatistics.GetStatistics().Should().HaveCount(2);
    }

    [Test]
    public void GetStatistics_ThrowArgumentNullException_AfterAdditionNull()
    {
        wordsStatistics
            .Invoking(word => word.AddWord(null))
            .Should()
            .Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'word')");
    }

    [Test]
    public void GetStatistics_IsEmpty_AfterAdditionWhiteSpaceString()
    {
        wordsStatistics.AddWord("   ");
        wordsStatistics.GetStatistics().Should().BeEmpty();
    }

    [Test]
    public void GetStatistics_LengthWord_AfterAdditionStringLongerThenTen()
    {
        wordsStatistics.AddWord("very long string");
        wordsStatistics.GetStatistics().Should().Equal(new WordCount("very long ", 1));
    }

    [Test]
    public void GetStatistics_Count_AfterAdditionSameWord()
    {
        for (var i = 0; i < 3; i++)
        {
            wordsStatistics.AddWord("word");
        }

        wordsStatistics.GetStatistics().First().Count.Should().Be(3);
    }

    [Test]
    public void GetStatistics_TypeOfWordCount_AfterAdditionWord()
    {
        wordsStatistics.AddWord("word");

        wordsStatistics.GetStatistics().First().GetType().Should().Be(typeof(WordCount));
    }

    [Test]
    public void GetStatistics_IsLowerWord_AfterAdditionWord()
    {
        wordsStatistics.AddWord("WORD");

        wordsStatistics.GetStatistics().First().Word.Should().Be("word");
    }

    [Test]
    public void GetStatistics_OrderByDescending_AfterAdditionDifferentWords()
    {
        wordsStatistics.AddWord("word3");
        
        for (var i = 0; i < 2; i++)
        {
            wordsStatistics.AddWord("word2");
        }

        for (var i = 0; i < 3; i++)
        {
            wordsStatistics.AddWord("word1");
        }

        wordsStatistics
            .GetStatistics()
            .Should()
            .Equal(new List<WordCount>{new ("word1", 3), new ("word2", 2), new ("word3", 1)});
    }
}