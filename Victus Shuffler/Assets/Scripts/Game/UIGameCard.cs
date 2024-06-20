using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameCard : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image cardImage;

    [Space]
    [SerializeField] private Sprite cardBG;
    [SerializeField] private Sprite cardFront;

    private bool isRotating;
    private bool isWin;

    public Action<UIGameCard> onCardClick;

    public Sprite CardFront { get => cardFront; }
    public bool IsWin { get => isWin; }

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            onCardClick?.Invoke(this);
        });
    }

    public void SetBG(Sprite sprite)
    {
        cardBG = sprite;    
    }

    public void SetFront(Sprite sprite)
    {
        cardFront = sprite;
    }

    public void ChangeSprite(bool isBg, bool showRotation)
    {
        if (isWin) return;

        if (showRotation && !isRotating)
        {
           StartCoroutine(ERotateCard(isBg));
        }
        else
        {
            if (isBg)
            {
                cardImage.sprite = cardBG;
            }
            else
            {
                cardImage.sprite = cardFront;
            }

            button.interactable = isBg;
        }
    }

    private IEnumerator ERotateCard(bool isBg)
    {
        isRotating = true;

        transform.DORotate(new Vector3(0, -90, 0), 0.25f);

        yield return new WaitForSeconds(0.5f);

        if (isBg)
        {
            cardImage.sprite = cardBG;
        }
        else
        {
            cardImage.sprite = cardFront;
        }

        transform.DORotate(new Vector3(0, 0, 0), 0.25f);

        button.interactable = isBg;
        isRotating = false;

        if (isWin)
        {
            button.interactable = false;
        }
    }

    public void Win()
    {
        isWin = true;
        button.interactable = false;
    }
}
