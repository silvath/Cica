using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [CollectionDataContract]
    public class CharacterCollection : List<Character>
    {
        public Character Create(string name, int x, int y, int width, int height)
        {
            Character character = new Character();
            character.Name = name;
            character.X = x;
            character.Y = y;
            character.Width = width;
            character.Height = height;
            this.Add(character);
            return (character);
        }

        public Character GetCharacter(string name) 
        {
            foreach (Character character in this)
                if (character.Name == name)
                    return (character);
            return (null);
        }
    }
}
