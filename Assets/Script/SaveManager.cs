using System;
using System.Collections;
using System.Collections.Generic;
using CI.QuickSave;
using CI.QuickSave.Core.Storage;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
	// 保存するScriptableObject
	[SerializeField] private StageScriptableObject GameIndex;
	// セーブの設定
	private QuickSaveSettings saveSettings;

	// Start is called before the first frame update
	void Start()
	{
		saveSettings = new QuickSaveSettings();
		saveSettings.SecurityMode = SecurityMode.Aes;
		saveSettings.Password = "CUNEY";
		saveSettings.CompressionMode = CompressionMode.Gzip;
	}

	private void OnApplicationQuit()
	{
		Save();
	}

	// セーブ
	public void Save()
	{
		Debug.Log("セーブデータ保存先:" + Application.persistentDataPath);

		// QuickSaveWriterのインスタンスを作成
		QuickSaveWriter writer = QuickSaveWriter.Create("SaveData", saveSettings);

		// データを書き込む
		writer.Write("ClearList", GameIndex.isClearList);
		writer.Write("Difficulty", GameIndex.DifficultyIndex);
		writer.Write("TutorialClear", GameIndex.tutorialClear);

		// 変更を反映
		writer.Commit();
	}

	// ロード
	public void Load()
	{
		if (FileAccess.Exists("SaveData") == false)
		{
			return;
		}

		// QuickSaveReaderのインスタンスを作成
		QuickSaveReader reader = QuickSaveReader.Create("SaveData", saveSettings);

		// データを読み込む
		GameIndex.isClearList = reader.Read<List<bool>>("ClearList");
		GameIndex.DifficultyIndex = reader.Read<int>("Difficulty");
		GameIndex.tutorialClear = reader.Read<bool>("TutorialClear");
	}
}
