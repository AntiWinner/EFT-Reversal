using UnityEngine;

public class PlayerAction : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E) && Physics.Raycast(Camera.main.ViewportPointToRay(Vector2.one / 2f), out var hitInfo, 2f))
		{
			ActionTrigger component = hitInfo.collider.GetComponent<ActionTrigger>();
			if (component != null)
			{
				component.Use();
			}
		}
	}
}
