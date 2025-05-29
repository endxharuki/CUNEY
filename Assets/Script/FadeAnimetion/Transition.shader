Shader "Unlit/Transition"
{
	Properties
	{
		// メインのテクスチャを定義(デフォルトは色)
		_MainTex("Texture", 2D) = "white" {}
		// 閾値を設定する所
		_Val("Val", Range(-1.0, 1.0)) = 1.0
	}
		
	SubShader
	{
		// 描画設定
		Tags { "RenderType" = "Opaque" }
		LOD 100

		// アルファブレンド設定
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			// 頂点シェーダーとフラグメントシェーダーを定義
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// フォグを有効化
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			// 頂点シェーダーに渡されるデータ構造体
			struct appdata
			{
				float4 vertex : POSITION;		// 頂点の座標
				float2 uv : TEXCOORD0;			// テクスチャ座標
			};

			// 頂点シェーダーからフラグメントシェーダーに渡されるデータ構造体
			struct v2f
			{
				float2 uv : TEXCOORD0;			// テクスチャ座標
				UNITY_FOG_COORDS(1)				// フォグ用のデータ
				float4 vertex : SV_POSITION;	// クリップ空間での頂点位置
			};

			sampler2D _MainTex;		// メインテクスチャ
			float4 _MainTex_ST;		// テクスチャの変更行列(スケールとオフセット)

			// 閾値用の変数(スライダーで設定可能)
			float _Val;

			// 頂点データをクリップ空間に変換
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);		// モデル空間の座標をクリップ空間に変換
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);			// テクスチャ座標の変換
				UNITY_TRANSFER_FOG(o,o.vertex);					// フォグの計算を頂点データに追加
				return o;
			}

			// ピクセルごとの色を計算
			fixed4 frag(v2f i) : SV_Target
			{
				// テクスチャからサンプリングして色を取得
				fixed4 col = tex2D(_MainTex, i.uv);

				// テクスチャの赤色成分と閾値を比較し、出力を計算
				float output = saturate(col.x - _Val);

				/*
				// フォグの適用
				UNITY_APPLY_FOG(i.fogCoord, col);
				*/

				// 閾値だけ変更(出力色を返す)
				return fixed4(1.0f, 1.0f, 1.0f, output);
			}
			ENDCG
		}
	}
}