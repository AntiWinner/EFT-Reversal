using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Systems.Effects;
using Comfort.Common;
using EFT.Ballistics;
using EFT.NetworkPackets;
using UnityEngine;
using UnityEngine.Serialization;

namespace EFT.Interactive;

[ExecuteInEditMode]
[_E2E2(22001)]
public sealed class WindowBreaker : MonoBehaviour, _EC07, WindowBreakerManager._E000
{
	public enum RotationAngle
	{
		Random,
		Square90,
		HorizontalRectangle180,
		VerticalRectangle180
	}

	private sealed class _E000
	{
		public string Id;

		public _EC12 Description;

		public Vector2 Center;

		public WindowBreakingConfig._E000 MeshPiece;

		public bool Stuck;

		public bool Edge;

		public float Mass;
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public WindowBreaker _003C_003E4__this;

		public int id;

		public _EC12 piece;

		internal void _E000(_EC23 info)
		{
			_003C_003E4__this._E015(in info, id);
			_003C_003E4__this._E00F();
		}

		internal void _E001(Vector3 p, Vector3 d)
		{
			piece.ChildBoxCollider.enabled = false;
			_003C_003E4__this._E014(p, d, id);
		}
	}

	public string Id;

	public bool BrakeByDefault;

	public Collider ObstructiveCollider;

	public BallisticCollider GlassBallisticCollider;

	[CompilerGenerated]
	private Vector3? m__E000;

	private int m__E001;

	private const string m__E002 = "window";

	private const float m__E003 = 9.81f;

	public WindowBreakingConfig BrokenWindow;

	public Material Material;

	public Renderer Renderer;

	[FormerlySerializedAs("RotationAngle")]
	[Header("Window")]
	public RotationAngle AngleMode;

	public float MinThickness = 0.002f;

	public float ThicknessMultyplier = 1f;

	public float EdgesWidth = 0.005f;

	public float CracksScale = 1f;

	[Header("Part")]
	public float MassMultyplier = 1f;

	public float AirDrag = 2f;

	public float TimeUntilPartDie = 1f;

	[Header("Shots")]
	public float FirstShotRadius = 0.4f;

	public float ShotRadius = 0.6f;

	public float FirstShotSoundVolume = 1f;

	public Vector2 TorqueRandomMinMaxCoefs = new Vector2(2f, 6f);

	public float InstantExplosionCoef = 15f;

	public float ShotDirectionCoef = 1.5f;

	[Space(32f)]
	[HideInInspector]
	[SerializeField]
	private int AxisX;

	[HideInInspector]
	[Space(32f)]
	[SerializeField]
	private int AxisY;

	[HideInInspector]
	[SerializeField]
	[Space(32f)]
	private int AxisZ;

	[HideInInspector]
	[SerializeField]
	private Vector2 UvMult;

	[HideInInspector]
	[SerializeField]
	private Vector2 UvAdd;

	[SerializeField]
	[HideInInspector]
	private Vector2 ZSurfs;

	[HideInInspector]
	[SerializeField]
	private Vector4 Box;

	[HideInInspector]
	[SerializeField]
	private Quaternion Rotation;

	[HideInInspector]
	[SerializeField]
	private bool NeedToSwap;

	private int m__E004;

	private float m__E005;

	private _E000[] m__E006;

	private MeshFilter m__E007;

	private Vector2 m__E008;

	private Vector2 m__E009 = new Vector2(1.5f, 6f);

	private Mesh m__E00A;

	private MeshCollider m__E00B;

	[CompilerGenerated]
	private static Action<WindowBreaker, _EC23, WindowBreakingConfig.Crack, float> m__E00C;

	private static readonly List<_E000> m__E00D = new List<_E000>(128);

	private static readonly float[] m__E00E = new float[3];

	public Vector3? FirstHitPosition
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E000 = value;
		}
	}

	public bool IsDamaged => FirstHitPosition.HasValue;

	public int NetId
	{
		get
		{
			if (string.IsNullOrWhiteSpace(Id))
			{
				return 0;
			}
			return Id.GetHashCode();
		}
	}

	public bool AvailableToSync => NetId != 0;

	GameObject _EC07.GameObject => base.gameObject;

	public string TypeKey => _ED3E._E000(209449);

	string _EC07.IdEditable
	{
		get
		{
			return Id;
		}
		set
		{
			Id = value;
		}
	}

	private int _E00F => Math.Abs(NetId);

	private bool _E010
	{
		get
		{
			if (this.m__E006 != null)
			{
				return this.m__E006.Length != 0;
			}
			return false;
		}
	}

	public static event Action<WindowBreaker, _EC23, WindowBreakingConfig.Crack, float> OnWindowHitAction
	{
		[CompilerGenerated]
		add
		{
			Action<WindowBreaker, _EC23, WindowBreakingConfig.Crack, float> action = WindowBreaker.m__E00C;
			Action<WindowBreaker, _EC23, WindowBreakingConfig.Crack, float> action2;
			do
			{
				action2 = action;
				Action<WindowBreaker, _EC23, WindowBreakingConfig.Crack, float> value2 = (Action<WindowBreaker, _EC23, WindowBreakingConfig.Crack, float>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref WindowBreaker.m__E00C, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<WindowBreaker, _EC23, WindowBreakingConfig.Crack, float> action = WindowBreaker.m__E00C;
			Action<WindowBreaker, _EC23, WindowBreakingConfig.Crack, float> action2;
			do
			{
				action2 = action;
				Action<WindowBreaker, _EC23, WindowBreakingConfig.Crack, float> value2 = (Action<WindowBreaker, _EC23, WindowBreakingConfig.Crack, float>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref WindowBreaker.m__E00C, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private void OnValidate()
	{
		_E000();
	}

	private void _E000()
	{
	}

	private void Awake()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		this.m__E001 = this._E00F;
		_E004();
		_E007();
		FirstHitPosition = null;
		if (BrakeByDefault)
		{
			FirstHitPosition = Vector3.zero;
			_E00A();
			return;
		}
		if (GlassBallisticCollider != null)
		{
			GlassBallisticCollider.HitType = EHitType.Window;
			GlassBallisticCollider.NetId = NetId;
			GlassBallisticCollider.OnHitAction += _E001;
		}
		else
		{
			Debug.LogError(_ED3E._E000(209440), this);
		}
		if (AvailableToSync && Singleton<GameWorld>.Instantiated)
		{
			Singleton<GameWorld>.Instance.RegisterWindow(this);
		}
	}

	private void _E001(_EC23 hitInfo)
	{
		MakeHit(in hitInfo);
	}

	public void MakeHit(in _EC23 hitInfo, bool instantFall = false)
	{
		if (!IsDamaged)
		{
			FirstHitPosition = hitInfo.HitPoint;
			_E013(in hitInfo, instantFall);
			_E003();
		}
	}

	public void TryToBlowUp(IExplosiveItem grenadeItem, in Vector3 grenadePosition)
	{
		Vector3 vector = grenadePosition - base.transform.position;
		float magnitude = vector.magnitude;
		if (_E002(grenadeItem, magnitude))
		{
			if (!IsDamaged)
			{
				FirstHitPosition = grenadePosition;
			}
			int piecesToDestroy = (int)Mathf.Lerp(1f, 10f, (grenadeItem.MaxExplosionDistance - magnitude) / grenadeItem.MaxExplosionDistance);
			Vector3 force = vector.normalized * (grenadeItem.MaxExplosionDistance - magnitude);
			_E016(in force, piecesToDestroy);
			_E003();
		}
	}

	private bool _E002(IExplosiveItem item, float distance)
	{
		if (distance <= item.MinExplosionDistance * 1.5f)
		{
			return true;
		}
		if (distance > item.MaxExplosionDistance * 1.5f)
		{
			return false;
		}
		float num = 1f - (distance - item.MinExplosionDistance) / (item.MaxExplosionDistance - item.MinExplosionDistance);
		if (num <= 0f)
		{
			return false;
		}
		return _E8EE.Float(this.m__E001) <= num;
	}

	private void _E003()
	{
		if (GlassBallisticCollider != null)
		{
			GlassBallisticCollider.UnsubscribeHitAction();
		}
	}

	private void _E004()
	{
		if (GlassBallisticCollider == null)
		{
			GlassBallisticCollider = GetComponent<BallisticCollider>();
			if (GlassBallisticCollider == null)
			{
				Debug.LogError(_ED3E._E000(209517), this);
			}
		}
		if (Renderer == null)
		{
			Renderer = GetComponent<Renderer>();
			if (Renderer == null)
			{
				Debug.LogError(_ED3E._E000(209553), this);
			}
		}
		if (Material == null)
		{
			if (Renderer != null)
			{
				Material = Renderer.sharedMaterial;
			}
			else
			{
				Debug.LogError(_ED3E._E000(209596), this);
			}
		}
	}

	private void _E005()
	{
		if (Renderer == null)
		{
			Debug.LogError(_ED3E._E000(209583) + base.name + _ED3E._E000(209600) + base.gameObject.scene.name + _ED3E._E000(147706), this);
		}
		else if (Renderer.GetComponent<MeshFilter>() == null)
		{
			Debug.LogError(_ED3E._E000(209652) + base.name + _ED3E._E000(209600) + base.gameObject.scene.name + _ED3E._E000(147706), this);
		}
		else if (Renderer.GetComponent<MeshFilter>().sharedMesh == null)
		{
			Debug.LogError(_ED3E._E000(209673) + base.name + _ED3E._E000(209600) + base.gameObject.scene.name + _ED3E._E000(147706), this);
		}
	}

	private void _E006()
	{
		MeshFilter component = Renderer.GetComponent<MeshFilter>();
		Vector3[] vertices = component.sharedMesh.vertices;
		Vector2[] uv = component.sharedMesh.uv;
		if (vertices.Length == 0)
		{
			Debug.LogError(_ED3E._E000(209696) + base.gameObject.name + _ED3E._E000(209735));
			return;
		}
		Bounds bounds = new Bounds(vertices[0], Vector3.zero);
		Vector3[] array = vertices;
		foreach (Vector3 point in array)
		{
			bounds.Encapsulate(point);
		}
		WindowBreakingConfig.DetectPlane(bounds.size, out AxisX, out AxisY, out AxisZ);
		float num = (bounds.min[AxisZ] + bounds.max[AxisZ]) * 0.5f;
		float a = (bounds.max[AxisZ] - num) * ThicknessMultyplier;
		a = Mathf.Max(a, MinThickness);
		ZSurfs = new Vector2(num - a, num + a);
		float num2 = vertices[0][AxisX] + vertices[0][AxisY];
		int num3 = 0;
		float num4 = num2;
		int num5 = num3;
		float num6 = vertices[0][AxisX] - vertices[0][AxisY];
		int num7 = 0;
		for (int j = 0; j < vertices.Length; j++)
		{
			Vector3 vector = vertices[j];
			if (!(vector[AxisZ] > num))
			{
				float num8 = vector[AxisX] + vector[AxisY];
				float num9 = vector[AxisX] - vector[AxisY];
				if (num8 > num4)
				{
					num5 = j;
					num4 = num8;
				}
				if (num8 < num2)
				{
					num3 = j;
					num2 = num8;
				}
				if (num9 > num6)
				{
					num7 = j;
					num6 = num9;
				}
			}
		}
		Vector2 v = new Vector2(vertices[num3][AxisX], vertices[num3][AxisY]);
		Vector2 vector2 = new Vector2(vertices[num5][AxisX], vertices[num5][AxisY]);
		Vector2 v2 = new Vector2(vertices[num7][AxisX], vertices[num7][AxisY]);
		Vector2 vector3 = uv[num3];
		Vector2 vector4 = uv[num5];
		Vector2 uv2 = uv[num7];
		NeedToSwap = _E008(v, v2, vector3, uv2);
		if (NeedToSwap)
		{
			int i = AxisY;
			int axisX = AxisX;
			AxisX = i;
			AxisY = axisX;
		}
		v = new Vector2(vertices[num3][AxisX], vertices[num3][AxisY]);
		vector2 = new Vector2(vertices[num5][AxisX], vertices[num5][AxisY]);
		Vector2 a2 = v;
		Vector2 vector5 = vector2 - v;
		Vector2 vector6 = vector3;
		Vector2 vector7 = vector4 - vector3;
		Vector3 zero = Vector3.zero;
		zero[AxisX] = 1f;
		Vector3 zero2 = Vector3.zero;
		zero2[AxisY] = 1f;
		Rotation = Quaternion.LookRotation(Vector3.Cross(zero, zero2), zero2);
		UvMult = new Vector2(vector7.x / vector5.x, vector7.y / vector5.y);
		UvAdd = vector6 - Vector2.Scale(a2, UvMult);
		a2 = Vector2.Min(v, vector2);
		Vector2 vector8 = Vector2.Max(v, vector2);
		Box = new Vector4(a2.x, a2.y, vector8.x, vector8.y);
	}

	private void _E007()
	{
		_E005();
		this.m__E007 = Renderer.GetComponent<MeshFilter>();
		this.m__E00B = GetComponent<MeshCollider>();
		if (MonoBehaviourSingleton<BetterAudio>.Instance != null)
		{
			MonoBehaviourSingleton<BetterAudio>.Instance.PrecacheGag(_ED3E._E000(209449));
		}
	}

	private static bool _E008(Vector2 v0, Vector2 v1, Vector2 uv0, Vector2 uv1)
	{
		Vector2 vector = v1 - v0;
		Vector2 vector2 = uv1 - uv0;
		bool num = Mathf.Abs(vector.x) > Mathf.Abs(vector.y);
		bool flag = Mathf.Abs(vector2.x) > Mathf.Abs(vector2.y);
		return num != flag;
	}

	private void _E009(WindowBreakingConfig.Crack crack, in Vector3 position, in Vector3 force, float angle, bool isStatic = false, bool instantFall = false)
	{
		_E01A();
		if (ObstructiveCollider != null)
		{
			ObstructiveCollider.gameObject.SetActive(value: false);
		}
		Vector3 vector = base.transform.InverseTransformPoint(position);
		Vector2 vector2 = new Vector2(vector[AxisX], vector[AxisY]);
		Vector2 vector3 = new Vector2(Vector3.zero[AxisX], Vector3.zero[AxisY]);
		float scale = _E8EE.Range(this.m__E001, CracksScale, CracksScale + 1f);
		if (AngleMode != 0)
		{
			Vector2 vector4 = vector3;
			Vector2 vector5 = vector2;
			vector2 = vector4;
			vector3 = vector5;
		}
		this.m__E008 = vector2;
		this.m__E006 = _E011(crack, vector2, vector3, angle, scale);
		for (int i = 0; i < this.m__E006.Length; i++)
		{
			_E000 obj = this.m__E006[i];
			if (!isStatic || obj.Stuck)
			{
				obj.Description = _E00B(obj, vector2);
				if (WindowsManager.InstanceIsActive())
				{
					WindowsManager.Instance.AddPiece(Id, obj.Id, obj.Description.MeshRenderer, obj.MeshPiece.GetMesh(obj.Center), Material);
				}
			}
			if (obj.Stuck)
			{
				_E00C(obj.Description, i);
			}
			else if (!isStatic)
			{
				_E00D(obj, wasStuck: false, in position, in force, instantFall);
			}
		}
		_E00F();
		this.m__E00B.enabled = false;
		if (WindowsManager.InstanceIsActive())
		{
			WindowsManager.Instance.BreakWindow(Id);
		}
	}

	private void _E00A()
	{
		Bounds bounds = this.m__E007.sharedMesh.bounds;
		Vector3 position = bounds.center + Vector3.Scale(_E018(this.m__E001, -1f, 1f), bounds.extents);
		float angle = (float)(this.m__E001 % 512) / 512f;
		WindowBreakingConfig.Crack crack = _E019();
		Vector3 position2 = base.transform.TransformPoint(position);
		Vector3 force = Vector3.zero;
		_E009(crack, in position2, in force, angle, isStatic: true, instantFall: true);
	}

	private _EC12 _E00B(_E000 piece, Vector2 add)
	{
		_EC12 result = ((WindowBreakerManager.Instance != null) ? WindowBreakerManager.Instance.GetBrokenWindowPiece() : WindowBreakerManager.CreateBrokenWindowPiece());
		this.m__E00A = piece.MeshPiece.GetMesh(piece.Center);
		GameObject gameObject = result.GameObject;
		result.MeshFilter.sharedMesh = this.m__E00A;
		result.ChildFragileCollider.enabled = true;
		gameObject.transform.parent = base.transform;
		Vector3 vector = piece.Center + add;
		vector.z = (0f - (ZSurfs.x + ZSurfs.y)) * 1f;
		gameObject.transform.localPosition = Rotation * vector;
		gameObject.transform.localRotation = Rotation;
		gameObject.transform.localScale = Vector3.one;
		gameObject.SetActive(value: true);
		return result;
	}

	private void _E00C(_EC12 piece, int id)
	{
		piece.MeshCollider.sharedMesh = this.m__E00A;
		piece.ChildBoxCollider.enabled = true;
		piece.ChildBoxCollider.size = piece.MeshCollider.bounds.size * 0.8f;
		piece.BallisticCollider.enabled = true;
		piece.BallisticCollider.TakeSettingsFrom(GlassBallisticCollider);
		piece.BallisticCollider.OnHitAction += delegate(_EC23 info)
		{
			_E015(in info, id);
			_E00F();
		};
		piece.ChildTransform.position = piece.MeshCollider.bounds.center;
		piece.ChildTransform.rotation = Quaternion.identity;
		piece.ChildFragileCollider.OnPlayerCollision += delegate(Vector3 p, Vector3 d)
		{
			piece.ChildBoxCollider.enabled = false;
			_E014(p, d, id);
		};
		piece.GameObject.layer = base.gameObject.layer;
	}

	private void _E00D(_E000 piece, bool wasStuck, in Vector3 position, in Vector3 force, bool instantFall = false)
	{
		if (wasStuck)
		{
			piece.Description.MeshCollider.enabled = false;
			piece.Description.BallisticCollider.UnsubscribeHitAction();
			piece.Description.BallisticCollider.enabled = false;
		}
		piece.Description.MeshRenderer.enabled = !WindowsManager.InstanceIsActive();
		piece.Description.MeshRenderer.sharedMaterial = Material;
		if (instantFall)
		{
			_E010(piece);
			return;
		}
		float destroyTime = UnityEngine.Random.Range(TimeUntilPartDie * 0.5f, TimeUntilPartDie * 1.5f);
		_E00E(piece, destroyTime, position, force.normalized).HandleExceptions();
	}

	private async Task _E00E(_E000 stuckPiece, float destroyTime, Vector3 position, Vector3 force)
	{
		Transform transform = stuckPiece.Description.GameObject.transform;
		Vector3 eulers = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(TorqueRandomMinMaxCoefs.x, TorqueRandomMinMaxCoefs.y);
		Vector3 position2 = transform.position;
		Vector3 vector = position2 + (position2 - position) / InstantExplosionCoef;
		float num = 0f;
		while (num <= destroyTime && transform != null)
		{
			num += Time.deltaTime;
			transform.Rotate(eulers);
			transform.position = vector + 9.81f * num * num / 2f * Vector3.down + num * ShotDirectionCoef * force;
			if (WindowsManager.InstanceIsActive())
			{
				WindowsManager.Instance.UpdatePieceTransform(stuckPiece.Id, transform);
			}
			await Task.Yield();
		}
		_E010(stuckPiece);
	}

	private void _E00F()
	{
		if (!WindowsManager.InstanceIsActive())
		{
			this.m__E007.sharedMesh = _E012(this.m__E006, this.m__E008, AxisX, AxisY, AxisZ, ZSurfs);
			Renderer.enabled = this.m__E006.Any((_E000 x) => x.Stuck);
		}
	}

	private static void _E010(_E000 piece)
	{
		piece.Description.ChildFragileCollider.UnsubscibeAction();
		if (WindowsManager.InstanceIsActive())
		{
			WindowsManager.Instance.RemovePiece(piece.Id);
		}
		if (WindowBreakerManager.Instance == null)
		{
			UnityEngine.Object.DestroyImmediate(piece.Description.GameObject);
		}
		else
		{
			WindowBreakerManager.Instance.ReturnToPool(piece.Description);
		}
	}

	private _E000[] _E011(WindowBreakingConfig.Crack crack, Vector2 add, Vector2 shift, float angle, float scale)
	{
		Vector4 box = Box;
		box.x -= add.x;
		box.z -= add.x;
		box.y -= add.y;
		box.w -= add.y;
		float num = UnityEngine.Random.Range(FirstShotRadius * 0.9f, FirstShotRadius * 1.1f);
		Vector2 xAxis = WindowBreakingConfig.GetXAxis(angle) * scale;
		for (int i = 0; i < crack.Polygons.Length; i++)
		{
			WindowBreakingConfig.Polygon polygon = crack.Polygons[i].CutPolygon(box, xAxis);
			if (polygon.Points != null && polygon.Points.Length >= 3)
			{
				Vector2 vector = Vector3.zero;
				Vector2[] points = polygon.Points;
				foreach (Vector2 vector2 in points)
				{
					vector += vector2;
				}
				vector /= (float)polygon.Points.Length;
				WindowBreakingConfig._E000 meshPiece = WindowBreakingConfig.GenerateMeshPice(polygon.Points, NeedToSwap, UvMult, UvAdd + Vector2.Scale(add, UvMult), ZSurfs, EdgesWidth);
				_E000 item = new _E000
				{
					Id = Id + _ED3E._E000(209791) + this.m__E004,
					MeshPiece = meshPiece,
					Center = vector,
					Stuck = ((shift - vector).magnitude > num),
					Edge = polygon.Intersects,
					Mass = polygon.Mass * MassMultyplier
				};
				this.m__E004++;
				WindowBreaker.m__E00D.Add(item);
			}
		}
		_E000[] result = WindowBreaker.m__E00D.ToArray();
		WindowBreaker.m__E00D.Clear();
		return result;
	}

	private static Mesh _E012(_E000[] pieces, Vector2 add, int axisX, int axisY, int axisZ, Vector2 zSurfs)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < pieces.Length; i++)
		{
			if (pieces[i].Stuck)
			{
				WindowBreakingConfig._E000 meshPiece = pieces[i].MeshPiece;
				num += meshPiece.Vertices.Length;
				num2 += meshPiece.Triangles.Length;
			}
		}
		if (num == 0)
		{
			return null;
		}
		int[] array = new int[num2];
		Vector3[] array2 = new Vector3[num];
		Vector3[] array3 = new Vector3[num];
		Vector2[] array4 = new Vector2[num];
		int num3 = 0;
		int num4 = 0;
		for (int j = 0; j < pieces.Length; j++)
		{
			if (pieces[j].Stuck)
			{
				WindowBreakingConfig._E000 meshPiece2 = pieces[j].MeshPiece;
				meshPiece2.Vertices.CopyTo(array2, num3);
				meshPiece2.Normals.CopyTo(array3, num3);
				meshPiece2.Uv.CopyTo(array4, num3);
				for (int k = 0; k < meshPiece2.Triangles.Length; k++)
				{
					array[num4++] = meshPiece2.Triangles[k] + num3;
				}
				num3 += meshPiece2.Vertices.Length;
			}
		}
		for (int l = 0; l < array2.Length; l++)
		{
			array2[l].x += add.x;
			array2[l].y += add.y;
		}
		float num5 = zSurfs.x + zSurfs.y;
		for (int m = 0; m < array2.Length; m++)
		{
			array2[m].z = num5 - array2[m].z;
		}
		for (int n = 0; n < array2.Length; n++)
		{
			WindowBreaker.m__E00E[axisX] = array2[n].x;
			WindowBreaker.m__E00E[axisY] = array2[n].y;
			WindowBreaker.m__E00E[axisZ] = array2[n].z;
			array2[n].x = WindowBreaker.m__E00E[0];
			array2[n].y = WindowBreaker.m__E00E[1];
			array2[n].z = WindowBreaker.m__E00E[2];
		}
		for (int num6 = 0; num6 < array3.Length; num6++)
		{
			WindowBreaker.m__E00E[axisX] = array3[num6].x;
			WindowBreaker.m__E00E[axisY] = array3[num6].y;
			WindowBreaker.m__E00E[axisZ] = 0f - array3[num6].z;
			array3[num6].x = WindowBreaker.m__E00E[0];
			array3[num6].y = WindowBreaker.m__E00E[1];
			array3[num6].z = WindowBreaker.m__E00E[2];
		}
		return new Mesh
		{
			vertices = array2,
			triangles = array,
			normals = array3,
			uv = array4,
			name = _ED3E._E000(209782)
		};
	}

	private void _E013(in _EC23 info, bool instantFall)
	{
		WindowBreakingConfig.Crack crack = _E019();
		float num = _E017();
		if (!instantFall && WindowBreakerManager.Instance != null)
		{
			WindowBreaker.m__E00C?.Invoke(this, info, crack, num);
		}
		else
		{
			_E009(crack, in info.HitPoint, in info.Direction, num, instantFall);
		}
		if (!instantFall)
		{
			_E01B(in info.HitPoint);
		}
	}

	private void _E014(Vector3 hitpoint, Vector3 direction, int id)
	{
		Vector2 center = this.m__E006[id].Center;
		float num = UnityEngine.Random.Range(ShotRadius * 0.6f, ShotRadius * 0.8f);
		_E000[] array = this.m__E006;
		foreach (_E000 obj in array)
		{
			if (obj.Stuck)
			{
				float num2 = (obj.Center - center).magnitude;
				if (obj.Edge)
				{
					num2 *= 1.5f;
				}
				if (num2 < num)
				{
					_E00D(obj, wasStuck: true, in hitpoint, in direction);
					obj.Stuck = false;
				}
			}
		}
		_E01C(in hitpoint);
		_E00F();
	}

	private void _E015(in _EC23 info, int id)
	{
		Vector2 center = this.m__E006[id].Center;
		float num = _E8EE.Range(this.m__E001 + id, ShotRadius * 0.9f, ShotRadius * 1.1f);
		_E000[] array = this.m__E006;
		foreach (_E000 obj in array)
		{
			if (obj.Stuck)
			{
				float num2 = (obj.Center - center).magnitude;
				if (obj.Edge)
				{
					num2 *= 1.5f;
				}
				if (num2 < num)
				{
					_E00D(obj, wasStuck: true, in info.HitPoint, in info.Direction);
					obj.Stuck = false;
				}
			}
		}
		_E01C(in info.HitPoint);
	}

	private void _E016(in Vector3 force, int piecesToDestroy)
	{
		if (this.m__E007.sharedMesh == null)
		{
			return;
		}
		Bounds bounds = this.m__E007.sharedMesh.bounds;
		Vector3 position = bounds.center + Vector3.Scale(_E018(this.m__E001, -0.5f, 0.5f), bounds.extents);
		_EC23 obj = default(_EC23);
		obj.DamageType = EDamageType.Blunt;
		obj.HitPoint = base.transform.TransformPoint(position);
		obj.Direction = force;
		_EC23 info = obj;
		if (!this._E010)
		{
			_E013(in info, instantFall: false);
			return;
		}
		int num = this.m__E006.Length;
		for (int i = 0; i < piecesToDestroy && i < num; i++)
		{
			_E015(in info, _E8EE.Int(this.m__E001 + i + piecesToDestroy, 0, num));
		}
		_E00F();
	}

	private float _E017()
	{
		return AngleMode switch
		{
			RotationAngle.Random => _E8EE.Float(this.m__E001), 
			RotationAngle.Square90 => (float)_E8EE.Int(this.m__E001, 0, 3) / 4f, 
			RotationAngle.HorizontalRectangle180 => ((float)_E8EE.Int(this.m__E001, 0, 1) + 1f) / 2f, 
			RotationAngle.VerticalRectangle180 => (float)_E8EE.Int(this.m__E001, 0, 1) / 2f, 
			_ => 0f, 
		};
	}

	private static Vector3 _E018(int seed, float min, float max)
	{
		return new Vector3(_E8EE.Range(seed, min, max), _E8EE.Range(seed + 1, min, max), _E8EE.Range(seed + 2, min, max));
	}

	private WindowBreakingConfig.Crack _E019()
	{
		return BrokenWindow.Cracks[this.m__E001 % BrokenWindow.Cracks.Length];
	}

	private void _E01A()
	{
		if (this._E010)
		{
			_E000[] array = this.m__E006;
			for (int i = 0; i < array.Length; i++)
			{
				_E010(array[i]);
			}
			this.m__E006 = null;
		}
	}

	private void _E01B(in Vector3 hitPoint)
	{
		SoundBank soundBank = Singleton<Effects>.Instance.AdditionalSoundEffects[1];
		if (!(soundBank == null))
		{
			float distance = _E8A8.Instance.Distance(hitPoint);
			MonoBehaviourSingleton<BetterAudio>.Instance.LimitedPlay(hitPoint, soundBank, distance, this.m__E009, 1f, FirstShotSoundVolume, -1f, EnvironmentType.Outdoor, EOcclusionTest.Fast, _ED3E._E000(209449));
		}
	}

	private void _E01C(in Vector3 hitPoint)
	{
		SoundBank soundBank = Singleton<Effects>.Instance.AdditionalSoundEffects[2];
		if (soundBank != null && Time.time > this.m__E005)
		{
			float distance = _E8A8.Instance.Distance(hitPoint);
			MonoBehaviourSingleton<BetterAudio>.Instance.LimitedPlay(hitPoint, soundBank, distance, this.m__E009, 0.5f, 1f, -1f, EnvironmentType.Outdoor, EOcclusionTest.Fast, _ED3E._E000(209449));
			this.m__E005 = Time.time + soundBank.ClipLength / 2f;
		}
	}

	void WindowBreakerManager._E000.Break(WindowBreakingConfig.Crack crack, in Vector3 position, in Vector3 force, float angle)
	{
		_E009(crack, in position, in force, angle);
	}
}
