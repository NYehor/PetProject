using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace KISS_MODEL_VIEWER
{
    public static class STLParser
    {
        /// <summary>
        ///     This method read ASCII and binnary stl file.
        /// </summary>
        /// <param name="path">The location of stl file.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// 


        // Key words for ASCII format file.
        private static string SOLID = "solid ";
        private static string FACET_NORMAL = "facet normal ";
        private static string OUTERLOOP = "outer loop";
        private static string VERTEX = "vertex ";
        private static string ENDLOOP = "endloop";
        private static string ENDFACET = "endfacet";
        private static string ENDSOLID = "endsolid ";

        public static Triangle[] ReadToTriangles(string path)
        {
            if (!File.Exists(path)) 
            {
                throw new FileNotFoundException("The program cannot find the file!");
            }

            byte[] file = File.ReadAllBytes(path);
            byte[] binaryHeader = new byte[80];
            Array.Copy(file, binaryHeader, 80);

            string headerFile = Encoding.UTF8.GetString(binaryHeader);

            if (headerFile.Contains("solid"))
            {
                return ReadASCIIFile(Encoding.UTF8.GetString(file));
            }
            else
            {
                return ReadBinaryFile(file);
            }

        }

        private static Triangle[] ReadASCIIFile(string file)
        {
            bool isLineCorrect(string line, string keyword)
            {
                if (line != null && line.Contains(keyword))
                {
                    return true;
                }
                else
                {
                    throw new Exception("The file format is invalid.");
                    return false;
                }
            }

            Vector readVector(string line, string keyWord)
            {
                isLineCorrect(line, keyWord);

                string vector = line.Substring(keyWord.Length);
                string[] vertexArray = vector.Replace('.',',').Split(" ");
                
                return new Vector(float.Parse(vertexArray[0]), float.Parse(vertexArray[1]), float.Parse(vertexArray[2]));
            }


            List<Triangle> result = new List<Triangle>();
            StringReader srFile = new StringReader(file);

            string line = srFile.ReadLine();

            if (isLineCorrect(line, SOLID))
            {
                string fileName = line.Substring(SOLID.Length);
            }

            while(!(line = srFile.ReadLine()).Contains(ENDSOLID) && line != null)
            {
                Triangle triangle = new Triangle();

                triangle.Normal = readVector(line, FACET_NORMAL);
                isLineCorrect(srFile.ReadLine(), OUTERLOOP);
                triangle.A = readVector(srFile.ReadLine(), VERTEX);
                triangle.B = readVector(srFile.ReadLine(), VERTEX);
                triangle.C = readVector(srFile.ReadLine(), VERTEX);
                isLineCorrect(srFile.ReadLine(), ENDLOOP);
                isLineCorrect(srFile.ReadLine(), ENDFACET);

                result.Add(triangle);
            }

            return result.ToArray();
        }

        private static Triangle[] ReadBinaryFile(byte[] file)
        {
            int currentIndex = 80;
            uint amountOfTriangle = BitConverter.ToUInt32(file, currentIndex);
            currentIndex += 4;

            Triangle[] triangles = new Triangle[amountOfTriangle];
            for (int i = 0; i < amountOfTriangle; i++)
            {
                triangles[i].Normal = new Vector(BitConverter.ToSingle(file, currentIndex), BitConverter.ToSingle(file, currentIndex + 4), BitConverter.ToSingle(file, currentIndex + 8));
                triangles[i].A = new Vector(BitConverter.ToSingle(file, currentIndex + 12), BitConverter.ToSingle(file, currentIndex + 16), BitConverter.ToSingle(file, currentIndex + 20));
                triangles[i].B = new Vector(BitConverter.ToSingle(file, currentIndex + 24), BitConverter.ToSingle(file, currentIndex + 28), BitConverter.ToSingle(file, currentIndex + 32));
                triangles[i].C = new Vector(BitConverter.ToSingle(file, currentIndex + 36), BitConverter.ToSingle(file, currentIndex + 38), BitConverter.ToSingle(file, currentIndex + 42));

                currentIndex += 50;
            }

            return triangles;
        }
    }
}
