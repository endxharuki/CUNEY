using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ClearSelectAnim : MonoBehaviour
{
	[Header("共有オブジェクト")]
	[SerializeField] private StageScriptableObject scriptableObject;
	[Header("触れなくするボタン")]
	[SerializeField] private List<Button> buttonList;
	[Header("アニメーションする画像")]
	[SerializeField] private List<Image> image;
	[Header("消すキャンバス")]
	[SerializeField] private List<CanvasGroup> canvas;

	private void Start()
	{

		Debug.Log(scriptableObject.StageNum);
		if (scriptableObject.oldIsClear[scriptableObject.StageNum] !=
			scriptableObject.isClearList[scriptableObject.StageNum])
		{
			if (scriptableObject.StageNum == 0 || scriptableObject.StageNum == 3 || scriptableObject.StageNum == 6)
				ClearAnim(0);
			else if (scriptableObject.StageNum == 1 || scriptableObject.StageNum == 4 || scriptableObject.StageNum == 7)
				ClearAnim(1);
			else if (scriptableObject.StageNum == 2 || scriptableObject.StageNum == 5)
				ClearAnim(-1);
		}

	}

	void ClearAnim(int buttonNum)
	{
		Debug.Log("呼ばれてます");

		// ボタンを触れない様に
		for (int i = 0; i < buttonList.Count; i++)
		{
			buttonList[i].enabled = false;
		}

		// シークエンスを確保
		var sequence = DOTween.Sequence();

		// 錠前を開けるアニメーション
		sequence.Append(image[buttonNum + 1].transform.DOLocalMoveY(20.0f, 1.0f));
		// 透過のアニメーション
		sequence.Append(canvas[buttonNum + 1].DOFade(0.0f, 1.0f));
		sequence.Play().OnComplete(() =>
		{
			// 完了後ボタンを触れる様に
			for (int i = 0; i < buttonList.Count; i++)
			{
				buttonList[i].enabled = true;
			}
		});
	}
}
