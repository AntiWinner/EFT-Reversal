using System;
using System.Collections.Generic;
using Comfort.Common;
using JetBrains.Annotations;

namespace EFT.InventoryLogic;

public interface IItemOwner : IContainer
{
	Item RootItem { get; }

	[CanBeNull]
	ContainerCollection QuestRaidItem { get; }

	[CanBeNull]
	ContainerCollection QuestStashItem { get; }

	bool InboundTransfer { get; }

	bool Locked { get; set; }

	string ContainerName { get; }

	EOwnerType OwnerType { get; }

	event Action<_EAF2> AddItemEvent;

	event Action<_EAF3> RemoveItemEvent;

	event Action<_EAF6> PlayerExamineEvent;

	event Action<_EB04> CreateMapMarkerEvent;

	event Action<_EB05> EditMapMarkerEvent;

	event Action<_EB06> DeleteMapMarkerEvent;

	event Action<_EAFF> RefreshItemEvent;

	event Action<_EAF1> ActiveEventsChanged;

	void InProcess(Item item, ItemAddress to, bool succeed, _EB72 operation, Callback callback);

	void OutProcess(Item item, ItemAddress from, ItemAddress to, _EB72 operation, Callback callback);

	void RaiseAddEvent(_EAF2 args);

	void RaiseRemoveEvent(_EAF3 args);

	void RaiseExamineEvent(_EAF6 args);

	void RaiseLoadMagazineEvent(_EAF7 args);

	void RaiseUnloadMagazineEvent(_EAF8 args);

	void RaiseInventoryCheckMagazine(_EA6A item, float time, bool status);

	void RaiseEvent(_EAFF args);

	void RaiseEvent(_EB01 args);

	void RaiseBindItemEvent(_EAFC args);

	void RaiseInsureItemsEvent(_EAFE args);

	void RaiseUnbindItemEvent(_EAFD args);

	void RaiseCreateMapMarkerEvent(_EB04 args);

	void RaiseEditMapMarkerEvent(_EB05 args);

	void RaiseDeleteMapMarkerEvent(_EB06 args);

	void RaiseDiscoverEvent(_EB07 args);

	void RaiseMergeEvent(_EB09 args);

	void RegisterView(_E63F view);

	void UnregisterView(_E63F view);

	IEnumerable<T> SelectEvents<T>(Item item = null) where T : _EAF1;

	IEnumerable<_EAF1> SelectEvents(Item item);

	bool CheckItemAction(Item item, [CanBeNull] ItemAddress location);

	void RemoveActiveEvent(_EAF1 activeEvent);

	[CanBeNull]
	ItemAddress ToItemAddress(_E673 descriptor);

	_EB20 ToSlotItemAddress(_E674 descriptor);

	_EB22 ToGridItemAddress(_E677 descriptor);

	_EB21 ToStackSlotItemAddress(_E675 descriptor);
}
