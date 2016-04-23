using UnityEngine;
using System.Collections;

public class PaintManager : MonoBehaviour {

    public GameObject PaintPrefab;
    public string ContainerName;

    private GameObject Container;

	void Awake () {
        Container = GameObject.Find(ContainerName);
    }

    public void NewParticle(Color PaintColor, Vector3 Position, Vector3 Normal) {

        // Get a particle, or make a new one
        GameObject Particle_ = GetInactiveObject();
        if (Particle_ == null)
            Particle_ = NewObject();

        // Set paint color
        ParticleSystem PS_ = Particle_.GetComponent<ParticleSystem>();
        PS_.startColor = PaintColor;

        // Set new postition
        Particle_.transform.position = Position;

        // Play the particle
        StartCoroutine(PlayParticle(Particle_));
    }


    IEnumerator PlayParticle(GameObject Particle_) {
        ParticleSystem PS_ = Particle_.GetComponent<ParticleSystem>();
        Particle_.SetActive(true);
        yield return new WaitForSeconds(PS_.duration);
        Particle_.SetActive(false);
        
    }


    GameObject NewObject() {
        // Make a new object
        GameObject GO = (GameObject)Instantiate(PaintPrefab, Vector3.zero, Quaternion.identity);
        // Add the object to container
        GO.transform.parent = Container.transform;
        return GO;
    }
	

    GameObject GetInactiveObject() {
        Transform[] Parent_ = Container.transform.GetComponentsInChildren<Transform>(true);
    
        // Get all the inactive childs in the parent 
        foreach (Transform Child_ in Parent_)
            if (Child_.gameObject.activeSelf == false)
                return Child_.gameObject;

        return null;
    }

    Vector3 GetAngleOfNormal(Vector3 Normal_ ) {
        Vector3 Angles_ = new Vector3();

        if (Normal_.y != 0)
            Angles_.x =  90 * -Normal_.y;




        return Angles_;
    }
}
