using QuickUnity.Audio;
using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    [IntegrationTest.DynamicTest("AudioSourcePlayerTests")]
    [IntegrationTest.SucceedWithAssertions]
    [IntegrationTest.Timeout(10)]
    public class AudioSourcePlayerFadeOutTest : MonoBehaviour
    {
        private AudioSourcePlayer audioPlayer;

        private GameObject targetGameObject;

        private void Awake()
        {
            targetGameObject = new GameObject();
            audioPlayer = targetGameObject.AddComponent<AudioSourcePlayer>();
            AudioClip clip = Resources.Load<AudioClip>("Audio/Music/Loop");

            audioPlayer.PlayAudio(clip);
            Invoke("FadeOutLater", 3f);
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

        private void FadeOutLater()
        {
            if (audioPlayer)
            {
                audioPlayer.FadeOut(3, 0, OnFadeOutCompleteCallback);
            }
        }

        private void OnFadeOutCompleteCallback()
        {
            if (audioPlayer.AudioSource.volume == 0)
            {
                IntegrationTest.Pass(gameObject);
            }
        }
    }
}