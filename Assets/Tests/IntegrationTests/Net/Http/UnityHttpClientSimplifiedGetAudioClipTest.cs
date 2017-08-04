using QuickUnity.Audio;
using QuickUnity.Net.Http;
using System;
using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    [IntegrationTest.DynamicTest("UnityHttpClientTests")]
    [IntegrationTest.SucceedWithAssertions]
    [IntegrationTest.Timeout(10)]
    public class UnityHttpClientSimplifiedGetAudioClipTest : MonoBehaviour
    {
        private UnityHttpClient client;

        private GameObject audioGameObject;

        // Use this for initialization
        private void Start()
        {
            client = UnityHttpClient.GetAudioClip("http://fjdx.sc.chinaz.com/Files/DownLoad/sound1/201707/9001.wav", AudioType.WAV, OnResult, OnError);
        }

        private void OnDisable()
        {
            if (client != null)
            {
                client.Dispose();
                client = null;
            }
        }

        private void OnResult(UnityHttpResponse response)
        {
            UnityHttpResponseAudioClip audioResp = (UnityHttpResponseAudioClip)response;
            AudioClip clip = audioResp.AudioClip;

            audioGameObject = new GameObject();
            AudioSourcePlayer player = audioGameObject.AddComponent<AudioSourcePlayer>();

            if (player)
            {
                player.AudioPlayCompleted.AddListener((AudioSourcePlayer p, EventArgs e) =>
                {
                    Destroy(audioGameObject);
                    IntegrationTest.Pass();
                });
                player.PlayAudio(clip);
            }
        }

        private void OnError(string errorMessage)
        {
            Debug.LogError(errorMessage);
            IntegrationTest.Fail(gameObject, errorMessage);
        }
    }
}