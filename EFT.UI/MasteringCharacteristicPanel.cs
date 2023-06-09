namespace EFT.UI;

public class MasteringCharacteristicPanel : CharacteristicPanel
{
	public int MaxVal = 1500;

	public override void SetValues()
	{
		base.SetValues();
		ValueText.text = string.Format(_ED3E._E000(182604), ItemAttribute.Base(), MaxVal);
	}
}
