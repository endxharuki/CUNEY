using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class ChangeButtonImage : MonoBehaviour
{
	[SerializeField] private StageScriptableObject scriptableObject;
	[SerializeField] private Button button;
	private Image buttonImage;
	[SerializeField] private Sprite[] changeSprites;

	// Start is called before the first frame update
	void Start()
	{
		buttonImage = button.GetComponentInChildren<Image>();
	}

	// Update is called once per frame
	void Update()
	{
		if (SceneManager.GetActiveScene().name == "StageSelect" || scriptableObject.oldSceneName != "StageSelect")
		{
			buttonImage.sprite = changeSprites[0];
		}
		else if (SceneManager.GetActiveScene().name == "SampleScene" || scriptableObject.oldSceneName == "StageSelect")
		{
			buttonImage.sprite = changeSprites[1];
		}
	}
}
