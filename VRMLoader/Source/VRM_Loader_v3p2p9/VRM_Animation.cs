using BrilliantSkies.Ftd.Avatar;
using BrilliantSkies.PlayerProfiles;
using UnityEngine;

namespace VRM_Loader
{
    public class VRM_Animation : MonoBehaviour
    {
        private Animator animator;

        private FtdKeyMap keyMap;

        void Start()
        {
            animator = GetComponent<Animator>();
            keyMap = ProfileManager.Instance.GetModule<FtdKeyMap>();
        }

        void Update()
        {
            I_All_cInterface clientInterface = ClientInterface.GetInterface();

            if (clientInterface != null)
            {
                enumCameraState cameraState = clientInterface.Get_I_world_cCameraControl().GetCameraState();
                float speed = 0;

                if (cameraState == enumCameraState.firstPerson || cameraState == enumCameraState.fixedOutside)
                {
                    bool F1 = keyMap.IsKey(KeyInputsFtd.W, KeyInputEventType.Held, ModifierAllows.AllowUnnecessary);
                    bool F2 = keyMap.IsKey(KeyInputsFtd.A, KeyInputEventType.Held, ModifierAllows.AllowUnnecessary);
                    bool F3 = keyMap.IsKey(KeyInputsFtd.S, KeyInputEventType.Held, ModifierAllows.AllowUnnecessary);
                    bool F4 = keyMap.IsKey(KeyInputsFtd.D, KeyInputEventType.Held, ModifierAllows.AllowUnnecessary);

                    if (F1 || F2 || F4)
                    {
                        speed += 1;
                    }

                    if (F3)
                    {
                        speed -= 1;
                    }
                }

                animator.SetFloat("Speed", speed);
            }
        }
    }
}
