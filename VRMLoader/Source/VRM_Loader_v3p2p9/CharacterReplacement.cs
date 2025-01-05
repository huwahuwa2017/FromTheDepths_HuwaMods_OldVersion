using BrilliantSkies.Core.Timing;
using BrilliantSkies.Ftd.Avatar;
using HuwaTech;
using ModManagement;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VRM;

namespace VRM_Loader
{
    public static class CharacterReplacement
    {
        private static GameObject AssetGameObject;

        private static Action Testtest;

        private static bool ModelUpdate;

        private static PlayerSetupBase PreLocalPlayer;



        public static AssetBundle AssetData { get; private set; }

        public static void Start()
        {
            Console.WriteLine("VRM_Loader Start");

            AssetData = AssetBundle.LoadFromFile(Path.Combine(ModInformation.MyModFolderPath, "AssetBundle", "assets"));
            ShaderLoader.Load(AssetData);

            string[] VRM_PathArray = VRM_FolderConfirmation();

            if (VRM_PathArray.Length == 0)
            {
                Replacement("Transparent");
            }
            else
            {
                Replacement(VRM_PathArray[0]);
            }
        }

        public static void Update(ITimeStep t)
        {
            I_All_cInterface clientInterface = ClientInterface.GetInterface();

            if (clientInterface != null)
            {
                Camera fCamera = CameraManager.me.foregroundCamera;
                Camera bCamera = CameraManager.me.backgroundCamera;

                bool isFirstPerson = clientInterface.Get_I_world_cCameraControl().GetCameraState() == enumCameraState.firstPerson;

                if (fCamera != null)
                {
                    if (isFirstPerson)
                    {
                        fCamera.cullingMask &= ~(1 << 10);
                    }
                    else
                    {
                        fCamera.cullingMask |= (1 << 10);
                    }
                }

                if (bCamera != null)
                {
                    if (isFirstPerson)
                    {
                        bCamera.cullingMask &= ~(1 << 10);
                    }
                    else
                    {
                        bCamera.cullingMask |= (1 << 10);
                    }
                }
            }



            PlayerSetupBase localPlayer = StaticPlayers.localPlayer;

            if (localPlayer != null && Testtest != null && ModelUpdate)
            {
                Testtest();
                ModelUpdate = false;
            }

            if (localPlayer != PreLocalPlayer)
            {
                PreLocalPlayer = localPlayer;
                ModelUpdate = true;
            }
        }

        public static void Replacement(string status)
        {
            ModelUpdate = true;

            Testtest = () =>
            {
                VRM_ModelDestroy();

                Console.WriteLine("VRM_Loader ModelStatus : " + status);

                if (status == "Rambot")
                {
                    RambotEnabledChange(true);
                }
                else if (status == "Transparent")
                {
                    RambotEnabledChange(false);
                }
                else
                {
                    AssetGameObject = VRM_Load(status);

                    string racName = "Assets/UnityChan/Animators/UnityChanLocomotions.controller";
                    RuntimeAnimatorController rac = UnityEngine.Object.Instantiate(AssetData.LoadAsset<RuntimeAnimatorController>(racName));

                    AssetGameObject.transform.SetParent(StaticPlayers.localPlayer.transform, false);
                    AssetGameObject.transform.localPosition = new Vector3(0, 0.2f, 0);
                    AssetGameObject.name = "VRM_Model";
                    AssetGameObject.AddComponent<VRM_Animation>();
                    AssetGameObject.GetComponent<Animator>().runtimeAnimatorController = rac;

                    SetLayerRecursively(AssetGameObject, 10);

                    RambotEnabledChange(false);
                }
            };
        }

        public static void VRM_ModelDestroy()
        {
            UnityEngine.Object.Destroy(AssetGameObject);
        }

        public static string[] VRM_FolderConfirmation()
        {
            return Directory.GetFiles(Path.Combine(ModInformation.MyModFolderPath, "VRM"), "*.vrm");
        }

        public static GameObject VRM_Load(string path)
        {
            VRMImporterContext context = new VRMImporterContext();
            context.Load(path);
            context.ShowMeshes();
            return context.Root;
        }

        public static void SetLayerRecursively(GameObject self, int layer)
        {
            self.layer = layer;

            foreach (Transform n in self.transform)
            {
                SetLayerRecursively(n.gameObject, layer);
            }
        }

        public static void RambotEnabledChange(bool enabled)
        {
            PlayerSetupBase localPlayer = StaticPlayers.localPlayer;

            if (localPlayer != null)
            {
                List<string> nameList = new List<string>() { "head", "body", "rocketR", "rocketL", "right upper arm", "left upper arm",
                    "right arm", "left arm","Object006", "Object007", "Object008", "Object009", "EngineRight", "EngineLeft", "headcam" };

                List<Transform> transformList = new List<Transform>();
                GetChildren(localPlayer.transform, ref transformList);

                foreach (Transform child in transformList)
                {
                    string name = child.name;

                    foreach (string targetName in nameList)
                    {
                        if (name == targetName)
                        {
                            Renderer mr = child.GetComponent<Renderer>();

                            if (mr != null)
                            {
                                mr.enabled = enabled;
                            }
                        }
                    }
                }
            }
        }

        public static void GetChildren(Transform obj, ref List<Transform> allChildren)
        {
            Transform children = obj.GetComponentInChildren<Transform>(true);

            if (children.childCount == 0)
            {
                return;
            }

            foreach (Transform ob in children)
            {
                allChildren.Add(ob);
                GetChildren(ob, ref allChildren);
            }
        }
    }
}
