using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsProjectWithThreads
{
    public partial class Form1 : Form
    {
        int[] array;
        int[] arrayBub;
        int[] arrayIns;
        int[] arraySel;

        TimeSpan tsBubble;
        TimeSpan tsIns;
        TimeSpan tsSel;

        bool fCancelBub;
        bool fCancelIns;
        bool fCancelSel;

        private void DisplayArray(int[] A, ListBox LB)
        {
            LB.Items.Clear();
            for (int i = 0; i < A.Length; i++)
            {
                LB.Items.Add(A[i]);
            }
        }

        private void Active(bool active)
        {
            label2.Enabled = active;
            label3.Enabled = active;
            label4.Enabled = active;
            label5.Enabled = active;
            label6.Enabled = active;
            label7.Enabled = active;
            label8.Enabled = active;
            BubbleSorting.Enabled = active;
            InsertionSorting.Enabled = active;
            SelectionSorting.Enabled = active;
            progressBar1.Enabled = active;
            progressBar2.Enabled = active;
            progressBar3.Enabled = active;
            button2.Enabled = active;
            button3.Enabled = active;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Active(false);

            label6.Text = "";
            label7.Text = "";
            label8.Text = "";

            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            if (!backgroundWorker2.IsBusy)
            {
                backgroundWorker2.RunWorkerAsync();
            }

            if (!backgroundWorker3.IsBusy)
            {
                backgroundWorker3.RunWorkerAsync();
            }

            if (!backgroundWorker4.IsBusy)
            {
                backgroundWorker4.RunWorkerAsync();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                backgroundWorker2.CancelAsync();
                backgroundWorker3.CancelAsync();
                backgroundWorker4.CancelAsync();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Random rnd = new Random();

            int n = Convert.ToInt32(textBox1.Text);

            array = new int[n];

            arrayBub = new int[n];
            arrayIns = new int[n];
            arraySel = new int[n];

            for (int i = 0; i < n; i++)
            {
                Thread.Sleep(1);
                array[i] = rnd.Next(1, n + 1);
                arrayBub[i] = arraySel[i] = arrayIns[i] = array[i];

                try
                {
                    backgroundWorker1.ReportProgress((i * 100) / n);
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            int x;

            tsBubble = new TimeSpan(DateTime.Now.Ticks);

            for (int i = 0; i < arrayBub.Length; i++)
            {
                Thread.Sleep(1);

                for (int j = arrayBub.Length - 1; j > i; j--)
                {
                    if (arrayBub[j - 1] > arrayBub[j])
                    {
                        x = arrayBub[j];
                        arrayBub[j] = arrayBub[j - 1];
                        arrayBub[j - 1] = x;
                    }
                }

                try
                {
                    backgroundWorker2.ReportProgress((i * 100) / arrayBub.Length);
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                if (backgroundWorker2.CancellationPending)
                {
                    fCancelBub = true;
                    break;
                }
            }
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            int x, i, j;

            tsIns = new TimeSpan(DateTime.Now.Ticks);

            for (i = 0; i < arrayIns.Length; i++)
            {
                Thread.Sleep(1);

                x = arrayIns[i];

                for (j = i - 1; j >= 0 && arrayIns[j] > x; j--)
                    arrayIns[j + 1] = arrayIns[j];
                arrayIns[j + 1] = x;

                try
                {
                    backgroundWorker3.ReportProgress((i * 100) / arrayIns.Length);
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                if (backgroundWorker3.CancellationPending)
                {
                    fCancelIns = true;
                    break;
                }
            }
        }

        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            int i, j, k;
            int x;

            tsSel = new TimeSpan(DateTime.Now.Ticks);

            for (i = 0; i < arraySel.Length; i++)
            {
                Thread.Sleep(1);

                k = i;

                x = arraySel[i];

                for (j = i + 1; j < arraySel.Length; j++)
                    if (arraySel[j] < x)
                    {
                        k = j;
                        x = arraySel[j];
                    }

                arraySel[k] = arraySel[i];
                arraySel[i] = x;

                try
                {
                    backgroundWorker4.ReportProgress((i * 100) / arraySel.Length);
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                if (backgroundWorker4.CancellationPending)
                {
                    fCancelSel = true;
                    break;
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            button1.Text = "Generate array " + e.ProgressPercentage.ToString() + "%";
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label8.Text = Convert.ToString(e.ProgressPercentage) + " %";
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label7.Text = Convert.ToString(e.ProgressPercentage) + " %";
            progressBar2.Value = e.ProgressPercentage;
        }

        private void backgroundWorker4_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label6.Text = Convert.ToString(e.ProgressPercentage) + " %";
            progressBar3.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button1.Text = "Generate array";

            Active(true);

            DisplayArray(array, BubbleSorting);
            DisplayArray(array, InsertionSorting);
            DisplayArray(array, SelectionSorting);
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (fCancelBub)
            {
                label8.Text = "";

                DisplayArray(array, BubbleSorting);

                fCancelBub = false;
            }
            else
            {
                TimeSpan time = new TimeSpan(DateTime.Now.Ticks) - tsBubble;
                label8.Text = String.Format("{0}:{1}:{2}:{3}", time.Hours, time.Minutes,
                time.Seconds, time.Milliseconds);

                DisplayArray(arrayBub, BubbleSorting);
            }

            progressBar1.Value = 0;
            button1.Enabled = true;
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (fCancelIns)
            {
                label7.Text = "";

                DisplayArray(array, InsertionSorting);

                fCancelIns = false;
            }
            else
            {
                TimeSpan time = new TimeSpan(DateTime.Now.Ticks) - tsIns;
                label7.Text = String.Format("{0}:{1}:{2}:{3}", time.Hours, time.Minutes,
                time.Seconds, time.Milliseconds);

                DisplayArray(arrayIns, InsertionSorting);
            }

            progressBar2.Value = 0;
            button1.Enabled = true;
        }

        private void backgroundWorker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (fCancelSel)
            {
                label6.Text = "";

                DisplayArray(array, SelectionSorting);

                fCancelSel = false;
            }
            else
            {
                TimeSpan time = new TimeSpan(DateTime.Now.Ticks) - tsSel;
                label6.Text = String.Format("{0}:{1}:{2}:{3}", time.Hours, time.Minutes,
                  time.Seconds, time.Milliseconds);

                DisplayArray(arraySel, SelectionSorting);
            }

            progressBar3.Value = 0;
            button1.Enabled = true;
        }


        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "";

            BubbleSorting.Items.Clear();
            InsertionSorting.Items.Clear();
            SelectionSorting.Items.Clear();

            progressBar1.Value = 0;
            progressBar2.Value = 0;
            progressBar3.Value = 0;

            Active(false);

            fCancelBub = false;
            fCancelIns = false;
            fCancelSel = false;

        }
    }
}
