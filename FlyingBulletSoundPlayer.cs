using System;
using System.Collections;
using System.Collections.Generic;
using Comfort.Common;
using EFT;
using EFT.Ballistics;
using EFT.CameraControl;
using UnityEngine;

public class FlyingBulletSoundPlayer : BulletSoundPlayer
{
	[SerializeField]
	private AudioClip[] _sources;

	[_E2BD(0f, 10f, -1f)]
	[SerializeField]
	private Vector2 _minMaxRadius = new Vector2(0f, 10f);

	[SerializeField]
	private AnimationCurve _vignette;

	[SerializeField]
	private float _vignetteTime = 0.4f;

	[SerializeField]
	private float _vignetteDelta = 0.2f;

	private List<BallisticsCalculator> m__E001;

	private Dictionary<string, int> m__E002;

	private string[] m__E003 = new string[20];

	private int m__E004;

	private PrismEffects m__E005;

	private float m__E006 = 0.4f;

	private IEnumerator _E007;

	private Player _E008;

	public override void Init(Camera camera, PlayerCameraController playerCameraController)
	{
		this.m__E005 = camera.GetComponent<PrismEffects>();
		this.m__E006 = this.m__E005.vignetteEnd;
		_E008 = playerCameraController.Player;
		if (_E008 == null)
		{
			Debug.LogError(_ED3E._E000(35664));
		}
	}

	public override void TryShot(_EC26 shot, Camera camera, string id, int currentIndex)
	{
		if (!this.m__E002.ContainsKey(id))
		{
			this.m__E002.Add(id, 0);
		}
		else if (this.m__E002[id] < currentIndex)
		{
			Vector3 start = shot.PositionHistory[this.m__E002[id]];
			Vector3 end = shot.PositionHistory[currentIndex];
			Vector3 vector = _E3BF.CalculateNormalFromPoint(start, end, camera.transform.position);
			if (vector != Vector3.zero)
			{
				_E001(shot, camera.transform.forward, vector);
			}
			this.m__E002[id] = currentIndex;
		}
	}

	private void Awake()
	{
		this.m__E002 = new Dictionary<string, int>();
		_E3BF.OnSniperZoneShoot += _E000;
	}

	private void OnDestroy()
	{
		_E3BF.OnSniperZoneShoot -= _E000;
	}

	private void _E000(SonicBulletSoundPlayer._E001 obj)
	{
		StartCoroutine(_E004());
	}

	private void _E001(_EC26 shot, Vector3 forward, Vector3 normal)
	{
		float magnitude = normal.magnitude;
		if (!(magnitude > _minMaxRadius.y))
		{
			string id = shot.Ammo.Id + shot.FragmentIndex;
			BallisticCollider hittedBallisticCollider = shot.HittedBallisticCollider;
			if (hittedBallisticCollider != null && hittedBallisticCollider.GetType() == typeof(BodyPartCollider))
			{
				_E002(id);
			}
			else
			{
				StartCoroutine(_E003(id, forward, normal, magnitude));
			}
		}
	}

	private void _E002(string id)
	{
		int num = ++this.m__E004 % this.m__E003.Length;
		this.m__E003[num] = id;
	}

	private IEnumerator _E003(string id, Vector3 forward, Vector3 normal, float magnitude)
	{
		yield return null;
		yield return null;
		if (Array.IndexOf(this.m__E003, id) <= 0)
		{
			_E002(id);
			int num = UnityEngine.Random.Range(0, _sources.Length - 1);
			float panStereo = _E3BF.CalculatePan(forward, normal);
			float num2 = Mathf.InverseLerp(_minMaxRadius.y, _minMaxRadius.x, magnitude);
			Singleton<BetterAudio>.Instance.PlayNonspatial(_sources[num], BetterAudio.AudioSourceGroupType.Environment, panStereo, num2);
			_E005(num2);
			_E008.IncreaseAwareness(5f * (1f + num2));
		}
	}

	private IEnumerator _E004()
	{
		yield return null;
		yield return null;
		int num = UnityEngine.Random.Range(0, _sources.Length - 1);
		float panStereo = UnityEngine.Random.Range(-0.9f, 0.9f);
		float num2 = Mathf.InverseLerp(_minMaxRadius.y, _minMaxRadius.x, UnityEngine.Random.Range(0.5f, 1f));
		Singleton<BetterAudio>.Instance.PlayNonspatial(_sources[num], BetterAudio.AudioSourceGroupType.Environment, panStereo, num2);
		_E005(num2);
		_E008.IncreaseAwareness(5f * (1f + num2));
	}

	private void _E005(float coef)
	{
		if (_E007 != null)
		{
			StopCoroutine(_E007);
		}
		this.m__E005.vignetteEnd = this.m__E006;
		_E007 = _E006(this.m__E005.vignetteEnd, coef);
		StartCoroutine(_E007);
	}

	private IEnumerator _E006(float savedValue, float coef)
	{
		float time = Time.time;
		while (Time.time < time + _vignetteTime)
		{
			float num = Time.time - time;
			num /= _vignetteTime;
			float num2 = _vignette.Evaluate(num);
			num2 *= _vignetteDelta * coef;
			this.m__E005.vignetteEnd = savedValue - num2;
			yield return null;
		}
		this.m__E005.vignetteEnd = savedValue;
	}
}
