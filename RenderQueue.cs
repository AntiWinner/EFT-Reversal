using UnityEngine;

public class RenderQueue : MonoBehaviour
{
	public enum Queues
	{
		Cancel = -1,
		Background = 1000,
		Geometry = 2000,
		AlphaTest = 2450,
		Transparent = 3000,
		Overlay = 4000
	}

	public int Queue = 3000;

	public Queues SetQueue = (Queues)(-2);

	private void Start()
	{
		_E000();
	}

	private void OnValidate()
	{
		if (SetQueue > (Queues)0)
		{
			Queue = (int)SetQueue;
		}
		SetQueue = (Queues)(-2);
		_E000();
	}

	private void _E000()
	{
		Material[] sharedMaterials = GetComponent<Renderer>().sharedMaterials;
		for (int i = 0; i < sharedMaterials.Length; i++)
		{
			sharedMaterials[i].renderQueue = Queue;
		}
	}
}
