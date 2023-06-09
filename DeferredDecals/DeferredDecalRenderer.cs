using System;
using System.Collections.Generic;
using System.Linq;
using EFT.Ballistics;
using UnityEngine;
using UnityEngine.Rendering;

namespace DeferredDecals;

public class DeferredDecalRenderer : MonoBehaviour
{
	public class _E000 : IDisposable
	{
		public readonly int VertexCount;

		public readonly Mesh Mesh;

		public readonly Vector3[] Vertices;

		public readonly Vector3[] Normals;

		public readonly Vector4[] Tangents;

		public readonly List<Vector4> Uvs0;

		public readonly List<Vector4> Uvs1;

		public readonly List<Vector4> Uvs2;

		private static int[] m__E000;

		private static int _E001;

		private const int _E002 = 10000;

		public _E000(int nDecals)
		{
			int num = nDecals * 24;
			Vertices = new Vector3[num];
			Normals = new Vector3[num];
			Tangents = new Vector4[num];
			Uvs0 = new List<Vector4>(num);
			Uvs1 = new List<Vector4>(num);
			Uvs2 = new List<Vector4>(num);
			for (int i = 0; i < num; i++)
			{
				Uvs0.Add(_E38D.Vector4Zero);
				Uvs1.Add(_E38D.Vector4Zero);
				Uvs2.Add(_E38D.Vector4Zero);
			}
			if (m__E000 == null || _E001 != nDecals)
			{
				m__E000 = new int[nDecals * 36];
				DeferredDecalMeshHelper.GenerateTrigs(m__E000);
			}
			_E001 = nDecals;
			Mesh = new Mesh
			{
				name = _ED3E._E000(92920)
			};
			Mesh.MarkDynamic();
			UpdateInnerMesh();
			Mesh.triangles = m__E000;
			Mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 10000f);
		}

		public void UpdateInnerMesh()
		{
			Mesh.vertices = Vertices;
			Mesh.normals = Normals;
			Mesh.tangents = Tangents;
			Mesh.SetUVs(0, Uvs0);
			Mesh.SetUVs(1, Uvs1);
			Mesh.SetUVs(2, Uvs2);
		}

		public void PasteProjectorIntoMiddle(int startvIndex, Vector3[] newDecalVerts, Vector4[] newDecalTangents, Vector3[] newDecalNormals, Vector4[] decalUv0Right, Vector4[] decalUv1Up, Vector4[] decalUv2Fwd)
		{
			for (int i = startvIndex; i < startvIndex + 24; i++)
			{
				Vertices[i] = newDecalVerts[i - startvIndex];
				Tangents[i] = newDecalTangents[i - startvIndex];
				Normals[i] = newDecalNormals[i - startvIndex];
				Uvs0[i] = decalUv0Right[i - startvIndex];
				Uvs1[i] = decalUv1Up[i - startvIndex];
				Uvs2[i] = decalUv2Fwd[i - startvIndex];
			}
			UpdateInnerMesh();
		}

		public void Dispose()
		{
			UnityEngine.Object.DestroyImmediate(Mesh);
		}
	}

	[Serializable]
	public class SingleDecal
	{
		public bool RandomizeRotation;

		[_E2BD(0f, 2f, -1f)]
		public Vector2 DecalSize = new Vector2(0.1f, 0.15f);

		[Range(1f, 10f)]
		public int TileSheetRows = 1;

		[Range(1f, 10f)]
		public int TileSheetColumns = 1;

		public MaterialType[] DecalMaterialType;

		public Material DecalMaterial;

		public Material DynamicDecalMaterial;

		[HideInInspector]
		public float TileUSize;

		[HideInInspector]
		public float TileVSize;

		[HideInInspector]
		public bool IsTiled;

		public void Init()
		{
			TileUSize = 1f / (float)TileSheetColumns;
			TileVSize = 1f / (float)TileSheetRows;
			IsTiled = TileSheetColumns != 1 || TileSheetRows != 1;
		}
	}

	private class _E001
	{
		public CommandBuffer StaticDecalsBuffer;

		public CommandBuffer DynamicDecalsBuffer;

		public CullingGroup CullGroup;

		public bool IsStaticBufferDirty = true;

		public bool IsDynamicBufferDirty = true;
	}

	[SerializeField]
	[Range(0.001f, 3f)]
	private float _decalProjectorHeight = 0.5f;

	[Range(0.001f, 3f)]
	[SerializeField]
	private float _decalProjectorHeightForGrenade = 0.7f;

	[Range(1f, 1024f)]
	[SerializeField]
	private int _maxDecals = 500;

	[Range(1f, 1024f)]
	[SerializeField]
	private int _maxDynamicDecals = 500;

	[SerializeField]
	[Range(1f, 1000f)]
	private int _cullDistance = 100;

	[SerializeField]
	private SingleDecal[] _decals;

	[SerializeField]
	private SingleDecal _environmentBlood;

	[SerializeField]
	private SingleDecal _bleedingDecal;

	[SerializeField]
	private SingleDecal _grenadeDecal;

	[SerializeField]
	private SingleDecal _genericDecal;

	[SerializeField]
	private Mesh _cube;

	private readonly Queue<_E000> m__E000 = new Queue<_E000>();

	private readonly Dictionary<Material, _E000> m__E001 = new Dictionary<Material, _E000>();

	private readonly Dictionary<MaterialType, SingleDecal> m__E002 = _E3A5<MaterialType>.GetDictWith<SingleDecal>();

	private int m__E003;

	private int m__E004;

	private int m__E005;

	private Dictionary<Camera, _E001> m__E006 = new Dictionary<Camera, _E001>();

	private BoundingSphere[] m__E007;

	private int m__E008;

	private List<DynamicDeferredDecalRenderer> m__E009;

	private Vector4[] m__E00A;

	private Vector4[] m__E00B;

	private Vector4[] m__E00C;

	private Vector3[] m__E00D;

	private Vector3[] _E00E;

	private Vector4[] _E00F;

	private Transform _E010;

	private const float _E011 = 0.4f;

	private CommandBuffer _E012;

	private CommandBuffer _E013;

	private bool _E014;

	private void Awake()
	{
		this.m__E004 = 24 * _maxDecals;
		this.m__E005 = Shader.PropertyToID(_ED3E._E000(44962));
		_environmentBlood.Init();
		_bleedingDecal.Init();
		_genericDecal.Init();
		_grenadeDecal.Init();
		SingleDecal[] decals = _decals;
		foreach (SingleDecal singleDecal in decals)
		{
			singleDecal.Init();
			MaterialType[] decalMaterialType = singleDecal.DecalMaterialType;
			foreach (MaterialType key in decalMaterialType)
			{
				this.m__E002.Add(key, singleDecal);
			}
		}
		this.m__E009 = new List<DynamicDeferredDecalRenderer>(_maxDynamicDecals);
		this.m__E007 = new BoundingSphere[_maxDynamicDecals];
		this.m__E008 = 0;
		this.m__E00A = new Vector4[24];
		this.m__E00B = new Vector4[24];
		this.m__E00C = new Vector4[24];
		this.m__E00D = new Vector3[24];
		_E00E = new Vector3[24];
		_E00F = new Vector4[24];
		_E010 = new GameObject(_ED3E._E000(92826)).transform;
		_E010.position = Vector3.zero;
		_E010.rotation = Quaternion.identity;
		UnityEngine.Object.DontDestroyOnLoad(_E010);
		_E001(this.m__E002.Count);
		_E003();
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(OnPreCullCameraRender));
		Camera.onPreRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPreRender, new Camera.CameraCallback(OnPreCameraRender));
	}

	public void DrawDecal(RaycastHit hit, BallisticCollider hitCollider)
	{
		DrawDecal(hit.point, hit.normal, hitCollider, isGrenade: false);
	}

	public void SetMaxStaticDecals(int maxDecals)
	{
		_maxDecals = maxDecals;
		this.m__E004 = 24 * _maxDecals;
		Material[] array = this.m__E001.Keys.ToArray();
		foreach (Material key in array)
		{
			this.m__E001[key] = new _E000(this.m__E004);
		}
	}

	public void SetMaxDynamicDecals(int maxDecals)
	{
		_maxDynamicDecals = maxDecals;
		for (int i = 0; i < this.m__E009.Count; i++)
		{
			UnityEngine.Object.Destroy(this.m__E009[i].gameObject);
		}
		_E004();
		_E003();
	}

	public void EmitBloodOnEnvironment(Vector3 position, Vector3 normal)
	{
		if (!this.m__E001.ContainsKey(_environmentBlood.DecalMaterial))
		{
			foreach (KeyValuePair<Camera, _E001> item in this.m__E006)
			{
				item.Value.IsStaticBufferDirty = true;
			}
			_E007(_environmentBlood);
		}
		_E006(position, normal, this.m__E001[_environmentBlood.DecalMaterial], _environmentBlood, _decalProjectorHeight);
	}

	public void EmitBleeding(Vector3 position, Vector3 normal)
	{
		if (!this.m__E001.ContainsKey(_bleedingDecal.DecalMaterial))
		{
			foreach (KeyValuePair<Camera, _E001> item in this.m__E006)
			{
				item.Value.IsStaticBufferDirty = true;
			}
			_E007(_bleedingDecal);
		}
		_E006(position, normal, this.m__E001[_bleedingDecal.DecalMaterial], _bleedingDecal, _decalProjectorHeight);
	}

	public void DrawDecal(Vector3 position, Vector3 normal, BallisticCollider hitCollider, bool isGrenade)
	{
		bool num = hitCollider == null || hitCollider.gameObject.isStatic;
		SingleDecal singleDecal = _E000(hitCollider, isGrenade);
		float projectorHeight = (isGrenade ? _decalProjectorHeightForGrenade : _decalProjectorHeight);
		if (num)
		{
			if (!this.m__E001.ContainsKey(singleDecal.DecalMaterial))
			{
				_E007(singleDecal);
				foreach (KeyValuePair<Camera, _E001> item in this.m__E006)
				{
					item.Value.IsStaticBufferDirty = true;
				}
			}
			_E006(position, normal, this.m__E001[singleDecal.DecalMaterial], singleDecal, projectorHeight);
			return;
		}
		_E005(position, normal, hitCollider, singleDecal, singleDecal.DynamicDecalMaterial, projectorHeight);
		foreach (KeyValuePair<Camera, _E001> item2 in this.m__E006)
		{
			item2.Value.IsDynamicBufferDirty = true;
		}
	}

	private SingleDecal _E000(BallisticCollider hitCollider, bool isGrenade)
	{
		if (isGrenade)
		{
			return _grenadeDecal;
		}
		MaterialType key = ((!(hitCollider == null)) ? hitCollider.TypeOfMaterial : MaterialType.None);
		if (!this.m__E002.ContainsKey(key))
		{
			return _genericDecal;
		}
		return this.m__E002[key];
	}

	private void _E001(int size)
	{
		for (int i = 0; i < size; i++)
		{
			this.m__E000.Enqueue(new _E000(_maxDecals));
		}
	}

	private _E000 _E002()
	{
		if (this.m__E000.Count == 0)
		{
			this.m__E000.Enqueue(new _E000(_maxDecals));
		}
		return this.m__E000.Dequeue();
	}

	private void _E003()
	{
		foreach (KeyValuePair<Camera, _E001> item in this.m__E006)
		{
			item.Value.CullGroup.SetBoundingSphereCount(_maxDynamicDecals);
		}
		for (int i = 0; i < _maxDynamicDecals; i++)
		{
			GameObject obj = new GameObject
			{
				name = _ED3E._E000(92800) + i
			};
			UnityEngine.Object.DontDestroyOnLoad(obj);
			DynamicDeferredDecalRenderer dynamicDeferredDecalRenderer = obj.AddComponent<DynamicDeferredDecalRenderer>();
			GameObject gameObject = new GameObject
			{
				name = _ED3E._E000(92853) + i
			};
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			dynamicDeferredDecalRenderer.enabled = false;
			dynamicDeferredDecalRenderer.GameObjectHelper = gameObject;
			dynamicDeferredDecalRenderer.TransformHelper = gameObject.transform;
			obj.transform.parent = _E010;
			dynamicDeferredDecalRenderer.TransformHelper.parent = _E010;
			this.m__E009.Add(dynamicDeferredDecalRenderer);
		}
	}

	private void _E004()
	{
		if (this.m__E009 == null)
		{
			return;
		}
		for (int i = 0; i < this.m__E009.Count; i++)
		{
			if (!(this.m__E009[i] == null))
			{
				if (this.m__E009[i].gameObject != null)
				{
					UnityEngine.Object.DestroyImmediate(this.m__E009[i].gameObject);
				}
				if (this.m__E009[i].GameObjectHelper != null)
				{
					UnityEngine.Object.DestroyImmediate(this.m__E009[i].GameObjectHelper);
				}
			}
		}
		this.m__E009.Clear();
	}

	private void _E005(Vector3 position, Vector3 normal, BallisticCollider hitCollider, SingleDecal currentDecal, Material currentMaterial, float projectorHeight)
	{
		DynamicDeferredDecalRenderer dynamicDeferredDecalRenderer = this.m__E009[this.m__E008];
		GameObject gameObject = dynamicDeferredDecalRenderer.gameObject;
		Transform transformHelper = dynamicDeferredDecalRenderer.TransformHelper;
		int cullingGroupSphereIndex = dynamicDeferredDecalRenderer.CullingGroupSphereIndex;
		float num = UnityEngine.Random.Range(currentDecal.DecalSize.x, currentDecal.DecalSize.y);
		float num2 = num * 2f;
		float rad = Mathf.Sqrt(num * num + num * num);
		this.m__E007[cullingGroupSphereIndex] = new BoundingSphere(position, rad);
		if (gameObject != null)
		{
			gameObject.transform.localScale = new Vector3(num2, projectorHeight, num2);
			gameObject.transform.up = normal;
			gameObject.transform.position = position;
			if (currentDecal.RandomizeRotation)
			{
				gameObject.transform.Rotate(Vector3.up, UnityEngine.Random.Range(0f, 359f), Space.Self);
			}
			if (transformHelper != null)
			{
				transformHelper.position = position;
				transformHelper.rotation = gameObject.transform.rotation;
				transformHelper.parent = hitCollider.transform;
			}
		}
		int num3 = UnityEngine.Random.Range(0, currentDecal.TileSheetColumns);
		int num4 = UnityEngine.Random.Range(0, currentDecal.TileSheetRows);
		Vector4 uvStartEnd = new Vector4((float)num4 * currentDecal.TileUSize, (float)num3 * currentDecal.TileVSize, (float)num4 * currentDecal.TileUSize + currentDecal.TileUSize, (float)num3 * currentDecal.TileVSize + currentDecal.TileVSize);
		dynamicDeferredDecalRenderer.enabled = true;
		dynamicDeferredDecalRenderer.Init(currentMaterial, _cube, normal, uvStartEnd, currentDecal.IsTiled, cullingGroupSphereIndex);
		this.m__E008 = (this.m__E008 + 1) % _maxDynamicDecals;
	}

	private void _E006(Vector3 position, Vector3 normal, _E000 mesh, SingleDecal decal, float projectorHeight)
	{
		float value = UnityEngine.Random.Range(decal.DecalSize.x, decal.DecalSize.y);
		value = Mathf.Clamp(value, 0f, 0.4f);
		Vector3 vector = new Vector3(value, projectorHeight, value);
		Vector3 normalized = normal.normalized;
		float num = Mathf.Abs(Vector3.Dot(normalized, Vector3.up));
		Vector3 vector2 = Vector3.Cross(normalized, (num > 0.999f) ? Vector3.right : Vector3.up).normalized;
		Vector3 vector3 = Vector3.Cross(normalized, vector2).normalized;
		if (decal.RandomizeRotation)
		{
			Quaternion quaternion = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 359), normalized);
			vector2 = quaternion * vector2;
			vector3 = quaternion * vector3;
		}
		this.m__E00D = DeferredDecalMeshHelper.GenerateVerts(this.m__E00D, position, vector, vector2, normalized, vector3);
		_E00F = DeferredDecalMeshHelper.GenerateTangents(_E00F, decal);
		_E00E.FillWith(vector);
		this.m__E00A.FillWith(new Vector4(vector2.x, vector2.y, vector2.z, position.x));
		this.m__E00B.FillWith(new Vector4(normalized.x, normalized.y, normalized.z, position.y));
		this.m__E00C.FillWith(new Vector4(vector3.x, vector3.y, vector3.z, position.z));
		mesh.PasteProjectorIntoMiddle(this.m__E003 * 24, this.m__E00D, _E00F, _E00E, this.m__E00A, this.m__E00B, this.m__E00C);
		this.m__E003 = (this.m__E003 + 1) % _maxDecals;
	}

	private void _E007(SingleDecal decal)
	{
		decal.TileUSize = 1f / (float)decal.TileSheetColumns;
		decal.TileVSize = 1f / (float)decal.TileSheetRows;
		this.m__E001.Add(decal.DecalMaterial, _E002());
	}

	public void OnDisable()
	{
		_E008();
	}

	private void _E008()
	{
		foreach (KeyValuePair<Camera, _E001> item in this.m__E006)
		{
			if (item.Key != null)
			{
				item.Key.RemoveCommandBuffer(CameraEvent.BeforeLighting, item.Value.StaticDecalsBuffer);
				item.Key.RemoveCommandBuffer(CameraEvent.BeforeLighting, item.Value.DynamicDecalsBuffer);
			}
			item.Value.CullGroup.Dispose();
			item.Value.CullGroup = null;
		}
		this.m__E006.Clear();
	}

	public void OnPreCullCameraRender(Camera currentCamera)
	{
		if (_E00C(currentCamera))
		{
			_E012 = null;
			_E013 = null;
			_E014 = false;
			if (this.m__E006.ContainsKey(currentCamera))
			{
				_E012 = this.m__E006[currentCamera].StaticDecalsBuffer;
				_E013 = this.m__E006[currentCamera].DynamicDecalsBuffer;
			}
			else
			{
				_E012 = new CommandBuffer();
				_E012.name = _ED3E._E000(92841);
				currentCamera.AddCommandBuffer(CameraEvent.BeforeLighting, _E012);
				_E013 = new CommandBuffer();
				_E013.name = _ED3E._E000(92880);
				currentCamera.AddCommandBuffer(CameraEvent.BeforeLighting, _E013);
				_E014 = true;
				this.m__E006.Add(currentCamera, new _E001
				{
					StaticDecalsBuffer = _E012,
					DynamicDecalsBuffer = _E013,
					CullGroup = new CullingGroup()
				});
				this.m__E006[currentCamera].CullGroup.targetCamera = currentCamera;
				this.m__E006[currentCamera].CullGroup.SetBoundingSpheres(this.m__E007);
			}
			if (this.m__E006[currentCamera].IsStaticBufferDirty || _E014)
			{
				this.m__E006[currentCamera].IsStaticBufferDirty = false;
				_E009(_E012, currentCamera, _E00B);
			}
		}
	}

	public void OnPreCameraRender(Camera currentCamera)
	{
		if (_E00C(currentCamera) && this.m__E006.ContainsKey(currentCamera) && (this.m__E006[currentCamera].IsDynamicBufferDirty || _E014))
		{
			this.m__E006[currentCamera].IsDynamicBufferDirty = false;
			_E009(_E013, currentCamera, _E00A);
		}
	}

	private void _E009(CommandBuffer buffer, Camera currentCamera, Action<Camera, CommandBuffer> drawFunc)
	{
		buffer.Clear();
		buffer.GetTemporaryRT(this.m__E005, -1, -1);
		buffer.Blit(BuiltinRenderTextureType.GBuffer2, this.m__E005);
		buffer.SetRenderTarget(new RenderTargetIdentifier[4]
		{
			BuiltinRenderTextureType.GBuffer0,
			BuiltinRenderTextureType.GBuffer1,
			BuiltinRenderTextureType.GBuffer2,
			currentCamera.allowHDR ? BuiltinRenderTextureType.CameraTarget : BuiltinRenderTextureType.GBuffer3
		}, BuiltinRenderTextureType.CameraTarget);
		drawFunc(currentCamera, buffer);
		buffer.ReleaseTemporaryRT(this.m__E005);
	}

	private void Update()
	{
		bool flag = false;
		for (int i = 0; i < this.m__E009.Count; i++)
		{
			if (this.m__E009[i].enabled && this.m__E009[i].ManualUpdate())
			{
				flag = true;
			}
		}
		if (flag)
		{
			_E00D();
		}
	}

	private void _E00A(Camera currentCamera, CommandBuffer buffer)
	{
		for (int i = 0; i < this.m__E009.Count; i++)
		{
			if (this.m__E009[i].enabled && this.m__E006[currentCamera].CullGroup.GetDistance(this.m__E009[i].CullingGroupSphereIndex) < _cullDistance)
			{
				buffer.DrawMesh(_cube, this.m__E009[i].ModelMatrix, this.m__E009[i].DecalMaterial);
			}
		}
	}

	private void _E00B(Camera currentCamera, CommandBuffer buffer)
	{
		foreach (KeyValuePair<Material, _E000> item in this.m__E001)
		{
			buffer.DrawMesh(item.Value.Mesh, Matrix4x4.identity, item.Key);
		}
	}

	private bool _E00C(Camera currentCamera)
	{
		if (currentCamera == null || !EFTHardSettings.Instance.DEFERRED_DECALS_ENABLED)
		{
			return false;
		}
		if (!currentCamera.isActiveAndEnabled)
		{
			return false;
		}
		bool flag = currentCamera.CompareTag(_ED3E._E000(42129));
		if (currentCamera.renderingPath != RenderingPath.DeferredShading && !flag)
		{
			return false;
		}
		if (!((_E8A8.Instance.Camera != null && _E8A8.Instance.Camera == currentCamera) || flag))
		{
			return false;
		}
		if (this.m__E001.Count == 0 && this.m__E009.Count == 0)
		{
			return false;
		}
		return true;
	}

	private void _E00D()
	{
		foreach (KeyValuePair<Camera, _E001> item in this.m__E006)
		{
			item.Value.IsDynamicBufferDirty = true;
		}
	}

	private void OnDestroy()
	{
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(OnPreCullCameraRender));
		Camera.onPreRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPreRender, new Camera.CameraCallback(OnPreCameraRender));
		_E008();
		_E004();
		foreach (_E000 item in this.m__E000)
		{
			item.Dispose();
		}
		this.m__E000.Clear();
		foreach (_E000 value in this.m__E001.Values)
		{
			value.Dispose();
		}
		this.m__E001.Clear();
		if (_E010 != null)
		{
			UnityEngine.Object.DestroyImmediate(_E010.gameObject);
		}
	}
}
