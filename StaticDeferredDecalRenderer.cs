using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Comfort.Common;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
[_E2E2(20010)]
public class StaticDeferredDecalRenderer : MonoBehaviour
{
	public class _E000
	{
		public string Hash;

		public Material ParentMaterial;

		public Material OwnMaterial;

		public int DecalQueue;

		public List<StaticDeferredDecal> Decals;

		public Bounds Bounds;

		public _E000(string hash, Material decalMaterial, int decalQueue, List<StaticDeferredDecal> decals, Bounds initialBounds)
		{
			Hash = hash;
			ParentMaterial = decalMaterial;
			OwnMaterial = new Material(ParentMaterial);
			DecalQueue = decalQueue;
			Decals = decals;
			Bounds = initialBounds;
		}
	}

	private class _E001
	{
		public CommandBuffer BeginCB;

		public CommandBuffer EndCB;

		public HashSet<CommandBuffer> DrawCBs = new HashSet<CommandBuffer>();
	}

	public int DecalsGridSize = 25;

	private _E7DD m__E000 = new _E7DD();

	public bool UseQuadTreeRenderInEditor;

	public bool UseQuadTreeRenderInPlayMode;

	public bool UseTexturesQuadTreeInEditor;

	public bool UseTexturesQuadTreeInPlayMode = true;

	public bool QuadTreeRecordPerFrame;

	public bool UseTreeAsync = true;

	private bool m__E001;

	private bool m__E002;

	public int TreeCmdBufMaxDepth = 5;

	public int TreeTexturesMaxDepth = 10;

	public int TreeThreadSleepTime = 100;

	private float m__E003 = 1f;

	private float m__E004 = 1f;

	[CompilerGenerated]
	private static StaticDeferredDecalRenderer m__E005;

	private int m__E006;

	private const int m__E007 = 60000;

	private int m__E008;

	private bool m__E009;

	private readonly _E373<string, _E000> m__E00A = new _E373<string, _E000>();

	private readonly SortedList<string, _E404> m__E00B = new SortedList<string, _E404>();

	private readonly Dictionary<Camera, _E001> m__E00C = new Dictionary<Camera, _E001>();

	private static readonly int m__E00D = Shader.PropertyToID(_ED3E._E000(44835));

	private static readonly int m__E00E = Shader.PropertyToID(_ED3E._E000(44970));

	private Mesh m__E00F;

	private _E405 m__E010;

	private Dictionary<Texture2D, _E408> m__E011;

	private List<_E408> m__E012;

	public int UpdateTexturesPerFrame = 10;

	private int m__E013 = -1;

	private static readonly int[] m__E014 = new int[36]
	{
		3, 1, 0, 3, 2, 1, 7, 5, 4, 7,
		6, 5, 11, 9, 8, 11, 10, 9, 15, 13,
		12, 15, 14, 13, 19, 17, 16, 19, 18, 17,
		23, 21, 20, 23, 22, 21
	};

	private static readonly Vector3[] m__E015 = new Vector3[24]
	{
		new Vector3(-0.5f, -0.5f, 0.5f),
		new Vector3(0.5f, -0.5f, 0.5f),
		new Vector3(0.5f, -0.5f, -0.5f),
		new Vector3(-0.5f, -0.5f, -0.5f),
		new Vector3(-0.5f, 0.5f, -0.5f),
		new Vector3(-0.5f, 0.5f, 0.5f),
		new Vector3(-0.5f, -0.5f, 0.5f),
		new Vector3(-0.5f, -0.5f, -0.5f),
		new Vector3(-0.5f, 0.5f, 0.5f),
		new Vector3(0.5f, 0.5f, 0.5f),
		new Vector3(0.5f, -0.5f, 0.5f),
		new Vector3(-0.5f, -0.5f, 0.5f),
		new Vector3(0.5f, 0.5f, -0.5f),
		new Vector3(-0.5f, 0.5f, -0.5f),
		new Vector3(-0.5f, -0.5f, -0.5f),
		new Vector3(0.5f, -0.5f, -0.5f),
		new Vector3(0.5f, 0.5f, 0.5f),
		new Vector3(0.5f, 0.5f, -0.5f),
		new Vector3(0.5f, -0.5f, -0.5f),
		new Vector3(0.5f, -0.5f, 0.5f),
		new Vector3(-0.5f, 0.5f, -0.5f),
		new Vector3(0.5f, 0.5f, -0.5f),
		new Vector3(0.5f, 0.5f, 0.5f),
		new Vector3(-0.5f, 0.5f, 0.5f)
	};

	private readonly RenderTargetIdentifier[] m__E016 = new RenderTargetIdentifier[4]
	{
		BuiltinRenderTextureType.GBuffer0,
		BuiltinRenderTextureType.GBuffer1,
		BuiltinRenderTextureType.GBuffer2,
		BuiltinRenderTextureType.CameraTarget
	};

	private static readonly int m__E017 = Shader.PropertyToID(_ED3E._E000(44962));

	private Vector4[] m__E018 = new Vector4[6];

	private Plane[] m__E019 = new Plane[6];

	private Vector3[] _E01A = new Vector3[8];

	public static StaticDeferredDecalRenderer Instance
	{
		[CompilerGenerated]
		get
		{
			return StaticDeferredDecalRenderer.m__E005;
		}
		[CompilerGenerated]
		private set
		{
			StaticDeferredDecalRenderer.m__E005 = value;
		}
	}

	private void Awake()
	{
		this.m__E008 = 0;
		_E001();
		_E00B();
		_E00D();
		_E00A();
		_E019();
		this.m__E006 = DecalsGridSize;
		if (Application.isPlaying)
		{
			if (Singleton<_E7DE>.Instantiated)
			{
				bool flag = Singleton<_E7DE>.Instance.Graphics.Settings.MipStreaming;
				UseTexturesQuadTreeInPlayMode &= flag;
			}
			this.m__E001 = UseQuadTreeRenderInPlayMode;
			this.m__E002 = UseTexturesQuadTreeInPlayMode;
		}
		else
		{
			this.m__E001 = UseQuadTreeRenderInEditor;
			this.m__E002 = UseTexturesQuadTreeInEditor;
		}
		Instance = this;
	}

	private void LateUpdate()
	{
		bool flag = (Application.isPlaying ? UseQuadTreeRenderInPlayMode : UseQuadTreeRenderInEditor);
		bool flag2 = (Application.isPlaying ? UseTexturesQuadTreeInPlayMode : UseTexturesQuadTreeInEditor);
		if (DecalsGridSize != this.m__E006 || flag != this.m__E001 || flag2 != this.m__E002)
		{
			OnDestroy();
			Awake();
			Refresh();
		}
		_ = this.m__E009;
	}

	private void _E000()
	{
		this.m__E010?.Dispose();
		this.m__E010 = null;
		if (this.m__E012 != null)
		{
			this.m__E012.Clear();
			this.m__E012 = null;
		}
		if (this.m__E011 == null)
		{
			return;
		}
		foreach (KeyValuePair<Texture2D, _E408> item in this.m__E011)
		{
			item.Value.Dispose();
		}
		this.m__E011.Clear();
		this.m__E011 = null;
	}

	private void OnDestroy()
	{
		_E00C();
		_E00E();
		_E015();
		_E000();
		_E017();
		_E003();
	}

	private void _E001()
	{
		if (this.m__E00F != null)
		{
			UnityEngine.Object.DestroyImmediate(this.m__E00F);
		}
		this.m__E00F = new Mesh
		{
			name = _ED3E._E000(44910)
		};
		this.m__E00F.vertices = StaticDeferredDecalRenderer.m__E015;
		this.m__E00F.triangles = StaticDeferredDecalRenderer.m__E014;
	}

	private Mesh _E002()
	{
		if (this.m__E00F == null)
		{
			_E001();
		}
		return this.m__E00F;
	}

	private void _E003()
	{
		if (this.m__E00F != null)
		{
			UnityEngine.Object.DestroyImmediate(this.m__E00F);
		}
	}

	private void _E004(StaticDeferredDecal decal, bool updateInstances)
	{
		if (this.m__E008 < 60000 && decal.enabled)
		{
			_E008(decal, updateInstances);
		}
	}

	private void _E005(StaticDeferredDecal decal, bool updateInstances)
	{
		_E009(decal, updateInstances);
	}

	private string _E006(StaticDeferredDecal decal)
	{
		string text = decal.DecalQueue.ToString();
		Texture mainTexture = decal.DecalMaterial.mainTexture;
		if ((bool)mainTexture)
		{
			text = text + mainTexture.width + mainTexture.height + (mainTexture as Texture2D).format;
		}
		if (decal.DecalMaterial.HasProperty(StaticDeferredDecalRenderer.m__E00D))
		{
			Texture texture = decal.DecalMaterial.GetTexture(StaticDeferredDecalRenderer.m__E00D);
			if ((bool)texture)
			{
				text = text + texture.width + texture.height + (texture as Texture2D).format;
			}
		}
		return text;
	}

	private void _E007(StaticDeferredDecal decal, ref Vector3 minPoint, ref Vector3 maxPoint)
	{
		Vector3[] vertices = _E002().vertices;
		Matrix4x4 localToWorldMatrix = decal.transform.localToWorldMatrix;
		for (int i = 0; i < vertices.Length; i++)
		{
			vertices[i] = localToWorldMatrix.MultiplyPoint(vertices[i]);
		}
		Vector3 vector = vertices[0];
		Vector3 vector2 = vertices[0];
		Vector3[] array = vertices;
		foreach (Vector3 rhs in array)
		{
			vector = Vector3.Min(vector, rhs);
			vector2 = Vector3.Max(vector2, rhs);
		}
		minPoint = vector;
		maxPoint = vector2;
	}

	private void _E008(StaticDeferredDecal decal, bool updateInstances)
	{
		string text = _E006(decal);
		Vector3 minPoint = Vector3.zero;
		Vector3 maxPoint = Vector3.zero;
		_E007(decal, ref minPoint, ref maxPoint);
		if (!this.m__E00A.ContainsKey(text))
		{
			Vector3 center = (maxPoint + minPoint) * 0.5f;
			Vector3 size = maxPoint - minPoint;
			_E000 value = new _E000(text, decal.DecalMaterial, decal.DecalQueue, new List<StaticDeferredDecal>(), new Bounds(center, size));
			this.m__E00A.Add(text, value);
		}
		_E000 byKey = this.m__E00A.GetByKey(text);
		List<StaticDeferredDecal> decals = byKey.Decals;
		if (!decals.Contains(decal))
		{
			decals.Add(decal);
			byKey.Bounds.Encapsulate(minPoint);
			byKey.Bounds.Encapsulate(maxPoint);
			this.m__E008++;
			if (updateInstances)
			{
				this.m__E009 = true;
			}
		}
	}

	private void _E009(StaticDeferredDecal decal, bool updateInstances)
	{
		string key = _E006(decal);
		if (!this.m__E00A.ContainsKey(key))
		{
			return;
		}
		_E000 byKey = this.m__E00A.GetByKey(key);
		List<StaticDeferredDecal> decals = this.m__E00A.GetByKey(key).Decals;
		if (!decals.Contains(decal))
		{
			return;
		}
		decals.Remove(decal);
		this.m__E008--;
		if (decals.Count == 0)
		{
			this.m__E00A.Remove(key);
			if (this.m__E00B.ContainsKey(key))
			{
				this.m__E00B[key].DestroyResources();
				this.m__E00B.Remove(key);
			}
			byKey.Bounds.SetMinMax(Vector3.zero, Vector3.zero);
		}
		else
		{
			Vector3 minPoint = Vector3.zero;
			Vector3 maxPoint = Vector3.zero;
			_E007(decals[0], ref minPoint, ref maxPoint);
			byKey.Bounds = new Bounds((maxPoint + minPoint) * 0.5f, maxPoint - minPoint);
			foreach (StaticDeferredDecal decal2 in byKey.Decals)
			{
				_E007(decal2, ref minPoint, ref maxPoint);
				byKey.Bounds.Encapsulate(minPoint);
				byKey.Bounds.Encapsulate(maxPoint);
			}
		}
		if (updateInstances)
		{
			this.m__E009 = true;
		}
	}

	public _E404[] GetDecalDrawInstancesArray()
	{
		_E404[] array = new _E404[this.m__E00B.Count];
		int num = 0;
		foreach (KeyValuePair<string, _E404> item in this.m__E00B)
		{
			array[num] = item.Value;
			num++;
		}
		return array;
	}

	private void _E00A()
	{
		StaticDeferredDecal[] array = _E3AA.FindUnityObjectsOfType<StaticDeferredDecal>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].DecalMaterial != null)
			{
				_E004(array[i], updateInstances: false);
			}
		}
	}

	private void _E00B()
	{
		StaticDeferredDecal.OnDecalRegister += _E004;
		StaticDeferredDecal.OnDecalUnregister += _E005;
	}

	private void _E00C()
	{
		StaticDeferredDecal.OnDecalRegister -= _E004;
		StaticDeferredDecal.OnDecalUnregister -= _E005;
	}

	private void _E00D()
	{
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(_E00F));
	}

	private void _E00E()
	{
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(_E00F));
	}

	private void _E00F(Camera currentCamera)
	{
		if (this.m__E00B.Count == 0 || currentCamera == null || !EFTHardSettings.Instance.STATIC_DEFERRED_DECALS_ENABLED)
		{
			return;
		}
		bool flag = currentCamera.CompareTag(_ED3E._E000(42129));
		if (currentCamera.CompareTag(_ED3E._E000(42407)) || flag || !Application.isPlaying)
		{
			_E016(currentCamera);
			if (!this.m__E00C.ContainsKey(currentCamera))
			{
				_E010(currentCamera);
			}
			_E013(currentCamera);
		}
	}

	private void _E010(Camera camera)
	{
		_E001 obj = new _E001();
		obj.BeginCB = new CommandBuffer();
		obj.BeginCB.name = _ED3E._E000(44940);
		camera.AddCommandBuffer(CameraEvent.BeforeReflections, obj.BeginCB);
		obj.EndCB = new CommandBuffer();
		obj.EndCB.name = _ED3E._E000(44984);
		camera.AddCommandBuffer(CameraEvent.BeforeReflections, obj.EndCB);
		this.m__E00C.Add(camera, obj);
	}

	public void UpdateInstancesBuffers()
	{
		this.m__E009 = false;
		Mesh mesh = _E002();
		List<_E404> list = new List<_E404>();
		List<List<StaticDeferredDecal>> list2 = new List<List<StaticDeferredDecal>>();
		List<Bounds> list3 = new List<Bounds>();
		int num = 0;
		for (int i = 0; i < this.m__E00A.Count; i++)
		{
			_E000 byIndex = this.m__E00A.GetByIndex(i);
			if (!this.m__E00B.ContainsKey(byIndex.Hash))
			{
				_E404 obj = new _E404();
				Material material = ((byIndex.DecalQueue == 0) ? byIndex.OwnMaterial : new Material(byIndex.OwnMaterial));
				obj.InitInstance(material, byIndex.DecalQueue != 0, byIndex.Decals.Count, mesh);
				this.m__E00B.Add(byIndex.Hash, obj);
			}
			this.m__E00B[byIndex.Hash].SetStartIndex(num);
			num += byIndex.Decals.Count;
			list.Add(this.m__E00B[byIndex.Hash]);
			list2.Add(byIndex.Decals);
			list3.Add(byIndex.Bounds);
		}
		_E404.InitGlobalBuffers(num);
		for (int j = 0; j < list.Count; j++)
		{
			list[j].UpdateBuffers(list2[j], mesh, list3[j]);
		}
		if (Application.isPlaying ? (UseQuadTreeRenderInPlayMode | UseTexturesQuadTreeInPlayMode) : (UseQuadTreeRenderInEditor | UseTexturesQuadTreeInEditor))
		{
			_E011();
		}
	}

	private void _E011()
	{
		if (this.m__E00A.Count == 0)
		{
			return;
		}
		_E015();
		_E000();
		this.m__E010 = new _E405();
		Bounds bounds = new Bounds(this.m__E00A.GetByIndex(0).Bounds.center, this.m__E00A.GetByIndex(0).Bounds.extents);
		for (int i = 1; i < this.m__E00A.Count; i++)
		{
			_E000 byIndex = this.m__E00A.GetByIndex(i);
			bounds.Encapsulate(byIndex.Bounds);
		}
		this.m__E010.Bounds = new Rect(bounds.min.x, bounds.min.z, bounds.size.x, bounds.size.z);
		this.m__E010.SizeLimit = DecalsGridSize;
		Mesh mesh = _E002();
		for (int j = 0; j < this.m__E00A.Count; j++)
		{
			foreach (StaticDeferredDecal decal in this.m__E00A.GetByIndex(j).Decals)
			{
				this.m__E010.Insert(decal, mesh);
			}
		}
		this.m__E011 = this.m__E010.ConstructCommandBuffersAndTexturesTrees(mesh, _E014);
		this.m__E012 = new List<_E408>();
		foreach (KeyValuePair<Texture2D, _E408> item in this.m__E011)
		{
			this.m__E012.Add(item.Value);
		}
	}

	private bool _E012(_E404 drawInstance)
	{
		_E01A[0] = drawInstance.Bounds.center + new Vector3(drawInstance.Bounds.extents.x, drawInstance.Bounds.extents.y, drawInstance.Bounds.extents.z);
		_E01A[1] = drawInstance.Bounds.center + new Vector3(drawInstance.Bounds.extents.x, drawInstance.Bounds.extents.y, 0f - drawInstance.Bounds.extents.z);
		_E01A[2] = drawInstance.Bounds.center + new Vector3(drawInstance.Bounds.extents.x, 0f - drawInstance.Bounds.extents.y, drawInstance.Bounds.extents.z);
		_E01A[3] = drawInstance.Bounds.center + new Vector3(drawInstance.Bounds.extents.x, 0f - drawInstance.Bounds.extents.y, 0f - drawInstance.Bounds.extents.z);
		_E01A[4] = drawInstance.Bounds.center + new Vector3(0f - drawInstance.Bounds.extents.x, drawInstance.Bounds.extents.y, drawInstance.Bounds.extents.z);
		_E01A[5] = drawInstance.Bounds.center + new Vector3(0f - drawInstance.Bounds.extents.x, drawInstance.Bounds.extents.y, 0f - drawInstance.Bounds.extents.z);
		_E01A[6] = drawInstance.Bounds.center + new Vector3(0f - drawInstance.Bounds.extents.x, 0f - drawInstance.Bounds.extents.y, drawInstance.Bounds.extents.z);
		_E01A[7] = drawInstance.Bounds.center + new Vector3(0f - drawInstance.Bounds.extents.x, 0f - drawInstance.Bounds.extents.y, 0f - drawInstance.Bounds.extents.z);
		for (int i = 0; i < 6; i++)
		{
			int num = 0;
			for (int j = 0; j < 8; j++)
			{
				if (Vector4.Dot(this.m__E018[i], new Vector4(_E01A[j].x, _E01A[j].y, _E01A[j].z, 1f)) < 0f)
				{
					num++;
				}
			}
			if (num == 8)
			{
				return false;
			}
		}
		return true;
	}

	private void _E013(Camera currentCamera)
	{
		_E001 obj = this.m__E00C[currentCamera];
		obj.BeginCB.Clear();
		obj.BeginCB.GetTemporaryRT(StaticDeferredDecalRenderer.m__E017, -1, -1);
		obj.BeginCB.Blit(BuiltinRenderTextureType.GBuffer2, StaticDeferredDecalRenderer.m__E017);
		obj.BeginCB.SetRenderTarget(this.m__E016, BuiltinRenderTextureType.CameraTarget);
		currentCamera.AddCommandBuffer(CameraEvent.BeforeReflections, obj.BeginCB);
		Mesh mesh = _E002();
		this.m__E000.Update(currentCamera);
		this.m__E000.GetPlanes(this.m__E019);
		for (int i = 0; i < 6; i++)
		{
			this.m__E018[i].x = this.m__E019[i].normal.x;
			this.m__E018[i].y = this.m__E019[i].normal.y;
			this.m__E018[i].z = this.m__E019[i].normal.z;
			this.m__E018[i].w = this.m__E019[i].distance;
		}
		bool flag = (Application.isPlaying ? UseQuadTreeRenderInPlayMode : UseQuadTreeRenderInEditor);
		bool flag2 = (Application.isPlaying ? UseTexturesQuadTreeInPlayMode : UseTexturesQuadTreeInEditor);
		for (int j = 0; j < this.m__E00B.Values.Count; j++)
		{
			_E404 obj2 = this.m__E00B.Values[j];
			if (!flag && _E012(obj2))
			{
				obj2.Material.SetInt(StaticDeferredDecalRenderer.m__E00E, obj2.StartIndex);
				obj.BeginCB.DrawMeshInstancedIndirect(mesh, 0, obj2.Material, -1, obj2.ArgsBuffer);
			}
		}
		_E405.ThreadSkipJob = !flag;
		_E405.ThreadSleepTime = TreeThreadSleepTime;
		_E408.ThreadSkipJob = !flag2;
		_E408.ThreadSleepTime = TreeThreadSleepTime;
		if (flag2 && this.m__E011 != null)
		{
			float val = (float)currentCamera.pixelWidth * currentCamera.nearClipPlane;
			float val2 = (float)currentCamera.pixelHeight * currentCamera.nearClipPlane;
			this.m__E003 = Math.Max(this.m__E003, val);
			this.m__E004 = Math.Max(this.m__E004, val2);
			if (this.m__E012 != null)
			{
				for (int k = 0; k < UpdateTexturesPerFrame; k++)
				{
					this.m__E013 = (this.m__E013 + 1) % this.m__E012.Count;
					this.m__E012[this.m__E013].SetThreadParams(currentCamera, this.m__E000, this.m__E003, this.m__E004);
					this.m__E012[this.m__E013].UpdateTextureRequestedMipMapLevel();
				}
			}
		}
		if (flag && this.m__E010 != null)
		{
			this.m__E010.SetThreadParams(this.m__E000, TreeCmdBufMaxDepth);
			if (UseTreeAsync && !QuadTreeRecordPerFrame)
			{
				List<CommandBuffer> commandBuffers = new List<CommandBuffer>();
				if (this.m__E010.GetThreadResults(ref commandBuffers))
				{
					foreach (CommandBuffer item in commandBuffers)
					{
						currentCamera.AddCommandBuffer(CameraEvent.BeforeReflections, item);
						obj.DrawCBs.Add(item);
					}
				}
			}
			else if (QuadTreeRecordPerFrame)
			{
				Mesh mesh2 = _E002();
				this.m__E010.RecordCommandBuffer(this.m__E000, obj.EndCB, TreeCmdBufMaxDepth, _E014, mesh2);
			}
			else
			{
				List<CommandBuffer> commandBuffers2 = new List<CommandBuffer>();
				this.m__E010.GetCommandBuffers(this.m__E000, ref commandBuffers2, TreeCmdBufMaxDepth);
				foreach (CommandBuffer item2 in commandBuffers2)
				{
					currentCamera.AddCommandBuffer(CameraEvent.BeforeReflections, item2);
					obj.DrawCBs.Add(item2);
				}
			}
		}
		currentCamera.AddCommandBuffer(CameraEvent.BeforeReflections, obj.EndCB);
		obj.EndCB.ReleaseTemporaryRT(StaticDeferredDecalRenderer.m__E017);
	}

	private void _E014(CommandBuffer cmdBuf)
	{
		cmdBuf.SetRenderTarget(this.m__E016, BuiltinRenderTextureType.CameraTarget);
	}

	private void _E015()
	{
		foreach (Camera key in this.m__E00C.Keys)
		{
			if (key != null)
			{
				_E016(key);
			}
		}
		this.m__E00C.Clear();
		this.m__E003 = 1f;
		this.m__E004 = 1f;
	}

	private void _E016(Camera cam)
	{
		if (!this.m__E00C.TryGetValue(cam, out var value))
		{
			return;
		}
		cam.RemoveCommandBuffer(CameraEvent.BeforeReflections, value.BeginCB);
		cam.RemoveCommandBuffer(CameraEvent.BeforeReflections, value.EndCB);
		foreach (CommandBuffer drawCB in value.DrawCBs)
		{
			cam.RemoveCommandBuffer(CameraEvent.BeforeReflections, drawCB);
		}
		value.DrawCBs.Clear();
	}

	private void _E017()
	{
		this.m__E00A.Clear();
		foreach (KeyValuePair<string, _E404> item in this.m__E00B)
		{
			item.Value.DestroyResources();
		}
		this.m__E00B.Clear();
	}

	public void Refresh()
	{
	}

	private void _E018()
	{
	}

	private void _E019()
	{
	}

	private void OnDrawGizmosSelected()
	{
	}
}
