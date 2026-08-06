using UnityEngine;
using UnityEngine.UI;

public class UIFillableBar : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] float _size = 1;
    [SerializeField] float _currentSize = 1;
    [SerializeField] bool _invert = false;
    [SerializeField] bool _altColor = false;

    private void Awake()
    {
        Material materialInstance = Instantiate(_image.material);
        _image.material = materialInstance;

        _image.material.SetColor("_AltColor", Color.red);

        _image.material.SetFloat("_Invert", _invert ? 1 : 0);
        _image.material.SetFloat("_Bars", _size);
        _image.material.SetFloat("_Fill", _currentSize);
    }

    public void Init(int size)
    {
        _image.material.SetFloat("_Bars", size);
        _image.material.SetFloat("_Fill", 1f);
    }

    public void UpdateBar(float currentSize, float size)
    {
        if (size != _size)
        {
            _image.material.SetFloat("_Bars", size);
            _size = size;
        }
        _currentSize = currentSize;
        _image.material.SetFloat("_Fill", (1f / _size) * _currentSize);
        _image.material.SetFloat("_UseAltColor", (_altColor ? 1f : 0f));
        _image.material.SetColor("_AltColor", Color.red);
    }

    public void SetColor(Color newColor)
    {
        _image.material.SetColor("_Color", newColor);
    }

    public void UseAltColor(bool altColor)
    {
        _altColor = altColor;
    }

    public void Invert(bool i)
    {
        _invert = i;
        _image.material.SetFloat("_Invert", _invert ? 1 : 0);
    }
}
