using UnityEditor;
using UnityEngine;

namespace TedLab.ModifyResolution.Editor
{
	[CustomEditor(typeof(CameraStableAspect))]
	public class CameraStableAspectEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("反映"))
			{
				var cameraStableAspect = target as CameraStableAspect;
				if (cameraStableAspect != null)
				{
					Undo.RecordObject(cameraStableAspect, "update CameraStableAspect");
					cameraStableAspect.UpdateCameraWithCheck();
				}
			}
		}
	}
}