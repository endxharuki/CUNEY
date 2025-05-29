using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
	// ボタンのアニメーションのコンポーネント
	private SelectAnimation anim;

	// シェーダーのマテリアルを参照
	public Material material;

	// 補完する値の開始点と終了点
	public float startValue = 1.0f; // 透明
	public float endValue = -1.0f;  // 不透明

	// フェードの時間
	public float fadeDuration = 2.0f;

	// シーン名
	public string nextSceneName;

	// フェード状態を管理
	private bool isFading = false;
	private float currentTime = 0.0f;

	// シェーダーのプロパティ名
	private string shaderPropertyName = "_Val";

	void Start()
	{
		//	コンポーネントを取得
		anim = GetComponent<SelectAnimation>();

		// マテリアルが設定されていない場合、自動で取得
		if (material == null)
		{
			Renderer renderer = GetComponent<Renderer>();
			if (renderer != null)
			{
				material = renderer.material;
			}
		}

		// シェーダーの初期値を設定
		material.SetFloat(shaderPropertyName, startValue);
	}

	void Update()
	{
		// フェード中であれば、フェードを進行
		if (isFading)
		{
			currentTime += Time.deltaTime;

			// フェード進行度を計算
			float t = currentTime / fadeDuration;
			float currentVal = Mathf.Lerp(startValue, endValue, t);

			// シェーダーの _Val に補完した値を設定
			material.SetFloat(shaderPropertyName, currentVal);

			// フェードが完了したらシーンを変更
			if (currentTime >= fadeDuration)
			{
				SceneManager.LoadScene(nextSceneName); // 次のシーンに切り替え
			}
		}
	}

	// フェードを開始する関数 (ボタンが押されたら呼ばれる)
	public void StartFade()
	{
		if (!isFading)
		{
			isFading = true;  // フェードを開始
			currentTime = 0.0f;  // フェード時間をリセット
		}
	}
}