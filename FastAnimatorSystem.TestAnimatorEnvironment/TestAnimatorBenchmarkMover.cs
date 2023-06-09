using System;
using System.Collections.Generic;
using UnityEngine;

namespace FastAnimatorSystem.TestAnimatorEnvironment;

public class TestAnimatorBenchmarkMover : MonoBehaviour
{
	public TestAnimatorSkeleton fastSkeletonPrefab;

	public TestAnimatorSkeleton origSkeletonPrefab;

	public float speed = 1.5f;

	public float deltaTreshhold = 0.1f;

	public float actualDelta;

	public int spawnCount = 60;

	private int m__E000 = Animator.StringToHash(_ED3E._E000(63706));

	private int _E001 = Animator.StringToHash(_ED3E._E000(63699));

	private int _E002 = Animator.StringToHash(_ED3E._E000(36788));

	private int _E003 = Animator.StringToHash(_ED3E._E000(127290));

	private List<IAnimator> _E004 = new List<IAnimator>();

	private List<TestAnimatorSkeleton> _E005 = new List<TestAnimatorSkeleton>();

	private float _E006;

	private float _E007;

	private void Update()
	{
		float num = 0f;
		float num2 = 0f;
		if (Input.GetKey(KeyCode.W))
		{
			num2 = 1f;
		}
		if (Input.GetKey(KeyCode.S))
		{
			num2 = -1f;
		}
		if (Input.GetKey(KeyCode.D))
		{
			num = 1f;
		}
		if (Input.GetKey(KeyCode.A))
		{
			num = -1f;
		}
		if (Input.GetKeyDown(KeyCode.F1))
		{
			_E000(spawnCount, origSkeletonPrefab);
		}
		else if (Input.GetKeyDown(KeyCode.F2))
		{
			_E000(spawnCount, fastSkeletonPrefab);
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			foreach (TestAnimatorSkeleton item in _E005)
			{
				item.transform.position = Vector3.zero;
			}
		}
		bool key = Input.GetKey(KeyCode.Space);
		bool flag = Math.Abs(num) > Mathf.Epsilon || Math.Abs(num2) > Mathf.Epsilon || key;
		if (!flag)
		{
			num = _E006;
			num2 = _E007;
		}
		foreach (IAnimator item2 in _E004)
		{
			item2.SetFloat(this.m__E000, num);
			item2.SetFloat(_E001, num2);
			item2.SetBool(_E002, flag);
			item2.SetBool(_E003, key);
		}
		foreach (TestAnimatorSkeleton item3 in _E005)
		{
			item3.Process(Time.deltaTime);
		}
		_E006 = num;
		_E007 = num2;
	}

	private void _E000(int count, TestAnimatorSkeleton skeletonPrefab)
	{
		foreach (TestAnimatorSkeleton item in _E005)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		_E005.Clear();
		_E004.Clear();
		for (int i = 0; i < count; i++)
		{
			GameObject obj = UnityEngine.Object.Instantiate(skeletonPrefab.gameObject);
			TestAnimatorSkeleton component = obj.GetComponent<TestAnimatorSkeleton>();
			_E005.Add(component);
			obj.SetActive(value: true);
			component.Init();
			_E004.Add(component.Animator);
		}
		StopAllCoroutines();
	}
}
