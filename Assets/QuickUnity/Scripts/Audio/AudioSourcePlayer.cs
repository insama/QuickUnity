using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace QuickUnity.Audio
{
    /// <summary>
    /// The <see cref="UnityEvent"/> presents
    /// </summary>
    /// <typeparam name="AudioSourcePlayer">The <see cref="AudioSourcePlayer"/> component.</typeparam>
    /// <typeparam name="AudioSource">The <see cref="AudioSource"/> compoent.</typeparam>
    /// <seealso cref="UnityEvent"/>
    [Serializable]
    public class AudioSourcePlayeCompleteEvent : UnityEvent<AudioSourcePlayer, AudioSource> { }

    /// <summary>
    /// Player for AudioSource component.
    /// </summary>
    /// <seealso cref="MonoBehaviour"/>
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourcePlayer : MonoBehaviour
    {
        /// <summary>
        /// The event of audio play completed.
        /// </summary>
        public AudioSourcePlayeCompleteEvent AudioPlayCompleted;

        /// <summary>
        /// The AudioSource component.
        /// </summary>
        private AudioSource audioSource;

        /// <summary>
        /// Gets the AudioSource component.
        /// </summary>
        /// <value>The AudioSource component.</value>
        public AudioSource AudioSource
        {
            get
            {
                if (!audioSource)
                {
                    audioSource = GetComponent<AudioSource>();
                }

                return audioSource;
            }
        }

        #region Messages

        /// <summary>
        /// Awakes this instance.
        /// </summary>
        private void Awake()
        {
            AudioPlayCompleted = new AudioSourcePlayeCompleteEvent();
        }

        private void OnDestroy()
        {
            AudioPlayCompleted.RemoveAllListeners();
            AudioPlayCompleted = null;
        }

        #endregion Messages

        #region Public Methods

        /// <summary>
        /// Play the audio source.
        /// </summary>
        public void PlayAudio()
        {
            if (AudioSource && AudioSource.clip)
            {
                StartCoroutine(DoPlayAudio());
            }
        }

        /// <summary>
        /// Plays the audio by setting audio clip.
        /// </summary>
        /// <param name="clip">The audio clip.</param>
        public void PlayAudio(AudioClip clip)
        {
            if (AudioSource && clip)
            {
                AudioSource.clip = clip;
                StartCoroutine(DoPlayAudio());
            }
        }

        /// <summary>
        /// This can be used in place of "PlayAudio" when it is desired to fade in the sound over time.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <param name="fromVolume">The volume start from.</param>
        /// <param name="toVolume">The volume to fade to.</param>
        /// <param name="startPosition">The start position.</param>
        /// <param name="completeCallback">The complete callback function.</param>
        /// <exception cref="NullReferenceException"><c>AudioSource</c> is <c>null</c> or <c>AudioSource.clip</c> is <c>null</c>.</exception>
        public void FadeIn(float duration = 0f, float fromVolume = 0f, float toVolume = 1f, float startPosition = 0f, Action completeCallback = null)
        {
            if (!AudioSource || !AudioSource.clip)
            {
                throw new NullReferenceException("AudioSource is null or AudioSource.clip is null.");
            }

            AudioSource.volume = fromVolume;
            AudioSource.time = startPosition;
            PlayAudio();

            if (duration <= 0.0f)
            {
                AudioSource.volume = toVolume;
            }
            else
            {
                StartCoroutine(ApplyFadeIn(duration, toVolume, completeCallback));
            }
        }

        /// <summary>
        /// This can be used in place of "PlayAudio" when it is desired to fade in the sound over time.
        /// </summary>
        /// <param name="clip">The clip.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="fromVolume">The volume start from.</param>
        /// <param name="toVolume">The volume to fade to.</param>
        /// <param name="startPosition">The start position.</param>
        /// <param name="completeCallback">The complete callback function.</param>
        /// <exception cref="ArgumentNullException"><c>clip</c> is <c>null</c>.</exception>
        /// <exception cref="NullReferenceException"><c>AudioSource</c> is <c>null</c>.</exception>
        public void FadeIn(AudioClip clip, float duration = 0f, float fromVolume = 0f, float toVolume = 1f, float startPosition = 0f, Action completeCallback = null)
        {
            if (!clip)
            {
                throw new ArgumentNullException("clip");
            }

            if (!AudioSource)
            {
                throw new NullReferenceException("AudioSource is null.");
            }

            AudioSource.volume = fromVolume;
            AudioSource.time = startPosition;
            PlayAudio(clip);

            if (duration <= 0.0f)
            {
                AudioSource.volume = toVolume;
            }
            else
            {
                StartCoroutine(ApplyFadeIn(duration, toVolume, completeCallback));
            }
        }

        /// <summary>
        /// This is used in place of "stop" when it is desired to fade the volume of the sound before stopping.
        /// </summary>
        /// <param name="duration">Duration of the fade out.</param>
        /// <param name="toVolume">The fade to volume.</param>
        /// <param name="completeCallback">The complete callback function.</param>
        public void FadeOut(float duration = 0.0f, float toVolume = 0.0f, Action completeCallback = null)
        {
            if (AudioSource && AudioSource.clip)
            {
                if (duration <= 0.0f)
                {
                    AudioSource.volume = toVolume;
                }
                else
                {
                    StartCoroutine(ApplyFadeOut(duration, toVolume, completeCallback));
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private IEnumerator ApplyFadeIn(float fadeInDuration = 0.0f, float fadeToVolume = 1.0f, Action completeCallback = null)
        {
            if (AudioSource)
            {
                float startVolume = AudioSource.volume;

                while (AudioSource.volume < fadeToVolume)
                {
                    AudioSource.volume += (startVolume + Time.deltaTime) / fadeInDuration;
                    yield return null;
                }

                AudioSource.volume = fadeToVolume;

                if (completeCallback != null)
                {
                    completeCallback.Invoke();
                }
            }
        }

        /// <summary>
        /// Applies the fade out effect.
        /// </summary>
        /// <param name="fadeOutDuration">Duration of the fade out.</param>
        /// <param name="fadeVolume">The fade volume.</param>
        /// <param name="completeCallback">The complete callback function.</param>
        /// <returns>The enumerator of this coroutine.</returns>
        private IEnumerator ApplyFadeOut(float fadeOutDuration = 0.0f, float fadeToVolume = 0.0f, Action completeCallback = null)
        {
            if (AudioSource)
            {
                float startVolume = AudioSource.volume;

                while (AudioSource.volume > fadeToVolume)
                {
                    AudioSource.volume -= startVolume * Time.deltaTime / fadeOutDuration;
                    yield return null;
                }

                AudioSource.volume = fadeToVolume;
                AudioSource.Stop();

                if (completeCallback != null)
                {
                    completeCallback.Invoke();
                }
            }
        }

        /// <summary>
        /// Play audio.
        /// </summary>
        /// <returns>The enumerator of this coroutine.</returns>
        private IEnumerator DoPlayAudio()
        {
            AudioSource.Play();
            yield return new WaitForSeconds(AudioSource.clip.length / AudioSource.pitch);

            if (AudioPlayCompleted != null)
            {
                AudioPlayCompleted.Invoke(this, AudioSource);
            }
        }

        #endregion Private Methods
    }
}