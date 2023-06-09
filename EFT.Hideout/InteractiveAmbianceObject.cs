using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EFT.Hideout;

public class InteractiveAmbianceObject<T> : AmbianceObject<T>, _E80A
{
	private bool m__E000;

	private bool _E001 = true;

	[CompilerGenerated]
	private ELightStatus _E002;

	[CompilerGenerated]
	private ELightStatus _E003;

	[CompilerGenerated]
	private bool _E004 = true;

	[CompilerGenerated]
	private bool _E005;

	protected ELightStatus PreviousStatus
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
		[CompilerGenerated]
		private set
		{
			_E002 = value;
		}
	}

	protected ELightStatus CurrentStatus
	{
		[CompilerGenerated]
		get
		{
			return _E003;
		}
		[CompilerGenerated]
		private set
		{
			_E003 = value;
		}
	}

	public ELightStatus CombinedStatus
	{
		get
		{
			if (this._E001)
			{
				Active = base.gameObject.activeInHierarchy;
				this._E001 = false;
			}
			if (Enabled && Active)
			{
				return CurrentStatus;
			}
			if (CurrentStatus == ELightStatus.None)
			{
				return ELightStatus.None;
			}
			return ELightStatus.OutOfFuel;
		}
	}

	public bool Enabled
	{
		[CompilerGenerated]
		get
		{
			return _E004;
		}
		[CompilerGenerated]
		private set
		{
			_E004 = value;
		}
	}

	public bool Active
	{
		[CompilerGenerated]
		get
		{
			return _E005;
		}
		[CompilerGenerated]
		private set
		{
			_E005 = value;
		}
	}

	public void SetEnable(bool value)
	{
		if (Enabled != value)
		{
			Enabled = value;
			this._E000 = true;
			Interact();
			this._E000 = false;
		}
	}

	protected virtual void OnEnable()
	{
		Active = true;
		Interact();
	}

	protected virtual void OnDisable()
	{
		Active = false;
		Interact();
	}

	protected async void Interact()
	{
		await PerformInteraction(CurrentStatus);
	}

	public override async Task<bool> PerformInteraction(ELightStatus status)
	{
		if (status == ELightStatus.None || this == null)
		{
			return false;
		}
		CurrentStatus = status;
		if (!base.gameObject.activeInHierarchy)
		{
			return false;
		}
		if (PreviousStatus == CombinedStatus)
		{
			return false;
		}
		PreviousStatus = CombinedStatus;
		if (this._E000)
		{
			return ImmediateInteraction(CombinedStatus);
		}
		return await base.PerformInteraction(CombinedStatus);
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task<bool> _E000(ELightStatus status)
	{
		return base.PerformInteraction(status);
	}
}
