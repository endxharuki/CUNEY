using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{
	//スクリプタぶるオブジェクトのClearは-1して
	//ステージセレクトから何番目のステージを選んだかを入れる変数
	[SerializeField] private StageScriptableObject StageObj;
	private int selecetStageNum = 0;
	[SerializeField] private GameObject map;
	//マップファイル一覧
	[Header("マップファイル一覧")]
	[SerializeField]
	private TextAsset TutorialMap;
	[SerializeField]
	private TextAsset MapFile1;
	[SerializeField]
	private TextAsset MapFile2;
	[SerializeField]
	private TextAsset MapFile3;
	[SerializeField]
	private TextAsset MapFile4;
	[SerializeField]
	private TextAsset MapFile5;
	[SerializeField]
	private TextAsset MapFile6;
	[SerializeField]
	private TextAsset MapFile7;
	[SerializeField]
	private TextAsset MapFile8;
	[SerializeField]
	private TextAsset MapFile9;

	private readonly List<TextAsset> mapFaileList = new();

	[Header("フェード")]
	[SerializeField] private Image fade = default;

	private GameObject mapObject;
	//このステージをクリアしたかどうか
	public bool isClear = false;

	private void Awake()
	{
		
		Application.targetFrameRate = 60;

		mapObject = null;
		isClear = false;

        selecetStageNum = 0;
        selecetStageNum = StageObj.StageNum;

		string textLines = null;

        if (SceneManager.GetActiveScene().name == "TutorialScene")
		{
            //最初だったらチュートリアルステージをやらせる

            mapObject = Instantiate(map, new Vector3(0, 0, 0), Quaternion.identity);
            mapObject.GetComponent<MapManager>().parentManager = this;
            mapObject.GetComponent<MapManager>().MapFile = TutorialMap;

			textLines = TutorialMap.text;
        }

		if (SceneManager.GetActiveScene().name == "SampleScene")
		{
			//リストから呼び出すときは-1してね
			//テクストファイルのリストにマップのデータを全て格納する
			mapFaileList.Add(MapFile1);
			mapFaileList.Add(MapFile2);
			mapFaileList.Add(MapFile3);
			mapFaileList.Add(MapFile4);
			mapFaileList.Add(MapFile5);
			mapFaileList.Add(MapFile6);
			mapFaileList.Add(MapFile7);
			mapFaileList.Add(MapFile8);
			mapFaileList.Add(MapFile9);

			mapObject = Instantiate(map, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
			mapObject.GetComponent<MapManager>().MapFile = mapFaileList[selecetStageNum - 1];
			mapObject.GetComponent<MapManager>().parentManager = this;

            textLines = mapFaileList[selecetStageNum - 1].text; // テキストの全体データの代入

        }
		
																   // 改行でデータを分割して配列に代入
		string[] textData = textLines.Split('\n');
		// 改行でデータを分割して配列に代入
		textData = textLines.Split('\n');

		// 行数と列数の取得
		int textXNumber = textData[0].Split(',').Length;
		int textYNumber = textData.Length;
		textYNumber -= 1;


		// ２次元配列の定義
		string[,] dungeonMap = new string[textYNumber, textXNumber];//マップ

		for (int i = 0; i < 1; i++)
		{
			string[] tempWords = textData[i].Split(',');
			for (int j = 0; j < 1; j++)
			{
				dungeonMap[i, j] = tempWords[j];

				StageObj.colorName = dungeonMap[i, j];
			}
		}

		if (StageObj.colorName != "red" && StageObj.colorName != "blue" && StageObj.colorName != "yellow" && StageObj.colorName != "green")
		{
			Debug.Log("色の名前ちゃんと見て");
		}
	}


	// Update is called once per frame
	void Update()
	{

	}
	public void DoneGoal()
	{

	}
	public GameObject GetMapObject()
	{
		return mapObject;
	}
	public void ChangeScene()
	{
		// ステージをクリアしたかどうかを設定
		if (SceneManager.GetActiveScene().name == "SampleScene")
		{
			StageObj.isClearList[selecetStageNum - 1] = true;
            // フェードイン・アウトを実行
            fade.GetComponent<FadeINOUT>().FadeToChangeScene(0);
        }
        // ステージをクリアしたかどうかを設定
        else if (SceneManager.GetActiveScene().name == "TutorialScene")
        {
            // フェードイン・アウトを実行
            fade.GetComponent<FadeINOUT>().FadeToChangeScene(1);
        }
        

	}
}
