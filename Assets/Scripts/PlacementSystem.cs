using System;
using System.Collections;
using System.Collections.Generic;
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

    private int pointsWorth = 0;

    private void Start(){
        previewRenderer = cellIndicator.GetComponentsInChildren<Renderer>();
        cellIndicatorSections = cellIndicator.GetComponentsInChildren<HexagonSection>();
    }

    private void Update(){
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        bool placementIsValid = CheckPlacementValidity();
        if(placementIsValid && numberOfTiles > 0){
            ToggleRenderOn();
        }else{
            ToggleRenderOff();
        }

        previewRenderer = cellIndicator.GetComponentsInChildren<Renderer>();
        cellIndicatorSections = cellIndicator.GetComponentsInChildren<HexagonSection>();
        

        pointsWorth = 0;

        foreach(HexagonSection section in cellIndicatorSections){
            if(section.isColliding){
                if(section.collidingWith.GetComponent<HexagonSection>().material == cellIndicatorSections[section.sectionNumber].material){
                    pointsWorth += 100;
                }
            }
        }

        
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);

        if(Input.GetMouseButtonDown(0) && numberOfTiles > 0){
            if(placementIsValid) {
                //create new colors for cell indicator
                tileGenerator.GetComponent<HexagonGenerator>().GenerateTile();
                //place the current cell indicator in the section of the grid the player is hovering over
                cellIndicator = Instantiate(tileGenerator.GetComponent<HexagonGenerator>().hexTilePrefab);
                numberOfTiles--;
            }
             
        }
        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0 && placementIsValid){
            cellIndicator.transform.Rotate(Vector3.up * 60f, Space.Self);
        }
        if(Input.GetAxisRaw("Mouse ScrollWheel") < 0 && placementIsValid){
            cellIndicator.transform.Rotate(Vector3.down * 60f, Space.Self);
        }
    }

    private bool CheckPlacementValidity()
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

    
}
