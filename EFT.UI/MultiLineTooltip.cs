using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace EFT.UI;

public sealed class MultiLineTooltip : Tooltip
{
	[SerializeField]
	private MultiLineRow _multiLineRow;

	[SerializeField]
	private TextMeshProUGUI _header;

	private readonly List<MultiLineRow> _E21D = new List<MultiLineRow>();

	public void Show(_EC80 multiLineInfo)
	{
		if (multiLineInfo.Header != null || multiLineInfo.Lines != null)
		{
			_EC7F[] lines = multiLineInfo.Lines;
			_E000(multiLineInfo.Header.Localized());
			_E001(lines.Length, _E21D);
			_E002(lines, _E21D);
			Show();
		}
	}

	private void _E000(string header)
	{
		_header.text = header;
	}

	private void _E001(int rowsNeededCount, List<MultiLineRow> rowList)
	{
		int num = rowsNeededCount - rowList.Count;
		for (int i = 0; i < num; i++)
		{
			MultiLineRow item = Object.Instantiate(_multiLineRow, _multiLineRow.transform.parent, worldPositionStays: false);
			rowList.Add(item);
		}
		rowList.ForEach(delegate(MultiLineRow s)
		{
			s.HideGameObject();
		});
	}

	private void _E002(_EC7F[] singleLineInfo, List<MultiLineRow> rowList)
	{
		for (int i = 0; i < singleLineInfo.Length; i++)
		{
			rowList[i].Show(singleLineInfo[i]);
		}
	}
}
