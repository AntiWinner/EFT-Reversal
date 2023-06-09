using System;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using ChatShared;
using Comfort.Common;
using Comfort.Communication;
using Comfort.Net;
using UnityEngine;

namespace Communications;

internal sealed class ChatClient : MonoBehaviourSingleton<ChatClient>, IUnityScheduller
{
	private string m__E000;

	private ICommunicator m__E001;

	private ITcpClient m__E002;

	private _E796 m__E003;

	private string m__E004;

	private int m__E005;

	[CompilerGenerated]
	private IChatsSession m__E006;

	[CompilerGenerated]
	private ChatController m__E007;

	[CompilerGenerated]
	private Action m__E008;

	public IChatsSession Session
	{
		[CompilerGenerated]
		get
		{
			return this.m__E006;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E006 = value;
		}
	}

	public ChatController ChatController
	{
		[CompilerGenerated]
		get
		{
			return this.m__E007;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E007 = value;
		}
	}

	public event Action Event
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m__E008;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E008, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m__E008;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E008, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	internal static ChatClient _E000(string versionId, GameObject gameObject = null, CommunicatorFactory communicatorFactory = null)
	{
		if (gameObject == null)
		{
			gameObject = new GameObject(_ED3E._E000(71440));
		}
		ChatClient chatClient = gameObject.AddComponent<ChatClient>();
		chatClient.m__E000 = versionId;
		chatClient.ChatController = ChatController.Create(chatClient);
		if (communicatorFactory == null)
		{
			communicatorFactory = _E455.Create(chatClient._E00A);
		}
		chatClient.m__E001 = communicatorFactory.CreateCommunicator(chatClient._E00A);
		chatClient.m__E002 = TcpClientFactory.CreateClient(chatClient._E006, chatClient);
		chatClient.m__E002.StatusChangedEvent += chatClient._E008;
		return chatClient;
	}

	internal void _E001(_E796 backEndSession)
	{
		this.m__E003 = backEndSession;
		if (!string.IsNullOrEmpty(this.m__E004))
		{
			_E005();
		}
		else
		{
			this.m__E003.GetChatServers(this.m__E000, _E003);
		}
	}

	internal void _E002()
	{
		this.m__E002.Disconnect();
		this.m__E003 = null;
	}

	private void _E003(Result<_E456[]> result)
	{
		if (this.m__E003 == null)
		{
			return;
		}
		if (result.Succeed && result.Value.Length != 0)
		{
			_E456 obj = result.Value.OrderByDescending((_E456 item) => item.DateTime).First();
			this.m__E004 = string.Format(_ED3E._E000(71425), obj.Ip, obj.Port);
			_E005();
		}
		else
		{
			StartCoroutine(_E009());
		}
	}

	public void Reconnect(string ip, int port)
	{
		this.m__E004 = string.Format(_ED3E._E000(71425), ip, port);
		ChatController.SessionStop();
		this.m__E002.Disconnect();
		StartCoroutine(_E004());
	}

	private IEnumerator _E004()
	{
		while (this.m__E002.Status != 0)
		{
			yield return null;
		}
		if (this.m__E003 != null)
		{
			_E005();
		}
	}

	private void _E005()
	{
		Debug.Log(_ED3E._E000(71481) + this.m__E004);
		string phpSessionId = this.m__E003.GetPhpSessionId();
		Status status = this.m__E002.Status;
		if (status == Status.Disconnected || status == Status.ConnectingTimeout)
		{
			this.m__E002.Connect(this.m__E004, phpSessionId, this.m__E000, TimeSpan.FromSeconds(30.0), TimeSpan.FromSeconds(30.0));
		}
		else
		{
			Debug.Log(_ED3E._E000(71459) + status);
		}
	}

	private void _E006(IConnection connection)
	{
		this.m__E005 = 0;
		Debug.Log(_ED3E._E000(71494));
		if (this.m__E003 == null)
		{
			connection.Close(Comfort.Net.ErrorCode.Disconnected);
			Debug.Log(_ED3E._E000(71529));
			return;
		}
		connection.ClosedEvent += delegate(object sender, ConnectionClosedEventArgs args)
		{
			_E007(args);
		};
		this.m__E001.Connection = connection;
		Debug.Log(_ED3E._E000(71565));
		Session = this.m__E001.Get<IChatsSession>();
		ChatController.SessionStart(this.m__E003.SocialNetwork);
	}

	private void _E007(ConnectionStateChangedEventArgs args)
	{
		Debug.Log(_ED3E._E000(71602) + args.ErrorCode);
		ChatController.SessionStop();
		Session = null;
	}

	private void _E008(StatusChangedEventArgs args)
	{
		Debug.Log(string.Format(_ED3E._E000(71631), _E5AD.UtcNow, args.Status, args.ErrorCode, args.StatusOld));
		if (args.Status == Status.ConnectingTimeout)
		{
			this.m__E001.Connection = null;
			StartCoroutine(_E009());
		}
	}

	private IEnumerator _E009()
	{
		if (this.m__E005 < 10)
		{
			this.m__E005++;
		}
		Debug.Log(_ED3E._E000(69695) + this.m__E005 * 30);
		yield return new WaitForSeconds(this.m__E005 * 30);
		this.m__E003?.GetChatServers(this.m__E000, _E003);
	}

	private void _E00A(string error)
	{
		Debug.LogError(error);
		_E002();
	}

	private void FixedUpdate()
	{
		this.m__E008?.Invoke();
	}

	public override void OnDestroy()
	{
		this.m__E003 = null;
		this.m__E002?.Destroy();
		base.OnDestroy();
	}

	Coroutine IUnityScheduller.StartCoroutine(IEnumerator enumerator)
	{
		return StartCoroutine(enumerator);
	}

	[CompilerGenerated]
	private void _E00B(object sender, ConnectionClosedEventArgs args)
	{
		_E007(args);
	}
}
