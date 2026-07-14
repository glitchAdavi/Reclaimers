using UnityEngine;
using UnityEngine.UI;

public class UIFillableBar : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] float _size = 1;
    [SerializeField] float _currentSize = 1;
    [SerializeField] bool _invert = false;

    private void Awake()
    {
        Material materialInstance = Instantiate(_image.material);
        _image.material = materialInstance;


        _image.material.SetFloat("_Invert", _invert ? 1 : 0);
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
    }

    public void SetColor(Color c)
    {
        _image.material.SetColor("_Color", c);
    }
}
