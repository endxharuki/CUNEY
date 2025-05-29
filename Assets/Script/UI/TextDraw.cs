using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextDraw : MonoBehaviour
{
	[Header("共有オブジェクト")]
	[SerializeField] private StageScriptableObject scriptableObject;

	[Header("表示するテキスト")]
	[SerializeField] private TMP_Text text;

	[Header("各フェーズごとのテキスト")]
	[SerializeField] private String[] tutoText;

	[Header("次の文字を表示するまでの時間[s]")]
	[SerializeField] private float delayDuration = 0.1f;

	// コルーチン格納用変数
	private Coroutine showCoroutine;

	private bool Faze2 = true;

	private void Start()
	{
		scriptableObject.textIndex = 0;
		Show();
	}

	private void Update()
	{
		AddTextIndex();
	}

	/// <summary>
	/// 文字送り演出を表示する
	/// </summary>
	public void Show()
	{
		// 前回の演出処理が走っていたら、停止
		if (showCoroutine != null)
			StopCoroutine(showCoroutine);

		// 文字列の更新
		if (scriptableObject.textIndex < 7)
			text.text = tutoText[scriptableObject.textIndex];

		// １文字ずつ表示する演出のコルーチンを実行する
		showCoroutine = StartCoroutine(ShowCoroutine());

		if (scriptableObject.textIndex == 2)
		{
			Faze2 = false;
		}
	}

	// １文字ずつ表示する演出のコルーチン
	private IEnumerator ShowCoroutine()
	{
		// 待機用コルーチン
		// GC Allocを最小化するためキャッシュしておく
		var delay = new WaitForSeconds(delayDuration);

		// テキスト全体の長さ
		var length = text.text.Length;

		// １文字ずつ表示する演出
		for (var i = 0; i < length; i++)
		{
			// 徐々に表示文字数を増やしていく
			text.maxVisibleCharacters = i;

			// 一定時間待機
			yield return delay;
		}

		// 演出が終わったら全ての文字を表示する
		text.maxVisibleCharacters = length;

		showCoroutine = null;
	}

	void AddTextIndex()
	{
		//if (Input.touchCount <= 0)
		//{ return; }

		//Touch touch = Input.GetTouch(0);
		if (scriptableObject.textIndex == 2 && Faze2 == true)
		{
			if (showCoroutine == null && scriptableObject.textIndex < 7)
			{
				if (scriptableObject.textIndex <= 6)
					Show();
			}
		}
		else if (scriptableObject.textIndex != 1)
		{
			if (/*touch.phase == TouchPhase.Began ||*/ Input.GetMouseButtonDown(0))
			{
				if (showCoroutine == null && scriptableObject.textIndex < 7)
				{
					scriptableObject.textIndex++;
					if (scriptableObject.textIndex <= 6)
						Show();
				}
				if (scriptableObject.textIndex == 7)
				{
					scriptableObject.tutorialClear = true;
				}
			}
		}


	}
}
