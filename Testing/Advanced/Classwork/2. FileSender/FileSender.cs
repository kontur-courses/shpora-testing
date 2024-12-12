using Advanced.Classwork.FileSender.Dependecies;
using System.Security.Cryptography.X509Certificates;
using File = Advanced.Classwork.FileSender.Dependecies.File;

namespace Advanced.Classwork.FileSender;

public class FileSender
{
    private readonly ICryptographer cryptographer;
    private readonly ISender sender;
    private readonly IRecognizer recognizer;

    public FileSender(
        ICryptographer cryptographer,
        ISender sender,
        IRecognizer recognizer)
    {
        this.cryptographer = cryptographer;
        this.sender = sender;
        this.recognizer = recognizer;
    }

    public Result SendFiles(File[] files, X509Certificate certificate)
    {
        return new Result
        {
            SkippedFiles = files
                .Where(file => !TrySendFile(file, certificate))
                .ToArray()
        };
    }

    private bool TrySendFile(File file, X509Certificate certificate)
    {
        Document document;
        if (!recognizer.TryRecognize(file, out document))
            return false;
        if (!CheckFormat(document) || !CheckActual(document))
            return false;
        var signedContent = cryptographer.Sign(document.Content, certificate);
        return sender.TrySend(signedContent);
    }

    private bool CheckFormat(Document document)
    {
        return document.Format == "4.0" || document.Format == "3.1";
    }

    private bool CheckActual(Document document)
    {
        return document.Created.AddMonths(1) > DateTime.Now;
    }

    public class Result
    {
        public File[] SkippedFiles { get; set; }
    }
}