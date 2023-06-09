using UnityEngine.UI;

namespace EFT.UI;

public class CustomToggle : Toggle
{
	public Graphic graphicDisabled;

	protected override void Awake()
	{
		onValueChanged.AddListener(_E000);
	}

	private void _E000(bool value)
	{
		if (graphicDisabled != null)
		{
			graphicDisabled.enabled = !value;
		}
	}
}
