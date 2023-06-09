using UnityEngine;

namespace EFT;

public class SceneCameraFollow : MonoBehaviour
{
	private Camera m__E000;

	private Camera _E001
	{
		get
		{
			if (this.m__E000 == null)
			{
				this.m__E000 = GetComponent<Camera>();
			}
			return this.m__E000;
		}
	}

	private void Update()
	{
	}

	private static void _E000(Camera follower, Camera target)
	{
		Transform transform = target.transform;
		Transform obj = follower.transform;
		obj.position = transform.position;
		obj.rotation = transform.rotation;
	}
}
