using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class PlaceAndScreenObject : MonoBehaviour
{
	private List<Quaternion> m__E000;

	private GameObject _E001;

	[SerializeField]
	private Camera _camera;

	public void Init(List<Vector3> screenshotAngles)
	{
		if (!_camera)
		{
			_camera = Camera.main;
		}
		this.m__E000 = new List<Quaternion>();
		foreach (Vector3 screenshotAngle in screenshotAngles)
		{
			this.m__E000.Add(Quaternion.Euler(screenshotAngle));
		}
	}

	public void PlaceObject(GameObject prefab)
	{
		if (_E001 != null)
		{
			Object.DestroyImmediate(_E001);
			_E001 = null;
		}
		_E001 = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
		_E001.name = prefab.name;
	}

	public string TakeScreenshot(string filePath, int texWidth, int texHeight)
	{
		if (_E001 == null)
		{
			return null;
		}
		if (!Directory.Exists(filePath))
		{
			Directory.CreateDirectory(filePath);
		}
		string text = string.Empty;
		List<string> list = new List<string>();
		for (int i = 0; i < this.m__E000.Count; i++)
		{
			_E001.transform.rotation = this.m__E000[i];
			_E3E4.CenterByBounds(_E001, _camera, 1f, new PreviewPivot.IconSettings());
			string text2 = string.Format(_ED3E._E000(46180), filePath, _E001.name, i);
			list.Add(text2);
			_E000(text2, texWidth, texHeight);
			if (string.IsNullOrEmpty(text))
			{
				text = text2;
			}
		}
		return text;
	}

	public void ClearScene()
	{
		if (_E001 != null)
		{
			Object.DestroyImmediate(_E001);
		}
	}

	private void _E000(string filePath, int texWidth, int texHeight)
	{
		_camera.clearFlags = CameraClearFlags.Color;
		_camera.backgroundColor = new Color(0f, 0f, 0f, 0f);
		_camera.useOcclusionCulling = false;
		RenderTexture renderTexture = new RenderTexture(texWidth, texHeight, 24)
		{
			name = _ED3E._E000(46235)
		};
		_camera.targetTexture = renderTexture;
		Texture2D obj = new Texture2D(texWidth, texHeight, TextureFormat.RGB24, mipChain: false)
		{
			name = _ED3E._E000(46211)
		};
		_camera.Render();
		RenderTexture.active = renderTexture;
		obj.ReadPixels(new Rect(0f, 0f, texWidth, texHeight), 0, 0);
		_camera.targetTexture = null;
		RenderTexture.active = null;
		Object.DestroyImmediate(renderTexture);
		byte[] bytes = obj.EncodeToPNG();
		File.WriteAllBytes(filePath, bytes);
	}
}
