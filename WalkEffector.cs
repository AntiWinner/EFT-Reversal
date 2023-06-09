using System;
using System.Linq;
using EFT.Animations;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class WalkEffector : IEffector
{
	public enum EWalkPreset
	{
		normal,
		lame,
		sprint,
		duck
	}

	public float StepFrequency = 1f;

	public float Intensity = 1f;

	public float SideSpeedMultyplyer = 1.6f;

	public float BackSpeedMultyplyer = 2f;

	public float Treshold = 0.01f;

	private Vector3 _lastPosition;

	private bool _isWalking;

	public WalkPreset[] Presets;

	[FormerlySerializedAs("CameraPresets")]
	public WalkPreset[] OverweightPresets;

	public _E948[][] PresetProcessors;

	public _E948[][] OverweightPresetProcessors;

	public Vector2[] IntensityMinMax = new Vector2[2]
	{
		new Vector2(0.5f, 1.33f),
		new Vector2(0.5f, 0.7f)
	};

	public float Overweight;

	public EWalkPreset CurrentWalkPreset;

	private float _speed;

	public Transform Transform { get; set; }

	public float Speed
	{
		get
		{
			return _speed;
		}
		set
		{
			_speed = value;
			Vector2 vector = ((CurrentWalkPreset == EWalkPreset.duck) ? IntensityMinMax[1] : IntensityMinMax[0]);
			Intensity = Mathf.Lerp(vector.x, vector.y, _speed);
		}
	}

	public void Initialize(PlayerSpring playerSpring)
	{
		Transform = playerSpring.TrackingTransform;
		PresetProcessors = Presets.Select((WalkPreset p) => p.Curves.Select((AnimVal curve) => new _E948(curve)).ToArray()).ToArray();
		OverweightPresetProcessors = OverweightPresets.Select((WalkPreset p) => p.Curves.Select((AnimVal curve) => new _E948(curve)).ToArray()).ToArray();
		foreach (_E948 item in PresetProcessors.SelectMany((_E948[] presets) => presets))
		{
			item.Initialize(playerSpring.CameraRotation, playerSpring.HandsPosition, playerSpring.HandsRotation, Intensity, StepFrequency, isHeadbobbing: true);
		}
		foreach (_E948 item2 in OverweightPresetProcessors.SelectMany((_E948[] presets) => presets))
		{
			item2.Initialize(playerSpring.CameraRotation, playerSpring.HandsPosition, playerSpring.HandsRotation, Intensity, StepFrequency, isHeadbobbing: true);
		}
	}

	public void OnStop()
	{
		if (PresetProcessors != null)
		{
			_E948[] array = PresetProcessors[(int)CurrentWalkPreset];
			foreach (_E948 obj in array)
			{
				obj.SetupParentValues(Intensity, StepFrequency);
				obj._E000();
			}
			array = OverweightPresetProcessors[(int)CurrentWalkPreset];
			foreach (_E948 obj2 in array)
			{
				obj2.SetupParentValues(Intensity, StepFrequency);
				obj2._E000();
			}
		}
	}

	public void Process(float deltaTime)
	{
		_E948[] array = PresetProcessors[(int)CurrentWalkPreset];
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ProcessRaw(deltaTime * StepFrequency, Intensity);
		}
		if (Overweight > 0f)
		{
			array = OverweightPresetProcessors[(int)CurrentWalkPreset];
			for (int i = 0; i < array.Length; i++)
			{
				array[i].ProcessRaw(deltaTime * StepFrequency, Overweight);
			}
		}
	}

	public string DebugOutput()
	{
		throw new NotImplementedException();
	}

	public void AdjustPose()
	{
		Speed = _speed;
	}
}
