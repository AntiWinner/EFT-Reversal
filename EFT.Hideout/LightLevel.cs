using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EFT.Hideout;

public sealed class LightLevel : SerializedMonoBehaviour
{
	[SerializeField]
	private List<_EBFD> _lightSwitchers;

	public bool Enabled
	{
		set
		{
			foreach (_EBFD lightSwitcher in _lightSwitchers)
			{
				lightSwitcher.Enabled = value;
			}
		}
	}
}
