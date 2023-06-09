using System.Collections;
using System.Collections.Generic;
using EFT.GlobalEvents;
using UnityEngine;

namespace EFT.NPC;

public class AnimationIntRandomizerByTimer : MonoBehaviour
{
	[SerializeField]
	private Animator _targetAnimator;

	[SerializeField]
	private Vector2 _randomizeMinMaxSeconds;

	[SerializeField]
	private string _paramForRandomize;

	[SerializeField]
	private int _defaultValue;

	[SerializeField]
	private Vector2 _randomValueFromTo;

	[Header("If false - fills list and randomize from it repeatedly")]
	[SerializeField]
	private bool fullRandom;

	[SerializeField]
	private BaseEventFilter _startWorkFilter;

	[SerializeField]
	private BaseEventFilter _stopWorkFilter;

	private Coroutine m__E000;

	private List<int> m__E001 = new List<int>();

	private void Awake()
	{
		if (!fullRandom)
		{
			_E000();
		}
		_startWorkFilter.OnFilterPassed += _E001;
		_stopWorkFilter.OnFilterPassed += _E002;
	}

	private void OnDestroy()
	{
		_E003(toggle: false);
		_startWorkFilter.OnFilterPassed -= _E001;
		_stopWorkFilter.OnFilterPassed -= _E002;
	}

	private void _E000()
	{
		this.m__E001.Clear();
		for (int i = (int)_randomValueFromTo.x; i <= (int)_randomValueFromTo.y; i++)
		{
			this.m__E001.Add(i);
		}
	}

	private void _E001(BaseEventFilter ef, _EBAD ev)
	{
		_E003(toggle: true);
	}

	private void _E002(BaseEventFilter ef, _EBAD ev)
	{
		_E003(toggle: false);
	}

	private void _E003(bool toggle)
	{
		if (!toggle)
		{
			if (this.m__E000 != null)
			{
				StopCoroutine(this.m__E000);
				this.m__E000 = null;
			}
		}
		else if (this.m__E000 == null)
		{
			this.m__E000 = StartCoroutine(_E005());
		}
	}

	private void _E004()
	{
		if (fullRandom)
		{
			_targetAnimator.SetInteger(_paramForRandomize, (int)Random.Range(_randomValueFromTo.x, _randomValueFromTo.y + 1f));
		}
		else
		{
			if (this.m__E001.Count == 0)
			{
				_E000();
			}
			int index = Random.Range(0, this.m__E001.Count);
			_targetAnimator.SetInteger(_paramForRandomize, this.m__E001[index]);
			this.m__E001.RemoveAt(index);
		}
		this.m__E000 = StartCoroutine(_E005());
	}

	private IEnumerator _E005()
	{
		float num = Random.Range(_randomizeMinMaxSeconds.x, _randomizeMinMaxSeconds.y);
		float num2 = 1f;
		if (num > 1f)
		{
			num -= num2;
		}
		else
		{
			num2 = num / 2f;
			num -= num2;
		}
		float num3 = 0f;
		while (num3 <= 1f)
		{
			num3 += Time.deltaTime / num2;
			yield return null;
		}
		_targetAnimator.SetInteger(_paramForRandomize, _defaultValue);
		num3 = 0f;
		while (num3 <= 1f)
		{
			num3 += Time.deltaTime / num;
			yield return null;
		}
		this.m__E000 = null;
		_E004();
	}
}
