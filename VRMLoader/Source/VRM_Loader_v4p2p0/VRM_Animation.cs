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

                // if (cameraState == enumCameraState.firstPerson || cameraState == enumCameraState.fixedOutside)
                if (cameraState == enumCameraState.firstPerson)
                {
                    bool F1 = keyMap.Bool(KeyInputsFtd.MoveForward, KeyInputEventType.Held);
                    bool F2 = keyMap.Bool(KeyInputsFtd.MoveLeft, KeyInputEventType.Held);
                    bool F3 = keyMap.Bool(KeyInputsFtd.MoveBackward, KeyInputEventType.Held);
                    bool F4 = keyMap.Bool(KeyInputsFtd.MoveRight, KeyInputEventType.Held);

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
