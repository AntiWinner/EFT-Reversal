using System.Runtime.CompilerServices;
using Comfort.Common;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class ReportPanel : UIElement
{
	[SerializeField]
	private Button _reportDropdownButton;

	[SerializeField]
	private Button _bugAbuseReportButton;

	[SerializeField]
	private Button _cheatUseReportButton;

	[SerializeField]
	private Button _offensiveNicknameButton;

	[SerializeField]
	private GameObject _reportDropdownPanel;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	private bool _E17E;

	private _E796 _E17F;

	private void Awake()
	{
		_reportDropdownButton.onClick.AddListener(_E000);
		_bugAbuseReportButton.onClick.AddListener(delegate
		{
			_E001(EReportType.Abuse);
		});
		_cheatUseReportButton.onClick.AddListener(delegate
		{
			_E001(EReportType.Cheat);
		});
		_offensiveNicknameButton.onClick.AddListener(delegate
		{
			_E001(EReportType.OffensiveNickname);
		});
	}

	public void Show(_E796 session)
	{
		ShowGameObject();
		_E17F = session;
	}

	private void _E000()
	{
		_E17E = !_E17E;
		_reportDropdownPanel.SetActive(_E17E);
	}

	private void _E001(EReportType reportType)
	{
		_E17F.SendReport(reportType, delegate(IResult result)
		{
			if (!result.Succeed)
			{
				_canvasGroup.SetUnlockStatus(value: true);
			}
		});
		_canvasGroup.SetUnlockStatus(value: false);
		_E000();
	}

	private void Update()
	{
		if (_E17E && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !RectTransformUtility.RectangleContainsScreenPoint((RectTransform)_reportDropdownPanel.transform, Input.mousePosition))
		{
			_E000();
		}
	}

	public override void Close()
	{
		if (_E17E)
		{
			_E000();
		}
		base.Close();
	}

	[CompilerGenerated]
	private void _E002()
	{
		_E001(EReportType.Abuse);
	}

	[CompilerGenerated]
	private void _E003()
	{
		_E001(EReportType.Cheat);
	}

	[CompilerGenerated]
	private void _E004()
	{
		_E001(EReportType.OffensiveNickname);
	}

	[CompilerGenerated]
	private void _E005(IResult result)
	{
		if (!result.Succeed)
		{
			_canvasGroup.SetUnlockStatus(value: true);
		}
	}
}
