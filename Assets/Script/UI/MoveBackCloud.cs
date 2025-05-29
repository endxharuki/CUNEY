using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveBackCloud : MonoBehaviour
{
	[Header("スクリプタブルオブジェクト")]
	[SerializeField] private StageScriptableObject scriptableObj;
	[Header("生成するプレハブ")]
	[SerializeField] private GameObject prefab;
	[Header("移動速度")]
	[SerializeField] private float Speed = -1.0f;
	[Header("削除する座標")]
	[SerializeField] private float DestroyPos = -20.0f;
	[Header("生成を開始する座標")]
	[SerializeField] private float CreatePos = -1.0f;
	[Header("生成される座標")]
	[SerializeField] private float GeneratePos = 0.0f;
	[Header("変換する画像")]
	[SerializeField] private Image Image;
	[Header("変換に使用する画像")]
	[SerializeField] private Sprite[] changeSprites;
	[Header("生成するキャンバス")]
	[SerializeField] private Canvas canvas;

	// 背景の枚数(２枚以上は生成しないため)
	private int BGcount = 0;
	// 背景の位置
	private Vector3 pos;

	// Update is called once per frame
	void Update()
	{
		pos = transform.position;

		// 移動処理
		pos.x += Speed * Time.deltaTime;
		transform.position = pos;

		// 背景が続くよう生成
		if (pos.x <= CreatePos && BGcount < 1)
		{
			CreateCloud();
		}

		// 削除判定
		if (pos.x <= DestroyPos)
		{
			// 削除
			Destroy(gameObject);
			BGcount--;
		}

		// 難易度に合わせて画像を変更
		if (canvas.gameObject.activeSelf == true)
		{
			switch (scriptableObj.DifficultyIndex)
			{
				case 1:
					Image.sprite = changeSprites[0];
					break;
				case 2:
					Image.sprite = changeSprites[1];
					break;
				case 3:
					Image.sprite = changeSprites[2];
					break;
			}
		}
	}

	void CreateCloud()
	{
		Vector3 createPos1 = new Vector3(GeneratePos, 0.0f, 0.0f);
		GameObject createPrefab = Instantiate(prefab, createPos1, Quaternion.identity);
		createPrefab.transform.SetParent(canvas.transform, false);
		BGcount++;
	}
}
