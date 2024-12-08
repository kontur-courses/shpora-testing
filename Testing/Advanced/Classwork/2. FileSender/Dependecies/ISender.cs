namespace Advanced.Classwork.FileSender.Dependecies;

public interface ISender
{
    bool TrySend(byte[] content);
}