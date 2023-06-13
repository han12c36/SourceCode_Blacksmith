using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class SoundManager : SubManager
{
    public float soundFadeTime;

    private AudioSource[] audioSources = new AudioSource[(int)SoundType.End];

    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    private Queue<AudioSource> surplusAudioSoruce_q = new Queue<AudioSource>();

    private GameObject soundBox;
    private GameObject bgmBox;
    private GameObject effectBox;
    private string[] soundNames;

    private Coroutine BGM_1_FadeInCoro;
    private Coroutine BGM_1_FadeOutCoro;
    private Coroutine BGM_2_FadeInCoro;
    private Coroutine BGM_2_FadeOutCoro;

    public AudioSource[] GetAudioSources { get { return audioSources; } }
    public override void SettingManagerForNextScene(int nextSceneIndex)
    {
        if (!isActivated) SetActivated(true);

        switch ((SceneIndex)nextSceneIndex)
        {
            case SceneIndex.Intro:
                ChangeBGMSound(Constants.BGM_Intro, true ,0.5f);
                break;
            case SceneIndex.Smithy:
                ChangeBGMSound(Constants.BGM_Smithy1, true, 0.1f);
                break;
            case SceneIndex.Workshop:
                ChangeBGMSound(Constants.BGM_Smithy0, true, 0.1f);
                break;
            case SceneIndex.Mine:
                ChangeBGMSound(Constants.BGM_Mine, true, 0.1f);
                break;
            case SceneIndex.Sell:
                ChangeBGMSound(Constants.BGM_Sell, true, 0.1f);
                break;
        }
    }
    public override void ManagerInitailized()
    {
        soundBox = PublicLibrary.GetNameBox(Constants.SoundObjectName);
        soundBox.transform.SetParent(gameObject.transform);
        soundNames = System.Enum.GetNames(typeof(SoundType));
        LoadBGMClips();
        LoadEffectClips();

    }
    private void LoadBGMClips()
    {
        AudioClip[] clips_1 = Resources.LoadAll<AudioClip>(Paths.AudioFolderPath_BGM);

        foreach (AudioClip clip in clips_1)
        {
            string clipName = clip.name;
            audioClips.Add(clipName, clip);
        }

        CreateAudioClipBox_BGM();
    }
    private void CreateAudioClipBox_BGM()
    {
        if (soundBox)
        {
            for(int i = (int)SoundType.BGM_1; i <= (int)SoundType.BGM_2; i++)
            {
                bgmBox = new GameObject { name = soundNames[i] };
                bgmBox.transform.SetParent(soundBox.transform);
                audioSources[i] = bgmBox.AddComponent<AudioSource>();
                audioSources[i].GetComponent<AudioSource>().volume = 0.0f;
                audioSources[i].loop = true;
            }
        }
    }

    private void LoadEffectClips()
    {
        AudioClip[] clips_2 = Resources.LoadAll<AudioClip>(Paths.AudioFolderPath_Effect);

        foreach (AudioClip clip in clips_2)
        {
            string clipName = clip.name;
            audioClips.Add(clipName, clip);
        }
        CreateAudioSourceBox_Effect();
    }

    private void CreateAudioSourceBox_Effect(int count = 50)
    {
        if (soundBox)
        {
            effectBox = new GameObject { name = soundNames[(int)SoundType.Effect] };
            effectBox.transform.SetParent(soundBox.transform);
            for (int i = 0; i < count; ++i)
            {
                CreateAudioSource_Effect(i);
            }
        }
    }
    private AudioSource CreateAudioSource_Effect(int num)
    {
        GameObject obj = new GameObject();
        obj.name = $"Surplus_{num}";
        obj.transform.parent = effectBox.transform;
        AudioSource surplusAudioSource = obj.AddComponent<AudioSource>();
        obj.AddComponent<AutoAudioSource>();
        surplusAudioSource.spatialBlend = 1f;
        obj.SetActive(false);
        surplusAudioSoruce_q.Enqueue(surplusAudioSource);
        return surplusAudioSource;
    }

    public AudioSource PlayBGMSound( string clipName, bool isFade = true, float volume = 1.0f)
    {
        AudioSource AS = null;
        if (audioSources[(int)SoundType.BGM_1].isPlaying)
        {
            if (audioSources[(int)SoundType.BGM_2].isPlaying)
            {
                AS = audioSources[(int)SoundType.BGM_1];
            }
            else
            {
                AS = audioSources[(int)SoundType.BGM_2];
            }
        }
        else
        {
            AS = audioSources[(int)SoundType.BGM_1];
        }
        AS.clip = GetClip(clipName);

        if (isFade)
        {
            if (AS == audioSources[(int)SoundType.BGM_1])
            {
                if(BGM_1_FadeOutCoro != null) GameManager.coroutineHelper.StopCoroutine(BGM_1_FadeOutCoro);
                if(BGM_1_FadeInCoro != null) GameManager.coroutineHelper.StopCoroutine(BGM_1_FadeInCoro);
                BGM_1_FadeInCoro = GameManager.coroutineHelper.StartCoroutine(Sound_FadeIn(audioSources[(int)SoundType.BGM_1], soundFadeTime, volume));
            }
            else if (AS == audioSources[(int)SoundType.BGM_2])
            {
                if (BGM_2_FadeOutCoro != null) GameManager.coroutineHelper.StopCoroutine(BGM_2_FadeOutCoro);
                if (BGM_2_FadeInCoro != null) GameManager.coroutineHelper.StopCoroutine(BGM_2_FadeInCoro);
                BGM_2_FadeInCoro = GameManager.coroutineHelper.StartCoroutine(Sound_FadeIn(audioSources[(int)SoundType.BGM_2], soundFadeTime, volume));
            }
        }
        else
        {
            AS.volume = volume;
            AS.Play();
        }

        return AS;
    }

    public void ChangeBGMSound(string clipName, bool isFade = true, float volume = 1.0f)
    {
        if (isFade)
        {
            AudioSource AS = PlayBGMSound(clipName, true, volume);
            if(AS == audioSources[(int)SoundType.BGM_1]) StopBGM(SoundType.BGM_2);
            else StopBGM(SoundType.BGM_1);
        }
        else
        {
            AudioSource AS = PlayBGMSound(clipName, false, volume);
            if (AS == audioSources[(int)SoundType.BGM_1]) StopBGM(SoundType.BGM_2,false);
            else StopBGM(SoundType.BGM_1,false);
        }
    }

    public void PlayEffectSound(string clipName,GameObject obj, float volume = 1f, float minPitch = 1f, float maxPitch = 1f)
    {
        AudioClip clip = GetClip(clipName);
        Vector3 pos = obj.transform.position;

        if (clip != null)
        {
            AudioSource aus = obj.transform.root.GetComponent<AudioSource>();
            if (aus)
            {
                aus.volume = volume;
                aus.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
                aus.PlayOneShot(clip);
            }
            else
            {
                AudioSource idleAS = GetIdleAudioSource();
                if (idleAS == null) idleAS = CreateAudioSource_Effect(surplusAudioSoruce_q.Count);

                idleAS.gameObject.SetActive(true);
                idleAS.volume = volume;
                idleAS.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
                idleAS.transform.position = pos;
                idleAS.PlayOneShot(clip);
            }
        }
    }
    
    private AudioSource GetIdleAudioSource()
    {
        for(int i = 0; i < surplusAudioSoruce_q.Count; i++)
        {
            AudioSource audio = surplusAudioSoruce_q.Dequeue();
            if (!audio.isPlaying)
            {
                surplusAudioSoruce_q.Enqueue(audio);
                return audio;
            }
            else surplusAudioSoruce_q.Enqueue(audio);
        }
        return null;
    }
    public void StopSound(string clipName)
    {
    }

    public void StopAllSound(bool isFade = true)
    {
        StopBGM(SoundType.BGM_1);
        StopBGM(SoundType.BGM_2);
        AudioSource[] all = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audio in all)
        {
            if (audio == audioSources[(int)SoundType.BGM_1]
                || audio == audioSources[(int)SoundType.BGM_2])
            {
                //audio.volume = 0.0f;
            }
            else audio.Stop();
        }
    }

    public void StopBGM(SoundType type = SoundType.BGM_1, bool isFade = true)
    {
        if (audioSources[(int)type])
        {
            if (isFade)
            {
                if (type == SoundType.BGM_1)
                {
                    if (BGM_1_FadeInCoro != null) GameManager.coroutineHelper.StopCoroutine(BGM_1_FadeInCoro);
                    if (BGM_1_FadeOutCoro != null) GameManager.coroutineHelper.StopCoroutine(BGM_1_FadeOutCoro);
                    BGM_1_FadeOutCoro = GameManager.coroutineHelper.StartCoroutine(Sound_FadeOut(audioSources[(int)SoundType.BGM_1], soundFadeTime));
                }
                else if (type == SoundType.BGM_2)
                {
                    if (BGM_2_FadeInCoro != null) GameManager.coroutineHelper.StopCoroutine(BGM_2_FadeInCoro);
                    if (BGM_2_FadeOutCoro != null) GameManager.coroutineHelper.StopCoroutine(BGM_2_FadeOutCoro);
                    BGM_2_FadeOutCoro = GameManager.coroutineHelper.StartCoroutine(Sound_FadeOut(audioSources[(int)SoundType.BGM_2], soundFadeTime));
                }
            }
            else audioSources[(int)type].Stop();
        }
    }

    //음향 : 점점 작아지고 화면 : 점점 검게 변함
    public IEnumerator Sound_FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;
        float durtaionTime = 0.0f;
        if (startVolume != 0.0f) durtaionTime = FadeTime * startVolume;
        else durtaionTime = FadeTime;

        while (audioSource.volume > 0)
        {
            yield return null;
            audioSource.volume -= startVolume * Time.deltaTime / durtaionTime;
        }

        audioSource.Stop();
    }

    //음향 : 점점 커지고 화면 : 점점 밝게 변함
    public IEnumerator Sound_FadeIn(AudioSource audioSource, float FadeTime, float volume)
    {
        audioSource.volume = 0f;
        audioSource.Play();
        
        while (audioSource.volume < volume)
        {
            audioSource.volume += Time.deltaTime / FadeTime;
            yield return null;
        }
    }

    public void Clear()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        audioClips.Clear();
    }
    private AudioClip GetClip(string clipName)
    {
        AudioClip clip = null;
        if(audioClips.TryGetValue(clipName, out clip)) return clip;
        return null;
    }
}

public class AutoAudioSource : MonoBehaviour
{
    [SerializeField]
    AudioSource AS;
    public void Awake() { AS = GetComponent<AudioSource>(); }

    public void Update() { if (!AS.isPlaying) gameObject.SetActive(false); }
}
