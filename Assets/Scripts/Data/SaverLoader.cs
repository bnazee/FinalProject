using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class SaverLoader : MonoBehaviour
{
    private Question[] _loadedQuestions = new Question[0];
    public delegate void OnJsonLoaded();
    public OnJsonLoaded onJsonLoadedCallback;

    private static readonly string ScoreKey = "Score";
    private static readonly string SpinCostKey = "SpinCost";
    private static readonly string MultiplierKey = "Multiplier";
    private static readonly string ToppingsKey = "Toppings";
    private static readonly string AnsweredKey = "Answered";
    private static readonly string QuizFileName = "quiz.json";

    public void Start()
    {
        StartCoroutine(LoadJson());
    }
    public void SaveScore(int score)
    {
        PlayerPrefs.SetInt(ScoreKey, score);
        PlayerPrefs.Save();
    }

    public int LoadScore()
    {
        int score = PlayerPrefs.GetInt(ScoreKey, 1);
        return score;
    }

    public void SaveSpinCost(int spinCost)
    {
        PlayerPrefs.SetInt(SpinCostKey, spinCost);
        PlayerPrefs.Save();
    }

    public int LoadSpinCost()
    {
        int spinCost = PlayerPrefs.GetInt(SpinCostKey, 20);
        return spinCost;
    }

    public void SaveMultiplier(int multiplier)
    {
        PlayerPrefs.SetInt(MultiplierKey, multiplier);
        PlayerPrefs.Save();
    }

    public int LoadMultiplier()
    {
        int multiplier = PlayerPrefs.GetInt(MultiplierKey, 1);
        return multiplier;
    }

    public void SaveToppings(List<Topping> toppings)
    {
        Topping[] toppingsArray = toppings.ToArray();
        string json = JsonUtility.ToJson(new Array<Topping>(toppingsArray));

        PlayerPrefs.SetString(ToppingsKey, json);
        PlayerPrefs.Save();
    }

    public List<Topping> LoadToppings()
    {
        string json = PlayerPrefs.GetString(ToppingsKey, "");
        if (string.IsNullOrEmpty(json))
        {
            return new List<Topping>();
        }

        Array<Topping> data = JsonUtility.FromJson<Array<Topping>>(json);
        if (data == null || data.array == null)
        {

            return new List<Topping>();
        }

        List<Topping> toppings = new List<Topping>(data.array);
        return toppings;
    }

    public void SaveAnswered(List<int> answered)
    {
        int[] answeredArray = answered.ToArray();
        string json = JsonUtility.ToJson(new Array<int>(answeredArray));

        PlayerPrefs.SetString(AnsweredKey, json);
        PlayerPrefs.Save();
    }

    public List<int> LoadAnswered()
    {
        string json = PlayerPrefs.GetString(AnsweredKey, "");
        if (string.IsNullOrEmpty(json))
        {
            return new List<int>();
        }

        Array<int> data = JsonUtility.FromJson<Array<int>>(json);
        if (data == null || data.array == null)
        {

            return new List<int>();
        }

        List<int> answered = new List<int>(data.array);
        return answered;
    }

    public IEnumerator LoadJson()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, QuizFileName);
        UnityWebRequest www = UnityWebRequest.Get(filePath);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Не удалось найти файл: " + filePath);
        }
        else
        {
            string jsonContents = www.downloadHandler.text;
            _loadedQuestions = JsonUtility.FromJson<Array<Question>>(jsonContents).array;

            if (_loadedQuestions == null)                        
            {
                Debug.LogError("Ошибка при десериализации JSON.");
            }
            onJsonLoadedCallback?.Invoke();
        }
    }

    public Question[] LoadQuestions()
    {
        return _loadedQuestions;
    } 
}

[System.Serializable]
public class Question
{
    public string question;
    public string answer;
}

[System.Serializable]
public class Topping
{
    public int prizeIndex;
    public Vector2 position;
    public Quaternion rotation;
    public bool isRofl;

    public Topping(int prizeIndex, Vector2 position, Quaternion rotation, bool isRofl)
    {
        this.prizeIndex = prizeIndex;
        this.position = position;
        this.rotation = rotation;
        this.isRofl = isRofl;
    }
}

[System.Serializable]
public class Array<T>
{
    public T[] array;

    public Array(T[] array)
    {
        this.array = array;
    }
}

