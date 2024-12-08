namespace Advanced.Samples.Mocks;

public interface IService
{
    string Get();
    bool TryGet(string id, out string value);
}
