using ComponentAce.Compression.Libs.zlib;
using EFT.Counters;
using UnityEngine;
using UnityEngine.Networking;

namespace EFT;

internal abstract class AbstractGameSession : AbstractSession
{
	internal new sealed class _E000 : MessageBase
	{
		internal string _E000;

		internal string _E001;

		internal bool _E002;

		internal byte[] _E003;

		internal int _E004;

		internal string _E005;

		public override void Deserialize(NetworkReader reader)
		{
			_E000 = reader.ReadString();
			_E001 = reader.ReadString();
			_E002 = reader.ReadBoolean();
			_E003 = reader.ReadBytesAndSize();
			_E004 = reader.ReadInt32();
			_E005 = reader.ReadString();
			base.Deserialize(reader);
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(_E000);
			writer.Write(_E001);
			writer.Write(_E002);
			writer.WriteBytesFull(_E003);
			writer.Write(_E004);
			writer.Write(_E005);
			base.Serialize(writer);
		}
	}

	internal class _E001 : MessageBase
	{
		public byte TracerId;

		public string[] Context;

		public override void Deserialize(NetworkReader reader)
		{
			TracerId = reader.ReadByte();
			ushort num = reader.ReadUInt16();
			if (num != 0)
			{
				Context = new string[num];
				for (int i = 0; i < num; i++)
				{
					byte[] bytes = reader.ReadBytesAndSize();
					Context[i] = SimpleZlib.Decompress(bytes);
				}
			}
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(TracerId);
			string[] context = Context;
			ushort num = (ushort)((context != null) ? ((uint)context.Length) : 0u);
			writer.Write(num);
			if (num > 0)
			{
				string[] context2 = Context;
				for (int i = 0; i < context2.Length; i++)
				{
					byte[] buffer = SimpleZlib.CompressToBytes(context2[i] ?? "", 9);
					writer.WriteBytesFull(buffer);
				}
			}
		}
	}

	internal sealed class _E002 : _E001
	{
		public string Message;

		public override void Deserialize(NetworkReader reader)
		{
			Message = reader.ReadString();
			base.Deserialize(reader);
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(Message);
			base.Serialize(writer);
		}
	}

	internal sealed class _E003 : _E001
	{
		public ETraceCode Code;

		public override void Deserialize(NetworkReader reader)
		{
			Code = (ETraceCode)reader.ReadByte();
			base.Deserialize(reader);
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write((byte)Code);
			base.Serialize(writer);
		}
	}

	internal sealed class _E004 : MessageBase
	{
		internal bool _E000;

		internal bool _E001;

		internal _E629 _E002;

		internal byte[] _E003;

		internal byte[] _E004;

		internal byte[] _E005;

		internal bool _E006;

		internal EMemberCategory _E007;

		internal float _E008;

		internal byte[] _E009;

		internal byte[] _E00A;

		internal Bounds _E00B;

		internal ushort _E00C;

		internal ENetLogsLevel _E00D;

		internal _E2B9 _E00E;

		internal bool _E00F;

		internal _E717.Config _E010 = new _E717.Config();

		internal _E75D _E011;

		public override void Deserialize(NetworkReader reader)
		{
			_E000 = reader.ReadBoolean();
			_E001 = reader.ReadBoolean();
			_E002 = _E629.Deserialize(reader);
			_E003 = reader.ReadBytesAndSize();
			_E004 = reader.ReadBytesAndSize();
			_E005 = reader.ReadBytesAndSize();
			_E006 = reader.ReadBoolean();
			_E007 = (EMemberCategory)reader.ReadInt32();
			_E008 = reader.ReadSingle();
			_E009 = reader.ReadBytesAndSize();
			_E00A = reader.ReadBytesAndSize();
			Vector3 min = reader.ReadVector3();
			Vector3 max = reader.ReadVector3();
			_E00B = new Bounds
			{
				min = min,
				max = max
			};
			_E00C = reader.ReadUInt16();
			_E00D = (ENetLogsLevel)reader.ReadByte();
			_E00E = _E2B9.Deserialize(reader);
			_E00F = reader.ReadBoolean();
			if (_E00F)
			{
				_E010.Deserialize(reader);
			}
			if (reader.ReadBoolean())
			{
				_E011 = new _E75D();
				_E011.Deserialize(reader);
			}
			base.Deserialize(reader);
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(_E000);
			writer.Write(_E001);
			_E002.Serialize(writer, gameOnly: true);
			writer.WriteBytesFull(_E003);
			writer.WriteBytesFull(_E004);
			writer.WriteBytesFull(_E005);
			writer.Write(_E006);
			writer.Write((int)_E007);
			writer.Write(_E008);
			writer.WriteBytesFull(_E009);
			writer.WriteBytesFull(_E00A);
			writer.Write(_E00B.min);
			writer.Write(_E00B.max);
			writer.Write(_E00C);
			writer.Write((byte)_E00D);
			_E00E.Serialize(writer);
			writer.Write(_E00F);
			if (_E00F)
			{
				_E010.Serialize(writer);
			}
			writer.Write(_E011 != null);
			_E011?.Serialize(writer);
			base.Serialize(writer);
		}
	}

	internal sealed class _E005 : MessageBase
	{
		internal const int _E000 = 100500;

		internal const int _E001 = 100501;

		internal const int _E002 = 100502;

		internal const int _E003 = 100503;

		internal int _E004;

		public override void Deserialize(NetworkReader reader)
		{
			_E004 = reader.ReadInt32();
			base.Deserialize(reader);
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(_E004);
			base.Serialize(writer);
		}
	}

	internal sealed class _E006 : MessageBase
	{
		internal float _E000;

		public override void Deserialize(NetworkReader reader)
		{
			_E000 = reader.ReadSingle();
			base.Deserialize(reader);
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(_E000);
			base.Serialize(writer);
		}
	}

	internal sealed class _E007 : MessageBase
	{
		internal byte[] _E000;

		public override void Serialize(NetworkWriter writer)
		{
			writer.WriteBytesFull(_E000);
			base.Serialize(writer);
		}

		public override void Deserialize(NetworkReader reader)
		{
			_E000 = reader.ReadBytesAndSize();
			base.Deserialize(reader);
		}
	}

	internal sealed class _E008 : MessageBase
	{
		internal int _E000;

		internal byte[] _E001;

		internal byte[] _E002;

		public override void Deserialize(NetworkReader reader)
		{
			_E000 = reader.ReadInt32();
			_E001 = reader.ReadBytesAndSize();
			_E002 = reader.ReadBytesAndSize();
			base.Deserialize(reader);
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(_E000);
			writer.WriteBytesAndSize(_E001, _E001.Length);
			writer.WriteBytesAndSize(_E002, _E002.Length);
			base.Serialize(writer);
		}
	}

	internal sealed class _E009 : MessageBase
	{
		internal string _E000;

		internal int _E001;

		internal float _E002;

		public override void Deserialize(NetworkReader reader)
		{
			_E000 = reader.ReadString();
			_E001 = reader.ReadInt32();
			_E002 = reader.ReadSingle();
			base.Deserialize(reader);
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(_E000);
			writer.Write(_E001);
			writer.Write(_E002);
			base.Serialize(writer);
		}
	}

	private new static int m__E000;

	private static int m__E001;

	private static int m__E002;

	private static int m__E003;

	private static int m__E004;

	private static int m__E005;

	private static int m__E006;

	private static int m__E007;

	private static int m__E008;

	private static int m__E009;

	private static int m__E00A;

	private static int m__E00B;

	private static int m__E00C;

	private static int m__E00D;

	private static int m__E00E;

	private static int m__E00F;

	private static int m__E010;

	private static int m__E011;

	private static int m__E012;

	private static int m__E013;

	private static int m__E014;

	private static int m__E015;

	private static int m__E016;

	private static int m__E017;

	private static int m__E018;

	private static int m__E019;

	private static int m__E01A;

	private static int m__E01B;

	private static int m__E01C;

	private static int m__E01D;

	private static int m__E01E;

	private static int m__E01F;

	private static int m__E020;

	private static int m__E021;

	private static int m__E022;

	private static int m__E023;

	private static int m__E024;

	private static int m__E025;

	private static int m__E026;

	private static int m__E027;

	private static int m__E028;

	private static int m__E029;

	private static int m__E02A;

	private static int m__E02B;

	private static int m__E02C;

	private static int m__E02D;

	protected new static _E077 _E000<_E077>(Transform parent, string name, string profileId, string token) where _E077 : AbstractGameSession
	{
		_E077 val = AbstractSession._E000<_E077>(parent, name, profileId, token);
		val.GetComponent<NetworkIdentity>().localPlayerAuthority = true;
		return val;
	}

	public override float GetNetworkSendInterval()
	{
		return 0f;
	}

	public override int GetNetworkChannel()
	{
		return _E6A8._E002;
	}

	[Command]
	protected new virtual void _E030()
	{
	}

	[Command]
	protected new virtual void _E031()
	{
	}

	[Command]
	protected new virtual void _E032()
	{
	}

	[Command]
	protected new virtual void _E033()
	{
	}

	[Command]
	protected new virtual void _E034()
	{
	}

	[Command]
	protected new virtual void _E035()
	{
	}

	[Command]
	protected new virtual void _E036()
	{
	}

	[Command]
	protected new virtual void _E037()
	{
	}

	[Command]
	protected new virtual void _E038()
	{
	}

	[Command]
	protected new virtual void _E039(string profileId)
	{
	}

	[Command]
	protected new virtual void _E03A()
	{
	}

	[Command]
	protected new virtual void _E03B()
	{
	}

	[Command]
	protected new virtual void _E03C(EPlayerSide side)
	{
	}

	[Command]
	protected new virtual void _E03D(EPlayerSide side)
	{
	}

	[Command]
	protected new virtual void _E03E(EPlayerSide side, int instanceId)
	{
	}

	[Command]
	protected new virtual void _E03F()
	{
	}

	[Command]
	protected new virtual void _E040()
	{
	}

	[Command]
	protected new virtual void _E041(int playerId)
	{
	}

	[Command]
	protected virtual void _E042()
	{
	}

	[Command]
	protected virtual void _E043(string playerProfileID, bool isPaused)
	{
	}

	[Command]
	protected virtual void _E044(string playerProfileID, CounterTag statisticsType, int valueToSet)
	{
	}

	[Command]
	protected virtual void CmdGetRadiotransmitterData(string playerProfileID)
	{
	}

	[ClientRpc]
	protected virtual void _E045()
	{
	}

	[ClientRpc]
	protected virtual void _E046(ushort activitiesCounter, ushort minCounter, int seconds)
	{
	}

	[ClientRpc]
	protected virtual void _E047(int seconds)
	{
	}

	[ClientRpc]
	protected virtual void _E048(Vector3 position, int exfiltrationId, string entryPoint)
	{
	}

	[ClientRpc]
	protected virtual void _E049(float pastTime, int sessionSeconds)
	{
	}

	[ClientRpc]
	protected virtual void _E04A()
	{
	}

	[ClientRpc]
	protected virtual void _E04B()
	{
	}

	[ClientRpc]
	protected virtual void _E04C()
	{
	}

	[ClientRpc]
	protected virtual void _E04D(ExitStatus exitStatus, int playTime)
	{
	}

	[ClientRpc]
	protected virtual void _E04E(long time)
	{
	}

	[ClientRpc]
	protected virtual void _E04F(byte[] data)
	{
	}

	[ClientRpc]
	protected virtual void _E050(byte[] data)
	{
	}

	[ClientRpc]
	protected virtual void _E051(byte[] data)
	{
	}

	[ClientRpc]
	protected virtual void _E052(EPlayerSide side, int instanceId)
	{
	}

	[ClientRpc]
	protected virtual void _E053(int sessionSeconds)
	{
	}

	[ClientRpc]
	protected virtual void _E054(int disconnectionCode, string additionalInfo, string technicalMessage)
	{
	}

	[ClientRpc]
	protected virtual void _E055(string reporter)
	{
	}

	[ClientRpc]
	protected virtual void _E056(byte[] data)
	{
	}

	[ClientRpc]
	protected virtual void _E057(byte[] data)
	{
	}

	[ClientRpc]
	protected virtual void _E058(bool canSendAirdrop)
	{
	}

	[ClientRpc]
	protected virtual void _E059(byte[] data)
	{
	}

	[ClientRpc]
	protected virtual void _E05A(_E638 data)
	{
	}

	[ClientRpc]
	protected virtual void _E05B(_E638 data)
	{
	}

	[ClientRpc]
	protected virtual void _E05C(_E634 data)
	{
	}

	private void _E001()
	{
	}

	protected static void _E002(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(106041));
		}
		else
		{
			((AbstractGameSession)obj)._E030();
		}
	}

	protected static void _E003(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(106068));
		}
		else
		{
			((AbstractGameSession)obj)._E031();
		}
	}

	protected static void _E004(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(106097));
		}
		else
		{
			((AbstractGameSession)obj)._E032();
		}
	}

	protected static void _E005(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(106120));
		}
		else
		{
			((AbstractGameSession)obj)._E033();
		}
	}

	protected static void _E006(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(106196));
		}
		else
		{
			((AbstractGameSession)obj)._E034();
		}
	}

	protected static void _E007(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(106213));
		}
		else
		{
			((AbstractGameSession)obj)._E035();
		}
	}

	protected static void _E008(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(106302));
		}
		else
		{
			((AbstractGameSession)obj)._E036();
		}
	}

	protected static void _E009(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(106327));
		}
		else
		{
			((AbstractGameSession)obj)._E037();
		}
	}

	protected static void _E00A(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(106349));
		}
		else
		{
			((AbstractGameSession)obj)._E038();
		}
	}

	protected static void _E00B(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(106375));
		}
		else
		{
			((AbstractGameSession)obj)._E039(reader.ReadString());
		}
	}

	protected static void _E00C(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(106454));
		}
		else
		{
			((AbstractGameSession)obj)._E03A();
		}
	}

	protected static void _E00D(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(106466));
		}
		else
		{
			((AbstractGameSession)obj)._E03B();
		}
	}

	protected static void _E00E(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(104495));
		}
		else
		{
			((AbstractGameSession)obj)._E03C((EPlayerSide)reader.ReadInt32());
		}
	}

	protected static void _E00F(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(104567));
		}
		else
		{
			((AbstractGameSession)obj)._E03D((EPlayerSide)reader.ReadInt32());
		}
	}

	protected static void _E010(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(104632));
		}
		else
		{
			((AbstractGameSession)obj)._E03E((EPlayerSide)reader.ReadInt32(), (int)reader.ReadPackedUInt32());
		}
	}

	protected static void _E011(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(104641));
		}
		else
		{
			((AbstractGameSession)obj)._E03F();
		}
	}

	protected static void _E012(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(104713));
		}
		else
		{
			((AbstractGameSession)obj)._E040();
		}
	}

	protected static void _E013(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(104792));
		}
		else
		{
			((AbstractGameSession)obj)._E041((int)reader.ReadPackedUInt32());
		}
	}

	protected static void _E014(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(104818));
		}
		else
		{
			((AbstractGameSession)obj)._E042();
		}
	}

	protected static void _E015(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(104839));
		}
		else
		{
			((AbstractGameSession)obj)._E043(reader.ReadString(), reader.ReadBoolean());
		}
	}

	protected static void _E016(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(104919));
		}
		else
		{
			((AbstractGameSession)obj)._E044(reader.ReadString(), (CounterTag)reader.ReadInt32(), (int)reader.ReadPackedUInt32());
		}
	}

	protected static void _E017(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(104980));
		}
		else
		{
			((AbstractGameSession)obj).CmdGetRadiotransmitterData(reader.ReadString());
		}
	}

	public void CallCmdSpawn()
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(104993));
			return;
		}
		if (base.isServer)
		{
			_E030();
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E000);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendCommandInternal(networkWriter, 0, _ED3E._E000(105077));
	}

	public void CallCmdRespawn()
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(105070));
			return;
		}
		if (base.isServer)
		{
			_E031();
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E001);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendCommandInternal(networkWriter, 0, _ED3E._E000(105148));
	}

	public void CallCmdStartGame()
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(105143));
			return;
		}
		if (base.isServer)
		{
			_E032();
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E002);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendCommandInternal(networkWriter, 0, _ED3E._E000(105159));
	}

	public void CallCmdStartGameAfterTeleport()
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(105204));
			return;
		}
		if (base.isServer)
		{
			_E033();
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E003);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendCommandInternal(networkWriter, 0, _ED3E._E000(105273));
	}

	public void CallCmdRestartGameInitiate()
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(105251));
			return;
		}
		if (base.isServer)
		{
			_E034();
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E004);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendCommandInternal(networkWriter, 0, _ED3E._E000(105317));
	}

	public void CallCmdRestartGame()
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(105356));
			return;
		}
		if (base.isServer)
		{
			_E035();
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E005);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendCommandInternal(networkWriter, 0, _ED3E._E000(105438));
	}

	public void CallCmdGameStarted()
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(105421));
			return;
		}
		if (base.isServer)
		{
			_E036();
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E006);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendCommandInternal(networkWriter, 0, _ED3E._E000(99359));
	}

	public void CallCmdStopGame()
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(99342));
			return;
		}
		if (base.isServer)
		{
			_E037();
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E007);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendCommandInternal(networkWriter, 0, _ED3E._E000(99421));
	}

	public void CallCmdSyncGameTime()
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(99409));
			return;
		}
		if (base.isServer)
		{
			_E038();
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E008);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendCommandInternal(networkWriter, 0, _ED3E._E000(99484));
	}

	public void CallCmdDevelopRequestBot(string profileId)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(99468));
			return;
		}
		if (base.isServer)
		{
			_E039(profileId);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E009);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(profileId);
		SendCommandInternal(networkWriter, 0, _ED3E._E000(99540));
	}

	public void CallCmdDevelopRequestBotZones()
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(99521));
			return;
		}
		if (base.isServer)
		{
			_E03A();
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E00A);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendCommandInternal(networkWriter, 0, _ED3E._E000(99590));
	}

	public void CallCmdDevelopRequestBotGroups()
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(99624));
			return;
		}
		if (base.isServer)
		{
			_E03B();
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E00B);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendCommandInternal(networkWriter, 0, _ED3E._E000(99694));
	}

	public void CallCmdDevelopmentSpawnBotRequest(EPlayerSide side)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(99729));
			return;
		}
		if (base.isServer)
		{
			_E03C(side);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E00C);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write((int)side);
		SendCommandInternal(networkWriter, 0, _ED3E._E000(99794));
	}

	public void CallCmdDevelopmentSpawnBotOnServer(EPlayerSide side)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(99824));
			return;
		}
		if (base.isServer)
		{
			_E03D(side);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E00D);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write((int)side);
		SendCommandInternal(networkWriter, 0, _ED3E._E000(99890));
	}

	public void CallCmdDevelopmentSpawnBotOnClient(EPlayerSide side, int instanceId)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(99921));
			return;
		}
		if (base.isServer)
		{
			_E03E(side, instanceId);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E00E);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write((int)side);
		networkWriter.WritePackedUInt32((uint)instanceId);
		SendCommandInternal(networkWriter, 0, _ED3E._E000(99987));
	}

	public void CallCmdDisconnectAcceptedOnClient()
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(100018));
			return;
		}
		if (base.isServer)
		{
			_E03F();
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E00F);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendCommandInternal(networkWriter, 0, _ED3E._E000(100083));
	}

	public void CallCmdWorldSpawnConfirm()
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(100113));
			return;
		}
		if (base.isServer)
		{
			_E040();
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E010);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendCommandInternal(networkWriter, 0, _ED3E._E000(100185));
	}

	public void CallCmdSpawnConfirm(int playerId)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(100166));
			return;
		}
		if (base.isServer)
		{
			_E041(playerId);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E011);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)playerId);
		SendCommandInternal(networkWriter, 0, _ED3E._E000(100241));
	}

	public void CallCmdReportVoipAbuse()
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(100225));
			return;
		}
		if (base.isServer)
		{
			_E042();
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E012);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendCommandInternal(networkWriter, 0, _ED3E._E000(100303));
	}

	public void CallCmdPlayerEffectsPause(string playerProfileID, bool isPaused)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(100346));
			return;
		}
		if (base.isServer)
		{
			_E043(playerProfileID, isPaused);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E013);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(playerProfileID);
		networkWriter.Write(isPaused);
		SendCommandInternal(networkWriter, 0, _ED3E._E000(98307));
	}

	public void CallCmdOnPlayerKeeperStatisticsChanged(string playerProfileID, CounterTag statisticsType, int valueToSet)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(98345));
			return;
		}
		if (base.isServer)
		{
			_E044(playerProfileID, statisticsType, valueToSet);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E014);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(playerProfileID);
		networkWriter.Write((int)statisticsType);
		networkWriter.WritePackedUInt32((uint)valueToSet);
		SendCommandInternal(networkWriter, 0, _ED3E._E000(98407));
	}

	public void CallCmdGetRadiotransmitterData(string playerProfileID)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(98434));
			return;
		}
		if (base.isServer)
		{
			CmdGetRadiotransmitterData(playerProfileID);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E015);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(playerProfileID);
		SendCommandInternal(networkWriter, 0, _ED3E._E000(98496));
	}

	protected static void _E018(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(98539));
		}
		else
		{
			((AbstractGameSession)obj)._E045();
		}
	}

	protected static void _E019(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(98560));
		}
		else
		{
			((AbstractGameSession)obj)._E046((ushort)reader.ReadPackedUInt32(), (ushort)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
		}
	}

	protected static void _E01A(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(98654));
		}
		else
		{
			((AbstractGameSession)obj)._E047((int)reader.ReadPackedUInt32());
		}
	}

	protected static void _E01B(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(98676));
		}
		else
		{
			((AbstractGameSession)obj)._E048(reader.ReadVector3(), (int)reader.ReadPackedUInt32(), reader.ReadString());
		}
	}

	protected static void _E01C(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(98694));
		}
		else
		{
			((AbstractGameSession)obj)._E049(reader.ReadSingle(), (int)reader.ReadPackedUInt32());
		}
	}

	protected static void _E01D(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(98723));
		}
		else
		{
			((AbstractGameSession)obj)._E04A();
		}
	}

	protected static void _E01E(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(98811));
		}
		else
		{
			((AbstractGameSession)obj)._E04B();
		}
	}

	protected static void _E01F(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(98834));
		}
		else
		{
			((AbstractGameSession)obj)._E04C();
		}
	}

	protected static void _E020(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(98856));
		}
		else
		{
			((AbstractGameSession)obj)._E04D((ExitStatus)reader.ReadInt32(), (int)reader.ReadPackedUInt32());
		}
	}

	protected static void _E021(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(98885));
		}
		else
		{
			((AbstractGameSession)obj)._E04E((long)reader.ReadPackedUInt64());
		}
	}

	protected static void _E022(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(98915));
		}
		else
		{
			((AbstractGameSession)obj)._E04F(reader.ReadBytesAndSize());
		}
	}

	protected static void _E023(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(98999));
		}
		else
		{
			((AbstractGameSession)obj)._E050(reader.ReadBytesAndSize());
		}
	}

	protected static void _E024(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(99015));
		}
		else
		{
			((AbstractGameSession)obj)._E051(reader.ReadBytesAndSize());
		}
	}

	protected static void _E025(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(99089));
		}
		else
		{
			((AbstractGameSession)obj)._E052((EPlayerSide)reader.ReadInt32(), (int)reader.ReadPackedUInt32());
		}
	}

	protected static void _E026(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(99166));
		}
		else
		{
			((AbstractGameSession)obj)._E053((int)reader.ReadPackedUInt32());
		}
	}

	protected static void _E027(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(99180));
		}
		else
		{
			((AbstractGameSession)obj)._E054((int)reader.ReadPackedUInt32(), reader.ReadString(), reader.ReadString());
		}
	}

	protected static void _E028(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(99257));
		}
		else
		{
			((AbstractGameSession)obj)._E055(reader.ReadString());
		}
	}

	protected static void _E029(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(99272));
		}
		else
		{
			((AbstractGameSession)obj)._E056(reader.ReadBytesAndSize());
		}
	}

	protected static void _E02A(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(101406));
		}
		else
		{
			((AbstractGameSession)obj)._E057(reader.ReadBytesAndSize());
		}
	}

	protected static void _E02B(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(101422));
		}
		else
		{
			((AbstractGameSession)obj)._E058(reader.ReadBoolean());
		}
	}

	protected static void _E02C(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(101496));
		}
		else
		{
			((AbstractGameSession)obj)._E059(reader.ReadBytesAndSize());
		}
	}

	protected static void _E02D(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(101520));
		}
		else
		{
			((AbstractGameSession)obj)._E05A(_ED3D._ReadRadioTransmitterData_None(reader));
		}
	}

	protected new static void _E02E(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(101592));
		}
		else
		{
			((AbstractGameSession)obj)._E05B(_ED3D._ReadRadioTransmitterData_None(reader));
		}
	}

	protected new static void _E02F(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(101602));
		}
		else
		{
			((AbstractGameSession)obj)._E05C(_ED3D._ReadLighthouseTraderZoneData_None(reader));
		}
	}

	public void CallRpcGameSpawned()
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(101672));
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E016);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendRPCInternal(networkWriter, 0, _ED3E._E000(101758));
	}

	public void CallRpcGameMatching(ushort activitiesCounter, ushort minCounter, int seconds)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(101741));
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E017);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32(activitiesCounter);
		networkWriter.WritePackedUInt32(minCounter);
		networkWriter.WritePackedUInt32((uint)seconds);
		SendRPCInternal(networkWriter, 0, _ED3E._E000(101820));
	}

	public void CallRpcGameStarting(int seconds)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(101804));
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E018);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)seconds);
		SendRPCInternal(networkWriter, 0, _ED3E._E000(101827));
	}

	public void CallRpcGameStartingWithTeleport(Vector3 position, int exfiltrationId, string entryPoint)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(101875));
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E019);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(position);
		networkWriter.WritePackedUInt32((uint)exfiltrationId);
		networkWriter.Write(entryPoint);
		SendRPCInternal(networkWriter, 0, _ED3E._E000(101942));
	}

	public void CallRpcGameStarted(float pastTime, int sessionSeconds)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(101978));
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E01A);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(pastTime);
		networkWriter.WritePackedUInt32((uint)sessionSeconds);
		SendRPCInternal(networkWriter, 0, _ED3E._E000(101992));
	}

	public void CallRpcGameRestarting()
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(102047));
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E01B);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendRPCInternal(networkWriter, 0, _ED3E._E000(102056));
	}

	public void CallRpcGameRestarted()
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(102106));
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E01C);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendRPCInternal(networkWriter, 0, _ED3E._E000(102122));
	}

	public void CallRpcGameStopping()
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(102171));
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E01D);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendRPCInternal(networkWriter, 0, _ED3E._E000(102186));
	}

	public void CallRpcGameStopped(ExitStatus exitStatus, int playTime)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(102234));
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E01E);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write((int)exitStatus);
		networkWriter.WritePackedUInt32((uint)playTime);
		SendRPCInternal(networkWriter, 0, _ED3E._E000(102248));
	}

	public void CallRpcSyncGameTime(long time)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(102303));
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E01F);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt64((ulong)time);
		SendRPCInternal(networkWriter, 0, _ED3E._E000(102318));
	}

	public void CallRpcDevelopSendBotData(byte[] data)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(102366));
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E020);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WriteBytesFull(data);
		SendRPCInternal(networkWriter, 0, _ED3E._E000(102379));
	}

	public void CallRpcDevelopSendBotDataZone(byte[] data)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(100369));
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E021);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WriteBytesFull(data);
		SendRPCInternal(networkWriter, 0, _ED3E._E000(100442));
	}

	public void CallRpcDevelopSendBotDataGroups(byte[] data)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(100476));
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E022);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WriteBytesFull(data);
		SendRPCInternal(networkWriter, 0, _ED3E._E000(100487));
	}

	public void CallRpcDevelopmentSpawnBotResponse(EPlayerSide side, int instanceId)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(100523));
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E023);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write((int)side);
		networkWriter.WritePackedUInt32((uint)instanceId);
		SendRPCInternal(networkWriter, 0, _ED3E._E000(100585));
	}

	public void CallRpcSoftStopNotification(int sessionSeconds)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(100616));
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E024);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)sessionSeconds);
		SendRPCInternal(networkWriter, 0, _ED3E._E000(100695));
	}

	public void CallRpcStartDisconnectionProcedure(int disconnectionCode, string additionalInfo, string technicalMessage)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(100735));
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E025);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)disconnectionCode);
		networkWriter.Write(additionalInfo);
		networkWriter.Write(technicalMessage);
		SendRPCInternal(networkWriter, 0, _ED3E._E000(100797));
	}

	public void CallRpcVoipAbuseNotification(string reporter)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(100828));
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E026);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(reporter);
		SendRPCInternal(networkWriter, 0, _ED3E._E000(100836));
	}

	public void CallRpcAirdropContainerData(byte[] data)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(100877));
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E027);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WriteBytesFull(data);
		SendRPCInternal(networkWriter, 0, _ED3E._E000(100948));
	}

	public void CallRpcMineDirectionExplosion(byte[] data)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(100988));
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E028);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WriteBytesFull(data);
		SendRPCInternal(networkWriter, 0, _ED3E._E000(100997));
	}

	public void CallRpcSuccessAirdropFlareEvent(bool canSendAirdrop)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(101039));
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E029);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(canSendAirdrop);
		SendRPCInternal(networkWriter, 0, _ED3E._E000(101106));
	}

	public void CallRpcBufferZoneData(byte[] data)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(101142));
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E02A);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WriteBytesFull(data);
		SendRPCInternal(networkWriter, 0, _ED3E._E000(101159));
	}

	public void CallRpcSendClientRadioTransmitterData(_E638 data)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(101201));
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E02B);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		_ED3D._WriteRadioTransmitterData_None(networkWriter, data);
		SendRPCInternal(networkWriter, 0, _ED3E._E000(101266));
	}

	public void CallRpcSendObserverRadioTransmitterData(_E638 data)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(101292));
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E02C);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		_ED3D._WriteRadioTransmitterData_None(networkWriter, data);
		SendRPCInternal(networkWriter, 0, _ED3E._E000(101359));
	}

	public void CallRpcSyncLighthouseTraderZoneData(_E634 data)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(111627));
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)AbstractGameSession.m__E02D);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		_ED3D._WriteLighthouseTraderZoneData_None(networkWriter, data);
		SendRPCInternal(networkWriter, 0, _ED3E._E000(111690));
	}

	static AbstractGameSession()
	{
		AbstractGameSession.m__E000 = -1723132743;
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E000, _E002);
		AbstractGameSession.m__E001 = 740792038;
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E001, _E003);
		AbstractGameSession.m__E002 = -1220356686;
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E002, _E004);
		AbstractGameSession.m__E003 = 1792897173;
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E003, _E005);
		AbstractGameSession.m__E004 = 273195288;
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E004, _E006);
		AbstractGameSession.m__E005 = -1501005473;
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E005, _E007);
		AbstractGameSession.m__E006 = -40021267;
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E006, _E008);
		AbstractGameSession.m__E007 = -750099178;
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E007, _E009);
		AbstractGameSession.m__E008 = 463608476;
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E008, _E00A);
		AbstractGameSession.m__E009 = -1035840717;
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E009, _E00B);
		AbstractGameSession.m__E00A = 1432950484;
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E00A, _E00C);
		AbstractGameSession.m__E00B = 930653927;
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E00B, _E00D);
		AbstractGameSession.m__E00C = -1581543574;
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E00C, _E00E);
		AbstractGameSession.m__E00D = 102630535;
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E00D, _E00F);
		AbstractGameSession.m__E00E = -349255409;
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E00E, _E010);
		AbstractGameSession.m__E00F = -1733636721;
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E00F, _E011);
		AbstractGameSession.m__E010 = 1240699829;
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E010, _E012);
		AbstractGameSession.m__E011 = -1317447737;
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E011, _E013);
		AbstractGameSession.m__E012 = 810388720;
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E012, _E014);
		AbstractGameSession.m__E013 = 905971479;
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E013, _E015);
		AbstractGameSession.m__E014 = -65034947;
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E014, _E016);
		AbstractGameSession.m__E015 = -942910572;
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E015, _E017);
		AbstractGameSession.m__E016 = -1952818640;
		NetworkBehaviour.RegisterRpcDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E016, _E018);
		AbstractGameSession.m__E017 = 2117859815;
		NetworkBehaviour.RegisterRpcDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E017, _E019);
		AbstractGameSession.m__E018 = -1157222870;
		NetworkBehaviour.RegisterRpcDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E018, _E01A);
		AbstractGameSession.m__E019 = 1572370779;
		NetworkBehaviour.RegisterRpcDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E019, _E01B);
		AbstractGameSession.m__E01A = -1838445225;
		NetworkBehaviour.RegisterRpcDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E01A, _E01C);
		AbstractGameSession.m__E01B = 94275293;
		NetworkBehaviour.RegisterRpcDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E01B, _E01D);
		AbstractGameSession.m__E01C = -1243884988;
		NetworkBehaviour.RegisterRpcDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E01C, _E01E);
		AbstractGameSession.m__E01D = -758380962;
		NetworkBehaviour.RegisterRpcDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E01D, _E01F);
		AbstractGameSession.m__E01E = -1825579357;
		NetworkBehaviour.RegisterRpcDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E01E, _E020);
		AbstractGameSession.m__E01F = 547040626;
		NetworkBehaviour.RegisterRpcDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E01F, _E021);
		AbstractGameSession.m__E020 = 1152897188;
		NetworkBehaviour.RegisterRpcDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E020, _E022);
		AbstractGameSession.m__E021 = -1920895376;
		NetworkBehaviour.RegisterRpcDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E021, _E023);
		AbstractGameSession.m__E022 = 314346392;
		NetworkBehaviour.RegisterRpcDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E022, _E024);
		AbstractGameSession.m__E023 = -1269941968;
		NetworkBehaviour.RegisterRpcDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E023, _E025);
		AbstractGameSession.m__E024 = -435294673;
		NetworkBehaviour.RegisterRpcDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E024, _E026);
		AbstractGameSession.m__E025 = 1124901489;
		NetworkBehaviour.RegisterRpcDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E025, _E027);
		AbstractGameSession.m__E026 = 1547608889;
		NetworkBehaviour.RegisterRpcDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E026, _E028);
		AbstractGameSession.m__E027 = -2040405782;
		NetworkBehaviour.RegisterRpcDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E027, _E029);
		AbstractGameSession.m__E028 = -689857055;
		NetworkBehaviour.RegisterRpcDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E028, _E02A);
		AbstractGameSession.m__E029 = -2141949542;
		NetworkBehaviour.RegisterRpcDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E029, _E02B);
		AbstractGameSession.m__E02A = 778150830;
		NetworkBehaviour.RegisterRpcDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E02A, _E02C);
		AbstractGameSession.m__E02B = -52162261;
		NetworkBehaviour.RegisterRpcDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E02B, _E02D);
		AbstractGameSession.m__E02C = 1358208182;
		NetworkBehaviour.RegisterRpcDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E02C, _E02E);
		AbstractGameSession.m__E02D = 361141025;
		NetworkBehaviour.RegisterRpcDelegate(typeof(AbstractGameSession), AbstractGameSession.m__E02D, _E02F);
		NetworkCRC.RegisterBehaviour(_ED3E._E000(111722), 0);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool flag = base.OnSerialize(writer, forceAll);
		bool flag2 = default(bool);
		return flag2 || flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		base.OnDeserialize(reader, initialState);
	}

	public override void PreStartClient()
	{
		base.PreStartClient();
	}
}
