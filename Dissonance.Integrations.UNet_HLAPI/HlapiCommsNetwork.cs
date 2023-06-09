using System;
using System.Collections.Generic;
using Dissonance.Datastructures;
using Dissonance.Extensions;
using Dissonance.Networking;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;

namespace Dissonance.Integrations.UNet_HLAPI;

[HelpURL("https://placeholder-software.co.uk/dissonance/docs/Basics/Quick-Start-UNet-HLAPI/")]
public class HlapiCommsNetwork : BaseCommsNetwork<_E4CF, _E4CD, _E4CE, Unit, Unit>
{
	public byte UnreliableChannel = 1;

	public byte ReliableSequencedChannel;

	public short TypeCode = 18385;

	private readonly ConcurrentPool<byte[]> _loopbackBuffers = new ConcurrentPool<byte[]>(8, () => new byte[1024]);

	private readonly List<ArraySegment<byte>> _loopbackQueue = new List<ArraySegment<byte>>();

	public bool Activated = true;

	protected override _E4CF CreateServer(Unit details)
	{
		return new _E4CF(this);
	}

	protected override _E4CD CreateClient(Unit details)
	{
		return new _E4CD(this);
	}

	protected override void Update()
	{
		if (base.IsInitialized)
		{
			if (NetworkManager.singleton != null && NetworkManager.singleton.isNetworkActive && (NetworkServer.active || NetworkClient.active))
			{
				if (!NetworkClient.active)
				{
					_ = 1;
				}
				else if (NetworkManager.singleton.client != null)
				{
					_ = NetworkManager.singleton.client.connection != null;
				}
				else
					_ = 0;
			}
			else
				_ = 0;
			if (Activated)
			{
				bool active = NetworkServer.active;
				bool active2 = NetworkClient.active;
				active2 = true;
				if (base.Mode.IsServerEnabled() != active || base.Mode.IsClientEnabled() != active2)
				{
					RunAsClient(Unit.None);
				}
			}
			else if (base.Mode != 0)
			{
				Stop();
				_loopbackQueue.Clear();
			}
			for (int i = 0; i < _loopbackQueue.Count; i++)
			{
				if (base.Client != null)
				{
					base.Client.NetworkReceivedPacket(_loopbackQueue[i]);
				}
				_loopbackBuffers.Put(_loopbackQueue[i].Array);
			}
			_loopbackQueue.Clear();
		}
		base.Update();
	}

	protected override void Initialize()
	{
		NetworkServer.RegisterHandler(TypeCode, NullMessageReceivedHandler);
		base.Initialize();
	}

	internal bool PreprocessPacketToClient(ArraySegment<byte> packet, _E4CE destination)
	{
		if (base.Server == null)
		{
			throw Log.CreatePossibleBugException("server packet preprocessing running, but this peer is not a server", "8f9dc0a0-1b48-4a7f-9bb6-f767b2542ab1");
		}
		if (base.Client == null)
		{
			return false;
		}
		if (NetworkManager.singleton.client.connection != destination.Connection)
		{
			return false;
		}
		if (base.Client != null)
		{
			_loopbackQueue.Add(packet.CopyTo(_loopbackBuffers.Get()));
		}
		return true;
	}

	internal bool PreprocessPacketToServer(ArraySegment<byte> packet)
	{
		if (base.Client == null)
		{
			throw Log.CreatePossibleBugException("client packet processing running, but this peer is not a client", "dd75dce4-e85c-4bb3-96ec-3a3636cc4fbe");
		}
		if (base.Server == null)
		{
			return false;
		}
		base.Server.NetworkReceivedPacket(new _E4CE(NetworkManager.singleton.client.connection), packet);
		return true;
	}

	internal static void NullMessageReceivedHandler([NotNull] NetworkMessage netmsg)
	{
		if (netmsg == null)
		{
			throw new ArgumentNullException("netmsg");
		}
		if (Logs.GetLogLevel(LogCategory.Network) <= LogLevel.Trace)
		{
			Debug.Log("Discarding Dissonance network message");
		}
		int num = (int)netmsg.reader.ReadPackedUInt32();
		for (int i = 0; i < num; i++)
		{
			netmsg.reader.ReadByte();
		}
	}

	internal ArraySegment<byte> CopyToArraySegment([NotNull] NetworkReader msg, ArraySegment<byte> segment)
	{
		if (msg == null)
		{
			throw new ArgumentNullException("msg");
		}
		byte[] array = segment.Array;
		if (array == null)
		{
			throw new ArgumentNullException("segment");
		}
		int num = (int)msg.ReadPackedUInt32();
		if (num > segment.Count)
		{
			throw Log.CreatePossibleBugException("receive buffer is too small", "A7387195-BF3D-4796-A362-6C64BB546445");
		}
		for (int i = 0; i < num; i++)
		{
			array[segment.Offset + i] = msg.ReadByte();
		}
		return new ArraySegment<byte>(array, segment.Offset, num);
	}

	internal int CopyPacketToNetworkWriter(ArraySegment<byte> packet, [NotNull] NetworkWriter writer)
	{
		if (writer == null)
		{
			throw new ArgumentNullException("writer");
		}
		byte[] array = packet.Array;
		if (array == null)
		{
			throw new ArgumentNullException("packet");
		}
		writer.SeekZero();
		writer.StartMessage(TypeCode);
		writer.WritePackedUInt32((uint)packet.Count);
		for (int i = 0; i < packet.Count; i++)
		{
			writer.Write(array[packet.Offset + i]);
		}
		writer.FinishMessage();
		return writer.Position;
	}
}
