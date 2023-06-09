using System.Text;
using TMPro;
using UnityEngine;

public class SpawnsFieldData : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _aliveField;

	[SerializeField]
	private TextMeshProUGUI _groupsField;

	public void UpdateData(_E106 spawn)
	{
		string text = ((spawn.SpawnBundlesWait == 0) ? "" : string.Format(_ED3E._E000(48666), spawn.SpawnBundlesWait));
		string text2 = string.Format(_ED3E._E000(48656), spawn.Alive, spawn.Delayed, spawn.ProfileWait, text, spawn.SpawnProcess, spawn.Hour);
		_aliveField.text = text2;
		if (spawn.DelayedCounts != null && spawn.DelayedCounts.Length != 0)
		{
			StringBuilder stringBuilder = new StringBuilder(_ED3E._E000(48728));
			for (int i = 0; i < spawn.DelayedCounts.Length; i++)
			{
				int num = spawn.DelayedCounts[i];
				stringBuilder.Append(string.Format(_ED3E._E000(48713), num));
			}
			_groupsField.text = stringBuilder.ToString();
		}
		else
		{
			_groupsField.text = _ED3E._E000(48711);
		}
	}
}
