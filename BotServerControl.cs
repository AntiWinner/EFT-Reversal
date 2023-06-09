using System.Runtime.CompilerServices;
using EFT;
using UnityEngine;

public class BotServerControl : MonoBehaviour
{
	private Vector3 m__E000;

	[CompilerGenerated]
	private BotOwner _E001;

	public BotOwner ControllableBot
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
		[CompilerGenerated]
		private set
		{
			_E001 = value;
		}
	}

	public void SetSpring(bool sprint)
	{
		ControllableBot.Mover.Sprint(sprint);
	}

	public void SetBotUnderControl(BotOwner bot)
	{
		ControllableBot = bot;
		if (ControllableBot != null)
		{
			ControllableBot.Brain.Deactivate();
		}
	}

	private void Update()
	{
		_E000();
	}

	private void _E000()
	{
		_ = ControllableBot == null;
	}
}
