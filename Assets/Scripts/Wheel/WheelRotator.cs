using UnityEngine;
using System.Collections;
using Zenject;
using System;
using Random = UnityEngine.Random;

public class WheelRotator : MonoBehaviour
{
    [SerializeField] private RectTransform _wheelTransform;
    [SerializeField] private RectTransform _shadowTransform;
    [SerializeField] private float _rotationSpeed = 360f;
    [SerializeField] private int _numberOfPrizes = 9;
    [SerializeField] private float _decelerationRate = 100f;

    public event Action<int> GotPrize;
    private float _anglePerPrize;
    private SpinController _spinController;

    [SerializeField] private float totalRotation;

    [Inject]
    public void Construct(SpinController spinController)
    {
        _spinController = spinController;
    }

    private void OnEnable()
    {
        _spinController.OnSpin += SpinWheel;
    }

    private void OnDisable()
    {
        _spinController.OnSpin -= SpinWheel;
    }

    void Start()
    {
        _anglePerPrize = 360f / _numberOfPrizes;
    }

    private void SpinWheel()
    {
        float randomAngle = Random.Range(1, 360);
        totalRotation = 360 * Random.Range(1, 5) + randomAngle;
        StartCoroutine(RotateWheel());
    }

    IEnumerator RotateWheel()
    {
        AudioManager.Instance.PlaySpinning();

        float currentRotation = 0f;
        float currentSpeed = _rotationSpeed;
        float minSpeed = _rotationSpeed / 10;
        float decelerationStartPoint = totalRotation - 360; 

        
        AnimationCurve decelerationCurve = AnimationCurve.EaseInOut(decelerationStartPoint, currentSpeed, totalRotation, minSpeed);

        while (currentRotation < totalRotation)
        {
            if (currentRotation >= decelerationStartPoint)
            {
                currentSpeed = decelerationCurve.Evaluate(currentRotation);
            }

            float rotationThisFrame = currentSpeed * Time.deltaTime;
            _wheelTransform.Rotate(0, 0, -rotationThisFrame);
            _shadowTransform.Rotate(0, 0, -rotationThisFrame);
            currentRotation += rotationThisFrame;

            if (currentRotation >= totalRotation)
            {
                _wheelTransform.Rotate(0, 0, -(currentRotation - totalRotation)); 
                break;
            }

            yield return null;
        }

        int prizeIndex = Mathf.FloorToInt((360 - _wheelTransform.eulerAngles.z) % 360 / _anglePerPrize);
        AudioManager.Instance.StopSpinning();
        GotPrize.Invoke(prizeIndex);
        Debug.Log("Prize: " + prizeIndex);
    }
}
