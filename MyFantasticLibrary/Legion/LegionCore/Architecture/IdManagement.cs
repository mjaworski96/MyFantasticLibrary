using LegionContract;
using System;
using System.Reflection;

namespace LegionCore.Architecture
{
    /// <summary>
    /// IdentifiedById getter and setter.
    /// </summary>
    public static class IdManagement
    {
        private static FieldInfo GetFieldInfo(IdentifiedById id)
        {
            FieldInfo fi = id.GetType().BaseType.BaseType.GetField("_Id", BindingFlags.NonPublic | BindingFlags.Instance);
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
