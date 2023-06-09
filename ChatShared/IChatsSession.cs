using System;
using Comfort.Common;

namespace ChatShared;

[ChatRPC]
public interface IChatsSession
{
	void Start(IChatMember member, Callback<IChatHandle, ChatInfo> callback);

	void Start(IChatMember member, string chatId, Callback<IChatHandle, ChatInfo> callback);

	void Close(Action callback);

	void Ban(string profileId, TimeSpan timeSpan, string description, Callback<DateTime> callback);

	void Unban(string profileId, string description, Callback callback);
}
