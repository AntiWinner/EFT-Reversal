using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;

namespace EFT.Interactive;

public class Appliance : InteractableObject, ISerializationCallbackReceiver, ISupportsPrefabSerialization
{
	[SerializeField]
	private List<_E80A> _ambianceObjects = new List<_E80A>();

	[SerializeField]
	private string _objectId;

	private bool _E054;

	[SerializeField]
	[HideInInspector]
	private SerializationData _serializationData;

	public string ObjectId => _objectId;

	public bool Enabled => _E054;

	SerializationData ISupportsPrefabSerialization.SerializationData
	{
		get
		{
			return _serializationData;
		}
		set
		{
			_serializationData = value;
		}
	}

	private void Awake()
	{
		foreach (_E80A ambianceObject in _ambianceObjects)
		{
			if (ambianceObject != null)
			{
				ambianceObject.SetEnable(_E838.GetStatus(this, defaultValue: true));
				_E054 = ambianceObject.Enabled;
			}
		}
	}

	public void TurnOn()
	{
		_E000(value: true);
	}

	public void TurnOff()
	{
		_E000(value: false);
	}

	private void _E000(bool value)
	{
		_E054 = value;
		foreach (_E80A ambianceObject in _ambianceObjects)
		{
			ambianceObject?.SetEnable(value);
		}
		_E838.SaveStatus(this);
		SetStateUpdateTime();
	}

	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
		UnitySerializationUtility.DeserializeUnityObject(this, ref _serializationData);
	}

	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
		UnitySerializationUtility.SerializeUnityObject(this, ref _serializationData);
	}
}
