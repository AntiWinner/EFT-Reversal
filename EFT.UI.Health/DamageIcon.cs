using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.Health;

public class DamageIcon : UIElement
{
	[SerializeField]
	private Image _icon;

	private HoverTrigger _E0C3;

	private SimpleTooltip _E02A;

	private string _E2C0;

	private void Awake()
	{
		_E0C3 = base.gameObject.GetOrAddComponent<HoverTrigger>();
		_E0C3.OnHoverStart += delegate
		{
			if (_E02A != null && !string.IsNullOrEmpty(_E2C0))
			{
				_E02A.Show(_E2C0, null, 0f, -1f);
			}
		};
		_E0C3.OnHoverEnd += delegate
		{
			if (_E02A != null)
			{
				_E02A.Close();
			}
		};
	}

	public void Show(DamageStats.EDamageResult iconType, List<DamageStats> damageList)
	{
		_E02A = ItemUiContext.Instance.Tooltip;
		if (EFTHardSettings.Instance.StaticIcons.DamageResultSprites.TryGetValue(iconType, out var value))
		{
			_icon.sprite = value;
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (DamageStats damage in damageList)
		{
			stringBuilder.AppendLine(damage.ToString());
		}
		_E2C0 = stringBuilder.ToString();
		ShowGameObject();
	}

	public override void Close()
	{
		base.Close();
		if (_E02A != null)
		{
			_E02A.Close();
		}
		_icon.sprite = null;
	}

	[CompilerGenerated]
	private void _E000(PointerEventData arg)
	{
		if (_E02A != null && !string.IsNullOrEmpty(_E2C0))
		{
			_E02A.Show(_E2C0, null, 0f, -1f);
		}
	}

	[CompilerGenerated]
	private void _E001(PointerEventData _)
	{
		if (_E02A != null)
		{
			_E02A.Close();
		}
	}
}
