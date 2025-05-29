using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSetter : MonoBehaviour
{
	[SerializeField] private StageScriptableObject StageNum;
	[SerializeField] private int SetNum;

	// Start is called before the first frame update
	public void StageSet()
	{
		StageNum.StageNum = SetNum;
	}
}
