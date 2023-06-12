using UDT.Core;
using UnityEngine;

namespace UDT.Audio
{
    [CreateAssetMenu(fileName = "AudioModuleData", menuName = "Runtime Data/AudioModuleData")]
    public class AudioModuleData : Data
    {
        [Header("Audio Settings")]
        public float masterVolume = 1f; // Master volume control
        public float soundEffectVolume = 1f; // Sound effects volume control
        public float musicVolume = 1f; // Music volume control

        [Header("Other Audio Settings")]
        public bool enableSpatialAudio = true; // Enable spatial audio simulation
        public bool enableReverb = true; // Enable global reverb effect
        // Add more audio settings as needed...

        // Any additional properties, methods, or customization specific to AudioModuleData...

    }
}