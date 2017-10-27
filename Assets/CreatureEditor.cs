using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Assets{
	public class CreatureEditor : EditorWindow{
		private static Creature _cachedCreature;
		private Vector2 _scrollPos;
		private bool _showInventory = true;

		[MenuItem("Window/Creature Editor 0.0.1")]
		private static void Init(){
			CreatureEditor window = (CreatureEditor)GetWindow(typeof(CreatureEditor), false, "Creature Editor");
			window.Show();
		}

		private void OnGUI(){
			GUILayout.BeginHorizontal(GUILayout.MaxWidth(300)); // Buttons start
			DisplayTopButtons();
			if(_cachedCreature == null)
				return;
			GUILayout.EndHorizontal(); // buttons end

			_scrollPos = GUILayout.BeginScrollView(_scrollPos);

			DisplayAttributes();
			DisplayInventory();

			_cachedCreature.Position = EditorGUILayout.Vector2Field("Position", _cachedCreature.Position);

			GUILayout.EndScrollView();
		}

		private static void DisplayAttributes(){
			_cachedCreature.Name = EditorGUILayout.TextField("Name", _cachedCreature.Name);
			_cachedCreature.Strength = EditorGUILayout.IntSlider("Strength", _cachedCreature.Strength, 0, 20);
			_cachedCreature.Dexterity = EditorGUILayout.IntSlider("Dexterity", _cachedCreature.Dexterity, 0, 20);
			_cachedCreature.Constitution = EditorGUILayout.IntSlider("Constitution", _cachedCreature.Constitution, 0, 20);
			_cachedCreature.CreatureRace = (CreatureRace)EditorGUILayout.EnumPopup("CreatureRace", _cachedCreature.CreatureRace);
			_cachedCreature.Type = (CreatureType)EditorGUILayout.EnumPopup("Type", _cachedCreature.Type);

			EditorGUILayout.LabelField("Description:");
			var descriptionStyle = new GUIStyle(GUI.skin.textArea){ fixedHeight = 50f };
			_cachedCreature.Description = EditorGUILayout.TextArea(_cachedCreature.Description, descriptionStyle);
		}

		private void DisplayInventory(){
			GUILayout.Label("Inventory");
			// Gold 
			GUILayout.BeginHorizontal();
			GUILayout.Label("Gold:");
			_cachedCreature.Gold = EditorGUILayout.IntField(_cachedCreature.Gold);
			GUILayout.EndHorizontal();

			// Inventory
			_showInventory = EditorGUILayout.Foldout(_showInventory, "Items");
			if(_showInventory){
				var itemCount = _cachedCreature.Inventory.Count;
				EditorGUI.indentLevel += 1;
				for(int i = 0; i < itemCount; i++){
					var item = _cachedCreature.Inventory[i];
					
					item.Name = EditorGUILayout.TextField("Name", item.Name);
					item.Type = (ItemType)EditorGUILayout.EnumPopup("Type", item.Type);

					GUILayout.BeginHorizontal();
					item.Value = EditorGUILayout.IntField("Value", item.Value);
					item.Weight = EditorGUILayout.DoubleField("Weight", item.Weight);

					if(GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(10)))
						//TODO: fix out-of-range error for all-but-last items
						_cachedCreature.Inventory.Remove(item);

					GUILayout.EndHorizontal();
					
					GUILayout.Space(10);
				}
				EditorGUI.indentLevel -= 1;

				if(GUILayout.Button("Add item"))
					_cachedCreature.Inventory.Add(new Item{ Name = "New Item" });

			}
		}

		private static void DisplayTopButtons(){
			if(GUILayout.Button("New creature")){
				_cachedCreature = CreateInstance<Creature>();
				_cachedCreature.Inventory = new List<Item>();
			}
			GUI.enabled = _cachedCreature != null;
			if(GUILayout.Button("Export")){
				ExportToJson(_cachedCreature);
			}
			GUI.enabled = true;
			if(GUILayout.Button("Import")){
				_cachedCreature = ImportFromJson();
			}
			if(GUILayout.Button("Generate")){
				_cachedCreature = GenerateCreature();
			}
		}


		private static Creature ImportFromJson(){
			var importedCreaturePath = EditorUtility.OpenFilePanel("Import creature", "Assets/Creatures", "json");
			var importedCreatureJson = File.ReadAllText(importedCreaturePath);
			var importedCreature = CreateInstance<Creature>();
			JsonUtility.FromJsonOverwrite(importedCreatureJson, importedCreature);

			if(importedCreature != null)
				return importedCreature;

			EditorUtility.DisplayDialog(
				"Import creature",
				string.Format("Deserialization of creature failed\n{0}", importedCreaturePath),
				"OK");
			return _cachedCreature;
		}

		private static void ExportToJson(Creature creature){
			if(creature == null){
				EditorUtility.DisplayDialog("Save creature", "No creature to save.", "OK");
				return;
			}
			var path = EditorUtility.SaveFilePanel("Save creature", "Assets/Creatures", creature.Name, "json");

			if(path.Length == 0)
				return;

			string toJson = JsonUtility.ToJson(creature, true);

			Debug.Log(toJson);
			if(toJson != null)
				File.WriteAllText(path, toJson);
		}

		//TODO: Creature Generator!!1!11!!!!111
		private static Creature GenerateCreature(){ throw new System.NotImplementedException(); }
	}
}