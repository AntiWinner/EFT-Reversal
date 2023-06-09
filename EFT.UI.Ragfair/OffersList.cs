using System;
using System.Collections.Generic;

namespace EFT.UI.Ragfair;

[Serializable]
public class OffersList
{
	public Offer[] offers = new Offer[0];

	public int offersCount;

	public Dictionary<string, int> categories = new Dictionary<string, int>();

	public string selectedCategory = string.Empty;
}
