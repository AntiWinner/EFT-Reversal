using System;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.Visual;

public class ThermalVisionDevice : MonoBehaviour, _E7F4
{
	[SerializeField]
	private Light _light;

	private Action m__E000;

	private ThermalVisionComponent _E001;

	public void Init(Item item, bool isAnimated)
	{
		_E001 = item.GetItemComponent<ThermalVisionComponent>();
		this.m__E000 = _E001.Togglable.OnChanged.Bind(delegate
		{
			_light.enabled = _E001.Togglable.On;
		});
	}

	public void Deinit()
	{
		this.m__E000();
	}

	[CompilerGenerated]
	private void _E000()
	{
		_light.enabled = _E001.Togglable.On;
	}
}
