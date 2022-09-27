using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Collections;
using System.Security.Cryptography;

namespace Container
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            openFileDialog.Filter = "txt files (*.txt)|*.txt";
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                string fileContent = File.ReadAllText(filePath);
                fileContent.Trim();
                HashSet<int> different_numbers=new HashSet<int>();
                Dictionary<int, HashSet<int>> containers = new Dictionary<int, HashSet<int>>();
                List<int> firstColumn=new List<int>();
                List<int> secondColumn=new List<int>();
                foreach(string currentComparison in fileContent.Split('\n'))
                {
                    if (currentComparison == "")
                    {
                        break;
                    }
                    int i = 0;
                    foreach(var currentString in currentComparison.Split(' '))
                    {
                        int current_number=int.Parse(currentString);
                        different_numbers.Add(current_number);
                        if (!containers.ContainsKey(current_number))
                        {
                            containers.Add(current_number, new HashSet<int>());
                        }
                        
                        if(i == 0) 
                        {
                            firstColumn.Add(current_number);
                        }
                        else
                        {
                            secondColumn.Add(current_number);
                        }
                        i++;
                    }
                }
                var iterList = firstColumn.Zip(secondColumn,(f, s) => new { firstColumn = f, secondColumn = s });
                foreach (var firstAndSecondColumn in iterList)
                {
                    containers.TryGetValue(firstAndSecondColumn.firstColumn, out var firstContainerSet);
                    containers.TryGetValue(firstAndSecondColumn.secondColumn, out var secondContainerSet);

                    firstContainerSet.Add(firstAndSecondColumn.secondColumn);
                    foreach (var i in secondContainerSet)
                    {
                        firstContainerSet.Add(i);
                    }

                    foreach(var hashSet in containers.Values)
                    {
                        if (hashSet.Contains(firstAndSecondColumn.firstColumn))
                        {
                            foreach(var i in firstContainerSet)
                            {
                                hashSet.Add(i);
                            }
                        }
                    }
                }
                bool isBiggestContainer = false;

                foreach (var (key,value) in containers.Select((k,v)=>(k,v)))
                {
                    if (key.Value.Count == different_numbers.Count - 1)
                    {
                        isBiggestContainer = true;
                        lblOut.Text = "Ja: Nummer " + key.Key.ToString();
                    }
                }
                if (!isBiggestContainer)
                {
                    lblOut.Text = "Nein";
                }
            }
        }
    }
}
