using UnityEngine;
using UnityEngine.InputSystem;

public class CannonNavigation
{
    private float maxAngle = 25;
    private float acceleration = 150f;
    private float deceleration = 200f;

    private readonly float recoilDistance = 0.2f;
    private readonly float recoilReturnSpeed = 0.9f;

    private float currentAngle = 0f;
    private float targetAngle = 0f;
    private float rotateSpeed = 0f;
    private float recoilOffset = 0f;
    private float recoilVelocity = 0f;

    private Vector3 rotateObjectStartPos;

    private GameObject rotateObject;
    private GameObject navigateGuideObject;

    public CannonNavigation(GameObject rotateObject, GameObject navigateGuideObject)
    {      
        this.rotateObject = rotateObject;
        this.navigateGuideObject = navigateGuideObject;
        rotateObjectStartPos = rotateObject.transform.position;
    }

    public void UpdateNavigation(float input)
    {
        if (Mathf.Abs(input) > 0.01f)
        {
            rotateSpeed += input * acceleration * Time.deltaTime;
        }
        else
        {
            rotateSpeed = Mathf.MoveTowards(rotateSpeed, 0, deceleration * Time.deltaTime);
        }

        currentAngle += rotateSpeed * Time.deltaTime;
        currentAngle = Mathf.Clamp(currentAngle, -maxAngle, maxAngle);

        recoilOffset = Mathf.SmoothDamp(recoilOffset, 0, ref recoilVelocity, 1f / recoilReturnSpeed);

        if (rotateObject != null)
        {
            rotateObject.transform.localRotation = Quaternion.Euler(0, 0, -currentAngle);

            Vector3 recoilDir = rotateObject.transform.up * recoilOffset;
            rotateObject.transform.position = rotateObjectStartPos + recoilDir;
        }
           
    }

    public Vector2 GetFireDirection()
    {
        if (navigateGuideObject == null)
            return Vector2.right;

        return -navigateGuideObject.transform.up.normalized;
    }

    public void ApplyRecoil()
    {
        recoilOffset = recoilDistance;
    }
}