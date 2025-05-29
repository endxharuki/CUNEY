using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MoveCloud : MonoBehaviour
{
	[SerializeField] private float Speed;

	[SerializeField] private RectTransform pos;
	private int Counter = 0;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		pos.position -= new Vector3(Speed, 0.0f, 0.0f);
		Counter++;

		if (Counter > 2500)
		{
			Destroy(gameObject);
		}
	}
}
