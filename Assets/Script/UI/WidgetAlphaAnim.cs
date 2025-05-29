using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(CanvasGroup))]
public class WidgetAlphaAnim : MonoBehaviour
{
	[Header("点滅時間の間隔")]
	[SerializeField]
	private float blinkIntervalTime = 1.5f;

	[Header("イージングの設定")]
	[SerializeField]
	private Ease easeType = Ease.Linear;

	// Start is called before the first frame update
	void Start()
	{
		var canvasGroup = GetComponent<CanvasGroup>();
		canvasGroup.DOFade(0.0f, blinkIntervalTime).SetEase(easeType).SetLoops(-1, LoopType.Yoyo);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
