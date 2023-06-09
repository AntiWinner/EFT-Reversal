using System;
using UnityEngine;

public class MuzzleJet : MuzzleEffect
{
	public enum EditorType
	{
		CurveDriven,
		Manually
	}

	[Serializable]
	public class Particle
	{
		public float Position;

		public float Size = 1f;

		public Vector2 RandomShift;

		public float AxisShift;
	}

	public const string MeshName = "MuzzleJetCombinedMesh";

	public Particle[] Particles;

	public EditorType Type;

	public int ParticlesCount = 3;

	public Vector3 JetBounds = new Vector3(0f, 1f, 1.5f);

	public AnimationCurve PositionDensity = new AnimationCurve(new Keyframe(0f, 0.5f), new Keyframe(1f, 0.5f));

	public AnimationCurve Sizes = new AnimationCurve(new Keyframe(0f, 0f, 1f, 1f), new Keyframe(1f, 1f, 1f, 1f));

	public AnimationCurve RandomShiftsX = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 0f));

	public AnimationCurve RandomShiftsY = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 0f));

	public AnimationCurve AxisShift = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 0f));

	public float SizesMult = 1f;

	public float AxisShiftMult = 1f;

	public float Chance = 1f;

	public Vector2 RandomShiftsMult = Vector2.one;

	private static readonly int _E002 = Shader.PropertyToID(_ED3E._E000(88402));

	private static readonly int _E003 = Shader.PropertyToID(_ED3E._E000(88390));

	private static readonly int _E004 = Shader.PropertyToID(_ED3E._E000(88445));

	public static void RandomizeMaterial(Material material, Vector2 cellSize)
	{
		material.SetVector(_E002, new Vector4(1f + UnityEngine.Random.value, 1f + UnityEngine.Random.value, 1f + UnityEngine.Random.value, 1f + UnityEngine.Random.value) * 878788.8f);
		Vector2 normalized = new Vector2(0.5f - UnityEngine.Random.value, 0.5f - UnityEngine.Random.value).normalized;
		material.SetVector(_E003, new Vector4(normalized.x, normalized.y, 0f - normalized.y, normalized.x));
		material.SetVector(_E004, new Vector4((float)UnityEngine.Random.Range(-255, 255) * cellSize.x, (float)UnityEngine.Random.Range(-255, 255) * cellSize.y, 0f, 0f));
	}

	private void _E000(ref int quadNum, Particle particle, Vector3 axis, Vector3 position, Vector2 cellSize, Vector2 posNorm, byte commonRnd, float addLengthRnd, Vector3[] vertices, Vector3[] normals, Vector2[] uv0, Vector2[] uv1, Color32[] colors, Vector4[] tangents)
	{
		int num = quadNum;
		int num2 = quadNum + 1;
		int num3 = quadNum + 2;
		int num4 = quadNum + 3;
		quadNum += 4;
		Vector2 vector = new Vector2(0.5f - UnityEngine.Random.value, 0.5f - UnityEngine.Random.value).normalized * particle.Size;
		Vector2 vector2 = new Vector2(vector.y, 0f - vector.x);
		uv1[num] = vector;
		uv1[num2] = -vector2;
		uv1[num3] = -vector;
		uv1[num4] = vector2;
		float num5 = (float)UnityEngine.Random.Range(-255, 255) * cellSize.x;
		float x = cellSize.x + num5;
		float num6 = (float)UnityEngine.Random.Range(-255, 255) * cellSize.y;
		float y = cellSize.y + num6;
		uv0[num] = new Vector2(num5, y);
		uv0[num2] = new Vector2(num5, num6);
		uv0[num3] = new Vector2(x, num6);
		uv0[num4] = new Vector2(x, y);
		float num7 = (particle.Position - posNorm.x) * posNorm.y;
		colors[num] = (colors[num2] = (colors[num3] = (colors[num4] = new Color32(commonRnd, (byte)UnityEngine.Random.Range(127, 255), (byte)(255f * num7), (byte)(255f * Chance)))));
		vertices[num] = (vertices[num2] = (vertices[num3] = (vertices[num4] = position + axis * particle.Position)));
		normals[num] = (normals[num2] = (normals[num3] = (normals[num4] = axis)));
		tangents[num] = (tangents[num2] = (tangents[num3] = (tangents[num4] = new Vector4(addLengthRnd * num7, particle.AxisShift, particle.RandomShift.x, particle.RandomShift.y))));
	}

	private void _E001(ref int quadNum, Transform patent, Vector2 cellSize, Vector3[] vertices, Vector3[] normals, Vector2[] uv0, Vector2[] uv1, Color32[] colors, Vector4[] tangents)
	{
		Vector3 axis = ((patent == base.transform) ? Vector3.down : (_E38B.GetRotationRelativeToParent(patent, base.transform) * Vector3.down));
		Vector3 position = ((patent == base.transform) ? Vector3.zero : _E38B.GetPositionRelativeToParent(patent, base.transform));
		float num = float.PositiveInfinity;
		float num2 = float.NegativeInfinity;
		Particle[] particles = Particles;
		foreach (Particle particle in particles)
		{
			num = Mathf.Min(num, particle.Position);
			num2 = Mathf.Max(num2, particle.Position);
		}
		Vector2 posNorm = new Vector2(num, 1f / (num2 - num));
		float addLengthRnd = JetBounds.z - JetBounds.y;
		byte commonRnd = (byte)(UnityEngine.Random.value * 255f);
		particles = Particles;
		foreach (Particle particle2 in particles)
		{
			_E000(ref quadNum, particle2, axis, position, cellSize, posNorm, commonRnd, addLengthRnd, vertices, normals, uv0, uv1, colors, tangents);
		}
	}

	public static Transform UpdateOrCreateMesh(MuzzleJet[] muzzleJets, Transform patent, Material material, Vector2 cellSize)
	{
		int num = 0;
		foreach (MuzzleJet muzzleJet in muzzleJets)
		{
			num += muzzleJet.Particles.Length;
		}
		int num2 = num << 2;
		Vector3[] vertices = new Vector3[num2];
		Vector3[] normals = new Vector3[num2];
		Vector4[] tangents = new Vector4[num2];
		Vector2[] array = new Vector2[num2];
		Vector2[] array2 = new Vector2[num2];
		Color32[] array3 = new Color32[num2];
		int[] array4 = new int[num * 6];
		int j = 0;
		int num3 = 0;
		for (; j < num; j++)
		{
			int num4 = j << 2;
			array4[num3++] = num4;
			array4[num3++] = num4 + 1;
			array4[num3++] = num4 + 2;
			array4[num3++] = num4 + 2;
			array4[num3++] = num4 + 3;
			array4[num3++] = num4;
		}
		int k = 0;
		int quadNum = 0;
		for (; k < muzzleJets.Length; k++)
		{
			muzzleJets[k]._E001(ref quadNum, patent, cellSize, vertices, normals, array, array2, array3, tangents);
		}
		Transform transform = patent.Find(_ED3E._E000(88327));
		if (transform == null)
		{
			GameObject obj = new GameObject(_ED3E._E000(88327), typeof(MeshFilter), typeof(MeshRenderer))
			{
				hideFlags = HideFlags.DontSave
			};
			transform = obj.transform;
			transform.parent = patent;
			transform.localPosition = Vector2.zero;
			transform.localRotation = Quaternion.identity;
			Mesh mesh = new Mesh
			{
				vertices = vertices,
				normals = normals,
				uv = array,
				uv2 = array2,
				triangles = array4,
				colors32 = array3,
				tangents = tangents,
				name = _ED3E._E000(88365)
			};
			mesh.MarkDynamic();
			obj.GetComponent<MeshFilter>().mesh = mesh;
			obj.GetComponent<Renderer>().material = material;
		}
		else
		{
			Mesh mesh2 = transform.GetComponent<MeshFilter>().sharedMesh;
			if (mesh2 == null)
			{
				mesh2 = new Mesh
				{
					name = _ED3E._E000(88365)
				};
				mesh2.MarkDynamic();
				transform.GetComponent<MeshFilter>().mesh = mesh2;
			}
			else
			{
				mesh2.Clear();
			}
			mesh2.vertices = vertices;
			mesh2.normals = normals;
			mesh2.uv = array;
			mesh2.uv2 = array2;
			mesh2.triangles = array4;
			mesh2.colors32 = array3;
			mesh2.tangents = tangents;
			transform.GetComponent<Renderer>().material = material;
		}
		HotObject hotObject = transform.gameObject.GetComponent<HotObject>();
		if (hotObject == null)
		{
			hotObject = transform.gameObject.AddComponent<HotObject>();
		}
		hotObject.IsApplyAllMaterials = true;
		hotObject.Temperature = new Vector3(0f, 1f, 5f);
		hotObject.SetTemperatureToRenderer();
		return transform;
	}
}
