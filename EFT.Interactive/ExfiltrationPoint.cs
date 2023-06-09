using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.Interactive;

public class ExfiltrationPoint : MonoBehaviour, IPhysicsTrigger, _E633
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public string profileId;

		internal string _E000(ExfiltrationRequirement x)
		{
			return x.GetLocalizedTip(profileId);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public string playerId;

		internal bool _E000(Player x)
		{
			return x.ProfileId == playerId;
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public Player player;

		public ExfiltrationPoint _003C_003E4__this;

		internal bool _E000(ExfiltrationRequirement x)
		{
			return !x.Met(player, _003C_003E4__this);
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public string profileId;

		internal bool _E000(Player x)
		{
			return x.ProfileId == profileId;
		}
	}

	public Collider ExtendedCollider;

	public Switch Switch;

	private string m__E000 = "";

	private readonly List<string> m__E001 = new List<string>();

	[SerializeField]
	private EExfiltrationStatus _status = EExfiltrationStatus.Pending;

	[CompilerGenerated]
	private readonly string m__E002 = _ED3E._E000(212006);

	[CompilerGenerated]
	private float m__E003;

	[CompilerGenerated]
	private int m__E004;

	public float ExfiltrationStartTime;

	[CompilerGenerated]
	private Action<ExfiltrationPoint, EExfiltrationStatus> m__E005;

	[SerializeField]
	private GameObject _root;

	[NonSerialized]
	public List<string> QueuedPlayers = new List<string>();

	public ExitTriggerSettings Settings = new ExitTriggerSettings();

	public ExfiltrationRequirement[] Requirements = new ExfiltrationRequirement[0];

	[CompilerGenerated]
	private Action<ExfiltrationPoint, Player> m__E006;

	[CompilerGenerated]
	private Action<ExfiltrationPoint, Player> m__E007;

	public readonly _ECEF<Player> Entered = new _ECEF<Player>();

	public string[] EligibleEntryPoints = new string[0];

	public bool Reusable;

	private BoxCollider _E008;

	private Coroutine _E009;

	private Coroutine _E00A;

	private bool _E00B;

	private List<Switch> _E00C;

	public string Description
	{
		[CompilerGenerated]
		get
		{
			return this.m__E002;
		}
	}

	public float FenceRep
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

	public int CharismaLevel
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

	public EExfiltrationStatus Status
	{
		get
		{
			return _status;
		}
		set
		{
			if (ExtendedCollider != null)
			{
				ExtendedCollider.enabled = value == EExfiltrationStatus.UncompleteRequirements;
			}
			if (value != EExfiltrationStatus.Countdown)
			{
				ExfiltrationStartTime = -1E-45f;
			}
			if (_status == value)
			{
				return;
			}
			if (value == EExfiltrationStatus.NotPresent)
			{
				Disable(_status);
			}
			else
			{
				Enable();
			}
			EExfiltrationStatus status = _status;
			_status = value;
			if (this.m__E005 != null)
			{
				this.m__E005(this, status);
			}
			foreach (Player item in Entered)
			{
				Proceed(item);
			}
		}
	}

	public _EBF1 TransferItemRequirement => Requirements.OfType<_EBF1>().FirstOrDefault();

	public bool HasRequirements => Requirements.Any();

	public event Action<ExfiltrationPoint, EExfiltrationStatus> OnStatusChanged
	{
		[CompilerGenerated]
		add
		{
			Action<ExfiltrationPoint, EExfiltrationStatus> action = this.m__E005;
			Action<ExfiltrationPoint, EExfiltrationStatus> action2;
			do
			{
				action2 = action;
				Action<ExfiltrationPoint, EExfiltrationStatus> value2 = (Action<ExfiltrationPoint, EExfiltrationStatus>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E005, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<ExfiltrationPoint, EExfiltrationStatus> action = this.m__E005;
			Action<ExfiltrationPoint, EExfiltrationStatus> action2;
			do
			{
				action2 = action;
				Action<ExfiltrationPoint, EExfiltrationStatus> value2 = (Action<ExfiltrationPoint, EExfiltrationStatus>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E005, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<ExfiltrationPoint, Player> OnStartExtraction
	{
		[CompilerGenerated]
		add
		{
			Action<ExfiltrationPoint, Player> action = this.m__E006;
			Action<ExfiltrationPoint, Player> action2;
			do
			{
				action2 = action;
				Action<ExfiltrationPoint, Player> value2 = (Action<ExfiltrationPoint, Player>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E006, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<ExfiltrationPoint, Player> action = this.m__E006;
			Action<ExfiltrationPoint, Player> action2;
			do
			{
				action2 = action;
				Action<ExfiltrationPoint, Player> value2 = (Action<ExfiltrationPoint, Player>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E006, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<ExfiltrationPoint, Player> OnCancelExtraction
	{
		[CompilerGenerated]
		add
		{
			Action<ExfiltrationPoint, Player> action = this.m__E007;
			Action<ExfiltrationPoint, Player> action2;
			do
			{
				action2 = action;
				Action<ExfiltrationPoint, Player> value2 = (Action<ExfiltrationPoint, Player>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E007, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<ExfiltrationPoint, Player> action = this.m__E007;
			Action<ExfiltrationPoint, Player> action2;
			do
			{
				action2 = action;
				Action<ExfiltrationPoint, Player> value2 = (Action<ExfiltrationPoint, Player>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E007, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private void Awake()
	{
		base.gameObject.layer = LayerMask.NameToLayer(_ED3E._E000(25347));
		_E008 = GetComponent<BoxCollider>();
		_E008.size = Vector3.Scale(_E008.size, base.transform.localScale);
		base.transform.localScale = Vector3.one;
	}

	public string[] GetTips(string profileId)
	{
		if (!string.IsNullOrEmpty(this.m__E000) && Requirements.OfType<_EBF9>().Any())
		{
			return new string[1] { this.m__E000.Localized() };
		}
		return (from x in UnmetRequirements(profileId)
			select x.GetLocalizedTip(profileId)).ToArray();
	}

	public virtual bool InfiltrationMatch(Player player)
	{
		if (!string.IsNullOrEmpty(player.Profile.Info.EntryPoint))
		{
			return EligibleEntryPoints.Contains(player.Profile.Info.EntryPoint.ToLower());
		}
		return false;
	}

	public void LoadSettings(_E556 settings, bool authority)
	{
		_E00B = authority;
		Settings.Load(settings, this);
		if (settings.PassageRequirement == ERequirementState.Reference)
		{
			if (Singleton<_E5CB>.Instance.RequirementReferences.ContainsKey(settings.Id))
			{
				Requirements = Singleton<_E5CB>.Instance.RequirementReferences[settings.Id].Select(delegate(ExfiltrationRequirement x)
				{
					ExfiltrationRequirement exfiltrationRequirement2 = (ExfiltrationRequirement)ExfiltrationRequirement.CreateRequirement(x.Requirement);
					if (exfiltrationRequirement2 != null)
					{
						exfiltrationRequirement2.Requirement = x.Requirement;
						exfiltrationRequirement2.Count = x.Count;
						exfiltrationRequirement2.Id = x.Id;
						exfiltrationRequirement2.RequiredSlot = x.RequiredSlot;
						exfiltrationRequirement2.RequirementTip = x.RequirementTip;
						exfiltrationRequirement2.Start(this);
					}
					return exfiltrationRequirement2;
				}).ToArray();
			}
			else
			{
				Debug.LogError(settings.Id + _ED3E._E000(209796));
			}
		}
		else
		{
			ExfiltrationRequirement exfiltrationRequirement = (ExfiltrationRequirement)ExfiltrationRequirement.CreateRequirement(settings.PassageRequirement);
			if (exfiltrationRequirement != null)
			{
				exfiltrationRequirement.Requirement = settings.PassageRequirement;
				exfiltrationRequirement.Count = settings.Count;
				exfiltrationRequirement.Id = settings.Id;
				exfiltrationRequirement.RequiredSlot = settings.RequiredSlot;
				exfiltrationRequirement.RequirementTip = settings.RequirementTip;
				exfiltrationRequirement.Start(this);
				Requirements = new ExfiltrationRequirement[1] { exfiltrationRequirement };
			}
			else
			{
				Requirements = new ExfiltrationRequirement[0];
			}
		}
		EligibleEntryPoints = (string.IsNullOrEmpty(settings.EntryPoints) ? new string[0] : settings.EntryPoints.ToLower().Split(','));
		SetInitialStatus();
		_E002();
		Entered.ItemAdded += _E001;
		Entered.ItemRemoved += _E000;
	}

	private void _E000(Player obj)
	{
		ExfiltrationRequirement[] requirements = Requirements;
		for (int i = 0; i < requirements.Length; i++)
		{
			requirements[i].Exit(obj, this);
		}
	}

	private void _E001(Player obj)
	{
		ExfiltrationRequirement[] requirements = Requirements;
		for (int i = 0; i < requirements.Length; i++)
		{
			requirements[i].Enter(obj, this);
		}
	}

	public void SetInitialStatus()
	{
		SetStatusLogged((Settings.ExfiltrationType == EExfiltrationType.SharedTimer || Requirements.OfType<_EBF9>().Any()) ? EExfiltrationStatus.UncompleteRequirements : EExfiltrationStatus.RegularMode, _ED3E._E000(209840));
	}

	private void _E002()
	{
		_E00C = new List<Switch>();
		if (Switch == null)
		{
			return;
		}
		Switch @switch = Switch;
		while (@switch != null && !_E00C.Contains(@switch))
		{
			_E00C.Add(@switch);
			@switch = @switch.PreviousSwitch;
		}
		foreach (Switch item in _E00C)
		{
			item.OnDoorStateChanged += _E004;
		}
		this.m__E000 = Switch.GetTip();
	}

	private void _E003()
	{
		if (_E00C == null)
		{
			return;
		}
		foreach (Switch item in _E00C)
		{
			item.OnDoorStateChanged -= _E004;
		}
	}

	private void _E004(WorldInteractiveObject arg1, EDoorState arg2, EDoorState arg3)
	{
		if (arg2 == arg3)
		{
			return;
		}
		string tip = Switch.GetTip();
		if (tip == this.m__E000)
		{
			return;
		}
		this.m__E000 = tip;
		foreach (Player item in Entered)
		{
			Proceed(item);
		}
	}

	public void OnItemTransferred(Player player)
	{
		QueuedPlayers.Add(player.ProfileId);
		player.OnPlayerDead += _E005;
		foreach (Player item in Entered)
		{
			Proceed(item);
		}
	}

	public void OnItemTransferred(string playerId)
	{
		QueuedPlayers.Add(playerId);
		Player player = Singleton<GameWorld>.Instance.RegisteredPlayers.FirstOrDefault((Player x) => x.ProfileId == playerId);
		if (player != null)
		{
			player.OnPlayerDead += _E005;
		}
		foreach (Player item in Entered)
		{
			Proceed(item);
		}
	}

	private void _E005(Player player, Player lastAgressor, _EC23 damage, EBodyPart part)
	{
		player.OnPlayerDead -= _E005;
		if (!QueuedPlayers.Contains(player.ProfileId))
		{
			return;
		}
		QueuedPlayers.Remove(player.ProfileId);
		if (Entered.Contains(player))
		{
			Entered.Remove(player);
		}
		if (QueuedPlayers.Count == 0)
		{
			SetStatusLogged(EExfiltrationStatus.UncompleteRequirements, _ED3E._E000(209825));
			return;
		}
		foreach (Player item in Entered)
		{
			Proceed(item);
		}
	}

	public void CooperationPlayerDied(Player player, Player lastAgressor, _EC23 damage, EBodyPart part)
	{
		if (Entered.Contains(player))
		{
			Entered.Remove(player);
		}
	}

	public bool TransferExitItem(Player player, Item item)
	{
		_EBF1 transferItemRequirement = TransferItemRequirement;
		if (QueuedPlayers.Count >= Settings.PlayersCount && Settings.PlayersCount > 0)
		{
			return false;
		}
		return transferItemRequirement.TransferExitItem(player, item);
	}

	public void Enable()
	{
		base.gameObject.SetActive(value: true);
		if (_root != null)
		{
			_root.SmartEnable();
		}
	}

	private void _E006()
	{
		if (!(Switch == null))
		{
			Switch @switch = Switch;
			while (@switch != null)
			{
				@switch.Operatable = false;
				@switch = @switch.PreviousSwitch;
			}
		}
	}

	public void Disable(EExfiltrationStatus prevStatus)
	{
		if (prevStatus == EExfiltrationStatus.Pending || !Reusable)
		{
			Disable();
			_E006();
		}
		Entered.Clear();
		ExfiltrationStartTime = -1E-45f;
	}

	public void Disable()
	{
		base.gameObject.SetActive(value: false);
		if (_root != null)
		{
			_root.SmartDisable();
		}
		Entered.Clear();
		if (_E00A != null)
		{
			StopCoroutine(_E00A);
			_E00A = null;
		}
		if (_E009 != null)
		{
			StopCoroutine(_E009);
			_E009 = null;
		}
		_E003();
	}

	private void OnDestroy()
	{
		ExfiltrationRequirement[] requirements = Requirements;
		for (int i = 0; i < requirements.Length; i++)
		{
			requirements[i].OnDestroy();
		}
		Requirements = new ExfiltrationRequirement[0];
	}

	public IEnumerable<ExfiltrationRequirement> UnmetRequirements(Player player)
	{
		if (!(player == null))
		{
			return Requirements.Where((ExfiltrationRequirement x) => !x.Met(player, this));
		}
		return Requirements;
	}

	public IEnumerable<ExfiltrationRequirement> UnmetRequirements(string profileId)
	{
		Player player = Singleton<GameWorld>.Instance.RegisteredPlayers.FirstOrDefault((Player x) => x.ProfileId == profileId);
		if (player == null)
		{
			_E57A.Instance.LogDebug(_ED3E._E000(209868), profileId);
		}
		return UnmetRequirements(player);
	}

	public void Proceed(Player player, bool manuallyStarted = false)
	{
		if (this.m__E001.Contains(player.ProfileId))
		{
			this.m__E001.Remove(player.ProfileId);
		}
		bool num = !UnmetRequirements(player).ToArray().Any();
		if (num)
		{
			if (!this.m__E001.Contains(player.ProfileId))
			{
				this.m__E001.Add(player.ProfileId);
			}
		}
		else
		{
			this.m__E007?.Invoke(this, player);
		}
		FenceRep = player.Profile.FenceInfo.FenceLoyalty.ExfiltrationPriceModifier;
		CharismaLevel = player.Profile.Skills.Charisma.Level;
		player.ExitTriggerStatusChanged(status: true);
		player.SetExfiltrationPoint(this, Status != EExfiltrationStatus.NotPresent);
		if (!num)
		{
			return;
		}
		if (Status == EExfiltrationStatus.UncompleteRequirements)
		{
			switch (Settings.ExfiltrationType)
			{
			case EExfiltrationType.SharedTimer:
				SetStatusLogged(EExfiltrationStatus.Countdown, _ED3E._E000(211995));
				break;
			case EExfiltrationType.Manual:
				SetStatusLogged(EExfiltrationStatus.AwaitsManualActivation, _ED3E._E000(211981));
				break;
			case EExfiltrationType.Individual:
				SetStatusLogged(EExfiltrationStatus.RegularMode, _ED3E._E000(211975));
				break;
			}
		}
		if (Status == EExfiltrationStatus.RegularMode)
		{
			this.m__E006?.Invoke(this, player);
		}
	}

	void IPhysicsTrigger.OnTriggerEnter(Collider col)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
		if (!(playerByCollider == null) && InfiltrationMatch(playerByCollider) && !Entered.Contains(playerByCollider))
		{
			Entered.Add(playerByCollider);
			Proceed(playerByCollider);
		}
	}

	void IPhysicsTrigger.OnTriggerExit(Collider col)
	{
		if (!base.enabled)
		{
			return;
		}
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
		if (!(playerByCollider == null))
		{
			Entered.Remove(playerByCollider);
			if (InfiltrationMatch(playerByCollider))
			{
				this.m__E007?.Invoke(this, playerByCollider);
				playerByCollider.ExitTriggerStatusChanged(status: false);
				playerByCollider.SetExfiltrationPoint(this, entered: false);
			}
		}
	}

	public bool HasMetRequirements(string profileId)
	{
		return this.m__E001.Contains(profileId);
	}

	public void ExternalSetStatus(EExfiltrationStatus targetStatus, string signature = "")
	{
		if (_E00B)
		{
			_E57A.Instance.LogDebug(_ED3E._E000(212025), Settings.Name, signature, Status, targetStatus);
			Status = targetStatus;
		}
	}

	public void SetStatusLogged(EExfiltrationStatus targetStatus, string signature = "")
	{
		_E57A.Instance.LogDebug(_ED3E._E000(212025), Settings.Name, signature, Status, targetStatus);
		Status = targetStatus;
	}

	[SpecialName]
	bool IPhysicsTrigger.get_enabled()
	{
		return base.enabled;
	}

	[SpecialName]
	void IPhysicsTrigger.set_enabled(bool value)
	{
		base.enabled = value;
	}

	[CompilerGenerated]
	private ExfiltrationRequirement _E007(ExfiltrationRequirement x)
	{
		ExfiltrationRequirement exfiltrationRequirement = (ExfiltrationRequirement)ExfiltrationRequirement.CreateRequirement(x.Requirement);
		if (exfiltrationRequirement != null)
		{
			exfiltrationRequirement.Requirement = x.Requirement;
			exfiltrationRequirement.Count = x.Count;
			exfiltrationRequirement.Id = x.Id;
			exfiltrationRequirement.RequiredSlot = x.RequiredSlot;
			exfiltrationRequirement.RequirementTip = x.RequirementTip;
			exfiltrationRequirement.Start(this);
		}
		return exfiltrationRequirement;
	}
}
