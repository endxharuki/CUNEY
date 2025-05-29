using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ゴールに当たった時、タグがPlayerだったらゴールにする
        if(collision.CompareTag("Player"))
        {
            GameMain gameMain = GameObject.FindObjectOfType<GameMain>();
            gameMain.OnGoal();
        }
    }
}
