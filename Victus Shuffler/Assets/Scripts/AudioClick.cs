using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AudioClick : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    
    private AudioSource source;
    private Button button;

    private void Start()
    {
        source = GameObject.Find("Audio").GetComponent<AudioSource>();
        button = GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            source.clip = clip;
            source.Play();
        });
    }
}
