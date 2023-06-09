using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.Hideout;

public sealed class ChangeLightButton : ComplementaryButton
{
	[SerializeField]
	private ChangeLightPanel _changeLightPanel;

	[SerializeField]
	private Image _trigger;

	public void Show()
	{
		_changeLightPanel.OnButtonSelected += _E000;
		UI.AddDisposable(delegate
		{
			_changeLightPanel.OnButtonSelected -= _E000;
		});
		_changeLightPanel.Show();
		Show(delegate
		{
			PointerExit();
			_E001();
		}, delegate
		{
			_trigger.raycastTarget = true;
			_changeLightPanel.Display();
		}, delegate
		{
			_trigger.raycastTarget = false;
			_E001();
		});
	}

	private void _E000(Sprite sprite)
	{
		PointerExit();
		_E001();
		SetIcon(sprite);
	}

	private void _E001()
	{
		_changeLightPanel.Hide();
	}

	public override void SetSelectedStatus(bool selected)
	{
	}

	public override void Close()
	{
		_E001();
		base.Close();
	}

	[CompilerGenerated]
	private void _E002()
	{
		_changeLightPanel.OnButtonSelected -= _E000;
	}

	[CompilerGenerated]
	private void _E003(bool arg)
	{
		PointerExit();
		_E001();
	}

	[CompilerGenerated]
	private void _E004(PointerEventData arg)
	{
		_trigger.raycastTarget = true;
		_changeLightPanel.Display();
	}

	[CompilerGenerated]
	private void _E005()
	{
		_trigger.raycastTarget = false;
		_E001();
	}
}
