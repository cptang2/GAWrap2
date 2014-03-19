using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GAWrap2.Editor;
using GAWrap2.Record;
using GAWrap2.Playback;

namespace GAWrap2
{
    class Driver
    {
        int _selectedIndex = -1;
        public int selectedIndex //Prevent out of index exceptions
        {
            set
            {
                _selectedIndex = value;
            }
            get
            {
                if (_selectedIndex >= testcases.Count)
                    _selectedIndex = testcases.Count - 1;
                else if (_selectedIndex < -1)
                    _selectedIndex = -1;

                return _selectedIndex;
            }
        }

        public List<int> checkedIndices = new List<int>(); //Store checked test cases to run
        List<string> testcases = new List<string>();       //Store path to test cases

        public Driver() { }

        /// <summary>
        /// Clean up
        /// </summary>
        public void close()
        {
        }

        /// <summary>
        /// Gets viable test cases from the target directory
        /// </summary>
        /// <param name="dir">The full path to the directory</param>
        /// <returns>The list of test cases</returns>
        public List<string> readTCs(string dir)
        {
            List<string> temp = new List<string>();

            selectedIndex = -1;
            checkedIndices.Clear();
            testcases.Clear();

            foreach (string d in Directory.GetDirectories(dir))
                if (File.Exists(Path.Combine(d, "testcase.csv")))
                    testcases.Add(d);

            testcases.ForEach((item) => temp.Add(Path.GetFileName(item))); //Copy test cases to testcases list
            return temp;
        }

        /// <summary>
        /// Get a copy of the internal test cases
        /// </summary>
        public List<string> getTCs()
        {
            List<string> temp = new List<string>();
            testcases.ForEach((item) => temp.Add(Path.GetFileName(item)));

            return temp;
        }

        /// <summary>
        /// Renames the selected test case on the hard drive
        /// </summary>
        public void updateTC(string dir)
        {
            if (!Directory.Exists(testcases[selectedIndex]) && dir.Length == 0)
                return;

            string newDir = Path.GetDirectoryName(testcases[selectedIndex]);
            newDir = Path.Combine(newDir, dir);

            try
            {
                Directory.Move(testcases[selectedIndex], newDir);
            }
            catch
            {
                System.Windows.MessageBox.Show("Could not rename test case");
                return;
            }

            testcases[selectedIndex] = newDir;
        }

        public void addTC()
        {
            string dir;

            if ((dir = GetDirectoryDialog.GetDirectory()) == null || dir.Length == 0)
                return;
            
            if  (!testcases.Contains(dir))
                testcases.Add(dir);
        }

        public void removeTC()
        {
            if (selectedIndex == -1)
                return;

            if (checkedIndices.Contains(_selectedIndex))
                checkedIndices.Remove(_selectedIndex);

            //Decrement all items greater than the selected index:
            for (int i = checkedIndices.Count - 1; i >= 0; i--)
                if (checkedIndices[i] > _selectedIndex)
                    checkedIndices[i]--;

            testcases.RemoveAt(_selectedIndex);
        }

        #region Record

        /// <summary>
        /// Toggle recording the test case
        /// </summary>
        /// <param name="dir">The full path to the target directory</param>
        public bool toggleRecord(object sender)
        {
            if (selectedIndex == -1)
                return false;

            if (!ConfirmDialog.GetConf())
                return true;

            ((Wrapper)sender).Visibility = System.Windows.Visibility.Hidden;
            RecordTrayApp.initRecordApp(testcases[selectedIndex]);
            ((Wrapper)sender).Visibility = System.Windows.Visibility.Visible;

            return true;
        }

        #endregion

        #region Edit

        /// <summary>
        /// Open an editor with the chosen test case loaded
        /// </summary>
        public bool showEditor(Object sender)
        {
            if (selectedIndex == -1)
                return false;

            if (!File.Exists(Path.Combine(testcases[selectedIndex], "testcase.csv")))
                return false;

            ((Wrapper)sender).Visibility = System.Windows.Visibility.Hidden;
            EditorGUI.initEditor(testcases[selectedIndex]);
            ((Wrapper)sender).Visibility = System.Windows.Visibility.Visible;

            return true;
        }

        #endregion

        #region Playback

        public void playback(string[] testcases)
        {
            PlaybackTrayApp.initPlaybackApp(testcases);
        }

        /// <summary>
        /// Get checked test case file paths for test cases that exist
        /// </summary>
        /// <returns>Checked test case file paths</returns>
        public string[] GetCheckedTCs()
        {
            List<string> TCs = new List<string>();

            foreach (int i in checkedIndices)
            {
                if (File.Exists(Path.Combine(testcases[i], "testcase.csv")))
                    TCs.Add(Path.Combine(testcases[i], "testcase.csv"));
            }

            return TCs.ToArray();
        }

        #endregion
    }
}
