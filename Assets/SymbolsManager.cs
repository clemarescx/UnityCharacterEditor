using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets
{
    // Not ready for use
    public class SymbolsManager
    {
        private readonly string _symbolsPath = "Assets/Data/symbols.json";
        private readonly JObject symbolsDatabase;

        public SymbolsManager()
        {
            var symbolsJson = File.ReadAllText(_symbolsPath);

            symbolsDatabase = JObject.Parse(symbolsJson);

            var topKeys = symbolsDatabase.PropertyValues().ToList();
            topKeys.ForEach(x => Debug.Log(x.ToString()));
            Debug.Log("Top keys: " + topKeys);
        }
    }
}
