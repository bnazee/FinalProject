using UnityEngine;
using System.Collections;
using Zenject;

public class PizzaRotator : MonoBehaviour
{
    [SerializeField] private RectTransform _pizzaTransform;
    [SerializeField] private RectTransform _shadowTransform;
    [SerializeField] private float _initialRotationSpeed = 360f;
    [SerializeField] private float _decelerationRate = 25f;

    private float _rotationSpeed;
    private float _angleToRotate;
    private ClickHandler _clickHandler;

    [Inject]
    private void Construct(ClickHandler clickHandler)
    {
        _clickHandler = clickHandler;
    }

    private void OnEnable()
    {
        _clickHandler.OnClick += StartRotation;
    }

    private void OnDisable()
    {
        _clickHandler.OnClick -= StartRotation;
    }

    private void Start()
    {
        _rotationSpeed = _initialRotationSpeed;
    }
    public void StartRotation()
    {
        _angleToRotate = Random.Range(-359f, 0f);
        StartCoroutine(RotatePizza());
    }

    private IEnumerator RotatePizza()
    {
        float remainingAngle = _angleToRotate;

        while (remainingAngle < 0) 
        {
            float rotationThisFrame = Mathf.Min(_rotationSpeed * Time.deltaTime, -remainingAngle); 
            _pizzaTransform.Rotate(0, 0, -rotationThisFrame); 
            _shadowTransform.Rotate(0, 0, -rotationThisFrame); 
            remainingAngle += rotationThisFrame; 

            _rotationSpeed -= _decelerationRate * Time.deltaTime;
            _rotationSpeed = Mathf.Max(_rotationSpeed, 0f);

            yield return null;
        }
        _rotationSpeed = _initialRotationSpeed;
    }
}

