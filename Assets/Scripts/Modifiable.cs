
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Modifiable<T>
{
    [SerializeField] protected T baseValue;
    [NonSerialized] protected List<T> additive = new();
    [NonSerialized] protected List<T> multiplicitive = new();

    public Modifiable(T baseValue) => this.baseValue = baseValue;
    public void Add(T mod)
    {
        // Lazy load due to reflection BS 
        if (additive == null) additive = new();
        additive.Add(mod);
    }
    public void Mul(T mod)
    {
        // Lazy load due to reflection BS 
        if (multiplicitive == null) additive = new();
        multiplicitive.Add(mod);
    }
    public abstract T Modified();
    public T Base() => baseValue;
}

[Serializable]
public class ModifiableInt : Modifiable<int>
{
    public static implicit operator ModifiableInt(int val) => new ModifiableInt(val);
    public ModifiableInt(int baseValue) : base(baseValue) { }
    public override int Modified()
    {
        // Lazy load due to reflection BS 
        if (multiplicitive == null) multiplicitive = new();
        if (additive == null) additive = new();

        int result = baseValue;

        foreach (var add in additive)
            result += add;

        int mulSum = 1;
        foreach (var mul in multiplicitive)
            mulSum += mul;

        result *= mulSum;

        return result;
    }
}

[Serializable]
public class ModifiableFloat : Modifiable<float>
{
    public static implicit operator ModifiableFloat(float val) => new ModifiableFloat(val);
    public ModifiableFloat(float baseValue) : base(baseValue) { }
    public override float Modified()
    {
        // Lazy load due to reflection BS 
        if (multiplicitive == null) multiplicitive = new();
        if (additive == null) additive = new();

        float result = baseValue;

        foreach (var add in additive)
            result += add;

        float mulSum = 1;
        foreach (var mul in multiplicitive)
            mulSum += mul;

        result *= mulSum;

        return result;
    }
}