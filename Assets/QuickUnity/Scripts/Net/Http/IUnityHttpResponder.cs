namespace QuickUnity.Net.Http
{
    public interface IUnityHttpResponder
    {
        void OnResult(UnityHttpResponse response);

        void OnError(string errorMessage);
    }
}