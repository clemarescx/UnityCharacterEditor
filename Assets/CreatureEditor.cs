﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assets
{
    //TODO: load race / types from json list
    //TODO: on the side of the editor, create tree view incl reorderableList (lookup in assetDatabase?) of all created creatures
    //TODO: make a character visualiser (sprites)
    //TODO: make a creature generator

    /// <summary>
    ///     Contains all layout code for the editor
    /// </summary>
    public class CreatureEditor : EditorWindow
    {
        private static Creature _cachedCreature;
        private static SymbolsManager _symbolsManager;
        private Vector2 _scrollPos;
        private bool _showInventory = true;


        [MenuItem("Window/Creature Editor 0.0.1")]
        private static void Init()
        {
            _symbolsManager = new SymbolsManager();
            ;
            var window = (CreatureEditor)GetWindow(typeof(CreatureEditor), false, "Creature Editor");
            window.Show();
        }

        /// <summary>
        ///     the "main" loop
        /// </summary>
        private void OnGUI()
        {
            GUILayout.BeginHorizontal(GUILayout.MaxWidth(300)); // Buttons start
            DisplayTopButtons();
            if(_cachedCreature == null)
                return;

            GUILayout.EndHorizontal(); // buttons end

            _scrollPos = GUILayout.BeginScrollView(_scrollPos);

            DisplayAttributes();
            DisplayInventory();

            GUILayout.EndScrollView();
        }

        private static void DisplayAttributes()
        {
            _cachedCreature.Name = EditorGUILayout.TextField("Name", _cachedCreature.Name);
            _cachedCreature.Strength = EditorGUILayout.IntSlider("Strength", _cachedCreature.Strength, 0, 20);
            _cachedCreature.Dexterity = EditorGUILayout.IntSlider("Dexterity", _cachedCreature.Dexterity, 0, 20);
            _cachedCreature.Constitution = EditorGUILayout.IntSlider(
                "Constitution",
                _cachedCreature.Constitution,
                0,
                20);
            _cachedCreature.CreatureRace =
                (CreatureRace)EditorGUILayout.EnumPopup("Race", _cachedCreature.CreatureRace);
            _cachedCreature.Type = (CreatureType)EditorGUILayout.EnumPopup("Type", _cachedCreature.Type);

            _cachedCreature.BlasonRootType = "Animal.Mammal.Fox.0";

            _cachedCreature.BlasonMaxim = "hello";

            EditorGUILayout.LabelField("Description:");
            var descriptionStyle = new GUIStyle(GUI.skin.textArea){ fixedHeight = 50f };
            _cachedCreature.Description = EditorGUILayout.TextArea(_cachedCreature.Description, descriptionStyle);
        }

        private void DisplayInventory()
        {
            GUILayout.Label("Inventory");
            // Gold 
            GUILayout.BeginHorizontal();
            GUILayout.Label("Gold:");
            _cachedCreature.Gold = EditorGUILayout.IntField(_cachedCreature.Gold);
            GUILayout.EndHorizontal();

            // Inventory
            _showInventory = EditorGUILayout.Foldout(_showInventory, "Items");
            if(_showInventory)
            {
                var itemCount = _cachedCreature.Inventory.Count;
                EditorGUI.indentLevel += 1;
                for(var i = itemCount - 1; i >= 0; i--)
                {
                    var item = _cachedCreature.Inventory[ i ];

                    DisplayItem(item);

                    GUILayout.Space(10);
                }

                EditorGUI.indentLevel -= 1;

                if(GUILayout.Button("Add item"))
                    _cachedCreature.Inventory.Add(new Item{ Name = "New Item" });
            }
        }

        private void DisplayItem(Item item)
        {
            item.Name = EditorGUILayout.TextField("Name", item.Name);
            item.Type = (ItemType)EditorGUILayout.EnumPopup("Type", item.Type);

            GUILayout.BeginHorizontal();
            item.Value = EditorGUILayout.IntField("Value", item.Value);
            item.Weight = EditorGUILayout.DoubleField("Weight", item.Weight);

            if(GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(10)))
                _cachedCreature.Inventory.Remove(item);
            GUILayout.EndHorizontal();
        }

        private static void DisplayTopButtons()
        {
            if(GUILayout.Button("New creature"))
                _cachedCreature = new Creature{ Inventory = new List<Item>() };
            GUI.enabled = _cachedCreature != null;
            if(GUILayout.Button("Export"))
                ExportToJson(_cachedCreature);
            GUI.enabled = true;
            if(GUILayout.Button("Import"))
                _cachedCreature = ImportFromJson();
            if(GUILayout.Button("Generate"))
                _cachedCreature = GenerateCreature();
        }


        private static Creature ImportFromJson()
        {
            var importedCreaturePath = EditorUtility.OpenFilePanel("Import creature", "Assets/Creatures", "json");
            var importedCreatureJson = File.ReadAllText(importedCreaturePath);
            var importedCreature = new Creature();
            JsonUtility.FromJsonOverwrite(importedCreatureJson, importedCreature);

            return importedCreature;
        }

        private static void ExportToJson(Creature creature)
        {
            if(creature == null)
            {
                EditorUtility.DisplayDialog("Save creature", "No creature to save.", "OK");
                return;
            }

            var path = EditorUtility.SaveFilePanel("Save creature", "Assets/Creatures", creature.Name, "json");

            if(path.Length == 0)
                return;

            var toJson = JsonUtility.ToJson(creature, true);

            Debug.Log(toJson);
            if(toJson != null)
                File.WriteAllText(path, toJson);
        }

        //TODO: Creature Generator!!1!11!!!!111
        private static Creature GenerateCreature()
        {
            throw new NotImplementedException();
        }
    }
}
