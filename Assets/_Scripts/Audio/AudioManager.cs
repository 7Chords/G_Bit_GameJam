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
    //���ϴ��ڵ�BGM��Ϣ
    public List<AudioInfo> bgmAudioInfoList;

    //���ϴ��ڵ�Sfx��Ϣ
    public List<AudioInfo> sfxAudioInfoList;

    //��Ƶ���� �������
    public float mainVolume;

    //BGM�������� BGM = mainVolume*bgmVolumeFactor
    public float bgmVolumeFactor;

    //Sfx�������� Sfx = mainVolume*sfxVolumeFactor
    public float sfxVolumeFactor;

    //������Ƶ����������
    public AudioDatas audioDatas;

    private GameObject _bgmSourcesRootGO;

    private GameObject _sfxSourcesRootGO;


    protected override void Awake()
    {
        base.Awake();

        //����BGM��SFX��AudioSource���صĸ�����
        _bgmSourcesRootGO = new GameObject("BGM_ROOT");

        _sfxSourcesRootGO = new GameObject("SFX_ROOT");

        _bgmSourcesRootGO.transform.SetParent(transform);

        _sfxSourcesRootGO.transform.SetParent(transform);
    }

    /// <summary>
    /// ����Bgm
    /// </summary>
    /// <param name="fadeInMusicName">���ŵ�������</param>
    /// <param name="fadeOutMusicName">������������ Ĭ�Ͽ� ������ģ��</param>
    /// <param name="fadeInDuration">���ֵ����ʱ��</param>
    /// <param name="fadeOutDuration">���ֵ�����ʱ��</param>
    /// <param name="loop">�Ƿ�ѭ��</param>
    public void PlayBgm(string fadeInMusicName, string fadeOutMusicName = "", float fadeInDuration = 0.5f, float fadeOutDuration = 0.5f, bool loop = true)
    {
        //����DoTween�����뵭����Ч��
        Sequence s = DOTween.Sequence();

        //�����Ҫ����ĳ����������
        if (fadeOutMusicName != "")
        {
            AudioInfo fadeOutInfo = bgmAudioInfoList.Find(x => x.audioName == fadeOutMusicName);

            if (fadeOutInfo == null)
            {
                Debug.LogWarning("��ǰ��δ����" + fadeOutMusicName + ",�޷�����������");
                return;
            }

            s.Append(fadeOutInfo.audioSource.DOFade(0, fadeOutDuration).OnComplete(() =>
            {
                fadeOutInfo.audioSource.Pause();
            }));
        }

        //���Ҫ���ŵ����ֵ�ǰ�����Ѿ����� ����ͣ�˾ͼ������ţ�
        AudioInfo audioInfo = bgmAudioInfoList.Find(x => x.audioName == fadeInMusicName);

        if (audioInfo != null)
        {
            s.Append(audioInfo.audioSource.DOFade(mainVolume * bgmVolumeFactor, fadeInDuration).OnComplete(() =>
            {
                audioInfo.audioSource.Play();
            }));

            return;
        }

        //�ҵ�Ҫ���ŵ���Ƶ����
        AudioData fadeInData = audioDatas.audioDataList.Find(x => x.audioName == fadeInMusicName);

        if (fadeInData == null)
        {
            Debug.LogWarning("��Ƶ����SO�в�������Ϊ" + fadeInMusicName + "����Ƶ����");
            return;
        }


        //����һ�����������AudioSource�����ò��Ų��� ���в���
        GameObject fadeInAudioGO = new GameObject(fadeInMusicName);

        fadeInAudioGO.transform.SetParent(_bgmSourcesRootGO.transform);

        AudioSource fadeInAudioSource = fadeInAudioGO.AddComponent<AudioSource>();

        fadeInAudioSource.clip = Resources.Load<AudioClip>("Audio/" + fadeInData.audioPath);

        fadeInAudioSource.loop = loop;

        fadeInAudioSource.Play();

        s.Append(fadeInAudioSource.DOFade(mainVolume * bgmVolumeFactor, fadeInDuration));

        AudioInfo info = new AudioInfo();

        //�Ѹ�bgm������Ϣ��ӵ��б�
        info.audioName = fadeInMusicName;

        info.audioSource = fadeInAudioSource;

        bgmAudioInfoList.Add(info);

        StartCoroutine(DetectingAudioPlayState(info, true));
    }


    /// <summary>
    /// ��ͣBgm
    /// </summary>
    /// <param name="pauseBgmName">��ͣ��������</param>
    /// <param name="fadeOutDuration">���ֵ�����ʱ��</param>
    public void PauseBgm(string pauseBgmName, float fadeOutDuration = 0.5f)
    {
        AudioInfo audioInfo = bgmAudioInfoList.Find(x => x.audioName == pauseBgmName);

        if (audioInfo == null)
        {
            Debug.LogWarning("��ǰ��δ����" + pauseBgmName + ",�޷���ͣ������");
            return;
        }

        Sequence s = DOTween.Sequence();

        s.Append(audioInfo.audioSource.DOFade(0, fadeOutDuration).OnComplete(() =>
        {
            audioInfo.audioSource.Pause();
        }));
    }



    /// <summary>
    /// ��ͣ����
    /// </summary>
    /// <param name="stopBgmName">��ͣ��������</param>
    /// <param name="fadeOutDuration">���ֵ�����ʱ��</param>
    public void StopBgm(string stopBgmName, float fadeOutDuration = 0.5f)
    {
        AudioInfo audioInfo = bgmAudioInfoList.Find(x => x.audioName == stopBgmName);

        if (audioInfo == null)
        {
            Debug.LogWarning("��ǰ��δ����" + stopBgmName + ",�޷�����������");
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
    /// ������Ч
    /// </summary>
    /// <param name="sfxName">���ŵ���Ч��</param>
    /// <param name="fadeInDuration">��Ч�����ʱ��</param>
    /// <param name="loop">�Ƿ�ѭ��</param>
    public void PlaySfx(string sfxName, float fadeInDuration = 0, bool loop = false)
    {
        Sequence s = DOTween.Sequence();

        //�ҵ�Ҫ���ŵ���Ч����
        AudioData sfxData = audioDatas.audioDataList.Find(x => x.audioName == sfxName);

        if (sfxData == null)
        {
            Debug.LogWarning("��Ƶ����SO�в�������Ϊ" + sfxName + "����Ƶ����");
            return;
        }


        //����һ�����������AudioSource�����ò��Ų��� ���в���
        GameObject sfxAudioGO = new GameObject(sfxName);

        sfxAudioGO.transform.SetParent(_sfxSourcesRootGO.transform);

        AudioSource sfxAudioSource = sfxAudioGO.AddComponent<AudioSource>();

        sfxAudioSource.clip = Resources.Load<AudioClip>("Audio/" + sfxData.audioPath);

        sfxAudioSource.loop = loop;

        sfxAudioSource.Play();

        s.Append(sfxAudioSource.DOFade(mainVolume * sfxVolumeFactor, fadeInDuration));

        AudioInfo info = new AudioInfo();

        //�Ѹ�bgm������Ϣ��ӵ��б�
        info.audioName = sfxName;

        info.audioSource = sfxAudioSource;

        sfxAudioInfoList.Add(info);

        StartCoroutine(DetectingAudioPlayState(info, false));
        //ThreadPool.QueueUserWorkItem(new WaitCallback(DetectingAudioPlayState), sfxAudioGO);//��������ӽ��̳߳أ����������
    }

    /// <summary>
    /// ��ͣ��Ч
    /// </summary>
    /// <param name="pauseSfxName">��ͣ����Ч��</param>
    public void PauseSfx(string pauseSfxName)
    {
        AudioInfo audioInfo = sfxAudioInfoList.Find(x => x.audioName == pauseSfxName);

        if (audioInfo == null)
        {
            Debug.LogWarning("��ǰ��δ����" + pauseSfxName + ",�޷���ͣ����Ч");
            return;
        }

        audioInfo.audioSource.Pause();
    }


    /// <summary>
    /// ֹͣ��Ч
    /// </summary>
    /// <param name="stopSfxName">ֹͣ����Ч��</param>
    public void StopSfx(string stopSfxName)
    {
        AudioInfo audioInfo = bgmAudioInfoList.Find(x => x.audioName == stopSfxName);

        if (audioInfo == null)
        {
            Debug.LogWarning("��ǰ��δ����" + stopSfxName + ",�޷���������Ч");
            return;
        }

        audioInfo.audioSource.Stop();

        bgmAudioInfoList.Remove(audioInfo);

        Destroy(audioInfo.audioSource.gameObject);
    }


    /// <summary>
    /// �ı�������
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
    /// �ı�Bgm��������
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
    /// �ı�Sfx��������
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
