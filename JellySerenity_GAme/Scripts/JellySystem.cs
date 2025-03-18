using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellySystem : MonoBehaviour
{
	//[SerializeField] private Gravity[] fluid;

	[Header("FXEnergi & Touch")]
	[SerializeField] public TakeJellyFX takeJellyFX;
	[SerializeField] private FxEnergeFluid energeFX;
	public bool isStartMoveToGravity;
	public CameraController cameraController;

	public MeshRenderer jellyMeshRenrr;
	public Material materialForFilling;
	private float speedFilling = 10f; // -3.9 10
	private bool startFillingcolor;
	public bool isColorCompletelyFilled;

	private MaterialPropertyBlock propertyBlock;

	//public MeshRenderer meshRenderer;
	[SerializeField] private Material colorBlue;
	[SerializeField] private Material colorPink;

	private Material colliderObjMaterial;
	private ContactPoint[] collisonTargetPoints;

	private float lastInteractionTime;
	private const float interactionCooldown = 0.85f;

	public float bounceSpeed = 100f;
	public float fallForce = 19f;
	public float stiffness = 2f;

	private MeshFilter meshFilter;
	private Mesh mesh;
	private MeshCollider meshCollider;
	private Mesh startMeshCollider;

	public bool isSelected;
	public bool isAccepts;
	public bool isGives;

	private Dictionary<string, Material> materialMap;

	Vector3[] initialVertices;
	Vector3[] currentVertices;
	Vector3[] vertexVelocities;

	private void Awake()
	{
		propertyBlock = new MaterialPropertyBlock();
		jellyMeshRenrr = GetComponent<MeshRenderer>();
		materialForFilling = jellyMeshRenrr.material;

		jellyMeshRenrr.GetPropertyBlock(propertyBlock);
		propertyBlock.SetFloat("_SecondTextureHeight", speedFilling);
		jellyMeshRenrr.SetPropertyBlock(propertyBlock);

		meshFilter = GetComponent<MeshFilter>();
		meshCollider = GetComponent<MeshCollider>();
		mesh = meshFilter.mesh;
		startMeshCollider = mesh;

		initialVertices = mesh.vertices;
		currentVertices = new Vector3[initialVertices.Length];
		vertexVelocities = new Vector3[initialVertices.Length];
		//SetGravityColor();
		takeJellyFX.SetStartColor(jellyMeshRenrr.material);
		energeFX.SetStartColor(jellyMeshRenrr.material);
		for (int i = 0; i < initialVertices.Length; i++)
		{
			currentVertices[i] = initialVertices[i];
		}
	}

	private void FixedUpdate()
	{
		UpdateVertices();
	   
		// if (isStartMoveToGravity)
		// {
		// 	foreach (var f in fluid)
		// 	{
		// 		f.WaterFalll2Point();
		// 	}           
		// }

		if (startFillingcolor && speedFilling > -3.9f)
		{
			speedFilling -= Time.deltaTime * 1.45f;
			 if (cameraController != null)
            {
                cameraController.ZoomToTarget(transform);
            }
			FillingColor();
		}
		else if (startFillingcolor && speedFilling <= -3.9f)
		{
			speedFilling = 10f;
 			cameraController.ReturnToDefault();
			FillingColor();
			SetNewMaterialColor(colliderObjMaterial);          
			isColorCompletelyFilled = true;
			isSelected = false;
			isAccepts = false;
			startFillingcolor = false;			
		}
	}

	

    void Start()
    {
        SetMaterialDictionarys();
    }
    private void SetMaterialDictionarys()
	{
	    materialMap = new Dictionary<string, Material>
		{
			{ "Blue", colorBlue },
			{ "Blue (Instance)", colorBlue },
			{ "Red", colorPink },
			{ "Red (Instance)", colorPink }
		};
	}

	private void SetNewMaterialColor(Material meshRendererGivers)
	{
		if (meshRendererGivers == null) return;

		if (materialMap.ContainsKey(meshRendererGivers.name.Replace(" (Instance)", "")))
		{
			Material newMaterial = materialMap[meshRendererGivers.name.Replace(" (Instance)", "")];
			takeJellyFX.SetNewColor(meshRendererGivers.name.Replace(" (Instance)", ""));
			energeFX.SetNewColor(meshRendererGivers.name.Replace(" (Instance)", ""));
			jellyMeshRenrr.material = newMaterial;
			materialForFilling = newMaterial;

			jellyMeshRenrr.GetPropertyBlock(propertyBlock);
			propertyBlock.SetFloat("_SecondTextureHeight", speedFilling);
			jellyMeshRenrr.SetPropertyBlock(propertyBlock);
		}
	}
	 
	public bool CheckTheSameColor(Material materialColr)
	{
		if (materialColr == null || jellyMeshRenrr.material == null)
		{
			return true;
		}

		string currentMaterialName = jellyMeshRenrr.material.name.Replace(" (Instance)", "");
		string targetMaterialName = materialColr.name.Replace(" (Instance)", "");

		return currentMaterialName == targetMaterialName;
	}

	private void UpdateVertices()
	{
		for (int i = 0; i < currentVertices.Length; i++)
		{
			Vector3 currentDisplacement = currentVertices[i] - initialVertices[i];
			vertexVelocities[i] -= currentDisplacement * bounceSpeed * Time.deltaTime;
			vertexVelocities[i] *= 1f - stiffness * Time.deltaTime;
			currentVertices[i] += vertexVelocities[i] * Time.deltaTime;
		}

		mesh.vertices = currentVertices;
		mesh.RecalculateBounds();
		mesh.RecalculateTangents();
	}

	private void OnCollisionEnter(Collision other)
	{
		if (Time.time - lastInteractionTime < interactionCooldown) return;

		JellySystem otherJelly = other.gameObject.GetComponent<JellySystem>();
		if (otherJelly == null || !otherJelly.isSelected || CheckTheSameColor(otherJelly.materialForFilling)) return;

		ContactPoint[] collisonPoints = other.contacts;

		foreach (var point in collisonPoints)
		{
			Vector3 inputPoint = point.point + (point.point * 0.1f);
			ApplyPressureToPoint(inputPoint, fallForce);

			if (isSelected && isGives)
			{
				isAccepts = false;
				StartFliudMoveGives(other);

				StartCoroutine(energeFX.FXRotation(otherJelly));
			}

			if (isSelected && isAccepts)
			{
				isGives = false;

				//StopFluidMovement();
				isColorCompletelyFilled = false;
				speedFilling = 10f; 
				startFillingcolor = true;

				//StartFliudMoveAccepts(other);
				colliderObjMaterial = otherJelly.materialForFilling;
			}
		}

		StartCoroutine(MeshStartReturner());
		lastInteractionTime = Time.time;
	}

	public void StartFliudMoveGives(Collision other)
	{
		if (collisonTargetPoints == null)
		{
			Vector3 transform = other.transform.position;

			isStartMoveToGravity = true;
		}
	}

	private void FillingColor()
	{
		jellyMeshRenrr.GetPropertyBlock(propertyBlock);
		propertyBlock.SetFloat("_SecondTextureHeight", speedFilling);
		jellyMeshRenrr.SetPropertyBlock(propertyBlock);

		if (speedFilling <= -3.9f)
		{
			speedFilling = 10f;
			SetNewMaterialColor(colliderObjMaterial);

			isColorCompletelyFilled = true;
			isSelected = false;
			isAccepts = false;
			startFillingcolor = false;
		}
	}

	public void ApplyPressureToPoint(Vector3 _point, float _pressure)
	{
		for (int i = 0; i < currentVertices.Length; i++)
		{
			ApplyPressureToVertex(i, _point, _pressure);
		}

		meshCollider.sharedMesh = mesh;
	}

	public void ApplyPressureToVertex(int _index, Vector3 _position, float _pressure)
	{
		Vector3 distanceVerticePoint = currentVertices[_index] - transform.InverseTransformPoint(_position);
		float adaptedPressure = _pressure / (1f + distanceVerticePoint.sqrMagnitude);
		float velocity = adaptedPressure * Time.deltaTime;
		vertexVelocities[_index] += distanceVerticePoint.normalized * velocity;
	}

	public IEnumerator MeshStartReturner()
	{
		yield return new WaitForSeconds(0.95f);
		meshCollider.sharedMesh = startMeshCollider;
	}

}