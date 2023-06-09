using UnityEngine;

namespace EFT.UI;

public class RepairWarningStatus : UIElement
{
	[SerializeField]
	private CustomTextMeshProUGUI _label;

	public void Show(string text)
	{
		ShowGameObject();
		_label.text = text;
	}

	public override void Close()
	{
		base.Close();
		Object.Destroy(base.gameObject);
	}
}
