using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private LayerMask placementLayermask;
    [SerializeField] private GameObject mouseIndicator, cellIndicator;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float itemsRadius = 1.5f;

    [SerializeField] private Grid grid;
    [SerializeField] private GameObject tileGenerator;
    private BoxCollider currentBoxCollider;

    private GameObject nextHexagon;
    private bool result;

    void Start(){
        tileGenerator.GetComponent<TempGenerator>().Init();
        cellIndicator = Instantiate(tileGenerator.GetComponent<TempGenerator>().hexTilePrefab);
        cellIndicator.GetComponent<BoxCollider>().enabled = false;

    }
    private void Update()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        mouseIndicator.transform.position = mousePosition;
        Vector3 worldGridPosition = grid.CellToWorld(gridPosition);
        cellIndicator.transform.position = worldGridPosition;
        bool canPlace = IsPossible(worldGridPosition);

        cellIndicator.SetActive(canPlace);

        if (Input.GetMouseButtonDown(0))
        {
            tileGenerator.GetComponent<TempGenerator>().GenerateTile(grid.CellToWorld(gridPosition));
            // grid.
            if (canPlace)
            {
                cellIndicator.GetComponent<BoxCollider>().enabled = true;
                cellIndicator = Instantiate(tileGenerator.GetComponent<TempGenerator>().hexTilePrefab);
                cellIndicator.GetComponent<BoxCollider>().enabled = false;

            };
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            cellIndicator.transform.Rotate(Vector3.up * 60f, Space.Self);
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            cellIndicator.transform.Rotate(Vector3.down * 60f, Space.Self);
        }
    }

    bool IsPossible(Vector3 gridPosition)
    {
        var hitColliders = new Collider[7];
        int hitNumber = Physics.OverlapSphereNonAlloc(gridPosition, itemsRadius, hitColliders, ~placementLayermask);
        int count = 0;
        foreach (Collider eachCollider in hitColliders)
        {
            if (eachCollider != null && eachCollider.transform.position == gridPosition)
            {
                count++;
            }
        }
        result = count == 0 &&  hitNumber > 0;
        return result;
    }

}
