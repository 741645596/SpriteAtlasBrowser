using UnityEngine;
using UnityEngine.Rendering;

public enum CameraCmdBuffer
{
    DRAW_RENDERER,
    DRAW_RENDERER_TARGET,
    DRAW_MESH,
    DRAW_MESH_TARGET
}

public class Graphics05CmdBufferCamera : MonoBehaviour
{
    public Material cmdMat;
    public Renderer cubeRenderer;
    public RenderTexture target;
    public Color clearColor = Color.red;
    public int triCount = 6;
    public float radius = 5;

    private CommandBuffer cmdBuffer;
    private Mesh mesh;

    public void DrawRenderer()
    {
        cmdBuffer.Clear();
        cmdBuffer.DrawRenderer(cubeRenderer, cmdMat);
    }

    public void DrawRendererToTarget()
    {
        cmdBuffer.Clear();
        cmdBuffer.SetRenderTarget(target);
        cmdBuffer.ClearRenderTarget(true, true, clearColor);
        cmdBuffer.DrawRenderer(cubeRenderer, cmdMat);
    }

    public void DrawMesh()
    {
        cmdBuffer.Clear();
        cmdBuffer.DrawMesh(mesh, Matrix4x4.identity, cmdMat);
    }

    public void DrawMeshToTarget()
    {
        cmdBuffer.Clear();
        cmdBuffer.SetRenderTarget(target);
        cmdBuffer.ClearRenderTarget(true, true, clearColor);

        cmdBuffer.DrawMesh(mesh, Matrix4x4.identity, cmdMat);
    }

    private void Start()
    {
        cmdBuffer = new CommandBuffer() { name = "CameraCmdBuffer" };

        Camera.main.AddCommandBuffer(CameraEvent.AfterForwardOpaque, cmdBuffer);

        if (mesh == null)
        {
            mesh = Graphics00Mesh.Instance.GetMesh(triCount, radius);
        }
    }

    private void OnValidate()
    {
        mesh = Graphics00Mesh.Instance.GetMesh(triCount, radius);
    }

    private void OnDisable()
    {
        Camera.main.RemoveAllCommandBuffers();
    }
}
