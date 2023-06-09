using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

namespace Dissonance.Integrations.UNet_HLAPI;

[RequireComponent(typeof(NetworkIdentity))]
public class HlapiPlayer : NetworkBehaviour, IDissonancePlayer
{
	[CompilerGenerated]
	private WrappedVoipAudioSource m__E000;

	private static readonly Log m__E001;

	private DissonanceComms m__E002;

	[CompilerGenerated]
	private bool m__E003;

	[SyncVar]
	private string m__E004;

	private static int m__E005;

	private static int _E006;

	WrappedVoipAudioSource IDissonancePlayer.VoipAudioSource
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
		[CompilerGenerated]
		set
		{
			this.m__E000 = value;
		}
	}

	public bool IsTracking
	{
		[CompilerGenerated]
		get
		{
			return this.m__E003;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E003 = value;
		}
	}

	public string PlayerId => this.m__E004;

	public Vector3 Position => base.transform.position;

	public Quaternion Rotation => base.transform.rotation;

	public NetworkPlayerType Type
	{
		get
		{
			if (this.m__E002 == null || this.m__E004 == null)
			{
				return NetworkPlayerType.Unknown;
			}
			if (!this.m__E002.LocalPlayerName.Equals(this.m__E004))
			{
				return NetworkPlayerType.Remote;
			}
			return NetworkPlayerType.Local;
		}
	}

	public string Network_playerId
	{
		get
		{
			return this.m__E004;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref this.m__E004, 1u);
		}
	}

	public void OnDestroy()
	{
		if (this.m__E002 != null)
		{
			this.m__E002.LocalPlayerNameChanged -= _E000;
		}
	}

	public void OnEnable()
	{
		this.m__E002 = Object.FindObjectOfType<DissonanceComms>();
	}

	public void OnDisable()
	{
		if (IsTracking)
		{
			_E004();
		}
	}

	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();
		DissonanceComms dissonanceComms = Object.FindObjectOfType<DissonanceComms>();
		if (dissonanceComms == null)
		{
			throw HlapiPlayer.m__E001.CreateUserErrorException(_ED3E._E000(117353), _ED3E._E000(117432), _ED3E._E000(117494), _ED3E._E000(117596));
		}
		if (dissonanceComms.LocalPlayerName != null)
		{
			_E000(dissonanceComms.LocalPlayerName);
		}
		dissonanceComms.LocalPlayerNameChanged += _E000;
	}

	private void _E000(string playerName)
	{
		if (IsTracking)
		{
			_E004();
		}
		Network_playerId = playerName;
		_E003();
		if (base.isLocalPlayer)
		{
			CallCmdSetPlayerName(playerName);
		}
	}

	public override void OnStartClient()
	{
		base.OnStartClient();
		if (!string.IsNullOrEmpty(PlayerId))
		{
			_E003();
		}
	}

	[Command]
	private void _E001(string playerName)
	{
		Network_playerId = playerName;
		CallRpcSetPlayerName(playerName);
	}

	[ClientRpc]
	private void _E002(string playerName)
	{
		if (!base.isLocalPlayer)
		{
			_E000(playerName);
		}
	}

	private void _E003()
	{
		if (IsTracking)
		{
			throw HlapiPlayer.m__E001.CreatePossibleBugException(_ED3E._E000(117625), _ED3E._E000(117686));
		}
		if (this.m__E002 != null)
		{
			this.m__E002.TrackPlayerPosition(this);
			IsTracking = true;
		}
	}

	private void _E004()
	{
		if (!IsTracking)
		{
			throw HlapiPlayer.m__E001.CreatePossibleBugException(_ED3E._E000(117715), _ED3E._E000(128019));
		}
		if (this.m__E002 != null)
		{
			this.m__E002.StopTracking(this);
			IsTracking = false;
		}
	}

	static HlapiPlayer()
	{
		HlapiPlayer.m__E001 = Logs.Create(LogCategory.Network, _ED3E._E000(128040));
		HlapiPlayer.m__E005 = -1254064873;
		NetworkBehaviour.RegisterCommandDelegate(typeof(HlapiPlayer), HlapiPlayer.m__E005, InvokeCmdCmdSetPlayerName);
		_E006 = 1332331777;
		NetworkBehaviour.RegisterRpcDelegate(typeof(HlapiPlayer), _E006, InvokeRpcRpcSetPlayerName);
		NetworkCRC.RegisterBehaviour(_ED3E._E000(128087), 0);
	}

	private void _E005()
	{
	}

	protected static void InvokeCmdCmdSetPlayerName(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(128075));
		}
		else
		{
			((HlapiPlayer)obj)._E001(reader.ReadString());
		}
	}

	public void CallCmdSetPlayerName(string playerName)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(128158));
			return;
		}
		if (base.isServer)
		{
			_E001(playerName);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)HlapiPlayer.m__E005);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(playerName);
		SendCommandInternal(networkWriter, 0, _ED3E._E000(128170));
	}

	protected static void InvokeRpcRpcSetPlayerName(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError(_ED3E._E000(128219));
		}
		else
		{
			((HlapiPlayer)obj)._E002(reader.ReadString());
		}
	}

	public void CallRpcSetPlayerName(string playerName)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError(_ED3E._E000(128242));
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)_E006);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(playerName);
		SendRPCInternal(networkWriter, 0, _ED3E._E000(128258));
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.Write(this.m__E004);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & (true ? 1u : 0u)) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m__E004);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			this.m__E004 = reader.ReadString();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if (((uint)num & (true ? 1u : 0u)) != 0)
		{
			this.m__E004 = reader.ReadString();
		}
	}

	public override void PreStartClient()
	{
	}
}
