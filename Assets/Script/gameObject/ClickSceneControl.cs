using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSceneControl : MonoBehaviour
{
    //共通データ
    [SerializeField]
    private StageScriptableObject scriptableObject;
    Camera mainCamera;

    private bool result = false;
    void Start()
    {
        enabled = false;
        result = false;
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //シーン切り替え処理
        if (result == true)
        {
            //Debug.Log("シーン変える");
            ClickChangeScene();
        }
        else if (mainCamera.GetComponent<CameraControl>().GetIsResult() == true)
        {
            Invoke(nameof(OnResultClick), 2.0f);
        }
    }

    void ClickChangeScene()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject.Find("StageManager").GetComponent<StageSelectManager>().ChangeScene();
        }
    }

    void OnResultClick()
    {
        result = true;
    }

}
