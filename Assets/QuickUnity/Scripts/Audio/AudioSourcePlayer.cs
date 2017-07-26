using QuickUnity.Events;
using System;
using System.Collections;
using UnityEngine;

namespace QuickUnity.Audio
{
    /// <summary>
    /// Player for AudioSource component.
    /// </summary>
    /// <seealso cref="QuickUnity.Events.BehaviourEventDispatcher"/>
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourcePlayer : BehaviourEventDispatcher
    {
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

        #region Public Functions

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
        /// This can be used in place of "play" when it is desired to fade in the sound over time.
        /// </summary>
        /// <param name="fadeInDuration">Duration of the fade in.</param>
        /// <param name="fadeToVolume">The volume need to reach.</param>
        /// <param name="startPosition">The start position.</param>
        /// <param name="completeCallback">The complete callback function.</param>
        public void FadeIn(float fadeInDuration = 0.0f, float fadeToVolume = 1.0f, float startPosition = 0.0f, Action completeCallback = null)
        {
            if (AudioSource && AudioSource.clip)
            {
                AudioSource.volume = 0.0f;
                AudioSource.time = startPosition;
                AudioSource.Play();

                if (fadeInDuration <= 0.0f)
                {
                    AudioSource.volume = fadeToVolume;
                }
                else
                {
                    StartCoroutine(ApplyFadeIn(fadeInDuration, fadeToVolume, completeCallback));
                }
            }
        }

        /// <summary>
        /// This is used in place of "stop" when it is desired to fade the volume of the sound before stopping.
        /// </summary>
        /// <param name="fadeOutDuration">Duration of the fade out.</param>
        /// <param name="fadeToVolume">The fade to volume.</param>
        /// <param name="completeCallback">The complete callback function.</param>
        public void FadeOut(float fadeOutDuration = 0.0f, float fadeToVolume = 0.0f, Action completeCallback = null)
        {
            if (AudioSource && AudioSource.clip)
            {
                if (fadeOutDuration <= 0.0f)
                {
                    AudioSource.volume = fadeToVolume;
                }
                else
                {
                    StartCoroutine(ApplyFadeOut(fadeOutDuration, fadeToVolume, completeCallback));
                }
            }
        }

        #endregion Public Functions

        #region Private Functions

        /// <summary>
        /// Applies the fade in effect.
        /// </summary>
        /// <param name="fadeInDuration">Duration of the fade in.</param>
        /// <param name="fadeVolume">The fade volume.</param>
        /// <param name="completeCallback">The complete callback function.</param>
        /// <returns>The enumerator of this coroutine.</returns>
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
                    AudioSource.volume -= (startVolume + Time.deltaTime) / fadeOutDuration;
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
            DispatchEvent(new AudioSourceEvent(AudioSourceEvent.PlayComplete, AudioSource));
        }

        #endregion Private Functions
    }
}