using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Windows.Media;
using System.Diagnostics;
using OpenTK;
using OpenTKLib;

namespace ICPLib
{
    public class DebugWriteUtils
    {
        public static void WriteMatrix(string name, Matrix4d m)
        {
            System.Diagnostics.Debug.WriteLine(name);
            
      
            for (int i = 0; i < 4; i++)
            {
                Debug.WriteLine(m[i, 0].ToString("0.0") + " " + m[i, 1].ToString("0.0") + " " + m[i, 2].ToString("0.0") + " " + m[i, 3].ToString("0.0"));

            }
        }
        public static void WriteTestOutput(string nameDisplayed, Matrix4d m, List<Vector3d> mypointsSource, List<Vector3d> myPointsTransformed, List<Vector3d> myPointsTarget)
        {
            WriteMatrix(nameDisplayed, m);

            long resultsWritten = mypointsSource.Count;
            if (resultsWritten > 5)
                resultsWritten = 5;
            System.Diagnostics.Debug.WriteLine("Points:");
            double meanDistance = 0;
            for (int i = 0; i < resultsWritten; i++)
            {
                Vector3d pToBeMatched = mypointsSource[i];
                Vector3d pTransformed = myPointsTransformed[i];
                Vector3d pReference = myPointsTarget[i];

                string p1 = pToBeMatched[0].ToString("0.0") + " " + pToBeMatched[1].ToString("0.0") + " " + pToBeMatched[2].ToString("0.0");
                string p2 = pTransformed[0].ToString("0.0") + " " + pTransformed[1].ToString("0.0") + " " + pTransformed[2].ToString("0.0");
                string p3 = pReference[0].ToString("0.0") + " " + pReference[1].ToString("0.0") + " " + pReference[2].ToString("0.0");

                double distance = MathBase.DistanceBetweenVectors(pTransformed, pReference);
                meanDistance += distance;
                Debug.WriteLine(i.ToString() + " : " + p1 + " :transformed: " + p2 + " :target: " + p3 + " : Distance: " + distance.ToString("0.0"));

            }
         //   Debug.WriteLine("--Mean Distance: " + (meanDistance / resultsWritten).ToString("0.0"));
        }
        public static void WriteTestOutputVertex(string nameDisplayed, Matrix4d m, List<Vertex> mypointsSource, List<Vertex> myPointsTransformed, List<Vertex> myPointsTarget)
        {
            WriteMatrix(nameDisplayed, m);

            long resultsWritten = mypointsSource.Count;
            if (resultsWritten > 5)
                resultsWritten = 5;
            System.Diagnostics.Debug.WriteLine("Points:");
            double meanDistance = 0;
            for (int i = 0; i < resultsWritten; i++)
            {
                Vector3d pToBeMatched = mypointsSource[i].Vector;
                Vector3d pTransformed = myPointsTransformed[i].Vector;
                Vector3d pReference = myPointsTarget[i].Vector;

                string p1 = pToBeMatched[0].ToString("0.0") + " " + pToBeMatched[1].ToString("0.0") + " " + pToBeMatched[2].ToString("0.0");
                string p2 = pTransformed[0].ToString("0.0") + " " + pTransformed[1].ToString("0.0") + " " + pTransformed[2].ToString("0.0");
                string p3 = pReference[0].ToString("0.0") + " " + pReference[1].ToString("0.0") + " " + pReference[2].ToString("0.0");

                double distance = MathBase.DistanceBetweenVectors(pTransformed, pReference);
                meanDistance += distance;
                Debug.WriteLine(i.ToString() + " : " + p1 + " :transformed: " + p2 + " :target: " + p3 + " : Distance: " + distance.ToString("0.0"));

            }
            //Debug.WriteLine("--Mean Distance: " + (meanDistance / resultsWritten).ToString("0.0"));
        }
    
    }
}
