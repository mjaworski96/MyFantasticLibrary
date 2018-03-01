using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        private List<Field> _fields;
        /// <summary>
        /// Key of Field.
        /// </summary>
        private string _key;
        /// <summary>
        /// Value of Field.
        /// </summary>
        private string _value;
        /// <summary>
        /// If true, children will not have keys.
        /// </summary>
        private bool list;

        /// <summary>
        /// Intializes new instance of Field.
        /// </summary>
        /// <param name="key">Key of Field.</param>
        /// <param name="value">Value of field</param>
        public Field(string key, string value)
        {
            _key = key;
            _value = value;
        }
        /// <summary>
        /// Intializes new instance of Field.
        /// </summary>
        public Field()
        {
            _fields = new List<Field>();
        }
        /// <summary>
        /// Gets Field asigned with key.
        /// If Field does not exists, it will be created.
        /// </summary>
        /// <param name="key">Key of field</param>
        /// <param name="depth">Current search depth (Used when key is complex).</param>
        /// <returns>Field asinged with key</returns>
        internal Field GetField(string key, int depth = 0)
        {
            string[] keys = key.Split('.');
            Field field = _fields.Where(f => f._key == keys[depth]).FirstOrDefault();
            if (field == null)
            {
                field = new Field();
                field._key = keys[depth];
                _fields.Add(field);
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
                return _value;
            }
            set
            {
                _value = value;
            }
        }
        /// <summary>
        /// Key of Field.
        /// </summary>
        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }
        /// <summary>
        /// Children fields.
        /// </summary>
        public List<Field> Fields
        {
            get
            {
                return _fields;
            }
            set
            {
                _fields = value;
            }
        }
        /// <summary>
        /// Creates string with tabs.
        /// </summary>
        /// <param name="count">Count of tabs.</param>
        /// <returns>String that contains tabs.</returns>
        internal string Tabs(int count)
        {
            return new string('\t', count - 1);
        }
        /// <summary>
        /// Saves Field to <see cref="StreamWriter"/>
        /// </summary>
        /// <param name="sw"><see cref="StreamWriter"/> to save Field.</param>
        /// <param name="depth">Depth of currently saved children.</param>
        internal void Save(StreamWriter sw, int depth)
        {
            if (depth == 0)
            {
                foreach (var item in _fields)
                {
                    item.Save(sw, 1);
                }
            }
            else
            {
                char separatorBegin, separatorEnd;
                if (!list)
                {
                    separatorBegin = '{';
                    separatorEnd = '}';
                }
                else
                {
                    separatorBegin = '[';
                    separatorEnd = ']';
                }
                if (_fields != null)
                {
                    if (_key != "")
                        sw.WriteLine(Tabs(depth) + _key + "=" + separatorBegin);
                    else
                        sw.WriteLine(Tabs(depth) + separatorBegin);
                    foreach (var item in _fields)
                    {
                        item.Save(sw, depth + 1);
                    }
                    sw.WriteLine(Tabs(depth) + separatorEnd);
                }
                else
                {
                    if (_key != "")
                        sw.WriteLine(Tabs(depth) + _key + "=" + _value);
                    else
                        sw.WriteLine(Tabs(depth) + _value);
                }
            }


        }
        /// <summary>
        /// Saves Field to file.
        /// </summary>
        /// <param name="path">Path to configuration file.</param>
        internal void Save(string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                Save(sw, 0);
            }
        }
        /// <summary>
        /// Removes white spaces from begining and ending of string.
        /// </summary>
        /// <param name="toCut">String to be cuted.</param>
        /// <returns>String without  begining and ending white spaces.</returns>
        internal static string CutWhiteSpaces(string toCut)
        {
            int forwardSpaces = 0;
            int backwardSpaces = 0;
            int iter = 0;
            while (iter < toCut.Length && (toCut[iter] == ' ' || toCut[iter] == '\t'))
            {
                forwardSpaces++;
                iter++;
            }
            iter = toCut.Length - 1;
            while (iter >= 0 && (toCut[iter] == ' ' || toCut[iter] == '\t'))
            {
                backwardSpaces++;
                iter--;
            }
            int length = toCut.Length - forwardSpaces - backwardSpaces;
            if (length <= 0 || forwardSpaces + length > toCut.Length)
                return toCut;
            return toCut.Substring(forwardSpaces, length);
        }
        /// <summary>
        /// Contect part of array with separator.
        /// </summary>
        /// <param name="cuted">Array to be cuted.</param>
        /// <param name="startIndex">Startin index.</param>
        /// <param name="separator">Separator between parts of array.</param>
        /// <returns>String builded with cuted strings and separators.</returns>
        internal static string ConnectCuttedStrings(string[] cuted, int startIndex, char separator)
        {
            string str = "";

            for (int i = startIndex; i < cuted.Length; i++)
            {
                str += cuted[i];
                if (i != cuted.Length - 1)
                    str += separator;
            }

            return str;
        }
        /// <summary>
        /// Loads field from <see cref="StreamReader"/>
        /// </summary>
        /// <param name="sr"><see cref="StreamReader"/> that contains Field information.</param>
        /// <param name="listItem">If true, element doesn't have to have key.</param>
        internal void Load(StreamReader sr, bool listItem)
        {
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (CutWhiteSpaces(line) == "}")
                    return;
                if (CutWhiteSpaces(line) == "\t")
                    continue;
                string[] keyValue = line.Split('=');
                string key;
                string value;
                if (!listItem && keyValue.Length < 2)
                    continue;
                else if (listItem)
                {
                    key = "";
                    value = CutWhiteSpaces(ConnectCuttedStrings(keyValue, 0, '='));
                }
                else
                {
                    key = CutWhiteSpaces(keyValue[0]);
                    value = CutWhiteSpaces(ConnectCuttedStrings(keyValue, 1, '='));
                }

                Field field;
                if (CheckChar(value[value.Length - 1], '}', ']'))
                    return;

                if (value[0] == '{')
                {
                    field = new Field();
                    field._key = key;
                    field.Load(sr, false);
                }
                else if (value[0] == '[')
                {
                    field = new Field();
                    field._key = key;
                    field.list = true;
                    field.Load(sr, true);
                }
                else
                {
                    field = new Field(key, value);
                }
                _fields.Add(field);
            }
        }
        /// <summary>
        /// Checks if array contains element.
        /// </summary>
        /// <param name="toCheck">Element to be searched in array.</param>
        /// <param name="searched">Array to search.</param>
        /// <returns>True if array contains element, false if not.</returns>
        internal bool CheckChar(char toCheck, params char[] searched)
        {
            if (searched.Contains(toCheck)) return true;
            return false;
        }
        /// <summary>
        /// Parses data in <see cref="StreamReader"/> to configuration format.
        /// </summary>
        /// <param name="sr"><see cref="StreamReader"/> with information about Field.</param>
        /// <returns><see cref="Stream"/> with parsed data.</returns>
        internal Stream Parse(StreamReader sr)
        {
            Stream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            bool beginBracklet = false;
            bool endBracklet = false;
            bool endLine = false;
            string line = "";
            while (!sr.EndOfStream)
            {
                line += sr.ReadLine() + '\n';
            }
            foreach (char item in line)
            {
                if ((beginBracklet || endBracklet) && !CheckChar(item, '\n'))
                    writer.Write('\n');
                if (CheckChar(item, '}', ']') && !endLine)
                    writer.Write('\n');

                beginBracklet = CheckChar(item, '{', '[');
                endBracklet = CheckChar(item, '}', ']');
                if (!(endLine && CheckChar(item, '\n')))
                    writer.Write(item);

                endLine = CheckChar(item, '\n');
            }

            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        /// <summary>
        /// Loads Field from file.
        /// </summary>
        /// <param name="path">Path to file.</param>
        internal void Load(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                //StreamReader r = new StreamReader(Parse(sr));
                //StreamWriter w = new StreamWriter("parsed.myconf");
                //while (!r.EndOfStream) w.WriteLine(r.ReadLine());
                //w.Flush();
                Load(new StreamReader(Parse(sr)), false);
            }
        }
    }
}
