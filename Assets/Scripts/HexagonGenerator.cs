using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HexagonGenerator : MonoBehaviour
{
    public GameObject innerHexagon;
    public GameObject hexagonSection_0;
    public GameObject hexagonSection_1;
    public GameObject hexagonSection_2;
    public GameObject hexagonSection_3;
    public GameObject hexagonSection_4;
    public GameObject hexagonSection_5;

    public Material[] materials = new Material[10];

    public void Start(){

    }

    public void Update(){

    }

    public void GenerateTile(){

        innerHexagon.GetComponent<HexagonTile>().material = materials[Random.Range(0, materials.Length)];

        hexagonSection_0.GetComponent<HexagonSection>().material = materials[Random.Range(0, materials.Length)];
        hexagonSection_1.GetComponent<HexagonSection>().material = materials[Random.Range(0, materials.Length)];
        hexagonSection_2.GetComponent<HexagonSection>().material = materials[Random.Range(0, materials.Length)];
        hexagonSection_3.GetComponent<HexagonSection>().material = materials[Random.Range(0, materials.Length)];
        hexagonSection_4.GetComponent<HexagonSection>().material = materials[Random.Range(0, materials.Length)];
        hexagonSection_5.GetComponent<HexagonSection>().material = materials[Random.Range(0, materials.Length)];
    }

    public void InstantiateTile(Vector3 pos){
        Instantiate(innerHexagon, pos, Quaternion.identity);
        Instantiate(hexagonSection_0, pos, Quaternion.identity);
        Instantiate(hexagonSection_1, pos, Quaternion.identity);
        Instantiate(hexagonSection_2, pos, Quaternion.identity);
        Instantiate(hexagonSection_3, pos, Quaternion.identity);
        Instantiate(hexagonSection_4, pos, Quaternion.identity);
        Instantiate(hexagonSection_5, pos, Quaternion.identity);
    }

    Vector3 GetMousePositionInWorld(){
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z += Camera.main.transform.position.z;
        mousePosition.y += Camera.main.transform.position.y;
        Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePosition);
        

        return mousePositionInWorld;
    }

}
