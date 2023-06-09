using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Health;

public class BuffableHealthParameterPanel : HealthParameterPanel
{
	private const string m__E000 = "Active";

	[SerializeField]
	private Image _upBuffArrow;

	[SerializeField]
	private Image _downBuffArrow;

	[SerializeField]
	private TMP_Text _buffValue;

	private static readonly Color _E001 = new Color(0.349f, 0.667f, 0.835f);

	private static readonly Color _E002 = new Color(0.729f, 0f, 0f);

	private Animator _E003;

	private Animator _E004;

	private float _E005 = float.MinValue;

	private bool? _E006;

	private bool _E007;

	public void SetBuffValue(float value, bool active, bool positiveIncrease = true)
	{
		if (!_E007)
		{
			_E000();
		}
		if (!Mathf.Approximately(value, _E005) || _E006 != active)
		{
			_E005 = value;
			_E006 = active;
			_downBuffArrow.gameObject.SetActive(value < 0f);
			_upBuffArrow.gameObject.SetActive(value > 0f);
			_buffValue.gameObject.SetActive(value > 0f || value < 0f);
			bool flag = (positiveIncrease ? value.Positive() : value.Negative());
			_buffValue.SetMonospaceText(value.ToString(_ED3E._E000(29354)));
			_buffValue.color = (flag ? _E001 : _E002);
			_downBuffArrow.color = (positiveIncrease ? _E002 : _E001);
			_upBuffArrow.color = (positiveIncrease ? _E001 : _E002);
			_E003.SetBool(_ED3E._E000(50949), active);
			_E004.SetBool(_ED3E._E000(50949), active);
		}
	}

	protected virtual void OnEnable()
	{
		_E006 = _E003.GetBool(_ED3E._E000(50949));
	}

	protected override void Awake()
	{
		if (!_E007)
		{
			_E000();
		}
		base.Awake();
	}

	private void _E000()
	{
		_E007 = true;
		_E003 = _upBuffArrow.gameObject.GetComponent<Animator>();
		_E004 = _downBuffArrow.gameObject.GetComponent<Animator>();
	}
}
