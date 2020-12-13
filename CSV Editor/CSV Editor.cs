using System;
using System.Linq;
using System.IO;
using System.Windows.Forms;

namespace CSV_Editor
{
    public partial class Form1 : Form
    {
        private string CurrentFileName = null;

        public Form1()
        {
            InitializeComponent();
        }

        public Form1(string filePath)
        {
            InitializeComponent();
            if(!string.IsNullOrEmpty(filePath))
            {
                OpenFile(filePath);
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var path = openFileDialog1.FileName;
                OpenFile(path);
            }
        }

        private void OpenFile(string path)
        {
            if (File.Exists(path))
            {
                CurrentFileName = path;
                string[] file = File.ReadAllLines(path);

                int columns = file[0].Split(';').Length;
                int rows = file.Length;

                dataGridView1.RowCount = rows + 1;

                for (int row = 0; row < rows; row++)
                {
                    var row_content = file[row].Split(';');
                    for (int col = 0; col < columns; col++)
                    {
                        dataGridView1.Rows[row].Cells[col].Value = row_content[col];
                    }
                }

                toolStripStatusLabel2.Text = string.Format("File {0} loaded successfully!", path);
            }
            else
            {
                toolStripStatusLabel2.Text = "File is not exists";
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {    
            if (string.IsNullOrEmpty(CurrentFileName))            
            {
                if(saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    CurrentFileName = saveFileDialog1.FileName;
                }
            }

            SaveFile(CurrentFileName);
        }

        private void SaveFile(string path)
        {
            int columns = 0;
            while (dataGridView1.Rows[0].Cells[columns].Value != null)
            {
                columns++;
            }

            int rows = 0;
            while (dataGridView1.Rows[rows].Cells[0].Value != null)
            {
                rows++;
            }

            string res = "";

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    res += (col + 1 == columns) ?
                        dataGridView1.Rows[row].Cells[col].Value :
                        string.Format("{0};", dataGridView1.Rows[row].Cells[col].Value);
                }
                if (row + 1 != rows) res += '\n';
            }

            File.WriteAllText(path, res);
            toolStripStatusLabel2.Text = string.Format("File {0} saved successfully!", path);
            return;
        }
    }
}
