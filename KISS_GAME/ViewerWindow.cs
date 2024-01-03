using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace KISS_MODEL_VIEWER
{
    internal class ViewerWindow: GameWindow
    {
        const string SHADERS_FOLDER_NAME = "Shaders";
        const string VERTEX_SHADER_FILE_NAME = "VertexShader.glsl";
        const string FRAGMENT_SHADER_FILE_NAME = "FragmentShader.glsl";

        float[] vertices = {
            -0.5f, -0.5f, 0.0f, //Bottom-left vertex
             0.5f, -0.5f, 0.0f, //Bottom-right vertex
             0.0f,  0.5f, 0.0f  //Top vertex
        };

        Shader shader;
        int VertexBufferObject;
        int FragmentBufferObject;
        int VertexArrayObject;

        public ViewerWindow(int width, int height, string title) :
            base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title})
        {

        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnLoad()
        {
            string currentProjectLocation = Directory.GetCurrentDirectory();
            int indexFolderName = currentProjectLocation.IndexOf("KISS_GAME");
            string projectPath = currentProjectLocation.Substring(0, indexFolderName + "KISS_GAME".Length);
            projectPath = Path.Combine(projectPath, SHADERS_FOLDER_NAME);

            string vertexShaderPath = Path.Combine(projectPath, VERTEX_SHADER_FILE_NAME);
            string fragmentShaderPath = Path.Combine(projectPath, FRAGMENT_SHADER_FILE_NAME);



            base.OnLoad();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            shader = new Shader(vertexShaderPath, fragmentShaderPath);
            shader.Use();
        }
        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteVertexArray(VertexArrayObject);

            shader.Dispose();
            base.OnUnload();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        { 
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            shader.Use();
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0,0, e.Width, e.Height);
        }
    }
}
