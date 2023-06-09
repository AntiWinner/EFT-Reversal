using System;
using UnityEngine;

[CreateAssetMenu]
public class ObservedPlayerInterpolationSettings : ScriptableObject
{
	[Serializable]
	public class ObservedMoveStateDumpers
	{
		[Range(0f, 1f)]
		public float ROTATION_DUMPER = 0.035f;

		[Range(0f, 1f)]
		public float DIRECTION_DUMPER = 0.035f;

		[Range(0f, 1f)]
		public float DISCRETE_DIRECTION_DAMPER = 0.2f;

		[Range(0f, 1f)]
		public float MOVE_DUMPER = 0.779f;

		[Range(0f, 1f)]
		public float MOVE_DUMPER_0_15_POW2 = 0.286f;

		[Range(0f, 1f)]
		public float MOVE_DUMPER_0_1_POW2 = 0.427f;

		[Range(0f, 1f)]
		public float MOVE_DUMPER_0_07_POW2 = 0.481f;

		[Range(0f, 1f)]
		public float MOVE_DUMPER_MAX = 0.538f;

		[Range(0f, 0.5f)]
		public float MIN_TIME_IN_MOVE_UNTIL_FORCE_STATE_CHANGE = 0.15f;

		[Range(0f, 1f)]
		public float MAX_EXTRAPOLATION_TIME = 0.3f;
	}

	[Serializable]
	public class ObservedIdleStateDumpers
	{
		[Range(0f, 1f)]
		public float ROTATION_DUMPER = 0.37f;

		[Range(0f, 1f)]
		public float MOVE_DUMPER = 0.758f;

		[Range(0f, 0.5f)]
		public float MIN_TIME_IN_IDLE_UNTIL_FORCE_STATE_CHANGE = 0.15f;
	}

	[Serializable]
	public class ObservedJumpStateDumpers
	{
		[Range(0f, 1f)]
		public float ROTATION_DUMPER = 0.05f;

		[Range(0f, 1f)]
		public float MOVE_DUMPER_0_15_POW2 = 0.286f;

		[Range(0f, 1f)]
		public float MOVE_DUMPER_0_1_POW2 = 0.427f;

		[Range(0f, 1f)]
		public float MOVE_DUMPER_0_07_POW2 = 0.481f;

		[Range(0f, 1f)]
		public float MOVE_DUMPER_MAX = 0.538f;

		[Range(0f, 0.5f)]
		public float MIN_TIME_IN_JUMP_UNTIL_FORCE_STATE_CHANGE = 0.15f;
	}

	[Serializable]
	public class ObservedSprintStateDumpers
	{
		[Range(0f, 1f)]
		public float ROTATION_DUMPER = 0.5f;

		[Range(0f, 1f)]
		public float MOVE_DUMPER_0_15_POW2 = 0.286f;

		[Range(0f, 1f)]
		public float MOVE_DUMPER_0_1_POW2 = 0.427f;

		[Range(0f, 1f)]
		public float MOVE_DUMPER_0_07_POW2 = 0.481f;

		[Range(0f, 1f)]
		public float MOVE_DUMPER_MAX = 0.538f;

		[Range(0f, 0.5f)]
		public float MIN_TIME_IN_SPRINT_UNTIL_FORCE_STATE_CHANGE = 0.15f;

		[Range(0f, 1f)]
		public float MAX_EXTRAPOLATION_TIME = 0.3f;
	}

	[Serializable]
	public class ObservedInteractDumper
	{
		[Range(0f, 1f)]
		public float ROTATION_DUMPER = 0.75f;

		[Range(0f, 1f)]
		public float MOVE_DUMPER = 0.758f;
	}

	[Serializable]
	public class RttDTMultiplier
	{
		public float RttMultiplier;

		public float DtMultiplier;

		public Color Color;
	}

	private static ObservedPlayerInterpolationSettings _E000;

	public ObservedIdleStateDumpers IdleStateDumpers = new ObservedIdleStateDumpers();

	public ObservedMoveStateDumpers MoveStateDumpers = new ObservedMoveStateDumpers();

	public ObservedSprintStateDumpers SprintStateDumpers = new ObservedSprintStateDumpers();

	public ObservedJumpStateDumpers JumpStateDumpers = new ObservedJumpStateDumpers();

	public ObservedInteractDumper InteractStateDumpers = new ObservedInteractDumper();

	[Header("---------- Warp params")]
	public float MOVE_STATE_DISTANCE = 1.5f;

	public float SPRINT_STATE_DISTANCE = 1.5f;

	public float JUMP_STATE_DISTANCE = 1.5f;

	public float IDLE_STATE_DISTANCE = 1.5f;

	[Header("---------- BufferSizeConfig")]
	public RttDTMultiplier[] RttDtMultipliers = new RttDTMultiplier[5]
	{
		new RttDTMultiplier
		{
			RttMultiplier = 5f,
			DtMultiplier = 4f,
			Color = Color.red
		},
		new RttDTMultiplier
		{
			RttMultiplier = 3f,
			DtMultiplier = 2f,
			Color = Color.green
		},
		new RttDTMultiplier
		{
			RttMultiplier = 1.5f,
			DtMultiplier = 1.3f,
			Color = Color.magenta
		},
		new RttDTMultiplier
		{
			RttMultiplier = 1.05f,
			DtMultiplier = 1.1f,
			Color = Color.blue
		},
		new RttDTMultiplier
		{
			RttMultiplier = 0.5f,
			DtMultiplier = 0.7f,
			Color = Color.white
		}
	};

	[Range(0f, 1f)]
	public double ALPHA = 0.699999988079071;

	[Range(0f, 1f)]
	public double BETA = 0.699999988079071;

	public bool UseMovingAverage = true;

	public bool UseDOUBLEMovingAverage;

	public static ObservedPlayerInterpolationSettings Instance
	{
		get
		{
			if (_E000 != null)
			{
				return _E000;
			}
			_E000 = _E3A2.Load<ObservedPlayerInterpolationSettings>(_ED3E._E000(45461));
			return _E000;
		}
	}
}
