using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using GameData;

public class AmmoCrateManager : NetworkBehaviour {

    [Command]
    public void CmdAddAmmo(GameObject Player, AmmoColors AmmoColor, float CooldownDuration, GameObject AmmoCrate) {
        WeaponManager WM = Player.GetComponent<WeaponManager>();

        switch (AmmoColor)
        {
            case AmmoColors.red:
                WM.AddAmmo(AmmoColor, 15);
                break;
            case AmmoColors.yellow:
                WM.AddAmmo(AmmoColor, 20);
                break;
            case AmmoColors.green:
                WM.AddAmmo(AmmoColor, 10);
                break;
            case AmmoColors.white:
                WM.AddAmmo(AmmoColor, 5);
                break;
            default:
                break;
        }

        StartCoroutine(Respawn(CooldownDuration, AmmoCrate));
    }

    IEnumerator Respawn(float Duration, GameObject AmmoCrate) {
        AmmoCrate.SetActive(false);
        yield return new WaitForSeconds(Duration);
        AmmoCrate.SetActive(true);
    }
}
