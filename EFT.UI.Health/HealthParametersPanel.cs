using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.HealthSystem;
using UnityEngine;

namespace EFT.UI.Health;

public class HealthParametersPanel : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E74F skills;

		public HealthParametersPanel _003C_003E4__this;

		internal void _E000()
		{
			float num = (skills.StrengthBuffElite ? _003C_003E4__this._E156.TotalWeightEliteSkill : _003C_003E4__this._E156.TotalWeight);
			if (_003C_003E4__this._weight != null)
			{
				float num2 = skills.CarryingWeightRelativeModifier * _003C_003E4__this._E190.CarryingWeightRelativeModifier;
				float carryingWeightAbsoluteModifier = _003C_003E4__this._E190.CarryingWeightAbsoluteModifier;
				float num3 = Singleton<_E5CB>.Instance.Stamina.LowerOverweightLimit * num2 + carryingWeightAbsoluteModifier;
				float num4 = Singleton<_E5CB>.Instance.Stamina.UpperOverweightLimit * num2 + carryingWeightAbsoluteModifier;
				_003C_003E4__this._weight.SetParameterValue(new ValueStruct
				{
					Current = num,
					Maximum = num4
				}, 0f, 1);
				_003C_003E4__this._weight.SetWarningColor(num > num3, num < num4);
			}
		}

		internal void _E001(_E992 effect)
		{
			if (effect is _E9BE)
			{
				_E000();
			}
		}

		internal void _E002()
		{
			_003C_003E4__this._E190.EffectStartedEvent -= delegate(_E992 effect)
			{
				if (effect is _E9BE)
				{
					_E000();
				}
			};
		}

		internal void _E003()
		{
			_003C_003E4__this._E190.EffectResidualEvent -= delegate(_E992 effect)
			{
				if (effect is _E9BE)
				{
					_E000();
				}
			};
		}
	}

	[SerializeField]
	private BuffableHealthParameterPanel _overallHealth;

	[SerializeField]
	private HealthParameterPanel _poisoning;

	[SerializeField]
	private HealthParameterPanel _radiation;

	[SerializeField]
	private HealthParameterPanel _bloodPressure;

	[SerializeField]
	private BuffableHealthParameterPanel _energy;

	[SerializeField]
	private BuffableHealthParameterPanel _hydration;

	[SerializeField]
	private BuffableHealthParameterPanel _temperature;

	[SerializeField]
	private HealthParameterPanel _weight;

	private _E9C4 _E190;

	private _EAE7 _E156;

	private _E74F _E135;

	public void Show(_E9C4 healthController, _EAE7 inventory, _E74F skills)
	{
		_E000 CS_0024_003C_003E8__locals0 = new _E000();
		CS_0024_003C_003E8__locals0.skills = skills;
		CS_0024_003C_003E8__locals0._003C_003E4__this = this;
		ShowGameObject();
		_E190 = healthController;
		_E156 = inventory;
		_E135 = CS_0024_003C_003E8__locals0.skills;
		UI.BindEvent(_E156.OnWeightUpdated, delegate
		{
			float num5 = (CS_0024_003C_003E8__locals0.skills.StrengthBuffElite ? CS_0024_003C_003E8__locals0._003C_003E4__this._E156.TotalWeightEliteSkill : CS_0024_003C_003E8__locals0._003C_003E4__this._E156.TotalWeight);
			if (CS_0024_003C_003E8__locals0._003C_003E4__this._weight != null)
			{
				float num6 = CS_0024_003C_003E8__locals0.skills.CarryingWeightRelativeModifier * CS_0024_003C_003E8__locals0._003C_003E4__this._E190.CarryingWeightRelativeModifier;
				float carryingWeightAbsoluteModifier2 = CS_0024_003C_003E8__locals0._003C_003E4__this._E190.CarryingWeightAbsoluteModifier;
				float num7 = Singleton<_E5CB>.Instance.Stamina.LowerOverweightLimit * num6 + carryingWeightAbsoluteModifier2;
				float num8 = Singleton<_E5CB>.Instance.Stamina.UpperOverweightLimit * num6 + carryingWeightAbsoluteModifier2;
				CS_0024_003C_003E8__locals0._003C_003E4__this._weight.SetParameterValue(new ValueStruct
				{
					Current = num5,
					Maximum = num8
				}, 0f, 1);
				CS_0024_003C_003E8__locals0._003C_003E4__this._weight.SetWarningColor(num5 > num7, num5 < num8);
			}
		});
		if (_E135?.Strength != null)
		{
			UI.BindEvent(_E135.Strength.OnLevelUp, delegate
			{
				float num = (CS_0024_003C_003E8__locals0.skills.StrengthBuffElite ? CS_0024_003C_003E8__locals0._003C_003E4__this._E156.TotalWeightEliteSkill : CS_0024_003C_003E8__locals0._003C_003E4__this._E156.TotalWeight);
				if (CS_0024_003C_003E8__locals0._003C_003E4__this._weight != null)
				{
					float num2 = CS_0024_003C_003E8__locals0.skills.CarryingWeightRelativeModifier * CS_0024_003C_003E8__locals0._003C_003E4__this._E190.CarryingWeightRelativeModifier;
					float carryingWeightAbsoluteModifier = CS_0024_003C_003E8__locals0._003C_003E4__this._E190.CarryingWeightAbsoluteModifier;
					float num3 = Singleton<_E5CB>.Instance.Stamina.LowerOverweightLimit * num2 + carryingWeightAbsoluteModifier;
					float num4 = Singleton<_E5CB>.Instance.Stamina.UpperOverweightLimit * num2 + carryingWeightAbsoluteModifier;
					CS_0024_003C_003E8__locals0._003C_003E4__this._weight.SetParameterValue(new ValueStruct
					{
						Current = num,
						Maximum = num4
					}, 0f, 1);
					CS_0024_003C_003E8__locals0._003C_003E4__this._weight.SetWarningColor(num > num3, num < num4);
				}
			});
		}
		_E190.EffectStartedEvent += delegate(_E992 effect)
		{
			if (effect is _E9BE)
			{
				CS_0024_003C_003E8__locals0._E000();
			}
		};
		_E190.EffectResidualEvent += delegate(_E992 effect)
		{
			if (effect is _E9BE)
			{
				CS_0024_003C_003E8__locals0._E000();
			}
		};
		UI.AddDisposable(delegate
		{
			CS_0024_003C_003E8__locals0._003C_003E4__this._E190.EffectStartedEvent -= delegate(_E992 effect)
			{
				if (effect is _E9BE)
				{
					CS_0024_003C_003E8__locals0._E000();
				}
			};
		});
		UI.AddDisposable(delegate
		{
			CS_0024_003C_003E8__locals0._003C_003E4__this._E190.EffectResidualEvent -= delegate(_E992 effect)
			{
				if (effect is _E9BE)
				{
					CS_0024_003C_003E8__locals0._E000();
				}
			};
		});
		_E000();
	}

	public void ShowSecondaryParameters(bool value)
	{
		if (_temperature != null)
		{
			_temperature.gameObject.SetActive(value);
		}
		if (_poisoning != null)
		{
			_poisoning.gameObject.SetActive(value);
		}
		if (_radiation != null)
		{
			_radiation.gameObject.SetActive(value);
		}
		if (_bloodPressure != null)
		{
			_bloodPressure.gameObject.SetActive(value);
		}
		if (_energy != null)
		{
			_energy.gameObject.SetActive(value);
		}
		if (_hydration != null)
		{
			_hydration.gameObject.SetActive(value);
		}
		if (_weight != null)
		{
			_weight.gameObject.SetActive(value);
		}
	}

	private void _E000()
	{
		ValueStruct bodyPartHealth = _E190.GetBodyPartHealth(EBodyPart.Common, rounded: true);
		_overallHealth.SetParameterValue(bodyPartHealth, 10f, 0);
		_overallHealth.SetBuffValue(_E190.HealthRate, !bodyPartHealth.AtMaximum);
		if (_hydration != null)
		{
			_hydration.SetParameterValue(_E190.Hydration, 10f, 0);
			_hydration.SetBuffValue(_E190.HydrationRate, !_E190.Hydration.AtMaximum);
		}
		if (_energy != null)
		{
			_energy.SetParameterValue(_E190.Energy, 10f, 0);
			_energy.SetBuffValue(_E190.EnergyRate, !_E190.Energy.AtMaximum);
		}
		if (_temperature != null)
		{
			_temperature.SetParameterValue(_E190.Temperature, 10f, 1);
			_temperature.SetBuffValue(_E190.TemperatureRate, !_E190.Temperature.AtMaximum, positiveIncrease: false);
		}
		if (_poisoning != null)
		{
			_poisoning.SetParameterValue(_E190.Poison, 100f, 0, countFromTop: true);
		}
		if (_radiation != null)
		{
			_radiation.SetParameterValue(new ValueStruct
			{
				Current = 0f,
				Minimum = 0f,
				Maximum = 100f
			}, -1f, 0);
		}
	}

	private void Update()
	{
		if (_E190 != null)
		{
			_E000();
		}
	}
}
