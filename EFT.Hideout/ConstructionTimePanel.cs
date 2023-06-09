using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;

namespace EFT.Hideout;

public sealed class ConstructionTimePanel : AbstractPanel<float>
{
	private const string _E000 = "HOURS";

	private const string _E001 = "INSTANT";

	[SerializeField]
	private CustomTextMeshProUGUI _constructTimeLabel;

	public override async Task ShowContents()
	{
		SetInfo();
		await Task.Yield();
	}

	public override void SetInfo()
	{
		if (base.Info <= 0f)
		{
			_constructTimeLabel.text = _ED3E._E000(165567).Localized().ToUpper();
		}
		else
		{
			_constructTimeLabel.text = Mathf.RoundToInt(base.Info / 3600f).ToString(CultureInfo.InvariantCulture) + _ED3E._E000(18502) + _ED3E._E000(103409).Localized();
		}
	}
}
