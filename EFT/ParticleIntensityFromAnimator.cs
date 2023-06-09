using UnityEngine;

namespace EFT;

public class ParticleIntensityFromAnimator : MonoBehaviour
{
	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private ParticleSystem _targetParticle;

	[SerializeField]
	private string _animatorParam;

	[SerializeField]
	private Vector2 _minMaxParamValues;

	[SerializeField]
	private Vector2 _minMaxEmissionValue;

	private float _E000;

	private Vector2 _E001;

	private Vector2 _E002;

	private float _E003;

	private float _E004;

	private void Awake()
	{
		_E003 = _minMaxParamValues.x;
		_E004 = _minMaxEmissionValue.x;
		_E001 = new Vector2(0f, _minMaxParamValues.y - _minMaxParamValues.x);
		_E002 = new Vector2(0f, _minMaxEmissionValue.y - _minMaxEmissionValue.x);
	}

	private void Update()
	{
		if (_animator.enabled)
		{
			_E000 = _animator.GetFloat(_animatorParam);
			if (_E000 <= _minMaxParamValues.x)
			{
				_targetParticle.Stop(withChildren: true);
			}
			else if (!_targetParticle.isPlaying)
			{
				_targetParticle.Play(withChildren: true);
			}
		}
	}
}
