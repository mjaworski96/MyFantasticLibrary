using System;
using System.Collections.Generic;

namespace ConfigurationManager
{
    /// <summary>
    /// Configuration. Contains data in Key-Value format. Can contain list of data without keys.
    /// Cnfiguration file format:
    /// simple=simpleValue
    /// complex = {
    ///     nestedKey = val
    ///     nested = value
    /// }
    /// list = [
    ///     simpleListItem
    ///     complexListItem = {
    ///     nested = 1
    ///     parameter = param
    ///     }
    /// ]
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Configuration. Key-Value format.
        /// </summary>
        Field configuration = new Field();
        /// <summary>
        /// Gets string asigned with key.
        /// </summary>
        /// <param name="key">Key to search value.</param>
        /// <returns>Value asigned to key</returns>
        public string GetString(string key)
        {
            return GetField(key)?.Value ?? "";
        }
        /// <summary>
        /// Gets <see cref="Field"/> asigned with key.
        /// </summary>
        /// <param name="key">Key to search <see cref="Field"/>.</param>
        /// <returns><see cref="Field"/> asigned with key.</returns>
        public Field GetField(string key)
        {
            return configuration.GetField(key);
        }
        /// <summary>
        /// Intializes new instance of Configuration.
        /// </summary>
        /// <param name="path">Path to file with initial configuration.</param>
        public Configuration(string path)
        {
            configuration.Load(path);
        }
        /// <summary>
        /// Initializes new instance of Configuration.
        /// </summary>
        public Configuration()
        {

        }
        /// <summary>
        /// Gets field asigned with key.
        /// </summary>
        /// <param name="key">Key to search value.</param>
        /// <returns>Field asigned to key</returns>
        public List<Field> GetListOfFields(string key)
        {
            return configuration.GetField(key)?.Fields ?? new List<Field>();
        }
        /// <summary>
        /// Sets string asigned with key.
        /// If key does not exists, it will be crated.
        /// </summary>
        /// <param name="key">Key of value.</param>
        /// <param name="value">Value to asign with key.</param>
        public void SetString(string key, string value)
        {
            Field field = GetField(key);
            field.Value = value;
        }
        /// <summary>
        /// Sets list of fields asigned with key.
        /// If key does not exists, it will be crated.
        /// </summary>
        /// <param name="key">Key of value.</param>
        /// <param name="fields">Fielts to asign with key.</param>
        public void SetFields(string key, List<Field> fields)
        {
            Field field = GetField(key);
            field.Fields = fields;
        }
        /// <summary>
        /// Sets <see cref="Field"/> asiged with key.
        /// </summary>
        /// <param name="key">Key of <see cref="Field"/></param>
        /// <param name="field"><see cref="Field"/> to asign with key</param>
        public void SetField(string key, Field field)
        {
            Field f = GetField(key);
            f.Key = field.Key;
            f.Value = field.Value;
            f.Fields = field.Fields;
        }
        /// <summary>
        /// Loads configuration from file.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if path is null or empty.</exception>
        /// <param name="path">Path to configuration file.</param>
        public void LoadConfiguration(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Parameter message can not be empty.", nameof(path));
            }

            configuration = new Field();
            configuration.Load(path);
        }
        /// <summary>
        /// Saves configuration to file.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if path is null or empty.</exception>
        /// <param name="path">Path to configuration file.</param>
        public void SaveConfiguration(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("message", nameof(path));
            }

            configuration?.Save(path);
        }
    }
}
