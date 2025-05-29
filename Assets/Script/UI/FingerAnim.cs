using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FingerAnim : MonoBehaviour
{
	[SerializeField] private Image image;
	[SerializeField] private float MoveSpeed;
	[SerializeField] private float MoveDistance;
	[SerializeField] private float startPos;
	[SerializeField] private bool Revars = false;

	void Update()
	{
		if (Revars)
		{
			image.transform.localPosition = new Vector3(image.transform.localPosition.x, ((Mathf.Sin(Time.time) * MoveSpeed) * MoveDistance + startPos) * -1, 0);
		}
		else
		{
			image.transform.localPosition = new Vector3(image.transform.localPosition.x, (Mathf.Sin(Time.time) * MoveSpeed) * MoveDistance + startPos, 0);
		}
	}
}
