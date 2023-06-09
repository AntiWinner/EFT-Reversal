using System;
using DG.Tweening;
using EFT.HealthSystem;
using EFT.UI.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Health;

public sealed class BodyPartView : UIElement
{
	private const float _E2F0 = 1f;

	[SerializeField]
	private CustomTextMeshProUGUI _valueText;

	[SerializeField]
	private Image _healthBar;

	[SerializeField]
	private Image _healthBarBorder;

	[SerializeField]
	private Image _image;

	[SerializeField]
	private Image _brokenCross;

	[SerializeField]
	private EffectsPanel _effectsPanel;

	[SerializeField]
	private DamagePanel _damagePanel;

	[SerializeField]
	private HealthBarButton _button;

	private Color _E2F1;

	private _E9C4 _E0AE;

	private EBodyPart _E0AF;

	private float _E2F2 = float.MinValue;

	private bool _E2F3 = true;

	private void Awake()
	{
		_E2F1 = _healthBarBorder.color;
	}

	public void Show(_E9C4 healthController, EBodyPart bodyPart, SimpleTooltip tooltip, DamageHistory damageHistory = null)
	{
		_E2F3 = true;
		ShowGameObject();
		_E0AE = healthController;
		_E0AF = bodyPart;
		_effectsPanel.Show(healthController, bodyPart, tooltip);
		UI.AddDisposable(_effectsPanel);
		_button.Show(healthController, bodyPart);
		UI.AddDisposable(_button);
		if (damageHistory != null)
		{
			_damagePanel.Show(healthController, bodyPart, damageHistory);
		}
		Update();
		_E2F3 = false;
	}

	private void Update()
	{
		ValueStruct bodyPartHealth = _E0AE.GetBodyPartHealth(_E0AF);
		float maximum = bodyPartHealth.Maximum;
		float current = bodyPartHealth.Current;
		float num = ((maximum / current < 2f) ? _E3A9.Floor(current) : _E3A9.Ceil(current));
		if (!(Math.Abs(num - _E2F2) < Mathf.Epsilon))
		{
			bool flag = _E0AE.IsBodyPartDestroyed(_E0AF);
			_E2F2 = num;
			float normalized = bodyPartHealth.Normalized;
			_valueText.text = string.Format(_ED3E._E000(182604), num, maximum);
			_valueText.color = ((normalized > 0.15f) ? Color.white : Color.red);
			_image.color = (flag ? new Color(0f, 0f, 0f, 1f) : ((normalized > 0.8f) ? new Color(1f, 0f, 0f, 0f) : new Color(1f, 0f, 0f, 1f - normalized / 0.8f)));
			if (!_E2F3 && !_E7A3.InRaid)
			{
				_healthBar.DOKill();
				_healthBar.DOFillAmount(bodyPartHealth.Normalized, 1f).SetEase(Ease.OutCubic);
			}
			else
			{
				_healthBar.fillAmount = bodyPartHealth.Normalized;
			}
			_healthBar.color = EHealthColorScheme.GreenToRed.GetBodyPartColor(normalized / 0.8f, flag);
			_healthBarBorder.color = (flag ? Color.red : _E2F1);
			_brokenCross.gameObject.SetActive(flag);
		}
	}
}
