using UnityEngine;
using System;
using System.Collections;
using GameData;

public class WeaponManager : MonoBehaviour {

    //public enum FireColors { blue, red, yellow, green, white };

    float curAmmo = 0;
    float blueAmmo = Mathf.Infinity;
    float redAmmo = 10;
    float yellowAmmo = 10;
    float greenAmmo = 10;
    float whiteAmmo = 10;

    FireColors curColor;
    PlayerUI PUI;

    void Start() {
        PUI = GetComponent<PlayerUI>();
    }

    public WeaponManager() {
        curAmmo = blueAmmo; // Set blue ammo as default
        curColor = FireColors.blue;
    }

    public void ChangeAmmoType(FireColors Color)
    {
        // Save cur ammo type to the old color
        switch (curColor)
        {
            case FireColors.blue:
                this.blueAmmo = this.curAmmo;
                break;
            case FireColors.red:
                this.redAmmo = this.curAmmo;
                break;
            case FireColors.yellow:
                this.yellowAmmo = this.curAmmo;
                break;
            case FireColors.green:
                this.greenAmmo = this.curAmmo;
                break;
            case FireColors.white:
                this.whiteAmmo = this.curAmmo;
                break;
            default:
                break;
        }

        // Set a new color
        switch (Color)
        {
            case FireColors.blue:
                this.curAmmo = this.blueAmmo;
                break;
            case FireColors.red:
                this.curAmmo = this.redAmmo;
                break;
            case FireColors.yellow:
                this.curAmmo = this.yellowAmmo;
                break;
            case FireColors.green:
                this.curAmmo = this.greenAmmo;
                break;
            case FireColors.white:
                this.curAmmo = this.whiteAmmo;
                break;
            default:
                break;
        }

        // Write new color as current color
        curColor = Color;

        // Set new ammo amount
        PUI.RpcUpdateAmmo(curAmmo.ToString());
    }


    public void ShotCurAmmo() {
        this.curAmmo--; // Make 1 shot
        PUI.RpcUpdateAmmo(curAmmo.ToString());
        //PUI.FlashCrosshair();
    }

    public float GetCurAmmo() { return this.curAmmo; }


    public void AddAmmo(AmmoColors AmmoType, float Amount) {
        // Add Amount to desired AmmoType
        switch (AmmoType)
        {
            case AmmoColors.red:
                this.redAmmo += Amount;
                break;
            case AmmoColors.yellow:
                this.yellowAmmo += Amount;
                break;
            case AmmoColors.green:
                this.greenAmmo += Amount;
                break;
            case AmmoColors.white:
                this.whiteAmmo += Amount;
                break;
            default:
                Debug.Log("No AmmoType found");
                return;
        }

        // Update current ammo
        if(curColor.ToString().Equals(AmmoType.ToString())) {
            this.curAmmo += Amount;
            PUI.RpcUpdateAmmo(curAmmo.ToString());
        }
    }

}
