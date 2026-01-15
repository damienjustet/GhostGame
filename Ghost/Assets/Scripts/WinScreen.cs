using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
   public void loadLobby()
    {
        Global.Instance.LoadAScene("LOBBY");
    }
}
