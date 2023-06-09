using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.Hideout;

public class ShrinkingCircleQTE : QTEAction
{
	private const float m__E000 = 3f;

	[SerializeField]
	private float _speed = 1f;

	[SerializeField]
	private Vector2 _successRange = new Vector2(0.3f, 0.05f);

	[SerializeField]
	private float _minScale = 0.25f;

	[SerializeField]
	private KeyCode _targetInput;

	[Space(10f)]
	[SerializeField]
	private Image _dynamicCircleImage;

	[SerializeField]
	private Transform _dynamicCircleInnerBorder;

	[SerializeField]
	private Transform _dynamicCircleOuterBorder;

	[SerializeField]
	[Space(10f)]
	private Image _staticCircleImage;

	[SerializeField]
	private Transform _staticCircleInnerBorder;

	[SerializeField]
	private Transform _staticCircleOuterBorder;

	[Space(10f)]
	[SerializeField]
	private Image _staticBackgroundImage;

	[SerializeField]
	private Sprite _staticBackgroundSpriteBad;

	[SerializeField]
	private Sprite _staticBackgroundSpriteGood;

	[Space(10f)]
	[SerializeField]
	private Sprite _spriteBad;

	[SerializeField]
	private Sprite _spriteGood;

	private double m__E001;

	private double m__E002;

	private bool m__E003;

	private int _E004;

	public override async Task<bool> StartAction(QuickTimeEvent quickTimeEvent)
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
		}
		_speed = quickTimeEvent.Speed;
		_targetInput = quickTimeEvent.Key;
		_successRange = quickTimeEvent.SuccessRange;
		_E004 = (int)(quickTimeEvent.EndDelay * 1000f);
		this.m__E003 = false;
		_dynamicCircleInnerBorder.DOScale(_minScale, 3f / _speed).SetEase(Ease.Linear);
		_dynamicCircleOuterBorder.DOScale(_minScale, 3f / _speed).SetEase(Ease.Linear);
		float num = (1f - _minScale) * _successRange.x + _minScale;
		_staticCircleOuterBorder.localScale = Vector3.one * (num + _successRange.y);
		_staticCircleInnerBorder.localScale = Vector3.one * num;
		this.m__E001 = num + _successRange.y;
		this.m__E002 = num;
		return await _E000();
	}

	private async Task<bool> _E000()
	{
		while (true)
		{
			await Task.Yield();
			if ((double)_dynamicCircleOuterBorder.transform.localScale.x <= this.m__E002)
			{
				await _E003(success: false);
				break;
			}
			if (Input.GetKeyDown(_targetInput))
			{
				if (!_E001())
				{
					await _E003(success: false);
				}
				else
				{
					await _E003(success: true);
				}
				break;
			}
		}
		return this.m__E003;
	}

	private bool _E001()
	{
		float x = _dynamicCircleOuterBorder.transform.localScale.x;
		bool flag = (double)x >= this.m__E002 && (double)x <= this.m__E001;
		return Input.GetKeyDown(_targetInput) && flag;
	}

	private void _E002(bool success)
	{
		if (success)
		{
			_staticCircleImage.sprite = _spriteGood;
			_staticBackgroundImage.sprite = _staticBackgroundSpriteGood;
			_dynamicCircleImage.sprite = _spriteGood;
		}
		else
		{
			_staticCircleImage.sprite = _spriteBad;
			_staticBackgroundImage.sprite = _staticBackgroundSpriteBad;
			_dynamicCircleImage.sprite = _spriteBad;
		}
	}

	private async Task _E003(bool success)
	{
		this.m__E003 = success;
		_E002(success);
		DOTween.Kill(_dynamicCircleInnerBorder);
		DOTween.Kill(_dynamicCircleOuterBorder);
		await Task.Delay(_E004);
	}
}
