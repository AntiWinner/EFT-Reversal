using EFT.InputSystem;

namespace EFT;

public class ExtendedPlayerOwner : PlayerOwner
{
	private new sealed class _E000 : _E7FD
	{
		public void AddWithLowPriority(InputNode node)
		{
		}

		public void Add(InputNode node)
		{
		}

		public void Remove(InputNode node)
		{
		}

		public bool Contains(InputNode node)
		{
			return false;
		}
	}

	internal static ExtendedPlayerOwner _E000(Player player)
	{
		return PlayerOwner._E000<ExtendedPlayerOwner>(player, new _E000());
	}

	public ETranslateResult ExtendedTranslateCommand(ECommand command)
	{
		return TranslateCommand(command);
	}

	public void ExtendedTranslateAxes(ref float[] axes)
	{
		TranslateAxes(ref axes);
	}

	public ECursorResult ExtendedShouldLockCursor()
	{
		return ShouldLockCursor();
	}
}
