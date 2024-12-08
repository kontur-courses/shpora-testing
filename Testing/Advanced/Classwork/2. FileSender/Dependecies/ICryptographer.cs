using System.Security.Cryptography.X509Certificates;

namespace Advanced.Classwork.FileSender.Dependecies;

public interface ICryptographer
{
    byte[] Sign(byte[] content, X509Certificate certificate);
}