using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ChartAndGraph;

public class CharItemEffectController : MonoBehaviour
{
	private List<ChartItemEffect> m__E000 = new List<ChartItemEffect>();

	private Transform _E001;

	[CompilerGenerated]
	private bool _E002;

	[CompilerGenerated]
	private bool _E003;

	private Vector3 _E004;

	internal bool _E005
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
		[CompilerGenerated]
		set
		{
			_E002 = value;
		}
	}

	internal bool _E006
	{
		[CompilerGenerated]
		get
		{
			return _E003;
		}
		[CompilerGenerated]
		set
		{
			_E003 = value;
		}
	}

	protected Transform Parent
	{
		get
		{
			if (_E001 == null)
			{
				_E001 = base.transform.parent;
			}
			return _E001;
		}
	}

	public CharItemEffectController()
	{
		_E006 = true;
	}

	private void Start()
	{
		_E004 = base.transform.localScale;
	}

	private void _E000()
	{
		_E004 = base.transform.localScale;
	}

	private void Update()
	{
		Transform parent = base.transform;
		if (_E005)
		{
			parent = Parent;
			if (parent == null)
			{
				return;
			}
		}
		Vector3 localScale = new Vector3(1f, 1f, 1f);
		if (_E006)
		{
			localScale = _E004;
		}
		Vector3 zero = Vector3.zero;
		Quaternion identity = Quaternion.identity;
		for (int i = 0; i < this.m__E000.Count; i++)
		{
			ChartItemEffect chartItemEffect = this.m__E000[i];
			if (chartItemEffect == null || chartItemEffect.gameObject == null)
			{
				this.m__E000.RemoveAt(i);
				i--;
				continue;
			}
			localScale.x *= chartItemEffect._E007.x;
			localScale.y *= chartItemEffect._E007.y;
			localScale.z *= chartItemEffect._E007.z;
			zero += chartItemEffect._E009;
			identity *= chartItemEffect._E008;
		}
		parent.localScale = localScale;
	}

	public void Unregister(ChartItemEffect effect)
	{
		this.m__E000.Remove(effect);
		if (this.m__E000.Count == 0)
		{
			base.enabled = false;
		}
	}

	public void Register(ChartItemEffect effect)
	{
		if (!this.m__E000.Contains(effect))
		{
			if (!base.enabled)
			{
				base.enabled = true;
			}
			this.m__E000.Add(effect);
		}
	}
}
