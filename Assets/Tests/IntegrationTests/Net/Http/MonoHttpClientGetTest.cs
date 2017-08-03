using CSharpExtensions.Net.Http;
using QuickUnity.Net.Http;
using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    [IntegrationTest.DynamicTest("MonoHttpClientTests")]
    [IntegrationTest.SucceedWithAssertions]
    [IntegrationTest.Timeout(10)]
    internal class MonoHttpClientGetTest : MonoBehaviour
    {
        private MonoHttpClient client;

        // Use this for initialization
        private void Start()
        {
            client = new MonoHttpClient();

            client.AddEventListener(MonoHttpEvent.DownloadInProgress, (Events.Event e) =>
            {
                MonoHttpEvent httpEvent = (MonoHttpEvent)e;
                float progress = (float)httpEvent.BytesRead / httpEvent.TotalLength;
                Debug.LogFormat("HTTP Download Progress: {0}", progress);
            });

            client.AddEventListener(MonoHttpEvent.DownloadCompleted, (Events.Event e) =>
            {
                MonoHttpEvent httpEvent = (MonoHttpEvent)e;
                string text = httpEvent.Response.Text;
                Debug.LogWarningFormat("File Download Completed: {0}", text);
                IntegrationTest.Pass();
            });

            client.AddEventListener(MonoHttpEvent.ExceptionCaught, (Events.Event e) =>
            {
                MonoHttpEvent httpEvent = (MonoHttpEvent)e;
                Debug.LogException(httpEvent.Exception);
            });

            HttpRequest req = new HttpRequest("http://www.baidu.com/");
            client.SendRequest(req);
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (client != null)
            {
                client.Update();
            }
        }
    }
}