using System;
using TMPro;
using UnityEngine;

namespace EFT.UI;

[RequireComponent(typeof(BattleUIComponentAnimation))]
public sealed class AzimuthPanel : UIElement
{
	[SerializeField]
	private TextMeshProUGUI _azimuthField;

	private BattleUIComponentAnimation _E093;

	private Func<int> _E0BA;

	private int _E0BB;

	public void Show(Func<int> azimuthFunc)
	{
		_E0BA = azimuthFunc;
		ShowGameObject();
		if (_E093 == null)
		{
			_E093 = base.gameObject.GetComponent<BattleUIComponentAnimation>();
		}
		_E093.Show(autoHide: false).HandleExceptions();
	}

	public void Hide()
	{
		if (_E093 != null)
		{
			_E093.Hide().HandleExceptions();
		}
	}

	private void LateUpdate()
	{
		if (_E0BA != null)
		{
			int num = _E0BA();
			if (num < 0)
			{
				num += 360;
			}
			if (num != _E0BB)
			{
				_E0BB = num;
				_azimuthField.SetMonospaceText(num.ToString());
			}
		}
	}

	private void OnDestroy()
	{
		_E0BA = null;
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
