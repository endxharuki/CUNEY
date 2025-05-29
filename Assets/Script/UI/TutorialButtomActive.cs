using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialButtomActive : MonoBehaviour
{
	[SerializeField] private StageScriptableObject scriptableObject;
	[SerializeField] private Button button;

	// Start is called before the first frame update
	void Start()
	{
		if (scriptableObject.DifficultyIndex == 1)
		{
			button.gameObject.SetActive(true);
		}
		else
		{
			button.gameObject.SetActive(false);
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (scriptableObject.DifficultyIndex == 1)
		{
			button.gameObject.SetActive(true);
		}
		else
		{
			button.gameObject.SetActive(false);
		}
	}
}
