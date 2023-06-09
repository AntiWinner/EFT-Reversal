using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using EFT;
using EFT.UI;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Screenshot360 : MonoBehaviour
{
	private class _E000
	{
		public GameObject HiddenGameObject;

		public bool IsActive;

		public _E000(GameObject obj)
		{
			HiddenGameObject = obj;
			if (HiddenGameObject != null)
			{
				IsActive = HiddenGameObject.activeSelf;
				HiddenGameObject.SetActive(value: false);
			}
		}

		public void AfterScreenshot()
		{
			HiddenGameObject?.SetActive(IsActive);
		}
	}

	public string _saveToFilePath = "";

	private RenderTexture m__E000;

	private Camera m__E001;

	private static List<_E000> m__E002 = new List<_E000>();

	private void _E000()
	{
		PrismEffects component = GetComponent<PrismEffects>();
		if ((bool)component)
		{
			component.useVignette = false;
		}
		CC_ContrastVignette component2 = GetComponent<CC_ContrastVignette>();
		if ((bool)component2)
		{
			component2.enabled = false;
		}
		ChromaticAberration component3 = GetComponent<ChromaticAberration>();
		if ((bool)component3)
		{
			component3.Shift = 0f;
		}
		SSAA component4 = GetComponent<SSAA>();
		if ((bool)component4)
		{
			component4.enabled = false;
		}
	}

	public void Setup(int resolution, string saveToFile, Vector3 direction, Vector3 up, int cameraDepth, PrismEffects masterPrismEffect, PostProcessLayer ppLayer)
	{
		_E000();
		_saveToFilePath = saveToFile;
		this.m__E001 = GetComponent<Camera>();
		this.m__E000 = new RenderTexture(resolution, resolution, 32, RenderTextureFormat.ARGB32);
		this.m__E001.targetTexture = this.m__E000;
		this.m__E001.fieldOfView = 90f;
		this.m__E001.transform.LookAt(this.m__E001.transform.position + direction, up);
		this.m__E001.depth = cameraDepth;
		if (masterPrismEffect != null && masterPrismEffect.useExposure)
		{
			PrismEffects component = GetComponent<PrismEffects>();
			if (component != null && component.useExposure)
			{
				masterPrismEffect.AddDependantEffectExposure(component);
			}
		}
		if (!(ppLayer != null))
		{
			return;
		}
		PostProcessBundle bundle = ppLayer.GetBundle<AutoExposure>();
		if (bundle != null && bundle.settings.enabled.value)
		{
			PostProcessLayer component2 = GetComponent<PostProcessLayer>();
			if (component2 != null)
			{
				ppLayer.AddDependedPostProcessExposure(component2);
			}
		}
	}

	public void ReadAndStoreResults()
	{
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = this.m__E000;
		int width = this.m__E000.width;
		Texture2D texture2D = new Texture2D(width, width, TextureFormat.RGB24, mipChain: false);
		texture2D.ReadPixels(new Rect(0f, 0f, width, width), 0, 0);
		texture2D.Apply();
		RenderTexture.active = null;
		this.m__E001.targetTexture = null;
		byte[] bytes = texture2D.EncodeToPNG();
		new FileInfo(_saveToFilePath).Directory.Create();
		File.WriteAllBytes(_saveToFilePath, bytes);
		RenderTexture.active = active;
		if (Application.isPlaying)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
	}

	private static IEnumerator _E001(Screenshot360[] comp360, int counter)
	{
		yield return new WaitForEndOfFrame();
		if (counter > 0)
		{
			comp360[0].StartCoroutine(_E001(comp360, counter - 1));
			yield break;
		}
		for (int i = 0; i < 6; i++)
		{
			comp360[i].ReadAndStoreResults();
		}
		_E006();
	}

	private static Camera _E002()
	{
		GameObject gameObject = GameObject.Find(_ED3E._E000(47354));
		if (gameObject == null)
		{
			Debug.LogError(_ED3E._E000(47341));
			return null;
		}
		return gameObject.GetComponent<Camera>();
	}

	public static void Make360Screenshot(int cubemapSize, bool alignToHorizon)
	{
		Camera camera = _E002();
		if (camera == null)
		{
			return;
		}
		GameObject[] array = new GameObject[6]
		{
			UnityEngine.Object.Instantiate(camera.gameObject),
			UnityEngine.Object.Instantiate(camera.gameObject),
			UnityEngine.Object.Instantiate(camera.gameObject),
			UnityEngine.Object.Instantiate(camera.gameObject),
			UnityEngine.Object.Instantiate(camera.gameObject),
			UnityEngine.Object.Instantiate(camera.gameObject)
		};
		Screenshot360[] array2 = new Screenshot360[6];
		for (int i = 0; i < 6; i++)
		{
			array2[i] = array[i].AddComponent<Screenshot360>();
		}
		Vector3 vector = camera.transform.forward;
		Vector3 vector2 = camera.transform.right;
		Vector3 vector3 = camera.transform.up;
		if (alignToHorizon)
		{
			vector = new Vector3(vector.x, 0f, vector.z).normalized;
			vector2 = new Vector3(vector2.x, 0f, vector2.z).normalized;
			vector3 = Vector3.Cross(vector, vector2);
		}
		Debug.Log(_ED3E._E000(47421) + Application.dataPath + _ED3E._E000(47446));
		array2[0].Setup(cubemapSize, Application.dataPath + _ED3E._E000(47425), vector, camera.transform.up, -100, null, null);
		PrismEffects component = array[0].GetComponent<PrismEffects>();
		PostProcessLayer component2 = array[0].GetComponent<PostProcessLayer>();
		if (component2 != null)
		{
			PostProcessBundle bundle = component2.GetBundle<AutoExposure>();
			if (bundle != null && bundle.settings.enabled.value)
			{
				component2.storeLastExposureTexture = true;
			}
		}
		array2[1].Setup(cubemapSize, Application.dataPath + _ED3E._E000(47466), vector2, vector3, -99, component, component2);
		array2[2].Setup(cubemapSize, Application.dataPath + _ED3E._E000(47505), -vector2, vector3, -98, component, component2);
		array2[3].Setup(cubemapSize, Application.dataPath + _ED3E._E000(47551), -vector, vector3, -97, component, component2);
		array2[4].Setup(cubemapSize, Application.dataPath + _ED3E._E000(47525), vector3, -vector, -96, component, component2);
		array2[5].Setup(cubemapSize, Application.dataPath + _ED3E._E000(47569), -vector3, vector, -95, component, component2);
		_E005();
		array2[0].StartCoroutine(_E001(array2, 2));
	}

	private static void _E003(Type monoBehaviourType)
	{
		UnityEngine.Object @object = UnityEngine.Object.FindObjectOfType(monoBehaviourType);
		if (@object != null)
		{
			Screenshot360.m__E002.Add(new _E000((@object as MonoBehaviour).gameObject));
		}
	}

	private static void _E004(GameObject go)
	{
		Screenshot360.m__E002.Add(new _E000(go));
	}

	private static void _E005()
	{
		Player player = UnityEngine.Object.FindObjectOfType<Player>();
		if (player != null)
		{
			_E004(player.HandsController.WeaponRoot.gameObject);
		}
		_E003(typeof(SimpleCharacterController));
		_E003(typeof(BattleUIScreen));
		_E003(typeof(ExtractionTimersPanel));
	}

	private static void _E006()
	{
		foreach (_E000 item in Screenshot360.m__E002)
		{
			item.AfterScreenshot();
		}
		Screenshot360.m__E002.Clear();
	}
}
