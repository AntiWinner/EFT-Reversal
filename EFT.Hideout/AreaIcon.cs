using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Comfort.Common;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using EFT.InventoryLogic;
using EFT.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.Hideout;

public class AreaIcon : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public GeneratorBehaviour generatorBehaviour;

		public AreaIcon _003C_003E4__this;

		public _E823 producer;

		internal void _E000()
		{
			generatorBehaviour.OnWorkingStateChanged -= _003C_003E4__this._E000;
		}

		internal void _E001()
		{
			producer.OnProductionSpeedChanged -= _003C_003E4__this._E001;
		}

		internal void _E002()
		{
			producer.OnProductionStarted -= _003C_003E4__this._E002;
		}

		internal void _E003()
		{
			producer.OnProductionComplete -= _003C_003E4__this._E003;
		}

		internal void _E004()
		{
			producer.OnGetProducedItems -= _003C_003E4__this._E006;
		}

		internal void _E005()
		{
			producer.OnWorkingStatusChanged -= _003C_003E4__this._E005;
		}
	}

	private const float _E02B = 0.3f;

	private const float _E02C = 1f;

	private const float _E02D = 0.65f;

	[SerializeField]
	private TextMeshProUGUI _currentAreaLevel;

	[SerializeField]
	private TextMeshProUGUI _nextAreaLevel;

	[SerializeField]
	private CanvasGroup _iconCanvasGroup;

	[SerializeField]
	private Image _selectedBorder;

	[SerializeField]
	private Image _hoverBorder;

	[SerializeField]
	private Image _areaIconImage;

	[SerializeField]
	private Image _backgroundImage;

	[SerializeField]
	private Image _constructingImage;

	[SerializeField]
	private Image _producingImage;

	[SerializeField]
	private GameObject _lockedIcon;

	[SerializeField]
	private GameObject _unlockedIcon;

	[SerializeField]
	private GameObject _readyToUpgradeIcon;

	[SerializeField]
	private GameObject _constructingIcon;

	[SerializeField]
	private GameObject _upgradingIcon;

	[SerializeField]
	private GameObject _producingIcon;

	[SerializeField]
	private GameObject _outOfFuelIcon;

	[SerializeField]
	private Image _eliteBackgroundImage;

	[SerializeField]
	private Sprite _defaultSprite;

	[SerializeField]
	private Sprite _readyToPerformSprite;

	[SerializeField]
	private Sprite _errorSprite;

	protected TweenerCore<Color, Color, ColorOptions> BackImageFade;

	private List<Tweener> _E02E = new List<Tweener>();

	private TweenerCore<Color, Color, ColorOptions> _E02F;

	private TweenerCore<float, float, FloatOptions> _E030;

	private TweenerCore<Color, Color, ColorOptions> _E031;

	[CompilerGenerated]
	private AreaData _E032;

	protected Image BackgroundImage => _backgroundImage;

	protected Sprite DefaultSprite => _defaultSprite;

	protected Sprite ErrorSprite => _errorSprite;

	protected GameObject LockedIcon => _lockedIcon;

	protected GameObject UnlockedIcon => _unlockedIcon;

	protected GameObject ReadyToUpgradeIcon => _readyToUpgradeIcon;

	protected GameObject ConstructingIcon => _constructingIcon;

	protected GameObject UpgradingIcon => _upgradingIcon;

	private AreaData _E000
	{
		[CompilerGenerated]
		get
		{
			return _E032;
		}
		[CompilerGenerated]
		set
		{
			_E032 = value;
		}
	}

	public void Show(AreaData data)
	{
		UI.Dispose();
		_E02E = new List<Tweener>
		{
			(UpgradingIcon.transform as RectTransform).DOAnchorPosY(-10f, 0.5f).SetLoops(-1, LoopType.Yoyo),
			_producingIcon.transform.DORotate(new Vector3(0f, 0f, -10f), 0.05f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental),
			_nextAreaLevel.DOFade(0.65f, 0.3f).SetEase(Ease.Flash).SetLoops(-1, LoopType.Yoyo)
		};
		SetData(data);
		if (data.DisplayOutOfFuelIcon)
		{
			UI.AddDisposable(this._E000.LightStatusChanged.Bind(_E007, this._E000.LightStatus));
		}
		_E831 areaBehaviour = this._E000.Template.AreaBehaviour;
		GeneratorBehaviour generatorBehaviour;
		if ((generatorBehaviour = areaBehaviour as GeneratorBehaviour) != null)
		{
			generatorBehaviour.OnWorkingStateChanged += _E000;
			UI.AddDisposable(delegate
			{
				generatorBehaviour.OnWorkingStateChanged -= _E000;
			});
		}
		_E823 producer = Singleton<_E815>.Instance.ProductionController.GetProducer(this._E000);
		if (producer == null)
		{
			return;
		}
		producer.OnProductionSpeedChanged += _E001;
		producer.OnProductionStarted += _E002;
		producer.OnProductionComplete += _E003;
		producer.OnGetProducedItems += _E006;
		producer.OnWorkingStatusChanged += _E005;
		if (producer.CompleteItemsStorage.AnyItemsReady)
		{
			_E004();
		}
		else
		{
			_E827 closestToEndProducingItem = producer.ClosestToEndProducingItem;
			if (closestToEndProducingItem != null)
			{
				_E002(closestToEndProducingItem);
			}
		}
		_producingIcon.SetActive(producer.IsWorking);
		UI.AddDisposable(delegate
		{
			producer.OnProductionSpeedChanged -= _E001;
		});
		UI.AddDisposable(delegate
		{
			producer.OnProductionStarted -= _E002;
		});
		UI.AddDisposable(delegate
		{
			producer.OnProductionComplete -= _E003;
		});
		UI.AddDisposable(delegate
		{
			producer.OnGetProducedItems -= _E006;
		});
		UI.AddDisposable(delegate
		{
			producer.OnWorkingStatusChanged -= _E005;
		});
	}

	private void _E000(_E810 generator, bool value)
	{
		_E00A();
	}

	protected void SetData(AreaData data)
	{
		this._E000 = data;
		UI.AddDisposable(this._E000.OnHover.Subscribe(_E008));
		UI.AddDisposable(this._E000.OnSelected.Subscribe(_E009));
		UI.AddDisposable(this._E000.LevelUpdated.Bind(SetLevel));
		UI.AddDisposable(this._E000.StatusUpdated.Bind(SetStatus));
		UI.AddDisposable(GeneratorBehaviour.OnGeneratorInstalledHandler.Bind(_E007, this._E000.LightStatus));
		_outOfFuelIcon.SetActive(value: false);
		_eliteBackgroundImage.gameObject.SetActive(data.Template.IsElite);
		ShowGameObject();
	}

	private void _E001()
	{
		_E827 obj = Singleton<_E815>.Instance.ProductionController.GetProducer(this._E000)?.ClosestToEndProducingItem;
		if (obj != null)
		{
			_E002(obj);
		}
	}

	private void _E002(_E827 item)
	{
		_E030?.Kill();
		_E823 producer = Singleton<_E815>.Instance.ProductionController.GetProducer(this._E000);
		_producingIcon.SetActive(producer.IsWorking);
		if (producer.CompleteItemsStorage.AnyItemsReady)
		{
			_producingImage.fillAmount = 1f;
			return;
		}
		_producingImage.fillAmount = (float)item.Progress;
		if (producer.ProductionSpeedCoefficient.Positive())
		{
			_E030 = _producingImage.DOFillAmount(1f, item.EstimatedTimeLeft).SetUpdate(isIndependentUpdate: true).SetUpdate(UpdateType.Late)
				.SetEase(Ease.Linear);
		}
	}

	private void _E003(Item[] items, _E827 producingItem)
	{
		_E004();
	}

	private void _E004()
	{
		_E823 producer = Singleton<_E815>.Instance.ProductionController.GetProducer(this._E000);
		_producingImage.fillAmount = 1f;
		_E030?.Kill(complete: true);
		_producingIcon.SetActive(producer.IsWorking);
		if (producer.IsFulfilled)
		{
			_E02F = _producingImage.DOFade(0.65f, 1f).SetEase(Ease.Flash).SetLoops(-1, LoopType.Yoyo);
		}
	}

	private void _E005()
	{
		_E823 producer = Singleton<_E815>.Instance.ProductionController.GetProducer(this._E000);
		if (producer == null || !producer.IsWorking)
		{
			_E030?.Kill();
			if (_producingIcon != null)
			{
				_producingIcon.SetActive(value: false);
			}
		}
		else
		{
			_E002(producer.ClosestToEndProducingItem);
		}
	}

	private void _E006(IEnumerable<Item> obj)
	{
		_E823 producer = Singleton<_E815>.Instance.ProductionController.GetProducer(this._E000);
		if (!producer.IsWorking)
		{
			_E030?.Kill(complete: true);
			_producingIcon.SetActive(value: false);
		}
		_E827 closestToEndProducingItem = producer.ClosestToEndProducingItem;
		if (closestToEndProducingItem != null)
		{
			_E002(closestToEndProducingItem);
		}
		else
		{
			_producingImage.fillAmount = 0f;
		}
		_E02F?.Kill(complete: true);
	}

	private void _E007(ELightStatus lightStatus)
	{
		if (!(_outOfFuelIcon == null))
		{
			_outOfFuelIcon.SetActive(lightStatus == ELightStatus.OutOfFuel && this._E000.IsInstalled && GeneratorBehaviour.IsGeneratorInstalled);
		}
	}

	private void _E008(bool value)
	{
		_hoverBorder.gameObject.SetActive(value);
	}

	private void _E009(bool value)
	{
		_selectedBorder.gameObject.SetActive(value);
	}

	protected virtual void SetLevel()
	{
		SetLevel(this._E000.CurrentLevel);
		if (this._E000.Template.MaxLevel <= 1)
		{
			_currentAreaLevel.gameObject.SetActive(value: false);
		}
	}

	protected void SetLevel(int level)
	{
		if (!(this == null))
		{
			_currentAreaLevel.text = level.LevelFormat();
			_currentAreaLevel.gameObject.SetActive(level > 0 && this._E000.DisplayLevel);
			_nextAreaLevel.text = (level + 1).LevelFormat();
			_areaIconImage.sprite = this._E000.Template.AreaIcon;
			_areaIconImage.SetNativeSize();
		}
	}

	protected virtual void SetStatus()
	{
		if (this == null)
		{
			return;
		}
		EAreaStatus status = this._E000.Status;
		_E00A();
		LockedIcon.SetActive(status == EAreaStatus.LockedToConstruct);
		UnlockedIcon.SetActive(status == EAreaStatus.ReadyToInstallConstruct || status == EAreaStatus.ReadyToConstruct);
		ReadyToUpgradeIcon.SetActive(status == EAreaStatus.ReadyToUpgrade);
		ConstructingIcon.SetActive(status == EAreaStatus.Constructing);
		UpgradingIcon.SetActive(status == EAreaStatus.Upgrading);
		_nextAreaLevel.gameObject.SetActive(status == EAreaStatus.ReadyToInstallUpgrade && this._E000.DisplayLevel);
		_iconCanvasGroup.alpha = ((status == EAreaStatus.LockedToConstruct || status == EAreaStatus.ReadyToConstruct) ? 0.3f : 1f);
		Color color = _eliteBackgroundImage.color;
		color.a = ((status == EAreaStatus.LockedToConstruct || status == EAreaStatus.ReadyToConstruct) ? 0.3f : 1f);
		_eliteBackgroundImage.color = color;
		switch (status)
		{
		case EAreaStatus.Constructing:
		case EAreaStatus.Upgrading:
		{
			float data = this._E000.NextStage.ConstructionTime.Data;
			float num = (float)(_E5AD.UtcNow - this._E000.CurrentStage.StartTime).TotalSeconds;
			float duration = data - num;
			_constructingImage.fillAmount = num / data;
			_constructingImage.DOFillAmount(1f, duration).SetUpdate(isIndependentUpdate: true).SetUpdate(UpdateType.Late)
				.SetEase(Ease.Linear);
			_constructingImage.material.mainTextureOffset = new Vector2(-0.15f, 0f);
			_E031?.Kill();
			break;
		}
		case EAreaStatus.ReadyToInstallConstruct:
		case EAreaStatus.ReadyToInstallUpgrade:
			_constructingImage.fillAmount = 1f;
			_constructingImage.material.mainTextureOffset = Vector2.zero;
			_E031 = _constructingImage.DOFade(0.5f, 0.3f).SetEase(Ease.Flash).SetLoops(-1, LoopType.Yoyo)
				.OnComplete(delegate
				{
					_constructingImage.color = Color.white;
				});
			break;
		case EAreaStatus.NotSet:
		case EAreaStatus.LockedToConstruct:
		case EAreaStatus.ReadyToConstruct:
		case EAreaStatus.LockedToUpgrade:
		case EAreaStatus.ReadyToUpgrade:
		case EAreaStatus.NoFutureUpgrades:
		case EAreaStatus.AutoUpgrading:
			_constructingImage.fillAmount = 0f;
			_E031?.Kill();
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	private void _E00A()
	{
		BackImageFade?.Pause();
		EAreaStatus status = this._E000.Status;
		bool flag = status == EAreaStatus.ReadyToConstruct || status == EAreaStatus.ReadyToUpgrade;
		BackgroundImage.sprite = ((status == EAreaStatus.LockedToConstruct || this._E000.Template.AreaBehaviour.HasError) ? ErrorSprite : (flag ? _readyToPerformSprite : DefaultSprite));
		if (flag)
		{
			BackImageFade = BackgroundImage.DOFade(0.65f, 0.3f).SetEase(Ease.Flash).SetLoops(-1, LoopType.Yoyo)
				.OnComplete(delegate
				{
					BackgroundImage.color = Color.white;
				});
		}
	}

	public override void Close()
	{
		foreach (Tweener item in _E02E)
		{
			item.Kill();
		}
		base.Close();
	}

	[CompilerGenerated]
	private void _E00B()
	{
		_constructingImage.color = Color.white;
	}

	[CompilerGenerated]
	private void _E00C()
	{
		BackgroundImage.color = Color.white;
	}
}
