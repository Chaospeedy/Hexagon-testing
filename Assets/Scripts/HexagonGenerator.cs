using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonGenerator : MonoBehaviour
{
    public GameObject hexTilePrefab;
    public Material[] materials = new Material[4];

    public Material[] newTileMaterials = new Material[6];
    // Start is called before the first frame update
    void Start()
    {
        GenerateMaterials();
        Instantiate(hexTilePrefab, new Vector3(0,0,0), Quaternion.identity);
    }


    public void GenerateTile(){

        int randomNumber = Random.Range(0, materials.Length);

        hexTilePrefab.transform.GetChild(0).GetComponent<HexagonTile>().material = newTileMaterials[randomNumber];

        hexTilePrefab.transform.GetChild(1).GetComponent<HexagonSection>().material = newTileMaterials[0];
        hexTilePrefab.transform.GetChild(2).GetComponent<HexagonSection>().material = newTileMaterials[1];
        hexTilePrefab.transform.GetChild(3).GetComponent<HexagonSection>().material = newTileMaterials[2];
        hexTilePrefab.transform.GetChild(4).GetComponent<HexagonSection>().material = newTileMaterials[3];
        hexTilePrefab.transform.GetChild(5).GetComponent<HexagonSection>().material = newTileMaterials[4];
        hexTilePrefab.transform.GetChild(6).GetComponent<HexagonSection>().material = newTileMaterials[5];

        for(int i = 1; i < newTileMaterials.Length; i++){
            if(Random.Range(0, 100) < 15){
                hexTilePrefab.transform.GetChild(i).GetComponent<HexagonSection>().material = newTileMaterials[randomNumber];
            }
        }
        
        GenerateMaterials();
    }


    private void GenerateMaterials(){

        for(int i = 0; i < newTileMaterials.Length; i++){
            newTileMaterials[i] = materials[Random.Range(0, materials.Length)];
        }
    }
}
