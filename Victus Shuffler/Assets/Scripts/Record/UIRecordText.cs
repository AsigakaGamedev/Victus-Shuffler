using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class UIRecordText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recordsText;
    [SerializeField] private GameDificulty dificulty;

    private RecordsManager recordsManager;

    [Inject]
    private void Construct(RecordsManager recordsManager)
    {
        this.recordsManager = recordsManager;
    }

    private void Start()
    {
        recordsText.text = recordsManager.GetRecord(dificulty).ToString();
    }
}
