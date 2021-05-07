using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SmallMap : MonoBehaviour
{
    private const float _displayRatio = 0.15f;
    // Start is called before the first frame update
    private CommandBuffer cmdBuffer = null;
    public Material cmdMat;
    public Renderer cubeRenderer;
    public MeshFilter mf;
    public RenderTexture target;
    public Color clearColor = Color.red;

    void Start()
    {
        cmdBuffer = new CommandBuffer() { name = "cameraCmdBuffer" };
    }

    // Update is called once per frame
    void Update()
    {
    }


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
        cmdBuffer.DrawMesh(mf.mesh, Matrix4x4.identity, cmdMat);
    }

    public void DrawMeshToTarget()
    {
        cmdBuffer.Clear();
        cmdBuffer.SetRenderTarget(target);
        cmdBuffer.ClearRenderTarget(true, true, clearColor);

        cmdBuffer.DrawMesh(mf.mesh, Matrix4x4.identity, cmdMat);
    }



    public bool Render(Camera camera, Material blitingMaterial)
    { 
        CommandBuffer command = CommandBufferPool.Get("RenderDebugRTs");

        command.SetViewProjectionMatrices(Matrix4x4.identity, Matrix4x4.identity);

        Vector2 RectSize = new Vector2(camera.pixelRect.width * _displayRatio,
            camera.pixelRect.height * _displayRatio);

        Rect displayRect = new Rect(0.0f, 0.0f, RectSize.x, RectSize.y);

        displayRect.x = (RectSize.x + 0) * 0;
        displayRect.y = camera.pixelRect.height - ((RectSize.y + 0) * 0 + RectSize.y);

        command.SetViewport(displayRect);

        command.DrawMesh(fullscreenMesh, Matrix4x4.identity, blitingMaterial);

        CommandBufferPool.Release(command);
        return true;
    }



    private Mesh _fullscreenMesh = null;
    private Mesh fullscreenMesh
    {
        get
        {
            if (_fullscreenMesh != null)
                return _fullscreenMesh;

            float topV = 1.0f;
            float bottomV = 0.0f;

            _fullscreenMesh = new Mesh { name = "Fullscreen Quad" };
            _fullscreenMesh.SetVertices(new List<Vector3>
                {
                    new Vector3(-1.0f, -1.0f, 0.0f),
                    new Vector3(-1.0f,  1.0f, 0.0f),
                    new Vector3(1.0f, -1.0f, 0.0f),
                    new Vector3(1.0f,  1.0f, 0.0f)
                });

            _fullscreenMesh.SetUVs(0, new List<Vector2>
                {
                    new Vector2(0.0f, bottomV),
                    new Vector2(0.0f, topV),
                    new Vector2(1.0f, bottomV),
                    new Vector2(1.0f, topV)
                });

            _fullscreenMesh.SetIndices(new[] { 0, 1, 2, 2, 1, 3 }, MeshTopology.Triangles, 0, false);
            _fullscreenMesh.UploadMeshData(true);
            return _fullscreenMesh;
        }
    }
}

