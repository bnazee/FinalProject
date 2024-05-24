using UnityEngine;
using Zenject;

public class ClickerInstaller : MonoInstaller
{
    [SerializeField] private LetterInputField _letterInputField;
    [SerializeField] private WordInputField _wordInputField;
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private WheelRotator _wheelRotator;
    [SerializeField] private QuestionsManager _questionsManager;
    [SerializeField] private ClickHandler _clickHandler;
    [SerializeField] private SpinController _spinController;
    [SerializeField] private SaverLoader _saverLoader;
    [SerializeField] private PrizeManager _prizeManager;
    [SerializeField] private ToppingManager _toppingManager;
    [SerializeField] private UIManager _uiManager;

    public override void InstallBindings()
    {
        Container.Bind<LetterInputField>().FromInstance(_letterInputField).AsSingle();
        Container.Bind<WordInputField>().FromInstance(_wordInputField).AsSingle();
        Container.Bind<ScoreManager>().FromInstance(_scoreManager).AsSingle();
        Container.Bind<WheelRotator>().FromInstance(_wheelRotator).AsSingle();
        Container.Bind<QuestionsManager>().FromInstance(_questionsManager).AsSingle();
        Container.Bind<ClickHandler>().FromInstance(_clickHandler).AsSingle();
        Container.Bind<SpinController>().FromInstance(_spinController).AsSingle();
        Container.Bind<SaverLoader>().FromInstance(_saverLoader).AsSingle();
        Container.Bind<PrizeManager>().FromInstance(_prizeManager).AsSingle();
        Container.Bind<ToppingManager>().FromInstance(_toppingManager).AsSingle();
        Container.Bind<UIManager>().FromInstance(_uiManager).AsSingle();
    }
}
