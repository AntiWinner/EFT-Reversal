using System;
using System.Collections.Generic;
using Comfort.Common;
using EFT;
using JsonType;
using UnityEngine;

public class TracersLight : MonoBehaviour
{
	private class _E000
	{
		public readonly Color BackCurrentTracerColor;

		public readonly Color CurrentTracerColor;

		public Vector3 Direction;

		public readonly Vector3 PositionShift;

		public readonly float SizePercent;

		public readonly float Speed;

		public _E000(Vector3 positionShift, Vector3 direction, Color currentTracerColor, Color backCurrentTracerColor, float sizePercent, float speed)
		{
			PositionShift = positionShift;
			Direction = direction;
			CurrentTracerColor = currentTracerColor;
			BackCurrentTracerColor = backCurrentTracerColor;
			SizePercent = sizePercent;
			Speed = speed;
		}
	}

	[ColorUsage(true, true, 0f, 8f, 0.125f, 3f)]
	public Color Color;

	[SerializeField]
	private float _maxFlyingTime = 7f;

	[SerializeField]
	private AnimationCurve _sizeTimeCurve;

	[SerializeField]
	private AnimationCurve _colorAlphaCurve;

	[SerializeField]
	private float _grenadeFlyingMaxTime = 0.25f;

	[SerializeField]
	private float _grenadeSizeCoef = 0.15f;

	[SerializeField]
	private MeshFilter _tracersMF;

	[SerializeField]
	private float _speedModifier = 0.005f;

	[SerializeField]
	private float _grenadeSpeedModifier = 0.02f;

	private List<_EC1E> m__E000;

	private List<_E000> m__E001;

	private bool m__E002;

	private bool m__E003;

	private static readonly float m__E004 = 100f;

	private void Awake()
	{
		this.m__E001 = new List<_E000>();
	}

	private void Update()
	{
		try
		{
			_E000();
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			Debug.LogError(_ED3E._E000(81983));
			base.enabled = false;
		}
	}

	private void _E000()
	{
		if (this.m__E001 == null)
		{
			this.m__E001 = new List<_E000>();
		}
		if (this.m__E001.Count > 0)
		{
			this.m__E001.Clear();
		}
		if (Singleton<GameWorld>.Instantiated)
		{
			if (Singleton<GameWorld>.Instance.SharedBallisticsCalculator != null)
			{
				_E001(Singleton<GameWorld>.Instance.SharedBallisticsCalculator);
			}
			if (Singleton<GameWorld>.Instance.ClientBallisticCalculator != null)
			{
				_E001(Singleton<GameWorld>.Instance.ClientBallisticCalculator);
			}
			if (this.m__E001.Count > 0)
			{
				_E005();
				this.m__E002 = true;
			}
			else if (this.m__E002)
			{
				_E006();
				this.m__E002 = false;
			}
		}
	}

	private void _E001(_EC1E ballisticsCalculator)
	{
		for (int i = 0; i < ballisticsCalculator.ActiveShotsCount; i++)
		{
			_EC26 activeShot = ballisticsCalculator.GetActiveShot(i);
			if (activeShot.GetTimeSinceShot < _maxFlyingTime && !activeShot.ShotProcessed)
			{
				_E002(activeShot);
			}
		}
	}

	private void _E002(_EC26 shot)
	{
		_EA12 obj = (_EA12)shot.Ammo;
		if (!obj.Tracer)
		{
			return;
		}
		List<Vector3> positionHistory = shot.PositionHistory;
		if (positionHistory.Count < 2)
		{
			return;
		}
		Vector3 vector = positionHistory[positionHistory.Count - 2];
		Vector3 vector2 = positionHistory[positionHistory.Count - 1];
		float sqrMagnitude = (vector2 - vector).sqrMagnitude;
		if (!(sqrMagnitude < 0.001f) && (positionHistory.Count != 2 || !(sqrMagnitude < TracersLight.m__E004)))
		{
			Color = obj.TracerColor.ToColor();
			if (obj.TracerColor == TaxonomyColor.tracerYellow)
			{
				_E004(shot, vector, vector2);
			}
			else
			{
				_E003(shot, vector, vector2);
			}
		}
	}

	private void _E003(_EC26 shot, Vector3 p0, Vector3 p1)
	{
		Vector3 normalized = (p1 - p0).normalized;
		float time = shot.GetTimeSinceShot / _maxFlyingTime;
		float sizePercent = _sizeTimeCurve.Evaluate(time);
		Color color = Color;
		float num = _colorAlphaCurve.Evaluate(time);
		color.a *= num;
		Color backCurrentTracerColor = color;
		float speed = shot.VelocityMagnitude * _speedModifier;
		this.m__E001.Add(new _E000(p1, normalized, color, backCurrentTracerColor, sizePercent, speed));
	}

	private void _E004(_EC26 shot, Vector3 p0, Vector3 p1)
	{
		if (!(shot.GetTimeSinceShot > _grenadeFlyingMaxTime))
		{
			Vector3 normalized = (p1 - p0).normalized;
			float num = shot.GetTimeSinceShot / _grenadeFlyingMaxTime;
			float num2 = _sizeTimeCurve.Evaluate(num);
			Color color = Color;
			float num3 = 1f - num;
			color.a *= num3;
			float speed = shot.VelocityMagnitude * _grenadeSpeedModifier;
			num2 *= _grenadeSizeCoef;
			this.m__E001.Add(new _E000(p1, normalized, color, Color.clear, num2, speed));
		}
	}

	private void _E005()
	{
		Vector3[] array = new Vector3[12 * this.m__E001.Count];
		int[] array2 = new int[18 * this.m__E001.Count];
		Vector2[] array3 = new Vector2[12 * this.m__E001.Count];
		Color[] array4 = new Color[array.Length];
		for (int i = 0; i < this.m__E001.Count; i++)
		{
			float speed = this.m__E001[i].Speed;
			float num = 0.5f * this.m__E001[i].SizePercent;
			array[12 * i] = new Vector3(0f - num, 0f - num, -0.03f * speed);
			array[1 + 12 * i] = new Vector3(num, 0f - num, -0.03f * speed);
			array[2 + 12 * i] = new Vector3(num, num, -0.03f * speed);
			array[3 + 12 * i] = new Vector3(0f - num, num, -0.03f * speed);
			array[4 + 12 * i] = new Vector3(0f, 0f - num, 0f);
			array[5 + 12 * i] = new Vector3(0f, 0f - num, 0f - speed);
			array[6 + 12 * i] = new Vector3(0f, num, 0f - speed);
			array[7 + 12 * i] = new Vector3(0f, num, 0f);
			array[8 + 12 * i] = new Vector3(0f - num, 0f, 0f);
			array[9 + 12 * i] = new Vector3(0f - num, 0f, 0f - speed);
			array[10 + 12 * i] = new Vector3(num, 0f, 0f - speed);
			array[11 + 12 * i] = new Vector3(num, 0f, 0f);
			Vector3 tangent = Vector3.zero;
			Vector3 binormal = Vector3.zero;
			Vector3.OrthoNormalize(ref this.m__E001[i].Direction, ref tangent, ref binormal);
			Matrix4x4 matrix4x = Matrix4x4.LookAt(Vector3.zero, this.m__E001[i].Direction, tangent);
			for (int j = 0; j < 12; j++)
			{
				array[i * 12 + j] = matrix4x.MultiplyPoint3x4(array[i * 12 + j]);
				array[i * 12 + j] += this.m__E001[i].PositionShift;
			}
			array2[18 * i] = 12 * i;
			array2[1 + 18 * i] = 1 + 12 * i;
			array2[2 + 18 * i] = 2 + 12 * i;
			array2[3 + 18 * i] = 12 * i;
			array2[4 + 18 * i] = 2 + 12 * i;
			array2[5 + 18 * i] = 3 + 12 * i;
			array2[6 + 18 * i] = 4 + 12 * i;
			array2[7 + 18 * i] = 5 + 12 * i;
			array2[8 + 18 * i] = 6 + 12 * i;
			array2[9 + 18 * i] = 4 + 12 * i;
			array2[10 + 18 * i] = 6 + 12 * i;
			array2[11 + 18 * i] = 7 + 12 * i;
			array2[12 + 18 * i] = 8 + 12 * i;
			array2[13 + 18 * i] = 9 + 12 * i;
			array2[14 + 18 * i] = 10 + 12 * i;
			array2[15 + 18 * i] = 8 + 12 * i;
			array2[16 + 18 * i] = 10 + 12 * i;
			array2[17 + 18 * i] = 11 + 12 * i;
			array3[12 * i] = new Vector2(0.5f, 0.5f);
			array3[1 + 12 * i] = new Vector2(1f, 0.5f);
			array3[2 + 12 * i] = new Vector2(1f, 1f);
			array3[3 + 12 * i] = new Vector2(0.5f, 1f);
			array3[4 + 12 * i] = new Vector2(0f, 0f);
			array3[5 + 12 * i] = new Vector2(1f, 0f);
			array3[6 + 12 * i] = new Vector2(1f, 0.5f);
			array3[7 + 12 * i] = new Vector2(0f, 0.5f);
			array3[8 + 12 * i] = new Vector2(0f, 0f);
			array3[9 + 12 * i] = new Vector2(1f, 0f);
			array3[10 + 12 * i] = new Vector2(1f, 0.5f);
			array3[11 + 12 * i] = new Vector2(0f, 0.5f);
			for (int k = 0; k < 12; k++)
			{
				array4[k + 12 * i] = ((k < 4) ? this.m__E001[i].BackCurrentTracerColor : this.m__E001[i].CurrentTracerColor);
			}
		}
		Mesh mesh = _tracersMF.mesh;
		mesh.Clear();
		mesh.vertices = array;
		mesh.triangles = array2;
		mesh.uv = array3;
		mesh.colors = array4;
		mesh.RecalculateNormals();
	}

	private void _E006()
	{
		_tracersMF.mesh.Clear();
	}
}
