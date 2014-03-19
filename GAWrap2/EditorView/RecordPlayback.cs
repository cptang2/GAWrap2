using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using GAWrap2.Playback;
using GAWrap2.Record;

namespace GAWrap2.Editor
{
    class RecordPlayback
    {
        /// <summary>
        /// Replay the steps in memory to a specified point
        /// </summary>
        public static void replay(EditorGUI sender, string file)
        {
            string[] testcase = new string[] { file };
            Action play = () =>
                {
                    PlaybackTrayApp.initPlaybackApp(testcase);
                };

            Thread playThread = new Thread(new ThreadStart(play));

            ((EditorGUI)sender).Hide();
            playThread.Start();

            while (playThread.IsAlive)
                Thread.Sleep(1000);
            ((EditorGUI)sender).Show();
        }

        public static void record(EditorGUI sender, string testDir, string recordDir, int stepIndex)
        {
            Action record = () =>
                {
                    RecordTrayApp.initRecordApp(recordDir);
                };

            Thread recordThread = new Thread(new ThreadStart(record));

            ((EditorGUI)sender).Hide();
            recordThread.Start();

            while (recordThread.IsAlive)
                Thread.Sleep(1000);
            ((EditorGUI)sender).Show();

            combine(testDir, recordDir, stepIndex);
        }

        // Kind of a hacky way to insert the steps
        static void combine(string testDir, string recordDir, int stepIndex)
        {
            int max = 0;
            foreach (string f in Directory.GetFiles(testDir))
                if (Path.GetExtension(f) == ".png")
                    if (int.Parse(Path.GetFileNameWithoutExtension(f)) > max)
                        max = int.Parse(Path.GetFileNameWithoutExtension(f));
            max++;

            //Insert temp recording into test case:
            string recordCase = Path.Combine(recordDir, "testcase.csv");
            string testCase = Path.Combine(testDir, "testcase.csv");
            string[] tempRecord = File.ReadAllLines(recordCase);

            List<string> testcase = new List<string>();
            foreach (string s in File.ReadAllLines(Path.Combine(testDir, "testcase.csv")))
                testcase.Add(s);

            //Find the index of the step index
            int tempIndex = 0;
            int imIndex = 0;
            for (imIndex = 0; imIndex < testcase.Count; imIndex++)
            {
                if (testcase[imIndex].Contains("image"))
                {
                    tempIndex++;

                    if (tempIndex == stepIndex + 1)
                        break;
                }
            }

            foreach (string s in tempRecord)
            {
                if (s.Split(',')[0] == "image")
                {
                    testcase.Insert(imIndex, "image," + max + ".png");
                    File.Copy(Path.Combine(recordDir, s.Split(',')[1]), Path.Combine(testDir, (max + ".png")));
                    imIndex++;
                    max++;
                }
                else
                {
                    testcase.Insert(imIndex, s);
                    imIndex++;
                }
            }

            File.WriteAllLines(Path.Combine(testDir, "testcase.csv"), testcase.ToArray());
        }

        /// <summary>
        /// Rename and remove image files so that they are organized sequentially
        /// </summary>
        /// <param name="dir">Full path to test case directory</param>
        /// <returns>Value indicating whether the operation was successful</returns>
        public static bool serialize(string dir)
        {
            string archive = Path.Combine(dir, "archive");

            try
            {
                if (Directory.Exists(archive))
                    Directory.Delete(archive, true);
                Directory.CreateDirectory(archive);

                //Backup test case directory before renaming operations:
                foreach (string f in Directory.GetFiles(dir))
                    File.Copy(f, Path.Combine(archive, Path.GetFileName(f)));
            }
            catch
            {
                System.Windows.MessageBox.Show("Serialize quit because it was unable to back up the test case files");
                return false;
            }

            Dictionary<string, string> nameDict = GetNewNames(dir);
            rewrite(Path.Combine(dir, "testcase.csv"), nameDict);
            CopyImages(dir, archive, nameDict);
            
            return true;
        }

        //Get new naming scheme
        static Dictionary<string, string> GetNewNames(string dir)
        {
            Dictionary<string, string> nameDict = new Dictionary<string, string>();
            int newIndex = 0;

            foreach (string s in File.ReadAllLines(Path.Combine(dir, "testcase.csv")))
            {
                if (s.Split(',')[0] == ("image"))
                {
                    string newName = newIndex + ".png";
                    nameDict.Add(s.Split(',')[1], newName);
                    newIndex++;
                }
            }

            return nameDict;
        }

        //Rewrite test case file contents to have simpler names
        static bool rewrite(string file, Dictionary<string, string> nameDict)
        {
            string[] lines = File.ReadAllLines(file);

            for (int i = 0; i < lines.Length; i++)
                if (lines[i].Split(',')[0] == "image")
                    lines[i] = "image," + nameDict[lines[i].Split(',')[1]];

            try
            {
                File.WriteAllLines(file, lines);
            }
            catch
            {
                System.Windows.MessageBox.Show("Serialize quit because it was unable to write to the test case file");
                return false;
            }

            return true;
        }

        static bool CopyImages(string targetDir, string backDir, Dictionary<string, string> nameDict)
        {
            //Delete image files in test case directory
            try
            {
                foreach (string f in Directory.GetFiles(targetDir))
                    if (Path.GetExtension(f) == ".png")
                        File.Delete(f);
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("Serialize quit because it was unable to delete original image files");
                System.Windows.MessageBox.Show(e.ToString());
                return false;
            }

            //Copy over image files from the backup directory and rename them based on the dictionary.
            try
            {
                foreach (string f in Directory.GetFiles(backDir))
                {
                    if (!(Path.GetExtension(f) == ".png") || !nameDict.ContainsKey(Path.GetFileName(f)))
                        continue;

                    File.Copy(Path.Combine(backDir, f), Path.Combine(targetDir, nameDict[Path.GetFileName(f)]));
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Serialize quit because it was unable to copy image files to the test case folder");
                return false;
            }


            return true;
        }
    }
}
