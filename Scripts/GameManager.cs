using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager instance;

    //Prefabs
    public GameObject playerControllerPrefab;
    public GameObject tankPawnPrefab;
    public Transform playerSpawntransform;
    public List<PlayerController> players;
    #endregion Variables

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        players= new List<PlayerController>();
    }

    // Update is called once per frame
    void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        GameObject newPlayerObj = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity);
        GameObject newPawnObj = Instantiate(tankPawnPrefab, playerSpawntransform.position, playerSpawntransform.rotation);

        Controller newController = newPlayerObj.GetComponent<Controller>();
        Pawn newPawn = newPawnObj.GetComponent<Pawn>();

        newController.pawn = newPawn;

    }
}
