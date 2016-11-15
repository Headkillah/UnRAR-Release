﻿using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace UnRar_Release
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static void processFile(string inputFileName, string outputDir, bool deleteInputFile)
        {
            try {
                string[] lines = File.ReadAllLines(inputFileName, Encoding.GetEncoding(28591));
                if (deleteInputFile)
                {
                    File.Delete(inputFileName);
                }
                string fileOutput = outputDir + @"\" + Path.GetFileName(inputFileName);
                File.WriteAllLines(fileOutput, lines, Encoding.GetEncoding(28591));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public static void extractSubs(string[] rarSubs, string releaseDir)
        {
            try {
                string[] rarSubs2;
                string releaseSubsDir = releaseDir + @"\" + "Subs";
                Directory.CreateDirectory(releaseSubsDir);
                extractArchiveUnthreaded(releaseSubsDir, rarSubs[0]);
                rarSubs2 = Directory.GetFiles(releaseSubsDir, "*.rar");
                if (rarSubs2 != null)
                {
                    extractArchiveUnthreaded(releaseSubsDir, rarSubs2[0]);
                    File.Delete(rarSubs2[0]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public static void extractArchiveUnthreaded(string outputDir, string file)
        {
            using (var archive = RarArchive.Open(@file))
            {
                foreach (var entry in archive.Entries)
                {
                    if (!entry.IsDirectory)
                    {
                        //entry.WriteToDirectory(@outputDir, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                        entry.WriteToDirectory(@outputDir);
                    }
                }
            }
        }

        public static string FormatBytes(long bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            return String.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }
    }
}