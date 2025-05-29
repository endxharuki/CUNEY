using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{

    private Vector3 position;//このFloorの2次元配列のポジション
    //プレイヤーが動く時の速さ
    [Header("プレイヤーの動く時の速さ")]
    [SerializeField]
    private float playerMoveTime = 0.0f;
    [Header("滑らかに動く度合")]
    private float moveFrame = 0;

    private int moveCount = 0;
    //プレイヤーのゴールまでのルートを格納するlist
    private List<Vector3> goalRoot;
    private bool onGoalMove = false;
    private float currentTime = 0.0f;

    //プレイヤーがステージをクリアした
    private bool playerClear = false;

    void Start()
    {
        //初期化
        moveCount = 0;
        goalRoot = new();
        onGoalMove = false;
        currentTime = 0.0f;
        playerClear = false;
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーがゴールまでいくモーション
        if (onGoalMove == true)
        {
            if (currentTime >= playerMoveTime * 0.1f)
            {
                MovePlayer();
                currentTime = 0.0f;
            }
            currentTime += Time.deltaTime;
        }

    }

    private void MovePlayer()
    {

        if(moveCount == goalRoot.Count)
        {
            onGoalMove = false;
            playerClear = true;
            transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            //クリア時のカメラ演出
            GameObject.Find("Main Camera").GetComponent<CameraControl>().OnCameraMove();
        }
        else if (moveCount != goalRoot.Count)
        {
            
            if (goalRoot[moveCount].x > position.x)
            {
                Vector3 transformPos = transform.position;
                transformPos.x += playerMoveTime;
                transform.position = transformPos;
                transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
               
            }
            else if (goalRoot[moveCount].x < position.x)
            {
                Vector3 transformPos = transform.position;
                transformPos.x -= playerMoveTime;
                transform.position = transformPos;
                transform.rotation = Quaternion.Euler(0.0f, 270.0f, 0.0f);
            }
            else if (goalRoot[moveCount].z > position.z)
            {
                Vector3 transformPos = transform.position;
                transformPos.z -= playerMoveTime;
                transform.position = transformPos;
                transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            }
            else if (goalRoot[moveCount].z < position.z)
            {
                Vector3 transformPos = transform.position;
                transformPos.z += playerMoveTime;
                transform.position = transformPos;
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }
            moveFrame += playerMoveTime;
           
            if (moveFrame >= playerMoveTime / playerMoveTime)
            {
                SEManager.Instance.PlaySE("Player_walk");
                position = goalRoot[moveCount];

                moveCount++;
                moveFrame = 0.0f;
            }
            //この下には処理書かない
        }
        
       
    }

    public void SetGoalRoot(List<Vector3> root)
    {
        goalRoot = root;
        
    }
    public void OnPlayerMove()
    {
        onGoalMove = true;
    }
    public void SetMapPosition(Vector3 pos)
    {
        position.x = pos.x;
        position.z = pos.z;

    }
    public bool GetClear()
    {
        return playerClear;
    }

    public bool GetOnPlayerMove()
    {
        return onGoalMove;
    }
}
