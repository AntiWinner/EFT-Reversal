using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public class SideSelectionToggle : Toggle
{
	[SerializeField]
	private EPlayerSide _side;

	[SerializeField]
	private UIAnimatedToggleSpawner _sideSelectionToggle;

	[NonSerialized]
	public _ECED<bool> OnHover = new _ECED<bool>();

	private Action<EPlayerSide> _onSelectedAction;

	public void Init(Action<EPlayerSide> onSelectedAction)
	{
		_onSelectedAction = onSelectedAction;
		onValueChanged.RemoveAllListeners();
		onValueChanged.AddListener(_E000);
	}

	private void _E000(bool value)
	{
		if (value)
		{
			_onSelectedAction?.Invoke(_side);
		}
		_sideSelectionToggle.SpawnedObject._E001 = value;
	}

	public override void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
		_sideSelectionToggle.SpawnedObject.OnPointerEnter(eventData);
		OnHover.Invoke(arg: true);
	}

	public override void OnPointerExit([NotNull] PointerEventData eventData)
	{
		base.OnPointerExit(eventData);
		_sideSelectionToggle.SpawnedObject.OnPointerExit(eventData);
		OnHover.Invoke(arg: false);
	}
}
