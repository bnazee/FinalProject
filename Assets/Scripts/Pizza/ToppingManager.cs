using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ToppingManager : MonoBehaviour
{
    [SerializeField] private RectTransform[] _positions;
    [SerializeField] private GameObject[] _toppingPrefabs;
    [SerializeField] private GameObject[] _roflToppings;
    [SerializeField] private RectTransform _pizzaPanel;
    [SerializeField] private RectTransform _pizzaImage;
    [SerializeField] private float _dropSpeed;
    [SerializeField] private PrizeData _prizeData;

    private List<Topping> _toppingsOnPizza;
    private SaverLoader _saverLoader;
    private SpinController _spinController;

    [Inject]
    private void Construct(SaverLoader saverLoader, SpinController spinController)
    {
        _saverLoader = saverLoader;
        _spinController = spinController;
    }

    private void Start()
    {
        CreateToppings();    
    }

    public void DropTopping(int prizeIndex)
    {
        StartCoroutine(DropToppingRoutine(prizeIndex));
    }
    private void CreateToppings()
    {
        _toppingsOnPizza = _saverLoader.LoadToppings();
        for (int i = 0; i < _toppingsOnPizza.Count; i++)
        {
            GameObject newTopping;
            Topping topping = _toppingsOnPizza[i];
            if (topping.isRofl)
            {
                newTopping = Instantiate(_roflToppings[topping.prizeIndex], _pizzaImage);
            }
            else
            {
                newTopping = Instantiate(_toppingPrefabs[topping.prizeIndex], _pizzaImage);
            }

            RectTransform toppingRect = newTopping.GetComponent<RectTransform>();
            toppingRect.anchoredPosition = topping.position;
            toppingRect.rotation = topping.rotation;
        }
    }

    private IEnumerator DropToppingRoutine(int index)
    {
        RectTransform topping;
        int randIndex;
        bool isRofl;
        if (index == _prizeData.GetIndex(PrizeID.ROFL))
        {
            randIndex = Random.Range(0, _roflToppings.Length);
            topping = Instantiate(_roflToppings[randIndex], _positions[index]).GetComponent<RectTransform>();
            index = randIndex;
            isRofl = true;
        }
        else
        {
            topping = Instantiate(_toppingPrefabs[index], _positions[index]).GetComponent<RectTransform>();
            isRofl = false;
        }

        float minY = _pizzaPanel.rect.yMin + _pizzaPanel.position.y;
        float maxY = _pizzaPanel.rect.yMax + _pizzaPanel.position.y;
        float randomY = Random.Range(minY, maxY);

        while (topping.position.y > randomY)
        {
            topping.position += Vector3.down * _dropSpeed * Time.deltaTime;
            yield return null;
        }

        topping.SetParent(_pizzaImage, true);
        topping.position = new Vector3(topping.position.x, randomY, 0);
        AudioManager.Instance.PlayRandomDrop();
        _toppingsOnPizza.Add(new Topping(index, topping.anchoredPosition, topping.rotation, isRofl));  
        _saverLoader.SaveToppings(_toppingsOnPizza);
        _spinController.IsPlaying = false;
    }
}
