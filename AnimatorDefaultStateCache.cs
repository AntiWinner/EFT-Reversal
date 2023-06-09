using System;
using UnityEngine;

public class AnimatorDefaultStateCache : MonoBehaviour
{
	[Serializable]
	private struct AnimatorParameterDefaultValue
	{
		[SerializeField]
		private int _parameterNameHash;

		[SerializeField]
		private AnimatorControllerParameterType _type;

		[SerializeField]
		private int _defaultIntValue;

		[SerializeField]
		private float _defaultFloatValue;

		[SerializeField]
		private bool _defaultBoolValue;

		public AnimatorParameterDefaultValue(AnimatorControllerParameter parameter)
		{
			_type = parameter.type;
			_defaultIntValue = 0;
			_defaultFloatValue = 0f;
			_defaultBoolValue = false;
			if (_type == AnimatorControllerParameterType.Int)
			{
				_defaultIntValue = parameter.defaultInt;
			}
			else if (_type == AnimatorControllerParameterType.Float)
			{
				_defaultFloatValue = parameter.defaultFloat;
			}
			else if (_type == AnimatorControllerParameterType.Bool)
			{
				_defaultBoolValue = parameter.defaultBool;
			}
			_parameterNameHash = parameter.nameHash;
		}

		public static void SetDefaultParameter(AnimatorParameterDefaultValue defaultValue, Animator animator)
		{
			if (!animator.IsParameterControlledByCurve(defaultValue._parameterNameHash))
			{
				if (defaultValue._type == AnimatorControllerParameterType.Int)
				{
					animator.SetInteger(defaultValue._parameterNameHash, defaultValue._defaultIntValue);
				}
				else if (defaultValue._type == AnimatorControllerParameterType.Float)
				{
					animator.SetFloat(defaultValue._parameterNameHash, defaultValue._defaultFloatValue);
				}
				else if (defaultValue._type == AnimatorControllerParameterType.Bool)
				{
					animator.SetBool(defaultValue._parameterNameHash, defaultValue._defaultBoolValue);
				}
			}
		}
	}

	[Serializable]
	private struct DefaultLayerState
	{
		[SerializeField]
		private int _layerIndex;

		[SerializeField]
		private float _weight;

		[SerializeField]
		private int _defaultStateHash;

		public DefaultLayerState(int layerIndex, float weight, int defaultStateHash)
		{
			_layerIndex = layerIndex;
			_weight = weight;
			_defaultStateHash = defaultStateHash;
		}

		public static void SetDefaultState(DefaultLayerState layerState, Animator animator)
		{
			animator.SetLayerWeight(layerState._layerIndex, layerState._weight);
			animator.Play(layerState._defaultStateHash, layerState._layerIndex);
		}
	}

	[SerializeField]
	private AnimatorParameterDefaultValue[] _parameters;

	[SerializeField]
	private DefaultLayerState[] _layersState;

	[SerializeField]
	private Animator _animator;

	public void SetupDefaultParameter()
	{
		AnimatorParameterDefaultValue[] parameters = _parameters;
		for (int i = 0; i < parameters.Length; i++)
		{
			AnimatorParameterDefaultValue.SetDefaultParameter(parameters[i], _animator);
		}
		DefaultLayerState[] layersState = _layersState;
		for (int i = 0; i < layersState.Length; i++)
		{
			DefaultLayerState.SetDefaultState(layersState[i], _animator);
		}
	}
}
