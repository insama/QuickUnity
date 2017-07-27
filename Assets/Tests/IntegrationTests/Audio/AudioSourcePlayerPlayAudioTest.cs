using QuickUnity.Audio;
using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    [IntegrationTest.DynamicTest("AudioSourcePlayerTests")]
    [IntegrationTest.SucceedWithAssertions]
    [IntegrationTest.Timeout(10)]
    public class AudioSourcePlayerPlayAudioTest : MonoBehaviour
    {
        private AudioSourcePlayer audioPlayer;

        private GameObject targetGameObject;

        private void Awake()
        {
            targetGameObject = new GameObject();
            audioPlayer = targetGameObject.AddComponent<AudioSourcePlayer>();
            audioPlayer.AudioPlayCompleted.AddListener(OnAudioPlayCompleted);
            AudioClip clip = Resources.Load<AudioClip>("Audio/Sounds/Ring");
            audioPlayer.PlayAudio(clip);
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

        private void OnAudioPlayCompleted(AudioSourcePlayer audioPlayer, AudioSource audioSource)
        {
            IntegrationTest.Pass(gameObject);
        }
    }
}