using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace EFT;

public sealed class BonusController
{
	private sealed class _E000
	{
		private List<_E5EA> m__E000 = new List<_E5EA>();

		[CompilerGenerated]
		private _E5EA m__E001;

		public _E5EA Result
		{
			[CompilerGenerated]
			get
			{
				return this.m__E001;
			}
			[CompilerGenerated]
			private set
			{
				this.m__E001 = value;
			}
		}

		public _E000(_E5EA bonus)
		{
			Add(bonus);
		}

		public double CalculateValue(double input)
		{
			if (Result == null)
			{
				return input;
			}
			return Result.CalculateValue(input);
		}

		public void Add(_E5EA bonus)
		{
			if (bonus != null && !this.m__E000.Contains(bonus))
			{
				this.m__E000.Add(bonus);
				_E000();
				_E001();
			}
		}

		public void Remove(_E5EA bonus)
		{
			if (bonus != null && this.m__E000.Contains(bonus))
			{
				this.m__E000.Remove(bonus);
				_E001();
			}
		}

		public bool HasBonus()
		{
			return this.m__E000.Count > 0;
		}

		public void SetBoost(double boostValue)
		{
			if (!(Result is _E5EC obj))
			{
				return;
			}
			obj.BoostValue = boostValue;
			foreach (_E5EC item in this.m__E000.OfType<_E5EC>())
			{
				item.BoostValue = boostValue;
			}
		}

		private void _E000()
		{
			if (Result is _E5EC obj)
			{
				SetBoost(obj.BoostValue);
			}
		}

		private void _E001()
		{
			Result = null;
			foreach (_E5EA item in this.m__E000)
			{
				if (Result == null)
				{
					Result = item.Clone();
				}
				else
				{
					Result.Add(item);
				}
			}
		}
	}

	private readonly Dictionary<Enum, _E000> _activeBonuses = new Dictionary<Enum, _E000>();

	private _E74F _skillManager;

	private float _E000 => _skillManager?.ZoneBonusBoost.Value ?? 0f;

	public event Action<bool, EBonusType> OnBonusChanged;

	public void InitSkillManager(_E74F skillManager)
	{
		if (_skillManager != null)
		{
			_skillManager.OnSkillLevelChanged -= _E000;
		}
		_skillManager = skillManager;
		_skillManager.OnSkillLevelChanged += _E000;
		_E001();
	}

	private void _E000(_E74E baseSkill)
	{
		if (baseSkill.Id == ESkillId.HideoutManagement)
		{
			_E001();
		}
	}

	private void _E001()
	{
		foreach (KeyValuePair<Enum, _E000> activeBonuse in _activeBonuses)
		{
			_E39D.Deconstruct(activeBonuse, out var _, out var value);
			value.SetBoost(this._E000);
		}
	}

	private void _E002(_E5EA bonus)
	{
		if (bonus is _E5EC obj)
		{
			obj.BoostValue = this._E000;
		}
	}

	public void AddBonus(_E5EA bonus, bool silent = false)
	{
		if (bonus != null)
		{
			_E002(bonus);
			if (!_activeBonuses.TryGetValue(bonus.TargetType, out var value))
			{
				_activeBonuses.Add(bonus.TargetType, new _E000(bonus));
			}
			else
			{
				value.Add(bonus);
			}
			if (!silent)
			{
				this.OnBonusChanged?.Invoke(arg1: true, bonus.BonusType);
			}
		}
	}

	public void RemoveBonus(_E5EA bonus, bool silent = false)
	{
		if (bonus != null && _activeBonuses.TryGetValue(bonus.TargetType, out var value))
		{
			value.Remove(bonus);
			if (value.Result == null)
			{
				_activeBonuses.Remove(bonus.TargetType);
			}
			if (!silent)
			{
				this.OnBonusChanged?.Invoke(arg1: false, bonus.BonusType);
			}
		}
	}

	public bool HasBonus(EBonusType type)
	{
		if (_activeBonuses.TryGetValue(type, out var value))
		{
			return value.HasBonus();
		}
		return false;
	}

	public double Calculate(_E751 skill, double input)
	{
		input = _E003(skill.Class, input);
		return _E003(skill.Id, input);
	}

	public double Calculate(EBonusType type, double input)
	{
		return _E003(type, input);
	}

	private double _E003(Enum type, double input)
	{
		if (!_activeBonuses.TryGetValue(type, out var value))
		{
			return input;
		}
		return value.CalculateValue(input);
	}

	public void Reset()
	{
		List<EBonusType> list = new List<EBonusType>();
		foreach (var (_, obj2) in _activeBonuses)
		{
			if (obj2.Result != null)
			{
				EBonusType bonusType = obj2.Result.BonusType;
				if (!list.Contains(bonusType))
				{
					list.Add(bonusType);
				}
			}
		}
		_activeBonuses.Clear();
		foreach (EBonusType item in list)
		{
			this.OnBonusChanged?.Invoke(arg1: false, item);
		}
	}
}
