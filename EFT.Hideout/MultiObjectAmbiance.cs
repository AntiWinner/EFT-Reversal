using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using EFT.Interactive;
using EFT.Utilities;
using EFT.Visual;
using UnityEngine;

namespace EFT.Hideout;

public sealed class MultiObjectAmbiance : InteractiveAmbianceObject<MultiObjectAmbiance.AmbianceAffectedObjects>
{
	[Serializable]
	public sealed class AmbianceAffectedObjects
	{
		public List<_E80B> ControlledObjects = new List<_E80B>();

		public float TransitionTime;

		public float RandomRange;

		public Ease TransitionType = Ease.OutQuad;

		private float _currentTransitionTime;

		public void PerformTransition()
		{
			GetTime(update: true);
			if (ControlledObjects.Count == 0)
			{
				return;
			}
			ControlledObjects[0].MainObject = true;
			foreach (_E80B controlledObject in ControlledObjects)
			{
				controlledObject.Parent = this;
				controlledObject.PerformTransition();
			}
		}

		public float GetTime(bool update)
		{
			if (!update)
			{
				return _currentTransitionTime;
			}
			_currentTransitionTime = UnityEngine.Random.Range(TransitionTime - RandomRange / 2f, TransitionTime + RandomRange / 2f);
			if (_currentTransitionTime < 0f)
			{
				_currentTransitionTime = 0f;
			}
			return _currentTransitionTime;
		}

		public void Disable()
		{
			foreach (_E80B controlledObject in ControlledObjects)
			{
				controlledObject.Disable();
			}
		}

		public void Destroy()
		{
			foreach (_E80B controlledObject in ControlledObjects)
			{
				controlledObject.Destroy();
			}
		}
	}

	[Serializable]
	public abstract class AnimatedAmbianceAffectedObject : _E80B
	{
		protected string TweenId;

		private string _propertyId;

		protected string PropertyId
		{
			get
			{
				return _propertyId ?? (_propertyId = GetType().Name);
			}
			set
			{
				_propertyId = value;
			}
		}

		protected string GetObjectId(UnityEngine.Object obj, string propertyId = null)
		{
			return string.Format(_ED3E._E000(168928), obj.GetInstanceID(), propertyId ?? PropertyId);
		}
	}

	[Serializable]
	public abstract class LoopedAmbianceAffectedObject<TUnityObject, TLoopedAmbiance> : AnimatedAmbianceAffectedObject where TUnityObject : UnityEngine.Object where TLoopedAmbiance : LoopedAmbianceAffectedObject<TUnityObject, TLoopedAmbiance>
	{
		public bool Loop;

		public TLoopedAmbiance LoopEffect;

		public TUnityObject AffectedObject;

		[SerializeField]
		[HideInInspector]
		private bool _secondaryAction;

		public void LoopTween(Tween tween, TUnityObject target, string propertyName = null, params Tween[] extraTweens)
		{
			if (TweenId == null)
			{
				TweenId = GetObjectId(target, propertyName);
			}
			DOTween.Kill(TweenId);
			if (extraTweens != null)
			{
				foreach (Tween tween2 in extraTweens)
				{
					if (tween2 != null)
					{
						tween2.stringId = TweenId;
					}
				}
			}
			if (tween != null)
			{
				tween.stringId = TweenId;
			}
			InitTween(tween);
			if (!Loop || LoopEffect == null)
			{
				return;
			}
			LoopEffect.Loop = true;
			LoopEffect.AffectedObject = AffectedObject;
			LoopEffect.LoopEffect = this as TLoopedAmbiance;
			LoopEffect.Parent = base.Parent;
			InitLoopEffect();
			if (tween != null)
			{
				tween.onComplete = delegate
				{
					LoopEffect.PerformTransition();
				};
			}
			else
			{
				LoopEffect.PerformTransition();
			}
		}

		public override void Disable()
		{
			if (TweenId != null)
			{
				DOTween.Complete(TweenId);
			}
			if (!_secondaryAction)
			{
				LoopEffect?.Disable();
			}
		}

		public override void Destroy()
		{
			if (TweenId != null)
			{
				DOTween.Kill(TweenId);
			}
			if (!_secondaryAction)
			{
				LoopEffect?.Destroy();
			}
		}

		protected virtual void InitTween(Tween tween)
		{
		}

		protected virtual void InitLoopEffect()
		{
		}

		private void _E000()
		{
			if (LoopEffect != null)
			{
				LoopEffect._secondaryAction = true;
				LoopEffect.Loop = false;
				LoopEffect.LoopEffect = null;
			}
		}

		protected bool SecondaryAction()
		{
			return _secondaryAction;
		}

		[CompilerGenerated]
		private void _E001()
		{
			LoopEffect.PerformTransition();
		}
	}

	[Serializable]
	public class AffectedMultiShaders : AmbianceAffectedShader
	{
		public List<GameObject> ExtraShaders = new List<GameObject>();

		private IEnumerable<Material> _extraMaterials;

		protected override void InitLoopEffect()
		{
			base.InitLoopEffect();
			(LoopEffect as AffectedMultiShaders).ExtraShaders = ExtraShaders;
		}

		protected override void InitTween(Tween tween)
		{
			base.InitTween(tween);
			if (_extraMaterials == null)
			{
				_extraMaterials = ExtraShaders.Select((GameObject x) => x.GetComponent<Renderer>().materials[MaterialNumber]);
			}
			_E001();
			if (tween == null)
			{
				return;
			}
			tween.onUpdate = delegate
			{
				foreach (Material extraMaterial in _extraMaterials)
				{
					if (Float || Random)
					{
						float @float = Material.GetFloat(PropertyName);
						extraMaterial.SetFloat(PropertyName, @float);
					}
					else if (Color)
					{
						Color color = Material.GetColor(PropertyName);
						extraMaterial.SetColor(PropertyName, color);
					}
				}
			};
		}

		[CompilerGenerated]
		private Material _E000(GameObject x)
		{
			return x.GetComponent<Renderer>().materials[MaterialNumber];
		}

		[CompilerGenerated]
		private void _E001()
		{
			foreach (Material extraMaterial in _extraMaterials)
			{
				if (Float || Random)
				{
					float @float = Material.GetFloat(PropertyName);
					extraMaterial.SetFloat(PropertyName, @float);
				}
				else if (Color)
				{
					Color color = Material.GetColor(PropertyName);
					extraMaterial.SetColor(PropertyName, color);
				}
			}
		}
	}

	[Serializable]
	public class AmbianceAffectedShader : LoopedAmbianceAffectedObject<GameObject, AmbianceAffectedShader>
	{
		public string PropertyName;

		public int MaterialNumber;

		public bool Float = true;

		public bool Color;

		public bool Random;

		public float PropertyValue;

		public Color PropertyColor;

		public RandomBetweenFloats PropertyRandom;

		protected Material Material;

		public override void PerformTransition()
		{
			float time = base.Parent.GetTime(base.MainObject);
			Tween tween = null;
			if (Material == null)
			{
				Material = AffectedObject.GetComponent<Renderer>().materials[MaterialNumber];
			}
			if (Material == null || !Material.HasProperty(PropertyName))
			{
				throw new NullReferenceException();
			}
			if (Float)
			{
				tween = Material.DOFloat(PropertyValue, PropertyName, time);
			}
			else if (Color)
			{
				tween = Material.DOColor(PropertyColor, PropertyName, time);
			}
			else if (Random)
			{
				tween = Material.DOFloat(PropertyRandom.Result, PropertyName, time);
			}
			tween?.SetEase(base.Parent.TransitionType);
			LoopTween(tween, AffectedObject, PropertyName + Material.name);
		}

		protected override void InitLoopEffect()
		{
			base.InitLoopEffect();
			LoopEffect.PropertyName = PropertyName;
			LoopEffect.MaterialNumber = MaterialNumber;
		}

		public void SetColor()
		{
			_E000();
			Color = true;
		}

		public void SetFloat()
		{
			_E000();
			Float = true;
		}

		public void SetRandom()
		{
			_E000();
			Random = true;
		}

		private void _E000()
		{
			Float = false;
			Color = false;
			Random = false;
		}
	}

	[Serializable]
	public sealed class AmbianceAffectedLight : LoopedAmbianceAffectedObject<Light, AmbianceAffectedLight>
	{
		public float Intensity;

		public bool ChangeColor;

		public Color Color;

		public override void PerformTransition()
		{
			Tween tween = null;
			float time = base.Parent.GetTime(base.MainObject);
			TweenerCore<float, float, FloatOptions> tweenerCore = AffectedObject.DOIntensity(Intensity, time);
			tweenerCore.SetEase(base.Parent.TransitionType);
			if (ChangeColor)
			{
				tween = AffectedObject.DOColor(Color, time);
				tween.SetEase(base.Parent.TransitionType);
			}
			LoopTween(tweenerCore, AffectedObject, null, tween);
		}
	}

	[Serializable]
	public sealed class AmbianceAffectedFlicker : _E80B
	{
		public LightFlicker AffectedFlicker;

		public bool IsActive;

		public override void PerformTransition()
		{
			AffectedFlicker.enabled = IsActive;
		}

		public override void Disable()
		{
		}

		public override void Destroy()
		{
		}
	}

	[Serializable]
	public sealed class AmbianceAffectedGameObject : _E80B
	{
		public GameObject AffectedObject;

		public bool IsActive;

		public override void PerformTransition()
		{
			AffectedObject.SetActive(IsActive);
		}

		public override void Disable()
		{
		}

		public override void Destroy()
		{
		}
	}

	[Serializable]
	public sealed class AmbianceAffectedLamp : _E80B
	{
		public LampController AffectedObject;

		public bool Active = true;

		public RandomBetweenFloats Flickering;

		[Range(0f, 100f)]
		public float FlickeringChance;

		public override void PerformTransition()
		{
			if (Active)
			{
				AffectedObject.Switch(Turnable.EState.On);
			}
			else if ((float)UnityEngine.Random.Range(0, 100) < FlickeringChance)
			{
				AffectedObject.Switch(Turnable.EState.ConstantFlickering);
				AffectedObject.FlickeringFreq = Flickering.Result;
			}
			else
			{
				AffectedObject.Switch(Turnable.EState.Off);
			}
		}

		public override void Disable()
		{
		}

		public override void Destroy()
		{
		}
	}

	[Serializable]
	public sealed class AmbianceAffectedComponent : _E80B
	{
		public MonoBehaviour AffectedComponent;

		public string MethodName;

		public override void PerformTransition()
		{
			AffectedComponent.Invoke(MethodName, 0f);
		}

		public override void Disable()
		{
		}

		public override void Destroy()
		{
		}
	}

	public override async Task<bool> PerformInteraction(ELightStatus status)
	{
		if (!(await base.PerformInteraction(status)) || !base.Active || !Patterns.TryGetValue(base.CombinedStatus, out var value))
		{
			return false;
		}
		value.Value.PerformTransition();
		return true;
	}

	private void OnDestroy()
	{
		foreach (KeyValuePair<ELightStatus, Pattern<AmbianceAffectedObjects>> pattern in Patterns)
		{
			_E39D.Deconstruct(pattern, out var _, out var value);
			value.Value.Destroy();
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		foreach (KeyValuePair<ELightStatus, Pattern<AmbianceAffectedObjects>> pattern in Patterns)
		{
			_E39D.Deconstruct(pattern, out var _, out var value);
			value.Value.Disable();
		}
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task<bool> _E000(ELightStatus status)
	{
		return base.PerformInteraction(status);
	}
}
