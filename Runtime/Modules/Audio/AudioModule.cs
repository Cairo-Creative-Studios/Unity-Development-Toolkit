using System.Collections.Generic;
using UDT.Core;
using UnityEngine;

namespace UDT.Audio
{
    /// <summary>
    /// The AudioModule is a Singleton that manages all the AudioSources in the Game.
    /// </summary>
    public class AudioModule : Runtime<AudioModule>, IData<AudioModuleData>
    {
        private List<SoundInfo> recentSounds = new List<SoundInfo>(); // Array to store recent sounds
        private List<AudioSourceController> audioSourceControllers = new List<AudioSourceController>(); // List of all AudioSourceController instances

        public static SoundInfo[] GetRecentSounds()
        {
            return Instance.recentSounds.ToArray();
        }

        /// <summary>
        /// Add the Sound Info to the recent sounds list
        /// </summary>
        /// <param name="controller"></param>
        public static void AddSound(SoundInfo soundInfo)
        {
            Instance.recentSounds.Add(soundInfo);
        }

        /// <summary>
        /// Remove a Sound from the recent sounds list by it's SoundInfo
        /// </summary>
        /// <param name="soundInfo"></param>
        public static void RemoveSound(SoundInfo soundInfo)
        {
            Instance.recentSounds.Remove(soundInfo);
        }
        
        /// <summary>
        /// Add an AudioSourceController to the list of all AudioSourceControllers
        /// </summary>
        /// <param name="audioSourceController"></param>
        public static void AddAudioSourceController(AudioSourceController audioSourceController)
        {
            // Add the AudioSourceController to the list
            Instance.audioSourceControllers.Add(audioSourceController);
        }
        
        /// <summary>
        /// Remove an AudioSourceController from the list of all AudioSourceControllers
        /// </summary>
        /// <param name="audioSourceController"></param>
        public static void RemoveAudioSourceController(AudioSourceController audioSourceController)
        {
            // Remove the AudioSourceController from the list
            Instance.audioSourceControllers.Remove(audioSourceController);
        }

        public bool Initialized { get; set; }
        public AudioModuleData _Data { get; set; }
    }
    
}