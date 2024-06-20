using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameController : MonoBehaviour
{
    [SerializeField] private Sprite defaultCardIcon;
    [SerializeField] private Sprite[] allCardIcons;

    [Space]
    [SerializeField] private TextMeshProUGUI shuffleTimer;
    [SerializeField] private TextMeshProUGUI timeTimer;
    [SerializeField] private GameObject rememberObj;

    private int timer;

    [Space]
    [SerializeField] private GameObject hearthPrefab;
    [SerializeField] private Transform hearthsContainer;

    [Space]
    [SerializeField] private float cardSpawnDelay = 0.2f;
    [SerializeField] private UIGameCard cardPrefab;
    [SerializeField] private Transform cardsContent;
    [SerializeField] private GridLayoutGroup grid;

    [Header("Shuffle")]
    [SerializeField] private float shuffleMoveTime = 1f;
    [SerializeField] private float shuffleDelay = 0.2f;

    [Space]
    [SerializeField] private Image searchImg;
    [SerializeField] private Sprite searchSprite;

    [Header("Lose")]
    [SerializeField] private GameObject losePanel;
    [SerializeField] private TextMeshProUGUI loseTimerText;

    [Header("Win")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI winTimerText;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioCorrect;
    [SerializeField] private AudioClip audioIncorrect;
    [SerializeField] private AudioClip audioWin;
    [SerializeField] private AudioClip audioLose;

    private GameManager gameManager;
    private RecordsManager recordsManager;

    private int hearths;

    private List<UIGameCard> allCards;

    private int winCount;

    public float ShuffleTime()
    {
        float shuffleTime = 0;

        switch (gameManager.Dificulty)
        {
            case GameDificulty.Begginer: shuffleTime = 12; break;
            case GameDificulty.Advanced: shuffleTime = 8; break;
            case GameDificulty.Pro: shuffleTime = 5; break;
        }

        return shuffleTime;
    }

    public int Hearths { get => hearths;
        set
        {
            foreach (Transform child in hearthsContainer)
            {
                Destroy(child.gameObject);
            }

            hearths = value;

            for (int i = 0; i < hearths; i++)
            {
                Instantiate(hearthPrefab, hearthsContainer);
            }

            if (hearths <= 0)
            {
                recordsManager.SetRecord(gameManager.Dificulty, timer);

                losePanel.SetActive(true);
                loseTimerText.text = timer.ToString();
                audioSource.clip = audioLose;
                audioSource.Play();
            }
        }
    }

    [Inject]
    private void Construct(GameManager gameManager, RecordsManager recordsManager)
    {
        this.gameManager = gameManager; 
        this.recordsManager = recordsManager;
    }

    private void Start()
    {
        losePanel.SetActive(false);
        winPanel.SetActive(false);

        timeTimer.text = $"TIME: {timer}";
        shuffleTimer.text = $"SHUFFLE IN: {ShuffleTime()}";

        Hearths = 3;

        allCards = new List<UIGameCard>();

        StartCoroutine(EStartGame());
        StartCoroutine(EStartTimer());
    }

    private void OnDestroy()
    {
        foreach (UIGameCard card in allCards)
        {
            card.onCardClick -= OnCardClick;
        }
    }

    private IEnumerator EStartGame()
    {
        rememberObj.SetActive(true);
        searchImg.gameObject.SetActive(false);

        int cardsCount = 0;

        switch (gameManager.Dificulty)
        {
            case GameDificulty.Begginer: cardsCount = 12; break;
            case GameDificulty.Advanced: cardsCount = 15; break;
            case GameDificulty.Pro: cardsCount = 18; break;
        }

        for (int i = 0; i < cardsCount; i++)
        {
            UIGameCard newCard = Instantiate(cardPrefab, cardsContent);
            newCard.SetBG(defaultCardIcon);
            newCard.SetFront(allCardIcons[Random.Range(0, allCardIcons.Length)]);
            allCards.Add(newCard);
            newCard.ChangeSprite(false, false);

            newCard.onCardClick += OnCardClick;

            yield return new WaitForSeconds(cardSpawnDelay);
        }

        grid.enabled = false;

        for (int i = (int)ShuffleTime(); i >= 0; i--)
        {
            shuffleTimer.text = $"SHUFFLE IN: {i}";
            yield return new WaitForSeconds(1);
        }

        foreach (UIGameCard card in allCards)
        {
            card.ChangeSprite(true, true);
        }

        searchSprite = allCards[Random.Range(0, allCards.Count)].CardFront;
        searchImg.sprite = searchSprite;
        searchImg.gameObject.SetActive(true);

        rememberObj.SetActive(false);
        StartCoroutine(EShuffle());
    }

    private IEnumerator EStartTimer()
    {
        timer = 0;

        while (true)
        {
            timer++;
            timeTimer.text = $"TIME: {timer}";
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator EShuffle()
    {
        for (int i = (int)ShuffleTime(); i >= 0; i--)
        {
            shuffleTimer.text = $"SHUFFLE IN: {i}";
            yield return new WaitForSeconds(1);
        }

        List<Vector3> cardPositions = new List<Vector3>();

        foreach (UIGameCard card in allCards)
        {
            if (card.IsWin) continue;

            card.ChangeSprite(true, false);
            cardPositions.Add(card.transform.position);
        }

        for (int i = 0; i < allCards.Count; i++)
        {
            if (allCards[i].IsWin) continue;

            int posIndex = Random.Range(0, cardPositions.Count);
            allCards[i].transform.DOMove(cardPositions[posIndex], shuffleMoveTime);
            cardPositions.RemoveAt(posIndex);

            yield return new WaitForSeconds(shuffleDelay); 
        }

        StartCoroutine(EShuffle());
    }

    private void OnCardClick(UIGameCard gameCard)
    {
        gameCard.ChangeSprite(false, true);

        if (gameCard.CardFront != searchSprite)
        {
            Hearths--;
            audioSource.clip = audioIncorrect;
            audioSource.Play();
        }
        else
        {
            gameCard.Win();
            winCount++;
            audioSource.clip = audioCorrect;
            audioSource.Play();

            if (winCount >= allCards.Count)
            {
                recordsManager.SetRecord(gameManager.Dificulty, timer);

                winPanel.SetActive(true);
                winTimerText.text = timer.ToString();
                audioSource.clip = audioWin;
                audioSource.Play();
            }
        }
    }
}
