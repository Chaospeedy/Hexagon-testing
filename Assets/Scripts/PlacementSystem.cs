using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator, cellIndicator;
    [SerializeField] private InputManager inputManager;

    [SerializeField] private Grid grid;
    [SerializeField] private GameObject tileGenerator;


    private GameObject nextHexagon;

    
    private void Update(){
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);

        if(Input.GetMouseButtonDown(0)){
            tileGenerator.GetComponent<TempGenerator>().GenerateTile(grid.CellToWorld(gridPosition));
            cellIndicator = Instantiate(tileGenerator.GetComponent<TempGenerator>().hexTilePrefab);
        }
        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0){
            cellIndicator.transform.Rotate(Vector3.up * 60f, Space.Self);
        }
        if(Input.GetAxisRaw("Mouse ScrollWheel") < 0){
            cellIndicator.transform.Rotate(Vector3.down * 60f, Space.Self);
        }
    }

    
}
