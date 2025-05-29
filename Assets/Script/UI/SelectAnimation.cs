using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SelectAnimation : MonoBehaviour
{
	[SerializeField] private Button moveButton;
	[SerializeField] private float MoveSpeed;
	[SerializeField] private float MoveDistance;
	[SerializeField] private float startPos;
	[SerializeField] private bool Revars = true;
	void Update()
	{
		if (Revars)
		{
			moveButton.transform.localPosition = new Vector3((Mathf.Sin(Time.time) * MoveSpeed) * MoveDistance + startPos, moveButton.transform.localPosition.y, 0);
		}
		else
		{
			moveButton.transform.localPosition = new Vector3(((Mathf.Sin(Time.time) * MoveSpeed) * MoveDistance + startPos) * -1, moveButton.transform.localPosition.y, 0);
		}
	}
}
