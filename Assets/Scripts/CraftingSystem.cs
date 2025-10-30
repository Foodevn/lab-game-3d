using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem Instance;

    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI;
    public List<string> inventoryItemList = new List<string>();

    public BluePrint AxeBLP = new BluePrint("Axe", 2, "Stone", 3, "Stick", 3);
    Button toolsBtn;

    Button craftAxeBtn;

    TMP_Text AxeReq1, AxeReq2;

    public bool isOpen;


    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        isOpen = false;
        
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        
        toolsBtn = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBtn.onClick.AddListener(delegate { OpenToolsCategory(); });

        AxeReq1 = toolsScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<TMP_Text>();
        AxeReq2 = toolsScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<TMP_Text>();

        craftAxeBtn = toolsScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeBtn.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });
    }

    private void CraftAnyItem(BluePrint bluePrintToCraft)
    {
        InventorySystem.Instance.AddToInventory(bluePrintToCraft.itemName);
        if (bluePrintToCraft.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(bluePrintToCraft.Req1, bluePrintToCraft.Req1amount);
        }
        else if (bluePrintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(bluePrintToCraft.Req1, bluePrintToCraft.Req1amount);
            InventorySystem.Instance.RemoveItem(bluePrintToCraft.Req2, bluePrintToCraft.Req2amount);
        }

        StartCoroutine(caculate());

        RefeshNeededItems();
    }

    public IEnumerator caculate()
    {
        yield return new WaitForSeconds(1f);
        InventorySystem.Instance.ReCalculeList();
    }

    private void RefeshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;
        inventoryItemList = InventorySystem.Instance.itemList;
        foreach (string item in inventoryItemList)
        {
            switch (item)
            {
                case "Stone":
                    stone_count++;
                    break;
                case "Stick":
                    stick_count++;
                    break;
            }
        }

        AxeReq1.text = "3 Stone [" + stone_count + "]";
        AxeReq2.text = "3 Stick [" + stick_count + "]";

        if (stone_count >= 3 && stick_count >= 3)
        {
            craftAxeBtn.gameObject.SetActive(true);
        }
        else
        {
            craftAxeBtn.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RefeshNeededItems();
        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {
            Debug.Log("C is pressed");
            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);
            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            isOpen = false;
        }
    }

    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(true);
    }
}