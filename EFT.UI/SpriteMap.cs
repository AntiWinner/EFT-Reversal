using System;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.UI;

[Serializable]
public class SpriteMap
{
	[Serializable]
	public class Data
	{
		public string Id;

		public Sprite Sprite;
	}

	[CompilerGenerated]
	private sealed class _E000
	{
		public string key;

		internal bool _E000(Data item)
		{
			return item.Id == key;
		}
	}

	[SerializeField]
	private Data[] _sprites;

	[CanBeNull]
	public Sprite this[string key] => (from item in _sprites
		where item.Id == key
		select item.Sprite).FirstOrDefault();

	[CanBeNull]
	public Sprite this[Enum key] => this[key.ToString()];
}
