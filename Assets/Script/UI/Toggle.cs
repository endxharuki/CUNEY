using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toggle : MonoBehaviour
{
	[SerializeField] private Image backgroundImage;
	[SerializeField] private RectTransform handle;
	[SerializeField] private bool onAwake;

	/// <summary>
	/// トグルの値
	/// </summary>
	[NonSerialized] public bool Value;

	/// <summary>
	/// 変換用画像
	/// </summary>
	[SerializeField] private Sprite[] changeSprites;

	private float handlePosX;
	private Sequence sequence;

	private static readonly Color OFF_BG_COLOR = new Color(1, 1, 1);
	private static readonly Color ON_BG_COLOR = new Color(1, 1, 1);

	private const float SWITCH_DURATION = 0.36f;

	private void Start()
	{
		handlePosX = Mathf.Abs(handle.anchoredPosition.x);
		Value = onAwake;
		UpdateToggle(0);
	}

	/// <summary>
	/// トグルのボタンアクションに設定しておく
	/// </summary>
	public void SwitchToggle()
	{
		Value = !Value;
		UpdateToggle(SWITCH_DURATION);
	}

	/// <summary>
	/// 状態を反映させる
	/// </summary>
	private void UpdateToggle(float duration)
	{
		var bgColor = Value ? ON_BG_COLOR : OFF_BG_COLOR;
		var handleDestX = Value ? handlePosX : -handlePosX;

		sequence?.Complete();
		sequence = DOTween.Sequence();
		sequence.Append(backgroundImage.DOColor(bgColor, duration))
			.Join(handle.DOAnchorPosX(handleDestX, duration / 2));

		if (Value)
		{
			backgroundImage.sprite = changeSprites[0];
		}
		else
		{
			backgroundImage.sprite = changeSprites[1];
		}
	}
}
