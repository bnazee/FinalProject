using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ColorCycler : MonoBehaviour
{
    [SerializeField] private float _alphaValue = 1f; // Альфа должна быть от 0 до 1
    [SerializeField] private float _changeValue = 0.5f; // Скорость изменения цвета
    private Image _image;
    private Color[] _colors;
    private int _currentColorIndex = 0;

    private ClickHandler _clickHandler;

    [Inject]
    private void Construct(ClickHandler clickHandler)
    {
        _clickHandler = clickHandler;
    }

    private void OnEnable()
    {
        _clickHandler.OnClick += ChangeColor;
    }

    private void OnDisable()
    {
        _clickHandler.OnClick -= ChangeColor;
    }

    private void Start()
    {
        _image = GetComponent<Image>();
        _colors = new Color[]{
            new Color(1f, 1f, 0f, _alphaValue),   // yellow
            new Color(1f, 0f, 0f, _alphaValue),   // red
            new Color(1f, 0f, 1f, _alphaValue),   // violet
            new Color(0f, 0f, 1f, _alphaValue),   // dark blue
            new Color(0f, 1f, 1f, _alphaValue),   // blue
            new Color(0f, 1f, 0f, _alphaValue)    // green
        };
    }

    private void ChangeColor()
    {
        Color targetColor = _colors[_currentColorIndex];
        Color currentColor = _image.color;

        // Изменяем текущий цвет на фиксированное значение _changeValue
        currentColor.r = Mathf.MoveTowards(currentColor.r, targetColor.r, _changeValue);
        currentColor.g = Mathf.MoveTowards(currentColor.g, targetColor.g, _changeValue);
        currentColor.b = Mathf.MoveTowards(currentColor.b, targetColor.b, _changeValue);
        currentColor.a = _alphaValue;

        _image.color = currentColor;

        // Проверяем, достиг ли текущий цвет целевого значения
        if (Mathf.Approximately(currentColor.r, targetColor.r) &&
            Mathf.Approximately(currentColor.g, targetColor.g) &&
            Mathf.Approximately(currentColor.b, targetColor.b))
        {
            // Переходим к следующему цвету в массиве
            _currentColorIndex = (_currentColorIndex + 1) % _colors.Length;
        }
    }
}
