using ComponentsLoader;
using StringManipulator;
using System;

namespace HostLibrary
{
    class ComponentsTest : ITest
    {
        public void Test()
        {
            LoadedComponent<IStringManipulator> reversing =
                Loader.GetComponentByName<IStringManipulator>("Reversing String Manipulator");
            Console.WriteLine(reversing.AssemblyName);
            foreach (var item in reversing.ReferencedAssembliesNames)
            {
                Console.WriteLine(item.Name);
            }
        }
    }
}
