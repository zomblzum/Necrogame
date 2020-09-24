using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperMinionInteractable : MonoBehaviour, Interactable
{
    [Header("Текст на экране, при взаимодействии с откупом")]
    public string interactText;
    [Header("Коллайдер для взаимодействия")]
    public Collider interactCollider;

    private BanditGroupController controller;

    private void Start()
    {
        controller = FindObjectOfType<BanditGroupController>();
    }

    public bool CanInteract()
    {
        return !GetComponent<Minion>().underControl;
    }

    public void DoInteraction()
    {
        controller.SetGroupUnderControl();
    }

    public string GetActionText()
    {
        return interactText;
    }

    public void SetActiveStatus(bool status)
    {
        if (interactCollider)
        {
            interactCollider.isTrigger = status;
        }
    }
}
