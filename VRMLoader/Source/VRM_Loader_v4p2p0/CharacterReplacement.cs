using BrilliantSkies.Core.Timing;
using BrilliantSkies.Ftd.Avatar;
using BrilliantSkies.Ftd.Avatar.Movement;
using ModManagement;
using System;
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
        private static string _status = "DefaultAvatar";
        private static GameObject _vrmModel = null;
        private static PlayerSetupBase _preLocalPlayer = null;

        private static bool _defaultAvatarSetActive = true;
        private static ModelSpawnData _modelSpawnData = null;

        public static void ModelUpdate()
        {
            _modelUpdate = true;
        }

        public static void SetModelSpawnData(ModelSpawnData modelSpawnData)
        {
            _modelSpawnData = modelSpawnData;
        }

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

        public static void DefaultAvatarSetActive()
        {
            Console.WriteLine("DefaultAvatarSetActive");

            Transform mainTransform = _modelSpawnData.Transform;
            Transform[] temp2 = mainTransform.GetComponentsInChildren<Transform>(true);

            foreach (Transform temp3 in temp2)
            {
                if (temp3 == mainTransform)
                    continue;

                Renderer[] temp4 = temp3.GetComponentsInChildren<Renderer>(true);

                foreach (Renderer temp5 in temp4)
                {
                    temp5.enabled = _defaultAvatarSetActive;
                }
            }
        }

        private static void DefaultAvatarSetActive(bool value)
        {
            _defaultAvatarSetActive = value;
            DefaultAvatarSetActive();
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
            _vrmModel.name = "VRM_Model";
            _vrmModel.AddComponent<VRM_Animation>();
            _vrmModel.GetComponent<Animator>().runtimeAnimatorController = rac;

            Transform[] temp0 = _vrmModel.GetComponentsInChildren<Transform>(true);

            foreach (Transform temp1 in temp0)
            {
                temp1.gameObject.layer = 10;
            }

            DefaultAvatarSetActive(false);
        }

        public static void Update(ITimeStep t)
        {
            cCameraControl temp0 = cCameraControl.Instance;

            if (temp0 != null)
            {
                Camera fCamera = CameraManager.me.foregroundCamera;
                Camera bCamera = CameraManager.me.backgroundCamera;

                bool isFirstPerson = temp0.IsFirstPerson;

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



            if ((_modelSpawnData != null) && (_modelSpawnData.Transform != null) && (_vrmModel != null))
            {
                Transform modelTransform = _modelSpawnData.Transform;
                _vrmModel.transform.SetParent(modelTransform, false);

                CapsuleCollider capsuleCollider = modelTransform.GetComponent<CapsuleCollider>();

                if (capsuleCollider != null)
                {
                    float offset = capsuleCollider.center.y - capsuleCollider.height * 0.5f;
                    offset *= modelTransform.localScale.y;
                    _vrmModel.transform.localPosition = new Vector3(0f, offset, 0f);
                }

                /*
                CapsuleCollider temp7 = _modelSpawnData.Transform.GetComponent<CapsuleCollider>();

                string text = "";

                text += "name : " + temp7.name + "\n";
                text += "center : " + temp7.center.ToString() + "\n";
                text += "radius : " + temp7.radius.ToString() + "\n";
                text += "height : " + temp7.height.ToString() + "\n";
                text += "localScale : " + _modelSpawnData.Transform.localScale.ToString() + "\n";

                Console.WriteLine(text);
                */
            }



            if (_modelUpdate)
            {
                Console.WriteLine("VRM_Loader ModelStatus : " + _status);

                if (_status == "DefaultAvatar")
                {
                    DestroyVRMModel();
                    DefaultAvatarSetActive(true);
                }
                else if (_status == "Transparent")
                {
                    DestroyVRMModel();
                    DefaultAvatarSetActive(false);
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
