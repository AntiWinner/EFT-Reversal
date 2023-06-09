using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

namespace AmplifyImpostors;

public class AmplifyImpostor : MonoBehaviour
{
	private const string m__E000 = "e82933f4c0eb9ba42aab0739f48efe21";

	private const string m__E001 = "57c23892d43bc9f458360024c5985405";

	private const string m__E002 = "31bd3cd74692f384a916d9d7ea87710d";

	private const string m__E003 = "572f9be5706148142b8da6e9de53acdb";

	private const string m__E004 = "e4786beb7716da54dbb02a632681cc37";

	private const string m__E005 = "089f3a2f6b5f48348a48c755f8d9a7a2";

	private const string m__E006 = "94e2ddcdfb3257a43872042f97e2fb01";

	private const string m__E007 = "990451a2073f6994ebf9fd6f90a842b3";

	private const string m__E008 = "47b6b3dcefe0eaf4997acf89caf8c75e";

	private const string m__E009 = "56236dc63ad9b7949b63a27f0ad180b3";

	private const string m__E00A = "175c951fec709c44fa2f26b8ab78b8dd";

	private const string _E00B = "0403878495ffa3c4e9d4bcb3eac9b559";

	private const string _E00C = "83dd8de9a5c14874884f9012def4fdcc";

	private const string _E00D = "da79d698f4bf0164e910ad798d07efdf";

	public static readonly int Property_HueVariation = Shader.PropertyToID(_ED3E._E000(116316));

	public static readonly int Property_Hue = Shader.PropertyToID(_ED3E._E000(238224));

	public static readonly int Property_Hemi = Shader.PropertyToID(_ED3E._E000(238221));

	public const string KEYWORD_HEMI_ON = "_HEMI_ON";

	public const string _WIND = "_WIND";

	public static readonly int Property_AlbedoArray = Shader.PropertyToID(_ED3E._E000(238219));

	public static readonly int Property_NormalsArray = Shader.PropertyToID(_ED3E._E000(238264));

	public static readonly int Property_Albedo = Shader.PropertyToID(_ED3E._E000(238254));

	public static readonly int Property_Normals = Shader.PropertyToID(_ED3E._E000(238246));

	public static readonly int Property_Specular = Shader.PropertyToID(_ED3E._E000(238303));

	public static readonly int Property_Emission = Shader.PropertyToID(_ED3E._E000(238289));

	public static readonly int Property_Index = Shader.PropertyToID(_ED3E._E000(238283));

	public static readonly int Property_Frames = Shader.PropertyToID(_ED3E._E000(238274));

	public static readonly int Property_ImpostorSize = Shader.PropertyToID(_ED3E._E000(238330));

	public static readonly int Property_Offset = Shader.PropertyToID(_ED3E._E000(86449));

	public static readonly int Property_DepthSize = Shader.PropertyToID(_ED3E._E000(238312));

	public static readonly int Property_ClipMask = Shader.PropertyToID(_ED3E._E000(238307));

	public static readonly int Property_AI_ShadowBias = Shader.PropertyToID(_ED3E._E000(238357));

	public static readonly int Property_AI_ShadowView = Shader.PropertyToID(_ED3E._E000(238340));

	public static readonly int Property_FramesX = Shader.PropertyToID(_ED3E._E000(238395));

	public static readonly int Property_FramesY = Shader.PropertyToID(_ED3E._E000(238380));

	public static readonly int Property_AI_Frames = Shader.PropertyToID(_ED3E._E000(238373));

	public static readonly int Property_AI_ImpostorSize = Shader.PropertyToID(_ED3E._E000(238424));

	public static readonly int Property_AI_Offset = Shader.PropertyToID(_ED3E._E000(238409));

	public static readonly int Property_AI_SizeOffset = Shader.PropertyToID(_ED3E._E000(238460));

	public static readonly int Property_TextureBias = Shader.PropertyToID(_ED3E._E000(238451));

	public static readonly int Property_Parallax = Shader.PropertyToID(_ED3E._E000(238432));

	public static readonly int Property_AI_DepthSize = Shader.PropertyToID(_ED3E._E000(238490));

	public static readonly int Property_AI_FramesX = Shader.PropertyToID(_ED3E._E000(238472));

	public static readonly int Property_AI_FramesY = Shader.PropertyToID(_ED3E._E000(238524));

	public static readonly int Property_AI_AlphaToCoverage = Shader.PropertyToID(_ED3E._E000(238512));

	[SerializeField]
	private AmplifyImpostorAsset m_data;

	[SerializeField]
	private Transform m_rootTransform;

	[SerializeField]
	private LODGroup m_lodGroup;

	[SerializeField]
	private Renderer[] m_renderers;

	public LODReplacement m_lodReplacement = LODReplacement.ReplaceLast;

	[SerializeField]
	public RenderPipelineInUse m_renderPipelineInUse;

	public int m_insertIndex = 1;

	[SerializeField]
	public GameObject m_lastImpostor;

	[SerializeField]
	public string m_folderPath;

	[NonSerialized]
	public string m_impostorName = string.Empty;

	[SerializeField]
	public CutMode m_cutMode;

	[NonSerialized]
	private const float _E00E = -90f;

	[NonSerialized]
	private const float _E00F = 90f;

	[NonSerialized]
	private const int _E010 = 256;

	[NonSerialized]
	private RenderTexture[] _E011;

	[NonSerialized]
	private RenderTexture[] _E012;

	[NonSerialized]
	private RenderTexture _E013;

	[NonSerialized]
	public Texture2D m_alphaTex;

	[NonSerialized]
	private float _E014;

	[NonSerialized]
	private float _E015;

	[NonSerialized]
	private Vector2 _E016 = Vector2.zero;

	[NonSerialized]
	private Bounds _E017;

	[NonSerialized]
	private Vector3 _E018 = Vector3.zero;

	[NonSerialized]
	private Quaternion _E019 = Quaternion.identity;

	[NonSerialized]
	private Vector3 _E01A = Vector3.one;

	[NonSerialized]
	private const int _E01B = 65536;

	public AmplifyImpostorAsset Data
	{
		get
		{
			return m_data;
		}
		set
		{
			m_data = value;
		}
	}

	public Transform RootTransform
	{
		get
		{
			return m_rootTransform;
		}
		set
		{
			m_rootTransform = value;
		}
	}

	public LODGroup LodGroup
	{
		get
		{
			return m_lodGroup;
		}
		set
		{
			m_lodGroup = value;
		}
	}

	public Renderer[] Renderers
	{
		get
		{
			return m_renderers;
		}
		set
		{
			m_renderers = value;
		}
	}

	private void _E000(List<TextureOutput> outputList, bool standardRendering)
	{
		_E011 = new RenderTexture[outputList.Count];
		if (standardRendering && m_renderPipelineInUse == RenderPipelineInUse.HD)
		{
			GraphicsFormat format = GraphicsFormat.R8G8B8A8_SRGB;
			GraphicsFormat format2 = GraphicsFormat.R8G8B8A8_UNorm;
			GraphicsFormat format3 = GraphicsFormat.R16G16B16A16_SFloat;
			_E011[0] = new RenderTexture((int)m_data.TexSize.x, (int)m_data.TexSize.y, 16, format);
			_E011[0].Create();
			_E011[1] = new RenderTexture((int)m_data.TexSize.x, (int)m_data.TexSize.y, 16, format2);
			_E011[1].Create();
			_E011[2] = new RenderTexture((int)m_data.TexSize.x, (int)m_data.TexSize.y, 16, format2);
			_E011[2].Create();
			_E011[3] = new RenderTexture((int)m_data.TexSize.x, (int)m_data.TexSize.y, 16, format3);
			_E011[3].Create();
			_E011[4] = new RenderTexture((int)m_data.TexSize.x, (int)m_data.TexSize.y, 16, format2);
			_E011[4].Create();
		}
		else
		{
			for (int i = 0; i < _E011.Length; i++)
			{
				_E011[i] = new RenderTexture((int)m_data.TexSize.x, (int)m_data.TexSize.y, 16, (!outputList[i].SRGB) ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.ARGB32);
				_E011[i].Create();
			}
		}
		_E013 = new RenderTexture((int)m_data.TexSize.x, (int)m_data.TexSize.y, 16, RenderTextureFormat.Depth);
		_E013.Create();
	}

	private void _E001(List<TextureOutput> outputList)
	{
		_E012 = new RenderTexture[outputList.Count];
		for (int i = 0; i < _E012.Length; i++)
		{
			_E012[i] = new RenderTexture(256, 256, 16, (!outputList[i].SRGB) ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.ARGB32);
			_E012[i].Create();
		}
		_E013 = new RenderTexture(256, 256, 16, RenderTextureFormat.Depth);
		_E013.Create();
	}

	private void _E002()
	{
		RenderTexture.active = null;
		RenderTexture[] array = _E011;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Release();
		}
		_E011 = null;
	}

	private void _E003()
	{
		RenderTexture.active = null;
		RenderTexture[] array = _E012;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Release();
		}
		_E012 = null;
	}

	public void RenderImpostor(ImpostorType impostorType, int targetAmount, bool impostorMaps = true, bool combinedAlphas = false, bool useMinResolution = false, Shader customShader = null)
	{
		if ((!impostorMaps && !combinedAlphas) || targetAmount <= 0)
		{
			return;
		}
		bool flag = customShader == null;
		Dictionary<Material, Material> dictionary = new Dictionary<Material, Material>();
		CommandBuffer commandBuffer = new CommandBuffer();
		if (impostorMaps)
		{
			commandBuffer.name = _ED3E._E000(237965);
			RenderTargetIdentifier[] array = new RenderTargetIdentifier[targetAmount];
			for (int i = 0; i < targetAmount; i++)
			{
				array[i] = _E011[i];
			}
			commandBuffer.SetRenderTarget(array, _E013);
			commandBuffer.ClearRenderTarget(clearDepth: true, clearColor: true, Color.clear, 1f);
		}
		CommandBuffer commandBuffer2 = new CommandBuffer();
		if (combinedAlphas)
		{
			commandBuffer2.name = _ED3E._E000(238012);
			RenderTargetIdentifier[] array2 = new RenderTargetIdentifier[targetAmount];
			for (int j = 0; j < targetAmount; j++)
			{
				array2[j] = _E012[j];
			}
			commandBuffer2.SetRenderTarget(array2, _E013);
			commandBuffer2.ClearRenderTarget(clearDepth: true, clearColor: true, Color.clear, 1f);
		}
		int horizontalFrames = m_data.HorizontalFrames;
		int num = m_data.HorizontalFrames;
		if (impostorType == ImpostorType.Spherical)
		{
			num = m_data.HorizontalFrames - 1;
			if (m_data.DecoupleAxisFrames)
			{
				num = m_data.VerticalFrames - 1;
			}
		}
		List<MeshFilter> list = new List<MeshFilter>();
		for (int k = 0; k < Renderers.Length; k++)
		{
			if (Renderers[k] == null || !Renderers[k].enabled || Renderers[k].shadowCastingMode == ShadowCastingMode.ShadowsOnly)
			{
				list.Add(null);
				continue;
			}
			MeshFilter component = Renderers[k].GetComponent<MeshFilter>();
			if (component == null || component.sharedMesh == null)
			{
				list.Add(null);
			}
			else
			{
				list.Add(component);
			}
		}
		int count = list.Count;
		for (int l = 0; l < horizontalFrames; l++)
		{
			for (int m = 0; m <= num; m++)
			{
				Bounds bounds = default(Bounds);
				Matrix4x4 matrix = _E004(impostorType, horizontalFrames, num, l, m);
				for (int n = 0; n < count; n++)
				{
					if (!(list[n] == null))
					{
						if (bounds.size == Vector3.zero)
						{
							bounds = list[n].sharedMesh.bounds.Transform(m_rootTransform.worldToLocalMatrix * Renderers[n].localToWorldMatrix);
						}
						else
						{
							bounds.Encapsulate(list[n].sharedMesh.bounds.Transform(m_rootTransform.worldToLocalMatrix * Renderers[n].localToWorldMatrix));
						}
					}
				}
				if (l == 0 && m == 0)
				{
					_E017 = bounds;
				}
				bounds = bounds.Transform(matrix);
				Matrix4x4 matrix4x = matrix.inverse * Matrix4x4.LookAt(bounds.center - new Vector3(0f, 0f, _E015 * 0.5f), bounds.center, Vector3.up);
				float num2 = _E014 * 0.5f;
				Matrix4x4 matrix4x2 = Matrix4x4.Ortho(0f - num2 + _E016.x, num2 + _E016.x, 0f - num2 + _E016.y, num2 + _E016.y, 0f, 0f - _E015);
				matrix4x = matrix4x.inverse * m_rootTransform.worldToLocalMatrix;
				if (flag && m_renderPipelineInUse == RenderPipelineInUse.HD)
				{
					matrix4x2 = GL.GetGPUProjectionMatrix(matrix4x2, renderIntoTexture: true);
				}
				if (impostorMaps)
				{
					commandBuffer.SetViewProjectionMatrices(matrix4x, matrix4x2);
					commandBuffer.SetViewport(new Rect(m_data.TexSize.x / (float)horizontalFrames * (float)l, m_data.TexSize.y / (float)(num + ((impostorType == ImpostorType.Spherical) ? 1 : 0)) * (float)m, m_data.TexSize.x / (float)m_data.HorizontalFrames, m_data.TexSize.y / (float)m_data.VerticalFrames));
					if (flag && m_renderPipelineInUse == RenderPipelineInUse.HD)
					{
						_ED34.SetupShaderVariableGlobals(matrix4x, matrix4x2, commandBuffer);
						commandBuffer.SetGlobalMatrix(_ED3E._E000(19211), matrix4x);
						commandBuffer.SetGlobalMatrix(_ED3E._E000(237998), matrix4x.inverse);
						commandBuffer.SetGlobalMatrix(_ED3E._E000(19168), matrix4x2);
						commandBuffer.SetGlobalMatrix(_ED3E._E000(238045), matrix4x2 * matrix4x);
						commandBuffer.SetGlobalVector(_ED3E._E000(238029), Vector4.zero);
					}
				}
				if (combinedAlphas)
				{
					commandBuffer2.SetViewProjectionMatrices(matrix4x, matrix4x2);
					commandBuffer2.SetViewport(new Rect(0f, 0f, 256f, 256f));
					if (flag && m_renderPipelineInUse == RenderPipelineInUse.HD)
					{
						_ED34.SetupShaderVariableGlobals(matrix4x, matrix4x2, commandBuffer2);
						commandBuffer2.SetGlobalMatrix(_ED3E._E000(19211), matrix4x);
						commandBuffer2.SetGlobalMatrix(_ED3E._E000(237998), matrix4x.inverse);
						commandBuffer2.SetGlobalMatrix(_ED3E._E000(19168), matrix4x2);
						commandBuffer2.SetGlobalMatrix(_ED3E._E000(238045), matrix4x2 * matrix4x);
						commandBuffer2.SetGlobalVector(_ED3E._E000(238029), Vector4.zero);
					}
				}
				for (int num3 = 0; num3 < count; num3++)
				{
					if (list[num3] == null)
					{
						continue;
					}
					Material[] sharedMaterials = Renderers[num3].sharedMaterials;
					for (int num4 = 0; num4 < sharedMaterials.Length; num4++)
					{
						Material value = null;
						_ = list[num3].sharedMesh;
						int num5 = 0;
						int num6 = 0;
						if (flag)
						{
							value = sharedMaterials[num4];
							num5 = value.FindPass(_ED3E._E000(238074));
							if (num5 == -1)
							{
								num5 = value.FindPass(_ED3E._E000(238067));
							}
							if (num5 == -1)
							{
								num5 = value.FindPass(_ED3E._E000(238052));
							}
							num6 = value.FindPass(_ED3E._E000(238108));
							if (num5 == -1)
							{
								num5 = 0;
								for (int num7 = 0; num7 < value.passCount; num7++)
								{
									if (value.GetTag(_ED3E._E000(238102), searchFallbacks: true).Equals(_ED3E._E000(238067)))
									{
										num5 = num7;
										break;
									}
								}
							}
							commandBuffer.EnableShaderKeyword(_ED3E._E000(238088));
						}
						else
						{
							num6 = -1;
							if (!dictionary.TryGetValue(sharedMaterials[num4], out value))
							{
								value = new Material(customShader)
								{
									hideFlags = HideFlags.HideAndDontSave
								};
								dictionary.Add(sharedMaterials[num4], value);
							}
						}
						bool flag2 = Renderers[num3].lightmapIndex > -1;
						bool flag3 = Renderers[num3].realtimeLightmapIndex > -1;
						if ((flag2 || flag3) && !flag)
						{
							commandBuffer.EnableShaderKeyword(_ED3E._E000(238141));
							if (flag2)
							{
								commandBuffer.SetGlobalVector(_ED3E._E000(238129), Renderers[num3].lightmapScaleOffset);
							}
							if (flag3)
							{
								commandBuffer.EnableShaderKeyword(_ED3E._E000(238114));
								commandBuffer.SetGlobalVector(_ED3E._E000(238157), Renderers[num3].realtimeLightmapScaleOffset);
							}
							else
							{
								commandBuffer.DisableShaderKeyword(_ED3E._E000(238114));
							}
							if (flag2 && flag3)
							{
								commandBuffer.EnableShaderKeyword(_ED3E._E000(238197));
							}
							else
							{
								commandBuffer.DisableShaderKeyword(_ED3E._E000(238197));
							}
						}
						else
						{
							commandBuffer.DisableShaderKeyword(_ED3E._E000(238141));
							commandBuffer.DisableShaderKeyword(_ED3E._E000(238114));
							commandBuffer.DisableShaderKeyword(_ED3E._E000(238197));
						}
						commandBuffer.DisableShaderKeyword(_ED3E._E000(238178));
						if (impostorMaps)
						{
							if (num6 > -1)
							{
								commandBuffer.DrawRenderer(Renderers[num3], value, num4, num6);
							}
							commandBuffer.DrawRenderer(Renderers[num3], value, num4, num5);
						}
						if (combinedAlphas)
						{
							if (num6 > -1)
							{
								commandBuffer2.DrawRenderer(Renderers[num3], value, num4, num6);
							}
							commandBuffer2.DrawRenderer(Renderers[num3], value, num4, num5);
						}
					}
				}
				if (impostorMaps)
				{
					Graphics.ExecuteCommandBuffer(commandBuffer);
				}
				if (combinedAlphas)
				{
					Graphics.ExecuteCommandBuffer(commandBuffer2);
				}
			}
		}
		list.Clear();
		foreach (KeyValuePair<Material, Material> item in dictionary)
		{
			Material value2 = item.Value;
			if (value2 != null)
			{
				if (!Application.isPlaying)
				{
					UnityEngine.Object.DestroyImmediate(value2);
				}
				value2 = null;
			}
		}
		dictionary.Clear();
		commandBuffer.Release();
		commandBuffer = null;
		commandBuffer2.Release();
		commandBuffer2 = null;
	}

	private Matrix4x4 _E004(ImpostorType impostorType, int hframes, int vframes, int x, int y)
	{
		Matrix4x4 result = Matrix4x4.identity;
		switch (impostorType)
		{
		case ImpostorType.Spherical:
		{
			float num = 0f;
			if (vframes > 0)
			{
				num = 0f - 180f / (float)vframes;
			}
			Quaternion quaternion = Quaternion.Euler(num * (float)y + 90f, 0f, 0f);
			Quaternion quaternion2 = Quaternion.Euler(0f, 360f / (float)hframes * (float)x + -90f, 0f);
			result = Matrix4x4.Rotate(quaternion * quaternion2);
			break;
		}
		case ImpostorType.Octahedron:
		{
			Vector3 vector2 = _E006((float)x / ((float)hframes - 1f) * 2f - 1f, (float)y / ((float)vframes - 1f) * 2f - 1f);
			result = Matrix4x4.Rotate(Quaternion.LookRotation(new Vector3(vector2.x * -1f, vector2.z * -1f, vector2.y * -1f), Vector3.up)).inverse;
			break;
		}
		case ImpostorType.HemiOctahedron:
		{
			Vector3 vector = _E007((float)x / ((float)hframes - 1f) * 2f - 1f, (float)y / ((float)vframes - 1f) * 2f - 1f);
			result = Matrix4x4.Rotate(Quaternion.LookRotation(new Vector3(vector.x * -1f, vector.z * -1f, vector.y * -1f), Vector3.up)).inverse;
			break;
		}
		}
		return result;
	}

	private Vector3 _E005(Vector2 oct)
	{
		Vector3 value = new Vector3(oct.x, oct.y, 1f - Mathf.Abs(oct.x) - Mathf.Abs(oct.y));
		float num = Mathf.Clamp01(0f - value.z);
		value.Set(value.x + ((value.x >= 0f) ? (0f - num) : num), value.y + ((value.y >= 0f) ? (0f - num) : num), value.z);
		return Vector3.Normalize(value);
	}

	private Vector3 _E006(float x, float y)
	{
		Vector3 value = new Vector3(x, y, 1f - Mathf.Abs(x) - Mathf.Abs(y));
		float num = Mathf.Clamp01(0f - value.z);
		value.Set(value.x + ((value.x >= 0f) ? (0f - num) : num), value.y + ((value.y >= 0f) ? (0f - num) : num), value.z);
		return Vector3.Normalize(value);
	}

	private Vector3 _E007(float x, float y)
	{
		float num = x;
		float num2 = y;
		x = (num + num2) * 0.5f;
		y = (num - num2) * 0.5f;
		return Vector3.Normalize(new Vector3(x, y, 1f - Mathf.Abs(x) - Mathf.Abs(y)));
	}

	public void GenerateAutomaticMesh(AmplifyImpostorAsset data)
	{
		_ED37.GenerateOutline(rect: new Rect(0f, 0f, m_alphaTex.width, m_alphaTex.height), texture: m_alphaTex, detail: data.Tolerance, alphaTolerance: 254, holeDetection: false, paths: out var paths);
		int num = 0;
		for (int i = 0; i < paths.Length; i++)
		{
			num += paths[i].Length;
		}
		data.ShapePoints = new Vector2[num];
		int num2 = 0;
		for (int j = 0; j < paths.Length; j++)
		{
			for (int k = 0; k < paths[j].Length; k++)
			{
				data.ShapePoints[num2] = paths[j][k] + new Vector2((float)m_alphaTex.width * 0.5f, (float)m_alphaTex.height * 0.5f);
				data.ShapePoints[num2] = Vector2.Scale(data.ShapePoints[num2], new Vector2(1f / (float)m_alphaTex.width, 1f / (float)m_alphaTex.height));
				num2++;
			}
		}
		data.ShapePoints = _ED39.ConvexHull(data.ShapePoints);
		data.ShapePoints = _ED39.ReduceVertices(data.ShapePoints, data.MaxVertices);
		data.ShapePoints = _ED39.ScaleAlongNormals(data.ShapePoints, data.NormalScale);
		for (int l = 0; l < data.ShapePoints.Length; l++)
		{
			data.ShapePoints[l].x = Mathf.Clamp01(data.ShapePoints[l].x);
			data.ShapePoints[l].y = Mathf.Clamp01(data.ShapePoints[l].y);
		}
		data.ShapePoints = _ED39.ConvexHull(data.ShapePoints);
		for (int m = 0; m < data.ShapePoints.Length; m++)
		{
			data.ShapePoints[m] = new Vector2(data.ShapePoints[m].x, 1f - data.ShapePoints[m].y);
		}
	}

	public Mesh GenerateMesh(Vector2[] points, Vector3 offset, float width = 1f, float height = 1f, bool invertY = true)
	{
		Vector2[] array = new Vector2[points.Length];
		Vector2[] array2 = new Vector2[points.Length];
		Array.Copy(points, array, points.Length);
		float num = width * 0.5f;
		float num2 = height * 0.5f;
		if (invertY)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Vector2(array[i].x, 1f - array[i].y);
			}
		}
		Array.Copy(array, array2, array.Length);
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = new Vector2(array[j].x * width - num + _E016.x, array[j].y * height - num2 + _E016.y);
		}
		_ED3B obj = new _ED3B(array);
		int[] triangles = obj.Triangulate();
		Vector3[] array3 = new Vector3[obj.Points.Count];
		for (int k = 0; k < array3.Length; k++)
		{
			array3[k] = new Vector3(obj.Points[k].x, obj.Points[k].y, 0f);
		}
		Mesh mesh = new Mesh();
		mesh.vertices = array3;
		mesh.uv = array2;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
		mesh.bounds = new Bounds(offset, _E017.size);
		return mesh;
	}

	internal static bool? _E008(AmplifyImpostor impostor, AmplifyImpostor reference)
	{
		if (reference == null || impostor == null)
		{
			return null;
		}
		if ((object)impostor == reference)
		{
			return true;
		}
		return Math.Abs(impostor._E014 - reference._E014) < 1E-06f && Math.Abs(impostor._E015 - reference._E015) < 1E-06f && impostor.m_data.HorizontalFrames == reference.m_data.HorizontalFrames && impostor.m_data.VerticalFrames == reference.m_data.VerticalFrames;
	}

	internal void _E009(Material material)
	{
		Vector4 vector = new Vector4(_E017.center.x, _E017.center.y, _E017.center.z, 1f);
		Vector4 offset = new Vector4(vector.x, vector.y, vector.z, (0f - _E016.y) / _E014);
		Vector4 sizeOffset = new Vector4(_E014, _E015, _E016.x / _E014 / (float)m_data.HorizontalFrames, _E016.y / _E014 / (float)m_data.VerticalFrames);
		_E00A(material, offset, sizeOffset);
	}

	private void _E00A(Material material, Vector4 offset, Vector4 sizeOffset)
	{
		material.SetFloat(Property_Frames, m_data.HorizontalFrames);
		material.SetFloat(Property_ImpostorSize, _E014);
		material.SetVector(Property_Offset, offset);
		material.SetFloat(Property_DepthSize, _E015);
		material.SetFloat(Property_FramesX, m_data.HorizontalFrames);
		material.SetFloat(Property_FramesY, m_data.VerticalFrames);
		material.SetFloat(Property_AI_Frames, m_data.HorizontalFrames);
		material.SetFloat(Property_AI_ImpostorSize, _E014);
		material.SetVector(Property_AI_Offset, offset);
		material.SetVector(Property_AI_SizeOffset, sizeOffset);
		material.SetFloat(Property_AI_DepthSize, _E015);
		material.SetFloat(Property_AI_FramesX, m_data.HorizontalFrames);
		material.SetFloat(Property_AI_FramesY, m_data.VerticalFrames);
	}
}
