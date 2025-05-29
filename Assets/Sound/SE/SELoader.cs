using UnityEngine;
using UnityEngine.Audio;

public class AudioLoader : MonoBehaviour
{
    [SerializeField]
    private AudioClip selectClip; // "Select"の音声クリップ
    [SerializeField]
    private AudioClip colorChangeClip;
    [SerializeField]
    private AudioClip BlockFitClip;
    [SerializeField]
    private AudioClip BlockCompletionClip;
    [SerializeField]
    private AudioClip CongratulationClip;
    [SerializeField]
    private AudioClip PlayerwalkClip;
    [SerializeField]
    private AudioClip RetryClip;
    [SerializeField]
    private AudioMixerGroup audioMixerGroup; // 出力先のAudioMixerGroup


    private void Start()
    {
        SEManager.Instance.SetAudioMixerGroup(audioMixerGroup); // AudioSourceの出力先を指定

        // SEManagerに音声クリップを追加
        SEManager.Instance.AddAudioClip("Select", selectClip);
        SEManager.Instance.AddAudioClip("ColorChange", colorChangeClip);
        SEManager.Instance.AddAudioClip("Block_Fit", BlockFitClip);
        SEManager.Instance.AddAudioClip("Block_Completion", BlockCompletionClip);
        SEManager.Instance.AddAudioClip("Congratulation", CongratulationClip);
        SEManager.Instance.AddAudioClip("Player_walk", PlayerwalkClip);
        SEManager.Instance.AddAudioClip("Retry", RetryClip);
    }
}
