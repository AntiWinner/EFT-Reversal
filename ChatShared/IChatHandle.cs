using System;

namespace ChatShared;

[ChatRPC]
public interface IChatHandle
{
	void Send(string text);

	void Replay(string text, string replayMessageId);

	void Close(Action callback);
}
