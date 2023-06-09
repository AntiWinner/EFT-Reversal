using System.Collections.Generic;
using AnimationSystem.RootMotionTable;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace FastAnimatorSystem;

public sealed class PlayableAnimator : MonoBehaviour, _E4FA
{
	public Animator outputAnimator;

	public InitialLayerInfo[] initialLayerInfo;

	private bool m__E000;

	private PlayableGraph _E001;

	private List<_E4FE> _E002;

	private List<AnimationClip>[] _E003;

	private AnimationPlayableOutput _E004;

	private AnimationLayerMixerPlayable _E005;

	private IAnimator _E006;

	private _E4FB _E007;

	private bool _E008;

	public bool Initialized => this.m__E000;

	public bool ManualUpdateMode => _E008;

	public IAnimator FastAnimator => _E006;

	public PlayableGraph Graph => _E001;

	public int LayersCount => _E006.layerCount;

	public void Init(IAnimator animator, _E56C parametersCache, RootMotionBlendTable rootMotionBlendTable, CharacterClipsKeeper clipsKeeper, bool manualUpdate)
	{
		this.m__E000 = true;
		_E006 = animator;
		_E001 = PlayableGraph.Create();
		_E004 = AnimationPlayableOutput.Create(_E001, _ED3E._E000(127232), GetComponent<Animator>());
		_E003 = new List<AnimationClip>[LayersCount];
		_E002 = new List<_E4FE>();
		_E008 = manualUpdate;
		_E001.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
		for (int i = 0; i < _E006.layerCount; i++)
		{
			_E003[i] = new List<AnimationClip>(clipsKeeper.Clips[i].clips.Length);
			for (int j = 0; j < clipsKeeper.Clips[i].clips.Length; j++)
			{
				_E003[i].Add(clipsKeeper.Clips[i].clips[j].clip);
			}
		}
		for (int k = 0; k < _E006.layerCount; k++)
		{
			_E002.Add(new _E4FE(k, this, parametersCache, rootMotionBlendTable, clipsKeeper, _E003[k]));
		}
		_E000();
	}

	public void Play()
	{
		if (!_E008)
		{
			_E001.Play();
		}
	}

	public void Stop()
	{
		if (!_E008)
		{
			_E001.Stop();
		}
	}

	public void Process(bool isVisible, float dt)
	{
		if (!_E007.ProcessAnimator(isVisible))
		{
			return;
		}
		for (int i = 0; i < _E002.Count; i++)
		{
			if (_E005.GetInputWeight(i) > Mathf.Epsilon)
			{
				_E002[i].Process(dt);
			}
		}
		if (_E008)
		{
			_E001.Evaluate(dt);
		}
		if (_E007.IsImportantParameterChanged && !isVisible)
		{
			if (!_E008)
			{
				_E001.Evaluate(dt);
			}
			_E007.IsImportantParameterChanged = false;
		}
	}

	public void SetLayerWeight(int layerIndex, float weight)
	{
		_E005.SetInputWeight(layerIndex, weight);
	}

	public float GetLayerWeight(int layerIndex)
	{
		if (layerIndex < 0 || layerIndex >= _E002.Count)
		{
			return -1f;
		}
		return _E005.GetInputWeight(layerIndex);
	}

	public void RaiseImmediateTransitionHappened(int layerIndex)
	{
		_E002[layerIndex].RaiseImmediateTransitionHappened();
	}

	public void SetCuller(_E4FB culler)
	{
		_E007 = culler;
	}

	public _E4FD GetLayerProcessor(int layerIndex)
	{
		if (layerIndex >= 0 && layerIndex < _E002.Count)
		{
			return _E002[layerIndex];
		}
		return null;
	}

	public AnimationClip GetClipByIndex(int layerIndex, int clipIndex)
	{
		if (layerIndex >= 0 && layerIndex < _E003.Length && clipIndex >= 0 && clipIndex < _E003[layerIndex].Count)
		{
			return _E003[layerIndex][clipIndex];
		}
		return null;
	}

	private void _E000()
	{
		_E005 = AnimationLayerMixerPlayable.Create(_E001, _E002.Count);
		for (uint num = 0u; num < initialLayerInfo.Length; num++)
		{
			if (initialLayerInfo[num].avatarMask != null)
			{
				_E005.SetLayerMaskFromAvatarMask(num, initialLayerInfo[num].avatarMask);
			}
			_E005.SetLayerAdditive(num, initialLayerInfo[num].isAdditive);
		}
		for (int i = 0; i < _E002.Count; i++)
		{
			_E001.Connect(_E002[i].Mixer, 0, _E005, _E002[i].LayerIndex);
			_E005.SetInputWeight(i, initialLayerInfo[i].weight);
		}
		_E004.SetSourcePlayable(_E005);
	}

	private void OnAnimatorMove()
	{
	}

	private void OnDestroy()
	{
		_E007 = null;
		if (this.m__E000)
		{
			_E001.Destroy();
		}
	}
}
