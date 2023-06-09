using DG.Tweening;
using UnityEngine;

namespace EFT.Hideout.ShootingRange;

public class Turnstile : InteractiveShootingRange
{
	[Range(0f, 1f)]
	[SerializeField]
	private float _duration = 1f;

	[SerializeField]
	private bool _opened;

	private Quaternion _E012 = Quaternion.Euler(0f, 0f, 90f);

	private Quaternion _E013 = Quaternion.Euler(0f, 0f, 0f);

	private float _E014;

	private Tween _E015;

	public override _EC3F InteractionStates(HideoutPlayerOwner owner)
	{
		_EC3F obj = base.InteractionStates(owner);
		if (_opened)
		{
			obj.Actions.Add(new _EC3E
			{
				Name = _ED3E._E000(3113),
				Action = Close
			});
		}
		else
		{
			obj.Actions.Add(new _EC3E
			{
				Name = _ED3E._E000(94194),
				Action = Open
			});
		}
		obj.SelectedAction = obj.Actions[0];
		return obj;
	}

	private void Awake()
	{
		_E014 = Quaternion.Angle(_E012, _E013);
	}

	public void Open()
	{
		_opened = true;
		float duration = _E000(base.transform.localRotation, _E012);
		_E015.Kill();
		_E015 = base.transform.DOLocalRotate(_E012.eulerAngles, duration).SetEase(Ease.Linear);
		SetStateUpdateTime();
	}

	public void Close()
	{
		_opened = false;
		float duration = _E000(base.transform.localRotation, _E013);
		_E015.Kill();
		_E015 = base.transform.DOLocalRotate(_E013.eulerAngles, duration).SetEase(Ease.Linear);
		SetStateUpdateTime();
	}

	private float _E000(Quaternion leftRotation, Quaternion rightRotation)
	{
		return Quaternion.Angle(leftRotation, rightRotation) / _E014 * _duration;
	}
}
