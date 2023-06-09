using AnimationSystem.RootMotionTable;
using UnityEngine;

namespace FastAnimatorSystem.TestAnimatorEnvironment;

public class TestAnimatorSkeleton : MonoBehaviour
{
	public Animator animator;

	public RuntimeAnimatorController originalAnimatorController;

	public TextAsset fastAnimatorJson;

	public RootMotionBlendTable blendTable;

	public CharacterClipsKeeper clipsKeeper;

	public Camera cam;

	public PlayableAnimator playableAnimator;

	public bool useFastAnimator;

	public bool usePlayableWithFast;

	public float speedMult = 1f;

	public Vector3 rotateAroundVec;

	public bool applyDeltaPosition = true;

	public bool isVisible = true;

	private Vector3 m__E000 = Vector3.zero;

	private IAnimator _E001;

	private int _E002 = -1;

	public IAnimator Animator => _E001;

	public Vector3 LastDelta
	{
		get
		{
			return this.m__E000;
		}
		set
		{
			this.m__E000 = value;
		}
	}

	public void Init()
	{
		if (playableAnimator != null)
		{
			playableAnimator.enabled = useFastAnimator && usePlayableWithFast;
		}
		if (useFastAnimator)
		{
			blendTable.LoadNodes();
			_E4EB fastAnimatorController = _E508.Deserialize(fastAnimatorJson.bytes);
			_E001 = _E563.CreateAnimator(fastAnimatorController, blendTable._loadedNodes, base.transform, playableAnimator);
			if (usePlayableWithFast)
			{
				_E502 obj = _E001 as _E502;
				playableAnimator.Init(_E001, obj.GetParametersCache(), blendTable, clipsKeeper, manualUpdate: true);
				playableAnimator.SetCuller(new _E500(playableAnimator));
				playableAnimator.Play();
				for (int i = 0; i < playableAnimator.initialLayerInfo.Length; i++)
				{
					obj.SetLayerWeight(i, playableAnimator.initialLayerInfo[i].weight);
				}
			}
		}
		else
		{
			_E001 = _E563.CreateAnimator(animator);
			_E001.runtimeAnimatorController = originalAnimatorController;
		}
	}

	public void Process(float dt)
	{
		if (useFastAnimator)
		{
			_E001.Update(dt);
			if (usePlayableWithFast)
			{
				playableAnimator.Process(isVisible, dt);
			}
			if (applyDeltaPosition)
			{
				Vector3 deltaPosition = _E001.deltaPosition;
				Vector3 vector = base.transform.rotation * deltaPosition;
				base.transform.position += vector;
				this.m__E000 = vector;
			}
		}
		else if (applyDeltaPosition)
		{
			Vector3 deltaPosition2 = _E001.deltaPosition;
			base.transform.position += deltaPosition2;
			this.m__E000 = deltaPosition2;
		}
	}

	private void _E000(string val)
	{
	}

	public AnimatorStateInfoWrapper GetCurrentState()
	{
		return _E001.GetCurrentAnimatorStateInfo(0);
	}

	public AnimatorStateInfoWrapper GetNextState()
	{
		return _E001.GetNextAnimatorStateInfo(0);
	}

	public Vector3 GetDeltaPosition()
	{
		return this.m__E000;
	}
}
