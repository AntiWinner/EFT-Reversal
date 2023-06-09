using System.Runtime.CompilerServices;
using UnityEngine;

namespace Diz.DependencyManager;

public class SystemTester : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public TestLoadable l;

		public SystemTester _003C_003E4__this;

		internal void _E000(TestLoadable t)
		{
			l.AddToken(_003C_003E4__this.System.Retain(t.gameObject.name));
		}
	}

	public _ED0E<TestLoadable> System;

	private TestLoadable[] m__E000;

	private void Start()
	{
		this.m__E000 = _E3AA.FindUnityObjectsOfType<TestLoadable>();
		System = new _ED0E<TestLoadable>(this.m__E000, string.Empty, null);
		TestLoadable[] array = this.m__E000;
		foreach (TestLoadable testLoadable in array)
		{
			TestLoadable i = testLoadable;
			i.OnClicked = delegate(TestLoadable t)
			{
				i.AddToken(System.Retain(t.gameObject.name));
			};
		}
	}

	private void Update()
	{
		TestLoadable[] array = this.m__E000;
		foreach (TestLoadable testLoadable in array)
		{
			testLoadable.SetRefCount(System.Nodes[testLoadable.Key].RefCount);
		}
	}
}
