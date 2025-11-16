using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutableList<T>
{
    int Count { get { return items.Count; } }

    List<T> itemsToAdd = new List<T>();
    List<T> itemsToRemove = new List<T>();

    HashSet<T> items = new HashSet<T>();

    public delegate void Execute(T item);
    Execute MethodToExecute;

    public ExecutableList() { }
    public ExecutableList(Execute methodToExecute)
    {
        MethodToExecute = methodToExecute;
    }

    public void TriggerAdd()
    {
        if (itemsToAdd.Count > 0)
        {
            foreach (T item in itemsToAdd)
            {
                items.Add(item);
            }
            itemsToAdd.Clear();
        }
    }

    public void TriggerRemove()
    {
        if (itemsToRemove.Count > 0)
        {
            foreach (T item in itemsToRemove)
            {
                items.Remove(item);
            }
            itemsToRemove.Clear();
        }
    }

    public void ExecuteAll()
    {
        TriggerAdd();

        TriggerRemove();

        foreach(T item in items)
        {
            if (MethodToExecute != null) MethodToExecute(item);    
        }

    }

    public void Add(T item)
    {
        if (itemsToRemove.Contains(item)) itemsToRemove.Remove(item);
        itemsToAdd.Add(item);
    }

    public void Remove(T item)
    {
        if (itemsToAdd.Contains(item)) itemsToAdd.Remove(item);
        itemsToRemove.Add(item);
    }

}
