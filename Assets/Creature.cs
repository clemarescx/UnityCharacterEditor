using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    [Serializable]
    public class Creature
    {
        public string Name;
        public int Strength;
        public int Dexterity;
        public int Constitution;
        public string Description;

        public CreatureRace CreatureRace;
        public CreatureType Type;

        public List<Item> Inventory;

        public int Gold;
        public string BlasonRootType;
        public string BlasonMaxim;
    }
}