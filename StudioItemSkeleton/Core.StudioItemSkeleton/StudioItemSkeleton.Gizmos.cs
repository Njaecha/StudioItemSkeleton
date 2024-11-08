using System.Collections.Generic;
using System.Linq;
using KKAPI.Studio;
using Studio;
using UnityEngine;
using UnityEngine.Rendering;

namespace StudioItemSkeleton
{
    internal class StudioItemSkeletonGizmos: MonoBehaviour
    {
        private static readonly int SrcBlend = Shader.PropertyToID("_SrcBlend");
        private static readonly int DstBlend = Shader.PropertyToID("_DstBlend");
        private static readonly int Cull = Shader.PropertyToID("_Cull");
        private static readonly int ZWrite = Shader.PropertyToID("_ZWrite");
        private static readonly int ZTest = Shader.PropertyToID("_ZTest");
        private Material _gizmoMaterial;

        private void Awake()
        {
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            _gizmoMaterial = new Material(shader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            
            // Turn on alpha blending
            _gizmoMaterial.SetInt(SrcBlend, (int)BlendMode.SrcAlpha);
            _gizmoMaterial.SetInt(DstBlend, (int)BlendMode.OneMinusSrcAlpha);
            // Always draw
            _gizmoMaterial.SetInt(Cull, (int)CullMode.Off);
            _gizmoMaterial.SetInt(ZWrite, 0);
            _gizmoMaterial.SetInt(ZTest, (int)CompareFunction.Always);
        }

        private void OnPostRender()
        {
            _gizmoMaterial.SetPass(0);
            GL.PushMatrix();
            GL.MultMatrix(Matrix4x4.identity);
            
            StudioAPI.GetSelectedObjects().ToList().ForEach(oci =>
            {
                if (!(oci is OCIItem item) || !item.itemFKCtrl.isActiveAndEnabled) return;
                GL.Begin(GL.LINES);
                GL.Color(StudioItemSkeleton.GizmoColor.Value);
                if (item.listBones.IsNullOrEmpty())
                {
                    GL.End();
                    return;
                }
                List<Transform> goBones = item.listBones.Select(bone => bone?.guideObject.transformTarget).ToList();
                item.listBones.ForEach(bone =>
                {
                    if (bone == null || !bone.guideObject || !bone.guideObject.transformTarget) return;
                    Transform parent = bone.guideObject.transformTarget.parent;
                    if (!parent) return;
                    while (!goBones.Contains(parent) && parent)
                    {
                        parent = parent.parent;
                        if (!parent || parent == item.objectItem.transform) return;
                    }
                    GL.Vertex(bone.guideObject.transformTarget.position);
                    GL.Vertex(parent.position);
                });
                GL.End();
            });
            
            GL.PopMatrix();
        }
    }
}