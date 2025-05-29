using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using System.ComponentModel.Design;


public class MapManager : MonoBehaviour
{
	public StageSelectManager parentManager;

	[Header("マップのデータファイル")]
	public TextAsset MapFile;

	private string[] textData;
	private string[,] dungeonMap;

	private int textXNumber; // 行数に相当
	private int textYNumber; // 列数に相当

	bool centerPosRegister = false;//マップのセンターポジションがあるかないか

	[Header("マテリアル")]
	[SerializeField]
	private Material color_2;
	[SerializeField]
	private Material color_3;
	[SerializeField]
	private Material color_4;

	//入れるよう
	[SerializeField]
	private GameObject Floor = null;

	[Header("ゴールのオブジェクト")]
	[SerializeField]
	private GameObject Goal;
	private GameObject goalObject = null;
	[SerializeField]
	private GameObject centerObject;
	[SerializeField]
	private GameObject GameManager;

	//プレイヤーを設定
	[Header("プレイヤーのオブジェクト")]
	[SerializeField]
	private GameObject Player;

	private GameObject playerObject = null;

	//センターのポジション決めるための変数達
	private Vector3 startPos;
	private Vector3 endPos;
	private Vector3 center;

	//Floorのlist
	private List<GameObject> mapObjects;
	//チェックするときの通ったオブジェクトを格納する
	private List<Floor> checkedFloorList;

	//ここみんな気を付けて
	public static bool floorChange = false;

	//マップチェックTime
	private bool isOff = false;
	private float waitTime = 0.0f;

	//ゴールのたどり着くための変数
	private bool isMapClear;//マップをクリアしたかの判定
	private bool onGoal;//ゴールに行くプレイヤーのモーションをOn(動的にoffにはしない)
	private List<Vector3> playerRoot;

	//面の数
	private int faceNum = 0;

    //ゴールエフェクトオブジェクト(後藤が追加しました)
    [Header("ゴールエフェクト")]
    [SerializeField]
    private GameObject[] GoalEffect = new GameObject[4];
    //[SerializeField]
    //private GameObject goalEffect = null;
    //[SerializeField]
    //private GameObject playerRootEffect = null;

    [Header("共通データ")]
	//共通データ
	[SerializeField]
	private StageScriptableObject scriptableObject;

	[Header("ゴールのエフェクトを出す時間")]
	[SerializeField]
	private float effectCreateTime = 0.2f;

	[Header("クリアの待つ時間")]
	[SerializeField]
	private float clearwaitTime = 1.0f;

	private void Start()
	{
		//初期化
		mapObjects = new();
		checkedFloorList = new();
		floorChange = false;

        playerObject = null;
		isOff = false;
		isMapClear = false;
		onGoal = false;
        playerRoot = new();

        faceNum = 0;
		
        string textLines = MapFile.text; // テキストの全体データの代入
										 
		// 改行でデータを分割して配列に代入
		textData = textLines.Split('\n');

		// 行数と列数の取得
		textXNumber = textData[0].Split(',').Length;
		textYNumber = textData.Length;
		textYNumber -= 1;

		// ２次元配列の定義
		dungeonMap = new string[textYNumber, textXNumber];//マップ
		int state = 0;

		Debug.Log(scriptableObject.colorName);

        for (int i = 0; i < 1; i++)
        {
            string[] tempWords = textData[i].Split(',');
            for (int j = 1; j < 2; j++)
            {
                dungeonMap[i, j] = tempWords[j];

                faceNum = int.Parse(dungeonMap[i, j]);
            }
        }

		Debug.Log(faceNum);

        if (faceNum == 2)
		{
			Floor.GetComponent<MeshRenderer>().material = color_2;
			Debug.Log("2色です");
		}
		if (faceNum == 3)
		{
			Floor.GetComponent<MeshRenderer>().material = color_3;
			Debug.Log("3色です");
		}
		if (faceNum == 4)
		{
			Floor.GetComponent<MeshRenderer>().material = color_4;
			Debug.Log("4色");
		}

		for (int i = 1; i < textYNumber; i++)
		{
			string[] tempWords = textData[i].Split(',');

			for (int j = 0; j < textXNumber; j++)
			{
				dungeonMap[i, j] = tempWords[j];

				state = int.Parse(dungeonMap[i, j]);

				if (dungeonMap[i, j] != null)
				{
					switch (state)//スイッチ文ゴミ
					{
						case 0:

							break;
						case 1:
							GameObject floor1 = Instantiate(Floor, new Vector3(transform.position.x + j, transform.position.y, transform.position.z - i), Quaternion.identity) as GameObject;
                            floor1.GetComponent<Floor>().SetParentmap(this);
                            floor1.GetComponent<Floor>().SetMapPosition(j, i - 1, "red");
							floor1.transform.Rotate(180.0f, 0.0f, 0.0f);
							floor1.GetComponent<Floor>().SetFaceCount(1);
							mapObjects.Add(floor1);
							

							break;

						case 2:
							GameObject floor2 = Instantiate(Floor, new Vector3(transform.position.x + j, transform.position.y, transform.position.z - i), Quaternion.identity) as GameObject;
                            floor2.GetComponent<Floor>().SetParentmap(this);
                            floor2.GetComponent<Floor>().SetMapPosition(j, i - 1, "blue");
							floor2.transform.Rotate(270.0f, 0.0f, 0.0f);
							floor2.GetComponent<Floor>().SetFaceCount(2);
							mapObjects.Add(floor2);
							

							break;
						case 3:
							GameObject floor3 = Instantiate(Floor, new Vector3(transform.position.x + j, transform.position.y, transform.position.z - i), Quaternion.identity) as GameObject;
                            floor3.GetComponent<Floor>().SetParentmap(this);
                            floor3.GetComponent<Floor>().SetMapPosition(j, i - 1, "yellow");
							floor3.transform.Rotate(0.0f, 0.0f, 0.0f);
							floor3.GetComponent<Floor>().SetFaceCount(3);
							mapObjects.Add(floor3);
							

							break;
						case 4:
							GameObject floor4 = Instantiate(Floor, new Vector3(transform.position.x + j, transform.position.y, transform.position.z - i), Quaternion.identity) as GameObject;
                            floor4.GetComponent<Floor>().SetParentmap(this);
                            floor4.GetComponent<Floor>().SetMapPosition(j, i - 1, "green");
							floor4.transform.Rotate(90.0f, 0.0f, 0.0f);
							mapObjects.Add(floor4);
							

							break;
						case 5:
							GameObject floor5 = Instantiate(Floor, new Vector3(transform.position.x + j, transform.position.y, transform.position.z - i), Quaternion.identity) as GameObject;
                            floor5.GetComponent<Floor>().SetParentmap(this);
                            floor5.GetComponent<Floor>().SetMapPosition(j, i - 1, "goal");
                            if (scriptableObject.colorName == "red") floor5.transform.Rotate(180.0f, 0.0f, 0.0f);
                            else if (scriptableObject.colorName == "blue") floor5.transform.Rotate(270.0f, 0.0f, 0.0f);
                            else if (scriptableObject.colorName == "yellow") floor5.transform.Rotate(0.0f, 0.0f, 0.0f);
                            else if (scriptableObject.colorName == "green") floor5.transform.Rotate(90.0f, 0.0f, 0.0f);

                            mapObjects.Add(floor5);

							//ゴールオブジェクト(旗の生成)
							goalObject = Instantiate(Goal, new Vector3(transform.position.x + j, transform.position.y + 0.5f, transform.position.z - i), Quaternion.identity) as GameObject;
							goalObject.transform.rotation = Quaternion.Euler(-30.0f, 180.0f, 0.0f);

							break;
						case 6:
							GameObject floor6 = Instantiate(Floor, new Vector3(transform.position.x + j, transform.position.y, transform.position.z - i), Quaternion.identity) as GameObject;
                            floor6.GetComponent<Floor>().SetParentmap(this);
                            floor6.GetComponent<Floor>().SetMapPosition(j, i - 1, "player");
							if(scriptableObject.colorName == "red") floor6.transform.Rotate(180.0f, 0.0f, 0.0f);
                            else if (scriptableObject.colorName == "blue") floor6.transform.Rotate(270.0f, 0.0f, 0.0f);
                            else if (scriptableObject.colorName == "yellow") floor6.transform.Rotate(0.0f, 0.0f, 0.0f);
                            else if (scriptableObject.colorName == "green") floor6.transform.Rotate(90.0f, 0.0f, 0.0f);

                            mapObjects.Add(floor6);
							
							//プレイヤー生成
							playerObject = Instantiate(Player, new Vector3(transform.position.x + j, transform.position.y + 0.5f, transform.position.z - i), Quaternion.identity) as GameObject;
							playerObject.GetComponent<PlayerControl>().SetMapPosition(floor6.GetComponent<Floor>().GetMapPosition());

							break;
						default:
							break;
					}
				}
			}
		}

		if (centerPosRegister == false)//センターポスがなかったら登録する
		{
			startPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
			endPos = new Vector3(transform.position.x + (textXNumber - 1), 0.0f, transform.position.z - (textYNumber - 1));

			center = (startPos + endPos) / 2;

			GameObject centerObj = Instantiate(centerObject, new Vector3(center.x, transform.position.y, center.z), Quaternion.identity) as GameObject;

			centerPosRegister = true;//一つ登録すればOK
            Debug.Log("センターを登録");
            GameObject.Find("Main Camera").GetComponent<CameraControl>().CentrCretae(centerObj);//カメラのセンターになるオブジェクトを見つける

		}

        //プレイヤーがゴールを向くようにする
        GameObject goalObj = mapObjects.Find(match => match.gameObject.GetComponent<Floor>().GetFloorState() == "goal");

		Vector3 transformLookPos = goalObj.transform.position;
		transformLookPos.y += 1;

		playerObject.transform.LookAt(transformLookPos);
	}

	private void Update()
	{
		
        if (onGoal == true)
        {
			//操作はオフ画面遷移はオン
            GetComponent<CursorManager>().enabled = false;
            GetComponent<TouchControl>().enabled = false;
			GetComponent<ClickSceneControl>().enabled = true;
			GetComponent<TouchSceneControl>().enabled = true;

			//ゴールのポジションを少しずらす
			Vector3 goalPos = goalObject.transform.position;
            goalPos.x -= 0.2f;
            goalPos.z += 0.2f;
			
			goalObject.transform.position = goalPos;

            //ゴールの回転を元に戻す
            goalObject.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);

            //エフェクト用ゴールのルートを格納
            List<Floor> goalRootFloor = new();
            parentManager.isClear = true;
            Debug.Log(parentManager.isClear);
            //いらないルートを消す
            foreach (Floor floor in checkedFloorList)
            {
                if (floor.GetRootCount() == 0)
                {
                    floor.CheckOldRoot();
					Debug.Log("ルート消去");
                }
            }
            //プレイヤーのポジションからゴールのルートまでのrootのpositionを格納
            foreach (Floor floor in checkedFloorList)
            {
                if (floor.GetRootCount() == 1)
                {
                    goalRootFloor.Add(floor);
                    playerRoot.Add(floor.GetMapPosition());
                }
            }
            //かぶっているポジションデータがあったら消去
            for (int i = 0; i < playerRoot.Count; i++)
            {
                if (i != 0 && i != playerRoot.Count)
                {
                    if (playerRoot[i].x == playerRoot[i - 1].x && playerRoot[i].z == playerRoot[i - 1].z)
                    {
                        Debug.Log("かぶった");
                        playerRoot.RemoveAt(i);
                    }
                }
            }

            //ゴールのpositionも格納
            Floor goal = mapObjects.Find(match => match.GetComponent<Floor>().GetFloorState() == "goal").GetComponent<Floor>();
            checkedFloorList.Add(goal);
            playerRoot.Add(goal.GetMapPosition());

            float waitTime = effectCreateTime;

            //ゴールまでのルートにエフェクトを生成
            foreach (Floor floor in goalRootFloor)
            {
                //Debug.Log(floor.GetRootCount());
                //Debug.Log(floor.GetMapPosition());
                Vector3 pos = floor.transform.position;
                pos.y -= 1.0f;
                StartCoroutine(CreateGoalEffect(waitTime, pos));
                waitTime += 0.1f;
            }

            //プレイヤーが通るルートを格納
            playerObject.GetComponent<PlayerControl>().SetGoalRoot(playerRoot);
            //プレイヤーが動く処理を遅延
            WaitPlayerMove(clearwaitTime);

			// BGM停止
			BGMManager.instance.StopBGM();

            //一回はいればよくね？
            onGoal = false;
        }
        //マップのチェンジがオフになったらマップを変えれるようにする
        if (isOff == true)
		{
			if(waitTime >= 30.0f)
			{
				AllFloorWaitOff();
                //マップをクリアしていなかったら操作できる
                if (isMapClear == false)
				{
					floorChange = false;
                    GetComponent<CursorManager>().enabled = true;
                    GetComponent<TouchControl>().enabled = true;
                }
                
                //チェンジができるようにする
                Debug.Log("チェンジ可能");
                isOff = false;//DoOnce
            }
			waitTime++;
		}
	}

	public List<GameObject> GetGameObjectList()
	{

		return mapObjects;
	}
	public void AllFloorWaitOff()
	{
		for (int i = 0; i < mapObjects.Count; i++)
		{
			mapObjects[i].GetComponent<Floor>().SetChangeWait(false);
		}
	}
	public void ChangeMap(GameObject obj)//床しか入れん
	{

		checkedFloorList.Clear();
		string state = obj.GetComponent<Floor>().GetFloorState();
		int num = obj.GetComponent<Floor>().GetFaceCount() + 1;

        obj.GetComponent<Floor>().SetFloorState(state);
        obj.GetComponent<Floor>().SetFaceCount(num);
        //マップチェンジオン
        obj.GetComponent<Floor>().OnChange();

	}

	public void CheckMap()
	{

		GameObject player = mapObjects.Find(match => match.gameObject.GetComponent<Floor>().GetFloorState() == "player");

		checkedFloorList.Clear();

		checkedFloorList.Add(player.GetComponent<Floor>());
		player.GetComponent<Floor>().CheckFloor();

	}

	public void LinkChangeFloor(GameObject gameObject)
	{
		GameObject obj_top = null;
		GameObject obj_bottom = null;
		GameObject obj_left = null;
		GameObject obj_right = null;

		//↑
		obj_top = mapObjects.Find(match => match.GetComponent<Floor>().GetMapPosition().x == gameObject.GetComponent<Floor>().GetMapPosition().x && match.GetComponent<Floor>().GetMapPosition().z == gameObject.GetComponent<Floor>().GetMapPosition().z - 1);
		//↓
		obj_bottom = mapObjects.Find(match => match.GetComponent<Floor>().GetMapPosition().x == gameObject.GetComponent<Floor>().GetMapPosition().x && match.GetComponent<Floor>().GetMapPosition().z == gameObject.GetComponent<Floor>().GetMapPosition().z + 1);
		//←
		obj_left = mapObjects.Find(match => match.GetComponent<Floor>().GetMapPosition().x == gameObject.GetComponent<Floor>().GetMapPosition().x - 1 && match.GetComponent<Floor>().GetMapPosition().z == gameObject.GetComponent<Floor>().GetMapPosition().z);
		//→
		obj_right = mapObjects.Find(match => match.GetComponent<Floor>().GetMapPosition().x == gameObject.GetComponent<Floor>().GetMapPosition().x + 1 && match.GetComponent<Floor>().GetMapPosition().z == gameObject.GetComponent<Floor>().GetMapPosition().z);


		if (obj_top != null) if (obj_top.GetComponent<Floor>().GetFloorState() == "player"|| obj_top.GetComponent<Floor>().GetFloorState() == "goal") obj_top = null;
		if (obj_bottom != null) if (obj_bottom.GetComponent<Floor>().GetFloorState() == "player" || obj_bottom.GetComponent<Floor>().GetFloorState() == "goal") obj_bottom = null;
		if (obj_left != null) if (obj_left.GetComponent<Floor>().GetFloorState() == "player" || obj_left.GetComponent<Floor>().GetFloorState() == "goal") obj_left = null;
		if (obj_right != null) if (obj_right.GetComponent<Floor>().GetFloorState() == "player" || obj_right.GetComponent<Floor>().GetFloorState() == "goal") obj_right = null;



		if (obj_top != null)
		{
			
			obj_top.GetComponent<Floor>().SetFaceCount(obj_top.GetComponent<Floor>().GetFaceCount() + 1);
			obj_top.GetComponent<Floor>().SetFloorState(obj_top.GetComponent<Floor>().GetFloorState());
		}
		if (obj_bottom != null)
		{
			
			obj_bottom.GetComponent<Floor>().SetFaceCount(obj_bottom.GetComponent<Floor>().GetFaceCount() + 1);
			obj_bottom.GetComponent<Floor>().SetFloorState(obj_bottom.GetComponent<Floor>().GetFloorState());
		}
		if (obj_left != null)
		{
			
			obj_left.GetComponent<Floor>().SetFaceCount(obj_left.GetComponent<Floor>().GetFaceCount() + 1);
			obj_left.GetComponent<Floor>().SetFloorState(obj_left.GetComponent<Floor>().GetFloorState());
		}
		if (obj_right != null)
		{ 
			obj_right.GetComponent<Floor>().SetFaceCount(obj_right.GetComponent<Floor>().GetFaceCount() + 1);
			obj_right.GetComponent<Floor>().SetFloorState(obj_right.GetComponent<Floor>().GetFloorState());
		}

	}
    public void LinkChangeFloorMotion(GameObject gameObject)
    {
        GameObject obj_top = null;
        GameObject obj_bottom = null;
        GameObject obj_left = null;
        GameObject obj_right = null;

        //↑
        obj_top = mapObjects.Find(match => match.GetComponent<Floor>().GetMapPosition().x == gameObject.GetComponent<Floor>().GetMapPosition().x && match.GetComponent<Floor>().GetMapPosition().z == gameObject.GetComponent<Floor>().GetMapPosition().z - 1);
        //↓
        obj_bottom = mapObjects.Find(match => match.GetComponent<Floor>().GetMapPosition().x == gameObject.GetComponent<Floor>().GetMapPosition().x && match.GetComponent<Floor>().GetMapPosition().z == gameObject.GetComponent<Floor>().GetMapPosition().z + 1);
        //←
        obj_left = mapObjects.Find(match => match.GetComponent<Floor>().GetMapPosition().x == gameObject.GetComponent<Floor>().GetMapPosition().x - 1 && match.GetComponent<Floor>().GetMapPosition().z == gameObject.GetComponent<Floor>().GetMapPosition().z);
        //→
        obj_right = mapObjects.Find(match => match.GetComponent<Floor>().GetMapPosition().x == gameObject.GetComponent<Floor>().GetMapPosition().x + 1 && match.GetComponent<Floor>().GetMapPosition().z == gameObject.GetComponent<Floor>().GetMapPosition().z);


        if (obj_top != null) if (obj_top.GetComponent<Floor>().GetFloorState() == "player" || obj_top.GetComponent<Floor>().GetFloorState() == "goal") obj_top = null;
        if (obj_bottom != null) if (obj_bottom.GetComponent<Floor>().GetFloorState() == "player" || obj_bottom.GetComponent<Floor>().GetFloorState() == "goal") obj_bottom = null;
        if (obj_left != null) if (obj_left.GetComponent<Floor>().GetFloorState() == "player" || obj_left.GetComponent<Floor>().GetFloorState() == "goal") obj_left = null;
        if (obj_right != null) if (obj_right.GetComponent<Floor>().GetFloorState() == "player" || obj_right.GetComponent<Floor>().GetFloorState() == "goal") obj_right = null;



        if (obj_top != null)
        {
			obj_top.GetComponent<Floor>().OnLinkChange();
        }
        if (obj_bottom != null)
        {
            obj_bottom.GetComponent<Floor>().OnLinkChange();
        }
        if (obj_left != null)
        {
			obj_left.GetComponent<Floor>().OnLinkChange();
        }
        if (obj_right != null)
        {
            obj_right.GetComponent<Floor>().OnLinkChange();
        }

    }

    public List<Floor> GetCheckedFloorList()
	{
		return checkedFloorList;
	}
　　
	public void IsMapClear()
	{
		isMapClear = true;
	}
	void OnGoal()
	{
		onGoal = true;
	}
	//ゴールした判定を遅延
	public void WaitOnGoal()
	{
		//この処理の仕方だとマップのサイズ増やしたらバグります。
		Invoke(nameof(OnGoal), 1.0f);
	}
    private void PlayerMove()
    {
        Debug.Log("プレイヤー動きます");
        playerObject.GetComponent<PlayerControl>().OnPlayerMove();
    }
    public void WaitPlayerMove(float waitTime)
	{
		
		Invoke(nameof(PlayerMove), waitTime);

    }
    public int GetFaceNum()
	{
		return faceNum;
	}
	public int GetMapSize()
	{
		return textXNumber;
	}
	public void ChangeOff()
	{
		isOff = true;
	}

	
	//エフェクト生成
	private IEnumerator CreateGoalEffect(float waitTime, Vector3 pos)
	{
		yield return new WaitForSeconds(waitTime);
        //ブロックに被ってしまうので少しposを上げる
        pos.y += 1.0f;
        //ゴールカラーによってエフェクトを変更
        switch (scriptableObject.colorName)
        {
            //赤なら配列0番目を
            case "red":
				Instantiate(GoalEffect[0], pos, Quaternion.identity);
                break;
            //青なら配列1番目を
            case "blue":
                Instantiate(GoalEffect[1], pos, Quaternion.identity);
                break;
            //黄色なら配列2番目を
            case "yellow":
                Instantiate(GoalEffect[2], pos, Quaternion.identity);
                break;
            //緑なら配列3番目を
            case "green":
                Instantiate(GoalEffect[3], pos, Quaternion.identity);
                break;
            default:
                Debug.Log("ゴールエフェクトエラー");
                break;
        }

        //Instantiate(goalEffect,pos,Quaternion.identity);
        //pos.y -= 1.0f;
        //Instantiate(playerRootEffect, pos, Quaternion.identity);
        SEManager.Instance.PlaySE("Block_Completion");

    }
	//マップのリセット
    public void MapReset()
	{

		SEManager.Instance.PlaySE("Retry");

		foreach(GameObject obj in mapObjects)
		{
			Destroy(obj);
		}

		mapObjects.RemoveAll(match => match != null);

		int state = 0;

        for (int i = 1; i < textYNumber; i++)
        {
            string[] tempWords = textData[i].Split(',');

            for (int j = 0; j < textXNumber; j++)
            {
                dungeonMap[i, j] = tempWords[j];

                state = int.Parse(dungeonMap[i, j]);

                if (dungeonMap[i, j] != null)
                {
                    switch (state)//スイッチ文ゴミ
                    {
                        case 0:

                            break;
                        case 1:
                            GameObject floor1 = Instantiate(Floor, new Vector3(transform.position.x + j, transform.position.y, transform.position.z - i), Quaternion.identity) as GameObject;
                            floor1.GetComponent<Floor>().SetParentmap(this);
                            floor1.GetComponent<Floor>().SetMapPosition(j, i - 1, "red");
                            floor1.transform.Rotate(180.0f, 0.0f, 0.0f);
                            floor1.GetComponent<Floor>().SetFaceCount(1);
                            mapObjects.Add(floor1);


                            break;

                        case 2:
                            GameObject floor2 = Instantiate(Floor, new Vector3(transform.position.x + j, transform.position.y, transform.position.z - i), Quaternion.identity) as GameObject;
                            floor2.GetComponent<Floor>().SetParentmap(this);
                            floor2.GetComponent<Floor>().SetMapPosition(j, i - 1, "blue");
                            floor2.transform.Rotate(270.0f, 0.0f, 0.0f);
                            floor2.GetComponent<Floor>().SetFaceCount(2);
                            mapObjects.Add(floor2);


                            break;
                        case 3:
                            GameObject floor3 = Instantiate(Floor, new Vector3(transform.position.x + j, transform.position.y, transform.position.z - i), Quaternion.identity) as GameObject;
                            floor3.GetComponent<Floor>().SetParentmap(this);
                            floor3.GetComponent<Floor>().SetMapPosition(j, i - 1, "yellow");
                            floor3.transform.Rotate(0.0f, 0.0f, 0.0f);
                            floor3.GetComponent<Floor>().SetFaceCount(3);
                            mapObjects.Add(floor3);


                            break;
                        case 4:
                            GameObject floor4 = Instantiate(Floor, new Vector3(transform.position.x + j, transform.position.y, transform.position.z - i), Quaternion.identity) as GameObject;
                            floor4.GetComponent<Floor>().SetParentmap(this);
                            floor4.GetComponent<Floor>().SetMapPosition(j, i - 1, "green");
                            floor4.transform.Rotate(90.0f, 0.0f, 0.0f);
                            mapObjects.Add(floor4);


                            break;
						case 5:
							GameObject floor5 = Instantiate(Floor, new Vector3(transform.position.x + j, transform.position.y, transform.position.z - i), Quaternion.identity) as GameObject;
							floor5.GetComponent<Floor>().SetParentmap(this);
							floor5.GetComponent<Floor>().SetMapPosition(j, i - 1, "goal");
                            if (scriptableObject.colorName == "red") floor5.transform.Rotate(180.0f, 0.0f, 0.0f);
                            else if (scriptableObject.colorName == "blue") floor5.transform.Rotate(270.0f, 0.0f, 0.0f);
                            else if (scriptableObject.colorName == "yellow") floor5.transform.Rotate(0.0f, 0.0f, 0.0f);
                            else if (scriptableObject.colorName == "green") floor5.transform.Rotate(90.0f, 0.0f, 0.0f);
                            mapObjects.Add(floor5);


							break;
						case 6:
							GameObject floor6 = Instantiate(Floor, new Vector3(transform.position.x + j, transform.position.y, transform.position.z - i), Quaternion.identity) as GameObject;
							floor6.GetComponent<Floor>().SetParentmap(this);
							floor6.GetComponent<Floor>().SetMapPosition(j, i - 1, "player");
							if (scriptableObject.colorName == "red") floor6.transform.Rotate(180.0f, 0.0f, 0.0f);
							else if (scriptableObject.colorName == "blue") floor6.transform.Rotate(270.0f, 0.0f, 0.0f);
							else if (scriptableObject.colorName == "yellow") floor6.transform.Rotate(0.0f, 0.0f, 0.0f);
							else if (scriptableObject.colorName == "green") floor6.transform.Rotate(90.0f, 0.0f, 0.0f);

							mapObjects.Add(floor6);

							break;
						default:
                            break;
                    }
                }
            }
        }

    }
}
