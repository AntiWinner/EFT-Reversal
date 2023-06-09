using System;
using Newtonsoft.Json;

namespace EFT;

[Serializable]
public sealed class CaptchaData
{
	[JsonProperty("code")]
	public string Code;

	[JsonProperty("items")]
	public string[] Items;

	[JsonProperty("title")]
	public string Title;

	[JsonProperty("description")]
	public string Description;

	[JsonProperty("type")]
	public string Type;
}
