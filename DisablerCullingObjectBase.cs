using System.Collections.Generic;
using Comfort.Common;
using EFT;
using EFT.Interactive;
using UnityEngine;

[_E2E2(21000)]
public abstract class DisablerCullingObjectBase : UpdateInEditorSystemComponent<DisablerCullingObjectBase>, _E05C
{
	[SerializeField]
	protected List<Collider> _colliders;

	[SerializeField]
	protected List<Collider> _inverseColliders;

	protected List<ColliderReporter> _enteredColliders = new List<ColliderReporter>(4);

	protected List<ColliderReporter> _enteredInverseColliders = new List<ColliderReporter>(4);

	protected List<ColliderReporter> _colliderReporters = new List<ColliderReporter>(4);

	protected List<ColliderReporter> _inverseColliderReporters = new List<ColliderReporter>(4);

	protected bool _updateComponentsStatus;

	public virtual bool HasEntered
	{
		get
		{
			if (_enteredColliders.Count > 0)
			{
				return _enteredInverseColliders.Count == 0;
			}
			return false;
		}
	}

	public virtual void OnPreProcess()
	{
		PrepareCullingObject();
	}

	protected void CollidersNullCheck()
	{
		CollidersNullCheck(ref _colliders);
		CollidersNullCheck(ref _inverseColliders);
	}

	protected void CollidersNullCheck(ref List<Collider> colliders)
	{
		if (colliders != null)
		{
			for (int num = colliders.Count - 1; num >= 0; num--)
			{
				if (colliders[num] == null)
				{
					colliders.RemoveAt(num);
				}
			}
		}
		else
		{
			colliders = new List<Collider>();
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
	}

	protected virtual void Awake()
	{
		Validate();
		_updateComponentsStatus = true;
	}

	public virtual void PrepareCullingObject()
	{
		CollidersNullCheck();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		foreach (ColliderReporter colliderReporter in _colliderReporters)
		{
			colliderReporter.OnTriggerEnterEvent -= OnTriggerEnterEvent;
			colliderReporter.OnTriggerExitEvent -= OnTriggerExitEvent;
			colliderReporter.RemoveOwner(this);
		}
		_colliderReporters.Clear();
	}

	public override void ManualUpdate(float dt)
	{
		if (_updateComponentsStatus)
		{
			SetComponentsEnabled(HasEntered);
			_updateComponentsStatus = false;
		}
	}

	protected abstract void SetComponentsEnabled(bool hasEntered);

	private void OnValidate()
	{
		Validate();
	}

	protected virtual void Validate()
	{
		_E000(_colliders, _colliderReporters, inverse: false);
		_E000(_inverseColliders, _inverseColliderReporters, inverse: true);
	}

	private void _E000(List<Collider> colliders, List<ColliderReporter> colliderReporters, bool inverse)
	{
		if (colliders == null)
		{
			return;
		}
		colliderReporters.Clear();
		for (int i = 0; i < colliders.Count; i++)
		{
			Collider collider = colliders[i];
			if (!(collider == null))
			{
				ColliderReporter orAddComponent = collider.gameObject.GetOrAddComponent<ColliderReporter>();
				orAddComponent.OnTriggerEnterEvent -= OnTriggerEnterEvent;
				orAddComponent.OnTriggerExitEvent -= OnTriggerExitEvent;
				orAddComponent.OnTriggerEnterEvent += OnTriggerEnterEvent;
				orAddComponent.OnTriggerExitEvent += OnTriggerExitEvent;
				orAddComponent.AddOwner(this);
				orAddComponent.IsInverse = inverse;
				if (colliderReporters.IndexOf(orAddComponent) == -1)
				{
					colliderReporters.Add(orAddComponent);
				}
				if (collider.gameObject.layer != _E37B.DisablerCullingObjectLayer)
				{
					collider.gameObject.layer = _E37B.DisablerCullingObjectLayer;
				}
				if (!collider.isTrigger)
				{
					collider.isTrigger = true;
				}
			}
		}
	}

	protected bool IsRightCollider(Collider col)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
		if (playerByCollider != null)
		{
			return playerByCollider.IsYourPlayer;
		}
		return false;
	}

	protected virtual void OnTriggerEnterEvent(ColliderReporter colliderReporter, Collider collider)
	{
		if (colliderReporter.IsInverse)
		{
			_E001(_enteredInverseColliders, colliderReporter, collider);
		}
		else
		{
			_E001(_enteredColliders, colliderReporter, collider);
		}
	}

	private void _E001(List<ColliderReporter> enteredColliders, ColliderReporter colliderReporter, Collider collider)
	{
		if (!enteredColliders.Contains(colliderReporter) && IsRightCollider(collider))
		{
			bool hasEntered = HasEntered;
			enteredColliders.Add(colliderReporter);
			if (hasEntered != HasEntered)
			{
				UpdateComponentsStatusOnUpdate();
			}
		}
	}

	protected virtual void OnTriggerExitEvent(ColliderReporter colliderReporter, Collider collider)
	{
		if (colliderReporter.IsInverse)
		{
			_E002(_enteredInverseColliders, colliderReporter, collider);
		}
		else
		{
			_E002(_enteredColliders, colliderReporter, collider);
		}
	}

	private void _E002(List<ColliderReporter> enteredColliders, ColliderReporter colliderReporter, Collider collider)
	{
		if (enteredColliders.Contains(colliderReporter) && IsRightCollider(collider))
		{
			bool hasEntered = HasEntered;
			enteredColliders.Remove(colliderReporter);
			if (hasEntered != HasEntered)
			{
				UpdateComponentsStatusOnUpdate();
			}
		}
	}

	protected virtual void UpdateComponentsStatusOnUpdate()
	{
		_updateComponentsStatus = true;
	}
}
