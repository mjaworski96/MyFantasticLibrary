using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ConfigurationManager
{
    /// <summary>
    /// Part of configuration. Data in Key-Value format.
    /// Can contains nested Fields.
    /// </summary>
    public class Field
    {
        private List<Field> _Childrens;
        private string _Key;
        private string _Value;
        /// <summary>
        /// Intializes new instance of Field.
        /// </summary>
        /// <param name="key">Key of Field.</param>
        /// <param name="value">Value of field</param>
        public Field(string key, string value)
        {
            _Key = key;
            _Value = value;
        }
        /// <summary>
        /// Intializes new instance of Field.
        /// </summary>
        public Field()
        {
            _Childrens = new List<Field>();
        }
        /// <summary>
        /// Gets Field asigned with key.
        /// If Field does not exists, it will be created.
        /// </summary>
        /// <param name="key">Key of field</param>
        /// <param name="depth">Current search depth (Used when key is complex).</param>
        /// <returns>Field asinged with key.</returns>
        public Field GetField(string key, int depth = 0)
        {
            string[] keys = key.Split('.');
            Field field = _Childrens.Where(f => f._Key == keys[depth]).FirstOrDefault();
            if (field == null)
            {
                field = new Field();
                field._Key = keys[depth];
                _Childrens.Add(field);
            }

            if (depth == keys.Length - 1)
                return field;
            else
                return field.GetField(key, depth + 1);
        }
        /// <summary>
        /// Value of Field.
        /// </summary>
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }
        /// <summary>
        /// Key of Field.
        /// </summary>
        public string Key
        {
            get
            {
                return _Key;
            }
            set
            {
                _Key = value;
            }
        }
        /// <summary>
        /// Children fields.
        /// </summary>
        public List<Field> Childrens
        {
            get
            {
                return _Childrens;
            }
            set
            {
                _Childrens = value;
            }
        }

        private XElement Save(XDocument xmlDocument)
        {
            if(Value != null)
            {
                return new XElement(Key ?? "config", Value);
            }
            else
            {
                return new XElement(Key ?? "config", Childrens.Select(x => x.Save(xmlDocument)));
            }
        }
        /// <summary>
        /// Saves Field to xml file.
        /// </summary>
        /// <param name="path">Path to xml configuration file.</param>
        public void Save(string path)
        {
            XDocument xmlDocument = new XDocument();
            xmlDocument.Add(Save(xmlDocument));
            xmlDocument.Save(path);
        }

        private void Load(XElement xmlElement)
        {
            Key = xmlElement.Name.ToString();
            if (xmlElement.HasElements)
            {
                _Childrens = new List<Field>();
                foreach (var child in xmlElement.Elements())
                {
                    Field childField = new Field();
                    childField.Load(child);
                    _Childrens.Add(childField);
                }
            }
            else
            {
                Value = xmlElement.Value;
            }


        }

        /// <summary>
        /// Loads Field from xml file.
        /// </summary>
        /// <param name="path">Path to xml file.</param>
        public void Load(string path)
        {
            XDocument xmlDocument = XDocument.Load(path);
            Load(xmlDocument.Root);
        }
    }
}
