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
        /// <summary>
        /// Children fields.
        /// </summary>
        private List<Field> _Fields;
        /// <summary>
        /// Key of Field.
        /// </summary>
        private string _Key;
        /// <summary>
        /// Value of Field.
        /// </summary>
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
            _Fields = new List<Field>();
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
            Field field = _Fields.Where(f => f._Key == keys[depth]).FirstOrDefault();
            if (field == null)
            {
                field = new Field();
                field._Key = keys[depth];
                _Fields.Add(field);
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
        public List<Field> Fields
        {
            get
            {
                return _Fields;
            }
            set
            {
                _Fields = value;
            }
        }
        /// <summary>
        /// Creates string with tabs.
        /// </summary>
        /// <param name="count">Count of tabs.</param>
        /// <returns>String that contains tabs.</returns>
        private static string Tabs(int count)
        {
            return new string('\t', count - 1);
        }
        /// <summary>
        /// Saves configuration to xml file.
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <returns>Xml element of current field</returns>
        private XElement Save(XDocument xmlDocument)
        {
            if(Value != null)
            {
                return new XElement(Key ?? "config", Value);
            }
            else
            {
                return new XElement(Key ?? "config", Fields.Select(x => x.Save(xmlDocument)));
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

        /// <summary>
        /// Loads field from xml element.
        /// </summary>
        /// <param name="xmlElement">Xml element to load field.</param>
        private void Load(XElement xmlElement)
        {
            Key = xmlElement.Name.ToString();
            if (xmlElement.HasElements)
            {
                _Fields = new List<Field>();
                foreach (var child in xmlElement.Elements())
                {
                    Field childField = new Field();
                    childField.Load(child);
                    _Fields.Add(childField);
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
