using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private float baseVal = 0;
    public float Base { get { return baseVal; } }
    [SerializeField] private float addVal = 0;
    public float Add { get { return addVal; } }
    [SerializeField] private float multVal = 0;
    public float Mult { get { return multVal; } }

    public Stat() { }

    public Stat(Stat stat)
    {
        baseVal = stat.Base;
        addVal = stat.Add;
        multVal = stat.Mult;
    }

    public Stat(float baseVal, float addVal, float multVal)
    {
        this.baseVal = baseVal;
        this.addVal = addVal;
        this.multVal = multVal;
    }

    public void SetBase(float baseVal)
    {
        this.baseVal = baseVal;
    }

    public void ModifyBase(float baseVal)
    {
        this.baseVal += baseVal;
    }

    public void SetAdd(float addVal)
    {
        this.addVal = addVal;
    }

    public void ModifyAdd(float addVal)
    {
        this.addVal += addVal;
    }

    public void SetMult(float multVal)
    {
        this.multVal = multVal;
    }

    public void ModifyMult(float multVal)
    {
        this.multVal += multVal;
    }

    public void SetValues(Stat stat)
    {
        SetValues(stat.baseVal, stat.addVal, stat.multVal);
    }

    public void SetValues(float baseVal, float addVal, float multVal)
    {
        this.baseVal = baseVal;
        this.addVal = addVal;
        this.multVal = multVal;
    }

    public void ModifyValues(Stat stat)
    {
        ModifyValues(stat.baseVal, stat.addVal, stat.multVal);
    }

    public void ModifyValues(float baseVal, float addVal, float multVal)
    {
        this.baseVal += baseVal;
        this.addVal += addVal;
        this.multVal += multVal;
    }

    public float Value()
    {
        float val = (baseVal + addVal) * multVal;

        return val;
    }

    public int ValueInt()
    {
        float val = Value();

        return Mathf.RoundToInt(val);
    }

    public static Stat operator -(Stat s)
    {
        return new Stat(-s.baseVal, -s.addVal, -s.multVal);
    }


}
