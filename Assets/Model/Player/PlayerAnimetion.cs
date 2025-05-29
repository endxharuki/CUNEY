using System.Collections;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private bool isYubisasiPlaying = false; // Yubisasiの状態を管理

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleYubisasiAnimation();
        HandleOtherAnimations();
    }

    // Yubisasiアニメーションの管理
    private void HandleYubisasiAnimation()
    {
        if (!isYubisasiPlaying && MapManager.floorChange)
        {
            PlayYubisasi();
        }
    }

    // Yubisasi再生
    private void PlayYubisasi()
    {
        animator.SetBool("Yubisasi", true);
        isYubisasiPlaying = true;
        MapManager.floorChange = false;
        StartCoroutine(OffYubisasi(1.5f)); // 3秒後にYubisasiを終了する
    }

    // 他のアニメーションの管理
    private void HandleOtherAnimations()
    {
        if (isYubisasiPlaying) return; // Yubisasiが再生中の場合、他のアニメーションを無効にする

        PlayerControl playerControl = GameObject.Find("Player(Clone)").GetComponent<PlayerControl>();

        if (playerControl.GetClear())
        {
            SetAnimationState("Jump");
        }
        else if (playerControl.GetOnPlayerMove())
        {
            SetAnimationState("Run");
        }
        else
        {
            SetAnimationState("Idle");
        }
    }

    // アニメーション状態の設定
    private void SetAnimationState(string state)
    {
        animator.SetBool("Idle", state == "Idle");
        animator.SetBool("Run", state == "Run");
        animator.SetBool("Jump", state == "Jump");
    }

private IEnumerator OffYubisasi(float delay)
{
    yield return new WaitForSeconds(delay);
    animator.SetBool("Yubisasi", false); // Yubisasiを終了
    isYubisasiPlaying = false;
}
}