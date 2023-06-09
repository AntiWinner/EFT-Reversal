using System.Threading.Tasks;
using UnityEngine;

namespace EFT.Hideout;

public sealed class DescriptionPanel : AbstractPanel<string>
{
	[SerializeField]
	private CustomTextMeshProUGUI _descriptionLabel;

	public override async Task ShowContents()
	{
		SetInfo();
		await Task.Yield();
	}

	public override void SetInfo()
	{
		_descriptionLabel.text = string.Format(_ED3E._E000(165559), (int)base.AreaData.Template.Type, base.Stage.Level).Localized();
	}
}
