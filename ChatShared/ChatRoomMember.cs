using System;
using EFT;
using Newtonsoft.Json;

namespace ChatShared;

public class ChatRoomMember
{
	public class MemberInfo
	{
		public string Nickname;

		public EChatMemberSide Side;

		public int Level;

		public EMemberCategory MemberCategory;

		public DateTime GlobalMuteTime;
	}

	[JsonProperty("_id")]
	public string Id;

	[JsonProperty("aid")]
	public string Aid;

	public MemberInfo Info;
}
