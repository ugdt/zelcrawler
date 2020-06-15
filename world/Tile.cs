using System;
using Godot;
using Godot.Collections;
using Object = Godot.Object;

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

        public void AddProperty(String propertyName, Object value)
        {
            properties.Add(propertyName, value);
        }

        public object GetProperty(String propertyName)
        {
            return properties[propertyName];
        }
    }
}