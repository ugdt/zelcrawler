using System;
using Godot.Collections;

namespace nextGame.world
{
    public class Tile
    {

        private String name;
        private Dictionary properties;

        public Tile(string name, Dictionary properties)
        {
            this.name = name;
            this.properties = properties;
        }

        public string Name => name;

        public Dictionary Properties => properties;
    }
}