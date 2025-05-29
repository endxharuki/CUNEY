using UnityEditor;
using UnityEngine;

namespace TedLab.ModifyResolution.Editor
{
	[CustomEditor(typeof(RectScalerWithViewport))]
	public class RectScalerWithViewportEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("反映"))
			{
				var rectScalerWithViewport = target as RectScalerWithViewport;
				if (rectScalerWithViewport != null)
				{
					Undo.RecordObject(rectScalerWithViewport, "update RectScalerWithViewport");
					rectScalerWithViewport.UpdateRectWithCheck();
				}
			}
		}
	}
}