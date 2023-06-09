using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.UI;

public class LocationButton : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public LocationButton _003C_003E4__this;

		public _E554.Location location;

		internal void _E000(bool isOn)
		{
			if (isOn)
			{
				_003C_003E4__this._E03D(location);
			}
		}
	}

	[SerializeField]
	private GameObject _infoPanel;

	[SerializeField]
	private CustomTextMeshProUGUI _infoText;

	[SerializeField]
	private RectTransform _secretBorder;

	[SerializeField]
	private GameObject _lockedIcon;

	[SerializeField]
	private GameObject _overloadedIcon;

	[SerializeField]
	private GameObject _availableIcon;

	[SerializeField]
	private GameObject _newIcon;

	[SerializeField]
	private UISpawnableToggle _spawnableToggle;

	private Action<_E554.Location> _E03D;

	public void Show(_E554.Location location, ESideType chosenSideType, bool onAtStart, int playerLevel, bool isOverloaded, bool isNew, Action<_E554.Location> onSelected)
	{
		UI.Dispose();
		ShowGameObject();
		_E03D = onSelected;
		_spawnableToggle.Toggle.group.allowSwitchOff = true;
		_spawnableToggle.Toggle.isOn = onAtStart;
		_spawnableToggle._E000 = onAtStart;
		_spawnableToggle._E001(location._Id + _ED3E._E000(70087), 25, null, null);
		bool flag = location.Locked || playerLevel < location.RequiredPlayerLevel || (chosenSideType == ESideType.Savage && location.DisabledForScav);
		_lockedIcon.SetActive(flag);
		_overloadedIcon.SetActive(isOverloaded);
		_newIcon.SetActive(isNew);
		_availableIcon.SetActive(!flag && !isOverloaded && !isNew);
		_secretBorder.gameObject.SetActive(location.IsSecret);
		_infoPanel.SetActive(flag || isOverloaded);
		if (flag)
		{
			_infoText.text = _ED3E._E000(249044).Localized().ToUpper();
		}
		else if (isOverloaded)
		{
			_infoText.text = _ED3E._E000(249027).Localized().ToUpper();
		}
		RectTransform obj = (RectTransform)base.transform;
		Vector2 anchorMax = (obj.anchorMin = location.RelativeMapPos);
		obj.anchorMax = anchorMax;
		obj.anchoredPosition = Vector2.zero;
		UI.SubscribeEvent(_spawnableToggle.Toggle.onValueChanged, delegate(bool isOn)
		{
			if (isOn)
			{
				_E03D(location);
			}
		});
	}

	public void Select(bool value)
	{
		_spawnableToggle.Toggle._E001 = true;
	}
}
