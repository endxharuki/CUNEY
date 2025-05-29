using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeOptionImage : MonoBehaviour
{
	[SerializeField] private StageScriptableObject GameIndex;
	[SerializeField] private Button Button;
	[SerializeField] private Sprite[] changeSprites;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		switch (GameIndex.DifficultyIndex)
		{
			case 1:
				Button.image.sprite = changeSprites[0];
				break;
			case 2:
				Button.image.sprite = changeSprites[1];
				break;
			case 3:
				Button.image.sprite = changeSprites[2];
				break;
		}
	}
}
