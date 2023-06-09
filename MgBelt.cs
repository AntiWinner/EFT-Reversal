using System;
using System.Collections.Generic;
using BezierSplineTools;
using UnityEngine;

public class MgBelt : MonoBehaviour
{
	public class _E000
	{
		public float Shift;

		public float Decay;

		public const float Cap = 3f;
	}

	[SerializeField]
	private BezierCurve Curve1;

	[SerializeField]
	private float _sleepCooldown = 4f;

	public Rigidbody[] KeyElemetns;

	public Collider[] KeyElementsColliders;

	public List<Transform> Links = new List<Transform>();

	public float WaveAmplitude;

	public AnimationCurve AmplitudeCurve;

	public AnimationCurve WeightCurve;

	private readonly List<_E000> m__E000 = new List<_E000>();

	private float _E001 = float.PositiveInfinity;

	private bool _E002;

	private bool _E003 = true;

	private Transform _E004;

	private int _E005;

	private bool _E006;

	private bool _E007;

	public bool On
	{
		get
		{
			return _E003;
		}
		set
		{
			_E003 = value;
			if (value)
			{
				_E001 = float.PositiveInfinity;
				_E002 = false;
				_E000(isPhysicsEnabled: true);
			}
			else
			{
				_E001 = Time.time + _sleepCooldown;
				_E002 = true;
			}
		}
	}

	private void _E000(bool isPhysicsEnabled)
	{
		if (isPhysicsEnabled && _E006 && On)
		{
			if (!_E007)
			{
				Rigidbody[] keyElemetns = KeyElemetns;
				for (int i = 0; i < keyElemetns.Length; i++)
				{
					_E320._E002.SupportRigidbody(keyElemetns[i], 0f);
				}
				_E007 = true;
			}
		}
		else if (_E007)
		{
			Rigidbody[] keyElemetns = KeyElemetns;
			for (int i = 0; i < keyElemetns.Length; i++)
			{
				_E320._E002.UnsupportRigidbody(keyElemetns[i]);
			}
			_E007 = false;
		}
	}

	public void SetVisible(bool isVisible)
	{
		if (_E006 != isVisible)
		{
			_E006 = isVisible;
			_E000(_E006);
		}
	}

	public void SetPivotPoint(Transform t)
	{
		_E004 = t;
	}

	public void Init()
	{
		Curve1.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
		_E000(isPhysicsEnabled: true);
		On = false;
		int num = KeyElemetns.Length;
		KeyElementsColliders = new Collider[num];
		for (int i = 0; i < num; i++)
		{
			KeyElementsColliders[i] = KeyElemetns[i].GetComponent<Collider>();
		}
		Curve1.points = new Vector3[num];
	}

	private void LateUpdate()
	{
		if (!_E002 && !On)
		{
			return;
		}
		if ((bool)_E004)
		{
			KeyElemetns[0].transform.SetPositionAndRotation(_E004.position, _E004.rotation);
		}
		if (Curve1 != null)
		{
			for (int i = 0; i < KeyElemetns.Length; i++)
			{
				Curve1.points[i] = KeyElemetns[i].position;
			}
		}
		for (int j = 0; j < Links.Count && Links[j].gameObject.activeInHierarchy; j++)
		{
			int num = (int)((float)Links.Count / 3f);
			int num2 = j / num;
			int num3 = num2 + 1;
			Rigidbody rigidbody = KeyElemetns[num2];
			Rigidbody rigidbody2 = KeyElemetns[num3];
			float num4 = (float)j / (float)Links.Count;
			Vector3 point = Curve1.GetPoint(num4);
			float t = (float)(j % num) / (float)num;
			if (this.m__E000.Count > 0)
			{
				Vector3 normalized = Vector3.Lerp(rigidbody.transform.forward, rigidbody2.transform.forward, t).normalized;
				float num5 = float.MinValue;
				foreach (_E000 item in this.m__E000)
				{
					num5 = Math.Max(num5, AmplitudeCurve.Evaluate(num4 - item.Shift) * WeightCurve.Evaluate(num4 * item.Decay));
				}
				point += num5 * WaveAmplitude * normalized;
			}
			Links[j].SetPositionAndRotation(point, Quaternion.Lerp(rigidbody.rotation, rigidbody2.rotation, t));
		}
		for (int num6 = this.m__E000.Count - 1; num6 >= 0; num6--)
		{
			_E000 obj = this.m__E000[num6];
			if (obj.Shift > AmplitudeCurve.GetDuration())
			{
				this.m__E000.RemoveAt(num6);
			}
			else
			{
				obj.Shift += Time.deltaTime;
			}
		}
		if (_E002 && Time.time > _E001)
		{
			_E002 = false;
			_E000(isPhysicsEnabled: false);
		}
	}

	public void AddWave()
	{
		this.m__E000.Add(new _E000
		{
			Shift = 0f,
			Decay = UnityEngine.Random.Range(1f, 2f)
		});
	}

	public void DisplayAll(int displayElements)
	{
		for (int i = 0; i < Links.Count; i++)
		{
			Links[i].gameObject.SetActive(i < displayElements);
		}
	}

	public void DisplayDelta(int displayElements)
	{
		for (int i = _E005; i < Links.Count && i < displayElements; i++)
		{
			Links[i].gameObject.SetActive(value: true);
			_E005 = i;
		}
		int num = Links.Count / (KeyElementsColliders.Length - 1);
		for (int j = 1; j < KeyElementsColliders.Length && displayElements > num * j; j++)
		{
			KeyElementsColliders[j].isTrigger = false;
		}
	}
}
