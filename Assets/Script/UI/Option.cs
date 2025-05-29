using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
	[SerializeField]
	private Canvas option = default;
	[SerializeField]
	private Button[] buttons;

	private CursorManager cursorManager;
	private TouchControl touchControl;

	// Start is called before the first frame update
	void Start()
	{
		option.enabled = false;
	}

	private void Update()
	{
		if (option.enabled == true && cursorManager != null || option.enabled == true && touchControl != null)
		{
			cursorManager.enabled = false;
			touchControl.enabled = false;
		}
	}

	public void CreateOption()
	{
		if (GameObject.Find("map(Clone)") != null)
		{
			cursorManager = GameObject.Find("map(Clone)").GetComponent<CursorManager>();
			touchControl = GameObject.Find("map(Clone)").GetComponent<TouchControl>();
			cursorManager.enabled = false;
			touchControl.enabled = false;
		}

		option.enabled = true;

		for (int i = 0; i < buttons.Length; i++)
		{
			buttons[i].enabled = false;
		}
	}

	public void DestroyOption()
	{
		if (GameObject.Find("map(Clone)") != null)
		{
			cursorManager = GameObject.Find("map(Clone)").GetComponent<CursorManager>();
			touchControl = GameObject.Find("map(Clone)").GetComponent<TouchControl>();
			cursorManager.enabled = true;
			touchControl.enabled = true;
		}

		option.enabled = false;

		for (int i = 0; i < buttons.Length; i++)
		{
			buttons[i].enabled = true;
		}
	}
}
