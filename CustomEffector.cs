using System;
using System.Linq;
using EFT;
using EFT.Animations;
using UnityEngine;

[Serializable]
public class CustomEffector : IEffector
{
	private bool _inited;

	public WalkPreset AimPose;

	public float RestTransitionSpeed = 2f;

	public float RestDelay = 10f;

	public AnimVal RestValues;

	public _E948[] _aimPose;

	private _E948 _restProcessor;

	private float _lastActivityTime;

	private float _initAimRotation;

	public Player.ValueBlender PoseBlender = new Player.ValueBlender
	{
		Speed = 4f
	};

	private float _poseNormalTime;

	private float _aimTarget;

	public Vector3 Offset => new Vector3(-0.002f, 0f, 0.02f) * (1f - PoseBlender.Value);

	public bool Aim
	{
		set
		{
			if (_inited)
			{
				PoseBlender.Target = (value ? 1 : 0);
				_aimPose[0].Processors[1].Intensity = ((PoseBlender.Target == 1f) ? _initAimRotation : (0f - _initAimRotation));
			}
		}
	}

	public void UpdateActivityTime()
	{
		_lastActivityTime = Time.time;
	}

	public void Initialize(PlayerSpring playerSpring)
	{
		UpdateActivityTime();
		_aimPose = AimPose.Curves.Select((AnimVal c) => new _E948(c)).ToArray();
		_E948[] aimPose = _aimPose;
		for (int i = 0; i < aimPose.Length; i++)
		{
			aimPose[i].Initialize(null, playerSpring.HandsPosition, playerSpring.HandsRotation);
		}
		if (RestValues != null)
		{
			_restProcessor = new _E948(RestValues);
			_restProcessor.Initialize(null, playerSpring.HandsPosition, playerSpring.HandsRotation);
		}
		_initAimRotation = AimPose[0].Vals[1].Intensity;
		_inited = true;
	}

	public void Process(float intensity)
	{
		if (!(Math.Abs(PoseBlender.Value) < float.Epsilon))
		{
			_E948[] aimPose = _aimPose;
			for (int i = 0; i < aimPose.Length; i++)
			{
				aimPose[i].ProcessAtTime(PoseBlender.Value, intensity);
			}
		}
	}

	public void ProcessRestPose()
	{
		if (_restProcessor != null)
		{
			float num = Time.time - _lastActivityTime - RestDelay;
			if (num > 0f)
			{
				num = Mathf.Clamp01(num * RestTransitionSpeed);
				_restProcessor.ProcessAtTime(num);
			}
		}
	}

	public string DebugOutput()
	{
		throw new NotImplementedException();
	}

	public void Clear()
	{
		RestValues = null;
		_restProcessor = null;
	}
}
