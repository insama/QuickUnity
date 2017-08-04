using QuickUnity.Net.Http;
using UnityEngine;
using UnityEngine.UI;

namespace QuickUnity.Tests.IntegrationTests
{
    [IntegrationTest.DynamicTest("UnityHttpClientTests")]
    [IntegrationTest.SucceedWithAssertions]
    [IntegrationTest.Timeout(10)]
    public class UnityHttpClientSimplifiedGetTextureTest : MonoBehaviour
    {
        private UnityHttpClient client;

        // Use this for initialization
        private void Start()
        {
            client = UnityHttpClient.GetTexture("http://mat1.gtimg.com/www/images/qq2012/qqLogoFilter.png", true, OnResult, OnError);
        }

        private void OnDisable()
        {
            if (client != null)
            {
                client.Dispose();
                client = null;
            }
        }

        private void OnResult(UnityHttpClient httpClient, UnityHttpResponse response)
        {
            UnityHttpResponseTexture respTex = (UnityHttpResponseTexture)response;
            Texture2D tex = respTex.Texture;

            if (tex != null)
            {
                RawImage img = FindObjectOfType<RawImage>();

                if (img)
                {
                    img.texture = tex;
                }

                IntegrationTest.Pass();
                return;
            }

            IntegrationTest.Fail();
        }

        private void OnError(UnityHttpClient httpClient, string errorMessage)
        {
            Debug.LogError(errorMessage);
            IntegrationTest.Fail(gameObject, errorMessage);
        }
    }
}