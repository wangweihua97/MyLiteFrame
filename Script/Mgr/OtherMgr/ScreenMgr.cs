using UnityEngine;

namespace OtherMgr
{
    [ExecuteInEditMode]
    [RequireComponent (typeof(Camera))]
    public class ScreenMgr : MonoBehaviour
    {
        public Shader briSatConShader;
        private Material briSatConMaterial;
        public Material material {  
            get {
                briSatConMaterial = CheckShaderAndCreateMaterial(briSatConShader, briSatConMaterial);
                return briSatConMaterial;
            }  
        }

        [Range(0.0f, 3.0f)]
        public float brightness = 1.0f;

        [Range(0.0f, 3.0f)]
        public float saturation = 1.0f;

        [Range(0.0f, 3.0f)]
        public float contrast = 1.0f;

        void OnRenderImage(RenderTexture src, RenderTexture dest) {
            if (material != null) {
                material.SetFloat("_Brightness", brightness);
                material.SetFloat("_Saturation", saturation);
                material.SetFloat("_Contrast", contrast);

                Graphics.Blit(src, dest, material);
            } else {
                Graphics.Blit(src, dest);
            }
        }
        // Called when start
        protected void CheckResources() {
            bool isSupported = CheckSupport();
		
            if (isSupported == false) {
                NotSupported();
            }
        }

        // Called in CheckResources to check support on this platform
        protected bool CheckSupport() {
            if (SystemInfo.supportsImageEffects == false) {
                Debug.LogWarning("This platform does not support image effects.");
                return false;
            }
		
            return true;
        }

        // Called when the platform doesn't support this effect
        protected void NotSupported() {
            enabled = false;
        }
	
        protected void Start() {
            CheckResources();
        }

        // Called when need to create the material used by this effect
        protected Material CheckShaderAndCreateMaterial(Shader shader, Material material) {
            if (shader == null) {
                return null;
            }
		
            if (shader.isSupported && material && material.shader == shader)
                return material;
		
            if (!shader.isSupported) {
                return null;
            }
            else {
                material = new Material(shader);
                material.hideFlags = HideFlags.DontSave;
                if (material)
                    return material;
                else 
                    return null;
            }
        }
    }
}