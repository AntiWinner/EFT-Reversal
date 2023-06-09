using System;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.UI;

public sealed class RaidStartIntro : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public RaidStartIntro _003C_003E4__this;

		public string playerName;

		public string location;

		public Action _003C_003E9__3;

		public Action _003C_003E9__2;

		public Action _003C_003E9__1;

		internal void _E000()
		{
			_003C_003E4__this.StartCoroutine(_003C_003E4__this._E002(_003C_003E4__this._dayLabel));
			_003C_003E4__this.WaitSeconds(0.3f, delegate
			{
				_003C_003E4__this.StartCoroutine(_003C_003E4__this._E001(_003C_003E4__this._nameLabel, playerName.Transliterate()));
				_003C_003E4__this.WaitSeconds(0.3f, delegate
				{
					_003C_003E4__this.StartCoroutine(_003C_003E4__this._E001(_003C_003E4__this._locationLabel, location));
					_003C_003E4__this.WaitSeconds(4f, delegate
					{
						_003C_003E4__this.StartCoroutine(_003C_003E4__this._E000());
						_003C_003E4__this.WaitSeconds(4f, _003C_003E4__this.Close);
					});
				});
			});
		}

		internal void _E001()
		{
			_003C_003E4__this.StartCoroutine(_003C_003E4__this._E001(_003C_003E4__this._nameLabel, playerName.Transliterate()));
			_003C_003E4__this.WaitSeconds(0.3f, delegate
			{
				_003C_003E4__this.StartCoroutine(_003C_003E4__this._E001(_003C_003E4__this._locationLabel, location));
				_003C_003E4__this.WaitSeconds(4f, delegate
				{
					_003C_003E4__this.StartCoroutine(_003C_003E4__this._E000());
					_003C_003E4__this.WaitSeconds(4f, _003C_003E4__this.Close);
				});
			});
		}

		internal void _E002()
		{
			_003C_003E4__this.StartCoroutine(_003C_003E4__this._E001(_003C_003E4__this._locationLabel, location));
			_003C_003E4__this.WaitSeconds(4f, delegate
			{
				_003C_003E4__this.StartCoroutine(_003C_003E4__this._E000());
				_003C_003E4__this.WaitSeconds(4f, _003C_003E4__this.Close);
			});
		}

		internal void _E003()
		{
			_003C_003E4__this.StartCoroutine(_003C_003E4__this._E000());
			_003C_003E4__this.WaitSeconds(4f, _003C_003E4__this.Close);
		}
	}

	private const float _E0A5 = 0.3f;

	private const float _E0A6 = 4f;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private CustomTextMeshProUGUI _raidNumberLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _dayLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _nameLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _locationLabel;

	private string _E0A7 = string.Empty;

	private DateTime _E0A8;

	private _E629 _E0A9;

	private bool _E0AA;

	private float _E0AB = 1f;

	private float _E0AC = 1f;

	public float AlphaMultiplier
	{
		get
		{
			return _E0AC;
		}
		set
		{
			_E0AC = value;
			_canvasGroup.alpha = _E0AB * _E0AC;
		}
	}

	public void ShowIntro(long raidNumber, DateTime registrationDate, _E629 locationTime, EPlayerSide side, string playerName, string location)
	{
		ShowGameObject();
		Reset();
		_E0A8 = registrationDate;
		_E0A9 = locationTime;
		_E0AA = false;
		StartCoroutine(_E001(_raidNumberLabel, string.Format(_ED3E._E000(250573).Localized(), raidNumber)));
		this.WaitSeconds(0.3f, delegate
		{
			StartCoroutine(_E002(_dayLabel));
			this.WaitSeconds(0.3f, delegate
			{
				StartCoroutine(_E001(_nameLabel, playerName.Transliterate()));
				this.WaitSeconds(0.3f, delegate
				{
					StartCoroutine(_E001(_locationLabel, location));
					this.WaitSeconds(4f, delegate
					{
						StartCoroutine(_E000());
						this.WaitSeconds(4f, Close);
					});
				});
			});
		});
	}

	private void Update()
	{
		if (_E0A9 != null)
		{
			DateTime dateTime = _E0A9.Calculate();
			int hours = dateTime.TimeOfDay.Hours;
			bool flag = hours >= 13 && (hours <= 24 || hours <= 0);
			_E0A7 = _ED3E._E000(250561).Localized() + _ED3E._E000(18502) + _E0A8.ToString(_ED3E._E000(250621) + dateTime.ToString(_ED3E._E000(250617))) + _ED3E._E000(18502) + (flag ? _ED3E._E000(250605) : _ED3E._E000(250610));
			if (_E0AA)
			{
				_dayLabel.text = _E0A7;
			}
		}
	}

	private IEnumerator _E000()
	{
		while (_canvasGroup.alpha >= 0f)
		{
			_E0AB -= Time.deltaTime;
			_canvasGroup.alpha = _E0AB * AlphaMultiplier;
			yield return null;
		}
	}

	private IEnumerator _E001(CustomTextMeshProUGUI label, string to)
	{
		string text = string.Empty;
		for (int i = 0; i < to.Length; i++)
		{
			string text2 = to.ElementAt(i).ToString();
			text = (label.text = text + text2);
			yield return new WaitForSeconds(0.03f);
		}
	}

	private IEnumerator _E002(CustomTextMeshProUGUI label)
	{
		string text = string.Empty;
		for (int i = 0; i < _E0A7.Length; i++)
		{
			string text2 = _E0A7.ElementAt(i).ToString();
			text = (label.text = text + text2);
			yield return new WaitForSeconds(0.03f);
		}
		_E0AA = true;
	}

	private void Reset()
	{
		StopAllCoroutines();
		_E0AB = 1f;
		_raidNumberLabel.text = string.Empty;
		_dayLabel.text = string.Empty;
		_nameLabel.text = string.Empty;
		_locationLabel.text = string.Empty;
	}
}
