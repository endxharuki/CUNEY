using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResizer : MonoBehaviour
{
    [SerializeField] private Slider positionSlider;  // スライダーの参照
    [SerializeField] private Image targetImage;      // 追従するImageの参照

    [SerializeField] private Vector2 moveRange;      // Imageの動く範囲（X軸）
    [SerializeField] private Vector2 scaleRange;     // Imageのスケール範囲

    private void Update()
    {
        // Imageの位置とスケール更新
        UpdateImagePositionAndScale();
    }

    private void UpdateImagePositionAndScale()
    {
        // スライダーの値を正規化して取得
        float normalizedValue = GetNormalizedSliderValue();

        // 位置とスケール設定
        SetImagePosition(normalizedValue);
        SetImageScale(normalizedValue);
    }

    private float GetNormalizedSliderValue()
    {
        // スライダーの現在の値を取得し、最小値と最大値に基づいて正規化
        float sliderValue = positionSlider.value;
        return (sliderValue - positionSlider.minValue) / (positionSlider.maxValue - positionSlider.minValue);
    }

    private void SetImagePosition(float normalizedValue)
    {
        // ｘ座標を更新
        float targetXPosition = Mathf.Lerp(moveRange.x, moveRange.y, normalizedValue);

        // 位置を設定
        targetImage.rectTransform.anchoredPosition = new Vector2(targetXPosition, targetImage.rectTransform.anchoredPosition.y);
    }

    private void SetImageScale(float normalizedValue)
    {
        // スケールを計算
        float scale = Mathf.Lerp(scaleRange.x, scaleRange.y, normalizedValue);

        // スケール設定
        targetImage.rectTransform.localScale = new Vector3(scale, scale, 1f); // Z軸は固定
    }
}
