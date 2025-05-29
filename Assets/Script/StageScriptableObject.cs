using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "StageNum", menuName = "ScriptableObjects/CreateStageNumAsset")]
public class StageScriptableObject : UnityEngine.ScriptableObject
{
	[Header("ステージ番号")]
	[SerializeField] public int StageNum = 1;
	[Header("難易度")]
	[SerializeField] public int DifficultyIndex = 1;
	[Header("各ステージのクリア判定(ステージ開始時)")]
	public List<bool> oldIsClear = new List<bool>();
	[Header("各ステージのクリア判定()")]
	public List<bool> isClearList = new List<bool>();
	[Header("チュートリアルクリアしてるか")]
	[SerializeField] public bool tutorialClear = false;
	[Header("ボタンの番号")]
	[SerializeField] public int ButtonNum = 0;
	[Header("各ステージの色")]
	public string colorName = "None";
	[Header("チュートリアルテキストの番号")]
	public int textIndex = 0;

	[SerializeField]
	public string oldSceneName = "None";
}
