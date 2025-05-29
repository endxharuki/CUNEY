using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeINOUT : MonoBehaviour
{
    // ScriptableObject
    [SerializeField]
    private StageScriptableObject scriptableObject;

    // フェードインに使用するマテリアル
    [SerializeField]
    private Material transitionIn;

    // フェードアウトに使用するマテリアル
    [SerializeField]
    private Material transitionOut;

    // フェード中に呼び出されるイベント
    [SerializeField]
    private UnityEvent OnTransition;

    // フェードが完了したときに呼び出されるイベント
    [SerializeField]
    private UnityEvent OnComplete;

    // ボタンリスト（インスペクターで設定）
    [SerializeField]
    private List<Button> transitionButtons;

    [Header("ボタンに対応するシーン名のリスト")]
    // ボタンに対応するシーン名のリスト
    [SerializeField]
    private List<string> sceneNames;

    private bool doOnce = false;

    public static bool buttonTap = true;

    // フェードアウトをトリガーする方法の選択肢
    public enum FadeTriggerMode { Tap, Button }

    // 現在のトリガー方法をインスペクターで設定
    [SerializeField]
    private FadeTriggerMode fadeTriggerMode = FadeTriggerMode.Tap;

    // ゲーム開始時にフェードインを開始
    void Start()
    {
        doOnce = false;
        buttonTap = false;
        // フェードインのコルーチンを開始
        StartCoroutine(BeginTransitionIn());

        // ボタンが設定されていて、ボタンモードが選択されている場合にクリックイベントを追加
        if (fadeTriggerMode == FadeTriggerMode.Button)
        {
            for (int i = 0; i < transitionButtons.Count; i++)
            {
                Button button = transitionButtons[i];
                // ボタンがクリックされたときに対応するシーンに切り替えるリスナーを追加
                if (button != null && i < sceneNames.Count) // sceneNamesの数もチェック
                {
                    int index = i; // ローカル変数でキャプチャ
                    button.onClick.AddListener(() =>
                    {
                        HandleButtonTransition(index);
                    });
                }
            }
        }
    }

    // フレームごとの処理で、画面タップでフェードアウトを開始
    void Update()
    {
        Debug.Log(scriptableObject.oldSceneName);
        if (buttonTap == false)
        {
            // 画面タップモードが選択されている場合、タップでフェードアウトを開始
            if (fadeTriggerMode == FadeTriggerMode.Tap && Input.GetMouseButtonDown(0))
            {
                scriptableObject.oldSceneName = SceneManager.GetActiveScene().name;
                Invoke(nameof(HandleTapTransition), 0.2f);
            }
        }
    }

    // ボタンからのフェードアウト処理をまとめたメソッド
    private void HandleButtonTransition(int index)
    {
            // SEを再生
        SEManager.Instance.PlaySE("Select");

        scriptableObject.oldSceneName = SceneManager.GetActiveScene().name;
        // 対応するシーンに切り替え
        StartCoroutine(BeginTransitionOut(sceneNames[index])); // 対応するシーンに切り替え
    }

    private void HandleTapTransition()
    {
        if (buttonTap == false)
        {
            if (doOnce == false)
            {
                if (scriptableObject.tutorialClear == true)
                {
                    // 最初のシーン名を使ってフェードアウトを開始
                    if (sceneNames.Count > 0)
                    {
                        StartCoroutine(BeginTransitionOut(sceneNames[0])); // 例として最初のシーン名を使用
                    }
                }
                else if (scriptableObject.tutorialClear == false)
                {
                    // 最初のシーン名を使ってフェードアウトを開始
                    if (sceneNames.Count > 0)
                    {
                        StartCoroutine(BeginTransitionOut(sceneNames[1])); // 例として最初のシーン名を使用
                    }
                }
                SEManager.Instance.PlaySE("Select");
                doOnce = true;
            }
        }
    }

    // フェードインを開始するコルーチン
    IEnumerator BeginTransitionIn()
    {
        // フェードインアニメーションを指定された時間で実行
        yield return Animate(transitionIn, 1);

        // フェードイン中に指定されたイベントを実行
        OnTransition?.Invoke();

        // フェードインが終了後に次のフレームを待つ
        yield return new WaitForEndOfFrame();
    }

    // フェードアウトを開始するコルーチン（シーン名を引数として受け取る）
    IEnumerator BeginTransitionOut(string sceneName)
    {
        // フェードアウトアニメーションを指定された時間で実行
        yield return Animate(transitionOut, 1);

        // フェードアウトが終了後に指定されたイベントを実行
        OnComplete?.Invoke();

        // フェードアウト完了後に次のフレームを待つ
        yield return new WaitForEndOfFrame();

        // 指定されたシーンに切り替え
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
            // フェードインを再開
            StartCoroutine(BeginTransitionIn());
        }
    }

    // 指定されたマテリアルでアニメーションを実行するコルーチン
    IEnumerator Animate(Material material, float time)
    {
        // UI Imageコンポーネントにマテリアルを適用
        GetComponent<Image>().material = material;

        float current = 0;

        // 指定された時間でフェードを進行
        while (current < time)
        {
            // シェーダーの "_Alpha" パラメータを時間経過に基づいて変更
            material.SetFloat("_Alpha", current / time);
            yield return new WaitForEndOfFrame(); // 次のフレームまで待機

            // 経過時間を更新
            current += Time.deltaTime;
        }

        // 最終的にフェードを完全に不透明に設定
        material.SetFloat("_Alpha", 1);
    }

    public void FadeToChangeScene(int num)
    {
        if(doOnce == false)
        {
            // 最初のシーン名を使ってフェードアウトを開始
            if (sceneNames.Count > 0)
            {
                SEManager.Instance.PlaySE("Select");
                doOnce = true;
                scriptableObject.oldSceneName = SceneManager.GetActiveScene().name;
                StartCoroutine(BeginTransitionOut(sceneNames[num])); // 例として最初のシーン名を使用
            }
        }
    }
}
