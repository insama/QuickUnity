using QuickUnity.Net.Http;
using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    [IntegrationTest.DynamicTest("UnityHttpClientTests")]
    [IntegrationTest.SucceedWithAssertions]
    [IntegrationTest.Timeout(10)]
    internal class UnityHttpClientSimplifiedGetTest : MonoBehaviour, IUnityHttpResponder
    {
        private UnityHttpClient client;

        // Use this for initialization
        private void Start()
        {
            client = UnityHttpClient.Get("http://www.baidu.com/", this);
        }

        private void OnDisable()
        {
            if (client != null)
            {
                client.Dispose();
                client = null;
            }
        }

        public void OnError(string errorMessage)
        {
            Debug.LogError(errorMessage);
            IntegrationTest.Fail();
        }

        public void OnResult(UnityHttpResponse response)
        {
            Debug.LogFormat("HTTP Status Code: {0}", response.StatusCode);
            Debug.LogFormat("HTTP Response Data: {0}", response.Text);
            IntegrationTest.Pass();
        }
    }
}