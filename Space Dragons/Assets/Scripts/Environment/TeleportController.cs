using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    [SerializeField] GameObject uiCanvas = null;
    [SerializeField] GameObject uiTeleportButton = null;
    [SerializeField] GameObject uiTeleportArrows = null;
    [SerializeField] TextMeshProUGUI nameReadout = null;
    [SerializeField] TextMeshProUGUI teleportLocationReadout = null;
    [SerializeField] TextMeshProUGUI moneyReadout = null;
    [SerializeField] TextMeshProUGUI costReadout = null;
    [SerializeField] TextMeshProUGUI messageReadout = null;
    [SerializeField] Animator TeleportTransition = null;

    public string LocationName = null;
    public float costmultiplier = 0.05f;
    public bool visited = false;
    int index = 0;
    int cost = 0;
    List<TeleportController> visitedTeleports = null;

    // Start is called before the first frame update
    void Start()
    {
        visited = LoadManager.Instance.saveData.VisitedTeleports.ToList().Exists(e => e == LocationName);

        nameReadout.text = LocationName;
        teleportLocationReadout.text = LocationName;
        visitedTeleports = FindObjectsOfType<TeleportController>().Where(e => e.visited == true).ToList();
        if (visitedTeleports.Count > 0) UpdateUI();
    }

    public void IncreaseIndex()
    {
        index++;

        if (index >= visitedTeleports.Count)
        {
            index = 0;
        }
        Debug.Log(index.ToString());
        UpdateUI();
    }

    void UpdateUI()
    {
        cost = (int)(Vector3.Distance(transform.position, visitedTeleports[index].transform.position) * costmultiplier);
        costReadout.text = (cost).ToString();
        moneyReadout.text = WorldManager.Instance.PlayerController.money.ToString();
        teleportLocationReadout.text = visitedTeleports[index].LocationName;
        messageReadout.text = "";
        if (visitedTeleports[index].LocationName == LocationName)
        {
            uiTeleportButton.SetActive(false);
            messageReadout.text += "Current Location. ";
        }
        else if (WorldManager.Instance.PlayerController.money < cost)
        {
            uiTeleportArrows.SetActive(false);
            messageReadout.text += "Not Enough Money. ";
        }
        else
        {
            uiTeleportButton.SetActive(true);
        }
        if (visitedTeleports.Count == 1)
        {
            uiTeleportArrows.SetActive(false);
            messageReadout.text += "No Other Teleports Active.";

        }
        else
        {
            uiTeleportArrows.SetActive(true);
        }

    }

    public void DecreaseIndex()
    {
        index--;
        if (index < 0)
        {
            index = visitedTeleports.Count - 1;
        }
        UpdateUI();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            visited = true;
            uiCanvas.SetActive(true);
            visitedTeleports = FindObjectsOfType<TeleportController>().Where(e => e.visited == true).ToList();
            UpdateUI();
            Time.timeScale = 0;
        }
    }

    public void CloseUI()
    {
        uiCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    public void TeleportToLocation()
    {
        UpdateUI();
        WorldManager.Instance.PlayerController.RemoveMoney(cost);
        WorldManager.Instance.SpawnWarpHole(transform.position);
        CloseUI();
        TeleportTransition.SetTrigger("Warp");
    }

    public void MovePlayer()
    {
        Vector3 pos = visitedTeleports[index].transform.position + (WorldManager.Instance.Ship.bodyPartPrefabs[0].transform.up * 5.5f); //add an offset
        WorldManager.Instance.SpawnWarpHole(visitedTeleports[index].transform.position);
        foreach (Transform t in WorldManager.Instance.Ship.bodyPartTransforms)
        {
            if(t)
            {
                t.position = pos;
            }
        }
    }
}
