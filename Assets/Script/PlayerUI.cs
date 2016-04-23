using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using GameData;

public class PlayerUI : NetworkBehaviour {

    public GameObject Canvas;
    
    GameObject UCanvas;
    Text UText;
    Text UType;
    Image UCrosshair;

    void Start() {
        Debug.Log("Test");
        UCanvas = Instantiate(Canvas, Vector3.zero, Quaternion.identity) as GameObject;
        UCanvas.name = "Local canvas";

        // Get Ammo text
        if (UCanvas != null)
            UText = UCanvas.transform.Find("AmmoText").GetComponent<Text>();

        // Get weapon text
        if (UCanvas != null)
            UType = UCanvas.transform.Find("WeaponType").GetComponent<Text>();

        // Get crosshair image
        if (UCanvas != null)
            UCrosshair = UCanvas.transform.Find("Crosshair").GetComponent<Image>();

        if (hasAuthority)
            UCanvas.SetActive(true);

    }

    [ClientRpc]
    public void RpcUpdateAmmo(string ammo) {
        if (hasAuthority) {
            if (ammo.Equals("Infinity"))
                ammo = "∞";

            UText.text = ammo;
        }
    }

    public void ChangeWeapon(FireColors color) {

        switch (color)
        {
            case FireColors.blue:
                UType.text = "Blauwe verf:";
                UType.color = new Color(0f, 0f, 1f, 1f);
                break;
            case FireColors.red:
                UType.text = "Rode verf:";
                UType.color = new Color(1f, 0f, 0f, 1f);
                break;
            case FireColors.yellow:
                UType.text = "Gele verf:";
                UType.color = new Color(1f, 1f, 0f, 1f);
                break;
            case FireColors.green:
                UType.text = "Groene verf:";
                UType.color = new Color(0f, 1f, 0f, 1f);
                break;
            case FireColors.white:
                UType.text = "Witte verf:";
                UType.color = new Color(1f, 1f, 1f, 1f);
                break;
            default:
                UType.text = "!!Unknown!!";
                UType.color = new Color(0f, 0f, 0f, 1f);
                break;
        }
    }

    [ClientRpc]
    public void RpcFlashCrosshair() {
        UCrosshair.color = Color.black;
        Invoke("ResetCrosshair", 0.25f);
    }

    private void ResetCrosshair() {
        UCrosshair.color = Color.white;
    }

}
