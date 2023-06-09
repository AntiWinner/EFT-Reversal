using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EFT.UI;

public sealed class SpecifiedContextButtons : SerializedMonoBehaviour
{
	public sealed class _E000
	{
		public SimpleContextMenuButton Button;

		public RectTransform Container;
	}

	[SerializeField]
	private List<Dictionary<Enum, _E000>> _buttons;

	private Dictionary<Enum, _E000> m__E000;

	private Dictionary<Enum, _E000> _E001 => this.m__E000 ?? (this.m__E000 = _buttons?.SelectMany((Dictionary<Enum, _E000> dict) => dict).ToDictionary((KeyValuePair<Enum, _E000> pair) => pair.Key, (KeyValuePair<Enum, _E000> pair) => pair.Value));

	public (SimpleContextMenuButton template, RectTransform container)? GetButton(Enum key)
	{
		if (_E001 == null || !_E001.TryGetValue(key, out var value))
		{
			return null;
		}
		return (value?.Button, value?.Container);
	}
}
