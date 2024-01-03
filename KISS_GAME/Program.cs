using System;
using System.IO;
using System.Linq;
using KISS_MODEL_VIEWER;
using OpenTK.Windowing.Desktop;

namespace KISS_MODEL_VIEWER
{
    class Program
    {
        const string PROJECT_NAME = "ThePheonixProject";
        const string CUBE_STL_FILENAME = "20mm_cube.stl";
        const string TEAPOT_STL_FILENAME = "Utah_teapot_(solid).stl";


        static void Main(string[] args)
        {
            string currentProjectLocation = Directory.GetCurrentDirectory();
            //int indexFolderName = currentProjectLocation.IndexOf(PROJECT_NAME);
            //string projectPath = currentProjectLocation.Substring(0, indexFolderName + PROJECT_NAME.Length);

            //string filePath = Path.Combine(projectPath, TEAPOT_STL_FILENAME);


            //Triangle[] triangles = STLParser.ReadToTriangles(filePath);

            using (ViewerWindow viewerWindow = new ViewerWindow(800, 600, "Model Viewer"))
            {
                viewerWindow.Run();
            }
        }
    }
}
