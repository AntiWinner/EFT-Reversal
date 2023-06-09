using Comfort.Common;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.ClientItems.ClientSpecItems;

public class RadioTransmitterView : MonoBehaviour
{
	[Header("Light signals")]
	[SerializeField]
	private Light _greenLight;

	[SerializeField]
	private Light _yellowLight;

	[SerializeField]
	private Light _redLight;

	[SerializeField]
	[Header("Sound signals")]
	private AudioClip _greenSoundSignal;

	[SerializeField]
	private AudioClip _yellowSoundSignal;

	[SerializeField]
	private AudioClip _redSoundSignal;

	private Player m__E000;

	private _E7F8 m__E001;

	public void Initialiaze(Player player)
	{
		if (player.RecodableItemsHandler.TryToGetRecodableComponent<RadioTransmitterRecodableComponent>(out var component))
		{
			if (this.m__E000 != null)
			{
				this.m__E000.OnPlayerDeadOrUnspawn -= _E000;
			}
			this.m__E000 = player;
			if (Singleton<AbstractGame>.Instance is _E7A6)
			{
				((_E7A6)Singleton<AbstractGame>.Instance).GetRadioTransmitterData(this.m__E000.ProfileId);
			}
			_E637 handler = component.Handler;
			if (this.m__E001 == null)
			{
				this.m__E001 = new _E7F8(this, base.transform, handler, _greenLight, _yellowLight, _redLight, _greenSoundSignal, _yellowSoundSignal, _redSoundSignal);
			}
			else
			{
				this.m__E001.SetHandler(handler);
			}
			this.m__E000.OnPlayerDeadOrUnspawn += _E000;
		}
	}

	private void _E000(Player player)
	{
		this.m__E000.OnPlayerDeadOrUnspawn -= _E000;
		this.m__E001.ClearData();
		this.m__E001 = null;
		this.m__E000 = null;
	}

	private void _E001(bool value)
	{
		_greenLight.enabled = value;
		_yellowLight.enabled = value;
		_redLight.enabled = value;
	}

	private void OnEnable()
	{
		if (this.m__E001 != null)
		{
			this.m__E001.OnEnable();
		}
		else
		{
			_E001(value: false);
		}
	}

	private void OnDisable()
	{
		if (this.m__E001 != null)
		{
			this.m__E001.OnDisable();
		}
	}

	private void OnDestroy()
	{
		if (this.m__E001 != null)
		{
			this.m__E001.OnDestroy();
		}
	}
}
