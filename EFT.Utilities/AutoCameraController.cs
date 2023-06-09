using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Cinemachine;
using UnityEngine;

namespace EFT.Utilities;

[RequireComponent(typeof(CinemachinePathBase))]
public class AutoCameraController : MonoBehaviour
{
	[Serializable]
	public enum EAction
	{
		None,
		AimObject,
		AimPoint,
		AimForward,
		Wait
	}

	[Serializable]
	public struct PointAction
	{
		public float Index;

		public EAction Action;

		public string StringValue;

		public float FloatValue;

		public Vector3 VectorValue;
	}

	private const string m__E000 = "Debug/AutoCamera";

	public CinemachineVirtualCamera CamVM;

	public float Speed;

	public PointAction[] Points;

	[CompilerGenerated]
	private Action _E001;

	[CompilerGenerated]
	private float _E002;

	private CinemachineTrackedDolly _E003;

	private Queue<(PointAction, Transform)> _E004;

	private float _E005;

	private bool _E006;

	private bool _E007;

	private GameObject _E008;

	private PointAction _E009;

	public bool IsComplete => _E006;

	public bool IsProcess => _E007;

	public float CurrentWaypointIndex
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

	public event Action OnComplete
	{
		[CompilerGenerated]
		add
		{
			Action action = _E001;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E001;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private void Awake()
	{
		CamVM.enabled = false;
	}

	public void Setup()
	{
		_E003 = CamVM.GetCinemachineComponent<CinemachineTrackedDolly>();
		_E003.m_PathPosition = 0f;
		_E006 = false;
		CurrentWaypointIndex = 0f;
		if (_E008 == null)
		{
			_E008 = new GameObject(_ED3E._E000(217486));
			_E008.transform.SetParent(base.transform, worldPositionStays: false);
		}
		if (_E004 == null)
		{
			_E004 = new Queue<(PointAction, Transform)>();
		}
		_E004.Enqueue((new PointAction
		{
			Action = EAction.AimForward
		}, null));
		foreach (PointAction item2 in Points.OrderBy((PointAction o) => o.Index))
		{
			Transform item = null;
			if (item2.Action == EAction.AimObject)
			{
				item = _E05E.FindObjectByFullPath(item2.StringValue);
			}
			_E004.Enqueue((item2, item));
		}
		_E003.m_Path.InvalidateDistanceCache();
		CamVM.enabled = true;
	}

	public void ManualUpdate(float deltaTime)
	{
		if (_E006)
		{
			return;
		}
		_E007 = true;
		_E005 -= deltaTime;
		if (_E005 > 0f)
		{
			return;
		}
		_E003.m_PathPosition += Speed * deltaTime;
		CurrentWaypointIndex = _E003.m_Path.ToNativePathUnits(_E003.m_PathPosition, CinemachinePathBase.PositionUnits.Distance);
		if (_E004.Count > 0)
		{
			var (pointAction, lookAt) = _E004.Peek();
			if (pointAction.Index <= CurrentWaypointIndex)
			{
				_E004.Dequeue();
				switch (pointAction.Action)
				{
				case EAction.AimObject:
					CamVM.LookAt = lookAt;
					break;
				case EAction.AimPoint:
					_E008.transform.position = pointAction.VectorValue;
					CamVM.LookAt = _E008.transform;
					break;
				case EAction.Wait:
					_E005 = pointAction.FloatValue;
					break;
				case EAction.AimForward:
					CamVM.LookAt = _E008.transform;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				case EAction.None:
					break;
				}
				_E009 = pointAction;
			}
		}
		if (_E009.Action == EAction.AimForward)
		{
			Quaternion quaternion = _E003.m_Path.EvaluateOrientationAtUnit(_E003.m_PathPosition + Speed / 2f, _E003.m_PositionUnits);
			Vector3 vector = _E003.m_Path.EvaluatePositionAtUnit(_E003.m_PathPosition + Speed / 2f, _E003.m_PositionUnits);
			_E008.transform.position = vector + quaternion * Vector3.forward;
		}
		if (_E003.m_PathPosition >= _E003.m_Path.PathLength)
		{
			_E000();
		}
	}

	public void Clear()
	{
		CamVM.enabled = false;
		_E003.m_PathPosition = 0f;
		CurrentWaypointIndex = 0f;
		_E004?.Clear();
		if (!_E006)
		{
			_E000();
		}
	}

	private void _E000()
	{
		_E007 = false;
		_E006 = true;
		_E001?.Invoke();
	}

	private void OnDestroy()
	{
		_E004 = null;
	}
}
