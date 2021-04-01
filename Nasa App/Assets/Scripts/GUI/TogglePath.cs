using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TextUpdater;


public class TogglePath : MonoBehaviour
{
    Pathfinding.GridGraph gg;
    public static bool shortestPath = false;
    public GameObject panel, player, ai, waypointPanel, path;
    public Looking looking;
    public TextUpdater pathName;

    void Start() {
        gg = AstarPath.active.data.gridGraph; // This gets the grid graph
        AstarPath.active.Scan(); // Updates the graph
        Cursor.lockState = CursorLockMode.None;
    }

    public void ShortestPath() {
        gg.erodeIterations = 1; // Makes the AI go a bit safer along the path
        gg.maxSlope = 34.5f; // Changes the slope that that AI can go on -- AI Slope is different from Players for whatever reason so it has to be higher
        shortestPath = true;
        pathName.textUpdate("Current Path: Shortest");
        DoStuff();
    }

    public void SafestPath() {
        gg.erodeIterations = 0;
        gg.maxSlope = 26.5f;
        shortestPath = false;
        pathName.textUpdate("Current Path: Safest");
        DoStuff();
    }

    private void DoStuff() {
        panel.SetActive(false);
        waypointPanel.SetActive(true);
        player.SetActive(true);
        // ai.SetActive(true);
        looking.setLookMode(true);
        AstarPath.active.Scan();
        path.SetActive(true);
    }
}
