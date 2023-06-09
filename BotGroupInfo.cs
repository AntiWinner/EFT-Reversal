using TMPro;
using UnityEngine;

public class BotGroupInfo : MonoBehaviour
{
	private _E10D _E000;

	public TextMeshProUGUI field;

	public void UpdateData(_E10D group)
	{
		_E000 = group;
		field.text = _E000.MessageInfo();
		base.gameObject.SetActive(value: true);
	}
}
