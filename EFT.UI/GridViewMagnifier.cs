using System.Runtime.CompilerServices;
using EFT.UI.DragAndDrop;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class GridViewMagnifier : MonoBehaviour
{
	[SerializeField]
	private GridView _gridView;

	[SerializeField]
	private ScrollRect _scrollRect;

	private Rect? m__E000;

	private RectTransform m__E001;

	private Vector2 m__E002;

	private LayoutElement m__E003;

	private void Awake()
	{
		if (!base.enabled)
		{
			return;
		}
		this.m__E001 = (RectTransform)_scrollRect.transform;
		_scrollRect.onValueChanged.AddListener(delegate(Vector2 position)
		{
			if (!position.Equals(this.m__E002))
			{
				this.m__E002 = position;
				_E001(calculate: false, forceMagnify: false);
			}
		});
		if (!(_gridView == null))
		{
			_gridView.IsMagnified = true;
			this.m__E003 = GetComponent<LayoutElement>();
		}
	}

	public void InitGridView(GridView gridView)
	{
		if (base.enabled)
		{
			_gridView = gridView;
			this.WaitOneFrame(delegate
			{
				_E001(calculate: true, forceMagnify: true);
			});
		}
	}

	private void Update()
	{
		if (base.enabled)
		{
			_E001(calculate: true, forceMagnify: false);
			_E000();
		}
	}

	private void _E000()
	{
		if (!(this.m__E003 == null))
		{
			this.m__E003.minHeight = _gridView.Grid.GridHeight.Value * 63;
		}
	}

	private void _E001(bool calculate, bool forceMagnify)
	{
		if ((object)this.m__E001 == null || (object)_gridView == null || (object)_scrollRect == null)
		{
			return;
		}
		if (calculate)
		{
			Rect rect = this.m__E001.rect;
			Vector3 vector = this.m__E001.TransformPoint(rect.position);
			Vector3 vector2 = this.m__E001.TransformPoint(rect.position + rect.size) - vector;
			rect = new Rect(vector, vector2);
			if (!forceMagnify && this.m__E000 == rect)
			{
				return;
			}
			this.m__E000 = rect;
		}
		if (this.m__E000.HasValue)
		{
			_gridView.MagnifyIfPossible(this.m__E000.Value, forceMagnify);
		}
	}

	private void OnDisable()
	{
		this.m__E000 = null;
	}

	[CompilerGenerated]
	private void _E002(Vector2 position)
	{
		if (!position.Equals(this.m__E002))
		{
			this.m__E002 = position;
			_E001(calculate: false, forceMagnify: false);
		}
	}

	[CompilerGenerated]
	private void _E003()
	{
		_E001(calculate: true, forceMagnify: true);
	}
}
