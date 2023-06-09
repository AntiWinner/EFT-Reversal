using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.Ragfair;

public class RequirementView : UIElement
{
	[SerializeField]
	private Button _addRequirementButton;

	[SerializeField]
	private CustomTextMeshProUGUI _selectedItemLabel;

	[SerializeField]
	private Image _currencyImage;

	[SerializeField]
	private GameObject _addGameObject;

	[SerializeField]
	private GameObject _removeGameObject;

	[SerializeField]
	private Image _durabilityIcon;

	[SerializeField]
	private Color _onlyFunctionalColor;

	[SerializeField]
	private Color _notOnlyFunctionalColor;

	[CompilerGenerated]
	private HandoverRequirement _E386;

	[CompilerGenerated]
	private _EBAB _E387;

	private Action _E37C;

	private SimpleTooltip _E02A;

	private Action<RequirementView> _E388;

	public HandoverRequirement SelectedRequirement
	{
		[CompilerGenerated]
		get
		{
			return _E386;
		}
		[CompilerGenerated]
		private set
		{
			_E386 = value;
		}
	}

	public _EBAB SelectedNode
	{
		[CompilerGenerated]
		get
		{
			return _E387;
		}
		[CompilerGenerated]
		private set
		{
			_E387 = value;
		}
	}

	private void Awake()
	{
		_addRequirementButton.onClick.AddListener(delegate
		{
			if (SelectedRequirement != null)
			{
				ResetRequirementInformation();
			}
			else if (_E388 != null)
			{
				_E388(this);
			}
		});
		_durabilityIcon.gameObject.AddComponent<HoverTrigger>().Init(delegate
		{
			_E02A.Show(SelectedRequirement.OnlyFunctional ? _ED3E._E000(242274).Localized() : _ED3E._E000(242264).Localized());
		}, delegate
		{
			_E02A.Close();
		});
	}

	public void Show(SimpleTooltip tooltip, Action onRequirementSelected, Action<RequirementView> onWindowShow)
	{
		_E02A = tooltip;
		_E388 = onWindowShow;
		_E37C = onRequirementSelected;
		ShowGameObject();
		ResetRequirementInformation();
	}

	public void ResetRequirementInformation()
	{
		UpdateRequirementInformation(null, null);
	}

	public void UpdateRequirementInformation([CanBeNull] HandoverRequirement requirement, [CanBeNull] _EBAB node)
	{
		SelectedRequirement = requirement;
		SelectedNode = node;
		if (_E37C != null)
		{
			_E37C();
		}
		if (requirement == null)
		{
			_selectedItemLabel.text = string.Empty;
			_addGameObject.SetActive(value: true);
			_removeGameObject.SetActive(value: false);
			_currencyImage.gameObject.SetActive(value: false);
			_durabilityIcon.gameObject.SetActive(value: false);
			return;
		}
		Sprite smallCurrencySign = EFTHardSettings.Instance.StaticIcons.GetSmallCurrencySign(requirement.TemplateId, nullable: true);
		if (smallCurrencySign != null)
		{
			_currencyImage.sprite = smallCurrencySign;
			_selectedItemLabel.text = _ED3E._E000(242191) + requirement.IntCount.FormatSeparate(_ED3E._E000(18502)) + _ED3E._E000(242227);
		}
		else
		{
			_selectedItemLabel.text = requirement.ItemName + _ED3E._E000(54246) + requirement.IntCount.FormatSeparate(_ED3E._E000(18502)) + _ED3E._E000(27308);
		}
		_currencyImage.gameObject.SetActive(smallCurrencySign != null);
		_addGameObject.SetActive(value: false);
		_removeGameObject.SetActive(value: true);
		_durabilityIcon.gameObject.SetActive(requirement.Item is _EA40);
		_durabilityIcon.color = (requirement.OnlyFunctional ? _onlyFunctionalColor : _notOnlyFunctionalColor);
	}

	[CompilerGenerated]
	private void _E000()
	{
		if (SelectedRequirement != null)
		{
			ResetRequirementInformation();
		}
		else if (_E388 != null)
		{
			_E388(this);
		}
	}

	[CompilerGenerated]
	private void _E001(PointerEventData arg)
	{
		_E02A.Show(SelectedRequirement.OnlyFunctional ? _ED3E._E000(242274).Localized() : _ED3E._E000(242264).Localized());
	}

	[CompilerGenerated]
	private void _E002(PointerEventData arg)
	{
		_E02A.Close();
	}
}
