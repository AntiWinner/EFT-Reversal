using EFT.UI;
using UnityEngine;

public class ShortStatsPanel : MonoBehaviour
{
	[SerializeField]
	private LocalizedText _raids;

	[SerializeField]
	private LocalizedText _kills;

	[SerializeField]
	private LocalizedText _leaveRate;

	[SerializeField]
	private LocalizedText _survivalRate;

	[SerializeField]
	private LocalizedText _killsDeathRate;

	[SerializeField]
	private LocalizedText _onlineTime;

	public void SetStats(float raids, float kills, float leaveRate, float survivalRate, float killsDeathRate, string onlineTime)
	{
		_raids.SetFormatValues(raids);
		_kills.SetFormatValues(kills);
		_leaveRate.SetFormatValues(leaveRate);
		_survivalRate.SetFormatValues(survivalRate);
		_killsDeathRate.SetFormatValues(killsDeathRate);
		_onlineTime.SetFormatValues(onlineTime);
	}
}
