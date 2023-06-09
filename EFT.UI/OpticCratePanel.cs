using UnityEngine;

namespace EFT.UI;

[RequireComponent(typeof(BattleUIComponentAnimation))]
public class OpticCratePanel : UIElement
{
	[SerializeField]
	private CustomTextMeshProUGUI _currentCrate;

	private BattleUIComponentAnimation _E093;

	public void Show(string message)
	{
		ShowGameObject();
		if (_E093 == null)
		{
			_E093 = base.gameObject.GetComponent<BattleUIComponentAnimation>();
		}
		_currentCrate.text = message;
		_E093.Show(autoHide: true).HandleExceptions();
	}

	public void Hide()
	{
		if (_E093 != null)
		{
			_E093.Hide().HandleExceptions();
		}
	}

	public override void Close()
	{
		if (_E093 != null)
		{
			_E093.Close();
		}
		base.Close();
	}
}
