using Reclaimers.DataPersistence;
using Reclaimers.DeveloperConsole;
using System;
using System.Reflection;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    public static void CopyValues<T>(T From, T To, Type customAttribute = null)
    {
        foreach (FieldInfo field in typeof(T).GetFields())
        {
            if (customAttribute != null)
            {
                if (!field.IsDefined(customAttribute, true)) continue;
            }

            Type fieldType = field.FieldType;

            ConstructorInfo copyConstructor = fieldType.GetConstructor(new[] { fieldType });
            if (copyConstructor != null)
            {
                object copiedValue = copyConstructor.Invoke(new[] { field.GetValue(From) });
                field.SetValue(To, copiedValue);
            }
            else
            {
                field.SetValue(To, field.GetValue(From));
            }
        }
    }


    public static void CopyValues<T1, T2>(T1 From, T1 To, Type customAttribute = null, System.Func<T2, bool> filter = null)
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

            Type fieldType = field.FieldType;

            ConstructorInfo copyConstructor = fieldType.GetConstructor(new[] { fieldType });
            if (copyConstructor != null)
            {
                object copiedValue = copyConstructor.Invoke(new[] { field.GetValue(From) });
                field.SetValue(To, copiedValue);
            }
            else
            {
                field.SetValue(To, field.GetValue(From));
            }
        }
    }

    public static void CopyValues<T1, T2>(T1 From, T2 To)
    {
        foreach (FieldInfo fieldT1 in typeof(T1).GetFields())
        {
            FieldInfo fieldT2 = typeof(T2).GetField(fieldT1.Name);
            if (fieldT2 != null)
            {
                Type fieldType = fieldT2.FieldType;

                ConstructorInfo copyConstructor = fieldType.GetConstructor(new[] { fieldType });
                if (copyConstructor != null)
                {
                    object copiedValue = copyConstructor.Invoke(new[] { fieldT1.GetValue(From) });
                    fieldT2.SetValue(To, copiedValue);
                }
                else
                {
                    fieldT2.SetValue(To, fieldT1.GetValue(From));
                }
            }
        }
    }
}
