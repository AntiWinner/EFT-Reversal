using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.UI;

public sealed class BanTimeWindow : DialogWindow<_EC7B>
{
	[SerializeField]
	private SelectSlider _selectSlider;

	private Action<int> m__E000;

	protected override void Awake()
	{
		base.Awake();
		base.AcceptButton.OnClick.RemoveAllListeners();
		base.AcceptButton.OnClick.AddListener(delegate
		{
			this.m__E000(_selectSlider.CurrentValue());
			Close();
		});
	}

	public _EC7B Show(string[] notches, Action<int> acceptAction, Action cancelAction)
	{
		_EC7B result = Show(_ED3E._E000(250339).Localized(), delegate
		{
		}, cancelAction);
		this.m__E000 = acceptAction;
		_selectSlider.Show(notches);
		_selectSlider.UpdateValue(0);
		UI.AddDisposable(_selectSlider);
		return result;
	}

	[CompilerGenerated]
	private void _E000()
	{
		this.m__E000(_selectSlider.CurrentValue());
		Close();
	}
}
