using System.Linq;
using UnityEngine;

namespace EFT.Hideout.ShootingRange;

public class PaperTargetControl : InteractiveShootingRange
{
	[SerializeField]
	[Space]
	private PaperTarget[] _paperTargets;

	[Space]
	[SerializeField]
	private AudioSource _replaceTargetsAudio;

	public override _EC3F InteractionStates(HideoutPlayerOwner owner)
	{
		_EC3F obj = base.InteractionStates(owner);
		obj.Actions.Add(new _EC3E
		{
			Name = _ED3E._E000(164649),
			Action = _E000
		});
		obj.SelectAction(obj.Actions[0]);
		return obj;
	}

	private void _E000()
	{
		_replaceTargetsAudio.Play();
		PaperTarget[] paperTargets = _paperTargets;
		for (int i = 0; i < paperTargets.Length; i++)
		{
			paperTargets[i].Replace();
		}
	}

	private void _E001()
	{
		_paperTargets = (from v in Resources.FindObjectsOfTypeAll<PaperTarget>()
			where v.gameObject.activeInHierarchy
			select v).ToArray();
	}
}
