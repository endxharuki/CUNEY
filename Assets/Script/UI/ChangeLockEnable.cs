using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLockEnable : MonoBehaviour
{
	// ScriptableObject
	[SerializeField] private StageScriptableObject scriptableObj;
	// 変えるボタン
	[Header("変えるボタン")]
	[SerializeField] private Button[] changeButtons;
	// 変えるキャンバス
	[Header("変えるキャンバス")]
	[SerializeField] private Canvas[] changeCanvasArray;

	private void Start()
	{
		SetLock();
		for (int i = 0; i < scriptableObj.isClearList.Count; i++)
		{
			Debug.Log(scriptableObj.isClearList[i]);
		}
	}

	public void SetLock()
	{
		switch (scriptableObj.DifficultyIndex)
		{
			case 1:
				changeCanvasArray[0].enabled = false;
				changeButtons[0].enabled = true;

				for (int i = 1; i < 3; i++)
				{
					if (scriptableObj.isClearList[i - 1] == true)
					{
						changeCanvasArray[i].enabled = false;
						changeButtons[i].enabled = true;
					}
					else
					{
						changeCanvasArray[i].enabled = true;
						changeButtons[i].enabled = false;
					}
				}
				break;
			case 2:
				for (int i = 0; i < 3; i++)
				{
					if (scriptableObj.isClearList[i + 2] == true)
					{
						changeCanvasArray[i].enabled = false;
						changeButtons[i].enabled = true;
					}
					else
					{
						changeCanvasArray[i].enabled = true;
						changeButtons[i].enabled = false;
					}
				}
				break;
			case 3:
				for (int i = 0; i < 3; i++)
				{
					if (scriptableObj.isClearList[i + 5] == true)
					{
						changeCanvasArray[i].enabled = false;
						changeButtons[i].enabled = true;
					}
					else
					{
						changeCanvasArray[i].enabled = true;
						changeButtons[i].enabled = false;
					}
				}
				break;
		}
	}
}
