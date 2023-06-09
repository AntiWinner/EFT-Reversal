using System;
using Comfort.Common;
using Diz.Binding;
using EFT;
using EFT.UI.Ragfair;

namespace ChatShared;

[Serializable]
public sealed class UpdatableChatMember : IUpdatable<UpdatableChatMember>
{
	public class UpdatableChatMemberInfo
	{
		public string Nickname;

		public EChatMemberSide Side;

		public int Level;

		public EMemberCategory MemberCategory;

		public bool Ignored;

		public bool Banned;

		public bool IsTrader
		{
			get
			{
				if (Side != EChatMemberSide.Trader)
				{
					return MemberCategory == EMemberCategory.Trader;
				}
				return true;
			}
		}
	}

	private string _id;

	public string AccountId;

	private UpdatableChatMemberInfo _info = new UpdatableChatMemberInfo();

	[NonSerialized]
	public _ECEC OnIgnoreStatusChanged = new _ECEC();

	[NonSerialized]
	public _ECED<bool> OnPlayerBanStatusChanged = new _ECED<bool>();

	public string Id => _id;

	public UpdatableChatMemberInfo Info => _info;

	public bool HasNickname => !string.IsNullOrEmpty(Info.Nickname);

	public string LocalizedNickname
	{
		get
		{
			if (Info.Side == EChatMemberSide.Trader || Info.MemberCategory == EMemberCategory.Trader)
			{
				return Info.Nickname.Localized();
			}
			return Singleton<_E7DE>.Instance.Game.Controller.GetCorrectedProfileNickname(Id, Info.Nickname);
		}
	}

	public static UpdatableChatMember FindOrCreate(string id, Func<string, UpdatableChatMember> constructor)
	{
		if (!_E79D.AllMembers.TryGetValue(id, out var value))
		{
			value = constructor(id);
			_E79D.AllMembers[id] = value;
		}
		return value;
	}

	public UpdatableChatMember(string id)
	{
		_id = id;
	}

	public void UpdateFromAnotherItem(UpdatableChatMember other)
	{
		if (other._info != null)
		{
			AccountId = other.AccountId;
			_info.Level = other._info.Level;
			_info.Nickname = other._info.Nickname;
			_info.Side = other._info.Side;
			_info.MemberCategory = other._info.MemberCategory;
			SetIgnoreStatus(other._info.Ignored);
			SetBanStatus(other._info.Banned);
		}
	}

	public void UpdateFromChatMember(ChatRoomMember member)
	{
		if (member.Aid != null)
		{
			AccountId = member.Aid;
		}
		_info.Level = member?.Info?.Level ?? 0;
		_info.Nickname = member?.Info?.Nickname ?? _ED3E._E000(70352);
		_info.MemberCategory = member?.Info?.MemberCategory ?? EMemberCategory.Trader;
		_info.Side = member?.Info?.Side ?? EChatMemberSide.Trader;
		SetBanStatus(member?.Info?.GlobalMuteTime.ToLocalTime() > _E5AD.Now);
	}

	public void UpdateFromMerchant(Offer._E000 merchant)
	{
		_info.Nickname = merchant.CorrectedNickname;
		_info.MemberCategory = merchant.MemberType;
	}

	public void UpdateFromTrader(_E5CB.TraderSettings traderSettings)
	{
		_info.Nickname = traderSettings.Nickname;
		_info.Side = EChatMemberSide.Trader;
		_info.MemberCategory = EMemberCategory.Trader;
	}

	public void SetIgnoreStatus(bool status)
	{
		if (status != _info.Ignored)
		{
			_info.Ignored = status;
			OnIgnoreStatusChanged?.Invoke();
		}
	}

	public void SetBanStatus(bool status)
	{
		if (status != _info.Banned)
		{
			_info.Banned = status;
			OnPlayerBanStatusChanged?.Invoke(status);
		}
	}

	public void SetNickname(string nickname)
	{
		_info.Nickname = nickname;
	}

	public void SetCategory(EMemberCategory category)
	{
		_info.MemberCategory = category;
	}

	public bool Compare(UpdatableChatMember other)
	{
		return _id == other._id;
	}
}
