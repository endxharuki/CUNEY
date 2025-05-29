using UnityEngine;
using UnityEditor;

namespace TedLab.ModifyResolution.Editor
{
	public abstract class ModifyResolution
	{
		[MenuItem("TedLab/ModifyResolution/シーンのオブジェクトを全て更新")]
		private static void UpdateAllModifyResolutionScripts()
		{
			var cameraStableAspects = Object.FindObjectsOfType<CameraStableAspect>();
			foreach (var cameraStableAspect in cameraStableAspects){
				Undo.RecordObject(cameraStableAspect, "update CameraStableAspect");
				cameraStableAspect.UpdateCameraWithCheck();
			}
			
			var rectScalerWithViewports = Object.FindObjectsOfType<RectScalerWithViewport>();
			foreach (var rectScalerWithViewport in rectScalerWithViewports){
				Undo.RecordObject(rectScalerWithViewport, "update RectScalerWithViewport");
				rectScalerWithViewport.UpdateRectWithCheck();
			}
		}
	}
}