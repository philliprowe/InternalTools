﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateServersList
{
    public class VersionType
    {
        public VersionType(string name, string version, string compile, string errors)
        {
            _name = name;
            _version = version;
            _compile = compile;
            _errors = errors;
            _itemArray = new string[0];
        }
        public string _name;
        public string _version;
        public string _compile;
        public string _errors;
        private string[] _itemArray;
        public string _items()
        {
            string r = string.Empty;
            foreach (string s in _itemArray) r += s + ", ";
            if (r.Length >= 2) r = r.Substring(0, r.Length - 2);
            return r;
        }
        public void addItem(string item)
        {
            Debug.Print(string.Format("Name : {0} Item : {1} Count : {2} UBound {3}", _name, item, _itemArray.Count().ToString(), _itemArray.GetUpperBound(0).ToString()));
            Array.Resize(ref _itemArray, _itemArray.Count() + 1);
            _itemArray[_itemArray.GetUpperBound(0)] = item;
        }
    }
}
