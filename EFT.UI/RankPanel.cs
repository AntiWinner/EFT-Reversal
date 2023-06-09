using UnityEngine;

namespace EFT.UI;

public sealed class RankPanel : UIElement
{
	[SerializeField]
	private CustomTextMeshProUGUI _rank;

	[SerializeField]
	private GameObject _eliteRank;

	public void Show(int rankLevel, int maxRank)
	{
		ShowGameObject();
		_rank.gameObject.SetActive(rankLevel < maxRank);
		_eliteRank.SetActive(rankLevel >= maxRank);
		_rank.text = _ECA1.GetTraderRankString(rankLevel);
	}
}
