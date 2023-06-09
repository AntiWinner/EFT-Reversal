using UnityEngine;
using UnityEngine.UI;

public class ColorBlinker : MonoBehaviour
{
	[SerializeField]
	private Image _target;

	[SerializeField]
	private AnimationCurve _curve;

	[SerializeField]
	private float _speed;

	public Color StartColor;

	public Color EndColor;

	private void Update()
	{
		_target.color = Color.Lerp(StartColor, EndColor, _curve.Evaluate(Mathf.Repeat(Time.time * _speed, 1f)));
	}
}
