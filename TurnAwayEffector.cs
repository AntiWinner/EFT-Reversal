using System;
using EFT;
using EFT.Animations;
using UnityEngine;

[Serializable]
public class TurnAwayEffector : IEffector
{
	[Serializable]
	public class AnimVal
	{
		public enum ComponentType
		{
			X,
			Y,
			Z
		}

		public float Intensity;

		public float AltIntensity;

		public float Intensity1;

		public float AltIntensity1;

		public AnimationCurve Curve;

		public ComponentType Component;

		private int _component;

		public void Initialize()
		{
			_component = (int)Component;
		}

		public virtual void Process(ref Vector3 vec, float value, EPointOfView pov, float weight)
		{
			if (pov == EPointOfView.FirstPerson)
			{
				vec[_component] = Curve.Evaluate(value) * Mathf.Lerp(Intensity, Intensity1, weight);
			}
			else
			{
				vec[_component] = Curve.Evaluate(value) * Mathf.Lerp(AltIntensity, AltIntensity1, weight);
			}
		}
	}

	public float OverlapValue;

	[SerializeField]
	private TurnAwayPose _rifleShift;

	[SerializeField]
	private TurnAwayPose _pistolShift;

	[SerializeField]
	private TurnAwayPose _proneShift;

	[SerializeField]
	private TurnAwayPose[] _elbows;

	private TurnAwayPose _currentShift;

	[SerializeField]
	private float _blendSpeed = 10f;

	[SerializeField]
	private float _turnAwayThreshold = 0.5f;

	[SerializeField]
	private float _playerOverlapThreshold = 0.6f;

	[SerializeField]
	private float _smoothTimeIn = 14f;

	[SerializeField]
	private float _smoothTimeOut = 8f;

	private EPointOfView _pov = EPointOfView.ThirdPerson;

	private bool _isPistol;

	private bool _isInProne;

	public bool IsDirty = true;

	private float _fovInput = 1f;

	public bool OverlapsWithPlayer;

	public float OverlapDepth;

	public float OriginZShift;

	private float _fovScale;

	private float _normalFovScale;

	public float Overlap;

	private float _blend;

	public AnimationCurve CameraShiftCurve;

	private Vector3 _position;

	private Vector3 _rotation;

	private Vector3 _cameraShift;

	private Vector3 _lElbowShift;

	private Vector3 _rElbowShift;

	private float _cameraZShift;

	private float _maxZShift = 0.4f;

	[SerializeField]
	private float _cameraShiftAtMaxOverlap = 0.02f;

	public bool IsInPronePose
	{
		set
		{
			_isInProne = value;
			UpdatePreset();
		}
	}

	public bool IsPistol
	{
		set
		{
			_isPistol = value;
			UpdatePreset();
		}
	}

	public Vector3 Position => _position;

	public Vector3 Rotation => _rotation;

	public float FovScale
	{
		set
		{
			_fovInput = value;
			_fovScale = ((_pov == EPointOfView.ThirdPerson) ? 1f : _fovInput);
			_normalFovScale = Mathf.InverseLerp(0.65f, 1f, _fovScale);
		}
	}

	public EPointOfView PointOfView
	{
		set
		{
			_pov = value;
			FovScale = _fovInput;
			UpdatePreset();
		}
	}

	public Vector3 CameraShift => _cameraShift;

	public Vector3 LElbowShift => _lElbowShift;

	public Vector3 RElbowShift => _rElbowShift;

	public void Initialize(PlayerSpring playerSpring)
	{
		TurnAwayPose[] array = new TurnAwayPose[3] { _rifleShift, _pistolShift, _proneShift };
		foreach (TurnAwayPose turnAwayPose in array)
		{
			AnimVal[] pos = turnAwayPose.Pos;
			for (int j = 0; j < pos.Length; j++)
			{
				pos[j].Initialize();
			}
			pos = turnAwayPose.Rot;
			for (int j = 0; j < pos.Length; j++)
			{
				pos[j].Initialize();
			}
		}
		array = _elbows;
		for (int i = 0; i < array.Length; i++)
		{
			AnimVal[] pos = array[i].Pos;
			for (int j = 0; j < pos.Length; j++)
			{
				pos[j].Initialize();
			}
		}
		UpdatePreset();
	}

	public void UpdatePreset()
	{
		_currentShift = (_isInProne ? _proneShift : (_isPistol ? _pistolShift : _rifleShift));
		_maxZShift = ((_pov == EPointOfView.FirstPerson) ? _currentShift.Pos[1].Intensity : _currentShift.Pos[1].AltIntensity);
	}

	public void Process(float deltaTime)
	{
		_position = (_rotation = (_lElbowShift = (_rElbowShift = (_cameraShift = Vector3.zero))));
		float num = (OverlapsWithPlayer ? (OverlapDepth / _playerOverlapThreshold) : (OverlapDepth / _turnAwayThreshold));
		OverlapValue = Mathf.Lerp(OverlapValue, num, (num > OverlapValue) ? (_smoothTimeIn * deltaTime) : (_smoothTimeOut * deltaTime));
		if (!(OverlapValue <= 0.001f))
		{
			float b = ((OverlapValue > 1f) ? 1 : 0);
			_blend = Mathf.Lerp(_blend, b, _blendSpeed * deltaTime);
			AnimVal[] pos = _currentShift.Pos;
			for (int i = 0; i < pos.Length; i++)
			{
				pos[i].Process(ref _position, OverlapValue, _pov, _blend);
			}
			_position.y = Mathf.Clamp(_position.y * _fovScale, 0f, _maxZShift - OriginZShift);
			pos = _currentShift.Rot;
			for (int i = 0; i < pos.Length; i++)
			{
				pos[i].Process(ref _rotation, OverlapValue, _pov, _blend);
			}
			pos = _elbows[0].Pos;
			for (int i = 0; i < pos.Length; i++)
			{
				pos[i].Process(ref _lElbowShift, OverlapValue, _pov, _blend);
			}
			pos = _elbows[1].Pos;
			for (int i = 0; i < pos.Length; i++)
			{
				pos[i].Process(ref _rElbowShift, OverlapValue, _pov, _blend);
			}
			if (_pov == EPointOfView.FirstPerson)
			{
				_cameraZShift = Mathf.Lerp(_cameraZShift, (OverlapValue > 1f) ? _cameraShiftAtMaxOverlap : (CameraShiftCurve.Evaluate(OverlapValue) * _normalFovScale), deltaTime * _blendSpeed);
				_cameraShift = new Vector3(0f, 0f, _cameraZShift);
			}
		}
	}

	public string DebugOutput()
	{
		throw new NotImplementedException();
	}
}
