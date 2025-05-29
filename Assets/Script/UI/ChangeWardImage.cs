using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeWardImage : MonoBehaviour
{
	[SerializeField] private StageScriptableObject GameIndex;

	[SerializeField] private Image changeImage;

	[SerializeField] private Sprite[] changeSprites;

	// Update is called once per frame
	void Update()
	{
		switch (GameIndex.DifficultyIndex)
		{
			case 1:
				changeImage.sprite = changeSprites[0];
				break;
			case 2:
				changeImage.sprite = changeSprites[1];
				break;
			case 3:
				changeImage.sprite = changeSprites[2];
				break;
		}
	}
}
