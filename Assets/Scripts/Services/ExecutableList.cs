using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExecutableList<T>
{
    public int Count 
    { 
        get 
        {
            TriggerAdd();
            TriggerRemove();
            return itemHash.Count; 
        } 
    }

    List<T> itemsToAdd = new List<T>();
    List<T> itemsToRemove = new List<T>();

    HashSet<T> itemHash = new HashSet<T>();

    public List<T> items
    {
        get
        {
            TriggerAdd();
            TriggerRemove();
            return itemHash.ToList();
        }
    }

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
                itemHash.Add(item);
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
                itemHash.Remove(item);
            }
            itemsToRemove.Clear();
        }
    }

    public void ExecuteAll()
    {
        TriggerAdd();
        TriggerRemove();

        foreach(T item in itemHash)
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
