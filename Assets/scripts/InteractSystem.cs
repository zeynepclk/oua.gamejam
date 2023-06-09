using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
public class InteractSystem : MonoBehaviour
{

    public bool isInRange;
    public KeyCode interactKey;
    public UnityEvent interactAction;
    GameObject player;
    [SerializeField] public TextMeshProUGUI _text;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        tagControl(isInRange);
    }
    void tagControl(bool isInRange)
    {
        if (isInRange && Input.GetKeyDown(interactKey))
            interactAction.Invoke();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            isInRange = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = false;
            _text.text = " ";
        }

    }
    public void ingmodule()
    {
        _text.text = "Tebrikler ! \r\nİngilizce modülünü\n tamamladınız";
    }
    public void coursera()
    {
        _text.text = "Tebrikler ! \r\nCoursera eğitimlerini\n tamamladınız";
    }
    public void tech()
    {
        _text.text = "Tebrikler ! \r\nTeknoloji Girişimciliği\n eğitimlerini tamamladınız";
    }
    public void unity()
    {
        _text.text = "Tebrikler ! \r\nUnity eğitimlerini\n tamamladınız";
    }
}
