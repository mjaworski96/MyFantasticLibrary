using LegionContract;
using System;
using System.Reflection;

namespace LegionCore.Architecture
{
    public static class IdManagement
    {
        private static FieldInfo GetFieldInfo(IdentifiedById id)
        {
            FieldInfo fi = id.GetType().BaseType.BaseType.GetField("id", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fi == null)
                throw new NullReferenceException();
            return fi;
        }
        internal static void SetId(IdentifiedById toSet, int id)
        {
            FieldInfo fi = GetFieldInfo(toSet);
            fi.SetValue(toSet, id);
        }
        internal static int GetId(IdentifiedById toGet)
        {
            FieldInfo fi = GetFieldInfo(toGet);
            return (int)fi.GetValue(toGet);
        }
    }
}
