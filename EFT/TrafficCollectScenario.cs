using System;
using System.Collections.Generic;
using Dissonance.Config;
using Dissonance.Networking;
using UnityEngine;

namespace EFT;

internal sealed class TrafficCollectScenario : MonoBehaviour
{
	internal sealed class _E000 : _E315
	{
		internal _E000()
			: base(_ED3E._E000(150793), LoggerMode.Add)
		{
		}
	}

	private struct _E001
	{
		public TimeSpan TimeSpan;

		public _E632 GameSent;

		public _E632 GameReceived;

		public _E632 VoipSent;

		public _E632 VoipReceived;
	}

	private _E61E m__E000;

	private TimeSpan m__E001;

	private _E000 m__E002;

	private DateTime _E003;

	private readonly List<_E001> _E004 = new List<_E001>();

	private _E001 _E005;

	private static TimeSpan _E006 => TimeSpan.FromMinutes(1.0);

	internal static TrafficCollectScenario _E000(_E61E game, TimeSpan? step = null, _E000 logger = null)
	{
		TrafficCollectScenario trafficCollectScenario = game.gameObject.AddComponent<TrafficCollectScenario>();
		trafficCollectScenario.m__E000 = game;
		trafficCollectScenario.m__E001 = step ?? _E006;
		trafficCollectScenario.m__E002 = logger ?? new _E000();
		trafficCollectScenario.m__E002.LogInfo(string.Format(_ED3E._E000(150694), VoiceSettings.Instance));
		return trafficCollectScenario;
	}

	public _E631 TrafficData()
	{
		return new _E631
		{
			GameSent = _E005.GameSent,
			GameReceived = _E005.GameReceived,
			VoipSent = _E005.VoipSent,
			VoipReceived = _E005.VoipReceived
		};
	}

	private void Update()
	{
		DateTime utcNow = DateTime.UtcNow;
		if (!(utcNow - _E003 < this.m__E001))
		{
			_E003 = utcNow;
			_E005 = _E001();
			_E004.Add(_E005);
			this.m__E002.LogInfo(string.Format(_ED3E._E000(150736), _E005.TimeSpan.TotalMinutes, _E005.GameSent, _E005.GameReceived, _E005.VoipSent, _E005.VoipReceived));
		}
	}

	private _E001 _E001()
	{
		_E001 result = default(_E001);
		result.TimeSpan = DateTime.UtcNow - _E003;
		result.GameSent = _E002(TrafficCounters.GameSentTraffic);
		result.GameReceived = _E002(TrafficCounters.GameReceivedTraffic);
		result.VoipSent = _E002(TrafficCounters.VoipSentTraffic);
		result.VoipReceived = _E002(TrafficCounters.VoipReceivedTraffic);
		return result;
	}

	private static _E632 _E002(TrafficCounter counter)
	{
		_E632 result = default(_E632);
		result.Bytes = counter.Bytes;
		result.BytesPerSecond = counter.BytesPerSecond;
		result.Packets = counter.Packets;
		return result;
	}
}
