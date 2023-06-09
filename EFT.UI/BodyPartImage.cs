using System;
using Comfort.Common;
using EFT.UI.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class BodyPartImage : UIElement
{
	private const float _E0AD = 0.5f;

	[SerializeField]
	private Image _bodyPartImage;

	private _E9C4 _E0AE;

	private EBodyPart _E0AF;

	private float _E0B0;

	private EHealthColorScheme _E0B1;

	public void Show(_E9C4 healthController, EBodyPart bodyPart)
	{
		_E0B1 = Singleton<_E7DE>.Instance.Game.Settings.HealthColor;
		_E0B0 = -1f;
		_E0AE = healthController;
		_E0AF = bodyPart;
		ShowGameObject();
	}

	private void Update()
	{
		if (_E0AE != null)
		{
			float normalized = _E0AE.GetBodyPartHealth(_E0AF).Normalized;
			if (!(Math.Abs(normalized - _E0B0) < Mathf.Epsilon))
			{
				_E0B0 = normalized;
				bool destroyed = _E0AE.IsBodyPartDestroyed(_E0AF);
				_bodyPartImage.color = _E0B1.GetBodyPartColor(_E0B0, destroyed).Saturation(0.5f);
			}
		}
	}
}
