using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameController : MonoBehaviour
{
    [SerializeField] private Sprite[] bgSkins;
    [SerializeField] private List<Sprite> allCardIcons;

    [Space]
    [SerializeField] private GameObject shuffleParent;
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
    [SerializeField] private Transform cardsContent12;
    [SerializeField] private Transform cardsContent15;
    [SerializeField] private Transform cardsContent18;

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
    [SerializeField] private AudioClip audioStart;
    [SerializeField] private AudioClip audioShuffle;

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
            case GameDificulty.Begginer: shuffleTime = 15; break;
            case GameDificulty.Advanced: shuffleTime = 10; break;
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
        shuffleParent.SetActive(gameManager.WithShuffle);

        audioSource.clip = audioStart;
        audioSource.Play();

        losePanel.SetActive(false);
        winPanel.SetActive(false);

        timeTimer.text = $"TIME: {timer}";
        shuffleTimer.text = $"SHUFFLE IN: {ShuffleTime()}";

        Hearths = 3;

        allCards = new List<UIGameCard>();

        StartCoroutine(EStartGame());
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
        Transform cardsContent = null;

        switch (gameManager.Dificulty)
        {
            case GameDificulty.Begginer: cardsCount = 12; cardsContent = cardsContent12; break;
            case GameDificulty.Advanced: cardsCount = 15; cardsContent = cardsContent15; break;
            case GameDificulty.Pro: cardsCount = 18; cardsContent = cardsContent18; break;
        }

        for (int i = 0; i < cardsCount; i++)
        {
            UIGameCard newCard = Instantiate(cardPrefab, cardsContent);
            newCard.SetBG(bgSkins[PlayerPrefs.GetInt("equip_skin", 0)]);
            Sprite cardFront = allCardIcons[Random.Range(0, allCardIcons.Count)];
            newCard.SetFront(cardFront);
            allCardIcons.Remove(cardFront);
            allCards.Add(newCard);
            newCard.ChangeSprite(false, false);

            newCard.onCardClick += OnCardClick;

            //yield return new WaitForSeconds(0);
        }

        shuffleTimer.text = $"SHUFFLE IN: {0}";
        yield return new WaitForSeconds(1);
        cardsContent.GetComponent<GridLayoutGroup>().enabled = false;

        for (int i = 10; i >= 0; i--)
        {
            timeTimer.text = $"TIME: {i}";
            yield return new WaitForSeconds(1);
        }

        foreach (UIGameCard card in allCards)
        {
            card.ChangeSprite(true, true);

            audioSource.clip = audioShuffle;
            audioSource.Play();

            yield return new WaitForSeconds(cardSpawnDelay);
        }

        searchSprite = allCards[Random.Range(0, allCards.Count)].CardFront;
        searchImg.sprite = searchSprite;
        searchImg.gameObject.SetActive(true);

        rememberObj.SetActive(false);
        if (gameManager.WithShuffle)
        {
            StartCoroutine(EShuffle());
        }
        StartCoroutine(EStartTimer());
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
        List<UIGameCard> shuffleCards = allCards.GetRange(0, allCards.Count);
        int removeCount = allCards.Count - 3;

        for (int i = 0; i < removeCount; i++)
        {
            shuffleCards.RemoveAt(Random.Range(0, shuffleCards.Count));
        }

        audioSource.clip = audioStart;
        audioSource.Play();

        foreach (UIGameCard card in shuffleCards)
        {
            if (card.IsWin) continue;

            card.ChangeSprite(true, false);
            cardPositions.Add(card.transform.position);

            audioSource.clip = audioShuffle;
            audioSource.Play();

            yield return new WaitForSeconds(cardSpawnDelay);
        }

        for (int i = 0; i < shuffleCards.Count; i++)
        {
            if (shuffleCards[i].IsWin) continue;

            int posIndex = Random.Range(0, cardPositions.Count);
            shuffleCards[i].transform.DOMove(cardPositions[posIndex], shuffleMoveTime);
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

            StartCoroutine(EDropCard(gameCard));
        }
        else
        {
            gameCard.Win();
            winCount++;
            audioSource.clip = audioCorrect;
            audioSource.Play();

            searchSprite = allCards[Random.Range(0, allCards.Count)].CardFront;
            searchImg.sprite = searchSprite;

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

    private IEnumerator EDropCard(UIGameCard card)
    {
        yield return new WaitForSeconds(10);
        card.ChangeSprite(true, true);
        audioSource.clip = audioIncorrect;
        audioSource.Play();
    }
}
