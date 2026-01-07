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
    GameObject levelLogic;
    GameObject global;
    GameObject mainCamera;
    GameObject player;
    GameObject soundEffects;
    GameObject moneyText;
    Transform playerSpawn;
    string[] gameplayScenes = {"LEVEL1", "TUTORIAL", "House"};
    string[] blankScenes = {"TITLESCREEN"};

    void Start()
    {
        if (Global.Instance == null)
        {
            bool isGameplay = gameplayScenes.Contains(SceneManager.GetActiveScene().name);
            global = (GameObject)Resources.Load("Create On Scene Load/global");
            global = Instantiate(global);
            global.GetComponent<Global>().gameplay = isGameplay;
            
        }
        if (!blankScenes.Contains(SceneManager.GetActiveScene().name))
        {
            StartScene();
        }
        else
        {
            MakeSoundManager();
        }
        
        DestroyImmediate(gameObject);
    }
    public void StartScene()
    {
        bool isGameplay = gameplayScenes.Contains(SceneManager.GetActiveScene().name);
        canvas = (GameObject)Resources.Load("Create On Scene Load/Canvas");
        if (isGameplay)
        {
            levelLogic = (GameObject)Resources.Load("Create On Scene Load/Level Logic");
        }
        mainCamera = (GameObject)Resources.Load("Create On Scene Load/Main Camera");
        player = (GameObject)Resources.Load("Create On Scene Load/player");
        

        playerSpawn = transform.GetChild(0);
        player.transform.position = playerSpawn.position;
        player.transform.rotation = playerSpawn.rotation;
        
        player = Instantiate(player);
        canvas = Instantiate(canvas);

        MakeSoundManager();

        if (isGameplay)
        {
            moneyText = (GameObject)Resources.Load("Create On Scene Load/MONEY");
            moneyText = Instantiate(moneyText);
            levelLogic.GetComponent<LevelLogic>().moneyText = moneyText;
            moneyText.transform.SetParent(canvas.transform);
        }

        if (isGameplay)
        {
            levelLogic = Instantiate(levelLogic);
        }
        mainCamera = Instantiate(mainCamera);

        
        
        mainCamera.transform.position = playerSpawn.position + new Vector3(5,5,0);
        mainCamera.transform.rotation = Quaternion.LookRotation(player.transform.position);
        
        player.GetComponent<Player>().cam = mainCamera.transform.Find("FreeLook Camera").GetComponent<CinemachineFreeLook>();
        mainCamera.transform.Find("FreeLook Camera").GetComponent<CinemachineFreeLook>().m_XAxis.Value = player.transform.rotation.eulerAngles.y;
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
            if (go.layer == layer && go.GetComponent<ItemCost>() != null)
            {
                go.GetComponent<ItemCost>().CreateShownValue();
            }
        }
    }

    void MakeSoundManager()
    {
        soundEffects = (GameObject)Resources.Load("Create On Scene Load/SoundEffects");
        soundEffects = Instantiate(soundEffects);
    }

}
