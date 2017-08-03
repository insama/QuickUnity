using QuickUnity.Audio;
using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    [IntegrationTest.DynamicTest("AudioSourcePlayerTests")]
    [IntegrationTest.SucceedWithAssertions]
    [IntegrationTest.Timeout(10)]
    internal class AudioSourcePlayerFadeInTest : MonoBehaviour
    {
        private AudioSourcePlayer audioPlayer;

        private GameObject targetGameObject;

        private void Awake()
        {
            targetGameObject = new GameObject();
            audioPlayer = targetGameObject.AddComponent<AudioSourcePlayer>();
            AudioClip clip = Resources.Load<AudioClip>("Audio/Music/Loop");
            audioPlayer.FadeIn(clip, 3, 0, 1, 0, OnFadeInCompleteCallback);
        }

        private void OnDisable()
        {
            if (audioPlayer)
            {
                audioPlayer.AudioPlayCompleted.RemoveAllListeners();
            }

            if (targetGameObject)
            {
                Destroy(targetGameObject);
            }
        }

        private void OnFadeInCompleteCallback()
        {
            if (audioPlayer.AudioSource.volume == 1)
            {
                IntegrationTest.Pass(gameObject);
            }
        }
    }
}