using UnityEngine;

public class Blooms : MonoBehaviour
{
    public Color outlineColor = Color.yellow; // 輪郭の色
    public float outlineWidth = 0.03f;        // 輪郭の太さ

    private Material outlineMaterial;

    void Start()
    {
        // 元のマテリアルを取得し、アウトラインマテリアルを作成
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            outlineMaterial = new Material(Shader.Find("Custom/OutlineShader"));
            outlineMaterial.SetColor("_OutlineColor", outlineColor);
            outlineMaterial.SetFloat("_OutlineWidth", outlineWidth);

            // メッシュにアウトラインを追加
            renderer.materials = new Material[] { renderer.material, outlineMaterial };
        }
    }
}
