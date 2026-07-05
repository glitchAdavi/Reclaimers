using Reclaimers.DataPersistence;
using Reclaimers.DeveloperConsole;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    public static void CopyValues<T>(T Base, T Copy, Type customAttribute = null)
    {
        foreach (FieldInfo field in typeof(T).GetFields())
        {
            if (customAttribute != null)
            {
                if (!field.IsDefined(customAttribute, true)) continue;
            }

            field.SetValue(Copy, field.GetValue(Base));
        }
    }
    public static void CopyValues<T1, T2>(T1 Base, T1 Copy, Type customAttribute = null, System.Func<T2, bool> filter = null)
    {
        foreach (FieldInfo field in typeof(T1).GetFields())
        {
            if (typeof(T2) != null)
            {
                if (field.FieldType != typeof(T2)) continue;
            }

            if (customAttribute != null)
            {
                if (!field.IsDefined(customAttribute, true)) continue;
            }

            field.SetValue(Copy, field.GetValue(Base));
        }
    }
}
