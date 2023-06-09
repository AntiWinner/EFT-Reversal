using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DG.Tweening;
using EFT.InputSystem;
using EFT.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.Hideout;

public sealed class AreasPanel : UIInputNode, IPointerExitHandler, IEventSystemHandler, IPointerEnterHandler
{
	private sealed class _E000 : IComparer<AreaData>
	{
		private static readonly Dictionary<EAreaStatus, int> _E000 = new Dictionary<EAreaStatus, int>
		{
			{
				EAreaStatus.NotSet,
				0
			},
			{
				EAreaStatus.ReadyToConstruct,
				1
			},
			{
				EAreaStatus.ReadyToUpgrade,
				2
			},
			{
				EAreaStatus.ReadyToInstallConstruct,
				3
			},
			{
				EAreaStatus.ReadyToInstallUpgrade,
				4
			},
			{
				EAreaStatus.Constructing,
				5
			},
			{
				EAreaStatus.Upgrading,
				6
			},
			{
				EAreaStatus.LockedToConstruct,
				7
			},
			{
				EAreaStatus.LockedToUpgrade,
				8
			},
			{
				EAreaStatus.NoFutureUpgrades,
				9
			},
			{
				EAreaStatus.AutoUpgrading,
				10
			}
		};

		public static readonly _E000 Instance = new _E000();

		public int Compare(AreaData x, AreaData y)
		{
			if (x == null || y == null)
			{
				return -1;
			}
			int num = _E000[x.Status];
			int num2 = _E000[y.Status];
			if (num != num2)
			{
				return num - num2;
			}
			return string.CompareOrdinal(x.Template.Name, y.Template.Name);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public Action<AreaPanel> onSelected;

		public AreasPanel _003C_003E4__this;

		internal void _E000(AreaData item, AreaPanel view)
		{
			view.Show(item, onSelected);
			_003C_003E4__this.UI.AddDisposable(item.StatusUpdated.Bind(_003C_003E4__this._E005));
		}

		internal void _E001()
		{
			_003C_003E4__this._scrollView.onValueChanged.RemoveListener(_003C_003E4__this._E001);
		}

		internal void _E002()
		{
			_003C_003E4__this._scrollView.OnStartDrag.RemoveListener(_003C_003E4__this.OnStartDrag);
		}
	}

	private const float _E039 = 200f;

	private const float _E03A = 0.5f;

	private const float _E03B = 120f;

	private const float _E03C = 0.3f;

	[SerializeField]
	private AreaPanel _areaPanelTemplate;

	[SerializeField]
	private RectTransform _areaPanelsContainer;

	[SerializeField]
	private AreasScrollRect _scrollView;

	[SerializeField]
	private Button _leftButton;

	[SerializeField]
	private Button _rightButton;

	private AreaData[] _E03D;

	private _EC71<AreaData, AreaPanel> _E03E;

	private Action<AreaPanel> _E03F;

	private AreaPanel _E040;

	private bool _E041;

	private bool _E042;

	private bool _E043 = true;

	private Vector2 _E044;

	private void Awake()
	{
		_leftButton.onClick.AddListener(delegate
		{
			_E000(-1);
		});
		_rightButton.onClick.AddListener(delegate
		{
			_E000(1);
		});
		LayoutElement component = _leftButton.gameObject.GetComponent<LayoutElement>();
		_E044 = new Vector2(component.preferredWidth, component.preferredHeight);
	}

	public async void Show(AreaData[] areas, Action<AreaPanel> onSelected)
	{
		ShowGameObject();
		bool flag = _E03D == null;
		_E03F = onSelected;
		_E03D = areas;
		_E03E = UI.AddDisposable(new _EC71<AreaData, AreaPanel>(new _ECEF<AreaData>(areas), _areaPanelTemplate, _areaPanelsContainer, delegate(AreaData item, AreaPanel view)
		{
			view.Show(item, onSelected);
			UI.AddDisposable(item.StatusUpdated.Bind(_E005));
		}));
		_scrollView.onValueChanged.AddListener(_E001);
		_scrollView.OnStartDrag.AddListener(OnStartDrag);
		UI.AddDisposable(delegate
		{
			_scrollView.onValueChanged.RemoveListener(_E001);
		});
		UI.AddDisposable(delegate
		{
			_scrollView.OnStartDrag.RemoveListener(OnStartDrag);
		});
		_E005();
		while (_areaPanelsContainer.rect.width <= 1f)
		{
			await Task.Yield();
		}
		bool flag2 = false;
		foreach (AreaData areaData in areas)
		{
			if (areaData.Selected)
			{
				SelectArea(areaData);
				flag2 = true;
				break;
			}
		}
		SetSelectedStatus(flag2);
		if (!flag2 && flag)
		{
			_E001(Vector2.zero);
		}
	}

	private void _E000(int direction)
	{
		AreaPanel areaPanel = _E040;
		int num = 0;
		do
		{
			num++;
			if (num > _E03E.Count)
			{
				return;
			}
			int num2 = _E03E.IndexOf(areaPanel) + direction;
			if (num2 > _E03D.Length - 1)
			{
				num2 = 0;
			}
			else if (num2 < 0)
			{
				num2 = _E03D.Length - 1;
			}
			areaPanel = _E03E.ViewAt(num2);
		}
		while (!areaPanel.Data.DisplayInterface);
		if (!(_E040 == areaPanel))
		{
			_E03F(areaPanel);
		}
	}

	public void SelectArea(AreaData area)
	{
		if (Array.IndexOf(_E03D, area) >= 0)
		{
			_E040 = _E03E[area];
			_E002(_E040).HandleExceptions();
		}
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (_E041)
		{
			return InputNode.GetDefaultBlockResult(command);
		}
		return ETranslateResult.Ignore;
	}

	public void SetSelectedStatus(bool status)
	{
		if (!status)
		{
			_E040 = null;
		}
		_E043 = status;
		Vector2 endValue = _E044;
		if (!_E043)
		{
			endValue.x = 0f;
		}
		_leftButton.gameObject.GetComponent<LayoutElement>().DOPreferredSize(endValue, 0.5f);
		_rightButton.gameObject.GetComponent<LayoutElement>().DOPreferredSize(endValue, 0.5f);
	}

	private void _E001(Vector2 scrollPosition)
	{
		if (!_E042)
		{
			float x = scrollPosition.x;
			float normalizedPosition = _E003(x);
			if (!normalizedPosition.Equals(x))
			{
				_E042 = true;
				_E004(normalizedPosition);
			}
		}
	}

	public void OnStartDrag()
	{
		_scrollView.DOKill();
	}

	public void OnPointerEnter(PointerEventData data)
	{
		_E041 = true;
	}

	public void OnPointerExit(PointerEventData data)
	{
		_E041 = false;
	}

	protected override void TranslateAxes(ref float[] axes)
	{
	}

	protected override ECursorResult ShouldLockCursor()
	{
		return ECursorResult.Ignore;
	}

	private async Task _E002(AreaPanel panel)
	{
		await TasksExtensions.Delay(0.3f);
		if (!(panel == null))
		{
			float normalizedPosition = panel.transform.localPosition.x / _areaPanelsContainer.rect.width;
			_E004(normalizedPosition);
		}
	}

	private float _E003(float normalizedPosition)
	{
		float width = _areaPanelsContainer.rect.width;
		float num = (((RectTransform)base.transform).rect.width - 240f) / 2f / width;
		return Mathf.Clamp(normalizedPosition, num, 1f - num);
	}

	private void _E004(float normalizedPosition)
	{
		normalizedPosition = _E003(normalizedPosition);
		if (normalizedPosition > 1f || normalizedPosition < 0f)
		{
			_E042 = false;
			return;
		}
		float width = _areaPanelsContainer.rect.width;
		float duration = (float)Math.Sqrt(Math.Abs(normalizedPosition - _scrollView.horizontalNormalizedPosition) * width / 200f) * 0.5f;
		_scrollView.DOKill();
		_scrollView.ResetDrag();
		Tweener tweener = _scrollView.DOHorizontalNormalizedPos(normalizedPosition, duration);
		tweener.onComplete = delegate
		{
			_E042 = false;
		};
		tweener.onKill = delegate
		{
			_E042 = false;
		};
	}

	private void _E005()
	{
		_E03E?.OrderBy((AreaData x) => x, AreasPanel._E000.Instance);
		if (_E040 != null)
		{
			_E002(_E040).HandleExceptions();
		}
	}

	[CompilerGenerated]
	private void _E006()
	{
		_E000(-1);
	}

	[CompilerGenerated]
	private void _E007()
	{
		_E000(1);
	}

	[CompilerGenerated]
	private void _E008()
	{
		_E042 = false;
	}

	[CompilerGenerated]
	private void _E009()
	{
		_E042 = false;
	}
}
