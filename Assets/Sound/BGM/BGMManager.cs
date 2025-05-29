using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;

    [SerializeField] private StageScriptableObject scriptableObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public AudioSource titleBGM;
    public AudioSource tutorialBGM;
    public AudioSource stageSelectBGM;
    public AudioSource clearBGM;
    public AudioSource easyBGM;
    public AudioSource mediumBGM;
    public AudioSource hardBGM;


    private AudioSource currentBGM;
    private string previousScene;

    private void Start()
    {
        previousScene = SceneManager.GetActiveScene().name;
        PlayBGMForScene(previousScene);

        // シーン変更時のイベントを登録
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
    {
        PlayBGMForScene(nextScene.name);
    }

    private void PlayBGMForScene(string sceneName)
    {
        // 既存のBGMを停止
        if (currentBGM != null)
        {
            currentBGM.Stop();
        }

        // シーンに基づくBGM切り替え
        if (sceneName == "TitleScene")
        {
            currentBGM = titleBGM;
        }
        else if (sceneName == "TutorialScene")
        {
            currentBGM = tutorialBGM;
        }

        else if (sceneName == "StageSelect")
        {
            currentBGM = stageSelectBGM;
        }
        else if (sceneName == "SampleScene")
        {
            // 難易度に基づくBGM選択
            switch (scriptableObject.DifficultyIndex)
            {
                case 1:
                    currentBGM = easyBGM;
                    break;
                case 2:
                    currentBGM = mediumBGM;
                    break;
                case 3:
                    currentBGM = hardBGM;
                    break;
                default:
                    Debug.LogWarning("無効な難易度インデックス");
                    return;
            }
        }

        // 選択したBGMを再生
        if (currentBGM != null)
        {
            currentBGM.Play();
        }

        // 現在のシーン名を保存
        previousScene = sceneName;
    }

    public void PlayBGM(string bgmName)
    {
        
        Debug.Log(clearBGM.name);

        if(bgmName == "TitleBGM")
        {
            currentBGM = titleBGM;
        }
        else if(bgmName == "StageSelect")
        {
            currentBGM = stageSelectBGM;
        }
        else if(bgmName == "Clear")
        {
            currentBGM = clearBGM;
        }
        else if (bgmName == "Morning")
        {
            currentBGM = easyBGM;
        }
        else if (bgmName == "Noon")
        {
            currentBGM = mediumBGM;
        }
        else if (bgmName == "Evening")
        {
            currentBGM = hardBGM;
        }

        currentBGM.Play();

    }

    public void StopBGM()
    {
        currentBGM.Stop();
    }


}
