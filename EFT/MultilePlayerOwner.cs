using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EFT.InputSystem;
using UnityEngine;

namespace EFT;

public class MultilePlayerOwner : InputNode
{
	protected enum EState
	{
		None,
		Started,
		Stopped
	}

	private List<ExtendedPlayerOwner> _E01F;

	[CompilerGenerated]
	private _E7FD _E020;

	[CompilerGenerated]
	private EState _E021;

	public _E7FD InputTree
	{
		[CompilerGenerated]
		get
		{
			return _E020;
		}
		[CompilerGenerated]
		private set
		{
			_E020 = value;
		}
	}

	protected EState State
	{
		[CompilerGenerated]
		get
		{
			return _E021;
		}
		[CompilerGenerated]
		private set
		{
			_E021 = value;
		}
	}

	internal static MultilePlayerOwner _E000(Player[] players, _E7FD inputTree)
	{
		MultilePlayerOwner multilePlayerOwner = new GameObject(typeof(MultilePlayerOwner).Name).AddComponent<MultilePlayerOwner>();
		multilePlayerOwner._E01F = new List<ExtendedPlayerOwner>();
		multilePlayerOwner.InputTree = inputTree;
		for (int i = 0; i < players.Length; i++)
		{
			ExtendedPlayerOwner item = ExtendedPlayerOwner._E000(players[i]);
			multilePlayerOwner._E01F.Add(item);
		}
		return multilePlayerOwner;
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		for (int i = 0; i < _E01F.Count; i++)
		{
			_E01F[i].ExtendedTranslateCommand(command);
		}
		if (command.IsCommand(ECommand.ChangePointOfView))
		{
			for (int j = 0; j < _E01F.Count; j++)
			{
				Player player = _E01F[j].Player;
				player.PointOfView = ((player.PointOfView == EPointOfView.FirstPerson) ? EPointOfView.ThirdPerson : EPointOfView.FirstPerson);
			}
		}
		return ETranslateResult.Ignore;
	}

	protected override void TranslateAxes(ref float[] axes)
	{
		for (int i = 0; i < _E01F.Count; i++)
		{
			_E01F[i].ExtendedTranslateAxes(ref axes);
		}
	}

	protected override ECursorResult ShouldLockCursor()
	{
		return ECursorResult.LockCursor;
	}

	internal void _E001()
	{
		foreach (ExtendedPlayerOwner item in _E01F)
		{
			item._E022();
		}
		InputTree.Add(this);
		State = EState.Started;
	}

	internal virtual void _E023()
	{
		if (State == EState.Started)
		{
			InputTree.Remove(this);
			State = EState.Stopped;
		}
	}
}
