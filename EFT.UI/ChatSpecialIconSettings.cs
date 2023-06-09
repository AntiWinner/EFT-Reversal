using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.UI;

[CreateAssetMenu(fileName = "ChatSpecialIconSettings", menuName = "EFT/Create ChatSpecialIconSettings object", order = 0)]
public sealed class ChatSpecialIconSettings : ScriptableObject
{
	[Serializable]
	public class IconsData
	{
		public string Name = _ED3E._E000(250640);

		public EMemberCategory Category;

		public Sprite IconSprite;

		public Color IconColor = Color.white;
	}

	[CompilerGenerated]
	private sealed class _E000
	{
		public EMemberCategory targetCategory;

		internal bool _E000(IconsData t)
		{
			return t.Category == targetCategory;
		}
	}

	private static readonly EMemberCategory[] m__E000 = (EMemberCategory[])Enum.GetValues(typeof(EMemberCategory));

	public EMemberCategory[] Priority;

	public IconsData[] IconsSettings;

	private readonly SortedList<int, EMemberCategory> _E001 = new SortedList<int, EMemberCategory>();

	public IconsData GetDataByMemberCategory(EMemberCategory category)
	{
		_E001.Clear();
		EMemberCategory[] array = ChatSpecialIconSettings.m__E000;
		foreach (EMemberCategory value in array)
		{
			if (category.Is(value))
			{
				int num = Array.IndexOf(Priority, value);
				if (num >= 0)
				{
					_E001.Add(num, value);
				}
			}
		}
		EMemberCategory targetCategory = ((_E001.Count > 0) ? _E001.Values[0] : category);
		return IconsSettings.SingleOrDefault((IconsData t) => t.Category == targetCategory);
	}
}
