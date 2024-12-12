namespace Advanced.Classwork.FileSender.Dependecies;

public interface IRecognizer
{
    bool TryRecognize(File file, out Document document);
}
