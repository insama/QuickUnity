using CSharpExtensions;
using CSharpExtensions.Net.Http;
using QuickUnity.Net.Http;
using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    [IntegrationTest.DynamicTest("UnityHttpClientTests")]
    [IntegrationTest.SucceedWithAssertions]
    [IntegrationTest.Timeout(10)]
    internal class UnityHttpClientGetTest : MonoBehaviour
    {
        private UnityHttpClient httpClient;

        private void Start()
        {
            httpClient = new UnityHttpClient();
            httpClient.DownloadInProgress.AddListener((UnityHttpClient client, DownloadInProgressEventArgs e) =>
            {
                float progress = (float)e.BytesRead / e.TotalLength;
                Debug.Log(progress);
            });

            httpClient.DownloadCompleted.AddListener((UnityHttpClient client, DownloadCompletedEventArgs e) =>
            {
                UnityHttpResponse resp = (UnityHttpResponse)e.Response;
                Debug.Log(resp.Text);
                IntegrationTest.Pass();
            });

            httpClient.ErrorReceived.AddListener((UnityHttpClient client, HttpErrorReceivedEventArgs e) =>
            {
                Debug.LogError(e.ErrorMessage);
            });

            httpClient.ExceptionCaught.AddListener((UnityHttpClient client, ExceptionCaughtEventArgs e) =>
            {
                Debug.LogException(e.Exception);
            });

            UnityHttpRequest req = new UnityHttpRequest("http://www.baidu.com/");
            httpClient.SendRequest(req);
        }

        private void OnDisable()
        {
            if (httpClient != null)
            {
                httpClient.Dispose();
                httpClient = null;
            }
        }
    }
}