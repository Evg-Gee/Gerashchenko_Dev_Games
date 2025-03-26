using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCoontroll : MonoBehaviour
{
    [SerializeField] private  float interactionCooldown = 0.01f;   
    [SerializeField] private HandTutorial handTutorial;
    [SerializeField] private float pressureForce = 180f;
    [SerializeField] private float pressureOffset = 2f;

    public static InputCoontroll instance;
    public JellySystem saveJjellyfierisGives;
    private JellySystem saveJjellyfierisAccepts;
    private List<JellySystem> jellysObjSelected;
    private float lastInteractionTime;
    private Ray mouseRay;
    private RaycastHit raycastHit;    

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        jellysObjSelected = new List<JellySystem>();
    }

    private void Update()
    {
        if (Time.time - lastInteractionTime < interactionCooldown) return;

        if (Input.GetMouseButton(0))
        {
            mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(mouseRay, out raycastHit))
            {
                JellySystem jellyfier = raycastHit.collider.GetComponent<JellySystem>();

                if (jellyfier != null)
                {
                    StartCoroutine(handTutorial.HideHand());
                    Vector3 inputPoint = raycastHit.point + (raycastHit.normal * pressureOffset);
                    jellyfier.ApplyPressureToPoint(inputPoint, pressureForce);
                    StartCoroutine(jellyfier.MeshStartReturner());
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(mouseRay, out raycastHit))
            {
                JellySystem jellyfierGives = raycastHit.collider.GetComponent<JellySystem>();

                if (jellyfierGives != null)
                {
                    CleanDellList();

                    if (!jellyfierGives.isSelected)
                    {
                        jellysObjSelected.Add(jellyfierGives);
                        saveJjellyfierisGives = jellyfierGives;
                        jellyfierGives.isGives = true;
                        jellyfierGives.isSelected = true;
                        jellyfierGives.takeJellyFX.PlayTakeJellyFX();
                    }
                    else
                    {
                        jellyfierGives.isGives = false;
                        jellyfierGives.isSelected = false;
                        jellyfierGives.takeJellyFX.StopFX();
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(mouseRay, out raycastHit))
            {
                JellySystem jellyfierAccepts = raycastHit.collider.GetComponent<JellySystem>();

                if (jellyfierAccepts != null && saveJjellyfierisGives != null && jellyfierAccepts != saveJjellyfierisGives)
                {
                    jellysObjSelected.Add(jellyfierAccepts);
                    saveJjellyfierisAccepts = jellyfierAccepts;
                    jellyfierAccepts.isAccepts = true;
                    jellyfierAccepts.isSelected = true;
                }
            }

            if (saveJjellyfierisAccepts == null && saveJjellyfierisGives != null)
            {
                saveJjellyfierisGives.isGives = false;
                saveJjellyfierisGives.isSelected = false;
                saveJjellyfierisGives = null;
            }

            if (saveJjellyfierisGives == null && saveJjellyfierisAccepts != null)
            {
                saveJjellyfierisAccepts.isSelected = false;
                saveJjellyfierisAccepts.isAccepts = false;
                saveJjellyfierisAccepts = null;
            }
        }

        lastInteractionTime = Time.time;
    }

    private void CleanDellList()
    {
        if (jellysObjSelected.Count != 0)
        {
            foreach (var jelly in jellysObjSelected)
            {
                jelly.isSelected = false;
                jelly.isGives = false;
                jelly.isAccepts = false;
            }

            jellysObjSelected.Clear();
        }
    }
}

