using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WordProgram;
using GUI;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics;

namespace GUI
{    
    public partial class FiveLettersFiveWords : Form
    {
        List<List<string>> lastResuslt = new List<List<string>>();

        string exportFileSeperator = "-";

        string importFilePath = string.Empty;
        int wordLength = 5;
        public FiveLettersFiveWords()
        {
            InitializeComponent();
            this.trackBar.Value = wordLength;
            this.ShowIcon = false;
        }


        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(importFilePath))
            {
                this.Enabled = false;
                dataGridView.Rows.Clear();
                dataGridView.Columns.Clear();
                displayTime.Text = "N/A";
                displayWordCount.Text = "N/A";


                Thread threadWordFinder = new Thread(() =>
                {
                    WordFinder wordFinder = new WordFinder();
                    wordFinder.ProgressChanged += WordFinder_ProgressChanged;
                    wordFinder.TimeChanged += WordFinder_TimeChanged;
                    var res = wordFinder.GatherWords(importFilePath, wordLength);

                    //TEST jj = new TEST();
                    //TEST.RUN();

                    lastResuslt = res;


                    if (res.Count > 0)
                    {                        
                        // This part should be executed on the UI thread
                        dataGridView.Invoke((MethodInvoker)delegate
                        {
                            displayWordCount.Text = res.Count.ToString();
                            for (int i = 0; i < res[0].Count; i++)
                            {
                                dataGridView.Columns.Add("Column" + i, "Word " + (i + 1));
                            }

                            foreach (List<string> row in res)
                            {
                                dataGridView.Rows.Add(row.ToArray());
                            }
                        });
                    }
                });
                threadWordFinder.Start();
            }
            else
            {
                MessageBox.Show("You need to choose a file to calculat");
            }            
        }

        // Used to update the state of the progressbar for the user to se
        private void WordFinder_ProgressChanged(int progress)
        {
            if (progressBar.InvokeRequired)
            {
                // Use a delegate to invoke the update on the UI thread
                progressBar.Invoke(new Action<int>(WordFinder_ProgressChanged), new object[] { progress });
            }
            else
            {
                // Update the progress bar on the UI thread
                if (!progressBar.Visible)
                {
                    progressBar.Visible = true;
                    labelProcentage.Visible = true;
                }
                progressBar.Value = progress;
                labelProcentage.Text = progress.ToString() + "%";
                if (progress >= 100)
                {
                    progressBar.Visible = false;
                    labelProcentage.Visible = false;
                    this.Enabled = true;
                }
            }
        }

        // Sets the time for the run for the user to se
        private void WordFinder_TimeChanged(Stopwatch stopwatch)
        {
            if (displayTime.InvokeRequired)
            {
                // Use a delegate to invoke the update on the UI thread
                displayTime.Invoke(new Action<Stopwatch>(WordFinder_TimeChanged), new object[] { stopwatch });
            }
            else
            {
                // Update the display time on the UI thread
                displayTime.Text = $"{stopwatch.Elapsed.Minutes}:{stopwatch.Elapsed.Seconds}:{stopwatch.Elapsed.Milliseconds}" ;
            }
        }
        

        // Register when the value of the word length slider changes
        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            wordLength = trackBar.Value;
            displayWordLength.Text = wordLength.ToString();
        }



        // Import file
        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            // Opens a input file dialog to choose what file to use to calculate
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                importFilePath = openFileDialog.FileName;
                this.displayFilePath.Text = importFilePath;
            }
        }

        // Export file
        private void buttonExport_Click(object sender, EventArgs e)
        {
            // Put the data into a sting that can be writen to a file
            string dataToWrite = string.Empty;
            foreach(var line in lastResuslt)
            {
                dataToWrite += string.Join(exportFileSeperator, line) + Environment.NewLine;
            }
            // Check if there is data to write
            if (!String.IsNullOrEmpty(dataToWrite))
            {
                // Opens a save file dialog for the user to choose where to save
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
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
                        // Display the error to the user it the file could not be writen
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
