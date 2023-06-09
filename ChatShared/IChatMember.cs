using System;

namespace ChatShared;

[ChatRPC]
public interface IChatMember
{
	void Receive(Message message);

	void ReceiveReplay(Message message, Message replayMessage);

	void Add(ChatRoomMember[] members);

	void Remove(string[] members);

	void SetBanned(string memberId, DateTime toDateTime);

	void SetUnbanned(string memberId);

	void Drop();
}
