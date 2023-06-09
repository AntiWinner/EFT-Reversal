using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OnRenderObjectManager : MonoBehaviour
{
	private List<OnRenderObjectConnector> m__E000 = new List<OnRenderObjectConnector>();

	private void Start()
	{
		this.m__E000 = new List<OnRenderObjectConnector>();
		OnRenderObjectConnector.OnRenderObjectConnectorAdd += _E000;
		OnRenderObjectConnector.OnRenderObjectConnectorRemove += _E001;
		_E002();
	}

	private void OnDestroy()
	{
		this.m__E000.Clear();
		OnRenderObjectConnector.OnRenderObjectConnectorAdd -= _E000;
		OnRenderObjectConnector.OnRenderObjectConnectorRemove -= _E001;
	}

	private void OnRenderObject()
	{
		Camera current = Camera.current;
		if (!current)
		{
			return;
		}
		_E8A8 instance = _E8A8.Instance;
		if (current == instance.Camera || current == instance.OpticCameraManager.Camera)
		{
			for (int i = 0; i < this.m__E000.Count; i++)
			{
				this.m__E000[i].ManualOnRenderObject(current);
			}
		}
	}

	private void _E000(OnRenderObjectConnector connector)
	{
		if (!this.m__E000.Contains(connector))
		{
			this.m__E000.Add(connector);
		}
	}

	private void _E001(OnRenderObjectConnector connector)
	{
		if (this.m__E000.Contains(connector))
		{
			this.m__E000.Remove(connector);
		}
	}

	private void _E002()
	{
		OnRenderObjectConnector[] array = Object.FindObjectsOfType<OnRenderObjectConnector>();
		for (int i = 0; i < array.Length; i++)
		{
			_E000(array[i]);
		}
	}
}
