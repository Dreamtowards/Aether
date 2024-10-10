//RealToon - DeNorSobOutline Effect (HDRP - Post Processing)
//MJQStudioWorks
//©2024

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System;

namespace RealToon.Effects
{

    [Serializable, VolumeComponentMenu("Post-processing/RealToon/DeNorSob Outline")]
    public sealed class DeNorSobOutline : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterOpaqueAndSky;

        const string kShaderName = "Hidden/HDRP/RealToon/Effects/DeNorSobOutline";

        [Space(10)]

        [Header("[RealToon Effects - DeNorSob Outline]")]
        [Header("*Hover your mouse on the option for descriptions/info.")]

        [Space(20)]

        [Tooltip("How thick or thin the outline is.")]
        public MinFloatParameter OutlineWidth = new MinFloatParameter(0f, 0, true);

        [Space(20)]

        [Header("**Depth and Normal Based Outline**")]

        [Tooltip("This will adjust the depth based outline threshold.")]
        public FloatParameter DepthThreshold = new FloatParameter(900.0f, true);

        [Space(10)]

        [Tooltip("This will adjust the normal based outline threshold.")]
        public FloatParameter NormalThreshold = new FloatParameter(1.3f, true);
        [Tooltip("This will adjust the min of the normal to get more normal based outline details.")]
        public FloatParameter NormalMin = new FloatParameter(1.0f, false);
        [Tooltip("This will adjust the max of the normal to get more normal based outline details.")]
        public FloatParameter NormalMax = new FloatParameter(1.0f, false);

        [Space(20)]

        [Header("**Sobel Outline**")]
        [Tooltip("This will render outline all on the screen")]
        public BoolParameter SobelOutline = new BoolParameter(false, false);

        [Tooltip("This will adjust the sobel threshold.\n\n*Sobel Outline is needed to be enabled for this to work.")]
        public MinFloatParameter SobelOutlineThreshold = new MinFloatParameter(0.0f, 0, false);

        [Space(6)]

        [Tooltip("The amount of whites or bright colors to be affected by the outline.\n\n*Sobel Outline is needed to be enabled for this to work.")]
        public FloatParameter WhiteThreshold = new FloatParameter(0.0f, false);

        [Tooltip("The amount of blacks or dark colors to be affected by the outline.\n\n*Sobel Outline is needed to be enabled for this to work.")]
        public FloatParameter BlackThreshold = new FloatParameter(0.0f, false);

        [Space(20)]

        [Header("**Color**")]
        [Tooltip("Outline Color")]
        public ColorParameter OutlineColor = new ColorParameter(Color.black, true);

        [Tooltip("How strong the outline color is.")]
        public FloatParameter ColorIntensity = new FloatParameter(1.0f, true);

        [Tooltip("Mix full screen color image to the outline color.")]
        public BoolParameter MixFullScreenColor = new BoolParameter(false);

        [Space(20)]

        [Header("**Settings**")]
        [Tooltip("Show the outline only.")]
        public BoolParameter ShowOutlineOnly = new BoolParameter(false, true);

        [Space(6)]

        [Tooltip("Mix Depth-Normal Based Outline and Sobel Outline.")]
        public BoolParameter MixDephNormalAndSobelOutline = new BoolParameter(false, true);

        [Space(10)]


        public Material m_Material;

        public bool IsActive() => m_Material != null && OutlineWidth.value > 0f;

        public override void Setup()
        {
            if (Shader.Find(kShaderName) != null)
                m_Material = new Material(Shader.Find(kShaderName));
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (m_Material == null)
                return;

            m_Material.SetFloat("_OutlineWidth", OutlineWidth.value);

            m_Material.SetFloat("_DepthThreshold", DepthThreshold.value);

            m_Material.SetFloat("_NormalThreshold", NormalThreshold.value);
            m_Material.SetFloat("_NormalMin", NormalMin.value);
            m_Material.SetFloat("_NormalMax", NormalMax.value);

            m_Material.SetFloat("_SobOutSel", SobelOutline.value ? 1 : 0);
            m_Material.SetFloat("_SobelOutlineThreshold", SobelOutlineThreshold.value);
            m_Material.SetFloat("_WhiThres", (1.0f - WhiteThreshold.value));
            m_Material.SetFloat("_BlaThres", BlackThreshold.value);

            m_Material.SetColor("_OutlineColor", OutlineColor.value);
            m_Material.SetFloat("_OutlineColorIntensity", ColorIntensity.value);
            m_Material.SetFloat("_ColOutMiSel", MixFullScreenColor.value ? 1 : 0);

            m_Material.SetFloat("_OutOnSel", ShowOutlineOnly.value ? 1 : 0);

            m_Material.SetFloat("_MixDeNorSob", MixDephNormalAndSobelOutline.value ? 1 : 0);

            m_Material.SetTexture("_InputTexture", source);

            switch (SobelOutline.value)
            {
                case true:
                    m_Material.EnableKeyword("RENDER_OUTLINE_ALL");
                    break;
                default:
                    m_Material.DisableKeyword("RENDER_OUTLINE_ALL");
                    break;
            }

            switch (MixDephNormalAndSobelOutline.value)
            {
                case true:
                    m_Material.EnableKeyword("MIX_DENOR_SOB");
                    break;
                default:
                    m_Material.DisableKeyword("MIX_DENOR_SOB");
                    break;
            }

            HDUtils.DrawFullScreen(cmd, m_Material, destination);
        }

        public override void Cleanup() => CoreUtils.Destroy(m_Material);
    }

}
