using System.Collections;
using UnityEngine;

namespace EFT.UI;

public class BattleUIPanel : UIElement
{
	[SerializeField]
	public CanvasGroup _infoPanel;

	protected IEnumerator Co_ShowInfoPanel(float delay, float speed)
	{
		_infoPanel.alpha = 0f;
		yield return new WaitForSeconds(delay);
		while (_infoPanel.alpha < 1f)
		{
			_infoPanel.alpha += speed * Time.deltaTime;
			yield return null;
		}
	}
}
