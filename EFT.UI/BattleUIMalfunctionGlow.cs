using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Comfort.Common;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class BattleUIMalfunctionGlow : UIElement
{
	public enum GlowType
	{
		Examined,
		Repaired,
		TypeExamined
	}

	[Serializable]
	public struct GlowParams
	{
		public GlowType Type;

		public Sprite Glow;

		public AnimationCurve Curve;
	}

	[CompilerGenerated]
	private sealed class _E000
	{
		public GlowType glowType;

		internal bool _E000(GlowParams t)
		{
			return t.Type == glowType;
		}
	}

	private const float _E099 = 2f;

	[SerializeField]
	private Image _glow;

	[SerializeField]
	private GlowParams[] _glows;

	private float _E09A;

	private int _E09B;

	private CancellationTokenSource _E09C;

	private float _E09D;

	private void Start()
	{
		_E09A = Time.time;
	}

	public bool ShowGlow(GlowType glowType, bool force = false, float alphaMult = 1f)
	{
		_E09D = alphaMult;
		if (Time.time - _E09A > 2f)
		{
			_E09A = Time.time;
			_E09B = 0;
		}
		_E09B++;
		int showGlowAttemptsCount = Singleton<_E5CB>.Instance.Malfunction.ShowGlowAttemptsCount;
		if (_E09B < showGlowAttemptsCount && !force)
		{
			return false;
		}
		_E09B = -1000;
		_E09C?.Cancel();
		_E09C = new CancellationTokenSource();
		GlowParams glowParams = _glows.First((GlowParams t) => t.Type == glowType);
		_E000(glowParams.Glow, glowParams.Curve, _E09C.Token).HandleExceptions();
		return true;
	}

	private async Task _E000(Sprite sprite, AnimationCurve curve, CancellationToken cancellationToken)
	{
		_glow.enabled = true;
		_glow.sprite = sprite;
		double utcNowUnix = _E5AD.UtcNowUnix;
		double num = utcNowUnix + (double)curve.keys[curve.length - 1].time;
		while (_E5AD.UtcNowUnix < num)
		{
			_glow.color = new Color(1f, 1f, 1f, _E09D * curve.Evaluate((float)(_E5AD.UtcNowUnix - utcNowUnix)));
			await Task.Yield();
			if (cancellationToken.IsCancellationRequested)
			{
				return;
			}
		}
		_glow.enabled = false;
	}
}
