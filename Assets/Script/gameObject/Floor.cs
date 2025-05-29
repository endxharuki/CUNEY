using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class Floor : MonoBehaviour
{
    
    private GameObject thisGameObject = null;
	//マウスの処理のOnOff
	[SerializeField]
	private bool Mouse = false;
	//タッチ操作のOnOff
    [SerializeField]
    private bool Tap = false;

	//Floorの変数
	private Vector3 position;//このFloorの2次元配列のポジション
	private string state;//２次元配列で登録されている文字ナンバー
	private MapManager parentMap = null;
	private Floor oldFloor = null;//このFloorのチェックに入る前の情報
	private int rootCount = 0;//このFloorからの分岐の数

	//ChangeFloorの変数
	[SerializeField]
	private float motionFrame = 0.1f;
	private int motionCount = 0;
	private float currentTime = 0.0f;
    [Header("チェンジモーションの速さ")]
    //モーションの速さ
    [SerializeField]
	private int changeMotionCount = 10;

	private bool changeWait = false;
	private bool change = false; //床が変わる
	private float currentLinkTime = 0.0f;
	private int  motionLinkCount = 0;
	private bool linkChange = false;

	[SerializeField]
	private float Hight;
	//カーソルが当たっているか
	private bool cursor = false;
	//色の数
	private int  faceCount = 1;
	//色情報を変える
	private bool doOnec = true;

	//選んでるときは色を発行させる
	[SerializeField]
	private float blockPlayerEmissive = 0.0f;
	[SerializeField]
	private float blockEmissive = 0.0f;

	[Header("共有オブジェクト")][SerializeField] private StageScriptableObject scriptableObject;

	[Header("エフェクト")]
	[SerializeField]
	private GameObject effectObject = null;
	private GameObject effect = null;
	[SerializeField]
	private GameObject linkEffectObject = null;

	// Start is called before the first frame update
	void Start()
	{
		//初期化
        oldFloor = null;
		rootCount = 0;
        motionCount = 0;
        currentTime = 0.0f;
        changeWait = false;
        change = false; //床が変わる
        currentLinkTime = 0.0f;
        motionLinkCount = 0;
        linkChange = false;

        cursor = false;
        doOnec = true;

        thisGameObject = parentMap.GetGameObjectList().Find(match => match == gameObject);
    }

	// Update is called once per frame
	void Update()
	{

		
		if (Tap == true) TouchUpdate();
		if (Mouse == true) CursorUpdate();

        ChangeFloor();
        LinkChangeFloor();

	}

	
	public Vector3 GetMapPosition()
	{
		return position;
	}

	public string GetFloorState()
	{
		return state;
	}

	public void SetParentmap(MapManager map)
	{
		parentMap = map;
	}
	public int GetRootCount()
	{
		return rootCount;
	}

	public void SetOldFloor(Floor floor)
	{
		oldFloor = floor;
	}
	public void SetFaceCount(int count)
	{
		faceCount = count;
	}
	public int GetFaceCount()
	{
		return faceCount;
	}
    public void OnChange()
    {
        change = true;
        currentTime = 0.0f;

    }
    public void OnLinkChange()
    {
        linkChange = true;
        currentLinkTime = 0.0f;
    }
    public void OnCursor()
    {
        cursor = true;
    }

    public bool GetChangeState()
    {
        return change;
    }
    public bool GetLinkChangeState()
    {
        return linkChange;
    }
    public void SetChangeWait(bool value)
    {
        changeWait = value;
    }
    public bool GetChangeWait()
    {
        return changeWait;
    }
    public void SetMapPosition(float posX, float posZ, string num)
    {

        position.x = posX;
        position.z = posZ;
        state = num;

        //光度変更
        if (state == "player") GetComponent<MeshRenderer>().material.color = Color.white * blockPlayerEmissive;
        else if (state == "goal") GetComponent<MeshRenderer>().material.color = Color.white;
        else if (state == scriptableObject.colorName) GetComponent<MeshRenderer>().material.color = Color.white * blockPlayerEmissive;
        else GetComponent<MeshRenderer>().material.color = Color.white * blockEmissive;

    }
    public void SetFloorState(string num)
    {

        if (parentMap.GetFaceNum() == 2)//二色の場合
        {
            if (num == "red")
            {
                state = "blue";
            }
            else if (num == "blue")
            {
                state = "red";
            }
        }
        if (parentMap.GetFaceNum() == 3)//三色の場合
        {
            if (num == "red")
            {
                state = "blue";
            }
            else if (num == "blue")
            {
                state = "yellow";
            }
            else if (num == "yellow")
            {
                state = "red";
            }
        }
        if (parentMap.GetFaceNum() == 4)//四色の場合
        {
            if (num == "red")
            {
                state = "blue";
            }
            else if (num == "blue")
            {
                state = "yellow";
            }
            else if (num == "yellow")
            {
                state = "green";
            }
            else if (num == "green")
            {
                state = "red";
            }
        }

    }

    private void ChangeFloor()
    {
        if (change == true)//床の色を変えるモーション
        {
            //一回しか入らないようにする
            if (doOnec == true)
            {
                //周辺の色情報を変更
                parentMap.LinkChangeFloor(thisGameObject);
                //プレイヤーが選択した位置に向く
                Vector3 lookPos = transform.position;
                lookPos.y += 0.1f;
                GameObject.Find("Player(Clone)").transform.LookAt(lookPos);

                doOnec = false;
            }
            currentTime += Time.deltaTime;

            if (motionCount < changeMotionCount * 3)
            {
                if (currentTime >= motionFrame)
                {
                    //2面と4面の時は通常
                    if (parentMap.GetFaceNum() == 2 || parentMap.GetFaceNum() == 4)
                    {
                        if (motionCount < changeMotionCount)
                        {
                            SEManager.Instance.PlaySE("ColorChange");
                            Vector3 transformPos = transform.position;
                            transformPos.y += 0.1f;
                            transform.position = transformPos;
                            //transform.Rotate((90.0f * 1.0f / changeMotionCount) / 2, 0.0f, 0.0f);
                        }
                        else if (motionCount < changeMotionCount * 2)
                        {
                            SEManager.Instance.PlaySE("ColorChange");
                            transform.Rotate(90.0f * 1.0f / changeMotionCount, 0.0f, 0.0f);

                        }
                        else if (motionCount < changeMotionCount * 3)
                        {
                            Vector3 transformPos = transform.position;
                            transformPos.y -= 0.1f;
                            transform.position = transformPos;
                            //transform.Rotate((90.0f * 1.0f / changeMotionCount) / 2, 0.0f, 0.0f);
                        }
                        motionCount++;
                        currentTime = 0.0f;
                    }
                    //3面の時
                    else if (parentMap.GetFaceNum() == 3)
                    {
                        Debug.Log("3色");
                        if (motionCount < changeMotionCount)
                        {
                            Vector3 transformPos = transform.position;
                            transformPos.y += 0.1f;
                            transform.position = transformPos;

                            //if (faceCount >= 4)
                            //{
                            //    Debug.Log("z軸");
                            //    SEManager.Instance.PlaySE("ColorChange");
                            //    transform.Rotate(0.0f, 0.0f, (90.0f * 1.0f / changeMotionCount) / 2);
                            //}
                            //else if (faceCount >= 1)
                            //{
                            //    Debug.Log("x軸");
                            //    SEManager.Instance.PlaySE("ColorChange");
                            //    transform.Rotate((90.0f * 1.0f / changeMotionCount) / 2, 0.0f, 0.0f);
                            //}

                        }
                        else if (motionCount < changeMotionCount * 2)
                        {
                            if (faceCount >= 4)
                            {
                                Debug.Log("z軸");
                                SEManager.Instance.PlaySE("ColorChange");
                                transform.Rotate(0.0f, 0.0f, 90.0f * 1.0f / changeMotionCount);
                            }
                            else if (faceCount >= 1)
                            {
                                Debug.Log("x軸");
                                SEManager.Instance.PlaySE("ColorChange");
                                transform.Rotate(90.0f * 1.0f / changeMotionCount, 0.0f, 0.0f);
                            }

                        }
                        else if (motionCount < changeMotionCount * 3)
                        {
                            Vector3 transformPos = transform.position;
                            transformPos.y -= 0.1f;
                            transform.position = transformPos;
                            //if (faceCount >= 4)
                            //{
                            //    Debug.Log("z軸");
                            //    SEManager.Instance.PlaySE("ColorChange");
                            //    transform.Rotate(0.0f, 0.0f, (90.0f * 1.0f / changeMotionCount) / 2);
                            //}
                            //else if (faceCount >= 1)
                            //{
                            //    Debug.Log("x軸");
                            //    SEManager.Instance.PlaySE("ColorChange");
                            //    transform.Rotate((90.0f * 1.0f / changeMotionCount) / 2, 0.0f, 0.0f);
                            //}
                        }
                        motionCount++;
                        currentTime = 0.0f;
                    }

                }
            }
        }
        if (motionCount >= changeMotionCount * 3)
        {
            //モーションが終わったらマップをチェック
            parentMap.CheckMap();
            //リンクするFloorのモーション処理
            parentMap.LinkChangeFloorMotion(gameObject);
            parentMap.AllFloorWaitOff();

            if (parentMap.GetFaceNum() == 3)
            {
                if (faceCount >= 4)
                {
                    transform.rotation = Quaternion.Euler(180.0f, 0.0f, 0.0f);
                    faceCount = 1;
                }
            }
            //光度変更
            //if (state == scriptableObject.colorName) GetComponent<MeshRenderer>().material.color = Color.white * blockPlayerEmissive;
            //else GetComponent<MeshRenderer>().material.color = Color.white * blockEmissive;

            //ブロックがはまった時のエフェクトを生成
            effect = Instantiate(effectObject, new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z + 0.5f), Quaternion.identity) as GameObject;
            effect.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            //ブロックがはまった時のエフェクトをした後にリンクしているエフェクトを生成
            Invoke(nameof(CreateLinkEffect), 0.2f);

            motionCount = 0;
            currentTime = 0.0f;

            changeWait = false;
            change = false;
            doOnec = true;
            SEManager.Instance.PlaySE("Block_Fit");
        }
    }
    //マウスでの操作
    private void CursorUpdate()
    {
        if (change == false)//カーソルが当たっている時のモーション
        {
            if (linkChange == false)
            {
                if (cursor == true)
                {
                    Vector3 pos = transform.position;
                    pos.y = 0.3f;//マジックナンバーごめん
                    transform.position = pos;
                    cursor = false;
                    return;
                }
                else if (cursor == false)
                {
                    Vector3 pos = transform.position;
                    pos.y = 0.0f;//マジックナンバーごめん
                    transform.position = pos;

                }
            }
        }

    }
    //スマホタップでの操作
    private void TouchUpdate()
    {
        if (change == false)//カーソルが当たっている時のモーション
        {
            if (linkChange == false)
            {
                if (changeWait == true)
                {
                    Vector3 pos = transform.position;
                    pos.y = 0.3f;//マジックナンバーごめん
                    transform.position = pos;
                    return;
                }
                else if (changeWait == false)
                {
                    Vector3 pos = transform.position;
                    pos.y = 0.0f;//マジックナンバーごめん
                    transform.position = pos;

                }
            }
        }
    }
    private void LinkChangeFloor()
    {
        //周りのオブジェクトをかえるモーション
        if (linkChange == true)
        {
            currentLinkTime += Time.deltaTime;

            if (currentLinkTime > motionFrame)
            {
                //2面と4面の時は通常
                if (parentMap.GetFaceNum() == 2 || parentMap.GetFaceNum() == 4)
                {
                    if (motionLinkCount >= changeMotionCount)
                    {

                        //Debug.Log("モーション終了");
                        //周りの変更が終わったら初期化
                        currentLinkTime = 0.0f;
                        motionLinkCount = 0;

                        linkChange = false;
                        //マップのチェンジが終わったことをMapManagerに伝える
                        parentMap.GetComponent<MapManager>().ChangeOff();
                    }
                    else if (motionLinkCount < changeMotionCount)
                    {
                        transform.Rotate(90.0f * 1.0f / changeMotionCount, 0.0f, 0.0f);
                        currentLinkTime = 0.0f;
                        //光度変更
                        //if (state == scriptableObject.colorName) GetComponent<MeshRenderer>().material.color = Color.white * blockPlayerEmissive;
                        //else GetComponent<MeshRenderer>().material.color = Color.white * blockEmissive;
                        motionLinkCount++;
                    }
                }
                //3面の時
                else if (parentMap.GetFaceNum() == 3)
                {
                    if (motionLinkCount >= changeMotionCount)
                    {
                        //周りの変更が終わったら初期化
                        currentLinkTime = 0.0f;
                        motionLinkCount = 0;

                        linkChange = false;
                        //マップのチェンジが終わったことをMapManagerに伝える
                        parentMap.GetComponent<MapManager>().ChangeOff();

                        if (faceCount >= 4)
                        {
                            transform.rotation = Quaternion.Euler(180.0f, 0.0f, 0.0f);
                            faceCount = 1;
                        }
                        //光度変更
                        //if (state == scriptableObject.colorName) GetComponent<MeshRenderer>().material.color = Color.white * blockPlayerEmissive;
                        //else GetComponent<MeshRenderer>().material.color = Color.white * blockEmissive;
                    }
                    else if (motionLinkCount < changeMotionCount)
                    {
                        if (faceCount >= 4)
                        {
                            transform.Rotate(0.0f, 0.0f, 90.0f * 1.0f / changeMotionCount);
                        }
                        else if (faceCount >= 1)
                        {
                            transform.Rotate(90.0f * 1.0f / changeMotionCount, 0.0f, 0.0f);
                        }
                        currentLinkTime = 0.0f;
                        motionLinkCount++;
                    }
                }

            }
        }
    }

    public void CheckFloor()
	{
		rootCount = 0;
		List<GameObject> objList = new();

        bool goal = false;

		GameObject obj1 = parentMap.GetGameObjectList().Find(match => match.GetComponent<Floor>().GetMapPosition().x == GetComponent<Floor>().GetMapPosition().x && match.GetComponent<Floor>().GetMapPosition().z == GetComponent<Floor>().GetMapPosition().z - 1);
		GameObject obj2 = parentMap.GetGameObjectList().Find(match => match.GetComponent<Floor>().GetMapPosition().x == GetComponent<Floor>().GetMapPosition().x && match.GetComponent<Floor>().GetMapPosition().z == GetComponent<Floor>().GetMapPosition().z + 1);
		GameObject obj3 = parentMap.GetGameObjectList().Find(match => match.GetComponent<Floor>().GetMapPosition().x == GetComponent<Floor>().GetMapPosition().x - 1 && match.GetComponent<Floor>().GetMapPosition().z == GetComponent<Floor>().GetMapPosition().z);
		GameObject obj4 = parentMap.GetGameObjectList().Find(match => match.GetComponent<Floor>().GetMapPosition().x == GetComponent<Floor>().GetMapPosition().x + 1 && match.GetComponent<Floor>().GetMapPosition().z == GetComponent<Floor>().GetMapPosition().z);


		if (obj1 != null)
		{
			if (parentMap.GetCheckedFloorList().Contains(obj1.GetComponent<Floor>()) == false)
			{ 
				if (obj1.GetComponent<Floor>().GetFloorState() == "goal")
				{
                    goal = true;
					parentMap.GetCheckedFloorList().Add(obj1.GetComponent<Floor>());//チェックしたFloorはlistに登録
					Debug.Log("goalです");
					rootCount++;
                    //ゴールしたことをマップマネージャーに伝える
                    parentMap.IsMapClear();
                    //チェックの処理が終わるまではプレイヤーモーション処理を待機させる
                    parentMap.WaitOnGoal();
				}
			}
		}
		if (obj2 != null)
		{
			if (parentMap.GetCheckedFloorList().Contains(obj2.GetComponent<Floor>()) == false)
			{
				if (obj2.GetComponent<Floor>().GetFloorState() == "goal")
				{
                    goal = true;
                    parentMap.GetCheckedFloorList().Add(obj2.GetComponent<Floor>());//チェックしたFloorはlistに登録
					Debug.Log("goalです");
					rootCount++;
                    //ゴールしたことをマップマネージャーに伝える
                    parentMap.IsMapClear();
                    //チェックの処理が終わるまではプレイヤーモーション処理を待機させる
                    parentMap.WaitOnGoal();

                }

			}
		}
		if (obj3 != null)
		{
			if (parentMap.GetCheckedFloorList().Contains(obj3.GetComponent<Floor>()) == false)
			{
				if (obj3.GetComponent<Floor>().GetFloorState() == "goal")
				{
                    goal = true;
                    parentMap.GetCheckedFloorList().Add(obj3.GetComponent<Floor>());//チェックしたFloorはlistに登録
					Debug.Log("goalです");
					rootCount++;
                    //ゴールしたことをマップマネージャーに伝える
                    parentMap.IsMapClear();
                    //チェックの処理が終わるまではプレイヤーモーション処理を待機させる
                    parentMap.WaitOnGoal();

                }

			}
		}
		if (obj4 != null)
		{
			if (parentMap.GetCheckedFloorList().Contains(obj4.GetComponent<Floor>()) == false)
			{
				if (obj4.GetComponent<Floor>().GetFloorState() == "goal")
				{
                    goal = true;
                    parentMap.GetCheckedFloorList().Add(obj4.GetComponent<Floor>());//チェックしたFloorはlistに登録
					Debug.Log("goalです");
					rootCount++;
                    //ゴールしたことをマップマネージャーに伝える
                    parentMap.IsMapClear();
                    //チェックの処理が終わるまではプレイヤーモーション処理を待機させる
                    parentMap.WaitOnGoal();

                }

			}
		}

        //ゴールに到達してなかったらチェック処理続行
        if (goal == false)
        {
            if (obj1 != null)
            {
                if (parentMap.GetCheckedFloorList().Contains(obj1.GetComponent<Floor>()) == false)
                {

                    //Debug.Log("探索してきたlistの中になかったのでチェックします"
                    if (obj1.GetComponent<Floor>().GetFloorState() == scriptableObject.colorName)
                    {
                        obj1.GetComponent<Floor>().SetOldFloor(this);
                        parentMap.GetCheckedFloorList().Add(obj1.GetComponent<Floor>());//チェックしたFloorはlistに登録

                        //rootの数と自分の情報を登録                                                      
                        rootCount++;
                        //処理を行うリストに格納
                        objList.Add(obj1);
                    }
                }
            }
            //Debug.Log("↓を調べます");
            if (obj2 != null)
            {
                if (parentMap.GetCheckedFloorList().Contains(obj2.GetComponent<Floor>()) == false)
                {
                    if (obj2.GetComponent<Floor>().GetFloorState() == scriptableObject.colorName)
                    {
                        obj2.GetComponent<Floor>().SetOldFloor(this);
                        parentMap.GetCheckedFloorList().Add(obj2.GetComponent<Floor>());//チェックしたFloorはlistに登録;
                        rootCount++;
                        //処理を行うリストに格納
                        objList.Add(obj2);
                        Debug.Log(position);
                    }
                }
            }

            //Debug.Log("←を調べます");
            if (obj3 != null)
            {
                if (parentMap.GetCheckedFloorList().Contains(obj3.GetComponent<Floor>()) == false)
                {
                    if (obj3.GetComponent<Floor>().GetFloorState() == scriptableObject.colorName)
                    {
                        obj3.GetComponent<Floor>().SetOldFloor(this);
                        parentMap.GetCheckedFloorList().Add(obj3.GetComponent<Floor>());//チェックしたFloorはlistに登録

                        //rootの数と自分の情報を登録
                        rootCount++;
                        //処理を行うリストに格納
                        objList.Add(obj3);
                    }
                }
            }

            //Debug.Log("→を調べます");
            if (obj4 != null)
            {
                if (parentMap.GetCheckedFloorList().Contains(obj4.GetComponent<Floor>()) == false)
                {
                    //探索していたlistの中になければ進む
                    if (obj4.GetComponent<Floor>().GetFloorState() == scriptableObject.colorName)
                    {
                        obj4.GetComponent<Floor>().SetOldFloor(this);
                        parentMap.GetCheckedFloorList().Add(obj4.GetComponent<Floor>());//チェックしたFloorはlistに登録

                        //rootの数と自分の情報を登録
                        rootCount++;
                        //処理を行うリストに格納
                        objList.Add(obj4);
                    }
                }
            }
        }
		
		float wait = 0.01f;

        if (objList.Count == 1)
        {
            StartCoroutine(objList[0].GetComponent<Floor>().Check(wait));
        }
        else if (objList.Count == 2)
        {
            StartCoroutine(objList[0].GetComponent<Floor>().Check(wait));
            StartCoroutine(objList[1].GetComponent<Floor>().Check(wait));

        }
        else if (objList.Count == 3)
        {
            StartCoroutine(objList[0].GetComponent<Floor>().Check(wait));
            StartCoroutine(objList[1].GetComponent<Floor>().Check(wait));
            StartCoroutine(objList[2].GetComponent<Floor>().Check(wait));

        }
        else if (objList.Count == 4)
        {
            StartCoroutine(objList[0].GetComponent<Floor>().Check(wait));
            StartCoroutine(objList[1].GetComponent<Floor>().Check(wait));
            StartCoroutine(objList[2].GetComponent<Floor>().Check(wait));
            StartCoroutine(objList[3].GetComponent<Floor>().Check(wait));

        }

    }
    public void CheckOldRoot()
    {
        //rootが一つもなかったら前の情報のrootも確認
        if (rootCount == 0)
        {
            Debug.Log("戻ります");
            if (oldFloor != null) oldFloor.CheckOldRoot();

            Debug.Log(position.x);
            Debug.Log(position.z);
        }

        if (rootCount > 0)
        {
            rootCount--;

            if (rootCount == 0)
            {
                Debug.Log("戻ります");
                if (oldFloor != null) oldFloor.CheckOldRoot();
                Debug.Log(position.x);
                Debug.Log(position.z);
            }
        }

    }

    public IEnumerator Check(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		CheckFloor();
	}
	
	//エフェクト
	void CreateLinkEffect()
	{

		float side = 0.7f;

        GameObject obj1 = parentMap.GetGameObjectList().Find(match => match.GetComponent<Floor>().GetMapPosition().x == GetComponent<Floor>().GetMapPosition().x && match.GetComponent<Floor>().GetMapPosition().z == GetComponent<Floor>().GetMapPosition().z - 1);
        GameObject obj2 = parentMap.GetGameObjectList().Find(match => match.GetComponent<Floor>().GetMapPosition().x == GetComponent<Floor>().GetMapPosition().x && match.GetComponent<Floor>().GetMapPosition().z == GetComponent<Floor>().GetMapPosition().z + 1);
        GameObject obj3 = parentMap.GetGameObjectList().Find(match => match.GetComponent<Floor>().GetMapPosition().x == GetComponent<Floor>().GetMapPosition().x - 1 && match.GetComponent<Floor>().GetMapPosition().z == GetComponent<Floor>().GetMapPosition().z);
        GameObject obj4 = parentMap.GetGameObjectList().Find(match => match.GetComponent<Floor>().GetMapPosition().x == GetComponent<Floor>().GetMapPosition().x + 1 && match.GetComponent<Floor>().GetMapPosition().z == GetComponent<Floor>().GetMapPosition().z);

        GameObject linkEffectTop = null;
        GameObject linkEffectRight = null;
        GameObject linkEffectBottom = null;
        GameObject linkEffectLeft = null;


        if (obj1 != null) if(obj1.GetComponent<Floor>().GetFloorState() is  "player" or "goal") obj1 = null;
        if (obj2 != null) if (obj2.GetComponent<Floor>().GetFloorState() is "player" or "goal") obj2 = null;
        if (obj3 != null) if (obj3.GetComponent<Floor>().GetFloorState() is "player" or "goal") obj3 = null;
        if (obj4 != null) if (obj4.GetComponent<Floor>().GetFloorState() is "player" or "goal") obj4 = null;

        if (obj1 != null) linkEffectTop = Instantiate(linkEffectObject, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + side), Quaternion.identity) as GameObject;
        if (obj2 != null) linkEffectBottom = Instantiate(linkEffectObject, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z - side), Quaternion.identity) as GameObject;
        if (obj3 != null) linkEffectLeft = Instantiate(linkEffectObject, new Vector3(transform.position.x - side, transform.position.y + 0.5f, transform.position.z), Quaternion.identity) as GameObject;
        if (obj4 != null) linkEffectRight = Instantiate(linkEffectObject, new Vector3(transform.position.x + side, transform.position.y + 0.5f, transform.position.z), Quaternion.identity) as GameObject;

        if (linkEffectTop != null) linkEffectTop.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        if (linkEffectBottom != null) linkEffectBottom.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        if (linkEffectLeft != null) linkEffectLeft.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        if (linkEffectRight != null) linkEffectRight.transform.rotation = Quaternion.Euler(0.0f, 270.0f, 0.0f);
    }
}
