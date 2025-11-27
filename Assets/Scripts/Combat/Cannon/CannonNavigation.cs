using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class CannonNavigation
{
    private float maxAngle = 50f;
    private float acceleration = 180f;
    private float deceleration = 200f;

    private readonly float recoilDistance = 0.2f;
    private readonly float recoilReturnSpeed = 0.9f;

    private float currentAngle = 0f;
    private float rotateSpeed = 0f;
    private float recoilOffset = 0f;
    private float recoilVelocity = 0f;

    private Vector3 rotateObjectStartPos;
    private Transform recoilPivot;

    private GameObject rotateObject;
    private GameObject navigateGuideObject;
    private bool isFront;

    public CannonNavigation(GameObject rotateObject, GameObject navigateGuideObject, Transform recoilPivot, bool isFront)
    {      
        this.rotateObject = rotateObject;
        this.navigateGuideObject = navigateGuideObject;
        this.recoilPivot = recoilPivot;   
        this.isFront = isFront;
    }

    public void UpdateNavigation(float input)
    {
        rotateObjectStartPos = recoilPivot.transform.position;

        if (Mathf.Abs(input) > 0.01f)
        {
            rotateSpeed += input * acceleration * Time.deltaTime;
        }
        else
        {
            rotateSpeed = Mathf.MoveTowards(rotateSpeed, 0, deceleration * Time.deltaTime);
        }

        currentAngle += rotateSpeed * Time.deltaTime;
        if (currentAngle >= maxAngle)
        {
            currentAngle = maxAngle;
            if (rotateSpeed > 0) rotateSpeed = 0;
        }
        else if (currentAngle <= -maxAngle)
        {
            currentAngle = -maxAngle;
            if (rotateSpeed < 0) rotateSpeed = 0;
        }

        currentAngle = Mathf.Clamp(currentAngle, -maxAngle, maxAngle);

        recoilOffset = Mathf.SmoothDamp(recoilOffset, 0, ref recoilVelocity, 1f / recoilReturnSpeed);

        if (rotateObject != null)
        {
            if (isFront)
            {
                rotateObject.transform.localRotation = Quaternion.Euler(0, 0, -currentAngle);

                Vector3 recoilDir = rotateObject.transform.up * recoilOffset;
                rotateObject.transform.position = rotateObjectStartPos + recoilDir;
            }
            else
            {
                rotateObject.transform.localRotation = Quaternion.Euler(0, 0, currentAngle);

                Vector3 recoilDir = rotateObject.transform.up * recoilOffset;
                rotateObject.transform.position = rotateObjectStartPos + recoilDir;
            }
        }
           
    }

    public Vector2 GetFireDirection()
    {
        if (navigateGuideObject == null)
            return Vector2.right;

        var baseDir = -navigateGuideObject.transform.up;      

        if (isFront)
        {
            var rotatedDir = Quaternion.Euler(0, 0, 0f) * baseDir;
            return rotatedDir.normalized;
        }
        else
        {
            var rotatedDir = Quaternion.Euler(0, 0, -0f) * baseDir;
            return -rotatedDir.normalized;
        }
    }

    public void ApplyRecoil()
    {
        if (isFront)
        {
            recoilOffset = recoilDistance;
        }
        else
        {
            recoilOffset = -recoilDistance;
        }
    }
}