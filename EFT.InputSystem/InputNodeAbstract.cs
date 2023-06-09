using System.Collections.Generic;
using UnityEngine;

namespace EFT.InputSystem;

public abstract class InputNodeAbstract : MonoBehaviour
{
	[SerializeField]
	protected List<InputNode> _children = new List<InputNode>();

	public void CheckDuplicateChildren()
	{
		HashSet<InputNode> hashSet = new HashSet<InputNode>();
		for (int num = _children.Count - 1; num >= 0; num--)
		{
			if (hashSet.Contains(_children[num]))
			{
				_children.RemoveAt(num);
			}
			else
			{
				hashSet.Add(_children[num]);
			}
		}
	}

	protected virtual void TranslateInput(List<ECommand> commands, ref float[] axes, ref ECursorResult shouldLockCursor)
	{
		for (int num = _children.Count - 1; num >= 0; num--)
		{
			InputNode inputNode = _children[num];
			if (inputNode == null)
			{
				_children.RemoveAt(num);
				num--;
			}
			else if (inputNode.enabled && inputNode.gameObject.activeSelf)
			{
				inputNode.TranslateInput(commands, ref axes, ref shouldLockCursor);
			}
		}
	}
}
