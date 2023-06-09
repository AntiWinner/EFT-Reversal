using System;
using EFT;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerSpiritBonesCreator : MonoBehaviour
{
	[Serializable]
	public class PoseConfig
	{
		public bool Collect = true;

		public PlayerSpiritBones.PoseType PoseType;

		public string AnimatorStateName;

		public bool ProneFlag;

		[Range(0f, 1f)]
		public float LevelParam = 1f;

		[Range(-5f, 5f)]
		public float TiltParam;

		[Range(-1f, 1f)]
		public int StepParam;
	}

	public GameObject PlayerSuperior;

	public PlayerSpirit PlayerSpirit;

	public string PlayerSpiritPath;

	[SerializeField]
	private Animator _playerAnimator;

	public PoseConfig[] PoseConfigs;
}
