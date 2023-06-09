using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using UnityEngine;

namespace EFT.Interactive;

[ExecuteInEditMode]
public class ExfiltrationDoor : ExfiltrationSubscriber
{
	[Serializable]
	public class DoorTransform
	{
		[HideInInspector]
		public Quaternion OpenRotation;

		[HideInInspector]
		public Quaternion CloseRotation;

		[HideInInspector]
		public Vector3 OpenPosition;

		[HideInInspector]
		public Vector3 ClosePosition;

		[HideInInspector]
		public Vector3 OpenScale = Vector3.one;

		[HideInInspector]
		public Vector3 CloseScale = Vector3.one;

		public Transform Transform;

		public bool PlaySoundAt;

		[HideInInspector]
		public Vector3 StartPosition;

		[HideInInspector]
		public Vector3 StartScale;

		[HideInInspector]
		public Quaternion StartRotation;

		public void StoreValues()
		{
			StartRotation = Transform.localRotation;
			StartPosition = Transform.localPosition;
			StartScale = Transform.localScale;
		}
	}

	[SerializeField]
	private DoorTransform[] _doorTransforms;

	[HideInInspector]
	[Obsolete]
	public Quaternion OpenRotation;

	[Obsolete]
	[HideInInspector]
	public Quaternion CloseRotation;

	[Obsolete]
	[HideInInspector]
	public Vector3 OpenPosition;

	[Obsolete]
	[HideInInspector]
	public Vector3 ClosePosition;

	[Obsolete]
	[HideInInspector]
	public Vector3 OpenScale = Vector3.one;

	[Obsolete]
	[HideInInspector]
	public Vector3 ClosedScale = Vector3.one;

	public bool DisableWhenClosed;

	private Coroutine m__E004;

	public AudioClip OpenClip;

	public AudioClip CloseClip;

	public float Volume = 1f;

	public int Rolloff = 50;

	public float LerpSpeed = 2f;

	public bool ForceStateOnStart;

	public AnimationCurve Curve;

	public EExfiltrationStatus[] OpenStatus = new EExfiltrationStatus[0];

	private bool _E005;

	[SerializeField]
	private List<Collider> _collisionColliders;

	private List<Collider> _E000
	{
		get
		{
			if (_collisionColliders == null || _collisionColliders.Count < 1)
			{
				_E000();
			}
			return _collisionColliders;
		}
	}

	public DoorTransform[] DoorTransforms => _doorTransforms;

	[ContextMenu("Cache collision colliders")]
	private void _E000()
	{
		_collisionColliders = new List<Collider>();
		Collider[] array;
		if (_doorTransforms.Length != 0)
		{
			array = new Collider[0];
			DoorTransform[] doorTransforms = _doorTransforms;
			foreach (DoorTransform doorTransform in doorTransforms)
			{
				array = array.Concat(doorTransform.Transform.GetComponentsInChildren<Collider>()).ToArray();
			}
		}
		else
		{
			array = base.gameObject.GetComponentsInChildren<Collider>();
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (_E004(array[j]))
			{
				_collisionColliders.Add(array[j]);
			}
		}
	}

	public void Awake()
	{
		if (OpenPosition.sqrMagnitude <= 0f && ClosePosition.sqrMagnitude <= 0f)
		{
			OpenPosition = (ClosePosition = base.transform.localPosition);
		}
		if (DoorTransforms == null || DoorTransforms.Length < 1)
		{
			_doorTransforms = new DoorTransform[1]
			{
				new DoorTransform
				{
					OpenPosition = OpenPosition,
					OpenRotation = OpenRotation,
					OpenScale = OpenScale,
					ClosePosition = ClosePosition,
					CloseRotation = CloseRotation,
					CloseScale = ClosedScale,
					Transform = base.transform
				}
			};
		}
	}

	private void _E001(bool value)
	{
	}

	public override void Start()
	{
		base.Start();
		if (ForceStateOnStart && (bool)Subscribee)
		{
			OnStatusChangedHandler(Subscribee, Subscribee.Status, force: true);
		}
	}

	protected override void OnStatusChangedHandler(ExfiltrationPoint point, EExfiltrationStatus prevStatus)
	{
		OnStatusChangedHandler(point, prevStatus, force: false);
	}

	protected void OnStatusChangedHandler(ExfiltrationPoint point, EExfiltrationStatus prevStatus, bool force)
	{
		bool flag = OpenStatus.Contains(point.Status);
		if (flag != _E005 || force)
		{
			_E005 = flag;
			bool num = Curve != null && Curve.keys.Length > 1;
			AudioClip audioClip = null;
			audioClip = (flag ? OpenClip : CloseClip);
			if (this.m__E004 != null)
			{
				StaticManager.KillCoroutine(this.m__E004);
				_E001(value: true);
			}
			if (num)
			{
				this.m__E004 = StaticManager.BeginCoroutine(flag ? _E002(isOpen: true, audioClip, Delay) : _E002(isOpen: false, audioClip, Delay));
			}
			else
			{
				this.m__E004 = StaticManager.BeginCoroutine(flag ? _E003(OpenRotation, OpenPosition, audioClip, Delay) : _E003(CloseRotation, ClosePosition, audioClip, Delay));
			}
		}
	}

	private IEnumerator _E002(bool isOpen, AudioClip clip, float delay)
	{
		yield return new WaitForSeconds(delay);
		if (DisableWhenClosed && _E005)
		{
			base.gameObject.SetActive(value: true);
		}
		float time = Time.time;
		_E001(value: false);
		DoorTransform[] doorTransforms = DoorTransforms;
		foreach (DoorTransform doorTransform in doorTransforms)
		{
			doorTransform.StoreValues();
			if (doorTransform.PlaySoundAt && clip != null && Singleton<BetterAudio>.Instantiated)
			{
				Singleton<BetterAudio>.Instance.PlayAtPoint(base.transform.position, clip, _E8A8.Instance.Distance(doorTransform.Transform.position), BetterAudio.AudioSourceGroupType.Environment, Rolloff, Volume);
			}
		}
		float duration = Curve.GetDuration();
		float num;
		do
		{
			num = Time.time - time;
			float t = Curve.Evaluate(num);
			doorTransforms = DoorTransforms;
			foreach (DoorTransform doorTransform2 in doorTransforms)
			{
				doorTransform2.Transform.localRotation = Quaternion.Lerp(doorTransform2.StartRotation, isOpen ? doorTransform2.OpenRotation : doorTransform2.CloseRotation, t);
				doorTransform2.Transform.localPosition = Vector3.Lerp(doorTransform2.StartPosition, isOpen ? doorTransform2.OpenPosition : doorTransform2.ClosePosition, t);
				doorTransform2.Transform.localScale = Vector3.Lerp(doorTransform2.StartScale, isOpen ? doorTransform2.OpenScale : doorTransform2.CloseScale, t);
			}
			yield return null;
		}
		while (num < duration);
		doorTransforms = DoorTransforms;
		foreach (DoorTransform doorTransform3 in doorTransforms)
		{
			doorTransform3.Transform.localRotation = (isOpen ? doorTransform3.OpenRotation : doorTransform3.CloseRotation);
			doorTransform3.Transform.localPosition = (isOpen ? doorTransform3.OpenPosition : doorTransform3.ClosePosition);
			doorTransform3.Transform.localScale = (isOpen ? doorTransform3.OpenScale : doorTransform3.CloseScale);
		}
		this.m__E004 = null;
		_E001(value: true);
		if (DisableWhenClosed && !_E005)
		{
			base.gameObject.SetActive(value: false);
		}
	}

	private IEnumerator _E003(Quaternion q, Vector3 pos, AudioClip clip, float delay)
	{
		yield return new WaitForSeconds(delay);
		if (clip != null && Singleton<BetterAudio>.Instantiated)
		{
			Singleton<BetterAudio>.Instance.PlayAtPoint(base.transform.position, clip, _E8A8.Instance.Distance(base.transform.position), BetterAudio.AudioSourceGroupType.Environment, Rolloff, Volume);
		}
		do
		{
			base.transform.localRotation = Quaternion.Lerp(base.transform.localRotation, q, Time.deltaTime * LerpSpeed);
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, pos, Time.deltaTime * LerpSpeed);
			yield return null;
		}
		while (base.transform != null && ((double)Quaternion.Angle(base.transform.localRotation, q) > 0.05 || Vector3.Distance(base.transform.localPosition, pos) > 0.0025f));
		base.transform.localRotation = q;
		base.transform.localPosition = pos;
		this.m__E004 = null;
	}

	[ContextMenu("try open")]
	public void TryOpen()
	{
		DoorTransform[] doorTransforms = DoorTransforms;
		foreach (DoorTransform doorTransform in doorTransforms)
		{
			doorTransform.Transform.localPosition = doorTransform.OpenPosition;
			doorTransform.Transform.localRotation = doorTransform.OpenRotation;
			doorTransform.Transform.localScale = doorTransform.OpenScale;
		}
	}

	[ContextMenu("try close")]
	public void TryClose()
	{
		DoorTransform[] doorTransforms = DoorTransforms;
		foreach (DoorTransform doorTransform in doorTransforms)
		{
			doorTransform.Transform.localPosition = doorTransform.ClosePosition;
			doorTransform.Transform.localRotation = doorTransform.CloseRotation;
			doorTransform.Transform.localScale = doorTransform.CloseScale;
		}
	}

	[ContextMenu("Set open")]
	public void SetOpenRotation()
	{
		DoorTransform[] doorTransforms = DoorTransforms;
		foreach (DoorTransform obj in doorTransforms)
		{
			obj.OpenRotation = obj.Transform.localRotation;
			obj.OpenPosition = obj.Transform.localPosition;
			obj.OpenScale = obj.Transform.localScale;
		}
	}

	[ContextMenu("Set close")]
	public void SetCloseRotation()
	{
		DoorTransform[] doorTransforms = DoorTransforms;
		foreach (DoorTransform obj in doorTransforms)
		{
			obj.CloseRotation = obj.Transform.localRotation;
			obj.ClosePosition = obj.Transform.localPosition;
			obj.CloseScale = obj.Transform.localScale;
		}
	}

	private void OnDestroy()
	{
		if (StaticManager.Instance != null)
		{
			StaticManager.KillCoroutine(this.m__E004);
		}
	}

	[CompilerGenerated]
	internal static bool _E004(Collider currentCollider)
	{
		return ((int)_E37B.PlayerStaticCollisionsMask & (1 << currentCollider.gameObject.layer)) != 0;
	}
}
