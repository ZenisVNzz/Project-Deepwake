using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class PickupToss : MonoBehaviour
{
    [Header("Motion")]
    [SerializeField] private float tossDistance = 2f;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private int bounceCount = 2;
    [SerializeField] private float duration = 0.8f;
    [SerializeField] private Ease easeType = Ease.OutQuad;

    [Header("Polish")]
    [SerializeField] private bool fadeInOnLaunch = true;
    [SerializeField] private float fadeInTime = 0.12f;
    [SerializeField] private float squashAmount = 0.08f; 
    [SerializeField] private float spinPerBounce = 140f;
    [SerializeField] private float dirJitterDegrees = 20f; 

    [Header("Shadow (fake height)")]
    [Tooltip("Assign the shadow GameObject (child). If null, will try to find child named 'Shadow'.")]
    [SerializeField] private Transform shadow;
    [SerializeField] private float shadowGroundYOffset = -0.02f;
    [SerializeField] private float shadowScaleAtGround = 1.0f;
    [SerializeField] private float shadowScaleAtApex = 0.6f;
    [SerializeField] private float shadowAlphaAtGround = 0.55f;
    [SerializeField] private float shadowAlphaAtApex = 0.15f;

    private SpriteRenderer sr;
    private SpriteRenderer shadowSr;
    private DG.Tweening.Sequence seq; 
    private Vector3 baseScale;
    private Vector3 baseShadowScale;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        baseScale = transform.localScale;
        EnsureShadowRefs();
    }

    private void OnEnable()
    {
        baseScale = transform.localScale;
        EnsureShadowRefs();
    }

    private void OnDisable()
    {
        seq?.Kill();
        seq = null;

        transform.DOKill();
        sr?.DOKill();

        transform.localScale = baseScale;

        if (shadow != null)
        {
            shadow.DOKill();
            shadowSr?.DOKill();
            shadow.localScale = baseShadowScale * shadowScaleAtGround;
            SetShadowAlpha(shadowAlphaAtGround);
        }
    }

    public void Launch(Vector2 fromPos)
    {
        Launch(fromPos, null, null);
    }

    public void Launch(Vector2 fromPos, Vector2? preferredDir, Action onComplete)
    {
        EnsureShadowRefs();
        transform.position = fromPos;
        transform.localScale = baseScale;

        transform.DOKill();
        sr?.DOKill();
        if (shadow != null)
        {
            shadow.DOKill();
            shadowSr?.DOKill();
        }

        seq?.Kill();
        seq = DOTween.Sequence().SetLink(gameObject);

        Vector2 dir = preferredDir.HasValue && preferredDir.Value.sqrMagnitude > 0.0001f
            ? preferredDir.Value.normalized
            : UnityEngine.Random.insideUnitCircle.normalized;

        if (dirJitterDegrees > 0f)
        {
            float jitter = UnityEngine.Random.Range(-dirJitterDegrees, dirJitterDegrees);
            float rad = jitter * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);
            dir = new Vector2(dir.x * cos - dir.y * sin, dir.x * sin + dir.y * cos).normalized;
        }

        Vector3 finalTarget = (Vector2)fromPos + dir * tossDistance;

        if (shadow != null)
        {
            baseShadowScale = shadow.localScale;
            shadow.position = new Vector3(fromPos.x, fromPos.y + shadowGroundYOffset, shadow.position.z);
            shadow.localScale = baseShadowScale * shadowScaleAtGround;
            SetShadowAlpha(fadeInOnLaunch ? 0f : shadowAlphaAtGround);
        }

        if (fadeInOnLaunch && sr != null)
        {
            Color c = sr.color;
            c.a = 0f;
            sr.color = c;
            seq.Insert(0f, sr.DOFade(1f, fadeInTime));
        }
        if (fadeInOnLaunch && shadowSr != null)
        {
            seq.Insert(0f, shadowSr.DOFade(shadowAlphaAtGround, fadeInTime));
        }

        int bCount = Mathf.Max(1, bounceCount);
        float minSegTime = Mathf.Max(0.1f, duration / (bCount * 2f));

        for (int i = 0; i < bCount; i++)
        {
            float startProgress = i / (float)bCount;
            float endProgress = (i + 1f) / bCount;

            Vector3 segmentStart = Vector2.Lerp(fromPos, finalTarget, startProgress);
            Vector3 segmentEnd = Vector2.Lerp(fromPos, finalTarget, endProgress);
            var landing = segmentEnd; 

            float inv = 1f - (i / (float)bCount);
            float segHeight = Mathf.Max(0.01f, jumpHeight * inv);
            float segTime = Mathf.Max(minSegTime, duration * (0.45f + 0.55f * inv));

            seq.Append(transform
                .DOJump(segmentEnd, segHeight, 1, segTime)
                .SetEase(easeType));

            if (Mathf.Abs(spinPerBounce) > 0.01f)
            {
                seq.Join(transform
                    .DORotate(new Vector3(0, 0, spinPerBounce * Mathf.Sign(UnityEngine.Random.Range(-1f, 1f))), segTime, RotateMode.FastBeyond360)
                    .SetEase(Ease.OutCubic));
            }

            if (shadow != null)
            {
                float t = 0f;
                var tracker = DOTween.To(() => t, x => t = x, 1f, segTime)
                    .SetEase(Ease.Linear)
                    .OnUpdate(() =>
                    {
 
                        Vector2 baseline = Vector2.Lerp(segmentStart, segmentEnd, t);
                        Vector3 itemPos = transform.position;

                        float height = Mathf.Max(0f, itemPos.y - baseline.y);
                        float hNorm = Mathf.Clamp01(height / Mathf.Max(0.0001f, segHeight));

                        shadow.position = new Vector3(baseline.x, baseline.y + shadowGroundYOffset, shadow.position.z);

                        float s = Mathf.Lerp(shadowScaleAtGround, shadowScaleAtApex, hNorm);
                        shadow.localScale = baseShadowScale * s;

                        if (shadowSr != null)
                        {
                            float a = Mathf.Lerp(shadowAlphaAtGround, shadowAlphaAtApex, hNorm);
                            SetShadowAlpha(a);
                        }
                    });
                seq.Join(tracker.SetLink(gameObject));

                seq.AppendCallback(() =>
                {                   
                    shadow.localScale = baseShadowScale * shadowScaleAtGround;
                    SetShadowAlpha(shadowAlphaAtGround);
                });
            }

            if (squashAmount > 0f)
            {
                float squashTime = Mathf.Min(0.08f, segTime * 0.25f);
                Vector3 squashScale = new Vector3(
                    baseScale.x * (1f + squashAmount),
                    baseScale.y * (1f - squashAmount),
                    baseScale.z
                );

                seq.Append(transform.DOScale(squashScale, squashTime));
                seq.Append(transform.DOScale(baseScale, squashTime));
            }
        }

        seq.Append(transform.DOScale(baseScale * 1.05f, 0.08f).SetLoops(2, LoopType.Yoyo));

        if (shadow != null)
        {
            seq.AppendCallback(() =>
            {
                shadow.localScale = baseShadowScale * shadowScaleAtGround;
                SetShadowAlpha(shadowAlphaAtGround);
            });
        }

        seq.OnKill(() =>
        {
            transform.localScale = baseScale;
            if (sr != null)
            {
                var c = sr.color; c.a = 1f; sr.color = c;
            }
            if (shadow != null)
            {
                shadow.localScale = baseShadowScale * shadowScaleAtGround;
                SetShadowAlpha(shadowAlphaAtGround);
            }
        });

        if (onComplete != null)
            seq.OnComplete(() => onComplete());
    }

    private void EnsureShadowRefs()
    {
        if (shadow == null)
        {
            var found = transform.Find("Shadow");
            if (found != null) shadow = found;
        }
        if (shadow != null && shadowSr == null)
        {
            shadowSr = shadow.GetComponent<SpriteRenderer>();
            baseShadowScale = shadow.localScale;
        }
    }

    private void SetShadowAlpha(float a)
    {
        if (shadowSr == null) return;
        var c = shadowSr.color;
        c.a = a;
        shadowSr.color = c;
    }
}
