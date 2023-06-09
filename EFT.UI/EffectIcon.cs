using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public class EffectIcon : UIElement
{
	private const float _E0BD = 0.5f;

	[SerializeField]
	private Image _icon;

	private DateTime _E0BE;

	private SimpleTooltip _E02A;

	private float _E0BF;

	private bool _E0C0;

	private _E98F _E0C1;

	private List<_E98D> _E0C2 = new List<_E98D>();

	private HoverTrigger _E0C3;

	private Coroutine _E0C4;

	private Coroutine _E0C5;

	private string _E000 => string.Join(_ED3E._E000(2540), (from x in _E0C2
		where !string.IsNullOrEmpty(x.Text)
		select _E0C1.ExactTime ? ((!_E0C0) ? x.TimeLeftDisplay : x.NameDisplay) : x.TimeLeftApproximatelyDisplay).ToArray());

	private void Awake()
	{
		_E0C3 = base.gameObject.GetOrAddComponent<HoverTrigger>();
		_E0C3.OnHoverStart += delegate
		{
			if (_E0C5 != null)
			{
				StopCoroutine(_E0C5);
			}
			_E0C5 = StartCoroutine(_E000());
			_E02A.Show(this._E000);
		};
		_E0C3.OnHoverEnd += delegate
		{
			if (_E0C5 != null)
			{
				StopCoroutine(_E0C5);
			}
			_E02A.Close();
		};
	}

	private void OnDestroy()
	{
		_E0C1.OnBuffsUpdated -= BuffsUpdatedHandler;
	}

	public void Show(_E98F variation, [CanBeNull] SimpleTooltip tooltip, bool blinking)
	{
		ShowGameObject();
		_E0C1 = variation;
		_E02A = tooltip;
		_E0C0 = blinking;
		_E0C1.OnBuffsUpdated += BuffsUpdatedHandler;
		BuffsUpdatedHandler();
		_icon.color = (_E0C0 ? new Color(1f, 1f, 1f, 0.2f) : Color.white);
		_icon.sprite = EFTHardSettings.Instance.StaticIcons.EffectIcons.EffectIcons[variation.Type];
		if (_E0C0)
		{
			if (_E0C4 != null)
			{
				StaticManager.KillCoroutine(_E0C4);
			}
			_E0C4 = StaticManager.BeginCoroutine(_E001(direction: true));
		}
	}

	public void BuffsUpdatedHandler()
	{
		_E0C2 = _E0C1.Buffs;
		base.gameObject.SetActive(_E0C2.Count > 0);
		if (_E0C2.Count > 0)
		{
			if (_E02A != null && _E02A.gameObject.activeSelf)
			{
				_E02A.SetText(this._E000);
			}
		}
		else if (_E02A != null)
		{
			_E02A.Close();
		}
	}

	private void Update()
	{
		if (_E0C2.Count <= 0)
		{
			return;
		}
		DateTime utcNow = _E5AD.UtcNow;
		DateTime dateTime = _E0BE.AddSeconds(0.5);
		if (utcNow < dateTime || utcNow == _E0BE)
		{
			return;
		}
		_E0BE = utcNow;
		bool flag = true;
		foreach (_E98D item in _E0C2)
		{
			if (item.TimeLeft > 0)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			Close();
		}
	}

	private IEnumerator _E000()
	{
		while (!(_E02A == null))
		{
			if (_E02A.gameObject.activeSelf)
			{
				_E02A.SetText(this._E000);
			}
			yield return new WaitForSeconds(1f);
		}
		_E0C5 = null;
	}

	private IEnumerator _E001(bool direction)
	{
		Color color = _icon.color;
		while (direction ? (color.a < 1f) : ((double)color.a >= 0.2))
		{
			color = (direction ? new Color(1f, 1f, 1f, color.a + Time.deltaTime) : new Color(1f, 1f, 1f, color.a - Time.deltaTime));
			_icon.color = color;
			yield return null;
		}
		StartCoroutine(_E001(!direction));
	}

	public override void Close()
	{
		_E0C1.OnBuffsUpdated -= BuffsUpdatedHandler;
		if (_E0C4 != null)
		{
			StaticManager.KillCoroutine(_E0C4);
		}
		if (_E02A != null)
		{
			_E02A.Close();
		}
		if (_E0C5 != null)
		{
			StopCoroutine(_E0C5);
		}
		base.Close();
	}

	[CompilerGenerated]
	private string _E002(_E98D x)
	{
		if (_E0C1.ExactTime)
		{
			if (!_E0C0)
			{
				return x.TimeLeftDisplay;
			}
			return x.NameDisplay;
		}
		return x.TimeLeftApproximatelyDisplay;
	}

	[CompilerGenerated]
	private void _E003(PointerEventData arg)
	{
		if (_E0C5 != null)
		{
			StopCoroutine(_E0C5);
		}
		_E0C5 = StartCoroutine(_E000());
		_E02A.Show(this._E000);
	}

	[CompilerGenerated]
	private void _E004(PointerEventData _)
	{
		if (_E0C5 != null)
		{
			StopCoroutine(_E0C5);
		}
		_E02A.Close();
	}
}
