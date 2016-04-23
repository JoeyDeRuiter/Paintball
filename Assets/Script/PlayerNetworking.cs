using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerNetworking : NetworkBehaviour {

    public PlayerController FPS_Controller;
    public PlayerShooting FPS_Shooting;
    public Camera FPS_Camera;
    public AudioListener FPS_AudioListener;
    public PlayerUI FPS_UI;

    Renderer[] renderers;
    Collider[] colliders;

    [SyncVar]
    public float Health = 100f; 

    void Start() {
        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider>();
    }

    void ToggleRenderer(bool status) {
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].enabled = status;
    }

    void ToggleColliders(bool status) {
        for (int i = 0; i < colliders.Length; i++)
            colliders[i].enabled = status;
    }

    void ToggleControls(bool status) {
        FPS_Controller.enabled = status;
        FPS_Shooting.enabled = status;
    }

    // If the player is a local player enable the controllers
    public override void OnStartLocalPlayer()
    {
        // Enable the local player
        FPS_Controller.enabled = true;
        FPS_Shooting.enabled = true;
        FPS_Camera.enabled = true;
        FPS_AudioListener.enabled = true;
        FPS_UI.enabled = true;

        // Set the name so we can easely find the local player
        gameObject.name = "Local player";

        base.OnStartLocalPlayer();
    }

    public void PlayerRespawn() {
        Debug.Log("-- Player die --");
        ToggleRenderer(true);
        ToggleColliders(true);

        if(isLocalPlayer)
            ToggleControls(true);
        Health = 100f;
    }

    [ClientRpc]
    public void RpcResolveHit(float Damage_) {
        //gameObject.transform.position += Vector3.up * 10;

        Health -= Damage_;

        if (Health <= 0)
        {
            Debug.Log("Disable renderers on: " + gameObject.name);

            // Toggle renderers on other clients
            ToggleRenderer(false);
            

            if (isLocalPlayer)
            {

                ToggleControls(false);
                // Change position on server/player client
                Transform spawnLocation = NetworkManager.singleton.GetStartPosition();
                transform.position = spawnLocation.position;
                transform.rotation = spawnLocation.rotation;

                ToggleControls(false);
            }

            //transform.position = Vector3.zerop;
            //transform.rotation = spawnLocation.rotation;
            ToggleColliders(false);
            ToggleControls(false);
            // Set the new pos

            Invoke("PlayerRespawn", 2f);
        }           
    }
}
