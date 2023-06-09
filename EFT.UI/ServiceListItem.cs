using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class ServiceListItem : UIElement
{
	[SerializeField]
	private Image _serviceIcon;

	[SerializeField]
	private Image _serviceIconBackground;

	[SerializeField]
	private Image _serviceIconBorder;

	[SerializeField]
	private Button _serviceButton;

	[SerializeField]
	private Image _serviceBackgroud;

	[SerializeField]
	private Image _selectedArrow;

	[SerializeField]
	private TextMeshProUGUI _serviceText;

	[SerializeField]
	private ColorMap _backColors;

	[SerializeField]
	private ColorMap _textColors;

	private Action<ServiceListItem> _E03D;

	[CompilerGenerated]
	private ServicesListView.ServiceInfo _E1EA;

	public ServicesListView.ServiceInfo ServiceInfo
	{
		[CompilerGenerated]
		get
		{
			return _E1EA;
		}
		[CompilerGenerated]
		private set
		{
			_E1EA = value;
		}
	}

	private void Awake()
	{
		_serviceButton.onClick.AddListener(delegate
		{
			_E03D(this);
		});
	}

	public void Init(Action<ServiceListItem> onSelected, ServicesListView.ServiceInfo serviceInfo)
	{
		ServiceInfo = serviceInfo;
		_E03D = onSelected;
		_serviceText.text = serviceInfo.Name.Localized();
		_serviceIcon.sprite = serviceInfo.Icon;
		_serviceIconBackground.sprite = serviceInfo.IconBackground;
		ShowGameObject();
	}

	public void UpdateView(bool selected)
	{
		Color color = (selected ? _backColors[_ED3E._E000(258597)] : _backColors[_ED3E._E000(30808)]);
		Color color2 = (selected ? _textColors[_ED3E._E000(258597)] : _textColors[_ED3E._E000(30808)]);
		_serviceIconBorder.color = color2;
		_serviceText.color = color2;
		_serviceBackgroud.color = color;
		_selectedArrow.gameObject.SetActive(selected);
	}

	[CompilerGenerated]
	private void _E000()
	{
		_E03D(this);
	}
}
