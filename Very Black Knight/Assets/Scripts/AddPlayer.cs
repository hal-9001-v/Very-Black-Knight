using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPlayer : MonoBehaviour
{
    public void addPlayer(string name) {
        PlayerPrefs.SetString("",name);
    }

}
