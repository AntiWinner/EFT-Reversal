using System.Collections.Generic;
using UnityEngine;

namespace AbsolutDecals;

public class DecalTester : MonoBehaviour
{
	public LayerMask Mask = -1;

	public Material Mat;

	private List<Transform> m__E000 = new List<Transform>();

	private List<Renderer> _E001 = new List<Renderer>();

	private int _E002;

	private int _E003;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var _, 1000f, Mask);
		}
		if (Input.GetKeyDown(KeyCode.P))
		{
			MonoBehaviourSingleton<DecalSystem>.Instance.UpdateNonStaticSceneData();
		}
	}

	private bool _E000(Renderer currentRenderer)
	{
		if (currentRenderer == null)
		{
			return false;
		}
		SkinnedMeshRenderer component = currentRenderer.GetComponent<SkinnedMeshRenderer>();
		if (component == null)
		{
			if (currentRenderer.GetComponent<MeshFilter>() == null)
			{
				return false;
			}
			this.m__E000.Add(currentRenderer.transform);
			_E001.Add(currentRenderer.GetComponent<Renderer>());
			if (currentRenderer.gameObject.isStatic)
			{
				if (!Application.isPlaying)
				{
					_E002++;
					_E003++;
				}
				else
				{
					_E002++;
					_E003++;
				}
			}
			else
			{
				_E002++;
				_E003++;
			}
		}
		else if (component.sharedMesh == null)
		{
			return false;
		}
		return true;
	}
}
