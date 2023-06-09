using System.Text;
using EFT;
using TMPro;
using UnityEngine;

public class BotZoneDebugInfo : MonoBehaviour
{
	public TextMeshProUGUI field;

	public void UpdateData(string nameZone, int[] blocks)
	{
		base.gameObject.SetActive(value: true);
		StringBuilder stringBuilder = new StringBuilder(nameZone);
		stringBuilder.Append(_ED3E._E000(48582));
		foreach (int num in blocks)
		{
			if (num > 0 && num < 40)
			{
				stringBuilder.Append(((WildSpawnType)(num - 1)).ToString());
				stringBuilder.Append(',');
			}
		}
		string text = stringBuilder.ToString();
		if (field != null)
		{
			field.text = text;
		}
	}
}
