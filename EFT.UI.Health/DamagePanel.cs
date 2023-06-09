using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.UI.Health;

public class DamagePanel : UIElement
{
	private sealed class _E000 : List<DamageStats>
	{
		public readonly DamageStats.EDamageResult ResultType;

		public _E000(DamageStats.EDamageResult resultType)
		{
			ResultType = resultType;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public _E9C4 healthController;

		public DamagePanel _003C_003E4__this;

		internal void _E000()
		{
			healthController.HealthChangedEvent -= _003C_003E4__this._E000;
		}
	}

	[SerializeField]
	private DamageIcon _damageIconTemplate;

	private _ECEF<_E000> _E2F4;

	private _E9C4 _E0AE;

	private EBodyPart _E0AF;

	private bool _E000 => _E0AE?.GetBodyPartHealth(_E0AF).AtMaximum ?? false;

	public void Show(_E9C4 healthController, EBodyPart bodyPart, DamageHistory damage)
	{
		_E002();
		_E0AF = bodyPart;
		_E0AE = healthController;
		if (!damage.BodyParts.TryGetValue(_E0AF, out var value) || value.Count == 0 || this._E000)
		{
			Close();
			return;
		}
		ShowGameObject();
		_E2F4 = new _ECEF<_E000> { _E001(DamageStats.EDamageResult.Regular, value) };
		if (damage.LethalDamagePart == _E0AF && damage.LethalDamage != null)
		{
			_E2F4.Add(_E001(DamageStats.EDamageResult.Lethal, new List<DamageStats> { damage.LethalDamage }));
		}
		UI.AddDisposable(_E002);
		UI.AddDisposable(new _EC71<_E000, DamageIcon>(_E2F4, _damageIconTemplate, base.transform, delegate(_E000 dmg, DamageIcon view)
		{
			view.Show(dmg.ResultType, dmg);
		}));
		healthController.HealthChangedEvent += _E000;
		UI.AddDisposable(delegate
		{
			healthController.HealthChangedEvent -= _E000;
		});
	}

	private void _E000(EBodyPart bodyPart, float diff, _EC23 damageInfo)
	{
		if (this._E000)
		{
			Close();
		}
	}

	private _E000 _E001(DamageStats.EDamageResult resultType, List<DamageStats> damageHistory)
	{
		DamageStats damageStats = null;
		_E000 obj = new _E000(resultType);
		foreach (DamageStats item in damageHistory)
		{
			if (damageStats != null && damageStats.IsEqual(item, compareShots: true))
			{
				damageStats.Add(item);
				continue;
			}
			if (damageStats != null)
			{
				obj.Add(damageStats);
			}
			damageStats = item.Clone();
		}
		if (damageStats != null)
		{
			obj.Add(damageStats);
		}
		return obj;
	}

	private void _E002()
	{
		_E2F4?.Clear();
		_E2F4 = null;
		_E0AE = null;
	}
}
