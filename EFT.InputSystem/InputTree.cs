using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.InputSystem;

[RequireComponent(typeof(InputManager))]
public sealed class InputTree : InputNodeAbstract, _E7FD
{
	[CompilerGenerated]
	private InputManager _E02B;

	private InputManager _E000
	{
		[CompilerGenerated]
		get
		{
			return _E02B;
		}
		[CompilerGenerated]
		set
		{
			_E02B = value;
		}
	}

	private void Awake()
	{
		_E000 = base.gameObject.GetComponent<InputManager>();
		_E000.TranslateDelegate = TranslateInput;
	}

	public void AddWithLowPriority(InputNode node)
	{
		_children.Insert(0, node);
	}

	public void Add(InputNode node)
	{
		_children.Add(node);
	}

	public void Remove(InputNode node)
	{
		_children.Remove(node);
	}

	public bool Contains(InputNode node)
	{
		return _children.Contains(node);
	}
}
