using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class ScreenshotCreator : MonoBehaviour
{
	[Serializable]
	public class CameraObject
	{
		public enum Hotkey
		{
			Hotkey,
			A,
			B,
			C,
			D,
			E,
			F,
			G,
			H,
			I,
			J,
			K,
			L,
			M,
			N,
			O,
			P,
			Q,
			R,
			S,
			T,
			U,
			V,
			W,
			X,
			Y,
			Z
		}

		public GameObject cam;

		public bool deleteQuestion;

		public Hotkey hotkey;
	}

	public enum FileType
	{
		PNG,
		JPG
	}

	public enum CaptureMethod
	{
		CaptureScreenshot,
		RenderTexture,
		Cutout
	}

	[HideInInspector]
	public Color signatureColor = new Color(1f, 0f, 0.5f);

	public List<CameraObject> list = new List<CameraObject>();

	[HideInInspector]
	public bool foldoutSettings;

	[Tooltip("The name of your screenshot or screenshot session. Camera name and current date will be added automatically.")]
	public string customName = "";

	public string customDirectory = "";

	[HideInInspector]
	public int lastCamID;

	[HideInInspector]
	public Camera lastCam;

	public bool includeCamName = true;

	public bool includeDate = true;

	public bool includeResolution = true;

	public FileType fileType;

	public CaptureMethod captureMethod;

	public bool singleCamera;

	public float renderSizeMultiplier = 1f;

	public int captureSizeMultiplier = 1;

	public Vector2 cutoutPosition;

	public Vector2 cutoutSize;

	private GUIStyle m__E000;

	public bool applicationPath;

	private Vector2 m__E001;

	private Vector2 m__E002;

	private float m__E003;

	public void Create()
	{
		list.Add(new CameraObject());
	}

	public void RequestDelete(int id)
	{
		list[id].deleteQuestion = true;
	}

	public void Delete(int id)
	{
		list.Remove(list[id]);
		if (list.Count == 0)
		{
			Create();
		}
	}

	private void Awake()
	{
		if (list.Count == 0)
		{
			Create();
			list[0].cam = Camera.main.gameObject;
		}
	}

	private void LateUpdate()
	{
		if (!Input.anyKeyDown)
		{
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].hotkey == CameraObject.Hotkey.Hotkey || !Input.GetKeyDown(list[i].hotkey.ToString().ToLower()))
			{
				continue;
			}
			if (list[i] != null)
			{
				if (captureMethod == CaptureMethod.RenderTexture)
				{
					Camera component = list[i].cam.GetComponent<Camera>();
					if (component == null)
					{
						CaptureScreenshots(i, fallback: true);
					}
					else
					{
						CaptureRenderTexture(component, i);
					}
				}
				else if (captureMethod == CaptureMethod.CaptureScreenshot)
				{
					CaptureScreenshots(i, fallback: false);
				}
				else
				{
					StartCoroutine(CaptureCutout(i));
				}
				lastCam = list[lastCamID].cam.GetComponent<Camera>();
			}
			else
			{
				Debug.Log(string.Concat(_ED3E._E000(96734), list[i].hotkey, _ED3E._E000(96709)));
			}
		}
	}

	private void _E000(int id)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].cam != null)
			{
				list[i].cam.SetActive(value: false);
			}
		}
		list[id].cam.SetActive(value: true);
	}

	public string getSaveDirectory()
	{
		string text = ((customDirectory != "") ? customDirectory : _ED3E._E000(96795));
		if (applicationPath)
		{
			return Application.persistentDataPath + _ED3E._E000(30703) + text + _ED3E._E000(30703);
		}
		return Directory.GetCurrentDirectory() + _ED3E._E000(30703) + text + _ED3E._E000(30703);
	}

	private string _E001()
	{
		string saveDirectory = getSaveDirectory();
		if (!Directory.Exists(saveDirectory))
		{
			Directory.CreateDirectory(saveDirectory);
		}
		return saveDirectory;
	}

	private void _E002()
	{
		this.m__E000 = new GUIStyle(GUI.skin.box);
		int num = 16;
		Color[] array = new Color[num * num];
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num; j++)
			{
				if (i == 0 || i == num - 1 || j == 0 || j == num - 1)
				{
					array[i * num + j] = Color.white;
				}
				else
				{
					array[i * num + j] = new Color(1f, 1f, 1f, 0.1f);
				}
			}
		}
		Texture2D texture2D = new Texture2D(num, num);
		texture2D.name = _ED3E._E000(96783);
		texture2D.SetPixels(array);
		texture2D.Apply();
		this.m__E000.normal.background = texture2D;
	}

	private void _E003()
	{
		cutoutPosition.x = Mathf.Clamp(cutoutPosition.x, cutoutSize.x / 2f, (float)Screen.width - cutoutSize.x / 2f);
		cutoutPosition.y = Mathf.Clamp(cutoutPosition.y, cutoutSize.y / 2f, (float)Screen.height - cutoutSize.y / 2f);
		cutoutSize.x = Mathf.Clamp(cutoutSize.x, 0f, Screen.width);
		cutoutSize.y = Mathf.Clamp(cutoutSize.y, 0f, Screen.height);
	}

	private void OnGUI()
	{
		if (this.m__E001.x == cutoutPosition.x && this.m__E001.y == cutoutPosition.y && this.m__E002.x == cutoutSize.x && this.m__E002.y == cutoutSize.y)
		{
			this.m__E003 -= Time.deltaTime;
		}
		else
		{
			this.m__E003 = 1f;
		}
		this.m__E001 = cutoutPosition;
		this.m__E002 = cutoutSize;
		if (!(this.m__E003 <= 0f) && captureMethod == CaptureMethod.Cutout)
		{
			_E002();
			_E003();
			GUI.Box(new Rect(cutoutPosition.x - cutoutSize.x / 2f, cutoutPosition.y - cutoutSize.y / 2f, cutoutSize.x, cutoutSize.y), "", this.m__E000);
		}
	}

	public void CaptureCutoutVoid(int id)
	{
		if (singleCamera)
		{
			_E000(id);
		}
		StartCoroutine(CaptureCutout(id));
	}

	public IEnumerator CaptureCutout(int id)
	{
		yield return new WaitForEndOfFrame();
		string text = _E001() + getFileName(id);
		_E004();
		_E003();
		int num = (int)(cutoutPosition.x - cutoutSize.x / 2f);
		int num2 = (int)((float)Screen.height - cutoutPosition.y - cutoutSize.y / 2f);
		int num3 = (int)cutoutSize.x;
		int num4 = (int)cutoutSize.y;
		Texture2D texture2D = new Texture2D(num3, num4, TextureFormat.RGB24, mipChain: false);
		texture2D.name = _ED3E._E000(96783);
		texture2D.ReadPixels(new Rect(num, num2, num3, num4), 0, 0);
		texture2D.Apply();
		byte[] bytes = texture2D.EncodeToPNG();
		UnityEngine.Object.Destroy(texture2D);
		File.WriteAllBytes(text, bytes);
		Debug.Log(_ED3E._E000(97155) + num3 + _ED3E._E000(96989) + num4 + _ED3E._E000(97243) + num + _ED3E._E000(97232) + num2 + _ED3E._E000(97234) + text);
	}

	private void _E004()
	{
		if (cutoutSize.x <= 8f || cutoutSize.y <= 8f)
		{
			Debug.Log(_ED3E._E000(96825));
			if (Screen.width < 500 || Screen.height < 500)
			{
				Debug.Log(_ED3E._E000(96874));
				cutoutSize = new Vector2(500f, 500f);
				return;
			}
			Debug.Log(_ED3E._E000(96959) + Screen.width + _ED3E._E000(96989) + Screen.height + _ED3E._E000(96991));
			cutoutSize = new Vector2(Screen.width, Screen.height);
		}
	}

	public void CaptureScreenshots(int id, bool fallback)
	{
		if (singleCamera)
		{
			_E000(id);
		}
		string text = _E001() + getFileName(id);
		ScreenCapture.CaptureScreenshot(text, captureSizeMultiplier);
		if (fallback)
		{
			Debug.Log(_ED3E._E000(96976) + text);
		}
		else
		{
			Debug.Log(_ED3E._E000(97104) + text);
		}
	}

	public void CaptureRenderTexture(Camera attachedCam, int id)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].cam != null)
			{
				list[i].cam.SetActive(value: false);
			}
		}
		list[id].cam.SetActive(value: true);
		string text = _E001() + getFileName(id);
		int num = (int)((float)attachedCam.pixelWidth * renderSizeMultiplier);
		int num2 = (int)((float)attachedCam.pixelHeight * renderSizeMultiplier);
		RenderTexture renderTexture = new RenderTexture(num, num2, 24);
		renderTexture.name = _ED3E._E000(97150);
		attachedCam.targetTexture = renderTexture;
		Texture2D obj = new Texture2D(num, num2, TextureFormat.RGB24, mipChain: false)
		{
			name = _ED3E._E000(96783)
		};
		attachedCam.Render();
		RenderTexture.active = renderTexture;
		obj.ReadPixels(new Rect(0f, 0f, num, num2), 0, 0);
		attachedCam.targetTexture = null;
		RenderTexture.active = null;
		UnityEngine.Object.DestroyImmediate(renderTexture);
		byte[] bytes = obj.EncodeToPNG();
		File.WriteAllBytes(text, bytes);
		Debug.Log(_ED3E._E000(97104) + text);
	}

	public string getFileName(int camID)
	{
		string text = "";
		if (customName != "")
		{
			text += customName;
		}
		else
		{
			string[] array = Application.dataPath.Split(_ED3E._E000(30703)[0]);
			text += array[array.Length - 2];
		}
		if (includeCamName)
		{
			text += _ED3E._E000(48793);
			if (camID < 0 || camID >= list.Count || list[camID] == null || list[camID].cam == null)
			{
				text += _ED3E._E000(97131);
				lastCamID = 0;
			}
			else
			{
				text += list[camID].cam.name;
				lastCamID = camID;
			}
		}
		if (includeDate)
		{
			text += _ED3E._E000(48793);
			text += _E5AD.Now.ToString(_ED3E._E000(97182));
		}
		if (includeResolution)
		{
			text += _ED3E._E000(48793);
			text += getResolution();
		}
		if (fileType == FileType.JPG)
		{
			text += _ED3E._E000(97162);
		}
		else if (fileType == FileType.PNG)
		{
			text += _ED3E._E000(45670);
		}
		return text;
	}

	public string getResolution()
	{
		if (lastCam == null || list[lastCamID].cam != lastCam.gameObject)
		{
			if (list[lastCamID].cam != null)
			{
				lastCam = list[lastCamID].cam.GetComponentInChildren<Camera>();
			}
			else
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] != null && !(list[i].cam == null))
					{
						lastCam = list[i].cam.GetComponentInChildren<Camera>();
						if (lastCam != null)
						{
							break;
						}
					}
				}
			}
		}
		if (lastCam == null)
		{
			return _ED3E._E000(97159);
		}
		if (captureMethod == CaptureMethod.RenderTexture)
		{
			return (int)((float)lastCam.pixelWidth * renderSizeMultiplier) + _ED3E._E000(96989) + (int)((float)lastCam.pixelHeight * renderSizeMultiplier);
		}
		return lastCam.pixelWidth * captureSizeMultiplier + _ED3E._E000(96989) + lastCam.pixelHeight * captureSizeMultiplier;
	}
}
