﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Printing;

namespace Notepad
{
    public partial class Form1 : Form
    {
        Note newNote;
        string defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public Form1()
        {
            InitializeComponent();
            newNote = new Note("new.txt", defaultPath);
            UpdateHeader(newNote.NoteName);
        }

        //////////////// FILE /////////////


        // New File
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(newNote.Path) == false && textField.Text != "")
            {
                DialogResult dialog = MessageBox.Show($"Do you want to save changes in {newNote.NoteName}", "Notepad", MessageBoxButtons.YesNoCancel);
                switch (dialog)
                {
                    case DialogResult.Yes:
                        SaveFile(sender, e, true);
                        OpenNewFileMethod();
                        break;
                    case DialogResult.No:
                        OpenNewFileMethod();
                        break;
                    case DialogResult.Cancel:
                        break;
                }
            }
            else
                OpenNewFileMethod();
        }

        // Open File
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "txt files(*.txt)| *.txt";
            dialog.ShowDialog();
            if (dialog.FileName != "")
            {
                newNote.Path = dialog.FileName;
                newNote.NoteName = Path.GetFileName(dialog.FileName);
                StreamReader note = new StreamReader(dialog.FileName);
                textField.Text = note.ReadToEnd();
                note.Close();
            }
            UpdateHeader(newNote.NoteName);
        }

        // Save File
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(sender, e, false);
            UpdateHeader(newNote.NoteName);
        }

        // Save As File
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(sender, e, true);
            UpdateHeader(newNote.NoteName);
        }

        // Print File
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDialog dialog = new PrintDialog();
            PrintDocument print = new PrintDocument();
            dialog.Document = print;
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {

                print.PrintPage += new PrintPageEventHandler(document_PrintPage);
                print.Print();
            }


        }

        // Exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(newNote.Path) == false && textField.Text != "")
            {
                DialogResult dialog = MessageBox.Show($"Do you want to save changes in {newNote.NoteName}", "Notepad", MessageBoxButtons.YesNoCancel);
                switch (dialog)
                {
                    case DialogResult.Yes:
                        SaveFile(sender, e, true);
                        this.Close();
                        break;
                    case DialogResult.No:
                        this.Close();
                        break;
                    case DialogResult.Cancel:
                        break;
                }
            }
            else
                this.Close();
        }

        // Print Preview
        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintPreviewDialog dialog = new PrintPreviewDialog();
            PrintDocument print = new PrintDocument();
            print.PrintPage += new PrintPageEventHandler(document_PrintPage);
            dialog.Document = print;
            dialog.ShowDialog();
        }

        ////////// EDIT //////////

        // Undo
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textField.Text != "")
                textField.Undo();
        }

        // Redo
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textField.Text == "")
                textField.Undo();
        }

        /////////// METHODS /////////////

        // Method to save file and save as file
        private void SaveFile(object sender, EventArgs e, bool saveAs)
        {
            if (File.Exists(newNote.Path) && saveAs == false)
            {
                StreamWriter note = new StreamWriter(newNote.Path);
                note.Write(textField.Text);
                note.Close();
            }
            else
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "txt files(*.txt)| *.txt";
                dialog.ShowDialog();
                if (dialog.FileName != "")
                {
                    newNote.Path = dialog.FileName;
                    newNote.NoteName = Path.GetFileName(dialog.FileName);
                    StreamWriter note = new StreamWriter(dialog.FileName);
                    note.Write(textField.Text);
                    note.Close();
                }
            }
        }

        // method to open new file and reset variables
        private void OpenNewFileMethod()
        {
            Note newNote = new Note("new.txt", defaultPath);
            textField.Text = "";
            UpdateHeader(newNote.NoteName);
        }

        // method to update header of window
        private void UpdateHeader(string headerFileName)
        {
            string temporary = headerFileName;
            this.Text = temporary + " --- Notepad";
        }

        // method to create print page
        void document_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(textField.Text, textField.Font, Brushes.Black, 20, 20);
        }
    }
}