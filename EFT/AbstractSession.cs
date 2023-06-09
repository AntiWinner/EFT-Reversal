using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

namespace EFT;

internal abstract class AbstractSession : NetworkBehaviour
{
	internal const short _E02E = 147;

	internal const short _E02F = 148;

	internal const short _E030 = 188;

	internal const short _E031 = 189;

	internal const short _E032 = 190;

	internal const short _E033 = 151;

	internal const short _E034 = 152;

	internal const short _E035 = 153;

	internal const short _E036 = 154;

	internal const short _E037 = 155;

	internal const short _E038 = 156;

	internal const short _E039 = 157;

	internal const short _E03A = 158;

	internal const short _E03B = 160;

	internal const short _E03C = 168;

	internal const short _E03D = 170;

	internal const short _E03E = 180;

	internal const short _E03F = 181;

	internal const short _E040 = 182;

	internal const short _E041 = 183;

	[CompilerGenerated]
	private EMemberCategory _E042;

	[CompilerGenerated]
	private NetworkConnection _E043;

	[CompilerGenerated]
	private string _E044;

	[CompilerGenerated]
	private string _E045;

	internal static NetworkHash128 _E000 => NetworkHash128.Parse(_ED3E._E000(18705));

	public EMemberCategory MemberCategory
	{
		[CompilerGenerated]
		get
		{
			return _E042;
		}
		[CompilerGenerated]
		protected set
		{
			_E042 = value;
		}
	}

	public NetworkConnection Connection
	{
		[CompilerGenerated]
		get
		{
			return _E043;
		}
		[CompilerGenerated]
		protected set
		{
			_E043 = value;
		}
	}

	public string ProfileId
	{
		[CompilerGenerated]
		get
		{
			return _E044;
		}
		[CompilerGenerated]
		private set
		{
			_E044 = value;
		}
	}

	public string Token
	{
		[CompilerGenerated]
		get
		{
			return _E045;
		}
		[CompilerGenerated]
		private set
		{
			_E045 = value;
		}
	}

	protected static _E077 _E000<_E077>(Transform parent, string name, string profileId, string token) where _E077 : AbstractSession
	{
		GameObject obj = new GameObject();
		obj.name = name;
		obj.transform.parent = parent;
		_E077 val = obj.AddComponent<_E077>();
		val.ProfileId = profileId;
		val.Token = token;
		return val;
	}

	public virtual void ProfileResourcesLoadProgress(int id, float progress)
	{
	}

	protected virtual void OnDestroy()
	{
	}

	private void _E001()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool result = default(bool);
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}

	public override void PreStartClient()
	{
	}
}
