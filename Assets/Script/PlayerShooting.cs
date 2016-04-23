using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using GameData;

public class PlayerShooting : NetworkBehaviour
{
    public ParticleSystem _muzzleSmoke;
    public PaintManager _PaintManager;
    public AudioSource _gunAudio;
    public GameObject CameraPos;

    [SyncVar]

    public float FireDamage = 34f;
    [SyncVar]

    public float FireRate = 0.25f;
    [SyncVar]
    private float FireRateCD = 0;

    [SyncVar]
    public FireColors FireColor = FireColors.blue;
    public WeaponManager WeaponManager_;

    PlayerUI PUI;

    void Start() {
        PUI = GetComponent<PlayerUI>();
    }

    void Update() {
        if (Input.GetButton("Fire1"))
            CmdPlayerShot(FireRate, FireDamage);

        if (Input.GetKey("1")) {
            CmdChangeWeapon(FireColors.blue);
            PUI.ChangeWeapon(FireColors.blue);
        }

        if (Input.GetKey("2")) {
            CmdChangeWeapon(FireColors.red);
            PUI.ChangeWeapon(FireColors.red);
        }

        if (Input.GetKey("3")) {
            CmdChangeWeapon(FireColors.yellow);
            PUI.ChangeWeapon(FireColors.yellow);
        }

        if (Input.GetKey("4")) {
            CmdChangeWeapon(FireColors.green);
            PUI.ChangeWeapon(FireColors.green);
        }

        if (Input.GetKey("5")) {
            CmdChangeWeapon(FireColors.white);
            PUI.ChangeWeapon(FireColors.white);
        }

    }

    IEnumerator CoBurstMuzzle(float WaitTime) {
        // Beautiful hack to solve a Unity 5 bug
        var ParticleEmission_ = _muzzleSmoke.emission;
        ParticleEmission_.enabled = true;
        yield return new WaitForSeconds(WaitTime);
        ParticleEmission_.enabled = false;
    }

    [Command]
    void CmdPlayerShot(float FireRate_, float FireDamage_)
    {
        // Fire rate
        if(Time.time > FireRateCD && WeaponManager_.GetCurAmmo() > 0 ) {
            // Set time when the gun can fire again
            FireRateCD = Time.time + FireRate_;
            // Remove a bullet from the ammo system
            WeaponManager_.ShotCurAmmo();
            // Show muzzle smoke out of the gun
            RpcMuzzle();
            // Play audio
            RpcPlayAudio();

            RaycastHit hit;
            Vector3 rayPos = CameraPos.transform.position + CameraPos.transform.forward;

            Debug.DrawRay(rayPos, CameraPos.transform.forward, Color.green, 0.1f, true);
            if (Physics.Raycast(rayPos, CameraPos.transform.forward, out hit, 10))
            {
                Debug.Log("Hit: " + hit.transform.gameObject.name);
                Debug.DrawRay(hit.point, hit.normal, Color.red, 2.0f);


                // Color
                switch (FireColor)
                {
                    case FireColors.blue:
                        RpcImpact(Color.blue, hit.point, hit.normal);
                        break;
                    case FireColors.red:
                        RpcImpact(Color.red, hit.point, hit.normal);
                        break;
                    case FireColors.yellow:
                        RpcImpact(Color.yellow, hit.point, hit.normal);
                        break;
                    case FireColors.green:
                        RpcImpact(Color.green, hit.point, hit.normal);
                        break;
                    case FireColors.white:
                        RpcImpact(Color.white, hit.point, hit.normal);
                        break;
                }

                // Get Other playerNetworking
                PlayerNetworking PN_ = hit.transform.gameObject.GetComponent<PlayerNetworking>();

                // Check if there is a PlayerNetworking script on the object
                if (PN_ != null) {
                
                    PN_.RpcResolveHit(FireDamage_);
                }
            }


            Debug.Log(WeaponManager_.GetCurAmmo());
        }
        else 
        if(WeaponManager_.GetCurAmmo() < 1) {
            // No ammo left

            Debug.Log("No ammo left");
        }
    }

    [ClientRpc]
    void RpcMuzzle() {
        // Show muzzle smoke out of the shooting gun
        StartCoroutine(CoBurstMuzzle(0.1f));
    }

    [ClientRpc]
    void RpcPlayAudio() {
        // Play audio at the local object
        _gunAudio.Play();
    }

    [ClientRpc]
    void RpcImpact(Color Color_, Vector3 Position_, Vector3 Normal_) {
        _PaintManager.NewParticle(Color_, Position_, Normal_);

        // Green splash dmg
        if (Color_ == Color.green) {
            Collider[] Coll_ = Physics.OverlapSphere(Position_, 1);

            foreach (Collider hit in Coll_) {
                PlayerNetworking PN_ = hit.transform.gameObject.GetComponent<PlayerNetworking>();

                if (PN_ != null && gameObject.name != "Local player")
                    PN_.RpcResolveHit(FireDamage * 0.5f);
            }
        }
    }

    [Command]
    void CmdChangeWeapon(FireColors Color_) {
        switch (Color_)
        {
            case FireColors.blue: // Normal dmg / Normal speed / normal reload
                this.FireColor = FireColors.blue;
                this.FireRate = 0.25f;
                this.FireDamage = 25f;
                break;
            case FireColors.red: // Higher dmg / Lower speed / normal reload
                this.FireColor = FireColors.red;
                this.FireRate = 0.35f;
                this.FireDamage = 34f;
                break;
            case FireColors.yellow: // Low dmg / High speed / low reload speed
                this.FireColor = FireColors.yellow;
                this.FireRate = 0.10f;
                this.FireDamage = 20f;
                break;
            case FireColors.green: // Low dmg / Low firerate / low reload speed / splash dmg
                this.FireColor = FireColors.green;
                this.FireRate = 0.70f;
                this.FireDamage = 25f;
                break;
            case FireColors.white: // High dmg / Low firerate / low reload speed 
                this.FireColor = FireColors.white;
                this.FireRate = 1;
                this.FireDamage = 100f;
                break;
            default:
                Debug.LogError("ChangeWeapon: Invalid Color");
                break;
        }

        WeaponManager_.ChangeAmmoType(Color_);
    }
}
