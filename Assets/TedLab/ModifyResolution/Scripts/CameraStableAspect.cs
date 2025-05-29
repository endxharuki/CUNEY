using UnityEngine;

namespace TedLab
{
	[RequireComponent(typeof(Camera))]
	public class CameraStableAspect : MonoBehaviour
	{
		[SerializeField] private Camera refCamera;
		[SerializeField] private int width = 1920;
		[SerializeField] private int height = 1080;
		[SerializeField] private float pixelPerUnit = 100f;
		
		private int _width = -1;
		private int _height = -1;

		private void Awake()
		{
			if (refCamera == null)
			{
				refCamera = GetComponent<Camera>();
			}
			UpdateCamera();
		}

		private void Update() => UpdateCameraWithCheck();

		public void UpdateCameraWithCheck()
		{
			if (_width == Screen.width && _height == Screen.height){
				return;
			}
			UpdateCamera();
		}

		private void UpdateCamera()
		{
			var screenW = (float)Screen.width;
			var screenH = (float)Screen.height;
			var targetW = (float)width;
			var targetH = (float)height;

			//アスペクト比
			var aspect = screenW / screenH;
			var targetAspect = targetW / targetH;
			var orthographicSize = (targetH / 2f / pixelPerUnit);

			//縦に長い
			if (aspect < targetAspect)
			{
				var bgScaleW = targetW / screenW;
				var camHeight = targetH / (screenH * bgScaleW);
				refCamera.rect = new Rect(0f, (1f - camHeight) * 0.5f, 1f, camHeight);
			}
			// 横に長い
			else
			{
				// カメラのorthographicSizeを横の長さに合わせて設定しなおす
				var bgScale = aspect / targetAspect;
				orthographicSize *= bgScale;

				var bgScaleH = targetH / screenH;
				var camWidth = targetW / (screenW * bgScaleH);
				refCamera.rect = new Rect((1f - camWidth) * 0.5f, 0f, camWidth, 1f);
			}

			refCamera.orthographicSize = orthographicSize;

			_width = Screen.width;
			_height = Screen.height;
		}
	}
}