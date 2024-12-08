using Advanced.Classwork.FileSender.Dependecies;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using System.Security.Cryptography.X509Certificates;
using Document = Advanced.Classwork.FileSender.Dependecies.Document;
using File = Advanced.Classwork.FileSender.Dependecies.File;

namespace Advanced.Classwork.FileSender;

//TODO: реализовать недостающие тесты
[TestFixture]
public class FileSender_Should
{
    private FileSender fileSender;
    private ICryptographer cryptographer;
    private ISender sender;
    private IRecognizer recognizer;

    private readonly X509Certificate certificate = new X509Certificate();
    private File file;
    private byte[] signedContent;

    [SetUp]
    public void SetUp()
    {
        // Постарайтесь вынести в SetUp всё неспецифическое конфигурирование так,
        // чтобы в конкретных тестах осталась только специфика теста,
        // без конфигурирования "обычного" сценария работы

        file = new File("someFile", new byte[] { 1, 2, 3 });
        signedContent = new byte[] { 1, 7 };

        cryptographer = A.Fake<ICryptographer>();
        sender = A.Fake<ISender>();
        recognizer = A.Fake<IRecognizer>();
        fileSender = new FileSender(cryptographer, sender, recognizer);
    }

    [TestCase("4.0")]
    [TestCase("3.1")]
    public void Send_WhenGoodFormat(string format)
    {
        var document = new Document(file.Name, file.Content, DateTime.Now, format);
        A.CallTo(() => recognizer.TryRecognize(file, out document))
            .Returns(true);
        A.CallTo(() => cryptographer.Sign(document.Content, certificate))
            .Returns(signedContent);
        A.CallTo(() => sender.TrySend(signedContent))
            .Returns(true);

        fileSender.SendFiles(new[] { file }, certificate)
            .SkippedFiles
            .Should().BeEmpty();
    }

    [Test]
    [Ignore("Not implemented")]
    public void Skip_WhenBadFormat()
    {
        throw new NotImplementedException();
    }

    [Test]
    [Ignore("Not implemented")]
    public void Skip_WhenOlderThanAMonth()
    {
        throw new NotImplementedException();
    }

    [Test]
    [Ignore("Not implemented")]
    public void Send_WhenYoungerThanAMonth()
    {
        throw new NotImplementedException();
    }

    [Test]
    [Ignore("Not implemented")]
    public void Skip_WhenSendFails()
    {
        throw new NotImplementedException();
    }

    [Test]
    [Ignore("Not implemented")]
    public void Skip_WhenNotRecognized()
    {
        throw new NotImplementedException();
    }

    [Test]
    [Ignore("Not implemented")]
    public void IndependentlySend_WhenSeveralFilesAndSomeAreInvalid()
    {
        throw new NotImplementedException();
    }

    [Test]
    [Ignore("Not implemented")]
    public void IndependentlySend_WhenSeveralFilesAndSomeCouldNotSend()
    {
        throw new NotImplementedException();
    }
}