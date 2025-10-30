using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Object = UnityEngine.Object;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { set; get; }

    public bool onTarget;
    public Object selectedObject;

    public GameObject interaction_Info_UI;
    TMP_Text interaction_text;

    private void Start()
    {
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponentInChildren<TMP_Text>();
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();

            if (interactable && interactable.playerInRange)
            {
                onTarget = true;
                selectedObject = interactable.gameObject;
                interaction_text.text = interactable.GetItemName();
                interaction_Info_UI.SetActive(true);
            }
            else // if there is a hit but without an interactable script
            {
                onTarget = false;
                interaction_Info_UI.SetActive(false);
            }
        }
        else // if there is no hit at all.
        {
            onTarget = false;
            interaction_Info_UI.SetActive(false);
        }
    }
}