using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;

using Newtonsoft.Json.Linq;

namespace Assets{
	public class SymbolsManager
	{
		private string _symbolsPath = "Assets/Data/symbols.json";
		private JObject symbolsDatabase;

		public SymbolsManager()
		{
			string symbolsJson = File.ReadAllText(_symbolsPath);
			
			symbolsDatabase = JObject.Parse(symbolsJson);

			var topKeys = symbolsDatabase.PropertyValues().ToList();
			topKeys.ForEach( x => Debug.Log(x.ToString()));
			Debug.Log("Top keys: " + topKeys);
		}
		
	}
}
