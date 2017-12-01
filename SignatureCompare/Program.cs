using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignatureUtils;
using AbstractionLayer;
using System.IO;
using SignatureUtils.Base;
using SignatureUtils.DTW;
using System.Diagnostics;
using System.Configuration;

namespace SignatureCompare
{
    /// <summary>
    /// Compare all signatures from a folder using DTW algorithm, to a reference signature from the specified folder
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string lFilePath = ConfigurationManager.AppSettings["outputFileName"] + DateTime.Now.Ticks.ToString() + ".csv";

            StreamWriter file1 = new StreamWriter(lFilePath, true);
            DTWConfiguration lDTWConfig = new DTWConfiguration()
                                    .UseXY()
                                    .UseX1Y1()
                                    .UseX2Y2()
                                    .UseForce()
                                    .UseForce1();

            Console.Write("Path to directory: ");
            string lPath = Console.ReadLine();

            string[] lSubdirectoryEntries = Directory.GetDirectories(lPath);


            file1.WriteLine("EER_a - XYX'Y'X''Y'' - T5");
            lDTWConfig = new DTWConfiguration()
                                    .UseXY()
                                    .UseX1Y1()
                                    .UseX2Y2();

            foreach (var directory in lSubdirectoryEntries)
            {
                Console.WriteLine("Processing " + directory + " directory");
                SignatureCompareFunctions.CompareMCYT(directory, 5, lDTWConfig, ref file1);
            }

            file1.Close();


        }
    }
}
