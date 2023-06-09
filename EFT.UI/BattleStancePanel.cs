using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

[RequireComponent(typeof(BattleUIComponentAnimation))]
public sealed class BattleStancePanel : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public BattleStancePanel _003C_003E4__this;

		public int current;

		public int to;

		public bool direction;

		internal void _E000()
		{
			_003C_003E4__this._E003(current, to, direction);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public float value;

		internal float _E000(float x)
		{
			return Math.Abs(value - x);
		}
	}

	[SerializeField]
	private List<BattleStance> _battleStances;

	[SerializeField]
	private BattleUINoiseLevel _noiseLevel;

	[SerializeField]
	private BattleStanceNotch _battleStanceNotchTemplate;

	[SerializeField]
	private BattleUiVoipPanel _voipPanel;

	[SerializeField]
	private BattleStanceNotch _finalNotch;

	[SerializeField]
	private RectTransform _notchesPlaceholder;

	[SerializeField]
	private Slider _stanceSlider;

	[SerializeField]
	private Slider _speedSlider;

	[SerializeField]
	private Image _sprintFillImage;

	[SerializeField]
	private Image _handsFillImage;

	[SerializeField]
	private Sprite _defaultFillSprite;

	[SerializeField]
	private Sprite _defaultFillSpriteForHands;

	[SerializeField]
	private Sprite _exhaustedFillSprite;

	private BattleUIComponentAnimation _E093;

	private Player _E094;

	private int _E095;

	private Coroutine _E096;

	private float _E097;

	private readonly Dictionary<BattleStanceNotch, float> _E098 = new Dictionary<BattleStanceNotch, float>();

	private void Awake()
	{
		_E002(1f, 1);
		_E005(1f);
		SetNoiseLevel(0);
		_E095 = 0;
		for (int i = 0; i < 20; i++)
		{
			BattleStanceNotch battleStanceNotch = UnityEngine.Object.Instantiate(_battleStanceNotchTemplate);
			battleStanceNotch.Init(_notchesPlaceholder);
			_E098.Add(battleStanceNotch, 0.05f * (float)i);
		}
		_E098.Add(_finalNotch, 1f);
	}

	public void SetNoiseLevel(int index)
	{
		_noiseLevel.SetNoiseLevel(index);
	}

	private void Update()
	{
		if ((object)_E094 != null)
		{
			_sprintFillImage.fillAmount = _E094.Physical.Stamina.NormalValue;
			_handsFillImage.fillAmount = _E094.Physical.HandsStamina.NormalValue;
			_sprintFillImage.sprite = ((_E094.Physical.Stamina.Current < 15f) ? _exhaustedFillSprite : _defaultFillSprite);
			_handsFillImage.sprite = ((_E094.Physical.HandsStamina.Current < 15f) ? _exhaustedFillSprite : _defaultFillSpriteForHands);
		}
	}

	public void Show(Player player)
	{
		UI.Dispose();
		ShowGameObject();
		if (_E093 == null)
		{
			_E093 = base.gameObject.GetComponent<BattleUIComponentAnimation>();
		}
		_E094 = player;
		_voipPanel.Show(_E094.VoipController);
		_E000();
		_E002(_E094.IsInPronePose ? (-0.35f) : _E094.PoseLevel, _E094.MovementContext.CovertNoiseLevel);
		_E094.MovementContext.OnSmoothedPoseLevelChange += _E002;
		_E094.OnSpeedChangedEvent += _E005;
		_E094.MovementContext.OnMaxSpeedChangedEvent += _E006;
		_E094.Physical.OnSprintStateChangedEvent += _E001;
		_E006(player.MovementContext.StateSpeedLimit, player.MovementContext.ClampedSpeed, player.MovementContext.MaxSpeed);
		SetNoiseLevel(_E094.MovementContext.CovertNoiseLevel);
		_E093.Close();
	}

	public void AnimatedShow(bool autohide)
	{
		if (_E093 != null)
		{
			_E093.Show(autohide).HandleExceptions();
		}
	}

	public void AnimatedHide(float delaySeconds = 0f)
	{
		if (_E093 != null)
		{
			_E093.Hide(delaySeconds).HandleExceptions();
		}
	}

	private void _E000()
	{
		if (!(_E094 == null))
		{
			_E094.MovementContext.OnSmoothedPoseLevelChange -= _E002;
			_E094.OnSpeedChangedEvent -= _E005;
			_E094.MovementContext.OnMaxSpeedChangedEvent -= _E006;
			_E094.Physical.OnSprintStateChangedEvent -= _E001;
		}
	}

	private void _E001(bool b)
	{
		SetNoiseLevel(_E094.MovementContext.CovertNoiseLevel);
	}

	private void _E002(float type, int noise)
	{
		if (Math.Abs(_E097 - type) < Mathf.Epsilon)
		{
			return;
		}
		SetNoiseLevel(noise);
		_E097 = type;
		foreach (BattleStance battleStance in _battleStances)
		{
			battleStance.StanceObject.SetActive(value: false);
		}
		float num = _E007(_battleStances.Select((BattleStance x) => x.PoseFactor), type);
		for (int i = 0; i < _battleStances.Count; i++)
		{
			if (Math.Abs(_battleStances[i].PoseFactor - num).IsZero())
			{
				_E004();
				_E003(_E095, i, _E095 < i);
				_stanceSlider.value = num;
				return;
			}
		}
		Debug.LogError(string.Format(_ED3E._E000(250384), type));
	}

	private void _E003(int current, int to, bool direction)
	{
		if (current != to)
		{
			_battleStances.ElementAt(current).StanceObject.SetActive(value: false);
			current += (direction ? 1 : (-1));
			_battleStances.ElementAt(current).StanceObject.SetActive(value: true);
			if (base.gameObject.activeSelf)
			{
				_E096 = this.WaitFrames(2, delegate
				{
					_E003(current, to, direction);
				});
			}
			return;
		}
		foreach (BattleStance battleStance in _battleStances)
		{
			battleStance.StanceObject.SetActive(value: false);
		}
		_E004();
		_battleStances.ElementAt(current).StanceObject.SetActive(value: true);
		_E095 = to;
	}

	private void _E004()
	{
		if (_E096 != null)
		{
			StopCoroutine(_E096);
		}
	}

	private void _E005(float inputSpeed, float maxCharacterSpeed = 1f, int noise = 1)
	{
		_speedSlider.maxValue = maxCharacterSpeed;
		_speedSlider.value = inputSpeed;
		SetNoiseLevel(noise);
	}

	private void _E006(float stateSpeedLimit, float clampedSpeed, float maxSpeed = 1f, int noise = 1)
	{
		foreach (KeyValuePair<BattleStanceNotch, float> item in _E098)
		{
			item.Key.Show(Math.Round(item.Value - stateSpeedLimit / maxSpeed, 2) <= (double)Mathf.Epsilon);
		}
		_E005(clampedSpeed, maxSpeed, noise);
	}

	private static float _E007(IEnumerable<float> values, float value)
	{
		return values.OrderBy((float x) => Math.Abs(value - x)).FirstOrDefault();
	}

	public override void Close()
	{
		if (_E093 != null)
		{
			_E093.StopAnimation();
		}
		_voipPanel.Close();
		_E004();
		_E000();
		base.Close();
	}
}
