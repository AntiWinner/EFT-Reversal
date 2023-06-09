using System.Threading.Tasks;
using EFT.UI;
using TMPro;
using UnityEngine;

namespace EFT.Hideout;

public sealed class BoostPanel : AbstractPanel<Requirement>
{
	[SerializeField]
	private RequirementsPanel _boostRequirementsPanel;

	[SerializeField]
	private DefaultUIButton _boostButton;

	[SerializeField]
	private TextMeshProUGUI _boostInfo;

	private void Awake()
	{
		_boostButton.OnClick.AddListener(delegate
		{
			Debug.LogError(_ED3E._E000(165519));
		});
	}

	public override async Task ShowContents()
	{
		await _boostRequirementsPanel.Show(base.RelatedData, base.Stage, base.LevelType, base.AreaData, base.Player, base.Session);
	}

	public override void SetInfo()
	{
	}

	public override void Close()
	{
		_boostRequirementsPanel.Close();
		base.Close();
	}
}
