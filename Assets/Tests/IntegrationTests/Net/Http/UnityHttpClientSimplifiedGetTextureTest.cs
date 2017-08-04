using QuickUnity.Audio;
using QuickUnity.Net.Http;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace QuickUnity.Tests.IntegrationTests
{
    [IntegrationTest.DynamicTest("UnityHttpClientTests")]
    [IntegrationTest.SucceedWithAssertions]
    [IntegrationTest.Timeout(10)]
    public class UnityHttpClientSimplifiedGetTextureTest : MonoBehaviour, IUnityHttpResponder
    {
        private UnityHttpClient client;

        // Use this for initialization
        private void Start()
        {
            client = UnityHttpClient.GetTexture("http://mat1.gtimg.com/www/images/qq2012/qqLogoFilter.png", true, this);
        }

        private void OnDisable()
        {
            if (client != null)
            {
                client.Dispose();
                client = null;
            }
        }

        public void OnResult(UnityHttpResponse response)
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

        public void OnError(string errorMessage)
        {
            Debug.LogError(errorMessage);
            IntegrationTest.Fail(gameObject, errorMessage);
        }
    }
}