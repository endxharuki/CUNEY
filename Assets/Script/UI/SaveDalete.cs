using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SaveDalete : MonoBehaviour
{
	[SerializeField] private GameObject prefab;

	public void Create()
	{
		GameObject prefab1 = Instantiate(prefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
		prefab1.transform.SetParent(GameObject.Find("TitleCanvas").transform, false);
		prefab1.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.5f).SetEase(Ease.OutBack);
	}

	public void Destroy()
	{
		if (GameObject.Find("DeleteBack(Clone)") != null)
		{
			GameObject.Find("DeleteBack(Clone)").transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 0.5f).SetEase(Ease.InBack).
				OnComplete(() =>
				{
					Destroy(GameObject.Find("DeleteBack(Clone)"));
					GameObject.Find("SaveDelete").GetComponent<Button>().enabled = true;
				});
		}
		else
		{
			GameObject.Find("Complete(Clone)").transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 0.5f).SetEase(Ease.InBack).
				OnComplete(() =>
				{
					Destroy(GameObject.Find("Complete(Clone)"));
					GameObject.Find("SaveDelete").GetComponent<Button>().enabled = true;
				});
		}
	}

	public void FastDelete()
	{
		Destroy(GameObject.Find("DeleteBack(Clone)"));
	}
}
