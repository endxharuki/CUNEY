using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CreateCloud : MonoBehaviour
{
	[SerializeField] private StageScriptableObject scriptableNum;

	[SerializeField] private GameObject MoningImage1;
	[SerializeField] private GameObject MoningImage2;
	[SerializeField] private GameObject LunchImage1;
	[SerializeField] private GameObject LunchImage2;
	[SerializeField] private GameObject EveningImage1;
	[SerializeField] private GameObject EveningImage2;

	[SerializeField]
	GameObject canvas;

	[SerializeField]
	private float CreateDeray;

	private float RandPosY = 0.0f;
	private int RandIndex = 0;

	// Use this for initialization
	void Start()
	{
		StartCoroutine("CreateImage");
	}

	IEnumerator CreateImage()
	{
		switch (scriptableNum.DifficultyIndex)
		{
			case 1:
				while (true)
				{
					RandIndex = Random.Range(1, 3);

					switch (RandIndex)
					{
						case 1:
							RandPosY = Random.Range(-960.0f, 575.0f);
							Vector3 CreatePos1 = new Vector3(1100.0f, RandPosY, 0.0f);
							GameObject prefab1 = Instantiate(MoningImage1, CreatePos1, Quaternion.identity);
							prefab1.transform.SetParent(canvas.transform, false);
							yield return new WaitForSeconds(CreateDeray);
							break;
						case 2:
							RandPosY = Random.Range(-960.0f, 575.0f);
							Vector3 CreatePos2 = new Vector3(1100.0f, RandPosY, 0.0f);
							GameObject prefab2 = Instantiate(MoningImage2, CreatePos2, Quaternion.identity);
							prefab2.transform.SetParent(canvas.transform, false);
							yield return new WaitForSeconds(CreateDeray);
							break;
					}
				}

			case 2:
				while (true)
				{
					RandIndex = Random.Range(1, 3);

					switch (RandIndex)
					{
						case 1:
							RandPosY = Random.Range(-960.0f, 575.0f);
							Vector3 CreatePos1 = new Vector3(1100.0f, RandPosY, 0.0f);
							GameObject prefab1 = Instantiate(LunchImage1, CreatePos1, Quaternion.identity);
							prefab1.transform.SetParent(canvas.transform, false);
							yield return new WaitForSeconds(CreateDeray);
							break;
						case 2:
							RandPosY = Random.Range(-960.0f, 575.0f);
							Vector3 CreatePos2 = new Vector3(1100.0f, RandPosY, 0.0f);
							GameObject prefab2 = Instantiate(LunchImage2, CreatePos2, Quaternion.identity);
							prefab2.transform.SetParent(canvas.transform, false);
							yield return new WaitForSeconds(CreateDeray);
							break;
					}
				}


			case 3:
				while (true)
				{
					RandIndex = Random.Range(1, 3);

					switch (RandIndex)
					{
						case 1:
							RandPosY = Random.Range(-960.0f, 575.0f);
							Vector3 CreatePos1 = new Vector3(1100.0f, RandPosY, 0.0f);
							GameObject prefab1 = Instantiate(EveningImage1, CreatePos1, Quaternion.identity);
							prefab1.transform.SetParent(canvas.transform, false);
							yield return new WaitForSeconds(CreateDeray);
							break;
						case 2:
							RandPosY = Random.Range(-960.0f, 575.0f);
							Vector3 CreatePos2 = new Vector3(1100.0f, RandPosY, 0.0f);
							GameObject prefab2 = Instantiate(EveningImage2, CreatePos2, Quaternion.identity);
							prefab2.transform.SetParent(canvas.transform, false);
							yield return new WaitForSeconds(CreateDeray);
							break;
					}
				}
			default:
				break;
		}
	}
}
