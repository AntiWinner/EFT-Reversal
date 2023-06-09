using System.Runtime.CompilerServices;
using EFT.UI;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.Hideout;

public sealed class BonusPanel : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public BonusPanel _003C_003E4__this;

		public HoverTrigger hoverTrigger;

		internal void _E000(PointerEventData eventData)
		{
			if (!string.IsNullOrEmpty(_003C_003E4__this._E029))
			{
				_003C_003E4__this._E02A.Show(_ED3E._E000(103088) + _003C_003E4__this._E029.Localized() + _ED3E._E000(59467));
			}
		}

		internal void _E001(PointerEventData eventData)
		{
			_003C_003E4__this._E02A.Close();
		}

		internal void _E002()
		{
			hoverTrigger.OnHoverStart -= delegate
			{
				if (!string.IsNullOrEmpty(_003C_003E4__this._E029))
				{
					_003C_003E4__this._E02A.Show(_ED3E._E000(103088) + _003C_003E4__this._E029.Localized() + _ED3E._E000(59467));
				}
			};
		}

		internal void _E003()
		{
			hoverTrigger.OnHoverEnd -= delegate
			{
				_003C_003E4__this._E02A.Close();
			};
		}
	}

	private const string _E026 = "hideout/bonus_is_inactive_due_to_energy";

	private const string _E027 = "hideout/bonus_is_inactive_due_to_production_state";

	[SerializeField]
	private Image _icon;

	[SerializeField]
	private TextMeshProUGUI _description;

	[SerializeField]
	private TextMeshProUGUI _effect;

	private _E5EA _E028;

	private string _E029;

	private SimpleTooltip _E02A;

	public void Show(_E5EA bonus, Sprite sprite)
	{
		ShowGameObject();
		_icon.sprite = sprite;
		_icon.SetNativeSize();
		_E028 = bonus;
		HoverTrigger hoverTrigger = base.GameObject.GetOrAddComponent<HoverTrigger>();
		_E02A = ItemUiContext.Instance.Tooltip;
		hoverTrigger.OnHoverStart += delegate
		{
			if (!string.IsNullOrEmpty(_E029))
			{
				_E02A.Show(_ED3E._E000(103088) + _E029.Localized() + _ED3E._E000(59467));
			}
		};
		hoverTrigger.OnHoverEnd += delegate
		{
			_E02A.Close();
		};
		UI.AddDisposable(delegate
		{
			hoverTrigger.OnHoverStart -= delegate
			{
				if (!string.IsNullOrEmpty(_E029))
				{
					_E02A.Show(_ED3E._E000(103088) + _E029.Localized() + _ED3E._E000(59467));
				}
			};
		});
		UI.AddDisposable(delegate
		{
			hoverTrigger.OnHoverEnd -= delegate
			{
				_E02A.Close();
			};
		});
	}

	public void UpdateView(ELightStatus lightStatus, bool activeProduction, ELevelType levelType)
	{
		double num = _E028.ResultValue;
		if (levelType == ELevelType.Current)
		{
			if (!_E028.Passive && lightStatus != ELightStatus.Working)
			{
				num = 0.0;
				_E029 = _ED3E._E000(165371);
			}
			else if (_E028.Production && !activeProduction)
			{
				num = 0.0;
				_E029 = _ED3E._E000(165395);
			}
			else
			{
				_E029 = string.Empty;
			}
		}
		_description.text = _E028.LocalizationKey.Localized();
		_effect.text = ((!string.IsNullOrEmpty(_E028.ValueFormat)) ? string.Format(_E028.ValueFormat, num) : string.Empty);
		if ((num.IsZero() || !_E028.IsPositive) && !(_E028 is _E5EB))
		{
			_effect.text = _ED3E._E000(103088) + _effect.text + _ED3E._E000(59467);
		}
	}

	public void UpdateIcon([CanBeNull] Texture2D resultValue)
	{
		if (!(this == null) && !(_icon == null))
		{
			if (resultValue == null)
			{
				_icon.sprite = null;
				return;
			}
			Sprite sprite = Sprite.Create(resultValue, new Rect(0f, 0f, resultValue.width, resultValue.height), Vector2.one * 0.5f);
			_icon.sprite = sprite;
		}
	}

	public override void Close()
	{
		_E02A.Close();
		base.Close();
	}
}
