using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFT.NPC;

public class NPCFootStepsSoundPlayer : MonoBehaviour
{
	[SerializeField]
	private Animator _npcAnimator;

	[SerializeField]
	private AudioSource _stepAudioSource;

	[SerializeField]
	private List<AudioClip> _footstepClips = new List<AudioClip>();

	private bool m__E000;

	private Coroutine m__E001;

	private void Awake()
	{
		this.m__E001 = StartCoroutine(_E001());
	}

	private void OnDestroy()
	{
		if (this.m__E001 != null)
		{
			StopCoroutine(this.m__E001);
			this.m__E001 = null;
		}
	}

	private void _E000()
	{
		_stepAudioSource.clip = _footstepClips[Random.Range(0, _footstepClips.Count)];
		_stepAudioSource.Play();
	}

	private IEnumerator _E001()
	{
		this.m__E000 = false;
		while (true)
		{
			if ((double)_npcAnimator.GetFloat(_ED3E._E000(130814)) > 0.85)
			{
				if (!this.m__E000)
				{
					this.m__E000 = true;
					_E000();
				}
			}
			else
			{
				this.m__E000 = false;
			}
			yield return null;
		}
	}
}
