using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets{
	[Serializable]
	public class Item {
		public string Name;
		public ItemType Type;
		public double Weight;
		public int Value;
	}
}