using System;
using EFT.InventoryLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class CharacteristicPanel : CompactCharacteristicPanel
{
	private const float _E11C = 0.5f;

	private const float _E11D = 0.2f;

	private const float _E11E = 0.6f;

	private const float _E11F = 0.40000004f;

	[SerializeField]
	protected Image Border;

	[SerializeField]
	protected Image Current;

	[SerializeField]
	protected Image Delta;

	[SerializeField]
	protected Image CompareDelta;

	[SerializeField]
	protected Color CurrentColor = new Color32(92, 98, 105, byte.MaxValue);

	[SerializeField]
	protected Color LowColor = new Color32(92, 98, 105, byte.MaxValue);

	[SerializeField]
	protected Color SubColor = new Color32(92, 0, 0, byte.MaxValue);

	[SerializeField]
	protected Color AddColor = new Color32(0, 0, 133, byte.MaxValue);

	[SerializeField]
	protected Sprite AddSprite;

	[SerializeField]
	protected Sprite SubSprite;

	private Color _E120;

	private float _E121;

	private float _E122;

	public void Show(Item item)
	{
	}

	public override void SetValues()
	{
		base.SetValues();
		if (Current == null || Delta == null || !(ItemAttribute is _EB11 obj))
		{
			return;
		}
		_EB11 obj2 = CompareItemAttribute as _EB11;
		float num = (Examined ? obj.Base() : 0f);
		float num2 = (Examined ? (num * obj.Delta()) : 0f);
		float num3 = (Examined ? (num + num2) : 0f);
		float num4 = (Examined ? obj2.Base() : 0f);
		float num5 = (Examined ? (num4 * obj2.Delta()) : 0f);
		float num6 = (Examined ? (num4 + num5) : 0f);
		float num7 = num6 - num3;
		float num8 = Mathf.InverseLerp(obj.Range.x, obj.Range.y, num3);
		float num9 = Mathf.InverseLerp(obj.Range.x, obj.Range.y, num);
		float num10 = Mathf.Min(num8, num9);
		float x = Mathf.Max(num8, num9);
		float num11 = Mathf.InverseLerp(obj.Range.x, obj.Range.y, num6);
		float b = Mathf.InverseLerp(obj.Range.x, obj.Range.y, num4);
		float b2 = Mathf.Min(num11, b);
		float num12 = Mathf.Abs(num11 - num8);
		Delta.sprite = AddSprite;
		if ((num2 < 0f && !obj.RangeIsInverted) || (num2 > 0f && obj.RangeIsInverted))
		{
			Delta.sprite = SubSprite;
		}
		Delta.color = ((!ItemAttribute.LessIsGood) ? ((num2 > 0f) ? AddColor : SubColor) : ((num2 > 0f) ? SubColor : AddColor));
		if (!num7.IsZero() && CompareDelta != null)
		{
			CompareDelta.gameObject.SetActive(value: true);
			CompareDelta.sprite = AddSprite;
			_E120 = ((!ItemAttribute.LessIsGood) ? ((num7 > 0f) ? AddColor : SubColor) : ((num7 > 0f) ? SubColor : AddColor));
			_E120.a = _E121;
			CompareDelta.color = _E120;
			if (num7.IsZero())
			{
				Current.rectTransform.anchorMax = new Vector2(Mathf.Min(num9, b), 0.5f);
				Delta.rectTransform.anchorMin = new Vector2(Mathf.Min(num9, b), 0.5f);
				Delta.rectTransform.anchorMax = new Vector2(Mathf.Min(num9, b), 0.5f);
				CompareDelta.rectTransform.anchorMin = new Vector2(Mathf.Min(num9, b), 0.5f);
				CompareDelta.rectTransform.anchorMax = new Vector2(Mathf.Max(num9, b), 0.5f);
			}
			else if (num7 > 0f)
			{
				Current.rectTransform.anchorMax = new Vector2(Mathf.Min(num10, b2), 0.5f);
				Delta.rectTransform.anchorMin = new Vector2(Mathf.Min(num10, b2), 0.5f);
				Delta.rectTransform.anchorMax = new Vector2(Mathf.Min(num8, num11), 0.5f);
				CompareDelta.rectTransform.anchorMin = new Vector2(Mathf.Min(num8, num11), 0.5f);
				CompareDelta.rectTransform.anchorMax = new Vector2(Mathf.Max(num8, num11), 0.5f);
			}
			else
			{
				Current.rectTransform.anchorMax = new Vector2(Mathf.Min(num10, b2), 0.5f);
				if (num6 < num)
				{
					CompareDelta.rectTransform.anchorMin = new Vector2(Mathf.Min(num10, b2), 0.5f);
					CompareDelta.rectTransform.anchorMax = new Vector2(Mathf.Min(num10, b2) + num12, 0.5f);
					Delta.rectTransform.anchorMin = new Vector2(Mathf.Min(num10, b2) + num12, 0.5f);
					Delta.rectTransform.anchorMax = new Vector2(Mathf.Max(num8, num11), 0.5f);
				}
				else
				{
					Delta.rectTransform.anchorMin = new Vector2(Mathf.Min(num10, b2), 0.5f);
					Delta.rectTransform.anchorMax = new Vector2(Mathf.Max(num8, num11) - num12, 0.5f);
					CompareDelta.rectTransform.anchorMin = new Vector2(Mathf.Max(num8, num11) - num12, 0.5f);
					CompareDelta.rectTransform.anchorMax = new Vector2(Mathf.Max(num8, num11), 0.5f);
				}
			}
		}
		else
		{
			CompareDelta?.gameObject.SetActive(value: false);
			Current.rectTransform.anchorMax = new Vector2(num10, 0.5f);
			Delta.rectTransform.anchorMin = new Vector2(num10, 0.5f);
			Delta.rectTransform.anchorMax = new Vector2(x, 0.5f);
		}
		float num13 = num5 - num2;
		if (Math.Abs(num13) > Mathf.Epsilon)
		{
			TextMeshProUGUI valueText = ValueText;
			valueText.text = valueText.text + _ED3E._E000(27312) + ((num13 > 0f) ? _ED3E._E000(29692) : string.Empty) + Mathf.RoundToInt(num13) + _ED3E._E000(27308);
		}
	}

	private void Update()
	{
		_E122 += Time.deltaTime / 0.5f * 0.40000004f;
		_E121 = 0.2f + Mathf.PingPong(_E122, 0.40000004f);
		_E120.a = _E121;
		if (CompareDelta != null)
		{
			CompareDelta.color = _E120;
		}
	}
}
