using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetMissionWard : MonoBehaviour
{
	[Header("共有オブジェクト")][SerializeField] private StageScriptableObject scriptableObject;
	[Header("変更する画像")][SerializeField] private Image image;
	[Header("切り替える用の画像")][SerializeField] private Sprite[] sprites;

	// Start is called before the first frame update
	void Start()
	{
		if (scriptableObject.colorName == "red")
		{
			image.sprite = sprites[0];
		}
		else if (scriptableObject.colorName == "blue")
		{
			image.sprite = sprites[1];
		}
		else if (scriptableObject.colorName == "yellow")
		{
			image.sprite = sprites[2];
		}
		else if (scriptableObject.colorName == "green")
		{
			image.sprite = sprites[3];
		}
	}
}
