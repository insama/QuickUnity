using CSharpExtensions.Net.Http;
using QuickUnity.Net.Http;
using System.IO;
using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    [IntegrationTest.DynamicTest("MonoHttpClientTest")]
    [IntegrationTest.SucceedWithAssertions]
    [IntegrationTest.Timeout(10)]
    public class MonoHttpClientGetTest : MonoBehaviour
    {
        private MonoHttpClient client;

        private FileStream fs;

        private void Awake()
        {
            fs = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "test.mp4"), FileMode.Create);
        }

        // Use this for initialization
        private void Start()
        {
            client = new MonoHttpClient(false);
            client.AddEventListener(HttpEvent.HttpStatusCodeReceived, (Events.Event e) =>
            {
                HttpEvent httpEvent = (HttpEvent)e;
                Debug.LogFormat("HTTP Status Code: {0}", httpEvent.HttpStatusCode);
            });

            client.AddEventListener(HttpEvent.HttpDownloadInProgress, (Events.Event e) =>
            {
                HttpEvent httpEvent = (HttpEvent)e;
                float progress = (float)httpEvent.BytesRead / httpEvent.TotalLength;
                Debug.LogFormat("HTTP Download Progress: {0}", progress);
            });

            client.AddEventListener(HttpEvent.HttpDownloadCompleted, (Events.Event e) =>
            {
                HttpEvent httpEvent = (HttpEvent)e;
                byte[] responseData = httpEvent.Response.ResponseData;
                fs.Write(responseData, 0, responseData.Length);
                OnDisable();
                Debug.LogWarning("File Download Completed");
            });

            client.AddEventListener(HttpEvent.HttpExceptionCaught, (Events.Event e) =>
            {
                HttpEvent httpEvent = (HttpEvent)e;
                Debug.LogException(httpEvent.ExceptionCaught);
            });

            HttpRequest req = new HttpRequest("http://192.168.0.3/010-000053.mp4-muxed.mp4");
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

        private void OnDisable()
        {
            if (fs != null)
            {
                fs.Close();
                fs = null;
            }
        }
    }
}