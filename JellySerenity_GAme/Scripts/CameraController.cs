using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private Camera mainCamera;
	[SerializeField] private Vector3 defaultPosition = new Vector3(-5.4f, 35.8f, -56f);
	[SerializeField] private Quaternion defaultRotation = Quaternion.Euler(30.4f, 17.95f, 0f);

	[SerializeField] private float zoomDuration = 0.75f;
	[SerializeField] private float zoomDistance = 32f;
	[SerializeField] private float returnDuration = 1.1f;
	[SerializeField] private float rotationSpeed = 190f;
	[SerializeField] private float rotationCount = 1f;
	[SerializeField] private float tiltAngle = 24f;
	private float rotationAngle = 95f;
	private Transform target;
	private bool isZooming = false;
	private Vector3 fixedTargetPosition;

	public void ZoomToTarget(Transform targetTransform)
	{
		if (targetTransform == null) return;

		target = targetTransform;
		fixedTargetPosition = target.position;

		StopAllCoroutines();
		StartCoroutine(ZoomIn());
	}

	public void ReturnToDefault()
	{
		if (!isZooming) return;

		StopAllCoroutines();
		StartCoroutine(ZoomOut());
	}

	private IEnumerator ZoomIn()
	{
		isZooming = true;

		Vector3 startPosition = transform.position;
		Quaternion startRotation = transform.rotation;

		Vector3 targetPosition = CalculateTiltedPosition(fixedTargetPosition, zoomDistance, tiltAngle);
		Quaternion targetRotation = Quaternion.LookRotation(fixedTargetPosition - targetPosition);

		float elapsedTime = 0f;
		while (elapsedTime < zoomDuration)
		{
			elapsedTime += Time.deltaTime;
			float t = elapsedTime / zoomDuration;

			transform.position = Vector3.Lerp(startPosition, targetPosition, t);
			transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

			  yield return null;
		}

		transform.position = targetPosition;
		transform.rotation = targetRotation;
				
		StartCoroutine(RotateAroundTarget());
	}

	private IEnumerator RotateAroundTarget()
	{
		float totalRotation = rotationAngle * rotationCount;
		float currentRotation = 0f;

		while (currentRotation < totalRotation)
		{
			float rotationStep = rotationSpeed * Time.deltaTime;
			currentRotation += rotationStep;

			transform.RotateAround(fixedTargetPosition, Vector3.up, rotationStep);
			yield return null;
		}
		
		StartCoroutine(ZoomOut());
	}

	private IEnumerator ZoomOut()
	{
		isZooming = false;

		Vector3 startPosition = transform.position;
		Quaternion startRotation = transform.rotation;

		float elapsedTime = 0f;
		while (elapsedTime < returnDuration)
		{
			elapsedTime += Time.deltaTime;
			float t = elapsedTime / returnDuration;

			transform.position = Vector3.Lerp(startPosition, defaultPosition, t);
			transform.rotation = Quaternion.Lerp(startRotation, defaultRotation, t);

			yield return null;
		}

		transform.position = defaultPosition;
		transform.rotation = defaultRotation;
	}

	private Vector3 CalculateTiltedPosition(Vector3 targetPosition, float distance, float angle)
	{
		Vector3 direction = (transform.position - targetPosition).normalized;
		Quaternion tiltRotation = Quaternion.Euler(angle, 0, 0);
		direction = tiltRotation * direction;

		return targetPosition + direction * distance;
	}
}