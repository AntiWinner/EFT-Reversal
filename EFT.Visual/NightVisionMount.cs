using System;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.Visual;

public class NightVisionMount : MonoBehaviour, _E7F4, _E640, _E63F, _E641
{
	[SerializeField]
	private CurveRotator _rotator;

	private Item m__E000;

	private Slot[] m__E001;

	private IItemOwner m__E002;

	private TogglableComponent m__E003;

	private Action _E004;

	public void Init(Item item, bool isAnimated)
	{
		this.m__E000 = item;
		_E000(initial: true);
		if (item.CurrentAddress != null)
		{
			this.m__E002 = item.Parent.GetOwnerOrNull();
			this.m__E002?.RegisterView(this);
		}
	}

	public void OnItemRemoved(_EAF3 obj)
	{
		if (obj.Status == CommandStatus.Succeed && obj.From is _EB20)
		{
			OnItemAddedOrRemoved((Slot)obj.From.Container, isAdded: false);
		}
	}

	public void OnItemAdded(_EAF2 obj)
	{
		if (obj.Status == CommandStatus.Succeed && obj.To is _EB20)
		{
			OnItemAddedOrRemoved((Slot)obj.To.Container, isAdded: true);
		}
	}

	public void OnItemAddedOrRemoved(Slot slot, bool isAdded)
	{
		if (this.m__E001.Contains(slot))
		{
			_E000(initial: false);
		}
	}

	private void _E000(bool initial)
	{
		this.m__E001 = ((this.m__E000 is _EA40) ? ((_EA40)this.m__E000).AllSlots.ToArray() : new Slot[0]);
		TogglableComponent togglableComponent = this.m__E000.GetItemComponentsInChildren<TogglableComponent>().FirstOrDefault();
		_E001(togglableComponent, initial);
	}

	private void _E001([CanBeNull] TogglableComponent togglableComponent, bool initial)
	{
		if (togglableComponent == this.m__E003)
		{
			return;
		}
		if (this.m__E003 != null)
		{
			_E004();
		}
		this.m__E003 = togglableComponent;
		if (this.m__E003 != null)
		{
			_E004 = this.m__E003.OnChanged.Subscribe(delegate
			{
				_E002(initial: false);
			});
		}
		_E002(initial);
	}

	private void _E002(bool initial)
	{
		if (_rotator == null)
		{
			Debug.LogError(_ED3E._E000(168714) + this.m__E000.Name.Localized() + _ED3E._E000(168759));
		}
		else
		{
			_rotator.Set(this.m__E003 != null && this.m__E003.On, initial);
		}
	}

	public void Deinit()
	{
		_E001(null, initial: false);
		this.m__E002?.UnregisterView(this);
	}

	[CompilerGenerated]
	private void _E003()
	{
		_E002(initial: false);
	}
}
