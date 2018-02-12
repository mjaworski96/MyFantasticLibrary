using ComponentsLoader;
using StringManipulator;
using System;

namespace Host
{
    class ComponentsTest : ITest
    {
        public void Test()
        {
            Loader loader = new Loader();
            LoadedComponent<IStringManipulator> reversing =
                loader.GetComponentByName<IStringManipulator>("Reversing String Manipulator");
            Console.WriteLine(reversing.AssemblyName);
            foreach (var item in reversing.ReferencesAssembliesNames)
            {
                Console.WriteLine(item.Name);
            }
        }
    }
}
