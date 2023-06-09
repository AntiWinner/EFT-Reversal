using JetBrains.Annotations;

namespace EFT.InventoryLogic;

public abstract class ItemAddress
{
	public readonly IContainer Container;

	public abstract Item Item { get; }

	public abstract string ContainerName { get; }

	protected ItemAddress(IContainer container)
	{
		Container = container;
	}

	public abstract _ECD9<int> GetMaxAddCount(Item item, string visitorId);

	public abstract _ECD9<int> Add(Item item, string[] visitorIds, bool simulate = false);

	public abstract _ECD9<int> AddWithoutRestrictions(Item item, string[] visitorIds);

	public abstract _ECD9<_EB23> Remove(Item item, string visitorId, bool simulate = false);

	public virtual void RaiseAddEvent(Item item, CommandStatus status, string profileId, bool silent = false)
	{
		this.GetOwner().RaiseAddEvent(new _EAF2(item, this, status, silent, profileId));
		foreach (Item allParentItem in this.GetAllParentItems())
		{
			allParentItem.ChildrenChanged.Invoke(item);
		}
	}

	public virtual void RaiseMergeEvent(Item item, CommandStatus status, string profileId, bool silent = false)
	{
		this.GetOwner().RaiseMergeEvent(new _EB09(item, this, status, silent, profileId));
		foreach (Item allParentItem in this.GetAllParentItems())
		{
			allParentItem.ChildrenChanged.Invoke(item);
		}
	}

	public void RaiseDiscoverEvent(Item item)
	{
		this.GetOwner().RaiseDiscoverEvent(new _EB07(item, this));
	}

	public virtual void RaiseRemoveEvent(Item item, CommandStatus status)
	{
		this.GetOwner().RaiseRemoveEvent(new _EAF3(item, this, status));
		foreach (Item allParentItem in this.GetAllParentItems())
		{
			allParentItem.ChildrenChanged.Invoke(item);
		}
	}

	public override bool Equals([CanBeNull] object obj)
	{
		if (this != obj)
		{
			if (obj is ItemAddress)
			{
				return Container == ((ItemAddress)obj).Container;
			}
			return false;
		}
		return true;
	}

	public override int GetHashCode()
	{
		return Container.GetHashCode();
	}

	public virtual int GetHashSum()
	{
		int num = 2777 * Container.ID.GetHashCode();
		string text = Container.ParentItem?.TemplateId;
		if (text != null)
		{
			num += 7901 * text.GetHashCode();
		}
		return num;
	}
}
