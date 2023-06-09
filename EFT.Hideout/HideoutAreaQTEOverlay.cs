using System.Threading.Tasks;
using EFT.InputSystem;
using EFT.UI;
using EFT.UI.Screens;
using UnityEngine;

namespace EFT.Hideout;

[RequireComponent(typeof(QTEController))]
public sealed class HideoutAreaQTEOverlay : UIScreen, _E83E
{
	private const int _E045 = 100;

	public static QTEController QteController;

	private HideoutCameraController _E046;

	private AreaData _E02E;

	private HideoutScreenRear _E047;

	private HideoutPlayerOwner _E030;

	public void Show(HideoutPlayerOwner player, AreaData areaData, HideoutScreenRear hideoutRear)
	{
		UIEventSystem.Instance.Enable();
		_E02E = areaData;
		_E047 = hideoutRear;
		_E046 = hideoutRear.HideoutCameraController;
		_E030 = player;
		if (!QteController)
		{
			QteController = GetComponent<QTEController>();
		}
		ShowGameObject();
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command == ECommand.Escape)
		{
			if ((bool)QteController)
			{
				QteController.Stop();
				QteController = null;
			}
			return ETranslateResult.Block;
		}
		return ETranslateResult.Block;
	}

	public async Task SpecialAreaActionSelected(AreaData data, bool wait)
	{
		if (_E047.Closed)
		{
			return;
		}
		wait = wait && _E02E != data;
		_E000(data);
		if (_E02E != null)
		{
			if (wait)
			{
				await TasksExtensions.Delay(0.5f);
			}
			if (!_E047.Closed)
			{
				_E046.AreaSelected = true;
			}
		}
	}

	private void _E000(AreaData area)
	{
		bool flag = area != null;
		if (_E02E != null)
		{
			if (!flag)
			{
				_E046.SetAnimationTime(_E02E.Template.CameraTimePosition);
			}
			_E02E.SetSelectedStatus(isSelected: false);
			_E02E.SpecialActionCamera.Priority = 0;
			_E02E.AreaCamera.Priority = 0;
		}
		_E02E = area;
		if (flag)
		{
			_E02E.SpecialActionCamera.Priority = 100;
		}
		else
		{
			_E046.AreaSelected = false;
		}
	}

	protected override ECursorResult ShouldLockCursor()
	{
		return ECursorResult.LockCursor;
	}

	public override void Close()
	{
		if (_E02E != null)
		{
			_E02E.SpecialActionCamera.Priority = 0;
		}
		base.Close();
	}
}
