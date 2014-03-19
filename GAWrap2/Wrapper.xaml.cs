using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using Forms = System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GAWrap2
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Wrapper : Window
    {
        Driver driver = new Driver();

        public Wrapper()
        {
            InitializeComponent();

            Closing += OnClosing;   //Add app close event handler
        }

        void OnClosing(object sender, CancelEventArgs e)
        {
            driver.close();
        }

        /// <summary>
        /// Record a test case
        /// </summary>
        void BtnRecordTC_Click(object sender, RoutedEventArgs e)
        {
            if (!driver.toggleRecord(this))
                System.Windows.MessageBox.Show("Choose a test case to record");
        }

        void btnOpenTC_Click(object sender, RoutedEventArgs e)
        {
            Forms.OpenFileDialog oFD = new Forms.OpenFileDialog();
            oFD.RestoreDirectory = true;
            oFD.Filter = "Test Cases File *.GUIAutomation |*.GUIAutomation";

            oFD.ShowDialog();

            if (oFD.FileName.Length > 0)
            {
                string dir = System.IO.Path.GetDirectoryName(oFD.FileName);
                driver.readTCs(dir);
                dispTCs();
            }
        }

        void dispTCs()
        {
            TestCases.Items.Clear();
            CheckBox cB;

            foreach (string s in driver.getTCs())
            {
                cB = new CheckBox();
                cB.Content = s;
                TestCases.Items.Add(cB);
            }

            TestCases.SelectedIndex = driver.selectedIndex;

            //Check stored indices
            driver.checkedIndices.ForEach((item) =>
                {
                    ((CheckBox)TestCases.Items[item]).IsChecked = true;
                });
        }

        private void TestCases_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TestCases.SelectedIndex == -1)
            {
                TestName.Visibility = Visibility.Hidden;
                btnSaveTC.Visibility = Visibility.Hidden;
                return;
            }

            TestName.Visibility = Visibility.Visible;
            btnSaveTC.Visibility = Visibility.Visible;
            TestName.Text = ((CheckBox)TestCases.SelectedValue).Content.ToString();
        }

        private void TestName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TestName.Text.Length > 0)
                ((CheckBox)TestCases.SelectedValue).Content = TestName.Text;
        }

        private void TestCases_MouseUp(object sender, MouseButtonEventArgs e)
        {
            driver.selectedIndex = TestCases.SelectedIndex;
            
            //Store checked indices
            UpdateChecked();
            
            //Display test cases stored in memory
            dispTCs();
        }

        private void UpdateChecked()
        {
            List<int> checks = new List<int>();

            for (int i = 0; i < TestCases.Items.Count; i++)
                if (((CheckBox)TestCases.Items[i]).IsChecked.Value)
                    checks.Add(i);

            driver.checkedIndices = checks;
        }

        private void btnSaveTC_Click(object sender, RoutedEventArgs e)
        {
            driver.updateTC(((CheckBox)TestCases.SelectedValue).Content.ToString());

            dispTCs();
        }

        private void btnAddTC_Click(object sender, RoutedEventArgs e)
        {
            driver.addTC();
            dispTCs();
        }
        
        private void btnRemoveTC_Click(object sender, RoutedEventArgs e)
        {
            driver.removeTC();
            dispTCs();
        }

        private void btnEditTC_Click(object sender, RoutedEventArgs e)
        {
            if (!driver.showEditor(this))
                MessageBox.Show("Choose a test case to edit");
        }

        private void BtnPlayback_Click(object sender, RoutedEventArgs e)
        {
            UpdateChecked();
            if (driver.checkedIndices.Count == 0)
                MessageBox.Show("Choose test cases to replay");
            else
            {
                this.Visibility = System.Windows.Visibility.Hidden;
                driver.playback(driver.GetCheckedTCs());
                this.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}
