using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Diz.Utils;
using Newtonsoft.Json;
using UnityEngine;
using WebSocketSharp;

internal class WsWebSender : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public WsWebSender _003C_003E4__this;

		public DateTime startTime;

		internal void _E000()
		{
			_003C_003E4__this.m__E010.Connect();
		}

		internal bool _E001()
		{
			return (DateTime.Now - startTime).TotalSeconds < 30.0;
		}
	}

	[CompilerGenerated]
	private sealed class _E00A
	{
		public WsWebSender _003C_003E4__this;

		public _E2DF reqToSend;

		public Func<bool> _003C_003E9__3;

		internal async Task<bool> _E000(_E2DF req)
		{
			bool flag = false;
			try
			{
				await AsyncWorker.RunOnBackgroundThread(delegate
				{
					_003C_003E4__this.m__E010.Send(reqToSend.RequestJsonText);
				});
			}
			catch (Exception exc)
			{
				flag = true;
				req.FinishWithException(exc);
			}
			return !flag;
		}

		internal void _E001()
		{
			_003C_003E4__this.m__E010.Send(reqToSend.RequestJsonText);
		}

		internal bool _E002()
		{
			return !reqToSend.CheckTimedOut();
		}
	}

	[CompilerGenerated]
	private sealed class _E00C
	{
		public string msgText;

		public WsWebSender _003C_003E4__this;

		internal _E2DD _E000()
		{
			return msgText.ParseJsonTo<_E2DD>(Array.Empty<JsonConverter>());
		}
	}

	[CompilerGenerated]
	private sealed class _E00D
	{
		public _E2DD response;

		public _E00C CS_0024_003C_003E8__locals1;

		internal void _E000()
		{
			CS_0024_003C_003E8__locals1._003C_003E4__this.m__E010.Send(response.id);
		}
	}

	private const int m__E000 = 1000;

	private const int m__E001 = 2000;

	private const float m__E002 = 1f;

	private const float m__E003 = 1f;

	private readonly float[] m__E004 = new float[7] { 0.2f, 0.4f, 0.8f, 1.6f, 3.2f, 6.4f, 12.8f };

	private const float m__E005 = 30f;

	private WsConnectionStatus m__E006;

	private CloseStatusCode m__E007;

	private CancellationTokenSource m__E008;

	private bool m__E009;

	private DateTime m__E00A;

	private float m__E00B = 1f;

	private float[] m__E00C;

	private bool m__E00D;

	private int m__E00E;

	private float m__E00F;

	private WebSocket m__E010;

	private readonly Stack<_E2DF> m__E011 = new Stack<_E2DF>();

	private readonly _E2E0 m__E012 = new _E2E0();

	private readonly _E2E0 m__E013 = new _E2E0();

	private readonly Dictionary<string, _E2DF> m__E014 = new Dictionary<string, _E2DF>();

	private readonly List<string> m__E015 = new List<string>();

	private readonly float[] m__E016 = new float[6] { 1f, 1f, 1f, 3f, 5f, 10f };

	public WsConnectionStatus ConnectionStatus => this.m__E006;

	public CloseStatusCode CloseStatusCode => this.m__E007;

	public bool DebugLastPingResult => this.m__E009;

	public bool DebugIsPingResultOutdated => (DateTime.Now - this.m__E00A).TotalSeconds >= (double)this.m__E00B;

	public int DebugCurrRunningRequestsCount => this.m__E012.Count + this.m__E013.Count + this.m__E014.Count;

	public int DebugCurrSerializingReqs => this.m__E012.Count;

	public int DebugCurrSendingReqs => this.m__E013.Count;

	public int DebugCurrWaitingResponseReqs => this.m__E014.Count;

	public float DebugCurrReconnectIntervalTimer => this.m__E00F;

	public bool DebugReqConfirmInProgress => this.m__E00D;

	public async Task EstablishConnection(_E2DE connParams)
	{
		if (this.m__E010 == null)
		{
			this.m__E010 = new WebSocket(connParams.EndpointUrl);
			this.m__E010.Log.Level = LogLevel.Trace;
			WebSocketSharp.Logger log = this.m__E010.Log;
			log.Output = (Action<LogData, string>)Delegate.Combine(log.Output, new Action<LogData, string>(_E012));
			this.m__E010.OnOpen += _E00E;
			this.m__E010.OnClose += _E00F;
			this.m__E010.OnMessage += _E010;
			this.m__E010.OnError += _E011;
			this.m__E010.SslConfiguration.EnabledSslProtocols = SslProtocols.Tls12;
			this.m__E010.SetToken(connParams.TokenB64, preAuth: true);
			if (connParams.KeepAliveInterval == 0f)
			{
				_E2D0.Logger.LogWarn(string.Format(_ED3E._E000(54374), 1f));
				this.m__E00B = 1f;
			}
			else
			{
				this.m__E00B = connParams.KeepAliveInterval;
			}
			if (connParams.ConfirmationTimeouts == null || connParams.ConfirmationTimeouts.Length == 0)
			{
				_E2D0.Logger.LogWarn(string.Format(_ED3E._E000(54433), this.m__E004));
				this.m__E00C = this.m__E004;
			}
			else
			{
				this.m__E00C = connParams.ConfirmationTimeouts;
			}
			DateTime startTime = DateTime.Now;
			this.m__E008 = new CancellationTokenSource();
			_E001(this.m__E008.Token);
			_E000(this.m__E008.Token);
			_E002(this.m__E008.Token);
			_E003(this.m__E008.Token);
			_E004(this.m__E008.Token);
			await AsyncWorker.RunOnBackgroundThread(delegate
			{
				this.m__E010.Connect();
			});
			await _E007(() => (DateTime.Now - startTime).TotalSeconds < 30.0, this.m__E008.Token);
			if ((DateTime.Now - startTime).TotalSeconds >= 30.0)
			{
				this.m__E008.Cancel();
				throw new _E2D9(_ED3E._E000(54540));
			}
		}
	}

	public async Task CloseConnection()
	{
		if (ConnectionStatus == WsConnectionStatus.InitialNotConnected)
		{
			_E2D0.Logger.LogError(_ED3E._E000(54577));
			return;
		}
		this.m__E008.Cancel();
		this.m__E008.Dispose();
		if (ConnectionStatus == WsConnectionStatus.Connected)
		{
			this.m__E010.Close();
		}
		WebSocketSharp.Logger log = this.m__E010.Log;
		log.Output = (Action<LogData, string>)Delegate.Remove(log.Output, new Action<LogData, string>(_E012));
		this.m__E010.OnOpen -= _E00E;
		this.m__E010.OnClose -= _E00F;
		this.m__E010.OnMessage -= _E010;
		this.m__E010.OnError -= _E011;
		this.m__E010 = null;
		this.m__E00D = false;
	}

	public async Task<_E2DD> SendRequest(_E2DC requestJson, int timeoutSeconds)
	{
		if (requestJson.id == _ED3E._E000(27314))
		{
			throw new _E2D9(_ED3E._E000(54653));
		}
		_E2DF obj = _E009();
		obj.Initialize(requestJson, timeoutSeconds);
		this.m__E012.Enqueue(obj);
		while (obj.Status == _E2DF.WsRequestStatus.InProgress)
		{
			await Task.Yield();
		}
		switch (obj.Status)
		{
		case _E2DF.WsRequestStatus.FinishiedWithResponse:
		{
			_E2DD result = obj.ResultResponse ?? throw new _E2D9(_ED3E._E000(54693) + requestJson.id + _ED3E._E000(54766));
			_E00A(obj);
			return result;
		}
		case _E2DF.WsRequestStatus.FinishedWithException:
			_E00A(obj);
			throw obj.ResultException;
		default:
			_E00A(obj);
			throw new _E2D9(_ED3E._E000(54761));
		}
	}

	public async Task SendNotification(_E2DC requestJson)
	{
		throw new NotImplementedException();
	}

	private async Task _E000(CancellationToken cancellationToken)
	{
		while (true)
		{
			cancellationToken.ThrowIfCancellationRequested();
			if (ConnectionStatus == WsConnectionStatus.Connected && (DateTime.Now - this.m__E00A).TotalSeconds >= (double)this.m__E00B)
			{
				await _E00C();
			}
			await Task.Yield();
		}
	}

	private async Task _E001(CancellationToken cancellationToken)
	{
		while (true)
		{
			cancellationToken.ThrowIfCancellationRequested();
			if (ConnectionStatus == WsConnectionStatus.AwaitingReconnect)
			{
				_E2D0.Logger.LogInfo(_ED3E._E000(54787) + this.m__E010.Url.ToString() + _ED3E._E000(54866));
				try
				{
					await AsyncWorker.RunOnBackgroundThread(delegate
					{
						this.m__E010.Connect();
					});
					if (await _E00C())
					{
						_E2D0.Logger.LogInfo(_ED3E._E000(54856));
						_E008();
					}
					else
					{
						float num = this.m__E016[this.m__E00E];
						this.m__E00E++;
						if (this.m__E00E >= this.m__E016.Length)
						{
							this.m__E00E = this.m__E016.Length - 1;
						}
						_E2D0.Logger.LogInfo(string.Format(_ED3E._E000(54887), num));
						this.m__E00F = num;
						while (this.m__E00F > 0f)
						{
							await Task.Yield();
							this.m__E00F -= Time.unscaledDeltaTime;
						}
						this.m__E00F = 0f;
					}
				}
				catch (Exception e)
				{
					_E2D0.Logger.LogException(e);
				}
			}
			await Task.Yield();
		}
	}

	private async Task _E002(CancellationToken cancellationToken)
	{
		while (true)
		{
			cancellationToken.ThrowIfCancellationRequested();
			while (this.m__E012.Count > 0)
			{
				this.m__E012.FinishTimedOutRequests();
				_E2DF obj = this.m__E012.Dequeue();
				await obj.SerializeDataToTextAsync();
				this.m__E013.Enqueue(obj);
			}
			await Task.Yield();
		}
	}

	private async Task _E003(CancellationToken cancellationToken)
	{
		while (true)
		{
			cancellationToken.ThrowIfCancellationRequested();
			while (this.m__E013.Count > 0)
			{
				this.m__E013.FinishTimedOutRequests();
				await _E007(delegate
				{
					this.m__E013.FinishTimedOutRequests();
					return this.m__E013.Count > 0;
				}, cancellationToken);
				cancellationToken.ThrowIfCancellationRequested();
				if (this.m__E013.Count > 0)
				{
					_E2DF reqToSend = this.m__E013.Dequeue();
					this.m__E00D = true;
					await _E005(reqToSend, cancellationToken);
					this.m__E00D = false;
				}
			}
			await Task.Yield();
		}
	}

	private async Task _E004(CancellationToken cancellationToken)
	{
		while (true)
		{
			cancellationToken.ThrowIfCancellationRequested();
			foreach (KeyValuePair<string, _E2DF> item in this.m__E014)
			{
				string key = item.Key;
				_E2DF value = item.Value;
				if (value.CheckTimedOut())
				{
					value.FinishWithTimeout();
					this.m__E015.Add(key);
				}
			}
			foreach (string item2 in this.m__E015)
			{
				this.m__E014.Remove(item2);
			}
			this.m__E015.Clear();
			await Task.Yield();
		}
	}

	private async Task _E005(_E2DF reqToSend, CancellationToken cancellationToken)
	{
		_E00A CS_0024_003C_003E8__locals0 = new _E00A();
		CS_0024_003C_003E8__locals0._003C_003E4__this = this;
		CS_0024_003C_003E8__locals0.reqToSend = reqToSend;
		if (!(await CS_0024_003C_003E8__locals0._E000(CS_0024_003C_003E8__locals0.reqToSend)))
		{
			return;
		}
		cancellationToken.ThrowIfCancellationRequested();
		int num = 0;
		this.m__E014.Add(CS_0024_003C_003E8__locals0.reqToSend.RequestId, CS_0024_003C_003E8__locals0.reqToSend);
		while (!_E015(CS_0024_003C_003E8__locals0.reqToSend))
		{
			DateTime now = DateTime.Now;
			float num2 = this.m__E00C[num];
			while (!_E015(CS_0024_003C_003E8__locals0.reqToSend) && (DateTime.Now - now).TotalSeconds < (double)num2)
			{
				cancellationToken.ThrowIfCancellationRequested();
				if (CS_0024_003C_003E8__locals0.reqToSend.CheckTimedOut())
				{
					CS_0024_003C_003E8__locals0.reqToSend.FinishWithTimeout();
					this.m__E014.Remove(CS_0024_003C_003E8__locals0.reqToSend.RequestId);
					return;
				}
				await Task.Yield();
			}
			if (!_E015(CS_0024_003C_003E8__locals0.reqToSend))
			{
				await _E007(() => !CS_0024_003C_003E8__locals0.reqToSend.CheckTimedOut(), cancellationToken);
				if (CS_0024_003C_003E8__locals0.reqToSend.CheckTimedOut())
				{
					CS_0024_003C_003E8__locals0.reqToSend.FinishWithTimeout();
					this.m__E014.Remove(CS_0024_003C_003E8__locals0.reqToSend.RequestId);
					break;
				}
				if (!(await CS_0024_003C_003E8__locals0._E000(CS_0024_003C_003E8__locals0.reqToSend)))
				{
					this.m__E014.Remove(CS_0024_003C_003E8__locals0.reqToSend.RequestId);
					break;
				}
				if (num < this.m__E00C.Length - 1)
				{
					num++;
				}
			}
		}
	}

	private async void _E006(object sender, MessageEventArgs e)
	{
		_ = 1;
		try
		{
			string msgText = e.Data;
			_E2D0.Logger.LogInfo(_ED3E._E000(54958) + msgText);
			if (int.TryParse(msgText, out var _))
			{
				if (this.m__E014.TryGetValue(msgText, out var value))
				{
					value.ConfirmReceiveFromServer();
				}
				return;
			}
			_E2DD response = await AsyncWorker.RunOnBackgroundThread(() => msgText.ParseJsonTo<_E2DD>(Array.Empty<JsonConverter>()));
			if (response.id == _ED3E._E000(27314))
			{
				throw new NotImplementedException(_ED3E._E000(54986));
			}
			if (this.m__E014.TryGetValue(response.id, out var value2))
			{
				value2.FinishWithResponse(response);
				this.m__E014.Remove(response.id);
			}
			else
			{
				_E2D0.Logger.LogError(_ED3E._E000(55103) + response.id + _ED3E._E000(55115));
			}
			await AsyncWorker.RunOnBackgroundThread(delegate
			{
				this.m__E010.Send(response.id);
			});
		}
		catch (Exception e2)
		{
			_E2D0.Logger.LogException(e2);
		}
	}

	private async Task _E007(Func<bool> updateContinueCondition, CancellationToken cancellationToken)
	{
		if (ConnectionStatus == WsConnectionStatus.Connected)
		{
			await _E00C();
		}
		else
		{
			_E008();
		}
		while (true)
		{
			bool flag = ConnectionStatus != WsConnectionStatus.Connected;
			if (!flag)
			{
				flag = !(await _E00D(1f));
			}
			if (!flag)
			{
				break;
			}
			if (ConnectionStatus != WsConnectionStatus.Connected)
			{
				while (ConnectionStatus == WsConnectionStatus.AwaitingReconnect)
				{
					cancellationToken.ThrowIfCancellationRequested();
					if (!updateContinueCondition())
					{
						return;
					}
					await Task.Yield();
				}
				if (ConnectionStatus != WsConnectionStatus.Connected)
				{
					throw new _E2D9(_ED3E._E000(55109));
				}
			}
			else
			{
				cancellationToken.ThrowIfCancellationRequested();
				if (!updateContinueCondition())
				{
					break;
				}
				await Task.Yield();
			}
		}
	}

	private void _E008()
	{
		this.m__E00F = 0f;
		this.m__E00E = 0;
	}

	private _E2DF _E009()
	{
		if (this.m__E011.Count == 0)
		{
			return new _E2DF();
		}
		return this.m__E011.Pop();
	}

	private void _E00A(_E2DF req)
	{
		req.Dispose();
		this.m__E011.Push(req);
	}

	private bool _E00B()
	{
		try
		{
			if (this.m__E010 == null)
			{
				this.m__E009 = false;
			}
			else
			{
				this.m__E009 = this.m__E010.Ping();
			}
		}
		catch (Exception e)
		{
			_E2D0.Logger.LogException(e);
			this.m__E009 = false;
		}
		finally
		{
			this.m__E00A = DateTime.Now;
		}
		return this.m__E009;
	}

	private async Task<bool> _E00C()
	{
		return await AsyncWorker.RunOnBackgroundThread(() => _E00B());
	}

	private async Task<bool> _E00D(float outdateIntervalSec)
	{
		if ((DateTime.Now - this.m__E00A).TotalSeconds > (double)outdateIntervalSec)
		{
			await _E00C();
		}
		return this.m__E009;
	}

	private void _E00E(object sender, EventArgs e)
	{
		this.m__E006 = WsConnectionStatus.Connected;
		_E2D0.Logger.LogInfo(_ED3E._E000(11166) + this.m__E010.Url.ToString() + _ED3E._E000(11164));
	}

	private void _E00F(object sender, CloseEventArgs e)
	{
		this.m__E007 = (CloseStatusCode)e.Code;
		this.m__E006 = (e.WasClean ? WsConnectionStatus.Disconnected : WsConnectionStatus.AwaitingReconnect);
		if (e.WasClean)
		{
			_E2D0.Logger.LogInfo(_ED3E._E000(11191) + this.m__E010.Url.ToString() + _ED3E._E000(11204) + string.Format(_ED3E._E000(11200), e.Code, (CloseStatusCode)e.Code, e.Reason, e.WasClean));
		}
		else
		{
			_E2D0.Logger.LogError(_ED3E._E000(54292) + this.m__E010.Url.ToString() + _ED3E._E000(11204) + string.Format(_ED3E._E000(11200), e.Code, (CloseStatusCode)e.Code, e.Reason, e.WasClean));
		}
	}

	private void _E010(object sender, MessageEventArgs e)
	{
		try
		{
			_E006(sender, e);
		}
		catch (Exception e2)
		{
			_E2D0.Logger.LogException(e2);
		}
	}

	private void _E011(object sender, ErrorEventArgs e)
	{
		_E2D0.Logger.LogError(_ED3E._E000(54310) + e.ToString());
	}

	private void _E012(LogData data, string message)
	{
		_E2D0.Logger.LogTrace(_ED3E._E000(54343) + data);
	}

	public async Task<bool> DebugPingManuallyAsync()
	{
		return await _E00C();
	}

	[CompilerGenerated]
	private void _E013()
	{
		this.m__E010.Connect();
	}

	[CompilerGenerated]
	private bool _E014()
	{
		this.m__E013.FinishTimedOutRequests();
		return this.m__E013.Count > 0;
	}

	[CompilerGenerated]
	internal static bool _E015(_E2DF req)
	{
		if (!req.ReceivingConfirmed)
		{
			return req.IsFinished;
		}
		return true;
	}

	[CompilerGenerated]
	private bool _E016()
	{
		return _E00B();
	}
}
