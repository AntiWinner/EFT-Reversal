using System;
using System.Collections.Generic;
using Comfort.Common;
using EFT;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Tracers : MonoBehaviour
{
	private readonly HashSet<_EC26> m__E000 = new HashSet<_EC26>();

	public float MaxDistance = 20f;

	public float Probobility = 1f;

	public int Count = 250;

	private Vector3[] m__E001;

	private Vector4[] m__E002;

	private Mesh m__E003;

	private Bounds _E004;

	private Material _E005;

	private bool _E006;

	private int _E007;

	private float _E008;

	private static readonly int _E009 = Shader.PropertyToID(_ED3E._E000(88654));

	private void Start()
	{
	}

	private void OnValidate()
	{
		Count = Math.Max(Count, 1);
		int val = 16383;
		Count = Math.Min(Count, val);
	}

	public void Awake()
	{
		int num = Count * 4;
		int num2 = Count * 6;
		this.m__E001 = new Vector3[num];
		int[] triangles = new int[num2];
		Vector2[] array = new Vector2[num];
		Vector2[] array2 = new Vector2[num];
		this.m__E002 = new Vector4[num];
		for (int i = 0; i < Count; i++)
		{
			Fill(i, array, array2, triangles);
		}
		_E004 = new Bounds(Vector3.zero, Vector3.zero);
		this.m__E003 = new Mesh
		{
			vertices = this.m__E001,
			triangles = triangles,
			uv = array,
			uv2 = array2,
			tangents = this.m__E002,
			name = _ED3E._E000(81951)
		};
		GetComponent<MeshFilter>().mesh = this.m__E003;
		_E005 = GetComponent<Renderer>().sharedMaterial;
	}

	public void Fill(int pos, Vector2[] uv0, Vector2[] uv1, int[] triangles)
	{
		int num = pos * 4;
		uv0[num] = new Vector2(-1f, 0f);
		uv0[num + 1] = new Vector2(1f, 0f);
		uv0[num + 2] = new Vector2(-1f, 1f);
		uv0[num + 3] = new Vector2(1f, 1f);
		uv1[num] = (uv1[num + 1] = (uv1[num + 2] = (uv1[num + 3] = new Vector2(UnityEngine.Random.value, UnityEngine.Random.value))));
		int num2 = pos * 6;
		for (int i = 0; i < 1; i++)
		{
			int num3 = num + (i << 1);
			int num4 = num3++;
			int num5 = num3++;
			int num6 = num3++;
			triangles[num2++] = num4;
			triangles[num2++] = num6;
			triangles[num2++] = num3;
			triangles[num2++] = num3;
			triangles[num2++] = num5;
			triangles[num2++] = num4;
		}
	}

	public void EmitSeg(Vector3 start, Vector3 direction, float time)
	{
		int num = 4;
		int i = _E007 * num;
		Vector3 vector = start + direction;
		Vector4 vector2 = new Vector4(direction.x, direction.y, direction.z, time);
		int num2 = i + num;
		int num3 = 0;
		for (; i < num2; i++)
		{
			this.m__E001[i] = start;
			this.m__E002[i] = vector2;
			num3++;
		}
		_E007++;
		if (_E007 >= Count)
		{
			_E007 = 0;
		}
		_E004.Encapsulate(start);
		_E004.Encapsulate(vector);
		_E000(ref _E004, start, vector);
		_E006 = true;
	}

	private static void _E000(ref Bounds bounds, Vector3 p0, Vector3 p1)
	{
		_E001(ref p0);
		_E001(ref p1);
		Vector3 vector = new Vector3(Math.Max(p0.x, p1.x) + 50f, Math.Max(p0.y, p1.y) + 50f, Math.Max(p0.z, p1.z) + 50f);
		Vector3 extents = bounds.extents;
		extents.x = ((extents.x > vector.x) ? extents.x : vector.x);
		extents.y = ((extents.y > vector.y) ? extents.y : vector.y);
		extents.z = ((extents.z > vector.z) ? extents.z : vector.z);
		bounds.extents = extents;
	}

	private static void _E001(ref Vector3 vec)
	{
		vec.x = ((vec.x > 0f) ? vec.x : (0f - vec.x));
		vec.y = ((vec.y > 0f) ? vec.y : (0f - vec.y));
		vec.z = ((vec.z > 0f) ? vec.z : (0f - vec.z));
	}

	private void Update()
	{
		try
		{
			_E002();
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			Debug.LogError(_ED3E._E000(81933));
			base.enabled = false;
		}
	}

	private void _E002()
	{
		GameWorld gameWorld = null;
		if (Singleton<GameWorld>.Instantiated)
		{
			gameWorld = Singleton<GameWorld>.Instance;
		}
		if (gameWorld == null || gameWorld.SharedBallisticsCalculator == null)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		float num = MaxDistance / deltaTime;
		_EC1E sharedBallisticsCalculator = gameWorld.SharedBallisticsCalculator;
		for (int i = 0; i < sharedBallisticsCalculator.ActiveShotsCount; i++)
		{
			_EC26 activeShot = sharedBallisticsCalculator.GetActiveShot(i);
			bool num2 = (float)activeShot.PositionHistory.Count * activeShot.Speed > num;
			float sqrMagnitude = (activeShot.PositionHistory[0] - activeShot.PositionHistory[activeShot.PositionHistory.Count - 1]).sqrMagnitude;
			if ((num2 || activeShot.BulletState != 0) && sqrMagnitude > Mathf.Epsilon && UnityEngine.Random.value < Probobility)
			{
				if (!this.m__E000.Contains(activeShot))
				{
					_E003(activeShot);
					this.m__E000.Add(activeShot);
				}
				else if (activeShot.IsShotFinished)
				{
					this.m__E000.Remove(activeShot);
				}
			}
		}
	}

	private void LateUpdate()
	{
		_E005.SetFloat(_E009, Time.time);
		if (_E006)
		{
			this.m__E003.vertices = this.m__E001;
			this.m__E003.tangents = this.m__E002;
			this.m__E003.bounds = _E004;
			_E006 = false;
		}
	}

	private void _E003(_EC26 shot)
	{
		if (shot.FragmentIndex == 0)
		{
			List<Vector3> positionHistory = shot.PositionHistory;
			Vector3 vector = positionHistory[0];
			Vector3 direction = positionHistory[positionHistory.Count - 1] - vector;
			float magnitude = direction.magnitude;
			direction *= Math.Min(magnitude, MaxDistance) / magnitude;
			EmitSeg(vector, direction, Time.time);
		}
	}
}
