using System;
using UDT.Core;
using UnityEngine;

namespace UDT.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceController : MonoBehaviour
    {
        public string soundTag; // Tag for the sound
        private AudioSource audioSource; // Reference to the AudioSource component
        public SoundInfo soundInfo;
        
        
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            soundInfo = new SoundInfo(soundTag, transform.position, this);
        }

        private void Update()
        {
            // If the AudioSource has finished playing, remove it from the AudioModule
            if (!audioSource.isPlaying)
            {
                AudioModule.RemoveSound(soundInfo);
            }
            soundInfo.Position = transform.position;
            
        }

        private void OnEnable()
        {
            // Add the AudioSourceController to the AudioModule when enabled
            if (AudioModule.Instance != null)
            {
                AudioModule.AddAudioSourceController(this);
            }
        }

        private void OnDisable()
        {
            // Remove the AudioSourceController from the AudioModule when disabled
            if (AudioModule.Instance != null)
            {
                AudioModule.RemoveAudioSourceController(this);
            }
        }

        public void PlaySound()
        {
            // Play the sound using the AudioSource component
            audioSource.Play();

            // Inform the AudioModule that the sound is being played
            if (AudioModule.Instance != null)
            {
                Vector3 position = transform.position;
                AudioModule.AddSound(soundInfo);
            }
        }

        public void StopSound()
        {
            // Stop the sound using the AudioSource component
            audioSource.Stop();

            // Inform the AudioModule that the sound has finished playing
            if (AudioModule.Instance != null)
            {
                AudioModule.RemoveSound(soundInfo);
            }
        }
    }
}