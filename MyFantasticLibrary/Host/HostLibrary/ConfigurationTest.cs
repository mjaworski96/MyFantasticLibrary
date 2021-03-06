﻿using ConfigurationManager;
using System;
namespace HostLibrary
{
    public class ConfigurationTest : ITest
    {
        public void Test()
        {
            Configuration config = new Configuration();
            config.LoadConfiguration("configuration.myconf");
            Console.WriteLine(config);
            config.SetString("value1", "newValue");
            config.SetString("value2", "crated ");
            config.SaveConfiguration("saved.myconf");
            Console.WriteLine(config.GetString("value1"));
            Console.WriteLine(config.GetString("value2"));
            Console.WriteLine(config.GetString("complex2.radio"));
        }
    }
}
