using UnityEngine;

public class ExampleSceneController : MonoBehaviour
{
	public GameObject PlayerFPS;

	public GameObject FreeCam;

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			bool activeSelf = PlayerFPS.activeSelf;
			PlayerFPS.SetActive(!activeSelf);
			FreeCam.SetActive(activeSelf);
		}
	}
}
