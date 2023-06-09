using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.Hideout;

public class AirFilteringUnitBehaviour : _E831, _E819, _E7A9, _E82F, _E81A, _E830
{
	[CompilerGenerated]
	private new Action<Item, int> m__E000;

	private _E80E m__E001;

	public _E80E ResourceConsumer => this.m__E001 ?? (this.m__E001 = new _E80E());

	public bool Changeable => !base.Data.Template.TakeFromSlotLocked;

	public _EA1E[] UsingItems => ResourceConsumer.UsingItems;

	public float Consumption => ResourceConsumer.ResultConsumption;

	public float ConsumptionReduction => ResourceConsumer.ConsumptionReduction;

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
					array[i] = items.FirstOrDefault().Value as _EA1E;
				}
				catch (IndexOutOfRangeException message)
				{
					Debug.LogError(message);
				}
			}
		}
		ResourceConsumer.StateChanged += _E000;
		ResourceConsumer.SetItems(array);
		ResourceConsumer.OnItemResourceEnded += _E001;
		_E000();
	}

	public void EnergyGenerationChanged(bool isOn)
	{
		ResourceConsumer.IsOn = isOn;
	}

	private new void _E000()
	{
		base.Data.SetProductionState(ResourceConsumer.IsWorking);
	}

	private void _E001(Item item, int index)
	{
		_EA1E[] array = new _EA1E[UsingItems.Length];
		Array.Copy(UsingItems, array, array.Length);
		array[index] = null;
		ResourceConsumer.SetItems(array);
		this.m__E000?.Invoke(null, index);
	}

	private void _E002()
	{
		_E837 obj = base.Data.CurrentStage.FuelSupply.Data?.FirstOrDefault();
		if (obj == null)
		{
			_E815._E000.Instance.LogWarn(_ED3E._E000(170730) + base.Data.CurrentStage.Level);
			return;
		}
		_E815._E000.Instance.LogInfo(_ED3E._E000(170805), obj.AmountPerSecond);
		ResourceConsumer.Consumption = obj.AmountPerSecond;
	}

	protected override void LevelUpdatedHandler()
	{
		base.LevelUpdatedHandler();
		_E002();
		_E003();
	}

	private new void _E003()
	{
		ResourceConsumer.UpdateSlotsData(base.Slots);
	}

	public void InstallConsumableItems(_EA1E[] installedSupplies)
	{
		ResourceConsumer.SetItems(installedSupplies);
		PlaySound(EAreaActivityType.ItemInstall);
	}

	public void Start(float profileDecayTime)
	{
		if (UsingItems == null || !this.m__E001.IsOn)
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

	public void Update(float deltaTime)
	{
		ResourceConsumer.Work(deltaTime);
	}
}
