using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchSceneControl : MonoBehaviour
{
    //共通データ
    [SerializeField]
    private StageScriptableObject scriptableObject;
    //カメラ
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
        //シーンの切り替え処理
        if (result == true)
        {
            TouchChangeScene();
        }
        else if (mainCamera.GetComponent<CameraControl>().GetIsResult() == true)
        {
            Invoke(nameof(OnResultTouch), 2.0f);
        }
    }

    void TouchChangeScene()
    {
        if (Input.touchCount <= 0) return;
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            GameObject.Find("StageManager").GetComponent<StageSelectManager>().ChangeScene();
        }
    }
    // 対象のオブジェクトを調べる処理
    void OnResultTouch()
    {
        result = true;
    }
}
