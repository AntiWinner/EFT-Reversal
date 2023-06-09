using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace ChartAndGraph;

[ExecuteInEditMode]
[RequireComponent(typeof(ChartItem))]
public class TextController : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public TextController _003C_003E4__this;

		public float scale;

		internal bool _E000(BillboardText x)
		{
			if (x == null)
			{
				return true;
			}
			if (!_003C_003E4__this._invalidated || x.transform.hasChanged || _003C_003E4__this._canvas.transform.hasChanged)
			{
				x.Rect.transform.position = x.transform.position;
				x.UIText.transform.localScale = new Vector3(x.Scale * scale, x.Scale * scale, 1f);
				x.transform.hasChanged = false;
			}
			return false;
		}
	}

	public Camera Camera;

	public float PlaneDistance = 4f;

	private const float PreviousScale = -1f;

	private Canvas _canvas;

	private bool _invalidated;

	private AnyChart _privateParent;

	private readonly List<BillboardText> _textList = new List<BillboardText>();

	internal List<BillboardText> _E000 => _textList;

	private Canvas _E001
	{
		get
		{
			_E001();
			return _canvas;
		}
	}

	internal AnyChart _E002
	{
		get
		{
			return _privateParent;
		}
		set
		{
			_privateParent = value;
			if (_privateParent != null)
			{
				Camera = ((IInternalUse)_privateParent).InternalTextCamera;
				PlaneDistance = ((IInternalUse)_privateParent).InternalTextIdleDistance;
				this._E001.planeDistance = PlaneDistance;
			}
		}
	}

	private void Start()
	{
		_E001();
		Canvas.willRenderCanvases += _E000;
	}

	private void OnDestroy()
	{
		DestroyAll();
		Canvas.willRenderCanvases -= _E000;
	}

	private void _E000()
	{
		ApplyTextPosition();
	}

	private void _E001()
	{
		if (!(_canvas == null))
		{
			return;
		}
		_canvas = GetComponentInParent<Canvas>();
		if (_canvas == null)
		{
			_canvas = base.gameObject.AddComponent<Canvas>();
			base.gameObject.AddComponent<CanvasScaler>();
			base.gameObject.AddComponent<GraphicRaycaster>();
			if (this._E002 != null && this._E002.VRSpaceText)
			{
				_canvas.renderMode = RenderMode.WorldSpace;
			}
			else
			{
				_canvas.renderMode = RenderMode.ScreenSpaceCamera;
			}
			_canvas.planeDistance = PlaneDistance;
			Camera = _E002();
			GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
		}
	}

	public void DestroyAll()
	{
		foreach (BillboardText text in _textList)
		{
			if (text != null && !text.Recycled)
			{
				if (text.UIText != null)
				{
					_ED15.SafeDestroy(text.UIText.gameObject);
				}
				if (text.RectTransformOverride != null)
				{
					_ED15.SafeDestroy(text.RectTransformOverride.gameObject);
				}
				text.UIText = null;
				text.RectTransformOverride = null;
				_ED15.SafeDestroy(text.gameObject);
			}
		}
		_textList.Clear();
	}

	public void AddText(BillboardText billboard)
	{
		if (!(billboard.UIText == null))
		{
			_invalidated = false;
			_textList.Add(billboard);
			TextDirection component = billboard.Rect.GetComponent<TextDirection>();
			if (component != null)
			{
				component.SetTextController(this);
			}
			GameObject gameObject = _ED15._E001();
			RectTransform component2 = gameObject.GetComponent<RectTransform>();
			gameObject.AddComponent<Canvas>();
			gameObject.transform.SetParent(base.transform, worldPositionStays: false);
			billboard.parent = component2;
			billboard.Rect.SetParent(component2, worldPositionStays: false);
			if (this._E002 != null)
			{
				gameObject.layer = this._E002.gameObject.layer;
				billboard.Rect.gameObject.layer = this._E002.gameObject.layer;
			}
			billboard.Rect.localRotation = Quaternion.identity;
			billboard.Rect.localPosition = Vector3.zero;
			billboard.Rect.localScale = new Vector3(1f, 1f, 1f);
			billboard.UIText.transform.localScale = new Vector3(billboard.Scale, billboard.Scale, 1f);
			RectTransform rect = billboard.Rect;
			if (component == null)
			{
				rect.anchorMin = Vector2.zero;
				rect.anchorMax = Vector2.zero;
			}
			billboard.parent.position = billboard.transform.position;
		}
	}

	private Camera _E002()
	{
		return _E003((Camera == null) ? Camera.main : Camera);
	}

	private Camera _E003(Camera cam)
	{
		Canvas canvas = this._E001;
		if (canvas.worldCamera != cam)
		{
			canvas.worldCamera = cam;
		}
		return cam;
	}

	public void ApplyTextPosition()
	{
		Camera = _E002();
		if (this._E002 != null && this._E002.VRSpaceText)
		{
			_canvas.transform.rotation = Quaternion.LookRotation(new Vector3(base.transform.position.x, 0f, base.transform.position.z) - new Vector3(Camera.transform.position.x, 0f, Camera.transform.position.z), Vector3.up);
			_canvas.transform.localScale = new Vector3(this._E002.VRSpaceScale, this._E002.VRSpaceScale, this._E002.VRSpaceScale);
		}
		float scale = 1f;
		if (_privateParent != null && _privateParent.KeepOrthoSize && Camera != null && Camera.orthographic && Camera.orthographicSize > 0.1f)
		{
			scale = 5f / Camera.orthographicSize;
		}
		if (Mathf.Abs(-1f - scale) < 0.01f)
		{
			_invalidated = false;
		}
		_textList.RemoveAll(delegate(BillboardText x)
		{
			if (x == null)
			{
				return true;
			}
			if (!_invalidated || x.transform.hasChanged || _canvas.transform.hasChanged)
			{
				x.Rect.transform.position = x.transform.position;
				x.UIText.transform.localScale = new Vector3(x.Scale * scale, x.Scale * scale, 1f);
				x.transform.hasChanged = false;
			}
			return false;
		});
		_invalidated = true;
	}
}
