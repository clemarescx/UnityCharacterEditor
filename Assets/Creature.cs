using System;
using System.Collections.Generic;

namespace Assets
{
    [Serializable]
    public class Creature
    {
        public string BlasonMaxim;
        public string BlasonRootType;
        public int Constitution;

        public CreatureRace CreatureRace;
        public string Description;
        public int Dexterity;

        public int Gold;

        public List<Item> Inventory;
        public string Name;
        public int Strength;
        public CreatureType Type;
    }
}
