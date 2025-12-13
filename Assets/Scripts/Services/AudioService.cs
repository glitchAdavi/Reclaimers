using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class AudioService : MonoBehaviour, IPause
{
    [SerializeField] List<AudioSource> _sources;
    [SerializeField] List<AudioSource> _reserved;
    [SerializeField] List<Timer> timers = new List<Timer>();

    [SerializeField] AudioSource backgroundSource;

    private void OnEnable()
    {
        backgroundSource = gameObject.AddComponent<AudioSource>();

        for (int i = 0; i < 10; i++)
        {
            Instantiate(GameManager.current.gameInfo.audioSourcePrefab);
        }

        GameManager.current.updateService.RegisterPause(this);
    }

    public void Pause(bool paused)
    {
        foreach (Timer t in timers)
        {
            t.Pause(paused);
        }
    }

    private void OnDisable()
    {
        GameManager.current.updateService.UnregisterPause(this);
    }

    public AudioSource GetAudioSource(bool force = false)
    {
        AudioSource result = null;
        foreach(AudioSource source in _sources)
        {
            if (!_reserved.Contains(source))
            {
                if (force)
                {
                    result = source;
                    break;
                }
                if (!source.isPlaying)
                {
                    result = source;
                    break;
                }
            }
        }
        return result;
    }

    public void PlaySound(AudioClip clip, Vector3? pos, float time = -1f, bool forcePlay = false)
    {
        PlaySound(clip, pos, time, forcePlay, null);
    }
    public void PlaySound(AudioClip clip, AudioSource external, float time = -1f, bool forcePlay = false)
    {
        PlaySound(clip, null, time, forcePlay, external);
    }
    public void PlaySound(AudioClip clip, Vector3? pos = null, float time = -1f, bool forcePlay = false, AudioSource external = null)
    {
        if (clip == null)
        {
            Debug.Log("Missing audio clip.");
            return;
        }

        AudioSource source;
        if (external == null)
        {
            source = GetAudioSource(forcePlay);
            if (pos != null) source.transform.position = (Vector3)pos;
            else source.transform.position = transform.position;
        }
        else source = external;

        if (time > 0f)
        {
            source.loop = true;
            _reserved.Add(source);
            timers.Add(GameManager.current.timerService.StartTimer(time, () =>
            {
                source.Stop();
                source.loop = false;
                _reserved.Remove(source);
            }));
            source.clip = clip;
            source.Play();
        }
        else source.PlayOneShot(clip);
    }

    public void PlayBackgroundMusic(AudioClip clip)
    {
        backgroundSource.loop = true;
        backgroundSource.Play();
        
    }


}
