using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TutorialBackScene : MonoBehaviour
{
    [Header("共通データ")]
    [SerializeField]
    private StageScriptableObject scriptableObject;
    [Header("フェード")]
    [SerializeField] private Image fade = default;

    public void BackScene()
    {
        if(scriptableObject.oldSceneName == "TitleScene")
        {
            fade.GetComponent<FadeINOUT>().FadeToChangeScene(0);
        }
        else if (scriptableObject.oldSceneName == "StageSelect")
        {
            fade.GetComponent<FadeINOUT>().FadeToChangeScene(1);
        }
    }

}
