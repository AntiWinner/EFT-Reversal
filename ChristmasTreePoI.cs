using System;
using System.Collections.Generic;
using Comfort.Common;
using EFT;
using UnityEngine;

public class ChristmasTreePoI : BotPointOfInterest
{
	private const float m__E000 = 3f;

	private const int m__E001 = 6;

	[SerializeField]
	private List<KhorovodPosition> points = new List<KhorovodPosition>();

	[SerializeField]
	private List<KhorovodState> _khorovodStates = new List<KhorovodState>();

	private int m__E002;

	private float m__E003;

	private float m__E004;

	private float m__E005;

	private List<EPhraseTrigger> m__E006 = new List<EPhraseTrigger>();

	[Range(0f, 1f)]
	[SerializeField]
	private float _lookDirectionDelta = 0.2f;

	[SerializeField]
	private float _phrasesRecalcPeriod = 1.2f;

	private int _E007;

	private float _E008;

	private float _E009;

	public float shootingTime = 0.6f;

	[Range(1f, 3f)]
	public int shootersLimit = 1;

	public int skipRateMin = 10;

	public int skipRateMax = 16;

	public int skips = 9999;

	public int skipRateRolled;

	private KhorovodState _E00A => _khorovodStates[this.m__E002 % _khorovodStates.Count];

	public EGesture Gesture
	{
		get
		{
			if (!RandomGesture)
			{
				return _E00A.Gesture;
			}
			return (EGesture)_E39D.Random(0f, 8f);
		}
	}

	public EPhraseTrigger Phrase
	{
		get
		{
			if (!RandomPhrase)
			{
				return _E00A.Phrase;
			}
			return this.m__E006[UnityEngine.Random.Range(0, this.m__E006.Count)];
		}
	}

	public bool HaveGesture => _E00A.HaveGesture;

	public bool RandomGesture => _E00A.RandomGesture;

	public bool RandomPhrase => _E00A.RandomPhrase;

	public bool ForcedAutomaticFireMode => _E00A.ForcedAutomaticFireMode;

	private float _E00B => _E00A.RotationVelocity;

	private float _E00C => _E00A.Radius;

	private float _E00D => _E00A.BotsVelocity;

	private float _E00E => _E00A.LookHeight;

	public float LookDirectionDelta => _lookDirectionDelta;

	private void _E000()
	{
		if (skips > skipRateRolled)
		{
			skips = 0;
			skipRateRolled = UnityEngine.Random.Range(skipRateMin, skipRateMax);
		}
	}

	public override void Init(BotLocationModifier modifier)
	{
		_E000();
		skips = skipRateRolled / 2;
		bool flag = _E39D.Random(0f, 100f) < modifier.KhorovodChance;
		base.gameObject.SetActive(flag);
		base.enabled = flag;
		if (flag)
		{
			Array values = Enum.GetValues(typeof(EPhraseTrigger));
			for (int i = 0; i < values.Length; i++)
			{
				this.m__E006.Add((EPhraseTrigger)values.GetValue(i));
			}
			Singleton<_E307>.Instance.OnKill += ResetEnemiesIgnoreState;
		}
	}

	public bool HavePhrase(BotOwner owner)
	{
		if (!_E00A.HavePhrase)
		{
			return false;
		}
		foreach (KhorovodPosition point in points)
		{
			if (point.attachedOwner == owner)
			{
				return point.havePhrase;
			}
		}
		return false;
	}

	private void FixedUpdate()
	{
		this.m__E004 += Time.fixedDeltaTime;
		this.m__E003 += Time.fixedDeltaTime * _E00B / ((float)Math.PI * 2f * _E00C);
		if (this.m__E004 > _E00A.StateDuration)
		{
			do
			{
				this.m__E002 = (this.m__E002 + 1) % _khorovodStates.Count;
				if (this.m__E002 % _khorovodStates.Count == 0)
				{
					skips++;
					_E000();
				}
				this.m__E004 = 0f;
			}
			while (_E00A.skip && skips != skipRateRolled);
		}
		if (Time.time > _E009 + shootingTime)
		{
			_E009 = Time.time;
			if (points.Count > 0)
			{
				_E007 = (_E007 + 1) % points.Count;
			}
			else
			{
				_E007 = 0;
			}
		}
	}

	private int _E001(BotOwner owner)
	{
		for (int i = 0; i < points.Count; i++)
		{
			if (points[i].attachedOwner == owner)
			{
				return i;
			}
		}
		return -1;
	}

	public bool Shooting(BotOwner owner)
	{
		int num = _E001(owner);
		switch (shootersLimit)
		{
		case 1:
			if (_E00A.Shooting)
			{
				return num == _E007;
			}
			return false;
		case 2:
			if (_E00A.Shooting)
			{
				if (num != _E007)
				{
					return (num + 3) % points.Count == _E007;
				}
				return true;
			}
			return false;
		case 3:
			if (_E00A.Shooting)
			{
				if (num != _E007 && (num + 2) % points.Count != _E007)
				{
					return (num + 4) % points.Count == _E007;
				}
				return true;
			}
			return false;
		default:
			if (_E00A.Shooting)
			{
				return num == _E007;
			}
			return false;
		}
	}

	private void _E002()
	{
		foreach (BotOwner item in BotsInKhorovod())
		{
			if (item.BewareGrenade.ShallRunAway())
			{
				ResetEnemiesIgnoreState(item.BewareGrenade.GrenadeDangerPoint.Grenade.Player);
			}
		}
	}

	public bool ProcessKhorovodPlace(BotOwner owner)
	{
		_E004();
		if (points.Count < 6)
		{
			return true;
		}
		foreach (KhorovodPosition point in points)
		{
			if (point.attachedOwner == owner)
			{
				return true;
			}
		}
		return points.Count < 6;
	}

	public Vector3 InitialPosition()
	{
		return base.transform.position + new Vector3(Mathf.Cos(0f), 0f, Mathf.Sin(0f)) * _E00C;
	}

	public Vector3 GetPoint(BotOwner owner)
	{
		_E004();
		bool flag = false;
		KhorovodPosition khorovodPosition = null;
		for (int i = 0; i < points.Count; i++)
		{
			KhorovodPosition khorovodPosition2 = points[i];
			if (khorovodPosition2.attachedOwner == null || khorovodPosition2.attachedOwner.IsDead)
			{
				khorovodPosition2.attachedOwner = owner;
			}
			if (khorovodPosition2.attachedOwner == owner)
			{
				khorovodPosition = khorovodPosition2;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			khorovodPosition = new KhorovodPosition();
			khorovodPosition.attachedOwner = owner;
			points.Add(khorovodPosition);
		}
		_E005();
		if (Time.time > this.m__E005)
		{
			this.m__E005 = Time.time + 3f;
			_E002();
		}
		if (Time.time > _E008)
		{
			_E008 = Time.time + _phrasesRecalcPeriod;
			_E003();
		}
		return base.transform.position + khorovodPosition.CalcPosition(_E00C, this.m__E003);
	}

	private void _E003()
	{
		foreach (KhorovodPosition point in points)
		{
			point.havePhrase = false;
		}
		int count = points.Count;
		int num = Mathf.Min(_E00A.speakersLimit, count);
		for (int i = 0; i < num; i++)
		{
			int num2 = UnityEngine.Random.Range(0, count);
			for (int j = num2; j < num2 + count; j++)
			{
				if (!points[j % count].havePhrase)
				{
					points[j % count].havePhrase = true;
					break;
				}
			}
		}
	}

	public float GetMoveVelocity()
	{
		return _E00D;
	}

	public Vector3 GetLookPoint()
	{
		return base.transform.position + Vector3.up * _E00E;
	}

	private void _E004()
	{
		for (int i = 0; i < points.Count; i++)
		{
			KhorovodPosition khorovodPosition = points[i];
			if (khorovodPosition.attachedOwner == null || khorovodPosition.attachedOwner.IsDead)
			{
				points.RemoveAt(i);
				i--;
				_E005();
			}
		}
	}

	private void _E005()
	{
		for (int i = 0; i < points.Count; i++)
		{
			points[i].angle = (float)Math.PI * 2f / (float)points.Count * (float)i;
		}
	}

	public void ResetEnemiesIgnoreState(_E5B4 player, _E5B4 target)
	{
		if (IsNear(target) || _E006())
		{
			ResetEnemiesIgnoreState(player);
		}
	}

	private bool _E006()
	{
		int num = 0;
		int num2 = 0;
		foreach (BotOwner item in BotsInKhorovod())
		{
			if (item.Brain != null)
			{
				if (item.Brain.BaseBrain.IsLayerActive<_E184>())
				{
					num++;
				}
				else
				{
					num2++;
				}
			}
		}
		return num2 > num;
	}

	public void ResetEnemiesIgnoreState(_E5B4 player)
	{
		if (player == null || player.IsAI)
		{
			return;
		}
		foreach (BotOwner item in BotsInKhorovod(nearOnly: false))
		{
			if (!(item == null) && !item.IsDead && item.BotsGroup != null && item.EnemiesController != null && item.EnemiesController.EnemyInfos != null)
			{
				item.BotsGroup.AddEnemy(player);
				if (item.EnemiesController.EnemyInfos.TryGetValue(player, out var value))
				{
					value.SetIgnoreState(state: false);
				}
			}
		}
	}

	public bool IsNear(_E5B4 player)
	{
		if (player == null || _E00A == null)
		{
			return false;
		}
		return player.Position.SqrDistance(base.transform.position) < _E00A.Radius * _E00A.Radius * 4f;
	}

	public IEnumerable<BotOwner> BotsInKhorovod(bool nearOnly = true)
	{
		foreach (KhorovodPosition point in points)
		{
			if (point != null && !(point.attachedOwner == null) && _E00A != null && point.attachedOwner != null && ((nearOnly && IsNear(point.attachedOwner)) || !nearOnly))
			{
				yield return point.attachedOwner;
			}
		}
	}

	private void OnDestroy()
	{
		if (Singleton<_E307>.Instance != null)
		{
			Singleton<_E307>.Instance.OnKill -= ResetEnemiesIgnoreState;
		}
	}

	private void OnDrawGizmos()
	{
		float radius = 0f;
		if (_E00A != null)
		{
			radius = _E00C;
		}
		_E36B.DrawCircle(base.transform.position, Vector3.up, Color.green, radius);
		_E005();
		foreach (KhorovodPosition point in points)
		{
			Gizmos.DrawSphere(base.transform.position + point.CalcPosition(radius, this.m__E003), 0.25f);
		}
	}
}
