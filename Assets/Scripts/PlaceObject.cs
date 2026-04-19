using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlaceObject : MonoBehaviour
{
    public GameObject victimPrefab;
    public GameObject rescuerPrefab;
    public GameObject instructionPanel;

    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private GameObject spawnedVictim;
    private GameObject spawnedRescuer;
    private bool arMode = false;

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        if (instructionPanel != null)
            instructionPanel.SetActive(false);
    }

    public void ActivateARMode()
    {
        arMode = true;
        if (instructionPanel != null)
        {
            Image bg = instructionPanel.GetComponent<Image>();
            if (bg != null)
                bg.color = new Color(0f, 0f, 0f, 0.1f);
        }
    }

    public void DeactivateARMode()
    {
        arMode = false;
        if (spawnedVictim != null) Destroy(spawnedVictim);
        if (spawnedRescuer != null) Destroy(spawnedRescuer);
        spawnedVictim = null;
        spawnedRescuer = null;
    }

    void Update()
    {
        if (!arMode) return;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Began) return;

            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose pose = hits[0].pose;

                if (spawnedVictim == null)
                {
                    // Victime allongée - taille normale
                    spawnedVictim = Instantiate(
                        victimPrefab,
                        pose.position,
                        Quaternion.Euler(0f, 0f, 0f)
                    );
                    spawnedVictim.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);

                    // Secouriste positionné AU DESSUS de la victime
                    // Décalé légèrement vers le thorax de la victime
                    Vector3 rescuerPos = pose.position + new Vector3(0f, 0f, -0.1f);
                    spawnedRescuer = Instantiate(
                        rescuerPrefab,
                        rescuerPos,
                        Quaternion.Euler(0f, 0f, 0f)
                    );
                    spawnedRescuer.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                }
                else
                {
                    spawnedVictim.transform.position = pose.position;
                    spawnedRescuer.transform.position = pose.position + new Vector3(0f, 0f, -0.1f);
                }
            }
        }
    }
}