using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioService : MonoBehaviour, IPause
{
    [SerializeField] List<AudioSource> _sources = new List<AudioSource>();
    [SerializeField] List<AudioSource> _reserved = new List<AudioSource>();
    [SerializeField] List<Timer> timers = new List<Timer>();

    [SerializeField] AudioSource backgroundSource;

    private void OnEnable()
    {
        GameManager.current.updateService.RegisterPause(this);
    }

    private void Start()
    {
        backgroundSource = gameObject.AddComponent<AudioSource>();

        for (int i = 0; i < 10; i++)
        {
            _sources.Add(Instantiate(GameManager.current.gameInfo.audioSourcePrefab).GetComponent<AudioSource>());
        }
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

    public void PlaySound(AudioClip clip, Vector3? pos, float variation = 0f, float time = -1f, bool forcePlay = false)
    {
        PlaySound(clip, pos, null, time, forcePlay, variation);
    }
    public void PlaySound(AudioClip clip, AudioSource external, float variation = 0f, float time = -1f, bool forcePlay = false)
    {
        PlaySound(clip, null, external, time, forcePlay, variation);
    }
    public void PlaySound(AudioClip clip, Vector3? pos = null, AudioSource external = null, float time = -1f, bool forcePlay = false, float variation = 0f)
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
            else source.transform.position = GameManager.current.gameInfo.playerPositionVar.Value;
            //else source.transform.position = transform.position;
        }
        else
        {
            if (forcePlay) external.Stop();
            source = external;
        }

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
        else
        {
            if (variation > 0f) source.pitch = Random.Range(1 - (variation / 2), 1 + (variation / 2));
            source.PlayOneShot(clip);
        }
    }

    public void PlayBackgroundMusic(AudioClip clip)
    {
        backgroundSource.loop = true;
        backgroundSource.Play();
        
    }


}
