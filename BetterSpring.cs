using System;
using UnityEngine;

[Serializable]
public class BetterSpring
{
	private Vector3 _pPos;

	private Vector3 _pVel;

	[SerializeField]
	private Vector3 _equilibriumPos = Vector3.zero;

	[SerializeField]
	public float _angularFrequency;

	[SerializeField]
	public float _dampingRatio = 0.5f;

	private const float Epsilon = 0.0001f;

	private float _za;

	private float _zb;

	private float _z1;

	private float _z2;

	private float _omegaZeta;

	private float _alpha;

	public Vector3 Value => _pPos;

	public Vector3 EquilibriumPos
	{
		get
		{
			return _equilibriumPos;
		}
		set
		{
			_equilibriumPos = value;
		}
	}

	[ContextMenu("Runtime update")]
	public void Cache()
	{
		_omegaZeta = _angularFrequency * _dampingRatio;
		_alpha = _angularFrequency * Mathf.Sqrt(1f - _dampingRatio * _dampingRatio);
		_za = (0f - _angularFrequency) * _dampingRatio;
		_zb = _angularFrequency * Mathf.Sqrt(_dampingRatio * _dampingRatio - 1f);
		_z1 = _za - _zb;
		_z2 = _za + _zb;
	}

	public void Process(float deltaTime)
	{
		Vector3 vector = _pPos - EquilibriumPos;
		Vector3 pVel = _pVel;
		if (_dampingRatio > 1.0001f)
		{
			float num = Mathf.Exp(_z1 * deltaTime);
			float num2 = Mathf.Exp(_z2 * deltaTime);
			Vector3 vector2 = (pVel - vector * _z2) / (-2f * _zb);
			Vector3 vector3 = vector - vector2;
			_pPos = EquilibriumPos + vector2 * num + vector3 * num2;
			_pVel = vector2 * (_z1 * num) + vector3 * (_z2 * num2);
		}
		else if (_dampingRatio > 0.9999f)
		{
			float num3 = Mathf.Exp((0f - _angularFrequency) * deltaTime);
			Vector3 vector4 = pVel + _angularFrequency * vector;
			Vector3 vector5 = vector;
			Vector3 vector6 = (vector4 * deltaTime + vector5) * num3;
			_pPos = EquilibriumPos + vector6;
			_pVel = vector4 * num3 - vector6 * _angularFrequency;
		}
		else
		{
			float num4 = Mathf.Exp((0f - _omegaZeta) * deltaTime);
			float num5 = Mathf.Cos(_alpha * deltaTime);
			float num6 = Mathf.Sin(_alpha * deltaTime);
			Vector3 vector7 = vector;
			Vector3 vector8 = (pVel + _omegaZeta * vector) / _alpha;
			_pPos = EquilibriumPos + num4 * (vector7 * num5 + vector8 * num6);
			_pVel = (0f - num4) * ((vector7 * _omegaZeta - vector8 * _alpha) * num5 + (vector7 * _alpha + vector8 * _omegaZeta) * num6);
		}
	}

	public void ApplyVelocity(Vector3 val)
	{
		_pVel += val;
	}

	public void ApplyVelocity(int comp, float val)
	{
		_pVel[comp] += val;
	}

	public void ApplyPosition(Vector3 position)
	{
		_pPos = position;
	}
}
