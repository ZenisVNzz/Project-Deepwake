using UnityEngine;

public class EnemyCannonNavigation : IEnemyCannonNavigation
{
    private float maxAngle = 50f;
    private float acceleration = 250f;
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

    public EnemyCannonNavigation(GameObject rotateObject, GameObject navigateGuideObject, Transform recoilPivot, bool isFront)
    {
        this.rotateObject = rotateObject;
        this.navigateGuideObject = navigateGuideObject;
        this.recoilPivot = recoilPivot;
        this.isFront = isFront;
    }

    public void UpdateNavigationTowardsTarget(Vector3 targetWorldPos)
    {
        if (recoilPivot == null) return;

        rotateObjectStartPos = recoilPivot.position;

        Vector2 baseDir = Vector2.right;
        if (navigateGuideObject != null)
            baseDir = navigateGuideObject.transform.up;

        Vector2 toTarget = (targetWorldPos - recoilPivot.position);
        if (toTarget.sqrMagnitude < 0.0001f)
        {
            rotateSpeed = Mathf.MoveTowards(rotateSpeed, 0f, deceleration * Time.deltaTime);
            currentAngle = Mathf.MoveTowards(currentAngle, 0f, deceleration * Time.deltaTime);
        }
        else
        {
            Vector2 targetDir = toTarget.normalized;
            float desiredAngle = Vector2.SignedAngle(baseDir, targetDir);

            if (!isFront)
            {
                desiredAngle = -desiredAngle;
            }

            desiredAngle = Mathf.Clamp(desiredAngle, -maxAngle, maxAngle);
            float error = desiredAngle - currentAngle;
            float sign = Mathf.Sign(error);
            rotateSpeed += sign * acceleration * Time.deltaTime;

            rotateSpeed = Mathf.MoveTowards(rotateSpeed, 0f, deceleration * Time.deltaTime);

            currentAngle = Mathf.MoveTowards(currentAngle, desiredAngle, Mathf.Abs(rotateSpeed) * Time.deltaTime + 0.1f);
        }

        recoilOffset = Mathf.SmoothDamp(recoilOffset, 0f, ref recoilVelocity, 1f / recoilReturnSpeed);

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

        var baseDir = navigateGuideObject.transform.up;

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
