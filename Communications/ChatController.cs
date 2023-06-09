using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using ChatShared;
using Comfort.Common;
using UnityEngine;

namespace Communications;

[RequireComponent(typeof(ChatClient))]
internal sealed class ChatController : MonoBehaviour, IChatMember
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public Message message;

		internal bool _E000(UpdatableChatMember member)
		{
			return member.Id == message.SenderId;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public Message replayMessage;

		public Message message;

		internal bool _E000(UpdatableChatMember member)
		{
			return member.Id == replayMessage.SenderId;
		}

		internal bool _E001(UpdatableChatMember member)
		{
			return member.Id == message.SenderId;
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public ChatRoomMember member;

		internal UpdatableChatMember _E000(string id)
		{
			UpdatableChatMember updatableChatMember = new UpdatableChatMember(id);
			updatableChatMember.UpdateFromChatMember(member);
			return updatableChatMember;
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public string[] members;

		internal bool _E000(UpdatableChatMember member)
		{
			return members.Contains(member.Id);
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public string memberId;

		internal bool _E000(UpdatableChatMember x)
		{
			return x.Id == memberId;
		}
	}

	[CompilerGenerated]
	private sealed class _E005
	{
		public string memberId;

		internal bool _E000(UpdatableChatMember x)
		{
			return x.Id == memberId;
		}
	}

	private ChatClient m__E000;

	private _E79D m__E001;

	private IChatHandle m__E002;

	private _E466 m__E003;

	[CompilerGenerated]
	private string m__E004;

	public string ChatId
	{
		[CompilerGenerated]
		get
		{
			return this.m__E004;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E004 = value;
		}
	}

	public static ChatController Create(ChatClient chatClient)
	{
		ChatController chatController = chatClient.gameObject.AddComponent<ChatController>();
		chatController.m__E000 = chatClient;
		return chatController;
	}

	public void SessionStart(_E79D socialNetwork)
	{
		this.m__E001 = socialNetwork;
		if (string.IsNullOrEmpty(ChatId))
		{
			this.m__E000.Session.Start(this, _E000);
		}
		else
		{
			this.m__E000.Session.Start(this, ChatId, _E000);
		}
	}

	public void SessionRestart(string chatId, string ip, int port)
	{
		ChatId = chatId;
		this.m__E000.Reconnect(ip, port);
	}

	public void SessionStop()
	{
		if (this.m__E001 != null)
		{
			this.m__E001._E01C = null;
			if (this.m__E003 != null)
			{
				this.m__E001.RemoveDialogue(this.m__E003);
			}
			this.m__E001 = null;
		}
		this.m__E003 = null;
		this.m__E002 = null;
	}

	private void _E000(Result<IChatHandle, ChatInfo> result)
	{
		if (this.m__E001 != null)
		{
			if (result.Succeed)
			{
				_E001(result);
				return;
			}
			Debug.LogError(result.Error);
			ChatId = null;
			this.m__E000.Session.Start(this, _E000);
		}
		else if (result.Succeed)
		{
			Debug.LogError(_ED3E._E000(69667));
		}
	}

	private void _E001(Result<IChatHandle, ChatInfo> result)
	{
		this.m__E001._E01C = _E002;
		this.m__E002 = result.Value0;
		ChatInfo value = result.Value1;
		UpdatableChatMember[] users = value.Members.Select(delegate(ChatRoomMember info)
		{
			UpdatableChatMember updatableChatMember = UpdatableChatMember.FindOrCreate(info.Id, (string id) => new UpdatableChatMember(id));
			updatableChatMember.UpdateFromChatMember(info);
			return updatableChatMember;
		}).ToArray();
		_E45E roomInformation = new _E45E
		{
			type = 9,
			Id = value.Id,
			Name = value.Name,
			@new = 0,
			attachmentsNew = 0,
			pinned = true,
			message = new _E464
			{
				LocalDateTime = _E5AD.Now,
				UtcDateTime = _E5AD.UtcNow,
				Text = "",
				Type = EMessageType.UserMessage,
				_id = Guid.NewGuid().ToString()
			},
			Users = users
		};
		this.m__E003 = this.m__E001.CreateDialogue(roomInformation, select: true);
	}

	private void _E002(_E464 message, _E464 replayToMessage, Action callback)
	{
		if (this.m__E002 != null)
		{
			if (replayToMessage != null)
			{
				this.m__E002.Replay(message.Text, replayToMessage._id);
			}
			else
			{
				this.m__E002.Send(message.Text);
			}
		}
		else
		{
			Debug.LogError(_ED3E._E000(69744));
		}
	}

	void IChatMember.Receive(Message message)
	{
		_E464 message2 = new _E464
		{
			LocalDateTime = _E5AD.Now,
			UtcDateTime = _E5AD.UtcNow,
			Text = message.Text,
			Type = EMessageType.UserMessage,
			_id = message.Id,
			Member = this.m__E003.ActiveUsers.Where((UpdatableChatMember x) => x != null).SingleOrDefault((UpdatableChatMember member) => member.Id == message.SenderId)
		};
		this.m__E001.DisplayMessage(message2, this.m__E003);
	}

	void IChatMember.ReceiveReplay(Message message, Message replayMessage)
	{
		if (this.m__E003 != null)
		{
			_E464 replyTo = new _E464
			{
				LocalDateTime = _E5AD.Now,
				UtcDateTime = _E5AD.UtcNow,
				Text = replayMessage.Text,
				Type = EMessageType.UserMessage,
				Member = this.m__E003.SessionMentionedUsers.Where((UpdatableChatMember x) => x != null).FirstOrDefault((UpdatableChatMember member) => member.Id == replayMessage.SenderId),
				_id = replayMessage.Id
			};
			_E464 message2 = new _E464
			{
				LocalDateTime = _E5AD.Now,
				UtcDateTime = _E5AD.UtcNow,
				Text = message.Text,
				Type = EMessageType.UserMessage,
				_id = message.Id,
				Member = this.m__E003.ActiveUsers.Where((UpdatableChatMember x) => x != null).FirstOrDefault((UpdatableChatMember member) => member.Id == message.SenderId),
				ReplyTo = replyTo
			};
			this.m__E001.DisplayMessage(message2, this.m__E003);
		}
	}

	void IChatMember.Drop()
	{
		SessionStop();
	}

	void IChatMember.Add(ChatRoomMember[] members)
	{
		if (this.m__E003 == null)
		{
			return;
		}
		if (this.m__E003.ActiveUsers == null)
		{
			Debug.LogError(_ED3E._E000(69789));
			return;
		}
		if (this.m__E003.SessionMentionedUsers == null)
		{
			Debug.LogError(_ED3E._E000(69806));
			return;
		}
		if (members == null)
		{
			Debug.LogError(_ED3E._E000(69873));
			return;
		}
		foreach (ChatRoomMember member in members)
		{
			if (member == null)
			{
				Debug.LogError(_ED3E._E000(69900));
				continue;
			}
			UpdatableChatMember updatableChatMember = UpdatableChatMember.FindOrCreate(member.Id, delegate(string id)
			{
				UpdatableChatMember updatableChatMember2 = new UpdatableChatMember(id);
				updatableChatMember2.UpdateFromChatMember(member);
				return updatableChatMember2;
			});
			if (!this.m__E003.ActiveUsers.Contains(updatableChatMember))
			{
				this.m__E003.ActiveUsers.Add(updatableChatMember);
			}
			if (this.m__E003.SessionMentionedUsers.Contains(updatableChatMember))
			{
				this.m__E003.SessionMentionedUsers.Remove(updatableChatMember);
			}
			this.m__E003.SessionMentionedUsers.AddLast(updatableChatMember);
		}
	}

	void IChatMember.Remove(string[] members)
	{
		if (this.m__E003 != null)
		{
			UpdatableChatMember[] array = (from x in this.m__E003.ActiveUsers
				where x != null
				select x into member
				where members.Contains(member.Id)
				select member).ToArray();
			foreach (UpdatableChatMember existingItem in array)
			{
				this.m__E003.ActiveUsers.Remove(existingItem);
			}
		}
	}

	void IChatMember.SetBanned(string memberId, DateTime toDateTime)
	{
		this.m__E001.SetBannedPlayerStatus(this.m__E003.ActiveUsers.Where((UpdatableChatMember x) => x != null).FirstOrDefault((UpdatableChatMember x) => x.Id == memberId), toDateTime);
	}

	void IChatMember.SetUnbanned(string memberId)
	{
		this.m__E001.SetUnbannedPlayerStatus(this.m__E003.ActiveUsers.Where((UpdatableChatMember x) => x != null).FirstOrDefault((UpdatableChatMember x) => x.Id == memberId));
	}

	private void OnDestroy()
	{
		Interlocked.Exchange(ref this.m__E002, null)?.Close(delegate
		{
		});
	}
}
