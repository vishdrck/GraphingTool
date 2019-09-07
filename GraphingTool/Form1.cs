using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace GraphingTool
{
    public partial class Form1 : Form
    {
        public String filePath;
        public String srcHtml;

        public int count = 0; //Counting the rows in csv file

        public String[] yLabels; //Array for y axis lables
        

        //File path to example html file 
        String htmlFilePath1 = System.IO.Directory.GetCurrentDirectory() + "\\part1.dat"; //Path for part1.dat
        String htmlFilePath2 = System.IO.Directory.GetCurrentDirectory() + "\\part2.dat"; //Path for part2.dat

        /*
         * This is the source for the html file which we use for plotting the graphs
         * All rights reserved @ Vishwajith Weerasinghe
         
            datasets: [{label: '27',backgroundColor: [
							'rgba(255, 99, 132, 0.2)',
							'rgba(255, 99, 132, 0.2)',
							'rgba(255, 99, 132, 0.2)',
							'rgba(255, 99, 132, 0.2)',
							'rgba(255, 99, 132, 0.2)'],
						borderColor: [
							'rgba(255, 99, 132, 1)',
							'rgba(255, 99, 132, 1)',
							'rgba(255, 99, 132, 1)',
							'rgba(255, 99, 132, 1)',
							'rgba(255, 99, 132, 1)'],
						borderWidth: 1,
						data: [6, 36, 2, 6, 50],
					},
         
         */


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnOpenFile.Enabled = true;
            btnCreateGraph.Enabled = false;
            
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if(openFile.ShowDialog()==DialogResult.OK)
            {
                try
                {
                    filePath = openFile.FileName;
                    lblFilePath.Text = filePath;
                    btnCreateGraph.Enabled = true;
                }
                catch(Exception ex)
                {
                    MessageBox.Show("File open error. Error code {" + ex.Message + " }\n\n" +
                        "Details : \n{" + ex.StackTrace + "}");
                }
            }
        }

        private void btnCreateGraph_Click(object sender, EventArgs e)
        {
            try
            {
                String textLine; //Scanned text is stored here
                //int index = 0; //Start the index at 1

                if (System.IO.File.Exists(filePath) == true)
                {
                    System.IO.StreamReader readFile = new System.IO.StreamReader(filePath); //scanning for the csv file
                    System.IO.StreamReader readFile2 = new System.IO.StreamReader(filePath); //finding for the no of rows
                    String xLablesForGraph = "labels: [";
                    String data = "datasets: [";
                    int calCount;

               
                    do
                    {
                        readFile2.ReadLine(); //Read the file line by line
                        count++; //increment the count which means the no of lines
                    } while (readFile2.Peek() != -1);

                    calCount = count; //Assinging the no of rows to the calCount

                    String[] Lables = new string[count];
                    double[][] readings = new double[calCount][];

                    count = 0;

                    readFile2.Close();

                    do
                    {
                        textLine = readFile.ReadLine(); //Read the file line by line

                        String[] xLabels = textLine.Split(','); //Array for x axis lables

                        Lables[count] = xLabels[0];
                        readings[count] = new double[xLabels.Length - 1];
                        for (int i = 1; i < xLabels.Length; i++)
                        {
                            readings[count][i - 1] = Double.Parse(xLabels[i]);
                        }
                        count++;

                    } while (readFile.Peek() != -1);

                    
                    xLablesForGraph += "\'" + Lables[1] + "\'";

                    for (int i = 2; i < Lables.Length; i++)
                    {

                        xLablesForGraph += ",\'" + Lables[i] + "\'";

                    }
                    xLablesForGraph += "],";

                    srcHtml = File.ReadAllText(htmlFilePath1);
                    textBox1.Text = srcHtml;
                    textBox1.AppendText(xLablesForGraph);
                

                    
                    textBox1.AppendText(data);
                    for (int i = 0; i < readings.Length-1; i++)
                    {
                       
                        data = "{label: ";
                        
                        data += "\'" + (readings[0][i]).ToString() + "\',";
                        data += "backgroundColor: [";

                        for (int k = 0; k<readings.Length;k++)
                        {
                            data += "\'rgba(255, 99, 132, 0.2)\',";
                        }

                        data += "],";
                        data += "borderColor: [";
                        for (int l = 0; l < readings.Length; l++)
                        {
                            data += "\'rgba(255, 99, 132, 1)\',";
                        }

                        data += "],";
                        data += "borderWidth: 1,";
                        data += "data: [";
                        for (int x=1;x<calCount;x++)
                        {
                            data += readings[x][i].ToString() + ",";
                        }

                        data += "],},";
                    
                        textBox1.AppendText(data);
                   
                    }
                    data = "]};";
                    textBox1.AppendText(data);
                    

                    srcHtml = File.ReadAllText(htmlFilePath2);
                    textBox1.AppendText(srcHtml);
                    readFile.Close();

                    System.IO.File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "\\index.html", textBox1.Text);
                    System.Diagnostics.Process.Start("Chrome", Uri.EscapeDataString(System.IO.Directory.GetCurrentDirectory() + "\\index.html"));

                    btnCreateGraph.Enabled = false;
                    count = 0;
                    textBox1.Text = "";
                    data = "";
                    srcHtml = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Source file open error. Error code {" + ex.Message + " }\n\n" +
                        "Details : \n{" + ex.StackTrace + "}");
            }

        }

    }
}
