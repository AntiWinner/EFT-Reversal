using System;
using UnityEngine;

namespace EFT.UI;

[Serializable]
public class ColorMap
{
	[Serializable]
	public class Data
	{
		public string Id;

		public Color Item;

		public Data(string id, Color color)
		{
			Id = id;
			Item = color;
		}
	}

	[SerializeField]
	private Data[] _items;

	public Color this[string key]
	{
		get
		{
			Data[] items = _items;
			foreach (Data data in items)
			{
				if (data.Id == key)
				{
					return data.Item;
				}
			}
			return Color.black;
		}
	}

	public Color this[Enum key] => this[key.ToString()];

	public ColorMap(Data[] items)
	{
		_items = items;
	}
}
