using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ClearAnimManager : MonoBehaviour
{
	// ステージマネージャー
	private StageSelectManager ssMng;
	// 移動させるボタン
	[SerializeField] private Button[] moveButton;
	// 移動させる画像
	[SerializeField] private Image[] moveImage;

	// Start is called before the first frame update
	void Start()
	{
		ssMng = GetComponent<StageSelectManager>();
	}

	// Update is called once per frame
	void Update()
	{
		if (ssMng.isClear)
		{
			// 画像の移動
			moveImage[0].transform.DOLocalMoveY(1150.0f, 1.0f).SetEase(Ease.OutCubic);
			moveImage[1].transform.DOLocalMoveY(1150.0f, 1.0f).SetEase(Ease.OutCubic);
			// ボタンの移動
			moveButton[0].transform.DOLocalMoveY(-1150.0f, 1.0f).SetEase(Ease.OutCubic);
			moveButton[1].transform.DOLocalMoveY(-1150.0f, 1.0f).SetEase(Ease.OutCubic);
		}
	}
}
