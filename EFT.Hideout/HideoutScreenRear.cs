using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using DG.Tweening;
using EFT.InputSystem;
using EFT.UI;
using EFT.UI.Screens;
using UnityEngine;

namespace EFT.Hideout;

public sealed class HideoutScreenRear : EftAsyncScreen<HideoutScreenRear._E000, HideoutScreenRear>
{
	public new sealed class _E000 : _EC91<_E000, HideoutScreenRear>
	{
		internal new bool _E000;

		public readonly AreaData[] AreaDatas;

		public readonly HideoutPlayerOwner PlayerOwner;

		public readonly _E796 Session;

		public override EEftScreenType ScreenType => EEftScreenType.Hideout;

		public override bool KeyScreen => true;

		protected override EShadingStateSwitcher ShadingType => EShadingStateSwitcher.Hideout;

		protected override EStateSwitcher UnrestrictedFrameRate => EStateSwitcher.Enabled;

		protected override EStateSwitcher IgnorePlayerInput => EStateSwitcher.LastState;

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.Enabled;

		protected override EStateSwitcher ShowEnvironment => EStateSwitcher.Disabled;

		protected override EStateSwitcher EnvironmentOverlay => EStateSwitcher.Disabled;

		protected override EStateSwitcher ShowEnvironmentCamera => EStateSwitcher.Disabled;

		protected override EStateSwitcher CameraBlur => EStateSwitcher.Disabled;

		public _E000(AreaData[] areaDatas, HideoutPlayerOwner playerOwner, _E796 session)
		{
			AreaDatas = areaDatas;
			PlayerOwner = playerOwner;
			Session = session;
		}

		public void UpdateEnvironmentSettings()
		{
			PrepareEnvironment();
		}
	}

	private enum EAreaSpecialAction
	{
		Qte = 1
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public HideoutPlayerOwner playerOwner;

		public HideoutScreenRear _003C_003E4__this;

		public Action<AreaPanel> _003C_003E9__6;

		internal void _E000()
		{
			playerOwner.OnShootingRangeStatusChange -= _003C_003E4__this._E006;
		}

		internal AreaWorldPanel _E001(AreaData arg)
		{
			return _003C_003E4__this._areaPanelTemplate;
		}

		internal Transform _E002(AreaData arg)
		{
			return _003C_003E4__this._areaIconsContainer;
		}

		internal void _E003(AreaData item, AreaWorldPanel view)
		{
			view.Show(item, delegate(AreaPanel arg)
			{
				_003C_003E4__this._hideoutScreenOverlay.AreaSelected(arg.Data, wait: true).HandleExceptions();
			});
			view.SetPointOfView(_003C_003E4__this.FirstPerson);
			_003C_003E4__this.m__E000.Add(view);
			_003C_003E4__this.UI.AddDisposable(item.OnSelected.Subscribe(_003C_003E4__this._E006));
		}

		internal void _E004(AreaPanel arg)
		{
			_003C_003E4__this._hideoutScreenOverlay.AreaSelected(arg.Data, wait: true).HandleExceptions();
		}
	}

	[SerializeField]
	private AreaWorldPanel _areaPanelTemplate;

	[SerializeField]
	private Transform _areaIconsContainer;

	[SerializeField]
	private CanvasGroup _areaIconsContainerCanvasGroup;

	[SerializeField]
	private HideoutScreenOverlay _hideoutScreenOverlay;

	[SerializeField]
	private HideoutAreaQTEOverlay _hideoutAreaQTEScreen;

	[SerializeField]
	private Dictionary<EAreaSpecialAction, _E83E> _specialActionsScreens;

	public HideoutCameraController HideoutCameraController;

	private new readonly List<AreaWorldPanel> m__E000 = new List<AreaWorldPanel>();

	private bool m__E001;

	private _EC6E<AreaData, AreaWorldPanel> m__E002;

	private bool m__E003;

	private bool m__E004 = true;

	private AreaData[] m__E005;

	private HideoutPlayerOwner m__E006;

	private _E796 m__E007;

	public bool FirstPerson
	{
		get
		{
			return this.m__E004;
		}
		set
		{
			if (this.m__E004 != value)
			{
				this.m__E004 = value;
				ScreenController._E000 = this.m__E004;
				_E007();
			}
		}
	}

	public bool Closed => ScreenController.Closed;

	public override void Show(_E000 controller)
	{
		throw new NotImplementedException();
	}

	public override async Task ShowAsync(_E000 controller)
	{
		if (!base.gameObject.activeSelf)
		{
			await Show(controller.AreaDatas, controller.PlayerOwner, controller.Session, ScreenController._E000);
		}
	}

	private async Task Show(AreaData[] areaDatas, HideoutPlayerOwner playerOwner, _E796 session, bool firstPerson)
	{
		this.m__E005 = areaDatas;
		this.m__E006 = playerOwner;
		this.m__E007 = session;
		this.m__E004 = firstPerson;
		this.m__E006.OnSelectArea += _E001;
		this.m__E006.OnSpecialAreaActionSelection += _E002;
		this.m__E006.OnToggleInfoIcons += _E008;
		this.m__E006.OnExitQte += ExitQte;
		HideoutCameraController.CreateFakePlayerCollider(this.m__E006.HideoutPlayer);
		this.m__E000.Clear();
		ShowGameObject(instant: true);
		this.m__E002?.Dispose();
		this.m__E002 = new _EC6E<AreaData, AreaWorldPanel>();
		UI.AddDisposable(this.m__E002);
		this.m__E003 = _E838.InfoIconsStatus;
		foreach (AreaData areaData in Singleton<_E815>.Instance.AreaDatas)
		{
			UI.AddDisposable(areaData.VisibilityChanged.Subscribe(_E005));
		}
		playerOwner.OnShootingRangeStatusChange += _E006;
		UI.AddDisposable(delegate
		{
			playerOwner.OnShootingRangeStatusChange -= _E006;
		});
		UI.AddDisposable(this.m__E006.HideoutPlayer.NightVisionObserver.Changed.Subscribe(_E000));
		UI.AddDisposable(this.m__E006.HideoutPlayer.ThermalVisionObserver.Changed.Subscribe(_E000));
		_E005(_: true);
		AreaData selectedArea = this.m__E005.FirstOrDefault((AreaData data) => data.Selected);
		_E004(this.m__E004, returnToFirstPerson: false, selectedArea);
		await this.m__E002.Init(new _ECEF<AreaData>(areaDatas), (AreaData arg) => _areaPanelTemplate, (AreaData arg) => _areaIconsContainer, delegate(AreaData item, AreaWorldPanel view)
		{
			view.Show(item, delegate(AreaPanel arg)
			{
				_hideoutScreenOverlay.AreaSelected(arg.Data, wait: true).HandleExceptions();
			});
			view.SetPointOfView(FirstPerson);
			this.m__E000.Add(view);
			UI.AddDisposable(item.OnSelected.Subscribe(_E006));
		});
		if (!(this == null))
		{
			bool selected = this.m__E002.Any((KeyValuePair<AreaData, AreaWorldPanel> x) => x.Key.Selected);
			_E006(selected);
		}
	}

	private void _E000()
	{
		HideoutCameraController.NightVisionState = this.m__E006.HideoutPlayer.NightVisionActive;
	}

	private void _E001(HideoutArea area)
	{
		_E004(firstPerson: false, area != null, (area != null) ? area.Data : null);
	}

	private void _E002(HideoutArea area)
	{
		_E003(EAreaSpecialAction.Qte, (area != null) ? area.Data : null);
	}

	public void ExitQte()
	{
		MonoBehaviourSingleton<PreloaderUI>.Instance.StartBlackScreenShow(1f, 2f, delegate
		{
			MonoBehaviourSingleton<PreloaderUI>.Instance.FadeBlackScreen(1f, -2f);
			_hideoutAreaQTEScreen.Close();
			ReturnToHideoutMode();
		});
	}

	private void _E003(EAreaSpecialAction eSpecialAction, AreaData selectedArea = null)
	{
		SetPointOfView(firstPerson: false);
		ScreenController.UpdateEnvironmentSettings();
		MonoBehaviourSingleton<PreloaderUI>.Instance.SetMenuChatBarVisibility(visible: false);
		MonoBehaviourSingleton<PreloaderUI>.Instance.StartBlackScreenShow(1f, 2f, delegate
		{
			MonoBehaviourSingleton<PreloaderUI>.Instance.FadeBlackScreen(0.5f, -2f);
		});
		_hideoutAreaQTEScreen.Show(this.m__E006, selectedArea, this);
		_hideoutAreaQTEScreen.SpecialAreaActionSelected(selectedArea, wait: false).HandleExceptions();
		_E006(selected: true);
	}

	public void ReturnToHideoutMode()
	{
		this.m__E001 = false;
		_hideoutScreenOverlay.Show(this.m__E006.HideoutPlayer, returnToFirstPerson: true, this.m__E007, this.m__E005, this);
		_hideoutScreenOverlay.ReturnToPreviousState();
		_hideoutScreenOverlay.AreaSelected(null, wait: false).HandleExceptions();
	}

	private void _E004(bool firstPerson, bool returnToFirstPerson, AreaData selectedArea = null)
	{
		SetPointOfView(firstPerson);
		ScreenController.UpdateEnvironmentSettings();
		if (!firstPerson)
		{
			_hideoutScreenOverlay.Show(this.m__E006.HideoutPlayer, returnToFirstPerson, this.m__E007, this.m__E005, this);
			_hideoutScreenOverlay.AreaSelected(selectedArea, wait: false).HandleExceptions();
		}
	}

	private void _E005(bool _)
	{
		this.m__E005 = Singleton<_E815>.Instance.AreaDatas.Where((AreaData x) => x.Enabled && x.Requirements.All((Requirement r) => r.Fulfilled)).ToArray();
	}

	public void SetPointOfView(bool firstPerson)
	{
		FirstPerson = firstPerson;
		if (!FirstPerson && this.m__E006.FirstPersonMode)
		{
			HideoutCameraController.ResetPlayerPosition(this.m__E006.Player);
		}
		this.m__E006.SetPointOfView(firstPerson);
	}

	protected override ECursorResult ShouldLockCursor()
	{
		if (!FirstPerson)
		{
			return ECursorResult.Ignore;
		}
		return ECursorResult.LockCursor;
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		return ETranslateResult.Ignore;
	}

	protected override void TranslateAxes(ref float[] axes)
	{
		if (!FirstPerson)
		{
			axes = null;
		}
	}

	private void _E006(bool selected)
	{
		this.m__E001 = selected;
		_E007();
	}

	private void _E007()
	{
		bool pointOfView = this.m__E001 || this.m__E004;
		foreach (AreaWorldPanel item in this.m__E000)
		{
			item.SetPointOfView(pointOfView);
		}
		bool flag = !this.m__E001 && (this.m__E003 || !FirstPerson);
		_areaIconsContainerCanvasGroup.DOKill();
		_areaIconsContainerCanvasGroup.DOFade(flag ? 1f : 0f, 0.8f);
		_areaIconsContainerCanvasGroup.blocksRaycasts = flag;
	}

	private void _E008()
	{
		this.m__E003 = !this.m__E003;
		_E838.InfoIconsStatus = this.m__E003;
		_E007();
	}

	private void Update()
	{
		Camera camera = _E8A8.Instance.Camera;
		if (camera == null)
		{
			return;
		}
		Vector2 vector = new Vector2(Screen.width, Screen.height);
		foreach (AreaWorldPanel item in this.m__E000)
		{
			if (item == null)
			{
				Debug.LogError(_ED3E._E000(164480));
				continue;
			}
			if (item.Data.AreaIconPoint == null)
			{
				Debug.LogError(_ED3E._E000(164573) + item.Data.Template.Name + _ED3E._E000(55115));
				continue;
			}
			Vector3 position = item.Data.AreaIconPoint.position;
			Vector3 vector2 = camera.WorldToViewportPoint(position);
			float num = (camera.transform.position - position).magnitude;
			if (vector2.z < 0f)
			{
				num *= -1f;
			}
			Vector3 position2 = new Vector3(vector2.x * vector.x, vector2.y * vector.y, num);
			item.SetPosition(position2);
		}
	}

	public override void Close()
	{
		this.m__E006.OnSelectArea -= _E001;
		this.m__E006.OnToggleInfoIcons -= _E008;
		this.m__E006.OnSpecialAreaActionSelection -= _E002;
		this.m__E006.OnExitQte -= ExitQte;
		HideoutCameraController.DestroyFakePlayerCollider();
		_hideoutScreenOverlay.Close();
		_hideoutAreaQTEScreen.Close();
		this.m__E000.Clear();
		base.Close();
	}

	[CompilerGenerated]
	private void _E009()
	{
		MonoBehaviourSingleton<PreloaderUI>.Instance.FadeBlackScreen(1f, -2f);
		_hideoutAreaQTEScreen.Close();
		ReturnToHideoutMode();
	}
}
