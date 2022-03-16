using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using StarterAssets;
using UnityEngine.Animations.Rigging;

public class ThirdPersonShootingController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private GameObject reticle;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private GameObject debugTransform;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private float aimAnimationTransitionSpeed;
    [SerializeField] private AudioSource shootAudioSource;
    [SerializeField] private Rig rig;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetInputs;
    private Animator animator;


    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        Transform hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.transform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
        }

        if (starterAssetInputs.aim)
        {
            rig.weight = 1;
            aimVirtualCamera.Priority = 12;
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * aimAnimationTransitionSpeed));
            reticle.SetActive(true);

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            //transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 10f);

            if (starterAssetInputs.shoot)
                Shoot();
            
        } else
        {
            rig.weight = 0;
            reticle.SetActive(false);
            aimVirtualCamera.Priority = 8;
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * aimAnimationTransitionSpeed));
        }

        void Shoot()
        {
           Debug.Log("Shoot");
           shootAudioSource.PlayOneShot(shootAudioSource.clip);

           if (hitTransform != null)
           {
                Enemy enemy = hitTransform.GetComponent<Enemy>();
                if (enemy != null)
                {
                Debug.Log("Hit Enemy");
                enemy.TakeDamage(10);
                 }
            }
                starterAssetInputs.shoot = false;
        }
    }

}
