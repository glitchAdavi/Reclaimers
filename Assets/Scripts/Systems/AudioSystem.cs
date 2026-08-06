using UnityEngine;
using Reclaimers.Systems;
using Reclaimers.UpdateSystem;
using System.Linq;

public class AudioSystem : MonoBehaviour, IPauseable
{
    [SerializeField] CustomList<AudioSource> _sources = new CustomList<AudioSource>();
    [SerializeField] CustomList<AudioSource> _reserved = new CustomList<AudioSource>();
    [SerializeField] CustomList<Timer> timers = new CustomList<Timer>();

    [SerializeField] AudioSource backgroundSource;

    private void OnEnable()
    {
        this.Register();
    }

    private void Start()
    {
        backgroundSource = gameObject.AddComponent<AudioSource>();

        for (int i = 0; i < 50; i++)
        {
            _sources.Add(Instantiate(GameManager.Instance.gameInfo.audioSourcePrefab).GetComponent<AudioSource>());
        }
    }

    public void ManagedPause(bool paused)
    {
        foreach (Timer t in timers)
        {
            t.Pause(paused);
        }
    }

    private void OnDisable()
    {
        foreach (AudioSource aSource in _sources)
        {
            _sources.Remove(aSource);
            Destroy(aSource);
        }

        foreach (AudioSource aSource in _reserved)
        {
            _reserved.Remove(aSource);
            Destroy(aSource);
        }

        Destroy(backgroundSource);

        this.Unregister();
    }

    public AudioSource GetAudioSource(bool force = false)
    {
        AudioSource result = null;
        foreach (AudioSource source in _sources)
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
            if (source != null)
            {
                if (pos != null) source.transform.position = (Vector3)pos;
                else source.transform.position = GameManager.Instance.gameInfo.playerPositionVar.Value;
            }
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
            timers.Add(GameManager.Instance.timerSystem.SetTimer(time, () =>
            {
                source.Stop();
                source.loop = false;
                _reserved.Remove(source);
            }));
            source.clip = clip;
            if (source != null) source.Play();
        }
        else
        {
            if (variation > 0f) source.pitch = Random.Range(1 - (variation / 2), 1 + (variation / 2));
            if (source != null) source.PlayOneShot(clip);
        }
    }

    public void PlayBackgroundMusic(AudioClip clip)
    {
        backgroundSource.loop = true;
        backgroundSource.Play();

    }
}
