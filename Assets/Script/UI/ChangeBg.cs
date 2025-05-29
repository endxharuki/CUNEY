using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBg : MonoBehaviour
{
	[SerializeField] private StageScriptableObject ScriptableNum;
	[SerializeField] private Sprite[] ChangeSprites;

	private Image ChangeImage;

	// Start is called before the first frame update
	void Start()
	{
		ChangeImage = GetComponent<Image>();

		switch (ScriptableNum.DifficultyIndex)
		{
			case 1:
				ChangeImage.sprite = ChangeSprites[0];
				break;
			case 2:
				ChangeImage.sprite = ChangeSprites[1];
				break;
			case 3:
				ChangeImage.sprite = ChangeSprites[2];
				break;
		}
	}
}
