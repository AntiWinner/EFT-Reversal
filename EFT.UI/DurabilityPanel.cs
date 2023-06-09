using System;
using UnityEngine;

namespace EFT.UI;

public sealed class DurabilityPanel : CharacteristicPanel
{
	public override void SetValues()
	{
		if (ItemAttribute is _EB11 obj)
		{
			if (ValueText == null)
			{
				Debug.LogError(_ED3E._E000(247030) + base.gameObject.name);
				return;
			}
			float num = obj.Base();
			float num2 = obj.Delta();
			Vector2 range = obj.Range;
			ValueText.text = string.Format(_ED3E._E000(247069), Math.Round(num, 1), (num + num2 < 10f) ? _ED3E._E000(29694) : _ED3E._E000(60648), Math.Round(num + num2, 1), PeakDurability);
			float num3 = num / range.y;
			Color color = ((num3 > 0.15f) ? CurrentColor : LowColor);
			Current.color = color;
			Border.color = color;
			float x = Mathf.InverseLerp(range.x, range.y, num + num2);
			Current.rectTransform.anchorMax = new Vector2(num3, 0.5f);
			Delta.rectTransform.anchorMax = new Vector2(x, 0.5f);
			CompareDelta?.gameObject.SetActive(value: false);
		}
	}
}
