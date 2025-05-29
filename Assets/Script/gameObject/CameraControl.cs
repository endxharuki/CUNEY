using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class CameraControl : MonoBehaviour
{
    
    private GameObject centerObject;  //カメラの中心のオブジェクト
    private Vector3 lookPosition;
    [SerializeField]
    private float playerLength = 0.0f;
    private float Hight = 0.0f;
    [SerializeField]
    private float cameraZpos = 0.0f;
    [SerializeField]
    private float sideLength = 0.0f;
    private bool center = false;
    private bool clearCameraMove = false;
    private bool isResult = false;

    // 補完スピードを決める
    [SerializeField]
    private int cameraMotionCount = 50;
    [SerializeField]
    private float rotationSpeed = 0.01f;
    private float moveSpeed = 0.0f;

    //Timeの計算用変数
    private float currentTime = 0.0f;
    private int motionCount = 0;
    void Start()
    {
        
        center = false;
        clearCameraMove = false;
        isResult = false;
        currentTime = 0.0f;
        motionCount = 0;

    }
    void Update()
    {

        if (GameObject.Find("map(Clone)").GetComponent<MapManager>().GetMapSize() == 5) moveSpeed = 0.20f;
        if (GameObject.Find("map(Clone)").GetComponent<MapManager>().GetMapSize() == 6) moveSpeed = 0.2f;
        if (GameObject.Find("map(Clone)").GetComponent<MapManager>().GetMapSize() == 7) moveSpeed = 0.2f;
        if (GameObject.Find("map(Clone)").GetComponent<MapManager>().GetMapSize() == 8) moveSpeed = 0.2f;

        if (centerObject != null)
        {
            //transform.LookAt(centerObject.transform);//カメラターゲットを登録したオブジェクトにしてる
            if (center == false)
            {
                Debug.Log("センターオブジェクトを見る");
                lookPosition = centerObject.transform.position;
                float x = lookPosition.x;
                Vector3 pos = lookPosition;
                pos.y = Hight;
                pos.z = lookPosition.z - (sideLength + cameraZpos);//マジークナンバーごめんなさい
                transform.position = pos;
                center = true;
               
            }


            //プレイヤーがゴールしたら
            if (GameObject.Find("Player(Clone)").GetComponent<PlayerControl>().GetClear() == true)
            {

                Vector3 relativePos = lookPosition - transform.position;
                // 方向を、回転情報に変換
                Quaternion rotation = Quaternion.LookRotation(relativePos);
                // 現在の回転情報と、ターゲット方向の回転情報を補完する
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, moveSpeed);

            }//最初の処理
            else
            {
                // ターゲット方向のベクトルを取得
                Vector3 lookpos = lookPosition;
                lookpos.z += cameraZpos;

                Vector3 relativePos = lookpos - transform.position;
                // 方向を、回転情報に変換
                Quaternion rotation = Quaternion.LookRotation(relativePos);
                // 現在の回転情報と、ターゲット方向の回転情報を補完する
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, moveSpeed);
            }
        }
        if (clearCameraMove == true)
        {
            MoveCamera();
        }
       
    }
    public void CentrCretae(GameObject center)
    {
        centerObject = center;//カメラのセンターになるオブジェクトを見つける
        
        if (GameObject.Find("map(Clone)").GetComponent<MapManager>().GetMapSize() == 5) Hight = 9.5f;
        if (GameObject.Find("map(Clone)").GetComponent<MapManager>().GetMapSize() == 6) Hight = 11.0f;
        if (GameObject.Find("map(Clone)").GetComponent<MapManager>().GetMapSize() == 7) Hight = 12.5f;
        if (GameObject.Find("map(Clone)").GetComponent<MapManager>().GetMapSize() == 8) Hight = 14.0f;

        

    }
   
    private void rotateCamera()
    {
        Vector3 angle = new Vector3(
                Input.GetAxis("Mouse X") * (moveSpeed * 10.0f),
                0,
                0
        );
       transform.RotateAround(centerObject.transform.position, Vector3.up, angle.x);
    }
    private void MoveCamera()
    {

        if (currentTime >= 0.02f)
        {
            currentTime = 0.0f;
            motionCount++;
        }
        if (motionCount < cameraMotionCount && motionCount >= 0)
        {
            transform.RotateAround(centerObject.transform.position, Vector3.right, -rotationSpeed);
            Vector3 pos = transform.position;
            pos.z -= moveSpeed * 0.2f;//マジックナンバーごめん
            transform.position = pos;
        }
        else if (motionCount < cameraMotionCount * 2 && motionCount >= cameraMotionCount)
        {
            centerObject = GameObject.Find("map(Clone)").GetComponent<MapManager>().GetGameObjectList().Find(match => match.GetComponent<Floor>().GetFloorState() == "goal");
            lookPosition = centerObject.transform.position;

            //見るポジションを取得
            Vector3 pos = lookPosition;
            pos.y = 2.0f;
            lookPosition = pos;

            pos.z = lookPosition.z - playerLength;//マジックナンバーごめん

            transform.position = Vector3.MoveTowards(transform.position, pos, moveSpeed);
        }
        else if (motionCount >= cameraMotionCount * 2)
        {
            //シーンを切り替え
            clearCameraMove = false;
            currentTime = 0.0f;
            motionCount = 0;
            isResult = true;
            Debug.Log("リザルトはいりました");
           
        }

        currentTime += Time.deltaTime;

    }
    public void OnCameraMove()
    {
        clearCameraMove = true;
    }
    public bool GetIsResult()
    {
        return isResult;
    }
}
