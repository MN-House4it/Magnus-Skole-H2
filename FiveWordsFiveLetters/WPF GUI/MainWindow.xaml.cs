using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WordProgram;

namespace WPF_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<List<string>> lastResuslt = new List<List<string>>();

        string exportFileSeperator = "-";

        string importFilePath = string.Empty;
        int wordLength = 5;
        public MainWindow()
        {
            InitializeComponent();
            this.Slider.Value = wordLength;
        }



        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(importFilePath))
            {
                this.IsEnabled = false;
                ListView.Items.Clear();
                ((GridView)ListView.View).Columns.Clear();
                DisplayWordCount.Content = "N/A";
                DisplayTime.Content = "N/A";
                wordLength = Convert.ToInt32(this.Slider.Value);

                Thread threadWordFinder = new Thread(() =>
                {
                    WordFinder wordFinder = new WordFinder();
                    wordFinder.ProgressChanged += WordFinder_ProgressChanged;
                    wordFinder.TimeChanged += WordFinder_TimeChanged;
                    var res = wordFinder.GatherWords(importFilePath, wordLength);

                    lastResuslt = res;

                    if (res.Count > 0)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            DisplayWordCount.Content = res.Count.ToString();

                            for (int i = 0; i < res[0].Count; i++)
                            {
                                ((GridView)ListView.View).Columns.Add(new GridViewColumn
                                {
                                    Header = "Word " + (i + 1),
                                    DisplayMemberBinding = new Binding($"[{i}]")
                                });
                            }

                            foreach (List<string> row in res)
                            {
                                ListView.Items.Add(row);
                            }
                        });
                    }
                });
                threadWordFinder.Start();
            }
            else
            {
                MessageBox.Show("You need to choose a file to calculate");
            }
        }
        // Used to update the state of the progressbar for the user to se
        private void WordFinder_ProgressChanged(int progress)
        {
            if (!Dispatcher.CheckAccess())
            {
                // Use Dispatcher to update the UI on the main thread
                Dispatcher.Invoke(() => WordFinder_ProgressChanged(progress));
            }
            else
            {
                // Update the progress bar and label on the UI thread
                if (progressBar.Visibility == Visibility.Hidden)
                {
                    progressBar.Visibility = Visibility.Visible;
                    //labelPercentage.Visibility = Visibility.Visible;
                }
                progressBar.Value = progress;
                //labelPercentage.Content = progress.ToString() + "%";
                if (progress >= 100)
                {
                    progressBar.Visibility = Visibility.Hidden;
                    //labelPercentage.Visibility = Visibility.Hidden;
                    this.IsEnabled = true;
                }
            }
        }


        // Sets the time for the run for the user to se
        private void WordFinder_TimeChanged(Stopwatch stopwatch)
        {
            //if (displayTime.InvokeRequired)
            //{
            //    // Use a delegate to invoke the update on the UI thread
            //    displayTime.Invoke(new Action<Stopwatch>(WordFinder_TimeChanged), new object[] { stopwatch });
            //}
            //else
            //{
            //    // Update the display time on the UI thread
            //    displayTime.Text = $"{stopwatch.Elapsed.Minutes}:{stopwatch.Elapsed.Seconds}:{stopwatch.Elapsed.Milliseconds}";
            //}
            Application.Current.Dispatcher.Invoke(() =>
            {
                DisplayTime.Content = $"{stopwatch.Elapsed.Minutes}:{stopwatch.Elapsed.Seconds}:{stopwatch.Elapsed.Milliseconds}";
            });
        }
        private void Slider_ValueChanged<T>(object sender, RoutedPropertyChangedEventArgs<T> e)
        {
            wordLength = Convert.ToInt32(Slider.Value);
            //displayWordLength.Text = wordLength.ToString();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            // Opens a file dialog to choose what file to use to calculate
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"; // Set filter if needed

            if (openFileDialog.ShowDialog() == true)
            {
                importFilePath = openFileDialog.FileName;
                this.DisplayPath.Content = importFilePath;
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            // Put the data into a string that can be written to a file
            string dataToWrite = string.Empty;
            foreach (var line in lastResuslt)
            {
                dataToWrite += string.Join(exportFileSeperator, line) + Environment.NewLine;
            }

            // Check if there is data to write
            if (!String.IsNullOrEmpty(dataToWrite))
            {
                // Opens a save file dialog for the user to choose where to save
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"; // Set filter if needed

                if (saveFileDialog.ShowDialog() == true)
                {
                    string saveFilePath = saveFileDialog.FileName;
                    try
                    {
                        // Write the data to the file
                        System.IO.File.WriteAllText(saveFilePath, dataToWrite);
                        MessageBox.Show("Data exported successfully to " + saveFilePath);
                    }
                    catch (Exception ex)
                    {
                        // Display the error to the user if the file could not be written
                        MessageBox.Show("An error occurred while exporting data: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("There is no calculations to export!");
            }
        }


    }
}
