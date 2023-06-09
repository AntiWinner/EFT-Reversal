using System.Linq;

namespace EFT.Quests;

public sealed class ConditionExitStatus : Condition
{
	public ExitStatus[] status;

	public override string FormattedDescription => string.Format(base.FormattedDescription, base.value, status.First());
}
