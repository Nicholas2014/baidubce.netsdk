namespace BceDotNetSdk
{
    public interface IBosRestClient
    {
        T Execute<T>(IBosRequest<T> request) where T : BosResponse, new();
    }
}
