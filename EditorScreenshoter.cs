using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class EditorScreenshoter : MonoBehaviour
{
	[SerializeField]
	private Camera _camera;

	[SerializeField]
	private Vector2 _textureDimensions = new Vector2(4096f, 4096f);

	[SerializeField]
	private Texture2D _screenshot;

	public const string ScreenshotsDirectory = "/EFT_Screenshots/";

	public static Texture2D MakeScreenshot(Camera camera, int width, int height)
	{
		Texture2D texture2D = new Texture2D(width, height);
		texture2D.name = _ED3E._E000(95171);
		texture2D.filterMode = FilterMode.Point;
		RenderTexture targetTexture = camera.targetTexture;
		RenderTexture temporary = RenderTexture.GetTemporary(width, height, 24, RenderTextureFormat.ARGB32);
		temporary.filterMode = FilterMode.Point;
		CommandBuffer[] commandBuffers = camera.GetCommandBuffers(CameraEvent.AfterLighting);
		CommandBuffer[] array = commandBuffers;
		foreach (CommandBuffer buffer in array)
		{
			camera.RemoveCommandBuffer(CameraEvent.AfterLighting, buffer);
		}
		camera.targetTexture = temporary;
		camera.Render();
		RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 24, RenderTextureFormat.ARGB32);
		temporary2.filterMode = FilterMode.Point;
		_E40B.TransformTexture(temporary, new Color(0f, 0f, 0f, 1f), new Color(1f, 1f, 1f, 0f), temporary2);
		RenderTexture.active = temporary2;
		texture2D.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
		texture2D.Apply();
		camera.targetTexture = targetTexture;
		RenderTexture.active = null;
		RenderTexture.ReleaseTemporary(temporary);
		RenderTexture.ReleaseTemporary(temporary2);
		array = commandBuffers;
		foreach (CommandBuffer buffer2 in array)
		{
			camera.AddCommandBuffer(CameraEvent.AfterLighting, buffer2);
		}
		return texture2D;
	}

	public void MakeScreenshotAndSave()
	{
		string text = Application.dataPath.Substring(0, Application.dataPath.Length - 7);
		if (!Directory.Exists(text + _ED3E._E000(95214)))
		{
			Directory.CreateDirectory(text + _ED3E._E000(95214));
		}
		_screenshot = MakeScreenshot(_camera, (int)_textureDimensions.x, (int)_textureDimensions.y);
		string text2 = text + _ED3E._E000(97304) + _E5AD.Now.ToString(_ED3E._E000(97336)) + _ED3E._E000(45670);
		File.WriteAllBytes(text2, _screenshot.EncodeToPNG());
		Debug.Log(_ED3E._E000(97317) + text2);
	}
}
