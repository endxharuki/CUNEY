using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTggleTex : MonoBehaviour
{
	/// <summary>
	/// •ÏŠ·—p‰æ‘œ
	/// </summary>
	[SerializeField] private Sprite[] ChangeSprite;
	private Toggle toggle;
	private Image BackImage;

	// Start is called before the first frame update
	void Start()
	{
		BackImage = GetComponent<Image>();
		toggle = GetComponent<Toggle>();

		UpdateImage(toggle.Value);
	}

	// Update is called once per frame
	void Update()
	{
		UpdateImage(toggle.Value);
	}

	void UpdateImage(bool Value)
	{
		if (Value)
		{
			BackImage.sprite = ChangeSprite[0];
		}
		else
		{
			BackImage.sprite = ChangeSprite[1];
		}
	}
}
