using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UDT.Audio;

public class SoundInfo
{
    public string Tag = ""; // Tag of the sound
    public Vector3 Position = Vector3.zero; // Position of the sound

    public AudioSourceController AudioSourceController = null; // Reference to the AudioSourceController

    public SoundInfo(string tag, Vector3 position, AudioSourceController controller = null)
    {
        Tag = tag;
        Position = position;
    }
}