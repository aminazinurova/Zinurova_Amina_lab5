using System;
namespace MuseumApp
{
    internal class Exhibit
    {
        private int _id;
        private string _name;
        private string _era;
        public int Id 
        { 
            get 
            { 
                return _id;
            } 
            set 
            { 
                _id = value;
            }
        }
        public string Name 
        { get 
            { 
                return _name;
            } 
            set 
            { 
                _name = value;
            } 
        }
        public string Era 
        { 
            get
            { 
                return _era; 
            } 
            set 
            { 
                _era = value; 
            }
        }
        public Exhibit(int id, string name, string era)
        {
            _id = id;
            _name = name;
            _era = era;
        }
        public override string ToString()
        {
            return $"ID: {_id} | Название: {_name} | Эпоха: {_era}";
        }
    }
}