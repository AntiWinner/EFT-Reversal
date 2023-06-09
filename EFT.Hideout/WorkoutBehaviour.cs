using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Comfort.Common;
using UI.Hideout;
using UnityEngine;

namespace EFT.Hideout;

public sealed class WorkoutBehaviour : _E831, _E832
{
	[CompilerGenerated]
	private new sealed class _E000
	{
		public int triggerLevel;

		internal bool _E000(QteHandleData.PropsData p)
		{
			if (p is QteHandleData.PropsVariantData propsVariantData)
			{
				return propsVariantData.Trigger == triggerLevel;
			}
			return false;
		}
	}

	private const int _E014 = 0;

	private const int _E015 = 1000;

	private const int _E016 = 2500;

	private const int _E017 = 1000;

	private const int _E018 = 2000;

	private const int _E019 = 500;

	private const string _E01A = "WorkoutBrokenArmBlockerMessage";

	private const string _E01B = "WorkoutTiredBlockerMessage";

	private const string _E01C = "arm broke";

	private HideoutPlayerOwner _E013;

	private _E981 _E01D;

	private float _E01E = -2f;

	private float _E01F = -2f;

	private float _E020 = -4f;

	private float _E021 = -4f;

	private QTEController _E022;

	private QteHandleData _E023;

	private List<QteHandleData.PropsData> _E024;

	private List<Transform> _E025 = new List<Transform>();

	private _E815 _E026;

	private new Vector3 _E000 => new Vector3(0f, -500f, 0f);

	private bool _E001
	{
		get
		{
			if (!_E013.HideoutPlayer.HealthController.IsBodyPartBroken(EBodyPart.LeftArm))
			{
				return _E013.HideoutPlayer.HealthController.IsBodyPartBroken(EBodyPart.RightArm);
			}
			return true;
		}
	}

	private bool _E002 => _E023.Requirements.All((Requirement requirement) => requirement.Fulfilled);

	public void StartQte(HideoutPlayerOwner owner, QteHandleData qteData)
	{
		_E013 = owner;
		_E01D = owner.HideoutPlayer.HealthController as _E981;
		_E023 = qteData;
		_E026 = Singleton<_E815>.Instance;
		if (_E023.Id.IsNullOrEmpty())
		{
			_E857.DisplayNotification(new _E89D(_ED3E._E000(170903)));
			return;
		}
		_E001(qteData);
		if (this._E001)
		{
			_E857.DisplayNotification(new _E89D(_ED3E._E000(170937).Localized()));
			return;
		}
		if (!this._E002)
		{
			_E857.DisplayNotification(new _E89D(_ED3E._E000(170968).Localized()));
			return;
		}
		owner.SelectSpecialAreaAction(base._E000);
		if (!_E022)
		{
			_E022 = HideoutAreaQTEOverlay.QteController;
		}
		if (_E025 == null || _E025.Count == 0)
		{
			_E000();
		}
		_E002(qteData.QuickTimeEvents).HandleExceptions();
	}

	private new void _E000()
	{
		float current = _E013.HideoutPlayer.Skills.GetSkill(ESkillId.Strength).Current;
		int triggerLevel = ((!(current < 1000f)) ? ((current < 2500f) ? 1000 : 2500) : 0);
		try
		{
			_E024 = _E023.Props.Where((QteHandleData.PropsData p) => p is QteHandleData.PropsVariantData propsVariantData && propsVariantData.Trigger == triggerLevel).ToList();
			_E025 = new List<Transform>
			{
				UnityEngine.Object.Instantiate(_E024[0].Prefab, Vector3.zero, Quaternion.identity).transform,
				UnityEngine.Object.Instantiate(_E024[1].Prefab, Vector3.zero, Quaternion.identity).transform
			};
		}
		catch (Exception value)
		{
			System.Console.WriteLine(value);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(_ED3E._E000(170947));
			stringBuilder.AppendLine(_ED3E._E000(170947));
			stringBuilder.AppendLine(string.Format(_ED3E._E000(170987), _E024.Count));
			for (int i = 0; i < _E024.Count; i++)
			{
				stringBuilder.AppendLine(string.Format(_ED3E._E000(164880), i, _E024[i].Prefab));
			}
			Debug.LogError(stringBuilder);
			throw;
		}
	}

	private void _E001(QteData qteData)
	{
		try
		{
			_E01E = qteData.Results[QteData.EQteEffectType.SingleSuccessEffect].Energy;
			_E020 = qteData.Results[QteData.EQteEffectType.SingleFailEffect].Energy;
			_E01F = qteData.Results[QteData.EQteEffectType.SingleSuccessEffect].Hydration;
			_E021 = qteData.Results[QteData.EQteEffectType.SingleFailEffect].Hydration;
		}
		catch (ArgumentOutOfRangeException value)
		{
			System.Console.WriteLine(value);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(_ED3E._E000(164917));
			stringBuilder.AppendLine(string.Format(_ED3E._E000(164956), qteData.Results.Count));
			foreach (KeyValuePair<QteData.EQteEffectType, QteResult> result in qteData.Results)
			{
				stringBuilder.AppendLine(string.Format(_ED3E._E000(164935), result.Key, result.Value, result.Value.Effects.Length, result.Value.Hydration, result.Value.Hydration));
			}
			Debug.LogError(stringBuilder);
			throw;
		}
	}

	private async Task _E002(QuickTimeEvent[] quickTimeEvents)
	{
		await Task.Delay(1000);
		_E008();
		_E026.IsClientBusy = true;
		_E83F obj = await _E022.StartQteBySequence(quickTimeEvents);
		float duration = 0f;
		if (_E023.Results.TryGetValue(QteData.EQteEffectType.FinishEffect, out var value))
		{
			QteEffect qteEffect = value.Effects.FirstOrDefault((QteEffect x) => x.Type == QteEffect.EQteRewardType.MusclePain);
			if (qteEffect != null)
			{
				duration = qteEffect.Duration;
			}
		}
		_E01D.CreateMusclePainEffect(duration);
		await Task.Delay(2000);
		if (obj.SequenceResults.Count > 0)
		{
			_E026.WorkoutOperation(obj.SequenceResults, _E023.Id);
		}
		_E013.ExitQte();
		await Task.Delay(500);
		_E013.StopWorkout();
		_E009();
	}

	private void _E003(bool succeed, _E83F qteResult)
	{
		_E01D.ChangeHydration(succeed ? _E01F : _E021);
		_E01D.ChangeEnergy(succeed ? _E01E : _E020);
		if (succeed)
		{
			_E007();
		}
		else
		{
			_E006(qteResult.ActionsFailed);
		}
		_E004(succeed);
	}

	private void _E004(bool succeed)
	{
		int num = UnityEngine.Random.Range(1, 4);
		_E013.HideoutPlayer._animators[0].SetTrigger(succeed ? string.Format(_ED3E._E000(165040), num) : string.Format(_ED3E._E000(165048), num));
	}

	private void _E005(string soundBank)
	{
		if (!(soundBank == _ED3E._E000(95889)))
		{
			if (soundBank == _ED3E._E000(165033))
			{
				_E013.HideoutPlayer.Say(EPhraseTrigger.OnBreath, demand: true, 0f, ETagStatus.Injured);
				return;
			}
			_E023.Sounds.TryGetValue(soundBank, out var value);
			if ((bool)value)
			{
				MonoBehaviourSingleton<BetterAudio>.Instance.PlayAtPoint(_E013.HideoutPlayer.Position, value, value.Rolloff, value.BaseVolume, -1f, EnvironmentType.Indoor);
			}
		}
		else
		{
			_E013.HideoutPlayer.Say(EPhraseTrigger.OnBreath, demand: true, 0f, ETagStatus.Healthy);
		}
	}

	private void _E006(int coef)
	{
		int level = _E013.HideoutPlayer.Skills.Strength.Level;
		float num = (_E01D.HasSevereMusclePainEffect() ? _E01D.GetSevereMusclePainSettings().TraumaChance : 0f);
		float chance = (float)(level / 10 + coef / 4) + num;
		if (Singleton<_E815>.Instance.InQteRandomChance(chance))
		{
			_E01D.DoFracture(Singleton<_E815>.Instance.InQteRandomChance(50f) ? EBodyPart.LeftArm : EBodyPart.RightArm);
			_E857.DisplayNotification(new _E89D(_ED3E._E000(165084).Localized()));
			if (_E023.Results[QteData.EQteEffectType.SingleFailEffect].Effects.First((QteEffect x) => x.Type == QteEffect.EQteRewardType.GymArmTrauma).Result == QteEffect.EQteResultType.Exit)
			{
				_E022.Stop();
			}
			_E013.HideoutPlayer.Say(EPhraseTrigger.OnBeingHurt, demand: true);
		}
	}

	private void _E007()
	{
		QteEffect[] array = _E023.Results[QteData.EQteEffectType.SingleSuccessEffect].Effects.Where((QteEffect effect) => !_E013.Player.Skills.GetSkill(effect.Skill).IsEliteLevel).ToArray();
		if (array.Length == 0)
		{
			return;
		}
		int num = ((array.Length != 1) ? Singleton<_E815>.Instance.QteRandomNext(0, array.Length) : 0);
		QteEffect qteEffect = array[num];
		_E751 skill = _E013.HideoutPlayer.Skills.GetSkill(qteEffect.Skill);
		float num2 = (_E01D.HasSevereMusclePainEffect() ? _E01D.GetSevereMusclePainSettings().GymEffectivity : (_E01D.HasMildMusclePainEffect() ? _E01D.GetMildMusclePainSettings().GymEffectivity : 0f));
		float num3 = 0f;
		QteEffect.SkillExperienceMultiplierData[] skillExpMultiplierData = qteEffect.SkillExpMultiplierData;
		for (int i = 0; i < skillExpMultiplierData.Length; i++)
		{
			QteEffect.SkillExperienceMultiplierData skillExperienceMultiplierData = skillExpMultiplierData[i];
			if (skill.Level >= skillExperienceMultiplierData.level)
			{
				num3 = skillExperienceMultiplierData.value;
			}
		}
		float fac = num3 - num3 * num2;
		float factorValue = _E013.HideoutPlayer.Skills.SkillProgress.Factor(fac).FactorValue;
		float num4 = ((skill.Level >= 9) ? factorValue : skill.CalculateExpOnFirstLevels(factorValue));
		skill.SetCurrent(skill.Current + num4, silent: true);
		_E857.DisplayNotification(new _E89D(string.Format(_ED3E._E000(165078).Localized(), skill.Id.ToString().Localized(), Math.Round(factorValue, 2))));
	}

	private void _E008()
	{
		_E013.PrepareWorkout(_E023.HideoutAnimatorController, _E023.PlayerPosition, _E023.PlayerRotation, _E025[0], _E025[1], _E024[0], _E024[1]);
		_E022.OnSingleQteFinished += _E003;
		GenericEventTranslator eventTranslator = _E013.HideoutPlayer.EventTranslator;
		eventTranslator.OnSoundBankPlay = (Action<string>)Delegate.Combine(eventTranslator.OnSoundBankPlay, new Action<string>(_E005));
	}

	private void _E009()
	{
		_E022.OnSingleQteFinished -= _E003;
		GenericEventTranslator eventTranslator = _E013.HideoutPlayer.EventTranslator;
		eventTranslator.OnSoundBankPlay = (Action<string>)Delegate.Remove(eventTranslator.OnSoundBankPlay, new Action<string>(_E005));
		_E025[0].SetParent(null);
		_E025[0].transform.position = this._E000;
		_E025[1].SetParent(null);
		_E025[1].transform.position = this._E000;
		_E013 = null;
		_E01D = null;
		_E023 = null;
	}

	[CompilerGenerated]
	private bool _E00A(QteEffect effect)
	{
		return !_E013.Player.Skills.GetSkill(effect.Skill).IsEliteLevel;
	}
}
