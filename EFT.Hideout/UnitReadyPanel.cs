using System.Threading.Tasks;
using UnityEngine;

namespace EFT.Hideout;

public sealed class UnitReadyPanel : AbstractPanel<EAreaStatus>
{
	private const string _E000 = "Unit is ready to be installed";

	[SerializeField]
	private CustomTextMeshProUGUI _statusLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _descriptionLabel;

	public override async Task ShowContents()
	{
		SetInfo();
		await Task.Yield();
	}

	public override void SetInfo()
	{
		_statusLabel.text = base.AreaData.Status.ToString().Localized().ToUpper();
		_descriptionLabel.text = _ED3E._E000(164030).Localized().ToUpper();
	}
}
