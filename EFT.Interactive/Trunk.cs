using System.Runtime.CompilerServices;

namespace EFT.Interactive;

public class Trunk : WorldInteractiveObject
{
	[CompilerGenerated]
	private new sealed class _E000
	{
		public _EBFE interactionResult;

		public Trunk _003C_003E4__this;

		internal void _E000()
		{
			switch (interactionResult.InteractionType)
			{
			case EInteractionType.Open:
				if (_003C_003E4__this.DoorState != EDoorState.Locked)
				{
					_003C_003E4__this.Open();
				}
				break;
			case EInteractionType.Close:
				_003C_003E4__this.Close();
				break;
			case EInteractionType.Unlock:
				if (((_EBFF)interactionResult).Succeed)
				{
					_003C_003E4__this.Unlock();
				}
				break;
			case EInteractionType.Lock:
				_003C_003E4__this.Lock();
				break;
			case EInteractionType.Breach:
				break;
			}
		}
	}

	public override string TypeKey => _ED3E._E000(210958);

	public override void Interact(_EBFE interactionResult)
	{
		this.StartBehaviourTimer(EFTHardSettings.Instance.DelayToOpenContainer, delegate
		{
			switch (interactionResult.InteractionType)
			{
			case EInteractionType.Open:
				if (DoorState != EDoorState.Locked)
				{
					Open();
				}
				break;
			case EInteractionType.Close:
				Close();
				break;
			case EInteractionType.Unlock:
				if (((_EBFF)interactionResult).Succeed)
				{
					Unlock();
				}
				break;
			case EInteractionType.Lock:
				Lock();
				break;
			case EInteractionType.Breach:
				break;
			}
		});
	}
}
