using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace ChartAndGraph;

public class CanvasLines : MaskableGraphic
{
	internal struct _E000
	{
		[CompilerGenerated]
		private bool m__E000;

		[CompilerGenerated]
		private Vector3 _E001;

		[CompilerGenerated]
		private Vector3 _E002;

		[CompilerGenerated]
		private Vector3 _E003;

		[CompilerGenerated]
		private Vector3 _E004;

		[CompilerGenerated]
		private Vector3 _E005;

		[CompilerGenerated]
		private Vector3 _E006;

		[CompilerGenerated]
		private Vector3 _E007;

		[CompilerGenerated]
		private float _E008;

		[CompilerGenerated]
		private Vector3 _E009;

		public bool Degenerated
		{
			[CompilerGenerated]
			get
			{
				return m__E000;
			}
			[CompilerGenerated]
			private set
			{
				m__E000 = value;
			}
		}

		public Vector3 P1
		{
			[CompilerGenerated]
			get
			{
				return _E001;
			}
			[CompilerGenerated]
			private set
			{
				_E001 = value;
			}
		}

		public Vector3 P2
		{
			[CompilerGenerated]
			get
			{
				return _E002;
			}
			[CompilerGenerated]
			private set
			{
				_E002 = value;
			}
		}

		public Vector3 P3
		{
			[CompilerGenerated]
			get
			{
				return _E003;
			}
			[CompilerGenerated]
			private set
			{
				_E003 = value;
			}
		}

		public Vector3 P4
		{
			[CompilerGenerated]
			get
			{
				return _E004;
			}
			[CompilerGenerated]
			private set
			{
				_E004 = value;
			}
		}

		public Vector3 From
		{
			[CompilerGenerated]
			get
			{
				return _E005;
			}
			[CompilerGenerated]
			private set
			{
				_E005 = value;
			}
		}

		public Vector3 To
		{
			[CompilerGenerated]
			get
			{
				return _E006;
			}
			[CompilerGenerated]
			private set
			{
				_E006 = value;
			}
		}

		public Vector3 Dir
		{
			[CompilerGenerated]
			get
			{
				return _E007;
			}
			[CompilerGenerated]
			private set
			{
				_E007 = value;
			}
		}

		public float Mag
		{
			[CompilerGenerated]
			get
			{
				return _E008;
			}
			[CompilerGenerated]
			private set
			{
				_E008 = value;
			}
		}

		public Vector3 Normal
		{
			[CompilerGenerated]
			get
			{
				return _E009;
			}
			[CompilerGenerated]
			private set
			{
				_E009 = value;
			}
		}

		public _E000(Vector3 from, Vector3 to, float halfThickness, bool hasNext, bool hasPrev)
		{
			this = default(_E000);
			Vector3 vector = to - from;
			float num = 0f;
			if (hasNext)
			{
				num += halfThickness;
			}
			if (hasPrev)
			{
				num += halfThickness;
			}
			Mag = vector.magnitude - num * 2f;
			Degenerated = false;
			if (Mag <= 0f)
			{
				Degenerated = true;
			}
			Dir = vector.normalized;
			Vector3 vector2 = halfThickness * 2f * Dir;
			if (hasPrev)
			{
				from += vector2;
			}
			if (hasNext)
			{
				to -= vector2;
			}
			From = from;
			To = to;
			Normal = new Vector3(Dir.y, 0f - Dir.x, Dir.z);
			P1 = From + Normal * halfThickness;
			P2 = from - Normal * halfThickness;
			P3 = to + Normal * halfThickness;
			P4 = to - Normal * halfThickness;
		}
	}

	internal class _E001
	{
		private List<Vector4> _E000 = new List<Vector4>();

		public int PointCount
		{
			get
			{
				if (_E000 == null)
				{
					return 0;
				}
				return _E000.Count;
			}
		}

		public int LineCount
		{
			get
			{
				if (_E000 == null)
				{
					return 0;
				}
				if (_E000.Count < 2)
				{
					return 0;
				}
				return _E000.Count - 1;
			}
		}

		public _E001(IList<Vector3> lines)
		{
			_E000.AddRange(lines.Select((Vector3 x) => new Vector4(x.x, x.y, x.z, -1f)));
		}

		public _E001(IList<Vector4> lines)
		{
			_E000.AddRange(lines);
		}

		public void ModifiyLines(List<Vector4> v)
		{
			_E000.Clear();
			_E000.AddRange(v);
		}

		public Vector4 getPoint(int index)
		{
			return _E000[index];
		}

		public void GetLine(int index, out Vector3 from, out Vector3 to)
		{
			from = _E000[index];
			to = _E000[index + 1];
		}

		public _E000 GetLine(int index, float halfThickness, bool hasPrev, bool hasNext)
		{
			Vector3 from = _E000[index];
			Vector3 to = _E000[index + 1];
			return new _E000(from, to, halfThickness, hasNext: false, hasPrev: false);
		}
	}

	public float Thickness = 2f;

	private float m__E000 = 1f;

	private bool m__E001;

	private bool m__E002;

	private float m__E003 = 5f;

	private Rect m__E004;

	private bool m__E005;

	private Material m__E006;

	private SensitivityControl m__E007;

	private ChartItemEffect m__E008;

	private ChartItemEffect m__E009;

	private List<ChartItemEffect> m__E00A = new List<ChartItemEffect>();

	private List<ChartItemEffect> m__E00B = new List<ChartItemEffect>();

	private List<_E001> m__E00C;

	private bool m__E00D;

	private float m__E00E;

	private float m__E00F;

	private float m__E010;

	private float m__E011;

	private int m__E012 = -1;

	private int m__E013 = -1;

	private Vector2 _E014;

	private GraphicRaycaster _E015;

	[CompilerGenerated]
	private Action<int, Vector2> _E016;

	[CompilerGenerated]
	private Action<int, Vector2> _E017;

	[CompilerGenerated]
	private Action _E018;

	private Rect? _E019;

	private _ED21 _E01A;

	private Rect? _E01B;

	private bool _E01C;

	public int refrenceIndex;

	private UIVertex[] _E01D = new UIVertex[4];

	private static readonly int _E01E = Shader.PropertyToID(_ED3E._E000(245686));

	public float Tiling
	{
		get
		{
			return this.m__E000;
		}
		set
		{
			this.m__E000 = value;
		}
	}

	public override Material material
	{
		get
		{
			return base.material;
		}
		set
		{
			_ED15.SafeDestroy(this.m__E006);
			if (value == null)
			{
				this.m__E006 = null;
				base.material = null;
				return;
			}
			this.m__E006 = new Material(value);
			this.m__E006.hideFlags = HideFlags.DontSave;
			if (this.m__E006.HasProperty(_ED3E._E000(245686)))
			{
				this.m__E006.SetFloat(_E01E, Tiling);
			}
			base.material = this.m__E006;
		}
	}

	public event Action<int, Vector2> Hover
	{
		[CompilerGenerated]
		add
		{
			Action<int, Vector2> action = _E016;
			Action<int, Vector2> action2;
			do
			{
				action2 = action;
				Action<int, Vector2> value2 = (Action<int, Vector2>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E016, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<int, Vector2> action = _E016;
			Action<int, Vector2> action2;
			do
			{
				action2 = action;
				Action<int, Vector2> value2 = (Action<int, Vector2>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E016, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<int, Vector2> Click
	{
		[CompilerGenerated]
		add
		{
			Action<int, Vector2> action = _E017;
			Action<int, Vector2> action2;
			do
			{
				action2 = action;
				Action<int, Vector2> value2 = (Action<int, Vector2>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E017, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<int, Vector2> action = _E017;
			Action<int, Vector2> action2;
			do
			{
				action2 = action;
				Action<int, Vector2> value2 = (Action<int, Vector2>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E017, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action Leave
	{
		[CompilerGenerated]
		add
		{
			Action action = _E018;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E018, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E018;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E018, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void SetViewRect(Rect r, Rect uvRect)
	{
		_E019 = r;
		_E01B = uvRect;
	}

	public ChartItemEffect LockHoverObject(int index)
	{
		int count = this.m__E00B.Count;
		ChartItemEffect chartItemEffect = null;
		if (count > 0)
		{
			chartItemEffect = this.m__E00B[count - 1];
			this.m__E00B.RemoveAt(count - 1);
		}
		else
		{
			if (this.m__E008 == null)
			{
				return null;
			}
			GameObject obj = UnityEngine.Object.Instantiate(this.m__E008.gameObject);
			MaskableGraphic component = obj.GetComponent<MaskableGraphic>();
			if (component != null)
			{
				component.maskable = false;
			}
			_ED15._E012<ChartItem>(obj);
			obj.transform.SetParent(base.transform);
			chartItemEffect = obj.GetComponent<ChartItemEffect>();
			chartItemEffect.Deactivate += _E000;
		}
		chartItemEffect._E000 = index;
		this.m__E00A.Add(chartItemEffect);
		return chartItemEffect;
	}

	private void _E000(ChartItemEffect obj)
	{
		this.m__E00A.Remove(obj);
		this.m__E00B.Add(obj);
	}

	public void SetHoverPrefab(ChartItemEffect prefab)
	{
		this.m__E008 = prefab;
	}

	public void MakePointRender(float pointSize)
	{
		this.m__E003 = pointSize;
		this.m__E002 = true;
	}

	public void MakeFillRender(Rect fillRect, bool stretchY)
	{
		this.m__E004 = fillRect;
		this.m__E001 = true;
		this.m__E005 = stretchY;
	}

	internal void _E001(List<Vector4> lines)
	{
		if (this.m__E00C.Count == 0)
		{
			this.m__E00C.Add(new _E001(lines.ToArray()));
			return;
		}
		this.m__E00C[0].ModifiyLines(lines);
		this.m__E00E = float.PositiveInfinity;
		this.m__E00F = float.PositiveInfinity;
		this.m__E010 = float.NegativeInfinity;
		this.m__E011 = float.NegativeInfinity;
		this.m__E007 = GetComponentInParent<SensitivityControl>();
		if (this.m__E00C != null)
		{
			for (int i = 0; i < this.m__E00C.Count; i++)
			{
				_E001 obj = this.m__E00C[i];
				int pointCount = obj.PointCount;
				for (int j = 0; j < pointCount; j++)
				{
					Vector3 vector = obj.getPoint(j);
					this.m__E00E = Mathf.Min(this.m__E00E, vector.x);
					this.m__E00F = Mathf.Min(this.m__E00F, vector.y);
					this.m__E010 = Mathf.Max(this.m__E010, vector.x);
					this.m__E011 = Mathf.Max(this.m__E011, vector.y);
				}
			}
		}
		SetVerticesDirty();
		Rebuild(CanvasUpdate.PostLayout);
		_E01C = true;
	}

	internal void _E002(List<_E001> lines)
	{
		this.m__E00C = lines;
		this.m__E00E = float.PositiveInfinity;
		this.m__E00F = float.PositiveInfinity;
		this.m__E010 = float.NegativeInfinity;
		this.m__E011 = float.NegativeInfinity;
		this.m__E007 = GetComponentInParent<SensitivityControl>();
		if (this.m__E00C != null)
		{
			for (int i = 0; i < this.m__E00C.Count; i++)
			{
				_E001 obj = this.m__E00C[i];
				int pointCount = obj.PointCount;
				for (int j = 0; j < pointCount; j++)
				{
					Vector3 vector = obj.getPoint(j);
					this.m__E00E = Mathf.Min(this.m__E00E, vector.x);
					this.m__E00F = Mathf.Min(this.m__E00F, vector.y);
					this.m__E010 = Mathf.Max(this.m__E010, vector.x);
					this.m__E011 = Mathf.Max(this.m__E011, vector.y);
				}
			}
		}
		SetAllDirty();
		this.m__E00D = false;
		for (int k = 0; k < this.m__E00A.Count; k++)
		{
			this.m__E00A[k].gameObject.SetActive(value: false);
			this.m__E00B.Add(this.m__E00A[k]);
		}
		this.m__E00A.Clear();
		if (this.m__E009 != null)
		{
			this.m__E009.gameObject.SetActive(value: false);
			this.m__E00B.Add(this.m__E009);
			this.m__E009 = null;
		}
		this.m__E012 = (this.m__E013 = -1);
		Rebuild(CanvasUpdate.PreRender);
	}

	protected override void UpdateMaterial()
	{
		base.UpdateMaterial();
		base.canvasRenderer.SetTexture(material.mainTexture);
	}

	private void _E003(Vector3 point, Vector3 dir, Vector3 normal, float dist, float size, float z, out Vector3 p1, out Vector3 p2)
	{
		point.z = z;
		point += dir * dist;
		normal *= size;
		p1 = point + normal;
		p2 = point - normal;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_ED15.SafeDestroy(this.m__E006);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
	}

	protected void Update()
	{
		Material material = this.material;
		if (this.m__E006 != null && material != null && this.m__E006.HasProperty(_ED3E._E000(245686)))
		{
			if (this.m__E006 != material)
			{
				this.m__E006.CopyPropertiesFromMaterial(material);
			}
			this.m__E006.SetFloat(_E01E, Tiling);
		}
		HandleMouseMove(_E01C);
		_E01C = false;
	}

	private IEnumerable<UIVertex> _E004()
	{
		if (this.m__E00C == null)
		{
			yield break;
		}
		float z = 0f;
		_ = this.m__E003 * 0.5f;
		int num = 0;
		while (num < this.m__E00C.Count)
		{
			_E001 obj = this.m__E00C[num];
			int pointCount = obj.PointCount;
			int num4;
			for (int num2 = 0; num2 < pointCount; num2 = num4)
			{
				Vector4 point = obj.getPoint(num2);
				if (point.w != 0f)
				{
					Vector3 vector = point;
					float num3 = this.m__E003 * 0.5f;
					if (point.w >= 0f)
					{
						num3 = point.w * 0.5f;
					}
					Vector3 pos = vector + new Vector3(0f - num3, 0f - num3, 0f);
					Vector3 pos2 = vector + new Vector3(num3, 0f - num3, 0f);
					Vector3 pos3 = vector + new Vector3(0f - num3, num3, 0f);
					Vector3 pos4 = vector + new Vector3(num3, num3, 0f);
					Vector2 uv = new Vector2(0f, 0f);
					Vector2 uv2 = new Vector2(1f, 0f);
					Vector2 uv3 = new Vector2(0f, 1f);
					Vector2 uv4 = new Vector2(1f, 1f);
					UIVertex uIVertex = _ED15._E00F(pos, uv, z);
					UIVertex uIVertex2 = _ED15._E00F(pos2, uv2, z);
					UIVertex uIVertex3 = _ED15._E00F(pos3, uv3, z);
					UIVertex uIVertex4 = _ED15._E00F(pos4, uv4, z);
					yield return uIVertex;
					yield return uIVertex2;
					yield return uIVertex3;
					yield return uIVertex4;
				}
				num4 = num2 + 1;
			}
			num4 = num + 1;
			num = num4;
		}
	}

	private Vector2 _E005(Vector2 uv)
	{
		if (!_E01B.HasValue)
		{
			return uv;
		}
		Rect value = _E01B.Value;
		float x = value.x + uv.x * value.width;
		float y = value.y + uv.y * value.height;
		return new Vector2(x, y);
	}

	private IEnumerable<UIVertex> _E006()
	{
		if (this.m__E00C == null)
		{
			yield break;
		}
		float z = 0f;
		int num = 0;
		while (num < this.m__E00C.Count)
		{
			_E001 obj = this.m__E00C[num];
			int lineCount = obj.LineCount;
			int num3;
			for (int num2 = 0; num2 < lineCount; num2 = num3)
			{
				obj.GetLine(num2, out var from, out var to);
				Vector2 to2 = to;
				Vector2 from2 = from;
				_E00E(this.m__E004.xMin, this.m__E004.yMin, this.m__E004.xMax, this.m__E004.yMin, xAxis: true, oposite: false, ref from2, ref to2);
				to = new Vector3(to2.x, to2.y, to.z);
				from = new Vector3(from2.x, from2.y, from.z);
				Vector3 pos = from;
				Vector3 pos2 = to;
				pos.y = this.m__E004.yMin;
				pos2.y = this.m__E004.yMin;
				float y = 1f;
				float y2 = 1f;
				if (!this.m__E005)
				{
					y = Mathf.Abs((from.y - this.m__E004.yMin) / this.m__E004.height);
					y2 = Mathf.Abs((to.y - this.m__E004.yMin) / this.m__E004.height);
				}
				float x = (from.x - this.m__E004.xMin) / this.m__E004.width;
				float x2 = (to.x - this.m__E004.xMin) / this.m__E004.width;
				Vector2 uv = _E005(new Vector2(x, y));
				Vector2 uv2 = _E005(new Vector2(x2, y2));
				Vector2 uv3 = _E005(new Vector2(x, 0f));
				Vector2 uv4 = _E005(new Vector2(x2, 0f));
				UIVertex uIVertex = _ED15._E00F(from, uv, z);
				UIVertex uIVertex2 = _ED15._E00F(to, uv2, z);
				UIVertex uIVertex3 = _ED15._E00F(pos, uv3, z);
				UIVertex uIVertex4 = _ED15._E00F(pos2, uv4, z);
				yield return uIVertex;
				yield return uIVertex2;
				yield return uIVertex3;
				yield return uIVertex4;
				num3 = num2 + 1;
			}
			num3 = num + 1;
			num = num3;
		}
	}

	private IEnumerable<UIVertex> _E007()
	{
		if (this.m__E00C == null)
		{
			yield break;
		}
		float num = Thickness * 0.5f;
		float num2 = 0f;
		int num3 = 0;
		while (num3 < this.m__E00C.Count)
		{
			_E001 obj = this.m__E00C[num3];
			int lineCount = obj.LineCount;
			_E000? obj2 = null;
			_E000? obj3 = null;
			float num4 = 0f;
			float num5 = 0f;
			for (int i = 0; i < lineCount; i++)
			{
				num5 += obj.GetLine(i, num, hasPrev: false, hasNext: false).Mag;
			}
			int num7;
			for (int num6 = 0; num6 < lineCount; num6 = num7)
			{
				bool hasNext = num6 + 1 < lineCount;
				_E000 value = ((!obj2.HasValue) ? obj.GetLine(num6, num, obj3.HasValue, hasNext) : obj2.Value);
				obj2 = null;
				if (num6 + 1 < lineCount)
				{
					obj2 = obj.GetLine(num6 + 1, num, hasPrev: true, num6 + 2 < lineCount);
				}
				Vector3 p = value.P1;
				Vector3 p2 = value.P2;
				Vector3 p3 = value.P3;
				Vector3 p4 = value.P4;
				Vector2 uv = new Vector2(num4 * Tiling, 0f);
				Vector2 uv2 = new Vector2(num4 * Tiling, 1f);
				num4 += value.Mag / num5;
				Vector2 uv3 = new Vector2(num4 * Tiling, 0f);
				Vector2 uv4 = new Vector2(num4 * Tiling, 1f);
				UIVertex uIVertex = _ED15._E00F(p, uv, num2);
				UIVertex uIVertex2 = _ED15._E00F(p2, uv2, num2);
				UIVertex uIVertex3 = _ED15._E00F(p3, uv3, num2);
				UIVertex uIVertex4 = _ED15._E00F(p4, uv4, num2);
				yield return uIVertex;
				yield return uIVertex2;
				yield return uIVertex3;
				yield return uIVertex4;
				Vector3 p6;
				if (obj2.HasValue)
				{
					float z = num2 + 0.2f;
					_E003(value.To, value.Dir, value.Normal, num * 0.5f, num * 0.6f, uIVertex3.position.z, out var p5, out p6);
					yield return uIVertex3;
					yield return uIVertex4;
					yield return _ED15._E00F(p5, uIVertex3.uv0, z);
					yield return _ED15._E00F(p6, uIVertex4.uv0, z);
					p5 = default(Vector3);
					p6 = default(Vector3);
				}
				if (obj3.HasValue)
				{
					float z = num2 + 0.2f;
					_E003(value.From, -value.Dir, value.Normal, num * 0.5f, num * 0.6f, uIVertex.position.z, out var p7, out p6);
					yield return _ED15._E00F(p7, uIVertex.uv0, z);
					yield return _ED15._E00F(p6, uIVertex2.uv0, z);
					yield return uIVertex;
					yield return uIVertex2;
					p6 = default(Vector3);
				}
				num2 -= 0.05f;
				obj3 = value;
				num7 = num6 + 1;
			}
			num7 = num3 + 1;
			num3 = num7;
		}
	}

	private IEnumerable<UIVertex> _E008()
	{
		if (this.m__E002)
		{
			return _E004();
		}
		if (this.m__E001)
		{
			return _E006();
		}
		return _E007();
	}

	protected override void OnPopulateMesh(VertexHelper vh)
	{
		base.OnPopulateMesh(vh);
		vh.Clear();
		int num = 0;
		foreach (UIVertex item in _E008())
		{
			_E01D[num++] = item;
			if (num == 4)
			{
				UIVertex uIVertex = _E01D[2];
				_E01D[2] = _E01D[3];
				_E01D[3] = uIVertex;
				num = 0;
				vh.AddUIVertexQuad(_E01D);
			}
		}
	}

	protected override void OnPopulateMesh(Mesh m)
	{
		if (_E01A == null)
		{
			_E01A = new _ED21(1);
		}
		else
		{
			_E01A.Clear();
		}
		int num = 0;
		foreach (UIVertex item in _E008())
		{
			_E01D[num++] = item;
			if (num == 4)
			{
				num = 0;
				_E01A.AddQuad(_E01D[0], _E01D[1], _E01D[2], _E01D[3]);
			}
		}
		_E01A.ApplyToMesh(m);
	}

	private void _E009(Vector3 mouse, out int segment, out int line)
	{
		float num = float.PositiveInfinity;
		segment = -1;
		line = -1;
		if (this.m__E00C == null)
		{
			return;
		}
		for (int i = 0; i < this.m__E00C.Count; i++)
		{
			_E001 obj = this.m__E00C[i];
			int lineCount = obj.LineCount;
			for (int j = 0; j < lineCount; j++)
			{
				obj.GetLine(j, out var from, out var to);
				float num2 = _ED15._E015(from, to, mouse);
				if (num2 < num)
				{
					num = num2;
					segment = i;
					line = j;
				}
			}
		}
		float num3 = 10f;
		if (this.m__E007 != null)
		{
			num3 = this.m__E007.Sensitivity;
		}
		float num4 = Thickness + num3;
		if ((_E019.HasValue && !_E019.Value.Contains(mouse)) || num > num4 * num4)
		{
			segment = -1;
			line = -1;
		}
	}

	private void _E00A(Vector3 mouse, out int i, out int j)
	{
		if (this.m__E002)
		{
			_E00B(mouse, out i, out j);
		}
		else
		{
			_E009(mouse, out i, out j);
		}
		if (j >= 0)
		{
			j += refrenceIndex;
		}
	}

	private void _E00B(Vector3 mouse, out int segment, out int point)
	{
		float num = float.PositiveInfinity;
		segment = -1;
		point = -1;
		if (this.m__E00C == null)
		{
			return;
		}
		float num2 = this.m__E003;
		for (int i = 0; i < this.m__E00C.Count; i++)
		{
			_E001 obj = this.m__E00C[i];
			int pointCount = obj.PointCount;
			for (int j = 0; j < pointCount; j++)
			{
				Vector4 point2 = obj.getPoint(j);
				if (point2.w == 0f)
				{
					continue;
				}
				float sqrMagnitude = (mouse - (Vector3)point2).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num2 = point2.w;
					if (num2 < 0f)
					{
						num2 = this.m__E003;
					}
					num = sqrMagnitude;
					segment = i;
					point = j;
				}
			}
		}
		float num3 = 10f;
		if (this.m__E007 != null)
		{
			num3 = this.m__E007.Sensitivity;
		}
		float num4 = num2 + num3;
		if ((_E019.HasValue && !_E019.Value.Contains(mouse)) || num > num4 * num4)
		{
			segment = -1;
			point = -1;
		}
	}

	private void _E00C()
	{
		if (this.m__E00A == null)
		{
			return;
		}
		foreach (ChartItemEffect item in this.m__E00A)
		{
			_E00D(item);
		}
	}

	private void _E00D([CanBeNull] ChartItemEffect hover)
	{
		if (hover == null || this.m__E00C == null || this.m__E00C.Count == 0)
		{
			return;
		}
		int num = hover._E000 - refrenceIndex;
		if (num < 0)
		{
			return;
		}
		if (this.m__E002)
		{
			if (num < this.m__E00C[0].PointCount)
			{
				Vector4 point = this.m__E00C[0].getPoint(num);
				RectTransform component = hover.GetComponent<RectTransform>();
				component.localScale = new Vector3(1f, 1f, 1f);
				component.sizeDelta = new Vector2(5f, 5f);
				component.anchoredPosition3D = new Vector3(point.x, point.y, 0f);
			}
		}
		else if (num < this.m__E00C[0].LineCount)
		{
			this.m__E00C[0].GetLine(num, out var from, out var to);
			if (_E019.HasValue)
			{
				Vector2 from2 = from;
				Vector2 to2 = to;
				_E00F(_E019.Value, ref from2, ref to2);
				from = new Vector3(from2.x, from2.y, from.z);
				to = new Vector3(to2.x, to2.y, to.z);
			}
			Vector3 vector = to - from;
			float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			RectTransform component2 = hover.GetComponent<RectTransform>();
			component2.sizeDelta = new Vector2(vector.magnitude, Thickness);
			component2.localScale = new Vector3(1f, 1f, 1f);
			component2.localRotation = Quaternion.Euler(0f, 0f, z);
			Vector3 vector2 = (from + to) * 0.5f;
			component2.anchoredPosition3D = new Vector3(vector2.x, vector2.y, 0f);
		}
	}

	private void _E00E(float x1, float y1, float x2, float y2, bool xAxis, bool oposite, ref Vector2 from, ref Vector2 to)
	{
		Vector2 a = new Vector2(x1, y1);
		Vector2 a2 = new Vector2(x2, y2);
		if (!_ED15._E008(a, a2, from, to, out var intersection))
		{
			return;
		}
		if (xAxis)
		{
			if ((to.y > from.y) ^ oposite)
			{
				from = intersection;
			}
			else
			{
				to = intersection;
			}
		}
		else if ((to.x > from.x) ^ oposite)
		{
			from = intersection;
		}
		else
		{
			to = intersection;
		}
	}

	private void _E00F(Rect r, ref Vector2 from, ref Vector2 to)
	{
		_E00E(r.xMin, r.yMin, r.xMax, r.yMin, xAxis: true, oposite: false, ref from, ref to);
		_E00E(r.xMin, r.yMax, r.xMax, r.yMax, xAxis: true, oposite: true, ref from, ref to);
		_E00E(r.xMin, r.yMin, r.xMin, r.yMax, xAxis: false, oposite: false, ref from, ref to);
		_E00E(r.xMax, r.yMin, r.xMax, r.yMax, xAxis: false, oposite: true, ref from, ref to);
	}

	private void _E010(ChartItemEffect hover)
	{
		hover.TriggerOut(deactivateOnEnd: true);
		ChartMaterialController component = hover.GetComponent<ChartMaterialController>();
		if ((bool)component)
		{
			component.TriggerOff();
		}
	}

	private void _E011(ChartItemEffect hover)
	{
		hover.TriggerIn(deactivateOnEnd: false);
		ChartMaterialController component = hover.GetComponent<ChartMaterialController>();
		if ((bool)component)
		{
			component.TriggerOn();
		}
	}

	private void _E012(Vector3 mouse, bool leave)
	{
		if (this.m__E00C == null)
		{
			return;
		}
		int num = this.m__E012;
		int num2 = this.m__E013;
		if (leave)
		{
			this.m__E012 = -1;
			this.m__E013 = -1;
		}
		else
		{
			_E00A(mouse, out this.m__E012, out this.m__E013);
		}
		if (num == this.m__E012 && num2 == this.m__E013)
		{
			return;
		}
		if (this.m__E009 != null)
		{
			_E010(this.m__E009);
			if (this.m__E012 == -1 && this.m__E013 == -1 && _E018 != null)
			{
				_E018();
			}
			this.m__E009 = null;
		}
		if (this.m__E012 == -1 || this.m__E013 == -1)
		{
			return;
		}
		this.m__E009 = LockHoverObject(this.m__E013);
		if (!(this.m__E009 == null))
		{
			if (_E016 != null)
			{
				_E016(this.m__E013, mouse);
			}
			this.m__E009.gameObject.SetActive(value: true);
			_E00D(this.m__E009);
			_E011(this.m__E009);
		}
	}

	public void HandleMouseMove()
	{
		HandleMouseMove(force: false);
	}

	public void HandleMouseMove(bool force)
	{
		_E015 = GetComponentInParent<GraphicRaycaster>();
		if (_E015 == null)
		{
			return;
		}
		RectTransformUtility.ScreenPointToLocalPointInRectangle(base.transform as RectTransform, Input.mousePosition, _E015.eventCamera, out var localPoint);
		float num = 10f;
		if (this.m__E007 != null)
		{
			num = this.m__E007.Sensitivity;
		}
		float num2 = Mathf.Max(Thickness, this.m__E003) + num;
		if (Input.GetMouseButtonDown(0))
		{
			_E013();
		}
		if (force)
		{
			_E00C();
		}
		if (localPoint.x < this.m__E00E - num2 || localPoint.y < this.m__E00F - num2 || localPoint.x > this.m__E010 + num2 || localPoint.y > this.m__E011 + num2)
		{
			if (this.m__E00D)
			{
				this.m__E00D = false;
				_E012(localPoint, leave: true);
			}
		}
		else if (!this.m__E00D)
		{
			this.m__E00D = true;
			_E014 = localPoint;
			_E012(localPoint, leave: false);
		}
		else if ((_E014 - localPoint).sqrMagnitude > 1f || force)
		{
			_E014 = localPoint;
			_E012(localPoint, leave: false);
		}
	}

	private void _E013()
	{
		if (this.m__E012 != -1 && this.m__E013 != -1 && _E017 != null)
		{
			_E015 = GetComponentInParent<GraphicRaycaster>();
			if (!(_E015 == null))
			{
				RectTransformUtility.ScreenPointToLocalPointInRectangle(base.transform as RectTransform, Input.mousePosition, _E015.eventCamera, out var localPoint);
				Vector3 vector = base.transform.InverseTransformPoint(localPoint);
				_E017(this.m__E013, vector);
			}
		}
	}
}
