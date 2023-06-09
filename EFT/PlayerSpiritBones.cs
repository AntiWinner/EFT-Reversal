using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EFT.MovingPlatforms;
using UnityEngine;

namespace EFT;

public class PlayerSpiritBones : MonoBehaviour, MovingPlatform._E000
{
	public enum PoseType : byte
	{
		None,
		Stand,
		Crouch,
		Prone,
		TiltLeft,
		TiltRight,
		SideStepLeft,
		SideStepRight,
		ProneSideStepLeft,
		ProneSideStepRight,
		ProneTiltLeft,
		ProneTiltRight,
		CrouchTiltLeft,
		CrouchTiltRight,
		CrouchSideStepLeft,
		CrouchSideStepRight
	}

	[Serializable]
	public struct BonePosition
	{
		public PlayerBoneType type;

		public Vector3 position;
	}

	[Serializable]
	public struct PoseSkeleton
	{
		public PoseType type;

		public BonePosition[] bones;
	}

	[CompilerGenerated]
	private sealed class _E000
	{
		public PlayerSpiritBones _003C_003E4__this;

		public PlayerBoneType boneType;

		internal Vector3 _E000()
		{
			return _003C_003E4__this._E001(boneType);
		}

		internal Quaternion _E001()
		{
			return _003C_003E4__this._E002(boneType);
		}

		internal Vector3 _E002()
		{
			return _003C_003E4__this._E001(boneType);
		}

		internal Quaternion _E003()
		{
			return _003C_003E4__this.transform.rotation;
		}

		internal Vector3 _E004()
		{
			return _003C_003E4__this.transform.forward;
		}

		internal Vector3 _E005()
		{
			return _003C_003E4__this.transform.up;
		}

		internal Vector3 _E006()
		{
			return _003C_003E4__this.transform.right;
		}

		internal Vector3 _E007()
		{
			return _003C_003E4__this.transform.eulerAngles;
		}

		internal Vector3 _E008(Vector3 value)
		{
			return _003C_003E4__this.transform.TransformPoint(value);
		}

		internal Vector3 _E009(Vector3 value)
		{
			return _003C_003E4__this.transform.InverseTransformPoint(value);
		}

		internal Vector3 _E00A(Vector3 value)
		{
			return _003C_003E4__this.transform.TransformVector(value);
		}

		internal Vector3 _E00B(Vector3 value)
		{
			return _003C_003E4__this.transform.InverseTransformVector(value);
		}
	}

	private _E33A m__E000;

	public List<PlayerBoneType> SimpleBones = new List<PlayerBoneType>();

	[SerializeField]
	private PoseType _currentPose;

	[SerializeField]
	private bool _isAiming;

	[SerializeField]
	private Vector3 aimingOffsetLocal;

	[SerializeField]
	private Quaternion aimingRotationLocal;

	[SerializeField]
	private PoseSkeleton[] _posesConfig;

	private Dictionary<PlayerBoneType, Vector3>[] m__E001;

	private PlayerSpiritBase m__E002;

	private Quaternion m__E003;

	private int m__E004;

	private readonly Dictionary<PlayerBoneType, BifacialTransform> _E005 = _E3A5<PlayerBoneType>.GetDictWith<BifacialTransform>();

	public Vector3 Center => this.m__E000.center;

	public float Height => this.m__E000.height;

	public float Tilt => this.m__E002._player.MovementContext.Tilt;

	public float Step => this.m__E002._player.MovementContext.Step;

	public float Radius => this.m__E000.radius;

	public float SkinWidth => this.m__E000.skinWidth;

	public PoseType Pose => _currentPose;

	public PoseType CurrentPose => _currentPose;

	[ContextMenu("AWAKE")]
	private void Awake()
	{
		_currentPose = PoseType.None;
		_E000();
	}

	private void _E000()
	{
		this.m__E001 = new Dictionary<PlayerBoneType, Vector3>[_E3A5<PlayerBoneType>.Count];
		PoseSkeleton[] posesConfig = _posesConfig;
		for (int i = 0; i < posesConfig.Length; i++)
		{
			PoseSkeleton poseSkeleton = posesConfig[i];
			Dictionary<PlayerBoneType, Vector3> dictWith = _E3A5<PlayerBoneType>.GetDictWith<Vector3>();
			BonePosition[] bones = poseSkeleton.bones;
			for (int j = 0; j < bones.Length; j++)
			{
				BonePosition bonePosition = bones[j];
				dictWith.Add(bonePosition.type, bonePosition.position);
			}
			this.m__E001[(uint)poseSkeleton.type] = dictWith;
		}
	}

	public void Init(PlayerSpiritBase spirit)
	{
		this.m__E000 = spirit.CharacterController;
		this.m__E002 = spirit;
	}

	public void SetupSimpleBones(Dictionary<PlayerBoneType, BifacialTransform> originalBones)
	{
		foreach (PlayerBoneType simpleBone in SimpleBones)
		{
			SetupBone(originalBones[simpleBone], simpleBone);
		}
	}

	public void SetupBone(BifacialTransform bone, PlayerBoneType boneType)
	{
		bone.SetImitators(() => _E001(boneType), delegate
		{
		}, () => _E002(boneType), delegate
		{
		}, delegate
		{
		}, () => _E001(boneType), delegate
		{
		}, () => base.transform.rotation, delegate
		{
		}, () => base.transform.forward, () => base.transform.up, () => base.transform.right, () => base.transform.eulerAngles, (Vector3 value) => base.transform.TransformPoint(value), (Vector3 value) => base.transform.InverseTransformPoint(value), (Vector3 value) => base.transform.TransformVector(value), (Vector3 value) => base.transform.InverseTransformVector(value), delegate
		{
		});
		_E005[boneType] = bone;
	}

	private Vector3 _E001(PlayerBoneType boneType)
	{
		int pose = (int)Pose;
		Vector3 relativePoint = this.m__E001[pose][boneType];
		_E003(boneType, ref relativePoint);
		Vector3 imitatorPositionLocal = GetImitatorPositionLocal(relativePoint);
		if (_isAiming && boneType == PlayerBoneType.Head)
		{
			imitatorPositionLocal += aimingOffsetLocal;
		}
		Vector3 relativePoint2 = base.transform.TransformPoint(imitatorPositionLocal);
		_E004(ref relativePoint2);
		return relativePoint2;
	}

	private Quaternion _E002(PlayerBoneType boneType)
	{
		if (_isAiming && boneType == PlayerBoneType.Head)
		{
			return base.transform.rotation * aimingRotationLocal;
		}
		return base.transform.rotation;
	}

	public Vector3 GetImitatorPositionLocal(Vector3 relativePoint)
	{
		Vector3 center = Center;
		float num = Height + SkinWidth;
		float radius = Radius;
		return new Vector3(center.x + radius * relativePoint.x, center.y + num * 0.5f * relativePoint.y, center.z + radius * relativePoint.z);
	}

	private void _E003(PlayerBoneType boneType, ref Vector3 relativePoint)
	{
		if (!Pose.IsTilt() && Tilt != 0f)
		{
			PoseType poseType;
			switch (Pose)
			{
			default:
				return;
			case PoseType.Stand:
				poseType = ((Tilt > 0f) ? PoseType.TiltRight : PoseType.TiltLeft);
				break;
			case PoseType.Crouch:
				poseType = ((Tilt > 0f) ? PoseType.CrouchTiltRight : PoseType.CrouchTiltLeft);
				break;
			case PoseType.Prone:
				poseType = ((Tilt > 0f) ? PoseType.ProneTiltRight : PoseType.ProneTiltLeft);
				break;
			}
			Vector3 b = this.m__E001[(uint)poseType][boneType];
			relativePoint = Vector3.Lerp(relativePoint, b, Tilt / (poseType.IsTiltRight() ? 5f : (-5f)));
		}
	}

	private void _E004(ref Vector3 relativePoint)
	{
		if (_currentPose.IsProne())
		{
			Vector3 vector = relativePoint - base.transform.position;
			Quaternion quaternion;
			if (Time.frameCount == this.m__E004)
			{
				quaternion = this.m__E003;
			}
			else
			{
				quaternion = (this.m__E003 = Quaternion.FromToRotation(Vector3.up, this.m__E002._player.MovementContext.PlayerSurfaceUpAlignNormal));
				this.m__E004 = Time.frameCount;
			}
			relativePoint = quaternion * vector + base.transform.position;
		}
	}

	public void SetPose(PoseType poseType)
	{
		_currentPose = poseType;
	}

	public void SetAimingState(bool aiming)
	{
		_isAiming = aiming;
	}

	public BifacialTransform GetBone(PlayerBoneType boneType)
	{
		return _E005[boneType];
	}

	public void SetPoseConfig(PoseSkeleton poseSkeleton)
	{
		for (int i = 0; i < _posesConfig.Length; i++)
		{
			if (_posesConfig[i].type == poseSkeleton.type)
			{
				_posesConfig[i] = poseSkeleton;
				return;
			}
		}
		Array.Resize(ref _posesConfig, _posesConfig.Length + 1);
		_posesConfig[_posesConfig.Length - 1] = poseSkeleton;
	}

	public Bounds GetLocalBounds()
	{
		Bounds result = default(Bounds);
		for (int i = 0; i < SimpleBones.Count; i++)
		{
			PlayerBoneType key = SimpleBones[i];
			BifacialTransform bifacialTransform = _E005[key];
			Vector3 vector = base.transform.InverseTransformPoint(bifacialTransform.position);
			if (i == 0)
			{
				result = new Bounds(vector, Vector3.zero);
			}
			else
			{
				result.Encapsulate(vector);
			}
		}
		return result;
	}

	public void Board(MovingPlatform platform)
	{
		this.m__E002._player.Board(platform);
	}

	public void GetOff(MovingPlatform platform)
	{
		this.m__E002._player.GetOff(platform);
	}
}
