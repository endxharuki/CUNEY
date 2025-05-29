using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{

    //共通データ
    [SerializeField]
    private StageScriptableObject scriptableObject;
   
    [SerializeField]
    Texture2D defaultCursor = null;
    Camera mainCamera;
    RaycastHit hit;
    GameObject targetObject;

    public bool CursorOn = false; 

    void Start()
    {
        enabled = true;
        //初期化
        
        mainCamera = Camera.main;
        SetCursor(true);
    }
    void Update()
    {
        if (CursorOn == true)
        {
            //カーソル処理
            CastRay();
            //チューリアルの時のカーソル処理
            TutorialCastRay();
        }
    }
    // マウスカーソルの位置から「レイ」を飛ばして、何かのコライダーに当たるかどうかをチェック
    void CastRay()
    {
        if (scriptableObject.tutorialClear == false) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
        {

            targetObject = hit.collider.gameObject;

            if (hit.collider.gameObject.CompareTag("Floor"))
            {
                if (targetObject.GetComponent<Floor>().GetFloorState() != "player" && targetObject.GetComponent<Floor>().GetFloorState() != "goal")
                {
                    if (targetObject.GetComponent<Floor>().GetChangeState() == false && targetObject.GetComponent<Floor>().GetLinkChangeState() == false)
                    {
                        if (MapManager.floorChange == false)
                        {
                            //カーソルが当たっているのをオブジェクトに伝える
                            targetObject.GetComponent<Floor>().OnCursor();

                            if (Input.GetMouseButtonDown(0))
                            {

                                Debug.Log("押しました");
                                MapManager.floorChange = true;
                                GetComponent<MapManager>().ChangeMap(targetObject);
                                enabled = false;

                            }
                        }
                    }
                }
            }
        }
    }
    void TutorialCastRay()
    {
        if (scriptableObject.tutorialClear == true) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
        {

            targetObject = hit.collider.gameObject;

            if (hit.collider.gameObject.CompareTag("Floor"))
            {
                if (targetObject.GetComponent<Floor>().GetFloorState() != "player")
                {
                    if (MapManager.floorChange == false)
                    {
                        //カーソルが当たっているのをオブジェクトに伝える
                        targetObject.GetComponent<Floor>().OnCursor();

                        if (Input.GetMouseButtonDown(0))
                        {
                            if (targetObject.GetComponent<Floor>().GetMapPosition().x == 1 && targetObject.GetComponent<Floor>().GetMapPosition().z == 1)//触っていいオブジェクト
                            {
                                if (scriptableObject.textIndex == 1)
                                {
                                    Debug.Log("押しました");
                                    GetComponent<MapManager>().ChangeMap(targetObject);
                                    enabled = false;
                                    scriptableObject.textIndex++;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    
    // 対象のオブジェクトを調べる処
    public void SetCursor(bool isDefault)
    {
        if (isDefault == true)
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }
    }
    
}
