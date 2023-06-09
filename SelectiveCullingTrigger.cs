using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SelectiveCullingTrigger : DisablerCullingObject
{
	[SerializeField]
	private Color _childGizmosColor = new Color(1f, 0f, 0f, 0.16f);

	[SerializeField]
	private List<SelectiveCullingTrigger> _childs = new List<SelectiveCullingTrigger>();

	private List<SelectiveCullingTrigger> _E007 = new List<SelectiveCullingTrigger>();

	public override bool HasEntered
	{
		get
		{
			if (!base.HasEntered)
			{
				return _E007.Count > 0;
			}
			return true;
		}
	}

	public bool HasEnteredSelfOnly => base.HasEntered;

	public override void NullClean()
	{
		base.NullClean();
		for (int num = _childs.Count - 1; num >= 0; num--)
		{
			if (_childs[num] == null)
			{
				_childs.RemoveAt(num);
			}
		}
	}

	public void OnParentEnter(SelectiveCullingTrigger parent)
	{
		if (!_E007.Contains(parent))
		{
			_E007.Add(parent);
			base.UpdateComponentsStatusOnUpdate();
		}
	}

	public void OnParentExit(SelectiveCullingTrigger parent)
	{
		if (_E007.Contains(parent))
		{
			_E007.Remove(parent);
			base.UpdateComponentsStatusOnUpdate();
		}
	}

	protected override void UpdateComponentsStatusOnUpdate()
	{
		base.UpdateComponentsStatusOnUpdate();
		foreach (SelectiveCullingTrigger child in _childs)
		{
			if (HasEnteredSelfOnly)
			{
				child.OnParentEnter(this);
			}
			else
			{
				child.OnParentExit(this);
			}
		}
	}
}
