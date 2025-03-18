using UnityEngine;
using System.Collections.Generic;


    public class JellySerenity : MonoBehaviour
    {
        [SerializeField] private GameObject jellyPinckPrefab; 
        [SerializeField] private GameObject jellyBluePrefab; 
        
        [SerializeField] private int jellyCount = 20;    
        [SerializeField] private float spacing = 2f;     

        private List<JellySystem> jellySystems = new List<JellySystem>();
        
        [SerializeField] private Material defaultMaterial; 
        [SerializeField] private Material redMaterial; 
        [SerializeField] private Material blueMaterial; 

        [SerializeField] private CameraController cameraController;
        
        private void Awake()
        {
            SpawnJellies(jellyCount);
        }
        public void SpawnJellies(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 position = new Vector3(i % 5 * spacing, 0, i / 5 * spacing);
                GameObject jellyPrefab = (i == count - 1) ? jellyPinckPrefab : jellyBluePrefab;

                GameObject jelly = Instantiate(jellyPrefab, position, Quaternion.identity);

                JellySystem jellySystem = jelly.GetComponent<JellySystem>();
                
                if (jellySystem != null)
                {
                    jellySystem.cameraController = cameraController;

                    jellySystems.Add(jellySystem);
                }
            }
        }
        
        public void SpawnJellies(int blueCount, int pinkCount)                                   
        {
            int total = blueCount + pinkCount;
            for (int i = 0; i < total; i++)
            {
                Vector3 position = new Vector3(i % 5 * spacing, 0, i / 5 * spacing);
                
                GameObject jellyPrefab = (i < blueCount) ? jellyBluePrefab : jellyPinckPrefab;
                
                GameObject jelly = Instantiate(jellyPrefab, position, Quaternion.identity);

                JellySystem jellySystem = jelly.GetComponent<JellySystem>();
                if (jellySystem != null)
                {
                    jellySystems.Add(jellySystem);
                }
            }
        }
        private void InitializeJellies()
        {
            if (jellySystems.Count != jellyCount)
            {
                Debug.LogError("В сцене должно быть ровно 20 желешек!");
                return;
            }

            jellySystems[0].jellyMeshRenrr.material = redMaterial;
            jellySystems[0].materialForFilling = redMaterial;

            jellySystems[1].jellyMeshRenrr.material = blueMaterial;
            jellySystems[1].materialForFilling = blueMaterial;

            for (int i = 2; i < jellySystems.Count; i++)
            {
                jellySystems[i].jellyMeshRenrr.material = blueMaterial;
                jellySystems[i].materialForFilling = blueMaterial;
            }
        }
        public void ResetGame()
        {
            foreach (var jelly in jellySystems)
            {
                jelly.jellyMeshRenrr.material = defaultMaterial;
                jelly.materialForFilling = defaultMaterial;
                jelly.isColorCompletelyFilled = false;
                jelly.isSelected = false;
                jelly.isGives = false;
                jelly.isAccepts = false;
            }

            InitializeJellies();
        }
        
        public List<JellySystem> GetJelliesList()
        {
            return jellySystems;
        }
    }

