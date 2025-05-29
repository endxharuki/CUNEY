using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ボタンを使用するためUIとSceneManage追加
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // シーン切り替え
    public void ChangeSceneToStageSelect()
    {
        SceneManager.LoadScene("StageSelect");
    }
    public void ChangeSceneToGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
