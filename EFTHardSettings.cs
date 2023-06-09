using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using EFT.Ballistics;
using EFT.InventoryLogic;
using EFT.UI;
using JetBrains.Annotations;
using UnityEngine;

public sealed class EFTHardSettings : ScriptableObject
{
	[Serializable]
	public class ShellsSettings
	{
		public float radius = 0.001f;

		[Tooltip("Playback speed")]
		public float playMult = 1f;

		[Tooltip("Bigger value for better smoothness")]
		public int maxCastCount = 10;

		public float deltaTimeStep = 0.3f;

		public float randomReboundSpread = 0.25f;

		[Tooltip("intial speed")]
		public float velocityMult = 1f;

		public float velocityRotation;

		public float bounceSpeedMult = 1f;

		public float enforceSurfaceNormals = 1f;

		public Vector2 ReboundRotationX;

		public Vector2 ReboundRotationY;

		public Vector2 ReboundRotationZ;

		public bool showDebug;
	}

	public const string PATH = "eftsettings";

	private static EFTHardSettings _E000;

	public AnimationCurve PoseInertiaDamp;

	public AnimationCurve SpeedLimitAfterFall;

	public AnimationCurve SpeedLimitDuration;

	public AnimationCurve PoseInertiaOverFallDistance;

	public AnimationCurve CrouchAccelerationWeightRange;

	public AnimationCurve MovementAccelerationRange;

	public float IdleStateMotionPreservation = 0.98f;

	public float InertiaInputMaxSpeed = 100f;

	public float DecelerationSpeed = 1.5f;

	public AnimationCurve InertiaTiltCurve;

	public float StartingSprintSpeed = 0.5f;

	public AnimationCurve sprintSpeedInertiaCurve;

	public AnimationCurve PlaneHeightChangeCurve;

	public AnimationCurve PlaneVolumeCurve;

	public AnimationCurve AdditionalTimeCurve;

	[Header("Airdrop")]
	public float WindFactor = 3f;

	public float WindTurningOffHeight = 100f;

	public float ShakingCoef = 5f;

	public float ShakingTime = 0.5f;

	public Vector3 DebugWind = new Vector3(1f, 0f, 1f);

	[Header("Swing")]
	public float TiltAngleDeceleration = 2.5f;

	public float SwingSpeed = 0.3f;

	public float MinTiltAngle = 10f;

	public float MaxTiltAngle = 30f;

	public float MinSwingValue = 5f;

	[Header("Torque")]
	public float MaxTorque = 30f;

	public float TorqueDeceleration = 2f;

	public float ToqueMinValue = 5f;

	public float DeltaTimeModificator = 50f;

	public AnimationCurve XPositionCurve;

	public AnimationCurve YPositionCurve;

	public AnimationCurve ZPositionCurve;

	public AnimationCurve XRotationCurve;

	public AnimationCurve YRotationCurve;

	public AnimationCurve ZRotationCurve;

	[NotNull]
	public AnimationCurve DoorCurve;

	public EPlayerState[] UnsuitableStates;

	public AnimationCurve TinnitusSound;

	public AnimationCurve TinnitusLowpas;

	public AnimationCurve MainChannelLevel;

	public AnimationCurve StairsSoundOcclusionCurve;

	[Range(0f, 2f)]
	public float VerticalSoundFactor = 1f;

	public float SoundOcclusionUpdateInterval = 0.4f;

	public float SoundOcclusionWidening = 1f;

	public float ListenerOcclusionWidening = 1f;

	public float OcclusionHeight = 0.3f;

	public bool DebugRays;

	public bool DebugPropagationPath;

	public bool SoundDebugConsole;

	public Vector3[] BleedingCenters = new Vector3[8]
	{
		new Vector3(0f, 0f, -1f),
		new Vector3(0f, 0f, -1f),
		new Vector3(0f, 0f, -1f),
		new Vector3(-1f, 0f, -0.5f),
		new Vector3(1f, 0f, -0.5f),
		new Vector3(-1f, 0f, -1f),
		new Vector3(1f, 0f, -1f),
		new Vector3(0f, 0f, 0f)
	};

	public float MAXIMUM_BODYPART_VELOCITY = 5f;

	public float HIT_FORCE = 150f;

	public float AIR_CONTROL_SAME_DIR = 1.2f;

	public float AIR_CONTROL_BACK_DIR = 0.6f;

	public float AIR_CONTROL_NONE_OR_ORT_DIR = 0.9f;

	public float JumpTimeDescendingForStateExit = 0.3f;

	public float PICKUP_DELAY = 1f;

	public AnimationCurve CURVA;

	public float LOOT_RAYCAST_DISTANCE = 1f;

	public float DOOR_RAYCAST_DISTANCE = 0.75f;

	public float BEHIND_CAST = 0.3f;

	public float PLAYER_RAYCAST_DISTANCE = 2.5f;

	public float CULL_RECOIL = 10f;

	public float CULL_GROUNDER = 40f;

	public float CULL_GROUNDER_QUALITY = 10f;

	[Header("Player.AudioController")]
	public AnimationCurve SoundRolloff;

	public AnimationCurve SpreadCurve;

	public float TURN_SOUND_DELAY = 0.5f;

	public float TURN_ANGLE = 45f;

	public float GEAR_SOUND_DELAY = 0.2f;

	public AnimationCurve LOWER_LOWPASS;

	public AnimationCurve UPPER_LOWPASS;

	public LayerMask GunshotMask;

	public LayerMask BaseMask;

	public Vector2 VERTICAL_DOT_RANGE;

	public Vector2 VERTICAL_DOT_RANGE_STAIRS;

	public AnimationCurve VOLUME_BY_FLOOR_THICKNESS;

	public bool SqrRootDecay;

	public Vector2 ABOVE_OR_BELOW = new Vector2(2f, -2f);

	public Vector2 ABOVE_OR_BELOW_STAIRS = new Vector2(2f, -2f);

	[Header("Weapon")]
	public float STOP_AIMING_AT;

	public bool OFFSET_FOLDED_WEAPON = true;

	public double IDLING_MAX_TIME = 300.0;

	public Weapon.EMalfunctionState FORCE_MALFUNCTION;

	public bool FORCE_MALFUNCTION_LOOPED;

	public bool FORCE_MALFUNCTION_ONCE;

	public bool NEXT_MALF_IS_UNKNOWN;

	public float FORCE_NEXT_MALF_CHANCE = -1f;

	public int FORCE_MALFUNCTION_IN_SHOT;

	public LayerMask WEAPON_OCCLUSION_LAYERS;

	public LayerMask WEAPON_OCCLUSION_SERVER_LAYERS;

	public bool OVERHEAT_ALLOW_REDUCE_COI = true;

	public bool OVERHEAT_ALLOW_BARREL_MOVE = true;

	public float OVERHEAT_INCREASE_MULT = 1f;

	public float DURABILITY_LOSS_MULT = 1f;

	[Header("Shell Shot Extraction")]
	public Vector2 SHELLS_EXCTRACT_FORCE_X_RANGE = new Vector2(1f, 2f);

	public Vector2 SHELLS_EXCTRACT_FORCE_Y_RANGE = new Vector2(0.1f, 0.72f);

	public Vector2 SHELLS_EXCTRACT_FORCE_Z_RANGE = new Vector2(-0.2f, 0.2f);

	public Vector2 SHELLS_EXCTRACT_ROTATION_RANGE_X = new Vector2(2f, 5f);

	public Vector2 SHELLS_EXCTRACT_ROTATION_RANGE_Y = new Vector2(2f, 5f);

	public Vector2 SHELLS_EXCTRACT_ROTATION_RANGE_Z = new Vector2(2f, 5f);

	public float SHELL_FORCE_MULTIPLIER = 1f;

	[Header("Shell MISFIRE Extraction")]
	public Vector2 SHELLS_MISFIRE_FORCE_X_RANGE = new Vector2(1f, 2f);

	public Vector2 SHELLS_MISFIRE_FORCE_Y_RANGE = new Vector2(0.1f, 0.72f);

	public Vector2 SHELLS_MISFIRE_FORCE_Z_RANGE = new Vector2(-0.2f, 0.2f);

	public Vector2 SHELLS_MISFIRE_ROTATION_RANGE_X = new Vector2(2f, 5f);

	public Vector2 SHELLS_MISFIRE_ROTATION_RANGE_Y = new Vector2(2f, 5f);

	public Vector2 SHELLS_MISFIRE_ROTATION_RANGE_Z = new Vector2(2f, 5f);

	[Header("Shell JAM Extraction")]
	public Vector2 SHELLS_JAM_FORCE_X_RANGE = new Vector2(1f, 2f);

	public Vector2 SHELLS_JAM_FORCE_Y_RANGE = new Vector2(0.1f, 0.72f);

	public Vector2 SHELLS_JAM_FORCE_Z_RANGE = new Vector2(-0.2f, 0.2f);

	public Vector2 SHELLS_JAM_ROTATION_RANGE_X = new Vector2(2f, 5f);

	public Vector2 SHELLS_JAM_ROTATION_RANGE_Y = new Vector2(2f, 5f);

	public Vector2 SHELLS_JAM_ROTATION_RANGE_Z = new Vector2(2f, 5f);

	[Header("Grenade")]
	public float GrenadeForce = 20f;

	public ForceMode GrenadeForceMode = ForceMode.Impulse;

	[Header("Visual Effects")]
	public bool PLAYER_HIT_DECALS_ENEBLED = true;

	public bool HIT_EFFECTS_ENABLED = true;

	public bool HEAT_EMITTER_ENABLED = true;

	public bool SHOT_EFFECTS_ENABLED = true;

	public bool DEFERRED_DECALS_ENABLED = true;

	public bool STATIC_DEFERRED_DECALS_ENABLED = true;

	[Header("Player")]
	public float SIDESTEP_TO_MOVE_DURATION = 0.333f;

	public float HumanPyramidExtraDepenetration = 2f;

	public float HumanPyramidSlopeLength = 0.70711356f;

	public bool IS_CHARACTER_SPEED_OVERRIDDEN;

	[Range(0f, 1f)]
	public float OVERRIDDEN_CHARACTER_SPEED = 1f;

	public float PRONE_ALIGN_SPEED = 5f;

	public AnimationCurve SWAY_STRENGTH_PER_KG;

	public AnimationCurve DISPLACEMENT_STRENGTH_PER_KG;

	public Vector2 MOUSE_LOOK_HORIZONTAL_LIMIT = new Vector2(-40f, 40f);

	public Vector2 MOUSE_LOOK_VERTICAL_LIMIT = new Vector2(-50f, 20f);

	public float MOUSE_LOOK_LIMIT_IN_AIMING_COEF = 0.7f;

	[SerializeField]
	private EPlayerState[] NO_AIM_STATES;

	public float DELTA_SPEED = 0.05f;

	public Vector2 SWAY_DAMPING_NORMAL_DAMAGED;

	public Vector2 SWAY_FREQ_NORMAL_DAMAGED;

	public float DELTA_LEVEL = 0.1f;

	public float DIRECTION_LERP_SPEED = 10f;

	public float HANDS_TO_BODY_MAX_ANGLE = 45f;

	public float POSE_CHANGING_SPEED = 3f;

	public float TILT_CHANGING_SPEED = 10f;

	public float CHARACTER_SPEED_CHANGING_SPEED = 3f;

	public float TRANSFORM_ROTATION_LERP_SPEED = 50f;

	public AnimationCurve DIRECTION_CURVE;

	public AnimationCurve LIFT_VELOCITY_BY_SPEED;

	public AnimationCurve JUMP_DELAY_BY_SPEED;

	public float AIR_MIN_SPEED;

	public float AIR_LERP;

	public LayerMask MOVEMENT_MASK;

	public bool MED_EFFECT_USING_PANEL;

	[Header("Roll State Parameters")]
	public bool UseRibcage = true;

	public Vector3 RollCheckShift = new Vector3(0.65f, 0f, -0.2f);

	public Vector3 RollCheckSize = new Vector3(0.5f, 0.1f, 2.4f);

	public Vector3 ProneTiltCheckShift = new Vector3(0.65f, 0f, -0.2f);

	public Vector3 ProneTiltCheckSize = new Vector3(0.5f, 0.1f, 2.4f);

	public float RollCheckGroundCastLength = 0.3f;

	[Header("Ballistic")]
	public BallisticPreset[] ColliderPresets;

	public float DECAL_SHIFT = 0.1f;

	public float AIM_PROCEDURAL_INTENSITY = 0.75f;

	public float PROCEDURAL_INTENSITY_AT_ZERO_LEVEL_POSE = 0.75f;

	[Header("BallisticEffects")]
	public float DRAW_BLOOD_ON_WALLS_MAX_DISTANCE = 5f;

	public float DRAW_BLEEDING_MAX_DISTANCE = 10f;

	public LayerMask ENVIRONMENT_HIT_MASK = -1;

	public bool DRAW_DEFERRED_DECALS = true;

	[Header("Door")]
	public float DOOR_BREACH_THRESHOLD = 0.95f;

	[Header("Icons")]
	public StaticIcons StaticIcons;

	public Quaternion[] LEFT_HAND_QTS;

	public Quaternion[] RIGHT_HAND_QTS;

	[Header("Culling")]
	public float CULLING_PLAYER_SPHERE_RADIUS = 1f;

	public Vector3 CULLING_PLAYER_SPHERE_SHIFT = new Vector3(0f, 1f, 0f);

	public float CULLING_PLAYER_DISTANCE = 10000f;

	public float PATRONS_MANIPULATIONS_VISIBLE_DISTANCE = 10f;

	public float FLYING_SHELLS_VISIBLE_DISTANCE = 25f;

	[Header("-------- Simple character controller")]
	public float SIMPLE_CHARACTER_CONTROLLER_FIXED_DELTA_DISTANCE = 0.3f;

	public LayerMask TriggersCastLayerMask;

	public LayerMask TriggersCastLayerMaskForObservedPlayers;

	public LayerMask ServerTriggersCastLayerMask;

	public float HoboCastTimeBreakInterval = 0.5f;

	public float DelayToOpenContainer = 0.5f;

	public float GrenadeFullSyncDelay = 5f;

	public bool DisableDoorColliderOnInteraction;

	public float AnimatorCullDistance = 10f;

	public AnimationCurve StrafeInertionCurve;

	public float StrafeInertionCoefficient;

	[Header("Shells")]
	public ShellsSettings Shells = new ShellsSettings();

	[Tooltip("This option will enable high quallity physics simulation if loot volume (size.x * size.y * size.z) is less than value\nNote: Continuous Collision Detection is only supported for Rigidbodies with Sphere-, Capsule- or BoxColliders")]
	public float LootVolumeForHighQuallityPhysicsClient = 0.0001f;

	public float LootVolumeForHighQuallityPhysicsServer = 999f;

	public float ThrowLootMakeVisibleDelay = 0.15f;

	[Header("Corpse")]
	public float CorpseEnergyToSleep = 0.05f;

	public float CorpseMaxDepenetrationVelocity = 4f;

	public float CorpseApplyHardSyncMinDistanceSqr = 2500f;

	public float MaxSmoothPhysicsDeltaTime = 0.03f;

	private HashSet<EPlayerState> _E001;

	public bool DEBUG_CORPSE_PHYSICS;

	public static EFTHardSettings Instance
	{
		get
		{
			if (EFTHardSettings._E000 != null)
			{
				return EFTHardSettings._E000;
			}
			Debug.LogError(_ED3E._E000(30524));
			EFTHardSettings._E000 = _E3A2.Load<EFTHardSettings>(_ED3E._E000(30637));
			return EFTHardSettings._E000;
		}
	}

	public static async Task Load()
	{
		ResourceRequest resourceRequest = Resources.LoadAsync<EFTHardSettings>(_ED3E._E000(30637));
		await resourceRequest.Await();
		EFTHardSettings._E000 = (EFTHardSettings)resourceRequest.asset;
	}

	public static void Set(string key, string value)
	{
		FieldInfo field = typeof(EFTHardSettings).GetField(key);
		if (field != null)
		{
			object obj = null;
			if (field.FieldType == typeof(int))
			{
				obj = int.Parse(value);
			}
			else if (field.FieldType == typeof(float))
			{
				obj = float.Parse(value);
			}
			else if (field.FieldType == typeof(string))
			{
				obj = value;
			}
			else if (field.FieldType == typeof(bool))
			{
				obj = bool.Parse(value);
			}
			else
			{
				if (!(field.FieldType == typeof(double)))
				{
					Debug.LogErrorFormat(_ED3E._E000(30625), value, field.FieldType);
					return;
				}
				obj = double.Parse(value);
			}
			field.SetValue(Instance, obj);
		}
		else
		{
			Debug.LogErrorFormat(_ED3E._E000(30717), key);
		}
	}

	public bool CanAimInState(EPlayerState currentStateName)
	{
		if (_E001 == null)
		{
			_E001 = new HashSet<EPlayerState>(NO_AIM_STATES);
		}
		return !_E001.Contains(currentStateName);
	}

	public BallisticPreset GetPresetFromCollider(BallisticCollider collider)
	{
		return ColliderPresets[collider.FindPresetIndex()];
	}
}
