using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using UnityEngine;

namespace EFT.MovingPlatforms;

public class Locomotive : MovingPlatform
{
	[Serializable]
	public class SoundEvent
	{
		public AudioClip Clip;

		public ETravelState TravelState = ETravelState.OnRouteToDestination;

		public float RelativeTime;

		[HideInInspector]
		public float Time;

		public bool Loop;

		public float FadeIn = 1f;

		public bool InterruptsOther;

		public float OtherSourceFadeOut = 2f;

		[Range(0f, 1f)]
		public float Volume = 1f;

		public float SetAbsoluteTime(float[] timings)
		{
			Time = timings[(int)TravelState] + RelativeTime;
			return Time;
		}
	}

	public enum ETravelState
	{
		NotStarted,
		OnRouteToDestination,
		Arrived,
		Departing,
		OnRouteBack,
		Finished
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public float maxDistance;

		public Locomotive _003C_003E4__this;

		internal bool _E000(Keyframe x)
		{
			return x.value == maxDistance;
		}

		internal bool _E001(Keyframe x)
		{
			if (x.value == maxDistance)
			{
				return x.time > _003C_003E4__this._travelStateTiming[2];
			}
			return false;
		}

		internal bool _E002(Keyframe x)
		{
			if (x.value == maxDistance)
			{
				return x.time > _003C_003E4__this._travelStateTiming[2];
			}
			return false;
		}
	}

	private static float _E006 = 90f;

	[SerializeField]
	[Header("Directions Cache")]
	private Vector3[] _cachedDirections;

	[SerializeField]
	private float _cachePredictionDistance = 12f;

	[SerializeField]
	[Header("Sounds")]
	private AudioSource[] Sources;

	public SoundEvent[] SoundEvents;

	[SerializeField]
	[Header("Wheels")]
	private Transform[] WheelTransforms;

	[SerializeField]
	private float _wheelRadius;

	[SerializeField]
	private float[] _travelStateTiming;

	public readonly _ECF5<ETravelState> TravelState = new _ECF5<ETravelState>(ETravelState.NotStarted, _E3A5<ETravelState>.EqualityComparer);

	public Carriage[] Carriage;

	public Action Arrived;

	public Action Departed;

	public float DepartTMinus = 3f;

	public bool OnRoute;

	public float SuspensionBlendFactor;

	private float _E007;

	private Queue<SoundEvent> _E008;

	private float _E009;

	private Action _E00A;

	public override Vector3[] CachedDirections => _cachedDirections;

	public float TravelSpeed => _E009 / Time.deltaTime;

	private new AudioSource _E000
	{
		get
		{
			AudioSource[] sources = Sources;
			foreach (AudioSource audioSource in sources)
			{
				if (audioSource.isPlaying)
				{
					return audioSource;
				}
			}
			return null;
		}
	}

	private new AudioSource _E001
	{
		get
		{
			AudioSource[] sources = Sources;
			foreach (AudioSource audioSource in sources)
			{
				if (!audioSource.isPlaying)
				{
					return audioSource;
				}
			}
			sources = Sources;
			foreach (AudioSource audioSource2 in sources)
			{
				if (!audioSource2.loop)
				{
					return audioSource2;
				}
			}
			return Sources[0];
		}
	}

	public override void Init(DateTime depart)
	{
		base.Init(depart);
		_E008 = new Queue<SoundEvent>();
		for (int i = 0; i < SoundEvents.Length; i++)
		{
			SoundEvent soundEvent = ((i + 1 < SoundEvents.Length) ? SoundEvents[i + 1] : null);
			soundEvent?.SetAbsoluteTime(_travelStateTiming);
			if (soundEvent == null || soundEvent.Time >= base.CurrentRouteTime)
			{
				_E008.Enqueue(SoundEvents[i]);
			}
		}
		_E007 = _E008.Peek().Time;
		if (base.CurrentRouteTime > 0f)
		{
			Move(force: true);
			_E003();
		}
		Carriage[] carriage = Carriage;
		foreach (Carriage obj in carriage)
		{
			obj.Init(depart);
			obj.Move(force: true);
		}
		_E00A = TravelState.Bind(_E000);
	}

	private void _E000(ETravelState state)
	{
		_E57A.Instance.LogInfo(_ED3E._E000(209261), state);
		OnRoute = state == ETravelState.OnRouteBack || state == ETravelState.OnRouteToDestination;
		if (Singleton<_E307>.Instantiated)
		{
			Singleton<_E307>.Instance.TrainCome(state);
		}
		if (state == ETravelState.Finished)
		{
			Carriage[] carriage = Carriage;
			for (int i = 0; i < carriage.Length; i++)
			{
				carriage[i].OnRouteFinished();
			}
			OnRouteFinished();
		}
	}

	public override void OnRouteFinished()
	{
		base.OnRouteFinished();
		_E00A?.Invoke();
	}

	public Vector3 FindNearestPointOnLine(Vector3 origin, Vector3 end, Vector3 point)
	{
		Vector3 vector = end - origin;
		Vector3 rhs = point - origin;
		Vector3 normalized = vector.normalized;
		float num = Vector3.Dot(normalized, rhs);
		if (num <= 0f)
		{
			return origin;
		}
		return origin + Vector3.ClampMagnitude(normalized * num, vector.magnitude);
	}

	private void LateUpdate()
	{
		_E003();
		_E002();
		if (OnRoute)
		{
			_E001();
		}
	}

	private void _E001()
	{
		Quaternion quaternion = Quaternion.Euler((0f - _E009) * 180f / (float)Math.PI / _wheelRadius, 0f, 0f);
		Transform[] wheelTransforms = WheelTransforms;
		for (int i = 0; i < wheelTransforms.Length; i++)
		{
			wheelTransforms[i].localRotation *= quaternion;
		}
	}

	private void _E002()
	{
		if (OnRoute && Carriage.Length != 0 && _E8A8.Instance.Camera != null)
		{
			Sources[0].transform.position = FindNearestPointOnLine(Carriage[Carriage.Length - 1].transform.position, Transform.position, _E8A8.Instance.Camera.transform.position);
		}
		if (!(base.CurrentRouteTime < _E007))
		{
			SoundEvent soundEvent = _E008.Dequeue();
			if (soundEvent.InterruptsOther)
			{
				StartCoroutine(FadeOut(soundEvent.OtherSourceFadeOut));
			}
			StartCoroutine(FadeIn(soundEvent));
			_E007 = ((_E008.Count > 0) ? _E008.Peek().Time : float.PositiveInfinity);
		}
	}

	public IEnumerator FadeIn(SoundEvent sound)
	{
		AudioSource audioSource = this._E001;
		audioSource.enabled = true;
		audioSource.loop = sound.Loop;
		audioSource.clip = sound.Clip;
		if (sound.FadeIn > 0f || sound.Loop)
		{
			audioSource.volume = 0f;
			audioSource.Play();
		}
		else if (Singleton<BetterAudio>.Instantiated)
		{
			Vector3 position = base.transform.position;
			Singleton<BetterAudio>.Instance.PlayAtPoint(position, sound.Clip, 0f, BetterAudio.AudioSourceGroupType.Environment, 400, sound.Volume, EOcclusionTest.None, MonoBehaviourSingleton<BetterAudio>.Instance.OutEnvironment);
			yield break;
		}
		while (audioSource.volume < sound.Volume)
		{
			audioSource.volume += Time.deltaTime / sound.FadeIn * sound.Volume;
			yield return null;
		}
	}

	public IEnumerator FadeOut(float time)
	{
		AudioSource audioSource = this._E000;
		if (!(audioSource == null))
		{
			while (audioSource.volume > 0f)
			{
				audioSource.volume -= Time.deltaTime / time;
				yield return null;
			}
			audioSource.Stop();
			audioSource.enabled = false;
		}
	}

	public void CacheDirections()
	{
		float currentDistance = 0f;
		float currentTime = 0f;
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; (float)i < CurveApproxLength; i++)
		{
			float currentTime2 = 0f;
			Vector3 pointForHomogeneousMovement = Spline.GetPointForHomogeneousMovement(currentDistance, i, Precision, ref currentTime);
			Vector3 pointForHomogeneousMovement2 = Spline.GetPointForHomogeneousMovement(0f, (float)i + _cachePredictionDistance, Precision, ref currentTime2);
			currentDistance = i;
			Vector3 vector = pointForHomogeneousMovement2 - pointForHomogeneousMovement;
			list.Add((Vector3.Distance(pointForHomogeneousMovement2, pointForHomogeneousMovement) < _cachePredictionDistance / 10f) ? Spline.GetDirection(currentTime) : vector.normalized);
		}
		_cachedDirections = list.ToArray();
	}

	public override void Move(bool force = false)
	{
		base.Move(force);
		float num = MovementCurve.Evaluate(base.CurrentRouteTime);
		_E009 = (OnRoute ? (num - _currentDistance) : 0f);
		if (OnRoute || force)
		{
			Vector3 pointForHomogeneousMovement = Spline.GetPointForHomogeneousMovement(_currentDistance, num, Precision, ref _currentTime);
			_currentDistance = num;
			pointForHomogeneousMovement = LinearInterpolator.GetPosition(pointForHomogeneousMovement, _currentDistance, out SuspensionBlendFactor);
			Quaternion rotation = base.RotationAtCurrentDistance;
			Vector3 position = pointForHomogeneousMovement - rotation * _contactPoint;
			Jitter(ref position, ref rotation);
			SetPositionAndRotation(position, rotation);
		}
	}

	private void _E003()
	{
		int value = 0;
		for (int i = 0; i < _travelStateTiming.Length; i++)
		{
			if (base.CurrentRouteTime >= _travelStateTiming[i])
			{
				value = i;
			}
		}
		TravelState.Value = (ETravelState)value;
	}

	[ContextMenu("Start_position")]
	public override void PlaceAtStartPosition()
	{
		float currentTime = 0f;
		Vector3 pointForHomogeneousMovement = Spline.GetPointForHomogeneousMovement(0f, MovementCurve.Evaluate(0f), Precision, ref currentTime);
		_currentDistance = MovementCurve.Evaluate(0f);
		Quaternion rotationAtCurrentDistance = base.RotationAtCurrentDistance;
		_currentDistance = 0f;
		Vector3 position = pointForHomogeneousMovement - rotationAtCurrentDistance * _contactPoint;
		base.transform.SetPositionAndRotation(position, rotationAtCurrentDistance);
		Carriage[] carriage = Carriage;
		for (int i = 0; i < carriage.Length; i++)
		{
			carriage[i].PlaceAtStartPosition();
		}
	}

	public float GetArrivedTiming()
	{
		return _travelStateTiming[2];
	}

	[ContextMenu("End_Position")]
	public override void PlaceAtEndPosition()
	{
		float num = MovementCurve.Evaluate(GetArrivedTiming());
		float currentTime = 0f;
		Vector3 pointForHomogeneousMovement = Spline.GetPointForHomogeneousMovement(0f, num, Precision, ref currentTime);
		Debug.Log(string.Format(_ED3E._E000(209229), base.gameObject.name, pointForHomogeneousMovement.x, pointForHomogeneousMovement.y, pointForHomogeneousMovement.z, num));
		pointForHomogeneousMovement = LinearInterpolator.GetPosition(pointForHomogeneousMovement, num);
		Quaternion quaternion = RotationAtDistance(num);
		Vector3 position = pointForHomogeneousMovement - quaternion * _contactPoint;
		base.transform.SetPositionAndRotation(position, quaternion);
		Carriage[] carriage = Carriage;
		for (int i = 0; i < carriage.Length; i++)
		{
			carriage[i].PlaceAtEndPosition();
		}
	}

	public override void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(base.transform.TransformPoint(_contactPoint), 0.2f);
		base.OnDrawGizmosSelected();
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(LinearInterpolator.Point1, 0.1f);
		Gizmos.DrawWireSphere(LinearInterpolator.Point2, 0.1f);
	}

	public void SetBoardingTime(float time)
	{
		if (!(time <= 1f))
		{
			float num = MovementCurve.keys[3].time - MovementCurve.keys[2].time;
			float num2 = time - num;
			if (num2 > 0f)
			{
				_E004(4, num2);
				_E004(3, num2);
			}
			else
			{
				_E004(3, num2);
				_E004(4, num2);
			}
			CalculateLength();
		}
	}

	private void _E004(int index, float dt)
	{
		MovementCurve.MoveKey(index, new Keyframe(MovementCurve.keys[index].time + dt, MovementCurve.keys[index].value, MovementCurve.keys[index].inTangent, MovementCurve.keys[index].outTangent, MovementCurve.keys[index].inWeight, MovementCurve.keys[index].outWeight));
	}

	public override void CalculateLength()
	{
		_travelStateTiming = new float[Enum.GetNames(typeof(ETravelState)).Length];
		_travelStateTiming[0] = 0f;
		_travelStateTiming[1] = float.Epsilon;
		float maxDistance = MovementCurve.keys.Max((Keyframe x) => x.value);
		_travelStateTiming[2] = MovementCurve.keys.FirstOrDefault((Keyframe x) => x.value == maxDistance).time;
		_travelStateTiming[3] = MovementCurve.keys.FirstOrDefault((Keyframe x) => x.value == maxDistance && x.time > _travelStateTiming[2]).time - DepartTMinus;
		_travelStateTiming[4] = MovementCurve.keys.FirstOrDefault((Keyframe x) => x.value == maxDistance && x.time > _travelStateTiming[2]).time;
		_travelStateTiming[5] = MovementCurve.GetDuration();
		base.CalculateLength();
	}
}
