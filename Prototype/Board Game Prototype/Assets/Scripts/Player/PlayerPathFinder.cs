using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerPathFinder : MonoBehaviour {


    private GameObject currentMouseOverObject;
    List<Tile> currentPath = new List<Tile>();
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void PathFindForHero(HeroControl movingHero, Camera viewCamera) {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.gameObject == currentMouseOverObject) {
                return;
            }
            else if (hit.collider.gameObject.GetComponentInParent<Tile>() != null) {
                UnhighlightPath();
                currentPath = FindShortestPath(movingHero.currentTile, hit.collider.gameObject.GetComponentInParent<Tile>());
                HighlightPath();
            }
        }
    }

    private void HighlightPath() {
        foreach (var tile in currentPath) {
            tile.Highlight();
        }
    }

    private void UnhighlightPath() {
        foreach (var tile in currentPath) {
            tile.Unhighlight();
        }
    }

    private List<Tile> FindShortestPath(Tile startTile, Tile destinationTile) {
        //change all the materials in the returned list to indicate the shortest path
        Dictionary<Tile, Tile> TileParentMap = new Dictionary<Tile, Tile>();
        return DFSPathFind(TileParentMap, startTile, destinationTile);

    }

    private List<Tile> DFSPathFind(Dictionary<Tile, Tile> TileParentMap, Tile startTile, Tile destinationTile) {
        List<Tile> shortestPath = new List<Tile>();
        List<Tile> visitedTiles = new List<Tile>();
        Queue<Tile> dfsQueue = new Queue<Tile>();

        dfsQueue.Enqueue(startTile);
        TileParentMap.Add(startTile, startTile);
        visitedTiles.Add(startTile);

        //Depth First Search the Tile Map to find the shortest path
        while (dfsQueue.Count != 0) {
            Tile currentTile = dfsQueue.Dequeue();
            foreach (Tile childTile in currentTile.neighbours) {
                if (childTile == destinationTile) {
                    shortestPath.Add(childTile);
                    Tile parentTile = currentTile;
                    while (parentTile != startTile) {
                        shortestPath.Add(parentTile);
                        TileParentMap.TryGetValue(parentTile, out parentTile);
                    }
                    return shortestPath;
                }
                if (!visitedTiles.Contains(childTile)) {
                    TileParentMap.Add(childTile, currentTile);
                    visitedTiles.Add(childTile);
                    dfsQueue.Enqueue(childTile);
                }
            }
        }
        return shortestPath;
    }
}