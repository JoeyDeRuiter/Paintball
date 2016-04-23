using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using GameData;

public class AmmoCrate : NetworkBehaviour {

    public AmmoCrateManager AmmoManager;

    public float RotateSpeed = 100f;
    public AmmoColors AmmoColor = AmmoColors.red;
    public float Cooldown = 2f;

    // Update is called once per frame
    void Update() {
        transform.Rotate(0, RotateSpeed * Time.deltaTime, 0, Space.World);
    }

    void OnTriggerEnter(Collider coll) {

        // Check if the trigger is a player
        if (coll.gameObject.tag == "Player")
            AmmoManager.CmdAddAmmo(coll.gameObject, AmmoColor, Cooldown, gameObject);

    }
}
