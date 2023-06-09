using System.Collections.Generic;
using UnityEngine;

namespace Koenigz.PerfectCulling;

[CreateAssetMenu]
public class PerfectCullingColorTable : ScriptableObject
{
	[SerializeField]
	[HideInInspector]
	private Color32[] m_colors;

	public static PerfectCullingColorTable Instance => PerfectCullingResourcesLocator.Instance.ColorTable;

	public Color32[] Colors => m_colors;

	[ContextMenu("Generate")]
	private void _E000()
	{
		List<Color32> list = new List<Color32>(65535);
		for (int i = 0; i <= 255; i++)
		{
			for (int j = 0; j <= 255; j++)
			{
				Color32 color = new Color32(0, (byte)i, (byte)j, byte.MaxValue);
				if (!(color == _E49D.ClearColor))
				{
					list.Add(color);
				}
			}
		}
		int num = list.Count;
		while (num > 1)
		{
			num--;
			int num2 = Random.Range(0, num + 1);
			int index = num2;
			List<Color32> list2 = list;
			int index2 = num;
			Color32 color2 = list[num];
			Color32 color3 = list[num2];
			Color32 color5 = (list[index] = color2);
			color5 = (list2[index2] = color3);
		}
		m_colors = list.ToArray();
	}
}
