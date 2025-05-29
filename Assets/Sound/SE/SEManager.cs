using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SEManager : MonoBehaviour
{
    public static SEManager Instance { get; private set; }

    private AudioSource audioSource;
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    private bool isPlaying; // 再生中かどうかを示すフラグ

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetAudioMixerGroup(AudioMixerGroup mixerGroup)
    {
        if (audioSource != null)
        {
            audioSource.outputAudioMixerGroup = mixerGroup;
        }
    }

    public void AddAudioClip(string clipName, AudioClip clip)
    {
        if (!audioClips.ContainsKey(clipName))
        {
            audioClips.Add(clipName, clip);
        }
    }

    public void PlaySE(string clipName)
    {
        if (audioClips.TryGetValue(clipName, out AudioClip clip))
        {
            //audioSource.PlayOneShot(clip); // 常に再生

            // フラグを設定し、コルーチンを開始
            if (clipName == "ColorChange")
            {
                Debug.Log(isPlaying);
                if (!isPlaying) // 再生中でない場合のみフラグを設定
                {
                    Debug.Log("再生します");
                    isPlaying = true; // 再生中フラグをセット
                    audioSource.PlayOneShot(clip); // 常に再生

                    StartCoroutine(ResetPlayingFlag(clip.length)); // コルーチンを開始
                }
            }
            else
            {
                audioSource.PlayOneShot(clip); // 常に再生
            }
        }
        else
        {
            Debug.LogWarning($"SEManager: Clip '{clipName}' not found!");
        }
    }

    private IEnumerator ResetPlayingFlag(float duration)
    {
        yield return new WaitForSeconds(duration); // クリップの長さだけ待機
        isPlaying = false; // 再生中フラグをリセット
    }
}
