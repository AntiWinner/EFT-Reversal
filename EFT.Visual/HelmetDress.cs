using System;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.Visual;

public class HelmetDress : MonoBehaviour, _E7F4, _E640, _E63F, _E641
{
	public Quaternion OffRotation;

	public Quaternion OnRotation;

	public Transform HingeTransform;

	public HysteresisFilter Filter;

	private _EA56 m__E000;

	private Slot[] m__E001;

	private IItemOwner m__E002;

	private FaceShieldComponent m__E003;

	private Action m__E004;

	public void Init(Item item, bool isAnimated)
	{
		Filter.Init(delegate(float x)
		{
			HingeTransform.localRotation = Quaternion.Lerp(OffRotation, OnRotation, x);
		});
		this.m__E000 = (item as _EA56) ?? ((item.CurrentAddress != null) ? (item.Parent.Container.ParentItem as _EA56) : null);
		if (this.m__E000 != null)
		{
			_E000(initial: true);
			if (item.CurrentAddress != null)
			{
				this.m__E002 = item.Parent.GetOwnerOrNull();
				this.m__E002?.RegisterView(this);
			}
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
		this.m__E001 = this.m__E000.AllSlots.ToArray();
		FaceShieldComponent faceShield = this.m__E000.GetItemComponentsInChildren<FaceShieldComponent>().FirstOrDefault();
		_E001(faceShield, initial);
	}

	private void _E001([CanBeNull] FaceShieldComponent faceShield, bool initial)
	{
		if (faceShield == this.m__E003)
		{
			return;
		}
		if (this.m__E003 != null && this.m__E003.Togglable != null)
		{
			this.m__E004();
		}
		this.m__E003 = faceShield;
		if (this.m__E003 != null && this.m__E003.Togglable != null)
		{
			this.m__E004 = this.m__E003.Togglable.OnChanged.Subscribe(delegate
			{
				_E002(initial: false);
			});
		}
		_E002(initial);
	}

	private void _E002(bool initial)
	{
		Filter.Set(this.m__E003 == null || this.m__E003.Togglable == null || this.m__E003.Togglable.On, initial);
	}

	public void Deinit()
	{
		_E001(null, initial: false);
		this.m__E002?.UnregisterView(this);
	}

	[CompilerGenerated]
	private void _E003(float x)
	{
		HingeTransform.localRotation = Quaternion.Lerp(OffRotation, OnRotation, x);
	}

	[CompilerGenerated]
	private void _E004()
	{
		_E002(initial: false);
	}
}
