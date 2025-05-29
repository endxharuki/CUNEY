using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetActive : MonoBehaviour
{
	[SerializeField] private StageScriptableObject scriptableObj;
	[SerializeField] private Button[] buttons;

	private void Update()
	{
		switch (scriptableObj.DifficultyIndex)
		{
			case 1:
				buttons[0].gameObject.SetActive(false);
				break;
			case 2:
				if (buttons[0].IsActive() == false)
					buttons[0].gameObject.SetActive(true);
				if (buttons[1].IsActive() == false)
					buttons[1].gameObject.SetActive(true);
				break;
			case 3:
				buttons[1].gameObject.SetActive(false);
				break;
		}
	}
}
