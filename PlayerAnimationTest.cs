using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

public class PlayerAnimationTest : MonoBehaviour
{
	[SerializeField]
	private GameObject _playerPrefab;

	[SerializeField]
	private int _count = 20;

	private List<Animator> m__E000 = new List<Animator>();

	private bool m__E001;

	private bool m__E002;

	private void Awake()
	{
		for (int i = 0; i < _count; i++)
		{
			GameObject gameObject = Object.Instantiate(_playerPrefab);
			_E000(gameObject);
			gameObject.SetActive(value: true);
			Animator componentInChildren = gameObject.GetComponentInChildren<Animator>();
			this.m__E000.Add(componentInChildren);
			gameObject.transform.position = Vector3.Lerp(Vector3.zero, new Vector3(0f, 0f, _count), (float)(i + 1) / (float)_count);
		}
	}

	private void _E000(GameObject gameObject)
	{
		Object.DestroyImmediate(gameObject.GetComponent<CharacterJoint>());
		Object.DestroyImmediate(gameObject.GetComponent<Collider>());
		Object.DestroyImmediate(gameObject.GetComponent<Rigidbody>());
		Object.DestroyImmediate(gameObject.GetComponent<FullBodyBipedIK>());
		Object.DestroyImmediate(gameObject.GetComponent<SolverManager>());
		Object.DestroyImmediate(gameObject.GetComponent<GrounderFBBIK>());
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			_E000(gameObject.transform.GetChild(i).gameObject);
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (this.m__E001)
			{
				_E002();
			}
			else
			{
				_E001();
			}
			this.m__E001 = !this.m__E001;
		}
		if (!Input.GetKeyDown(KeyCode.Backspace) && !this.m__E002)
		{
			return;
		}
		foreach (Animator item in this.m__E000)
		{
			item.SetFloat(_ED3E._E000(63706), Random.value);
		}
		this.m__E002 = true;
	}

	private void _E001()
	{
		foreach (Animator item in this.m__E000)
		{
			item.SetBool(_ED3E._E000(36788), value: true);
			item.SetBool(_ED3E._E000(50050), value: false);
			item.SetFloat(_ED3E._E000(63682), 1f);
		}
	}

	private void _E002()
	{
		foreach (Animator item in this.m__E000)
		{
			item.SetBool(_ED3E._E000(36788), value: false);
		}
	}
}
