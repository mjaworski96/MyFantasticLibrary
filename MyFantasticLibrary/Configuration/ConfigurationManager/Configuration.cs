using System.Collections.Generic;

namespace ConfigurationManager
{
    /// <summary>
    /// Configuration. Contains data in Key-Value format. Can contain list of data without keys.
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
            return configuration.GetField(key)?.Value ?? "";
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
            Field field = configuration.GetField(key);
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
            Field field = configuration.GetField(key);
            field.Fields = fields;
        }
        /// <summary>
        /// Loads configuration from file.
        /// </summary>
        /// <param name="path">Path to configuration file.</param>
        public void LoadConfiguration(string path)
        {
            configuration = new Field();
            configuration.Load(path);
        }
        /// <summary>
        /// Saves configuration to file.
        /// </summary>
        /// <param name="path">Path to configuration file.</param>
        public void SaveConfigration(string path)
        {
            configuration?.Save(path);
        }
    }
}
