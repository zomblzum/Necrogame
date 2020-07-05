using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Текстовое поле интерфейса для вывода текста взаимодействия")]
    public TextMeshProUGUI interactText;
    [Header("Общая часть текста взаимодействия")]
    public string interactTextStart = "Press E to";

    private Interactable interactable;
    private string interactButton = "Use";

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Interactable>() != null)
        {
            interactable = other.GetComponent<Interactable>();

            if(interactable.CanInteract())
            {
                interactText.enabled = true;
                interactText.SetText(interactTextStart + " " + interactable.GetActionText());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Interactable>() != null && other.GetComponent<Interactable>() == interactable)
        {
            interactable = null;
            interactText.enabled = false;
        }
    }

    private void Start()
    {
        interactText.enabled = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown(interactButton) && interactable != null && interactable.CanInteract())
        {
            interactable.DoInteraction();
        }
    }
}
