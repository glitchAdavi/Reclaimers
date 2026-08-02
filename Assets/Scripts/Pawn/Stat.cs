using System;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private float _base = 0f;
    public float Base { get { return _base; } }

    [SerializeField] private float _mult = 1f;
    public float Mult { get { return _mult; } }

    [SerializeField] private float _addTotal = 0f;
    public float AddTotal { get { return _addTotal; } }

    [SerializeField] private float _multTotal = 1f;
    public float MultTotal { get { return _multTotal; } }

    [SerializeField] private float _min;
    public float Min { get { return _min; } }

    [SerializeField] private float _max;
    public float Max { get { return _max; } }

    public Stat(float value = 0f, float mult = 1f, float min = -Single.PositiveInfinity, float max = Single.PositiveInfinity)
    {
        _base = value;
        _mult = mult;

        if (min > max)
        {
            _min = min;
            _max = min;
        } else
        {
            _min = min;
            _max = max;
        }
    }
    public Stat(Stat stat)
    {
        _base = stat.Base;
        _mult = stat.Mult;
        _addTotal = stat.AddTotal;
        _multTotal = stat.MultTotal;
        _min = stat.Min;
        _max = stat.Max;
    }

    public void SetB(float val)
    {
        _base = val;
    }

    public void SetMult(float val)
    {
        _mult = val;
    }

    public void SetAddTotal(float val)
    {
        _addTotal = val;
    }

    public void SetMultTotal(float val)
    {
        _multTotal = val;
    }

    public void SetMin(float val)
    {
        if (val > _max) _min = _max;
        else _min = val;
    }

    public void SetMax(float val)
    {
        if (val < _min) _max = _min;
        else _max = val;
    }

    public void ModB(float val)
    {
        _base += val;
    }

    public void ModMult(float val)
    {
        _mult += val;
    }

    public void ModAddTotal(float val)
    {
        _addTotal += val;
    }

    public void ModMultTotal(float val)
    {
        _multTotal += val;
    }

    public void ModMin(float val)
    {
        if (_min + val > _max) _min = _max;
        else _min += val;
    }

    public void ModMax(float val)
    {
        if (_max + val < _min) _max = _min;
        else _max += val;
    }

    public void Set(float val, float mult)
    {
        _base = val;
        _mult = mult;
    }

    public void Modify(float val, float mult)
    {
        _base += val;
        _mult += mult;
    }

    public float Value()
    {
        return Mathf.Clamp((_base * _mult) * _multTotal + _addTotal, _min, _max);
    }

    public int ValueInt()
    {
        return Mathf.FloorToInt(Value());
    }

    public string ValueString(bool asInt = false, int info = 0)
    {
        string s = "";

        switch (info)
        {
            case 0:
                s = $"{Value().ToString("0.##")}";
                break;
            case 1:
                s = $"{Value().ToString("0.##")} = ({_base.ToString("0.##")} * {_mult.ToString("0.##")}) * {_multTotal.ToString("0.##")} + {_addTotal.ToString("0.##")}";
                break;
            case 2:
                s = $"{Value().ToString("0.##")} = ({_base.ToString("0.##")} * {_mult.ToString("0.##")}) * {_multTotal.ToString("0.##")} + {_addTotal.ToString("0.##")} (Min: {_min.ToString("0.##")}|Max: {_max.ToString("0.##")})";
                break;
            default:
                s = $"{Value().ToString("0.##")}";
                break;
        }

        return s;
    }

    public bool IsEmpty()
    {
        if (_base != 0f || _mult != 0f) return false;
        return true;
    }

    public static Stat operator -(Stat s)
    {
        Stat neg = new Stat(-s.Base, -s.Mult, s.Min, s.Max);
        neg.SetAddTotal(-neg.AddTotal);
        neg.SetMultTotal(-neg.MultTotal);
        return neg;
    }
}
