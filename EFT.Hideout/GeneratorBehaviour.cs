using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using EFT.Communications;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.Hideout;

public class GeneratorBehaviour : _E831, _E819, _E810, _E82F, IDisposable, _E830
{
	private const int _E00C = 5;

	public static readonly _ECED<ELightStatus> OnGeneratorInstalledHandler = new _ECED<ELightStatus>();

	[CompilerGenerated]
	private static bool _E00D;

	[CompilerGenerated]
	private new Action<Item, int> m__E000;

	[CompilerGenerated]
	private Action<_E810, bool> _E00E;

	private _E8A2 _E00F;

	private _E8A2 _E010;

	private _E80E m__E001;

	private static _E835 _E011;

	private bool _E012;

	public static bool IsGeneratorInstalled
	{
		[CompilerGenerated]
		get
		{
			return _E00D;
		}
		[CompilerGenerated]
		private set
		{
			_E00D = value;
		}
	}

	public _E80E ResourceConsumer => this.m__E001 ?? (this.m__E001 = new _E80E());

	public static _E835 FuelDetails => _E011 ?? (_E011 = new _E835());

	public override bool HasError
	{
		get
		{
			if (base.Data.CurrentLevel != 0)
			{
				return !IsWorking;
			}
			return false;
		}
	}

	public bool Changeable => !base.Data.Template.TakeFromSlotLocked;

	public _EA1E[] UsingItems => ResourceConsumer.UsingItems;

	public float Consumption => ResourceConsumer.ResultConsumption;

	public float ConsumptionReduction => ResourceConsumer.ConsumptionReduction;

	public bool SwitchedOn => ResourceConsumer.IsOn;

	public bool IsWorking
	{
		get
		{
			return _E012;
		}
		private set
		{
			if (_E012 != value)
			{
				_E012 = value;
				_E00E?.Invoke(this, value);
				_E004();
			}
		}
	}

	public event Action<Item, int> OnConsumableItemChanged
	{
		[CompilerGenerated]
		add
		{
			Action<Item, int> action = this.m__E000;
			Action<Item, int> action2;
			do
			{
				action2 = action;
				Action<Item, int> value2 = (Action<Item, int>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Item, int> action = this.m__E000;
			Action<Item, int> action2;
			do
			{
				action2 = action;
				Action<Item, int> value2 = (Action<Item, int>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<_E810, bool> OnWorkingStateChanged
	{
		[CompilerGenerated]
		add
		{
			Action<_E810, bool> action = _E00E;
			Action<_E810, bool> action2;
			do
			{
				action2 = action;
				Action<_E810, bool> value2 = (Action<_E810, bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E00E, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<_E810, bool> action = _E00E;
			Action<_E810, bool> action2;
			do
			{
				action2 = action;
				Action<_E810, bool> value2 = (Action<_E810, bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E00E, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public override void Init(AreaData area, AreaInfo profileInfo, _E74F skills)
	{
		base.Init(area, profileInfo, skills);
		_E002();
		_EA1E[] array = new _EA1E[base.Slots.NumberOfSlots];
		for (int i = 0; i < profileInfo.slots.Length; i++)
		{
			_E818 obj = profileInfo.slots[i];
			if (obj?.Items != null && obj.Items.Length != 0)
			{
				Dictionary<string, Item> items = Singleton<_E63B>.Instance.FlatItemsToTree(obj.Items).Items;
				try
				{
					array[i] = items.FirstOrDefault().Value as _EA32;
				}
				catch (IndexOutOfRangeException message)
				{
					Debug.LogError(message);
				}
			}
		}
		ResourceConsumer.StateChanged += _E000;
		ResourceConsumer.SetItems(array);
		IsWorking = ResourceConsumer.IsWorking;
		_E000();
		Singleton<BonusController>.Instance.OnBonusChanged += _E001;
	}

	private new void _E000()
	{
		base.Data.SetProductionState(ResourceConsumer.IsWorking);
	}

	protected override void Init(AreaData data)
	{
		base.Init(data);
		_E00F = new _E8A3(EHideoutNotificationType.FuelIsLow, base.Data.Template, base._E003);
		_E010 = new _E8A3(EHideoutNotificationType.NoFuel, base.Data.Template, base._E003);
		_E004();
	}

	protected override void LevelUpdatedHandler()
	{
		base.LevelUpdatedHandler();
		FuelDetails.IsActive = base.Data.CurrentLevel > 0;
		_E003();
		_E002();
		if (base.Data.CurrentLevel > 0 && !IsGeneratorInstalled)
		{
			IsGeneratorInstalled = true;
			OnGeneratorInstalledHandler?.Invoke(SwitchedOn ? ELightStatus.Working : ELightStatus.OutOfFuel);
		}
	}

	private void _E001(bool isActive, EBonusType bonusType)
	{
		if (bonusType == EBonusType.FuelConsumption)
		{
			_E002();
		}
	}

	private void _E002()
	{
		_E837 obj = base.Data.CurrentStage.FuelSupply.Data?.FirstOrDefault();
		if (obj == null)
		{
			_E815._E000.Instance.LogWarn(_ED3E._E000(170837));
			return;
		}
		ResourceConsumer.Consumption = (float)Singleton<BonusController>.Instance.Calculate(EBonusType.FuelConsumption, obj.AmountPerSecond);
		_E815._E000.Instance.LogInfo(_ED3E._E000(170864), ResourceConsumer.Consumption);
		_E004();
	}

	private new void _E003()
	{
		ResourceConsumer.UpdateSlotsData(base.Slots);
		_E004();
	}

	public void InstallConsumableItems(_EA1E[] installedSupplies)
	{
		if (UsingItems != null)
		{
			ResourceConsumer.SetItems(installedSupplies);
			PlaySound(EAreaActivityType.ItemInstall);
		}
	}

	public void Start(float profileDecayTime)
	{
		if (UsingItems == null || !ResourceConsumer.IsOn)
		{
			return;
		}
		float num = profileDecayTime * Consumption;
		_EA1E[] usingItems = UsingItems;
		for (int i = 0; i < usingItems.Length; i++)
		{
			ResourceComponent resourceComponent = usingItems[i]?.GetItemComponent<ResourceComponent>();
			if (resourceComponent != null && !(resourceComponent.Value < Mathf.Epsilon))
			{
				float num2 = Mathf.Min(num, resourceComponent.Value);
				resourceComponent.Value -= num2;
				num -= num2;
				if (num < Mathf.Epsilon)
				{
					break;
				}
			}
		}
	}

	public bool Work(float deltaTime)
	{
		IsWorking = ResourceConsumer.Work(deltaTime);
		if (IsWorking)
		{
			_E004();
		}
		return IsWorking;
	}

	public void SetSwitchedStatus(bool isOn)
	{
		ResourceConsumer.IsOn = isOn;
		_E004();
	}

	private void _E004()
	{
		_E835 fuelDetails = FuelDetails;
		if (!ResourceConsumer.IsOn)
		{
			fuelDetails.Type = EDetailsType.SwitchedOff;
		}
		else if (IsWorking)
		{
			fuelDetails.Type = EDetailsType.Fuel;
			if (ResourceConsumer.Consumption >= float.Epsilon)
			{
				int percentages = fuelDetails.Percentages;
				fuelDetails.Percentages = (int)Math.Ceiling(ResourceConsumer.TotalResourceCountLeft / ResourceConsumer.TotalResourceCount * 100.0);
				fuelDetails.Time = ResourceConsumer.Timer.TimeLeft;
				if (percentages >= 5 && fuelDetails.Percentages < 5)
				{
					_E00F.Show();
				}
				else if (percentages < 5 && fuelDetails.Percentages >= 5)
				{
					_E00F.Hide();
				}
				if (percentages > 0 && fuelDetails.Percentages == 0)
				{
					if (_E00F.Active)
					{
						_E00F.Hide();
					}
					_E010.Show();
				}
				else if (percentages == 0 && fuelDetails.Percentages > 0)
				{
					_E010.Hide();
				}
			}
			else
			{
				fuelDetails.Percentages = 100;
				fuelDetails.Time = 0;
			}
		}
		else
		{
			fuelDetails.Type = EDetailsType.NoFuel;
		}
	}

	public void Dispose()
	{
		Singleton<BonusController>.Instance.OnBonusChanged -= _E001;
		ResourceConsumer.StateChanged -= _E000;
	}
}
