using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour {

    public static EffectsManager instance;

    [System.Serializable]
    public struct PlayOnStart
    {
        public string audioName;
        public bool loop;
        [Range(0, 1)]
        public float volume;
    }

    [Header("Audio Proporties")]
    public AudioClip[] allAudioclips;
    public PlayOnStart[] playOnStartClips;
    public int startSources;

    private List<AudioSource> audioSources = new List<AudioSource>();
    private int lastID;

    [Header("Particle Proporties")]
    public ParticleSystem[] allParticleSystems;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        for (int i = 0; i < startSources; i++)
        {
            CreateNewAudioSource();
        }

        foreach(PlayOnStart playOnStart in playOnStartClips)
        {
            PlayAudio(playOnStart.audioName, volume: playOnStart.volume, loop: playOnStart.loop);
        }
    }

    #region Public Audio Functions

    //------------------------------------------------------ Public Audio Functions ----------------------------------------------\\

    /// <summary>
    /// Plays an Audioclip.
    /// <para>If you don't have an AudioClip to play, use the FindAudioClip function.</para>
    /// </summary>
    /// <param name="toPlay">Audio clip to play</param>
    /// <param name="volume">volume of the audioSource</param>
    /// <param name="loop">Should the Audio be looping?</param>
    /// <param name="pitch">pitch of the Audio</param>
    /// <param name="spatialBlend">Should the audio be 3D?</param>
    /// <param name="audioPosition">Position of audio in worldspace. Only effective if the spatialBlend parameter is not 0</param>
    /// <returns>Returns AudioSource ID used to stop that specific audioSource.</returns>
    public int PlayAudio(AudioClip toPlay, float volume = 1, bool loop = false, float pitch = 1, float spatialBlend = 0, Vector3 audioPosition = default(Vector3))
    {
        AudioSource source = FindAvailableSource();
        int toReturn = source.GetComponent<SH_AudioSourceID>().id;
        source.clip = toPlay;

        source.volume = volume;
        source.loop = loop;
        source.pitch = pitch;
        source.spatialBlend = spatialBlend;
        source.transform.position = audioPosition;

        source.Play();

        return toReturn;
    }

    /// <summary>
    /// Plays an Audioclip.
    /// </summary>
    /// <param name="audioName">Name of the AudioClip that will be played (Clip needs to be stored in the Effects Manager)</param>
    /// <param name="volume">volume of the audioSource</param>
    /// <param name="loop">Should the Audio be looping?</param>
    /// <param name="pitch">pitch of the Audio</param>
    /// <param name="spatialBlend">Should the audio be 3D?</param>
    /// <param name="audioPosition">Position of audio in worldspace. Only effective if the spatialBlend parameter is not 0</param>
    /// <returns>Returns AudioSource ID used to stop that specific audioSource.</returns>
    public int PlayAudio(string audioName, float volume = 1, bool loop = false, float pitch = 1, float spatialBlend = 0, Vector3 audioPosition = default(Vector3))
    {
        AudioClip toPlay = FindAudioClip(audioName);

        return PlayAudio(toPlay, volume, loop, pitch, spatialBlend, audioPosition);
    }

    /// <summary>
    /// Simplified PlayAudio Function to use with UnityEvents.
    /// </summary>
    /// <param name="toPlay"></param>
    public void SimplePlayAudio(AudioClip toPlay)
    {
        PlayAudio(toPlay);
    }

    /// <summary>
    /// Finds an AudioClip stored in the Effect Manager
    /// </summary>
    /// <param name="audioName">Name of the AudioClip stored in the Effects Manager</param>
    /// <returns>Returns the AudioClip that will be played</returns>
    public AudioClip FindAudioClip(string audioName)
    {
        for (int i = 0; i < allAudioclips.Length; i++)
        {
            if (allAudioclips[i] != null)
            {
                if (allAudioclips[i].name == audioName)
                {
                    return allAudioclips[i];
                }
            }
            else
            {
                Debug.LogError("There is a null audio element in the Effect Manager");
            }
        }

        Debug.LogError("There is no AudioClip named " + audioName + " In the Effects Manager");
        return null;
    }

    /// <summary>
    /// Checks if the toCheck AudioClip is already being played.
    /// </summary>
    /// <param name="toCheck"></param>
    /// <returns></returns>
    public bool AudioClipIsPlaying(AudioClip toCheck)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].clip != null)
            {
                if (audioSources[i].clip.name == toCheck.name)
                {
                    if (audioSources[i].isPlaying)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Checks if the toCheck AudioClip is already being played.
    /// </summary>
    /// <param name="clipName">Name of the AudioClip that will be checked</param>
    /// <returns></returns>
    public bool AudioClipIsPlaying(string clipName)
    {
        AudioClip toCheck = FindAudioClip(clipName);

        return AudioClipIsPlaying(toCheck);
    }

    /// <summary>
    /// Stops the audio that is being played on the audiosource with audioSourceID.
    /// </summary>
    /// <param name="audiosourceID">ID of the Audio Source that will be checked.</param>
    public void StopAudio(int audioSourceID)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].GetComponent<SH_AudioSourceID>().id == audioSourceID)
            {
                audioSources[i].Stop();
                audioSources[i].clip = null;
            }
        }
    }

    /// <summary>
    /// Stops all instances of the toStop AudioClip.
    /// </summary>
    /// <param name="toStop"></param>
    public void StopAllPlayingClips(AudioClip toStop)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].clip.name == toStop.name)
            {
                audioSources[i].Stop();
                audioSources[i].clip = null;
            }
        }
    }

    /// <summary>
    /// Stops all instances of the toStop AudioClip.
    /// </summary>
    /// <param name="clipName">Name of the AudioClip that will be stopped</param>
    public void StopAllPlayingClips(string clipName)
    {
        AudioClip toStop = FindAudioClip(clipName);

        StopAllPlayingClips(toStop);
    }

    public void AdjustVolume(int audioSourceID, float volume)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].GetComponent<SH_AudioSourceID>().id == audioSourceID)
            {
                audioSources[i].volume = volume;
            }
        }
    }

    #endregion

    #region Private Audio Functions
    //------------------------------------------------------ Private Audio Functions ----------------------------------------------\\

    private AudioSource FindAvailableSource()
    {
        for (int s = 0; s < audioSources.Count; s++)
        {
            if (!audioSources[s].isPlaying)
            {
                return audioSources[s];
            }
        }

        return CreateNewAudioSource();
    }

    private AudioSource CreateNewAudioSource()
    {
        GameObject newSource = new GameObject();
        newSource.name = "AudioSource";
        newSource.transform.SetParent(transform);

        SH_AudioSourceID ID = newSource.AddComponent<SH_AudioSourceID>();
        ID.id = lastID;
        lastID += 1;

        AudioSource toReturn = newSource.AddComponent<AudioSource>();
        toReturn.playOnAwake = false;
        audioSources.Add(toReturn);

        return toReturn;
    }

    #endregion

    #region Public Particle Functions

    //------------------------------------------------------ Public Particle Functions ----------------------------------------------\\

    /// <summary>
    /// Play a particle.
    /// <para>If you don't have a particle to play, use the FindParticle function.</para>
    /// </summary>
    /// <param name="toPlay">The particlesystem that will be played</param>
    /// <param name="position">position in world space where the particle will be played at</param>
    /// <param name="rotation">rotation in world space how the particle will be played.</param>
    public void PlayParticle(ParticleSystem toPlay, Vector3 position, Quaternion rotation)
    {
        ParticleSystem playTo = CreateNewParticleSystem(toPlay);
        playTo.transform.position = position;
        playTo.transform.rotation = rotation;

        playTo.Play();

        StartCoroutine(ParticleDeathTimer(playTo));
    }

    /// <summary>
    /// Play a particle.
    /// <para>If you don't have a particle to play, use the FindParticle function.</para>
    /// </summary>
    /// <param name="particleName">name of the particle that will be played</param>
    /// <param name="position">position in world space where the particle will be played at</param>
    /// <param name="rotation">rotation in world space how the particle will be played.</param>
    public void PlayParticle(string particleName, Vector3 position, Quaternion rotation)
    {
        ParticleSystem toPlay = FindParticle(particleName);

        PlayParticle(toPlay, position, rotation);
    }

    /// <summary>
    /// Finds a particle stored in the Effect Manager.
    /// </summary>
    /// <param name="particleName"></param>
    /// <returns></returns>
    public ParticleSystem FindParticle(string particleName)
    {
        for (int i = 0; i < allParticleSystems.Length; i++)
        {
            if (allParticleSystems[i] != null)
            {
                if (allParticleSystems[i].name == particleName)
                {
                    return allParticleSystems[i];
                }
            }
            else
            {
                Debug.LogError("There is a null particle element in the Effect Manager");
            }
        }

        Debug.LogError("There is no Particle System named " + particleName + " in the Effects Manager");
        return null;
    }

    #endregion

    #region Private Particle Functions

    //------------------------------------------------------ Private Particle Functions ----------------------------------------------\\

    private ParticleSystem CreateNewParticleSystem(ParticleSystem system)
    {
        GameObject newObject = Instantiate(system.gameObject, transform.position, Quaternion.identity);
        newObject.name = "Particle System";

        return newObject.GetComponent<ParticleSystem>();
    }

    private IEnumerator ParticleDeathTimer(ParticleSystem system)
    {
        yield return new WaitForSeconds(system.main.duration);
        system.Stop();
        Destroy(system.gameObject);
    }

    #endregion
}
