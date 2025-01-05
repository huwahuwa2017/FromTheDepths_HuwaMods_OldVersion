using BrilliantSkies.Core.Timing;
using BrilliantSkies.Ftd.Avatar;
using ModManagement;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UniVRM10;

namespace VRM_Loader
{
    public static class CharacterReplacement
    {
        private static AssetBundle _assetsData = null;
        private static AssetBundle _shaderData = null;

        private static bool _modelUpdate = false;
        private static string _status = "Rambot";
        private static GameObject _vrmModel = null;
        private static PlayerSetupBase _preLocalPlayer = null;

        public static AssetBundle GetShaderData()
        {
            return _shaderData;
        }

        public static string[] VRM_FolderConfirmation()
        {
            return Directory.GetFiles(Path.Combine(ModInformation.MyModFolderPath, "VRM"), "*.vrm");
        }

        public static void Replacement(string status)
        {
            _status = status;
            _modelUpdate = true;
        }

        public static void Start()
        {
            Console.WriteLine("VRM_Loader Start");

            _assetsData = AssetBundle.LoadFromFile(Path.Combine(ModInformation.MyModFolderPath, "AssetBundle", "assets"));
            _shaderData = AssetBundle.LoadFromFile(Path.Combine(ModInformation.MyModFolderPath, "AssetBundle", "shader"));

            string[] vrmPathArray = VRM_FolderConfirmation();

            if (vrmPathArray.Length == 0)
            {
                Replacement("Transparent");
            }
            else
            {
                Replacement(vrmPathArray[0]);
            }
        }

        private static void GetChildren(Transform obj, ref List<Transform> allChildren)
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

        private static void RambotEnabledChange(bool enabled)
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

        private static void SetLayerRecursively(GameObject self, int layer)
        {
            self.layer = layer;

            foreach (Transform n in self.transform)
            {
                SetLayerRecursively(n.gameObject, layer);
            }
        }

        private static void DestroyVRMModel()
        {
            UnityEngine.Object.Destroy(_vrmModel);
            _vrmModel = null;
        }

        private static async void LoadAsync()
        {
            string racName = "Assets/UnityChan/Animators/UnityChanLocomotions.controller";
            RuntimeAnimatorController rac = UnityEngine.Object.Instantiate(_assetsData.LoadAsset<RuntimeAnimatorController>(racName));

            Console.WriteLine("Vrm10.LoadPathAsync Start");
            Vrm10Instance vrmInstance = await Vrm10.LoadPathAsync(path: _status, materialGenerator: new VRM_Loader_MaterialDescriptorGenerator());
            Console.WriteLine("Vrm10.LoadPathAsync End");

            DestroyVRMModel();

            _vrmModel = vrmInstance.gameObject;
            _vrmModel.transform.SetParent(StaticPlayers.localPlayer.transform, false);
            _vrmModel.transform.localPosition = new Vector3(0f, 0.1f, 0f);
            _vrmModel.name = "VRM_Model";
            _vrmModel.AddComponent<VRM_Animation>();
            _vrmModel.GetComponent<Animator>().runtimeAnimatorController = rac;

            SetLayerRecursively(_vrmModel, 10);

            RambotEnabledChange(false);
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

            if (localPlayer != _preLocalPlayer)
            {
                _preLocalPlayer = localPlayer;
                _modelUpdate = localPlayer != null;
            }

            if (_modelUpdate)
            {
                Console.WriteLine("VRM_Loader ModelStatus : " + _status);

                if (_status == "Rambot")
                {
                    DestroyVRMModel();
                    RambotEnabledChange(true);
                }
                else if (_status == "Transparent")
                {
                    DestroyVRMModel();
                    RambotEnabledChange(false);
                }
                else
                {
                    LoadAsync();
                }

                _modelUpdate = false;
            }
        }
    }
}
