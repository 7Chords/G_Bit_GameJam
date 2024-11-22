using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioInfo
{
    public string audioName;

    public AudioSource audioSource;
}
public class AudioManager : SingletonPersistent<AudioManager>
{
    //场上存在的BGM信息
    public List<AudioInfo> bgmAudioInfoList;

    //场上存在的Sfx信息
    public List<AudioInfo> sfxAudioInfoList;

    //音频音量 整体控制
    public float mainVolume;

    //BGM音量参数 BGM = mainVolume*bgmVolumeFactor
    public float bgmVolumeFactor;

    //Sfx音量参数 Sfx = mainVolume*sfxVolumeFactor
    public float sfxVolumeFactor;

    //所有音频的配置数据
    public AudioDatas audioDatas;

    private GameObject _bgmSourcesRootGO;

    private GameObject _sfxSourcesRootGO;


    protected override void Awake()
    {
        base.Awake();

        //创建BGM和SFX的AudioSource挂载的父物体
        _bgmSourcesRootGO = new GameObject("BGM_ROOT");

        _sfxSourcesRootGO = new GameObject("SFX_ROOT");

        _bgmSourcesRootGO.transform.SetParent(transform);

        _sfxSourcesRootGO.transform.SetParent(transform);
    }

    /// <summary>
    /// 播放Bgm
    /// </summary>
    /// <param name="fadeInMusicName">播放的音乐名</param>
    /// <param name="fadeOutMusicName">淡出的音乐名 默认空 及叠加模仿</param>
    /// <param name="fadeInDuration">音乐淡入的时长</param>
    /// <param name="fadeOutDuration">音乐淡出的时长</param>
    /// <param name="loop">是否循环</param>
    public void PlayBgm(string fadeInMusicName, string fadeOutMusicName = "", float fadeInDuration = 0.5f, float fadeOutDuration = 0.5f, bool loop = true)
    {
        //利用DoTween做淡入淡出的效果
        Sequence s = DOTween.Sequence();

        //如果需要淡出某个背景音乐
        if (fadeOutMusicName != "")
        {
            AudioInfo fadeOutInfo = bgmAudioInfoList.Find(x => x.audioName == fadeOutMusicName);

            if (fadeOutInfo == null)
            {
                Debug.LogWarning("当前并未播放" + fadeOutMusicName + ",无法淡出该音乐");
                return;
            }

            s.Append(fadeOutInfo.audioSource.DOFade(0, fadeOutDuration).OnComplete(() =>
            {
                fadeOutInfo.audioSource.Pause();
            }));
        }

        //如果要播放的音乐当前场上已经存在 被暂停了就继续播放？
        AudioInfo audioInfo = bgmAudioInfoList.Find(x => x.audioName == fadeInMusicName);

        if (audioInfo != null)
        {
            s.Append(audioInfo.audioSource.DOFade(mainVolume * bgmVolumeFactor, fadeInDuration).OnComplete(() =>
            {
                audioInfo.audioSource.Play();
            }));

            return;
        }

        //找到要播放的音频数据
        AudioData fadeInData = audioDatas.audioDataList.Find(x => x.audioName == fadeInMusicName);

        if (fadeInData == null)
        {
            Debug.LogWarning("音频数据SO中不存在名为" + fadeInMusicName + "的音频数据");
            return;
        }


        //创建一个新物体挂上AudioSource并设置播放参数 进行播放
        GameObject fadeInAudioGO = new GameObject(fadeInMusicName);

        fadeInAudioGO.transform.SetParent(_bgmSourcesRootGO.transform);

        AudioSource fadeInAudioSource = fadeInAudioGO.AddComponent<AudioSource>();

        fadeInAudioSource.clip = Resources.Load<AudioClip>("Audio/" + fadeInData.audioPath);

        fadeInAudioSource.loop = loop;

        fadeInAudioSource.Play();

        s.Append(fadeInAudioSource.DOFade(mainVolume * bgmVolumeFactor, fadeInDuration));

        AudioInfo info = new AudioInfo();

        //把该bgm播放信息添加到列表
        info.audioName = fadeInMusicName;

        info.audioSource = fadeInAudioSource;

        bgmAudioInfoList.Add(info);

        StartCoroutine(DetectingAudioPlayState(info, true));
    }


    /// <summary>
    /// 暂停Bgm
    /// </summary>
    /// <param name="pauseBgmName">暂停的音乐名</param>
    /// <param name="fadeOutDuration">音乐淡出的时长</param>
    public void PauseBgm(string pauseBgmName, float fadeOutDuration = 0.5f)
    {
        AudioInfo audioInfo = bgmAudioInfoList.Find(x => x.audioName == pauseBgmName);

        if (audioInfo == null)
        {
            Debug.LogWarning("当前并未播放" + pauseBgmName + ",无法暂停该音乐");
            return;
        }

        Sequence s = DOTween.Sequence();

        s.Append(audioInfo.audioSource.DOFade(0, fadeOutDuration).OnComplete(() =>
        {
            audioInfo.audioSource.Pause();
        }));
    }



    /// <summary>
    /// 暂停音乐
    /// </summary>
    /// <param name="stopBgmName">暂停的音乐名</param>
    /// <param name="fadeOutDuration">音乐淡出的时长</param>
    public void StopBgm(string stopBgmName, float fadeOutDuration = 0.5f)
    {
        AudioInfo audioInfo = bgmAudioInfoList.Find(x => x.audioName == stopBgmName);

        if (audioInfo == null)
        {
            Debug.LogWarning("当前并未播放" + stopBgmName + ",无法结束该音乐");
            return;
        }

        Sequence s = DOTween.Sequence();

        s.Append(audioInfo.audioSource.DOFade(0, fadeOutDuration).OnComplete(() =>
        {
            audioInfo.audioSource.Stop();

            Destroy(audioInfo.audioSource.gameObject);
        }));

        bgmAudioInfoList.Remove(audioInfo);

    }


    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="sfxName">播放的音效名</param>
    /// <param name="fadeInDuration">音效淡入的时长</param>
    /// <param name="loop">是否循环</param>
    public void PlaySfx(string sfxName, float fadeInDuration = 0, bool loop = false)
    {
        Sequence s = DOTween.Sequence();

        //找到要播放的音效数据
        AudioData sfxData = audioDatas.audioDataList.Find(x => x.audioName == sfxName);

        if (sfxData == null)
        {
            Debug.LogWarning("音频数据SO中不存在名为" + sfxName + "的音频数据");
            return;
        }


        //创建一个新物体挂上AudioSource并设置播放参数 进行播放
        GameObject sfxAudioGO = new GameObject(sfxName);

        sfxAudioGO.transform.SetParent(_sfxSourcesRootGO.transform);

        AudioSource sfxAudioSource = sfxAudioGO.AddComponent<AudioSource>();

        sfxAudioSource.clip = Resources.Load<AudioClip>("Audio/" + sfxData.audioPath);

        sfxAudioSource.loop = loop;

        sfxAudioSource.Play();

        s.Append(sfxAudioSource.DOFade(mainVolume * sfxVolumeFactor, fadeInDuration));

        AudioInfo info = new AudioInfo();

        //把该bgm播放信息添加到列表
        info.audioName = sfxName;

        info.audioSource = sfxAudioSource;

        sfxAudioInfoList.Add(info);

        StartCoroutine(DetectingAudioPlayState(info, false));
        //ThreadPool.QueueUserWorkItem(new WaitCallback(DetectingAudioPlayState), sfxAudioGO);//将方法添加进线程池，并传入参数
    }

    /// <summary>
    /// 暂停音效
    /// </summary>
    /// <param name="pauseSfxName">暂停的音效名</param>
    public void PauseSfx(string pauseSfxName)
    {
        AudioInfo audioInfo = sfxAudioInfoList.Find(x => x.audioName == pauseSfxName);

        if (audioInfo == null)
        {
            Debug.LogWarning("当前并未播放" + pauseSfxName + ",无法暂停该音效");
            return;
        }

        audioInfo.audioSource.Pause();
    }


    /// <summary>
    /// 停止音效
    /// </summary>
    /// <param name="stopSfxName">停止的音效名</param>
    public void StopSfx(string stopSfxName)
    {
        AudioInfo audioInfo = bgmAudioInfoList.Find(x => x.audioName == stopSfxName);

        if (audioInfo == null)
        {
            Debug.LogWarning("当前并未播放" + stopSfxName + ",无法结束该音效");
            return;
        }

        audioInfo.audioSource.Stop();

        bgmAudioInfoList.Remove(audioInfo);

        Destroy(audioInfo.audioSource.gameObject);
    }


    /// <summary>
    /// 改变主音量
    /// </summary>
    /// <param name="volume"></param>
    public void ChangeMainVolume(float volume)
    {
        mainVolume = volume;

        foreach (var info in bgmAudioInfoList)
        {
            info.audioSource.volume = mainVolume * bgmVolumeFactor;

        }
        foreach (var info in sfxAudioInfoList)
        {
            info.audioSource.volume = mainVolume * sfxVolumeFactor;
        }
    }

    /// <summary>
    /// 改变Bgm音量因子
    /// </summary>
    /// <param name="factor"></param>
    public void ChangeBgmVolume(float factor)
    {
        bgmVolumeFactor = factor;

        foreach (var info in bgmAudioInfoList)
        {
            info.audioSource.volume = mainVolume * bgmVolumeFactor;
        }
    }


    /// <summary>
    /// 改变Sfx音量因子
    /// </summary>
    /// <param name="factor"></param>
    public void ChangeSfxVolume(float factor)
    {
        sfxVolumeFactor = factor;

        foreach (var info in sfxAudioInfoList)
        {
            info.audioSource.volume = mainVolume * sfxVolumeFactor;
        }
    }

    IEnumerator DetectingAudioPlayState(AudioInfo info, bool isBgm)
    {
        AudioSource audioSource = info.audioSource;
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        if (isBgm)
        {
            bgmAudioInfoList.Remove(info);
        }
        else
        {
            sfxAudioInfoList.Remove(info);
        }

        Destroy(info.audioSource.gameObject);
    }
}
