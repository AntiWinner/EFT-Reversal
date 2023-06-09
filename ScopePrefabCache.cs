using System;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.CameraControl;
using JetBrains.Annotations;
using UnityEngine;

public class ScopePrefabCache : MonoBehaviour
{
	[Serializable]
	public class ScopeModeInfo
	{
		public GameObject ModeGameObject;

		public CollimatorSight CollimatorSight;

		public OpticSight OpticSight;

		public bool IgnoreOpticsForCameraPlane;
	}

	[Serializable]
	public struct DistaneAngle
	{
		public float Distance;

		public float Angle;
	}

	[SerializeField]
	public bool CanChangeAngleByDistance;

	[SerializeField]
	public Transform WeaponScopeAxis;

	[SerializeField]
	public DistaneAngle[] AngleByRange;

	private const string m__E000 = "mode_";

	[SerializeField]
	private ScopeModeInfo[] _scopeModeInfos = new ScopeModeInfo[0];

	private int _E001;

	[CompilerGenerated]
	private bool _E002;

	[CompilerGenerated]
	private bool _E003;

	public bool HasOptics
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

	public bool CurrentModHasOptics => _scopeModeInfos[CurrentModeId].OpticSight != null;

	public bool CurrentModIgnoreOpticsForCameraPlane => _scopeModeInfos[CurrentModeId].IgnoreOpticsForCameraPlane;

	public OpticSight CurrentModOpticSight => _scopeModeInfos[CurrentModeId].OpticSight;

	public bool HasCollimators
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

	public int ModesCount => _scopeModeInfos.Length;

	public OpticSight FirstOptic
	{
		get
		{
			ScopeModeInfo[] scopeModeInfos = _scopeModeInfos;
			foreach (ScopeModeInfo scopeModeInfo in scopeModeInfos)
			{
				if (scopeModeInfo.OpticSight != null)
				{
					return scopeModeInfo.OpticSight;
				}
			}
			throw new Exception(_ED3E._E000(45479));
		}
	}

	public CollimatorSight FirstCollimator
	{
		get
		{
			ScopeModeInfo[] scopeModeInfos = _scopeModeInfos;
			foreach (ScopeModeInfo scopeModeInfo in scopeModeInfos)
			{
				if (scopeModeInfo.CollimatorSight != null)
				{
					return scopeModeInfo.CollimatorSight;
				}
			}
			throw new Exception(_ED3E._E000(45566));
		}
	}

	public int CurrentModeId => _E001;

	[CanBeNull]
	public OpticSight GetOpticSight(int index)
	{
		return GetScopeModeInfo(index).OpticSight;
	}

	public ScopeModeInfo GetScopeModeInfo(int index)
	{
		return _scopeModeInfos[index];
	}

	private void Awake()
	{
		HasOptics = _scopeModeInfos.Any((ScopeModeInfo sm) => sm.OpticSight != null);
		HasCollimators = _scopeModeInfos.Any((ScopeModeInfo sm) => sm.CollimatorSight != null);
	}

	public void RotateToAngleByDistance(float distance)
	{
		if (CanChangeAngleByDistance && WeaponScopeAxis != null && _E000(distance, out var angle))
		{
			Quaternion localRotation = WeaponScopeAxis.localRotation;
			WeaponScopeAxis.transform.localRotation = Quaternion.Euler(angle, localRotation.y, localRotation.z);
		}
	}

	private bool _E000(float distance, out float angle)
	{
		angle = 0f;
		if (AngleByRange == null)
		{
			return false;
		}
		DistaneAngle[] angleByRange = AngleByRange;
		for (int i = 0; i < angleByRange.Length; i++)
		{
			DistaneAngle distaneAngle = angleByRange[i];
			if (distaneAngle.Distance == distance)
			{
				angle = distaneAngle.Angle;
				return true;
			}
		}
		return false;
	}

	public bool IsOpticBone(Transform bone)
	{
		ScopeModeInfo[] scopeModeInfos = _scopeModeInfos;
		foreach (ScopeModeInfo scopeModeInfo in scopeModeInfos)
		{
			if (scopeModeInfo.OpticSight != null && scopeModeInfo.OpticSight.ScopeTransform == bone)
			{
				return true;
			}
		}
		return false;
	}

	public void SetMode(int modeId)
	{
		if (_scopeModeInfos.Length < 2)
		{
			return;
		}
		if (modeId < 0 || modeId >= _scopeModeInfos.Length)
		{
			Debug.LogErrorFormat(this, _ED3E._E000(45586), modeId, this);
		}
		else if (CurrentModeId != modeId)
		{
			if (CurrentModeId >= 0)
			{
				_scopeModeInfos[CurrentModeId].ModeGameObject.SetActive(value: false);
			}
			_E001 = modeId;
			_scopeModeInfos[CurrentModeId].ModeGameObject.SetActive(value: true);
		}
	}

	public void LookAt(Vector3 point, Vector3 worldUp)
	{
		for (int i = 0; i < _scopeModeInfos.Length; i++)
		{
			ScopeModeInfo scopeModeInfo = _scopeModeInfos[i];
			if (scopeModeInfo.CollimatorSight != null)
			{
				scopeModeInfo.CollimatorSight.LookAt(point, worldUp);
			}
			if (scopeModeInfo.OpticSight != null)
			{
				scopeModeInfo.OpticSight.LookAt(point, worldUp);
			}
		}
	}

	public void LookAtCollimatorOnly(Vector3 point, Vector3 worldUp)
	{
		for (int i = 0; i < _scopeModeInfos.Length; i++)
		{
			ScopeModeInfo scopeModeInfo = _scopeModeInfos[i];
			if (scopeModeInfo.CollimatorSight != null)
			{
				scopeModeInfo.CollimatorSight.LookAt(point, worldUp);
			}
		}
	}

	public Transform GetLensCenter()
	{
		return FirstCollimator.transform;
	}

	public Vector3 GetLocalCollimatorCameraTarget(Vector3 worldCameraTarget)
	{
		return FirstCollimator.transform.InverseTransformPoint(worldCameraTarget);
	}

	public Vector3 GetLocalOpticCameraTarget(Vector3 worldCameraTarget)
	{
		return FirstOptic.transform.InverseTransformPoint(worldCameraTarget);
	}

	public Vector3 GetLensTransformForward()
	{
		return FirstCollimator.transform.forward;
	}

	public Vector3 GetCollimatorWorldCameraPosition(Vector3 localCameraTarget)
	{
		return _scopeModeInfos[0].CollimatorSight.transform.TransformPoint(localCameraTarget);
	}

	public Vector3 GetOpticsWorldCameraPosition(Vector3 localCameraTarget)
	{
		return FirstOptic.transform.TransformPoint(localCameraTarget);
	}

	public float GetAnyOpticsDistanceToCamera()
	{
		if (_scopeModeInfos[CurrentModeId].OpticSight != null)
		{
			return _scopeModeInfos[CurrentModeId].OpticSight.DistanceToCamera;
		}
		return FirstOptic.DistanceToCamera;
	}
}
