#define BATTLEYE_ANTICHEAT
using System;
using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using BattlEye;
using Dissonance.Networking;
using UnityEngine;
using UnityEngine.Networking;

namespace EFT;

[_E2E2(10010)]
public sealed class ChannelCombined : MonoBehaviour
{
	private sealed class _E000 : IDisposable
	{
		private readonly ChannelCombined m__E000;

		private ArrayPool<byte> m__E001;

		private byte[] _E002;

		private int _E003;

		internal bool _E004 => _E003 > 0;

		internal _E000(ChannelCombined owner, ArrayPool<byte> pool = null)
		{
			this.m__E000 = owner;
			this.m__E001 = pool;
			_E002 = ((pool != null) ? pool.Rent(8388608) : new byte[8388608]);
		}

		public void Dispose()
		{
			ArrayPool<byte> arrayPool = this.m__E001;
			this.m__E001 = null;
			arrayPool?.Return(_E002);
			_E002 = null;
		}

		internal bool _E000(in ArraySegment<byte> segment)
		{
			int count = segment.Count;
			if (_E002.Length - _E003 - count - 5 <= 0)
			{
				return false;
			}
			_E002[_E003++] = (byte)((uint)count & 0xFFu);
			_E002[_E003++] = (byte)((uint)(count >> 8) & 0xFFu);
			_E002[_E003++] = (byte)((uint)(count >> 16) & 0xFFu);
			_E002[_E003++] = (byte)((uint)(count >> 24) & 0xFFu);
			Buffer.BlockCopy(segment.Array, segment.Offset, _E002, _E003, count);
			_E003 += count;
			return true;
		}

		internal bool _E001()
		{
			if (_E003 == 0)
			{
				return false;
			}
			int num = 0;
			int num2 = _E002[num++] | (_E002[num++] << 8) | (_E002[num++] << 16) | (_E002[num++] << 24);
			ArraySegment<byte> segment = new ArraySegment<byte>(_E002, num, num2);
			this.m__E000._E007(in segment);
			if (!this.m__E000._E00A(PacketPriority.Critical))
			{
				return false;
			}
			num += num2;
			_E003 -= num;
			if (_E003 > 0)
			{
				Buffer.BlockCopy(_E002, num, _E002, 0, _E003);
			}
			return true;
		}
	}

	public sealed class _E001 : _E315
	{
		private const string _E011 = "channels";

		public _E001()
			: base(_ED3E._E000(158302), LoggerMode.Add)
		{
		}

		public void PlayerCreated(string nickname, long connectionId, byte channelId)
		{
			LogInfo(_ED3E._E000(158295), nickname, connectionId, channelId);
		}

		internal void _E000(string nickname, _E6E5.EOperationType operationType, PacketPriority priority, byte channelId, int observersCount)
		{
			LogInfo(_ED3E._E000(158358), nickname, operationType, priority, channelId, observersCount);
		}

		internal void _E001(string nickname, _E6E5.EOperationType operationType, PacketPriority priority, byte channelId, long connectionId)
		{
			LogInfo(_ED3E._E000(158463), nickname, operationType, priority, connectionId, channelId);
		}

		internal void _E002(_E6E5.EOperationType operationType, PacketPriority priority, byte channelId, long connectionId)
		{
			LogInfo(_ED3E._E000(158504), operationType, priority, connectionId, channelId);
		}

		internal new void _E003(string message, _E6E5.EOperationType operationType, byte channelId, long connectionId)
		{
			LogInfo(_ED3E._E000(158620), message, operationType, connectionId, channelId);
		}
	}

	public delegate bool _E002(byte channelId, _E524 reader, int rtt);

	public delegate void _E003(_E6AB networkQualityParam, EDisconnectionCode disconnectionCode, float time, float period);

	public const byte MsgTypeChannelData = 170;

	public const int DEFERRED_BUFFER_SIZE = 8388608;

	private _E003 m__E000;

	private _E6AC m__E001;

	private _E6AC m__E002;

	private bool m__E003;

	private _E001 m__E004;

	private static int m__E005;

	private int m__E006;

	[CompilerGenerated]
	private string m__E007;

	private NetworkConnection m__E008;

	private _E002 m__E009;

	private readonly MemoryStream m__E00A = new MemoryStream();

	private readonly _E518 m__E00B = new _E518(new byte[2400]);

	private bool m__E00C;

	private _E000 m__E00D;

	private readonly short[] m__E00E = new short[255];

	private bool m__E00F;

	private bool m__E010;

	private byte[] _E011;

	private int _E012;

	private static readonly byte[] _E013 = new byte[20000];

	[CompilerGenerated]
	private bool _E014;

	public bool NetworkQualityWatcherStarted
	{
		get
		{
			return this.m__E003;
		}
		set
		{
			this.m__E003 = value;
			if (this.m__E001 != null)
			{
				this.m__E001._E007 = value;
			}
			if (this.m__E002 != null)
			{
				this.m__E002._E007 = value;
			}
		}
	}

	internal string _E015
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

	internal bool _E016
	{
		[CompilerGenerated]
		get
		{
			return _E014;
		}
		[CompilerGenerated]
		set
		{
			_E014 = value;
		}
	}

	internal static ChannelCombined _E000(GameObject gameObject, string profileId, NetworkConnection connection, _E002 receiveAction, _E001 logger, bool encryptionEnabled, bool decryptionEnabled, byte[] openEncryptionKey, int openEncryptionKeyLength, _E6AA networkQualityParams = null, _E003 kickAction = null, ArrayPool<byte> pool = null)
	{
		ChannelCombined channelCombined = gameObject.AddComponent<ChannelCombined>();
		ChannelCombined.m__E005++;
		channelCombined.m__E006 = ChannelCombined.m__E005;
		channelCombined.m__E004 = logger;
		channelCombined.m__E00D = new _E000(channelCombined, pool);
		channelCombined.m__E00F = encryptionEnabled;
		channelCombined.m__E010 = decryptionEnabled;
		channelCombined.m__E010 = true;
		channelCombined.m__E00F = false;
		logger.LogInfo(_ED3E._E000(159473), _ED3E._E000(159458), channelCombined.m__E00F, _ED3E._E000(159501), channelCombined.m__E010);
		if (networkQualityParams != null && kickAction != null)
		{
			channelCombined.m__E000 = kickAction;
			channelCombined.m__E001 = new _E6AC(networkQualityParams.Ping);
			channelCombined.m__E002 = new _E6AC(networkQualityParams.Loss);
		}
		channelCombined._E015 = profileId ?? "";
		channelCombined.m__E008 = connection;
		channelCombined.m__E009 = receiveAction;
		for (int i = 0; i < channelCombined.m__E00E.Length; i++)
		{
			channelCombined.m__E00E[i] = -1;
		}
		return channelCombined;
	}

	private void OnDestroy()
	{
		this.m__E00D.Dispose();
	}

	internal void _E001()
	{
		_E016 = true;
	}

	internal void _E002(byte channelIndex)
	{
		var (b, b2) = _E004(channelIndex);
		this.m__E00E[b] = 0;
		this.m__E00E[b2] = 0;
		this.m__E004.LogInfo(string.Format(_ED3E._E000(159544), b, b2));
	}

	internal void _E003(byte channelIndex)
	{
		(byte, byte) tuple = _E004(channelIndex);
		byte item = tuple.Item1;
		byte item2 = tuple.Item2;
		short num = this.m__E00E[item];
		short num2 = this.m__E00E[item2];
		this.m__E00E[item] = -1;
		this.m__E00E[item2] = -1;
		this.m__E004.LogInfo(string.Format(_ED3E._E000(159563), item, item2, num, num2));
	}

	private static (byte, byte) _E004(byte channelIndex)
	{
		byte item = channelIndex;
		byte item2 = channelIndex;
		if ((int)channelIndex % 2 == 0)
		{
			item = (byte)(channelIndex - 1);
		}
		else
		{
			item2 = (byte)(channelIndex + 1);
		}
		return (item, item2);
	}

	internal void _E005(byte channelIndex, in ArraySegment<byte> data, PacketPriority priority)
	{
		if (data.Offset != 0)
		{
			this.m__E004.LogError(string.Format(_ED3E._E000(159655), data.Offset));
		}
		ArraySegment<byte> segment = Encrypt(in data);
		if (segment.Count <= 0 || segment.Count >= 1200)
		{
			this.m__E004.LogError(string.Format(_ED3E._E000(159680), segment.Count, this.m__E006, channelIndex, priority, this.m__E008._E000()));
			return;
		}
		if (!this.m__E008.isConnected)
		{
			this.m__E008.Close(_ED3E._E000(157750), isError: false);
			_E016 = true;
		}
		if (!_E016 && (!this.m__E00C || priority != 0))
		{
			_E009(channelIndex, in segment, priority);
		}
	}

	public void Encrypt(byte[] sourcePacket, int sourcePacketOffset, int sourcePacketLength, byte[] encryptedPacket, int encryptedPacketOffset, ref int cryptoBufferLength)
	{
		BEClient.EncryptPacket(sourcePacket, sourcePacketOffset, sourcePacketLength, encryptedPacket, encryptedPacketOffset, ref cryptoBufferLength);
	}

	public ArraySegment<byte> Encrypt(in ArraySegment<byte> segment)
	{
		if (this.m__E00F)
		{
			int cryptoBufferLength = _E013.Length;
			byte[] encryptedPacket = _E013;
			byte[] array = segment.Array;
			int offset = segment.Offset;
			int count = segment.Count;
			int encryptedPacketOffset = 0;
			Encrypt(array, offset, count, encryptedPacket, encryptedPacketOffset, ref cryptoBufferLength);
			return new ArraySegment<byte>(_E013, 0, cryptoBufferLength);
		}
		return segment;
	}

	private void _E006(byte channelId, in ArraySegment<byte> segment)
	{
		this.m__E00A.Flush();
		this.m__E00A.SetLength(0L);
		ushort num = (ushort)this.m__E00A.Length;
		this.m__E00A.WriteByte(0);
		this.m__E00A.WriteByte(0);
		this.m__E00A.WriteByte(170);
		this.m__E00A.WriteByte(0);
		this.m__E00A.WriteByte(channelId);
		_E00E(this.m__E00A, (ushort)segment.Count);
		this.m__E00A.Write(segment.Array, segment.Offset, segment.Count);
		this.m__E00A.Flush();
		byte[] buffer = this.m__E00A.GetBuffer();
		ushort num2 = (ushort)(this.m__E00A.Length - num - 4);
		buffer[num] = (byte)(num2 & 0xFFu);
		buffer[num + 1] = (byte)((uint)(num2 >> 8) & 0xFFu);
	}

	private void _E007(in ArraySegment<byte> segment)
	{
		this.m__E00A.Flush();
		this.m__E00A.SetLength(0L);
		this.m__E00A.Write(segment.Array, segment.Offset, segment.Count);
		this.m__E00A.Flush();
	}

	private void _E008()
	{
		string text = string.Format(_ED3E._E000(157739), this.m__E006, this.m__E008.hostId, this.m__E008.connectionId, this.m__E008.isConnected);
		this.m__E004.LogError(text);
		this.m__E008.Close(text, isError: true);
		_E016 = true;
	}

	private void _E009(byte channelIndex, in ArraySegment<byte> segment, PacketPriority priority)
	{
		byte b = ((priority == PacketPriority.Critical) ? channelIndex : ((byte)(channelIndex + 1)));
		if (this.m__E00E[b] >= 0 && priority == PacketPriority.Low)
		{
			this.m__E00E[b] = (short)(this.m__E00E[b] + 1);
			return;
		}
		if (this.m__E00D._E004)
		{
			if (priority == PacketPriority.Low)
			{
				return;
			}
			_E006(b, in segment);
			ArraySegment<byte> segment2 = new ArraySegment<byte>(this.m__E00A.GetBuffer(), 0, (int)this.m__E00A.Length);
			if (!this.m__E00D._E000(in segment2))
			{
				_E008();
				return;
			}
			int num = 0;
			while (this.m__E00D._E001())
			{
				num++;
				if (num >= 100)
				{
					break;
				}
			}
			return;
		}
		_E006(b, in segment);
		if (!_E00A(priority) && priority != 0)
		{
			ArraySegment<byte> segment3 = new ArraySegment<byte>(this.m__E00A.GetBuffer(), 0, (int)this.m__E00A.Length);
			if (!this.m__E00D._E000(in segment3))
			{
				_E008();
			}
		}
	}

	private bool _E00A(PacketPriority priority)
	{
		byte channelId = ((priority == PacketPriority.Critical) ? _E6A8._E005 : ((byte)(_E6A8._E005 + 1)));
		byte[] buffer = this.m__E00A.GetBuffer();
		int num = (int)this.m__E00A.Length;
		byte error;
		bool num2 = this.m__E008.TransportSend(buffer, num, channelId, out error);
		if (num2)
		{
			TrafficCounters.GameSentTraffic.Update(num);
			this.m__E00C = false;
			return num2;
		}
		_E00B(error, channelId, num);
		return num2;
	}

	private void _E00B(byte error, byte channelId, int count)
	{
		if (count == 0)
		{
			this.m__E004.LogError(string.Format(_ED3E._E000(157795), channelId));
		}
		switch (error)
		{
		case 4:
			this.m__E00C = true;
			break;
		case 2:
			this.m__E008.Close(string.Format(_ED3E._E000(157825), error), isError: false);
			_E016 = true;
			break;
		}
	}

	internal void _E00C(NetworkReader reader)
	{
		byte b = reader.ReadByte();
		ushort num = reader.ReadUInt16();
		if (num > 1200)
		{
			this.m__E004.LogError(string.Format(_ED3E._E000(157874), num, this.m__E006, b));
		}
		TrafficCounters.GameReceivedTraffic.Update(num + 1);
		this.m__E00B.Reset();
		try
		{
			if (this.m__E008 == null)
			{
				Debug.LogErrorFormat(_ED3E._E000(157948), _ED3E._E000(157933), _ED3E._E000(157981));
			}
			byte error;
			int rtt = ((this.m__E008 != null) ? NetworkTransport.GetCurrentRTT(this.m__E008.hostId, this.m__E008.connectionId, out error) : 0);
			byte error2;
			int loss = ((this.m__E008 != null) ? NetworkTransport.GetOutgoingPacketNetworkLossPercent(this.m__E008.hostId, this.m__E008.connectionId, out error2) : 0);
			if (this.m__E010)
			{
				reader.ReadBytesNonAlloc(_E013, 0, num);
				int decryptedPacketLength = this.m__E00B.Buffer.Length;
				BEClient.DecryptPacket(_E013, 0, num, this.m__E00B.Buffer, 0, ref decryptedPacketLength);
				if (decryptedPacketLength == 0)
				{
					throw new _E318(_ED3E._E000(157969));
				}
			}
			else
			{
				reader.ReadBytesNonAlloc(this.m__E00B.Buffer, 0, num);
			}
			try
			{
				this.m__E009?.Invoke(b, this.m__E00B, rtt);
			}
			catch (OutOfMemoryException e)
			{
				this.m__E004.LogException(e);
				_E00D(e).HandleExceptions();
			}
			_E010(Time.time, rtt, loss);
		}
		catch (_E51E obj)
		{
			this.m__E004.LogException(obj);
			string text = (obj.Bytes ?? new byte[0]).ToHexString();
			int num2 = ((obj.Bytes != null) ? obj.BytesRead : ((int)reader.Position));
			string format = string.Format(_ED3E._E000(157955), obj.Size, obj.MaxSize, num2, text);
			this.m__E004.LogError(format);
		}
		catch (_E318 obj2)
		{
			this.m__E004.LogException(obj2);
			string text2 = string.Format(_ED3E._E000(158065), obj2.Message, this.m__E006, b, this.m__E008.hostId, this.m__E008.connectionId);
			this.m__E004.LogError(text2);
			this.m__E008.Close(text2, isError: true);
			_E016 = true;
		}
		catch (Exception e2)
		{
			this.m__E004.LogException(e2);
		}
	}

	private async Task _E00D(Exception e)
	{
		this.m__E009 = null;
		AbstractGameSession._E005 msg = new AbstractGameSession._E005
		{
			_E004 = 100503
		};
		this.m__E008.SendByChannel(148, msg, _E6A8._E003);
		await Task.Delay(3000);
		this.m__E008?.Close(e.ToString(), isError: true);
	}

	private static void _E00E(Stream stream, ushort value)
	{
		stream.WriteByte((byte)value);
		stream.WriteByte((byte)(value >> 8));
	}

	private void Update()
	{
		_E00F();
	}

	private void FixedUpdate()
	{
		_E00F();
	}

	private void _E00F()
	{
		if ((this.m__E001 != null || this.m__E002 != null) && this.m__E000 != null && this.m__E008.isConnected)
		{
			byte error;
			int currentRTT = NetworkTransport.GetCurrentRTT(this.m__E008.hostId, this.m__E008.connectionId, out error);
			byte error2;
			int outgoingPacketNetworkLossPercent = NetworkTransport.GetOutgoingPacketNetworkLossPercent(this.m__E008.hostId, this.m__E008.connectionId, out error2);
			_E010(Time.time, currentRTT, outgoingPacketNetworkLossPercent);
		}
	}

	private void _E010(float time, int rtt, int loss)
	{
		_E6AC._E000 obj = this.m__E001?.Update(time, rtt);
		_E6AB networkQualityParam = this.m__E001?.NetworkQualityParam;
		EDisconnectionCode eDisconnectionCode = EDisconnectionCode.BAD_RTT;
		if (obj == null)
		{
			obj = this.m__E002?.Update(time, loss);
			if (obj == null)
			{
				return;
			}
			networkQualityParam = this.m__E002?.NetworkQualityParam;
			eDisconnectionCode = EDisconnectionCode.HIGH_PACKETS_LOSS;
		}
		if (this.m__E001 != null)
		{
			this.m__E001._E007 = false;
		}
		if (this.m__E002 != null)
		{
			this.m__E002._E007 = false;
		}
		float num = time - obj.Time;
		this.m__E004.LogError(string.Format(_ED3E._E000(158116), _E015, this.m__E008.hostId, this.m__E008.connectionId, eDisconnectionCode, time, obj.Time, num));
		Interlocked.Exchange(ref this.m__E000, null)?.Invoke(networkQualityParam, eDisconnectionCode, obj.Time, num);
	}
}
