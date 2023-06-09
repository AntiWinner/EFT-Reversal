using System;
using System.Runtime.CompilerServices;
using System.Threading;
using EFT.InventoryLogic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public class CompactCharacteristicPanel : UIElement
{
	[SerializeField]
	protected Image Icon;

	[SerializeField]
	protected TextMeshProUGUI NameText;

	[SerializeField]
	protected TextMeshProUGUI ValueText;

	[SerializeField]
	private Color _increasingColor = new Color(84f, 193f, 255f, 255f);

	[SerializeField]
	private Color _decreasingColor = new Color(196f, 0f, 0f, 255f);

	protected SimpleTooltip Tooltip;

	protected _EB10 ItemAttribute;

	protected _EB10 CompareItemAttribute;

	protected int PeakDurability;

	protected bool Examined;

	[CompilerGenerated]
	private float _E124;

	[CompilerGenerated]
	private EventHandler _E125;

	private string _E126;

	private _EC76 _E127 = new _EC76();

	public float TextWidth
	{
		[CompilerGenerated]
		get
		{
			return _E124;
		}
		[CompilerGenerated]
		private set
		{
			_E124 = value;
		}
	}

	public event EventHandler OnTextWidthCalculated
	{
		[CompilerGenerated]
		add
		{
			EventHandler eventHandler = _E125;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _E125, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler eventHandler = _E125;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _E125, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	protected virtual void Awake()
	{
		HoverTrigger orAddComponent = this.GetOrAddComponent<HoverTrigger>();
		orAddComponent.OnHoverStart += delegate
		{
			if (!string.IsNullOrEmpty(_E126))
			{
				Tooltip.Show(_E126);
			}
		};
		orAddComponent.OnHoverEnd += delegate
		{
			if (Tooltip != null && Tooltip.isActiveAndEnabled)
			{
				Tooltip.Close();
			}
		};
	}

	private void OnDisable()
	{
		if (Tooltip != null && Tooltip.isActiveAndEnabled)
		{
			Tooltip.Close();
		}
	}

	public void Show(_EB10 itemAttribute, SimpleTooltip tooltip, bool examined = true, int peakDurability = 100)
	{
		UI.Dispose();
		ShowGameObject();
		ItemAttribute = itemAttribute;
		CompareItemAttribute = ItemAttribute;
		_E126 = itemAttribute.FullStringValue();
		PeakDurability = peakDurability;
		Examined = examined;
		Tooltip = tooltip;
		if (itemAttribute is _E9D2 && !Examined)
		{
			HideGameObject();
			return;
		}
		if (NameText != null)
		{
			NameText.text = ItemAttribute.DisplayName;
		}
		else
		{
			Debug.LogError(_ED3E._E000(252228) + base.gameObject.name);
		}
		Sprite attributeIcon = EFTHardSettings.Instance.StaticIcons.GetAttributeIcon(ItemAttribute.Id);
		Icon.sprite = attributeIcon;
		Icon.gameObject.SetActive(attributeIcon != null);
		Icon.SetNativeSize();
		ItemAttribute.OnUpdate += SetValues;
		UI.AddDisposable(delegate
		{
			ItemAttribute.OnUpdate -= SetValues;
		});
		UI.AddDisposable(_E127);
		if (!string.IsNullOrEmpty(ItemAttribute.Tooltip()) && Examined)
		{
			ValueText.GetOrAddComponent<HoverTooltipArea>().SetMessageText(ItemAttribute.Tooltip);
		}
		if (ItemAttribute.Enhancement != null && Examined)
		{
			ValueText.color = new Color32(84, 193, byte.MaxValue, byte.MaxValue);
		}
		this.WaitOneFrame(_E000);
	}

	public virtual void CompareWith(_EB10 attribute)
	{
		_E127.Dispose();
		if (attribute != null && attribute != ItemAttribute)
		{
			CompareItemAttribute = attribute;
			CompareItemAttribute.OnUpdate += SetValues;
			_E127.AddDisposable(delegate
			{
				CompareItemAttribute.OnUpdate -= SetValues;
			});
		}
		else
		{
			CompareItemAttribute = ItemAttribute;
		}
		SetValues();
	}

	public virtual void SetValues()
	{
		if (ValueText == null)
		{
			return;
		}
		ValueText.gameObject.SetActive(ItemAttribute.IsDisplayable);
		if (!ItemAttribute.IsDisplayable)
		{
			return;
		}
		if (Examined)
		{
			string text = string.Empty;
			float num = ItemAttribute.Base();
			float num2 = CompareItemAttribute.Base() - num;
			string text2 = ItemAttribute.StringValue();
			if (ItemAttribute.LabelVariations == EItemAttributeLabelVariations.Colored)
			{
				if (num < 0f)
				{
					ValueText.color = (ItemAttribute.LessIsGood ? _increasingColor : _decreasingColor);
					if (!text2.Contains(_ED3E._E000(29690)))
					{
						text = _ED3E._E000(29690);
					}
				}
				else if (num > 0f)
				{
					ValueText.color = (ItemAttribute.LessIsGood ? _decreasingColor : _increasingColor);
					text = _ED3E._E000(29692);
				}
			}
			ValueText.text = text + text2;
			if (!num2.IsZero())
			{
				TextMeshProUGUI valueText = ValueText;
				valueText.text = valueText.text + _ED3E._E000(27312) + ((num2 > 0f) ? _ED3E._E000(29692) : string.Empty) + num2.ToString(_ED3E._E000(56089)) + _ED3E._E000(27308);
			}
		}
		else
		{
			ValueText.text = _ED3E._E000(91186);
		}
		this.WaitOneFrame(_E000);
	}

	private void _E000()
	{
		if (!(NameText == null) && NameText.IsActive())
		{
			float num = NameText.preferredWidth;
			if (ValueText != null && ValueText.gameObject.activeSelf && !string.IsNullOrEmpty(ValueText.text))
			{
				num += ValueText.preferredWidth + 5f;
			}
			TextWidth = num;
			_E125?.Invoke(this, EventArgs.Empty);
		}
	}

	[CompilerGenerated]
	private void _E001(PointerEventData arg)
	{
		if (!string.IsNullOrEmpty(_E126))
		{
			Tooltip.Show(_E126);
		}
	}

	[CompilerGenerated]
	private void _E002(PointerEventData arg)
	{
		if (Tooltip != null && Tooltip.isActiveAndEnabled)
		{
			Tooltip.Close();
		}
	}

	[CompilerGenerated]
	private void _E003()
	{
		ItemAttribute.OnUpdate -= SetValues;
	}

	[CompilerGenerated]
	private void _E004()
	{
		CompareItemAttribute.OnUpdate -= SetValues;
	}
}
