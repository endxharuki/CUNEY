using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DrawClear : MonoBehaviour
{
	// カメラコントロール
	private CameraControl cameraControl;
	[Header("生成する画像")][SerializeField] private GameObject[] Images;
	[Header("生成するキャンバス")][SerializeField] private Canvas canvas;
	[Header("生成する座標")][SerializeField] private float[] createPos;
	[Header("画像のアニメーション速度")][SerializeField] private float duration;
	private int creatCount = 0;

	// Start is called before the first frame update
	void Start()
	{
		cameraControl = GetComponent<CameraControl>();
		creatCount = 0;
	}

	// Update is called once per frame
	void Update()
	{
		if (cameraControl.GetIsResult() == true && creatCount < 1)
		{
			Debug.Log("せいせいされてます");
			GameObject prefab1 = Instantiate(Images[0], new Vector3(0.0f, createPos[0], 0.0f), Quaternion.identity);
			prefab1.transform.SetParent(canvas.transform, false);
			prefab1.transform.DOScale(new Vector3(1.0f, 1.0f, 0.0f), duration).SetEase(Ease.OutBack);
			GameObject prefab2 = Instantiate(Images[1], new Vector3(0.0f, createPos[1], 0.0f), Quaternion.identity);
			prefab2.transform.SetParent(canvas.transform, false);
			prefab2.transform.DOLocalMoveY(-700.0f, duration).SetEase(Ease.OutCirc).SetDelay(1f);

			// クリアSE再生
            SEManager.Instance.PlaySE("Congratulation");

			// BGM再生
			BGMManager.instance.PlayBGM("Clear");

			creatCount++;
		}
	}
}
