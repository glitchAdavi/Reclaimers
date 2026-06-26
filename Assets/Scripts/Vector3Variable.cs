using UnityEngine;

[CreateAssetMenu(fileName = "Vector3Variable", menuName = "Variables/Vector3Variable")]
public class Vector3Variable : ScriptableObject
{
    public Vector3 Value;

    public void SetValue(float x, float y, float z)
    {
        SetValue(new Vector3(x, y, z));
    }
    public void SetValue(Vector3Variable value)
    {
        SetValue(value.Value);
    }
    public void SetValue(Vector3 value)
    {
        Value.x = value.x;
        Value.y = value.y;
        Value.z = value.z;
    }
}
