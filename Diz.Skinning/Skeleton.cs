using System.Collections.Generic;
using UnityEngine;

namespace Diz.Skinning;

public class Skeleton : MonoBehaviour, ISerializationCallbackReceiver
{
	public Dictionary<string, Transform> Bones = new Dictionary<string, Transform>();

	[SerializeField]
	private List<string> _keys = new List<string>();

	[SerializeField]
	private List<Transform> _values = new List<Transform>();

	public void OnBeforeSerialize()
	{
		_keys.Clear();
		_values.Clear();
		foreach (KeyValuePair<string, Transform> bone in Bones)
		{
			_keys.Add(bone.Key);
			_values.Add(bone.Value);
		}
	}

	public void OnAfterDeserialize()
	{
		Bones.Clear();
		int count = _keys.Count;
		for (int i = 0; i < count; i++)
		{
			Bones.Add(_keys[i], _values[i]);
		}
	}
}
