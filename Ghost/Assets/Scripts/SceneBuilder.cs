using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneBuilder : MonoBehaviour
{
    GameObject canvas;
    GameObject global;
    GameObject levelLogic;
    GameObject mainCamera;
    GameObject player;
    GameObject soundEffects;
    GameObject moneyText;
    Transform playerSpawn;
    string[] gameplayScenes = {"LEVEL1", "TUTORIAL", "House"};

    void Start()
    {
        bool isGameplay = gameplayScenes.Contains(SceneManager.GetActiveScene().name);
        canvas = (GameObject)Resources.Load("Create On Scene Load/Canvas");
        global = (GameObject)Resources.Load("Create On Scene Load/global");
        if (isGameplay)
        {
            levelLogic = (GameObject)Resources.Load("Create On Scene Load/Level Logic");
        }
        mainCamera = (GameObject)Resources.Load("Create On Scene Load/Main Camera");
        player = (GameObject)Resources.Load("Create On Scene Load/player");
        soundEffects = (GameObject)Resources.Load("Create On Scene Load/SoundEffects");

        player = Instantiate(player);
        canvas = Instantiate(canvas);
        if (isGameplay)
        {
            moneyText = (GameObject)Resources.Load("Create On Scene Load/MONEY");
            moneyText = Instantiate(moneyText);
            levelLogic.GetComponent<LevelLogic>().moneyText = moneyText;
            moneyText.transform.SetParent(canvas.transform);
        }

        soundEffects = Instantiate(soundEffects);
        global = Instantiate(global);
        if (isGameplay)
        {
            levelLogic = Instantiate(levelLogic);
        }
        mainCamera = Instantiate(mainCamera);

        playerSpawn = transform.GetChild(0);
        player.transform.position = playerSpawn.position;
        player.transform.rotation = playerSpawn.rotation;
        mainCamera.transform.position = playerSpawn.position + new Vector3(5,5,0);
        mainCamera.transform.rotation = Quaternion.LookRotation(player.transform.position);

        player.GetComponent<Player>().cam = mainCamera.transform.Find("FreeLook Camera").GetComponent<CinemachineFreeLook>();
        player.GetComponent<Player>().target = mainCamera.transform.Find("FreeLook Camera").transform;

        mainCamera.GetComponent<PixelArtCamera>()._rawImage = canvas.transform.Find("Camera Display").GetComponent<RawImage>();
        mainCamera.transform.Find("In-Game Text Camera").GetComponent<PixelArtCamera>()._rawImage = canvas.transform.Find("Camera Display").GetComponent<RawImage>();
        mainCamera.transform.Find("cameraLockTarget").GetComponent<CameraTarget>().player = player;
        mainCamera.GetComponent<PixelArtCamera>().UpdateRenderTexture();
        mainCamera.transform.Find("In-Game Text Camera").GetComponent<Camera>().targetTexture = mainCamera.GetComponent<Camera>().targetTexture;

        int layer = LayerMask.NameToLayer("item");
        GameObject[] allGameObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject go in allGameObjects)
        {
            if (go.layer == layer && go.GetComponent<posseion>() != null)
            {
                go.GetComponent<posseion>().CreateShownValue();
            }
        }


        Destroy(gameObject);
    }

}
