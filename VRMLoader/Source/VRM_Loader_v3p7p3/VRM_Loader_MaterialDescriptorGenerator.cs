using System;
using System.Collections.Generic;
using System.Reflection;
using UniGLTF;
using UnityEngine;
using UniVRM10;
using VRMShaders;

namespace VRM_Loader
{
    // BuiltInVrm10MaterialDescriptorGeneratorが元になっている
    public sealed class VRM_Loader_MaterialDescriptorGenerator : IMaterialDescriptorGenerator
    {
        private static readonly string _mtoon_assetName = "Assets/VRMShaders/VRM10/MToon10/Resources/VRM10/vrmc_materials_mtoon.shader";

        private static readonly string _uniUnlit_assetName = "Assets/VRMShaders/GLTF/UniUnlit/Resources/UniGLTF/UniUnlit.shader";

        private static readonly FieldInfo _materialDescriptor_Shader = typeof(MaterialDescriptor).GetField("Shader", BindingFlags.Instance | BindingFlags.Public);

        public MaterialDescriptor Get(GltfData data, int i)
        {
            Console.WriteLine("VRM_Loader_MaterialDescriptorGenerator.Get");

            AssetBundle asset = CharacterReplacement.GetShaderData();

            // mtoon
            if (BuiltInVrm10MToonMaterialImporter.TryCreateParam(data, i, out MaterialDescriptor matDesc))
            {
                _materialDescriptor_Shader.SetValue(matDesc, asset.LoadAsset<Shader>(_mtoon_assetName));
                return matDesc;
            }

            // unlit
            if (BuiltInGltfUnlitMaterialImporter.TryCreateParam(data, i, out matDesc))
            {
                _materialDescriptor_Shader.SetValue(matDesc, asset.LoadAsset<Shader>(_uniUnlit_assetName));
                return matDesc;
            }

            // pbr
            if (BuiltInGltfPbrMaterialImporter.TryCreateParam(data, i, out matDesc))
            {
                return matDesc;
            }

            // fallback
            if (Symbols.VRM_DEVELOP)
            {
                Debug.LogWarning($"material: {i} out of range. fallback");
            }

            return new MaterialDescriptor(
                GltfMaterialImportUtils.ImportMaterialName(i, null),
                BuiltInGltfPbrMaterialImporter.Shader,
                null,
                new Dictionary<string, TextureDescriptor>(),
                new Dictionary<string, float>(),
                new Dictionary<string, Color>(),
                new Dictionary<string, Vector4>(),
                new Action<Material>[] { });
        }

        public MaterialDescriptor GetGltfDefault()
        {
            return BuiltInGltfDefaultMaterialImporter.CreateParam();
        }
    }
}
