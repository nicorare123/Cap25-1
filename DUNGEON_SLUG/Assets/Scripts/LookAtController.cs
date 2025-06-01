using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtController : MonoBehaviour
{
    public Animator animator;

    public bool holdUp;
    public bool holdDown;

    public PlayerMove playerMove;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        //upperBodyLookTarget.SetParent(transform);
        initialUpperBodyLookTargetLocalPos = Target.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        holdUp = playerMove.holdUp;
        holdDown = playerMove.holdDown;
    }

    public Transform Target;
    [Range(0, 1)] public float upperBodyLookWeight = 1.0f;
    [Range(0, 1)] public float bodyWeight = 0.5f;
    [Range(0, 1)] public float headWeight = 1.0f;
    [Range(0, 1)] public float clampWeight = 0.5f;

    public float lookVerticalRange = 0.5f;
    public float lookSmoothTime = 0.1f;

    private Vector3 initialUpperBodyLookTargetLocalPos;
    private Vector3 currentLookTargetVelocity;


    void OnAnimatorIK(int layerIndex)
    {
        float facing = Mathf.Sign(playerMove.transform.localScale.x); // +1 또는 -1

        Vector3 offset = Vector3.zero;

        if (holdUp)
        {
            offset = Vector3.up * lookVerticalRange;
        }
        else if (holdDown)
        {
            offset = Vector3.down * lookVerticalRange;
        }

        // x 방향을 캐릭터 좌우 반전에 따라 반전
        offset.x *= facing;

        Vector3 targetLocalPosition = initialUpperBodyLookTargetLocalPos + offset;
        targetLocalPosition.z = initialUpperBodyLookTargetLocalPos.z;

        Target.localPosition = Vector3.SmoothDamp(Target.localPosition, targetLocalPosition, ref currentLookTargetVelocity, lookSmoothTime);

        // 최종 LookAt 적용
        animator.SetLookAtWeight(upperBodyLookWeight, bodyWeight, headWeight, 0f, clampWeight);
        animator.SetLookAtPosition(Target.position);
    }
}
