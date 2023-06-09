using System;
using Comfort.Common;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.HandBook;

[Serializable]
public class HandbookData : BaseHandbookData
{
	public string Icon;

	public float Price;

	public string Color;

	public int Order;

	public string Name;

	public Item Item;

	public ENodeType Type;

	public bool FromBuild;

	private Color _backgroundColor = UnityEngine.Color.clear;

	public Color BackgroundColor
	{
		get
		{
			if (_backgroundColor != UnityEngine.Color.clear)
			{
				return _backgroundColor;
			}
			if (string.IsNullOrEmpty(Color))
			{
				return UnityEngine.Color.clear;
			}
			ColorUtility.TryParseHtmlString(Color, out var color);
			_backgroundColor = color;
			return color;
		}
	}

	public HandbookData CloneAsCategory()
	{
		return _E000(ENodeType.Category);
	}

	public HandbookData Clone()
	{
		return _E000(Type);
	}

	private HandbookData _E000(ENodeType type)
	{
		return new HandbookData
		{
			Id = Id,
			ParentId = ParentId,
			Icon = Icon,
			Price = Price,
			Color = Color,
			Order = Order,
			Type = type,
			Name = (Name ?? ((Type == ENodeType.Category) ? Id : (Id + _ED3E._E000(70087)))),
			Item = ((type == ENodeType.Item) ? (Item ?? Singleton<_E63B>.Instance.GetPresetItem(Id)) : null),
			FromBuild = FromBuild
		};
	}

	public override string ToString()
	{
		return _ED3E._E000(197280) + Id + _ED3E._E000(197341) + ParentId;
	}
}
