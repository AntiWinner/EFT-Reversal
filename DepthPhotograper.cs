using System;
using Comfort.Common;
using UnityEngine;
using UnityEngine.Profiling;

public class DepthPhotograper : MonoBehaviour
{
	[SerializeField]
	private PowOfTwoDimensions _depthTextureDimension = PowOfTwoDimensions._4096;

	[SerializeField]
	private float _yBias;

	private float m__E000;

	private float m__E001;

	private Vector3 m__E002;

	private Camera _E003;

	private Material _E004;

	[SerializeField]
	private RenderTexture _depthRT;

	private RenderTexture _E005;

	private static readonly int _E006 = Shader.PropertyToID(_ED3E._E000(84878));

	private static readonly int _E007 = Shader.PropertyToID(_ED3E._E000(84864));

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(84922));

	private const float _E009 = 100f;

	private int _E00A = 11;

	private CustomSampler _E00B = CustomSampler.Create(_ED3E._E000(84839));

	private Bounds _E00C;

	private float _E00D => Math.Max(_E00C.size.x, _E00C.size.z);

	public void Start()
	{
		_E00C = Singleton<LevelSettings>.Instance.RainBounds;
	}

	[ContextMenu("Render")]
	public void Render()
	{
		_E00A = LayerMask.NameToLayer(_ED3E._E000(25436));
		_E004 = new Material(_E3AC.Find(_ED3E._E000(37897)));
		_depthRT = new RenderTexture((int)_depthTextureDimension, (int)_depthTextureDimension, 0, RenderTextureFormat.RFloat)
		{
			name = _ED3E._E000(84770),
			wrapMode = TextureWrapMode.Clamp
		};
		_E005 = new RenderTexture((int)_depthTextureDimension, (int)_depthTextureDimension, 16)
		{
			name = _ED3E._E000(84815),
			format = RenderTextureFormat.Depth,
			wrapMode = TextureWrapMode.Clamp,
			filterMode = FilterMode.Bilinear
		};
		_E000();
		_E001();
		_E002();
		UnityEngine.Object.DestroyImmediate(_E004);
		UnityEngine.Object.DestroyImmediate(_E003.gameObject);
		UnityEngine.Object.DestroyImmediate(_E005);
	}

	public bool CheckIfCameraUnderRain(Transform cameraTransform)
	{
		if (cameraTransform == null || WeatherObstacle.Instance == null || WeatherObstacle.Instance.MeshCollider == null)
		{
			return false;
		}
		Vector3 position = cameraTransform.position;
		Vector3 origin = new Vector3(position.x, position.y + 100f, position.z);
		Ray ray = new Ray(origin, Vector3.down);
		RaycastHit hitInfo;
		return !WeatherObstacle.Instance.MeshCollider.Raycast(ray, out hitInfo, 100f);
	}

	private void _E000()
	{
		GameObject gameObject = new GameObject(_ED3E._E000(84851));
		gameObject.transform.SetParent(base.transform);
		_E003 = gameObject.AddComponent<Camera>();
		_E003.enabled = false;
		_E003.aspect = 1f;
		_E003.orthographic = true;
		_E003.orthographicSize = 0.5f * _E00D;
		_E003.cullingMask = 1 << _E00A;
		_E003.nearClipPlane = 0.1f;
		_E003.farClipPlane = _E00C.size.y;
		_E003.useOcclusionCulling = false;
		_E003.clearFlags = CameraClearFlags.Color;
		_E003.renderingPath = RenderingPath.Forward;
		_E003.depthTextureMode = DepthTextureMode.Depth;
		_E003.targetTexture = _E005;
		_E003.transform.position = new Vector3(_E00C.center.x, _E00C.max.y, _E00C.center.z);
		_E003.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
	}

	private void _E001()
	{
		Terrain[] array = _E3AA.FindUnityObjectsOfType<Terrain>();
		bool[] array2 = new bool[array.Length];
		DisablerTerrainCullingObject[] array3 = new DisablerTerrainCullingObject[array.Length];
		float num = -1f;
		for (int i = 0; i < array.Length; i++)
		{
			Terrain terrain = array[i];
			array2[i] = terrain.drawHeightmap;
			array3[i] = terrain.GetComponent<DisablerTerrainCullingObject>();
			terrain.drawHeightmap = array3[i] != null || array2[i];
			terrain.transform.position += Vector3.up * num;
		}
		if (WeatherObstacle.Instance != null)
		{
			Graphics.DrawMesh(WeatherObstacle.Instance.MeshCollider.sharedMesh, Matrix4x4.identity, _E004, LayerMask.NameToLayer(_ED3E._E000(25436)));
		}
		_E003.Render();
		_E003.targetTexture = null;
		for (int j = 0; j < array.Length; j++)
		{
			Terrain obj = array[j];
			obj.drawHeightmap = array2[j];
			obj.transform.position -= Vector3.up * num;
		}
		RenderTexture active = RenderTexture.active;
		Graphics.Blit(_E005, _depthRT);
		RenderTexture.active = active;
	}

	private void _E002()
	{
		Vector3 vector = new Vector3(_E00C.min.x, _E00C.min.y + _yBias, _E00C.min.z);
		Vector3 vector2 = new Vector3(1f / _E00D, 1f / _E00C.size.y, 1f / _E00D);
		Shader.SetGlobalVector(_E006, vector);
		Shader.SetGlobalVector(_E007, vector2);
		Shader.SetGlobalTexture(_E008, _depthRT);
	}

	private void OnDestroy()
	{
		UnityEngine.Object.Destroy(_depthRT);
	}
}
