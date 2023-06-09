using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT;

public abstract class NetworkPlayer : Player
{
	[Flags]
	public enum EMovementCommand : byte
	{
		MoveStart = 1,
		MoveEnd = 2,
		SprintStart = 4,
		SprintEnd = 8,
		StartProne = 0x10,
		StopProne = 0x20,
		JumpStart = 0x40,
		JumpEnd = 0x80
	}

	public new class _E000 : _ECD1
	{
		public readonly _E9F7 ContainerId;

		public _E000(_E9F7 containerId)
		{
			ContainerId = containerId;
		}

		public override string ToString()
		{
			return _ED3E._E000(135628) + ContainerId.Id + _ED3E._E000(63757) + ContainerId.ParentId + _ED3E._E000(135679);
		}
	}

	[Serializable]
	public struct ClientShot
	{
		public bool Approved;

		public int BodyPart;

		public float Damage;

		public float LeftBodyPartHealth;

		public string TargetName;

		public override string ToString()
		{
			return string.Format(_ED3E._E000(135666), Approved, (EBodyPart)BodyPart, Damage, LeftBodyPartHealth, TargetName);
		}
	}

	[CompilerGenerated]
	private new sealed class _E001
	{
		public string id;

		internal bool _E000(Item item)
		{
			return item?.Id == id;
		}

		internal bool _E001(Item item)
		{
			return item.Id == id;
		}
	}

	protected readonly Dictionary<uint, Tuple<_EB73, Action>> CurrentOperations = new Dictionary<uint, Tuple<_EB73, Action>>();

	protected readonly Dictionary<uint, Callback<int, bool, EOperationStatus>> OperationCallbacks = new Dictionary<uint, Callback<int, bool, EOperationStatus>>();

	protected ushort LastStationaryWeaponOperationId;

	public static readonly EPhraseTrigger[] LocalPhrases = new EPhraseTrigger[4]
	{
		EPhraseTrigger.OnAgony,
		EPhraseTrigger.OnBeingHurt,
		EPhraseTrigger.OnBreath,
		EPhraseTrigger.OnDeath
	};

	[CompilerGenerated]
	private bool _E0D8;

	[CompilerGenerated]
	private _E62D _E0D9;

	private _E5B5 _E0DA = new _E5B5(LoggerMode.Add);

	protected _E980 NetworkHealthController => base.HealthController as _E980;

	public abstract byte ChannelIndex { get; }

	public override bool IsVisible
	{
		[CompilerGenerated]
		get
		{
			return _E0D8;
		}
		[CompilerGenerated]
		set
		{
			_E0D8 = value;
		}
	}

	public override AbstractHandsController HandsController
	{
		get
		{
			return _handsController;
		}
		protected set
		{
			if ((object)value != _handsController)
			{
				base.HandsController = value;
			}
		}
	}

	protected _E62D FrameIndexer
	{
		[CompilerGenerated]
		get
		{
			return _E0D9;
		}
		[CompilerGenerated]
		private set
		{
			_E0D9 = value;
		}
	}

	internal static _E077 _E000<_E077>(ResourceKey assetName, int playerId, Vector3 position, string prefix, _E62D frameIndexer, EUpdateQueue updateQueue, EUpdateMode armsUpdateMode, EUpdateMode bodyUpdateMode, CharacterControllerSpawner.Mode characterControllerMode, Func<float> getSensitivity, Func<float> getAimingSensitivity, bool isThirdPerson) where _E077 : NetworkPlayer
	{
		_E077 val = Player.Create<_E077>(assetName, playerId, position, updateQueue, armsUpdateMode, bodyUpdateMode, characterControllerMode, getSensitivity, getAimingSensitivity, prefix, isThirdPerson);
		val.FrameIndexer = frameIndexer;
		return val;
	}

	public virtual void OnDeserializeFromServer(byte channelId, _E524 reader, int rtt)
	{
		throw new Exception(_ED3E._E000(137104));
	}

	public override void Dispose()
	{
		base.Dispose();
		FrameIndexer = null;
	}

	protected void ProcessCommonPacket(_E745 commonPacket)
	{
		_E738? obj = commonPacket.StartSearchingActionPacket;
		while (obj.HasValue)
		{
			_E738 valueOrDefault = obj.GetValueOrDefault();
			try
			{
				ProcessStartSearchingAction(valueOrDefault.ItemId);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			obj = valueOrDefault.GetNested();
		}
		_E739? obj2 = commonPacket.StopSearchingActionPacket;
		while (obj2.HasValue)
		{
			_E739 valueOrDefault2 = obj2.GetValueOrDefault();
			try
			{
				_E001(valueOrDefault2.ItemId);
			}
			catch (Exception exception2)
			{
				Debug.LogException(exception2);
			}
			obj2 = valueOrDefault2.GetNested();
		}
		_E73B? obj3 = commonPacket.SetSearchedPacket;
		while (obj3.HasValue)
		{
			_E73B valueOrDefault3 = obj3.GetValueOrDefault();
			try
			{
				_E003(valueOrDefault3.ItemId, valueOrDefault3.GridId, valueOrDefault3.Silent);
			}
			catch (Exception exception3)
			{
				Debug.LogException(exception3);
			}
			obj3 = valueOrDefault3.GetNested();
		}
		_E73A? obj4 = commonPacket.UpdateAccessStatusPacket;
		while (obj4.HasValue)
		{
			_E73A valueOrDefault4 = obj4.GetValueOrDefault();
			try
			{
				_E002(valueOrDefault4.ItemId, valueOrDefault4.State);
			}
			catch (Exception exception4)
			{
				Debug.LogException(exception4);
			}
			obj4 = valueOrDefault4.GetNested();
		}
		if (commonPacket.StopSearchingPacket.HasValue)
		{
			_E962 valueOrDefault5 = commonPacket.StopSearchingPacket.GetValueOrDefault();
			try
			{
				ProcessStopSearching(valueOrDefault5.ItemId);
			}
			catch (Exception exception5)
			{
				Debug.LogException(exception5);
			}
		}
		if (commonPacket.SyncPositionPacket.HasValue)
		{
			_E963 valueOrDefault6 = commonPacket.SyncPositionPacket.GetValueOrDefault();
			ApplyTeleportPacket(valueOrDefault6);
		}
		if (commonPacket.SwitchRenderersPacket.HasValue)
		{
			_E73E valueOrDefault7 = commonPacket.SwitchRenderersPacket.GetValueOrDefault();
			try
			{
				SwitchRenderer(valueOrDefault7.Value);
			}
			catch (Exception exception6)
			{
				Debug.LogException(exception6);
			}
		}
		ProcessChangeSkillExperience(commonPacket.ChangeSkillLevelPacket, silent: false);
		ProcessChangeMasteringExperience(commonPacket.ChangeMasteringLevelPacket, silent: false);
	}

	protected virtual void ApplyTeleportPacket(_E963 packet)
	{
		try
		{
			if (this is ClientPlayer)
			{
				_E0DA.LogInfoFields(_ED3E._E000(137090), packet.Reason, _ED3E._E000(137145), packet.Position.ToStringVerbose(), _ED3E._E000(137138), base.CharacterControllerCommon?.SpeedLimit, _ED3E._E000(137125), base.MovementContext?.CurrentState.Name, _ED3E._E000(137178), base.Skills?.Strength.SummaryLevel, _ED3E._E000(137159), Physical?.WalkSpeedLimit);
				NetworkGameSession.ClientServerConnectionLags = true;
			}
			Teleport(packet.Position);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	protected void ProcessStartSearchingAction(string itemId)
	{
		Debug.LogFormat(_ED3E._E000(137206), itemId);
		if (FindItemById(itemId).OrElse(null) is _EA91 item)
		{
			if (this is ObservedPlayer)
			{
				_E0DE.StartSearchingAction(item);
			}
		}
		else
		{
			Debug.LogError(_ED3E._E000(135190));
		}
	}

	private void _E001(string itemId)
	{
		Debug.LogFormat(_ED3E._E000(135217), itemId);
		if (FindItemById(itemId).OrElse(null) is _EA91 item)
		{
			if (this is ObservedPlayer)
			{
				_E0DE.StopSearchingAction(item);
			}
		}
		else
		{
			Debug.LogError(_ED3E._E000(135248));
		}
	}

	private void _E002(string itemId, SearchedState state)
	{
		Debug.LogFormat(_ED3E._E000(135282), itemId, state);
		_ECD9<Item> obj = FindItemById(itemId);
		if (obj.Failed)
		{
			Debug.LogError(_ED3E._E000(135313) + obj.Error);
			return;
		}
		if (!(obj.Value is _EA91 obj2))
		{
			Debug.LogError(string.Concat(_ED3E._E000(135313), obj, _ED3E._E000(135340)));
			return;
		}
		obj2.GetSearchState(_E0DE.ID).Value = state;
		IfItemFullySearched(obj2);
	}

	private void _E003(string itemId, _E9F7 gridId, bool silent)
	{
		_ECD9<bool> obj = _E004(itemId, gridId, silent);
		if (obj.Failed)
		{
			Debug.LogError(_ED3E._E000(135391) + obj.Error);
		}
	}

	private _ECD9<bool> _E004(string itemId, _E9F7 gridId, bool silent)
	{
		Debug.Log(_ED3E._E000(135367) + itemId + _ED3E._E000(135407) + gridId.Id + _ED3E._E000(135393) + gridId.ParentId);
		_ECD9<Item> obj = FindItemById(itemId);
		if (obj.Failed)
		{
			return obj.Error;
		}
		if (!(FindContainer(gridId) is _E9EF obj2))
		{
			return new _E000(gridId);
		}
		if (!obj2.Contains(obj.Value))
		{
			return new _E9EF._E004(obj.Value, obj2);
		}
		_EB22 obj3 = new _EB22(obj2, obj2.ContainedItems[obj.Value]);
		obj3.LocationInGrid.isSearched = true;
		_E0DE.SetSearched(obj.Value, obj3, silent);
		if (!(this is ObservedPlayer))
		{
			_E0DE.SetSearchedRaiseEvents(obj.Value, obj3, silent);
		}
		return true;
	}

	protected virtual void ProcessStopSearching(string itemId)
	{
		Debug.Log(_ED3E._E000(135444));
		_E0DE.StopSearching(itemId);
	}

	protected void ProcessNetworkDeath(string aggressorName, string aggressorMainCharacterName, EPlayerSide aggressorSide, EBodyPart part, string weaponName, EMemberCategory memberCategory)
	{
		if (NetworkHealthController != null)
		{
			base.Profile.Stats.Aggressor = new AggressorStats(null, null, aggressorName, aggressorMainCharacterName, aggressorSide, part, weaponName, memberCategory, null);
		}
	}

	protected void ProcessPoisonResourceChange(string id, float value)
	{
		using (_ECC9.BeginSampleWithToken(_ED3E._E000(135479), _ED3E._E000(135497)))
		{
			Item item2 = base.Inventory.Equipment.Slots.Select((Slot slot) => slot.ContainedItem).FirstOrDefault((Item item) => item?.Id == id) ?? base.Inventory.AllPlayerItems.FirstOrDefault((Item item) => item.Id == id);
			if (item2 == null)
			{
				_ECD9<Item> obj = Singleton<GameWorld>.Instance.FindItemById(id);
				if (obj.Failed)
				{
					Debug.LogError(_ED3E._E000(135533) + obj.Error);
					return;
				}
				item2 = obj.Value;
			}
			if (item2.TryGetItemComponent<SideEffectComponent>(out var component))
			{
				component.Value = value;
				item2.RaiseRefreshEvent(refreshIcon: false, checkMagazine: false);
			}
		}
	}

	protected void ProcessArmorPointsChange(string id, float value)
	{
		_preAllocatedArmorComponents.Clear();
		base.Inventory.GetPutOnArmorsNonAlloc(_preAllocatedArmorComponents);
		foreach (ArmorComponent preAllocatedArmorComponent in _preAllocatedArmorComponents)
		{
			if (preAllocatedArmorComponent.Item.Id == id)
			{
				preAllocatedArmorComponent.Repairable.Durability = value;
				preAllocatedArmorComponent.Buff.TryDisableComponent(preAllocatedArmorComponent.Repairable.Durability);
				preAllocatedArmorComponent.Item.RaiseRefreshEvent(refreshIcon: false, checkMagazine: false);
				return;
			}
		}
		_ECD9<Item> obj = Singleton<GameWorld>.Instance.FindItemById(id);
		if (obj.Failed)
		{
			Debug.LogError(_ED3E._E000(135557) + obj.Error);
			return;
		}
		ArmorComponent itemComponent = obj.Value.GetItemComponent<ArmorComponent>();
		if (itemComponent != null)
		{
			itemComponent.Repairable.Durability = value;
			itemComponent.Buff.TryDisableComponent(itemComponent.Repairable.Durability);
			itemComponent.Item.RaiseRefreshEvent(refreshIcon: false, checkMagazine: false);
		}
	}

	protected override void SearchOperationsOnItemRemoved(_EB94 obj)
	{
	}

	protected void ProcessChangeSkillExperience(_E73C? packet, bool silent)
	{
		if (base.Skills == null)
		{
			return;
		}
		while (packet.HasValue)
		{
			_E73C valueOrDefault = packet.GetValueOrDefault();
			try
			{
				ESkillId skillId = (ESkillId)valueOrDefault.SkillId;
				(base.Skills.GetSkill(skillId) ?? throw new Exception(_ED3E._E000(135586) + skillId)).UpdateFromServer(valueOrDefault, silent);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			packet = valueOrDefault.GetNested();
		}
	}

	protected void ProcessChangeMasteringExperience(_E73D? packet, bool silent)
	{
		while (packet.HasValue)
		{
			_E73D valueOrDefault = packet.GetValueOrDefault();
			try
			{
				base.Skills?.ChangeMasteringLevel(valueOrDefault.GroupName, valueOrDefault.Value, silent);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			packet = valueOrDefault.GetNested();
		}
	}
}
