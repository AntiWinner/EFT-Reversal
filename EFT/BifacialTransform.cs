using System;
using UnityEngine;

namespace EFT;

[Serializable]
public class BifacialTransform
{
	[SerializeField]
	public Transform Original;

	private Func<Vector3> _positionImitator;

	private Action<Vector3> _positionSetImitator;

	private Func<Quaternion> _rotationImitator;

	private Action<Quaternion> _rotationSetImitator;

	private Action<Vector3, Quaternion> _positionAndRotationSetImitator;

	private Func<Vector3> _localPositionImitator;

	private Action<Vector3> _localPositionSetImitator;

	private Func<Quaternion> _localRotationImitator;

	private Action<Quaternion> _localRotationSetImitator;

	private Func<Vector3> _forwardImitator;

	private Func<Vector3> _upImitator;

	private Func<Vector3> _rightImitator;

	private Func<Vector3> _eulerAnglesImitator;

	private Func<Vector3, Vector3> _transformPointImitator;

	private Func<Vector3, Vector3> _inverseTransformVectorImitator;

	private Func<Vector3, Vector3> _transformVectorImitator;

	private Func<Vector3, Vector3> _inverseTransformPointImitator;

	private Action<Vector3, Vector3> _lookAtImitator;

	private bool _useImitation;

	private bool _accumulatePositionAndRotation;

	private Vector3 _accumulatedPosition;

	private Quaternion _accumulatedRotation;

	public bool UseImitation
	{
		get
		{
			return _useImitation;
		}
		set
		{
			_useImitation = value;
		}
	}

	public bool AccumulatePositionAndRotation
	{
		get
		{
			return _accumulatePositionAndRotation;
		}
		set
		{
			_accumulatePositionAndRotation = value;
			if (_accumulatePositionAndRotation)
			{
				_accumulatedPosition = position;
				_accumulatedRotation = rotation;
			}
		}
	}

	public Vector3 position
	{
		get
		{
			if (_accumulatePositionAndRotation)
			{
				return _accumulatedPosition;
			}
			if (_useImitation)
			{
				return _positionImitator();
			}
			return Original.position;
		}
		set
		{
			if (_accumulatePositionAndRotation)
			{
				_accumulatedPosition = value;
			}
			else if (_useImitation)
			{
				_positionSetImitator(value);
			}
			else
			{
				Original.position = value;
			}
		}
	}

	public Quaternion rotation
	{
		get
		{
			if (_accumulatePositionAndRotation)
			{
				return _accumulatedRotation;
			}
			if (_useImitation)
			{
				return _rotationImitator();
			}
			return Original.rotation;
		}
		set
		{
			if (_accumulatePositionAndRotation)
			{
				_accumulatedRotation = value;
			}
			else if (_useImitation)
			{
				_rotationSetImitator(value);
			}
			else
			{
				Original.rotation = value;
			}
		}
	}

	public Vector3 localPosition
	{
		get
		{
			if (_useImitation)
			{
				return _localPositionImitator();
			}
			return Original.localPosition;
		}
		set
		{
			if (_useImitation)
			{
				_localPositionSetImitator(value);
			}
			else
			{
				Original.localPosition = value;
			}
		}
	}

	public Quaternion localRotation
	{
		get
		{
			if (_useImitation)
			{
				return _localRotationImitator();
			}
			return Original.localRotation;
		}
		set
		{
			if (_useImitation)
			{
				_localRotationSetImitator(value);
			}
			else
			{
				Original.localRotation = value;
			}
		}
	}

	public Vector3 forward
	{
		get
		{
			if (_useImitation)
			{
				return _forwardImitator();
			}
			return Original.forward;
		}
	}

	public Vector3 up
	{
		get
		{
			if (_useImitation)
			{
				return _upImitator();
			}
			return Original.up;
		}
	}

	public Vector3 right
	{
		get
		{
			if (_useImitation)
			{
				return _rightImitator();
			}
			return Original.right;
		}
	}

	public Vector3 eulerAngles
	{
		get
		{
			if (_accumulatePositionAndRotation)
			{
				return _accumulatedRotation.eulerAngles;
			}
			if (_useImitation)
			{
				return _eulerAnglesImitator();
			}
			return Original.eulerAngles;
		}
	}

	public void SetImitators(Func<Vector3> positionImitator, Action<Vector3> positionSetImitator, Func<Quaternion> rotationImitator, Action<Quaternion> rotationSetImitator, Action<Vector3, Quaternion> positionAndRotationSetImitator, Func<Vector3> localPositionImitator, Action<Vector3> localPositionSetImitator, Func<Quaternion> localRotationImitator, Action<Quaternion> localRotationSetImitator, Func<Vector3> forwardImitator, Func<Vector3> upImitator, Func<Vector3> rightImitator, Func<Vector3> eulerAnglesImitator, Func<Vector3, Vector3> transformPointImitator, Func<Vector3, Vector3> inverseTransformPointImitator, Func<Vector3, Vector3> transformVectorImitator, Func<Vector3, Vector3> inverseTransformVectorImitator, Action<Vector3, Vector3> lookAtImitator)
	{
		_positionImitator = positionImitator;
		_positionSetImitator = positionSetImitator;
		_rotationImitator = rotationImitator;
		_rotationSetImitator = rotationSetImitator;
		_positionAndRotationSetImitator = positionAndRotationSetImitator;
		_localPositionImitator = localPositionImitator;
		_localPositionSetImitator = localPositionSetImitator;
		_localRotationImitator = localRotationImitator;
		_localRotationSetImitator = localRotationSetImitator;
		_forwardImitator = forwardImitator;
		_upImitator = upImitator;
		_rightImitator = rightImitator;
		_eulerAnglesImitator = eulerAnglesImitator;
		_transformPointImitator = transformPointImitator;
		_inverseTransformPointImitator = inverseTransformPointImitator;
		_transformVectorImitator = transformVectorImitator;
		_inverseTransformVectorImitator = inverseTransformVectorImitator;
		_lookAtImitator = lookAtImitator;
	}

	public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
	{
		if (_accumulatePositionAndRotation)
		{
			_accumulatedPosition = position;
			_accumulatedRotation = rotation;
		}
		else
		{
			_E000(position, rotation);
		}
	}

	private void _E000(Vector3 position, Quaternion rotation)
	{
		if (_useImitation)
		{
			_positionAndRotationSetImitator(position, rotation);
		}
		else
		{
			Original.SetPositionAndRotation(position, rotation);
		}
	}

	public void ApplyAccumulatedPositionAndRotation()
	{
		_E000(_accumulatedPosition, _accumulatedRotation);
	}

	public Vector3 TransformPoint(Vector3 position)
	{
		if (_useImitation)
		{
			return _transformPointImitator(position);
		}
		return Original.TransformPoint(position);
	}

	public Vector3 InverseTransformPoint(Vector3 point)
	{
		if (_useImitation)
		{
			return _inverseTransformPointImitator(point);
		}
		return Original.InverseTransformPoint(point);
	}

	public Vector3 InverseTransformVector(Vector3 vector)
	{
		if (_useImitation)
		{
			return _inverseTransformVectorImitator(vector);
		}
		return Original.InverseTransformVector(vector);
	}

	public Vector3 TransformVector(Vector3 vector)
	{
		if (_useImitation)
		{
			return _transformVectorImitator(vector);
		}
		return Original.TransformVector(vector);
	}

	public void LookAt(Vector3 worldPosition, Vector3 worldUp)
	{
		if (_useImitation)
		{
			_lookAtImitator(worldPosition, worldUp);
		}
		else
		{
			Original.LookAt(worldPosition, worldUp);
		}
	}

	public void CopyImitators(BifacialTransform donor)
	{
		_positionImitator = donor._positionImitator;
		_positionSetImitator = donor._positionSetImitator;
		_rotationImitator = donor._rotationImitator;
		_rotationSetImitator = donor._rotationSetImitator;
		_localPositionImitator = donor._localPositionImitator;
		_localPositionSetImitator = donor._localPositionSetImitator;
		_localRotationImitator = donor._localRotationImitator;
		_localRotationSetImitator = donor._localRotationSetImitator;
		_forwardImitator = donor._forwardImitator;
		_upImitator = donor._upImitator;
		_rightImitator = donor._rightImitator;
		_eulerAnglesImitator = donor._eulerAnglesImitator;
		_transformPointImitator = donor._transformPointImitator;
		_inverseTransformVectorImitator = donor._inverseTransformVectorImitator;
		_transformVectorImitator = donor._transformVectorImitator;
		_inverseTransformPointImitator = donor._inverseTransformPointImitator;
		_lookAtImitator = donor._lookAtImitator;
	}

	public void SyncOriginal()
	{
		Original.localPosition = _positionImitator();
		Original.localRotation = _rotationImitator();
	}

	public void SyncImitators()
	{
		_positionSetImitator(Original.localPosition);
		_rotationSetImitator(Original.localRotation);
	}
}
