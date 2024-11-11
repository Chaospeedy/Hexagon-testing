using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private InputManager inputManager;

    [SerializeField] private Grid grid;
    [SerializeField] private GameObject tileGenerator;
    [SerializeField] private int numberOfTiles;


    private Renderer[] previewRenderer;

    private HexagonSection[] cellIndicatorSections;

    private GameObject[] adjacentHexagons;

    private int pointsWorth = 0;

    public bool placementIsValid;
    private ScoreCounter scoreCounter;
    private TileCounter tileCounter;
    private int matchingSections = 0;

    private void Start(){
        previewRenderer = cellIndicator.GetComponentsInChildren<Renderer>();
        tileGenerator.GetComponent<HexagonGenerator>().GenerateTile();
        cellIndicator = Instantiate(tileGenerator.GetComponent<HexagonGenerator>().hexTilePrefab);
        cellIndicatorSections = cellIndicator.GetComponentsInChildren<HexagonSection>();
        adjacentHexagons = new GameObject[6];
        GameObject scoreGO = GameObject.Find("ScoreCounter");
        GameObject tileGO = GameObject.Find("TileCounter");

        scoreCounter = scoreGO.GetComponent<ScoreCounter>();
        tileCounter = tileGO.GetComponent<TileCounter>();

        tileCounter.tilesRemaining = numberOfTiles;

    }

    private void Update(){
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        placementIsValid = CheckPlacementValidity();

        if(Input.GetMouseButtonDown(0) && numberOfTiles > 0){
            if(placementIsValid) {
                //create new colors for cell indicator
                tileGenerator.GetComponent<HexagonGenerator>().GenerateTile();
                //place the current cell indicator in the section of the grid the player is hovering over

                CalculatePoints();

                cellIndicator.GetComponentInChildren<HexagonTile>().matchingSections = matchingSections;

                cellIndicator = Instantiate(tileGenerator.GetComponent<HexagonGenerator>().hexTilePrefab, gridPosition, Quaternion.identity);
                
                tileCounter.tilesRemaining = --numberOfTiles;
                scoreCounter.score += pointsWorth;
            }
             
        }
        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0 && placementIsValid){
            cellIndicator.transform.Rotate(Vector3.up * 60f, Space.Self);
        }
        if(Input.GetAxisRaw("Mouse ScrollWheel") < 0 && placementIsValid){
            cellIndicator.transform.Rotate(Vector3.down * 60f, Space.Self);
        }

        DisplayIndicator(gridPosition);
        if(numberOfTiles == 0){
            ToggleRenderOff();
        }
    }

    public bool CheckPlacementValidity()
    {   
        bool atLeastOne = false;
        if(cellIndicator.GetComponentInChildren<HexagonTile>().isColliding){
            return false;
        }
        foreach(HexagonSection section in cellIndicatorSections){
            
            if(section.GetComponent<HexagonSection>().isColliding){
                atLeastOne = true;
            }
            
        }
        
        return atLeastOne;
    }
    private void ToggleRenderOff(){
        foreach(Renderer renderers in previewRenderer){
            renderers.enabled = false;
        }
    }
    private void ToggleRenderOn(){
        foreach(Renderer renderers in previewRenderer){
            renderers.enabled = true;
        }
    }

    private void DisplayIndicator(Vector3Int gridPosition){
        if(placementIsValid){
            ToggleRenderOn();
        }else{
            ToggleRenderOff();
        }

        previewRenderer = cellIndicator.GetComponentsInChildren<Renderer>();
        cellIndicatorSections = cellIndicator.GetComponentsInChildren<HexagonSection>();
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }

    private void CalculatePoints(){
        pointsWorth = 0;
        matchingSections = 0;
        Array.Clear(adjacentHexagons, 0, adjacentHexagons.Length);
        for(int i = 0; i < cellIndicatorSections.Length; i++){
            if(cellIndicatorSections[i].isColliding){
                adjacentHexagons[i] = cellIndicatorSections[i].collidingWith.transform.parent.gameObject;
                if(cellIndicatorSections[i].collidingWith.GetComponent<HexagonSection>().material == cellIndicatorSections[cellIndicatorSections[i].sectionNumber].material){
                    pointsWorth += 100; 
                    matchingSections++; 
                    if(pointsWorth == 600){
                        numberOfTiles++;
                        pointsWorth += 400;
                    }
                    if(adjacentHexagons[i] != null){
                        adjacentHexagons[i].GetComponentInChildren<HexagonTile>().matchingSections++;
                        if(adjacentHexagons[i].GetComponentInChildren<HexagonTile>().matchingSections == 6){
                            pointsWorth += 150;
                            numberOfTiles++;
                        }
                    }
                            
                }
            }
        }
    }

    
}
