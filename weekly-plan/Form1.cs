using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace weekly_plan
{
    public partial class Form1 : Form
    {
        private List<Panel> todoLists = new List<Panel>();
        private const string dataDirectory = "data";
        private const string listsFilePath = "data/lists.txt";

        public Form1()
        {
            InitializeComponent();

            // Create data directory if it doesn't exist
            Directory.CreateDirectory(dataDirectory);

            // Load existing to-do lists
            LoadTodoLists();
        }

        private void CreateNewListButton_Click(object sender, EventArgs e)
        {
            // Create a new panel for the to-do list
            Panel newListPanel = CreateTodoListPanel();
            todoLists.Add(newListPanel);

            // Add the new panel to the form
            MainFormLayout.Controls.Add(newListPanel);
        }

        private void LoadTodoLists()
        {
            if (File.Exists(listsFilePath))
            {
                string[] lines = File.ReadAllLines(listsFilePath);
                foreach (string line in lines)
                {
                    Panel newListPanel = CreateTodoListPanel();
                    string[] tasks = line.Split(',');
                    for (int i = 0; i < tasks.Length && i < 10; i++) // Limit to 10 tasks per list
                    {
                        if (newListPanel.Controls[i + 1] is TextBox textBox)
                        {
                            textBox.Text = tasks[i];
                        }
                    }
                    todoLists.Add(newListPanel);
                    MainFormLayout.Controls.Add(newListPanel);
                }
            }
        }

        private Panel CreateTodoListPanel()
        {
            // Create a new panel for the to-do list
            Panel panel = new Panel();
            panel.Dock = DockStyle.Top;
            panel.Padding = new Padding(30); // Increased padding for the panel
            panel.Height = 300;
            panel.Width = 400;

            // Create a TextBox for the list name
            TextBox listNameTextBox = new TextBox();
            listNameTextBox.Text = "New List";
            listNameTextBox.Tag = "listTitle";
            listNameTextBox.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
            listNameTextBox.Width = 200; // Adjust width as needed
            panel.Controls.Add(listNameTextBox);

            // Create a FlowLayoutPanel for tasks
            FlowLayoutPanel taskPanel = new FlowLayoutPanel();
            taskPanel.Dock = DockStyle.Top;
            taskPanel.AutoSize = true; // Set flow direction to TopDown
            panel.Controls.Add(taskPanel);

            // Create text boxes for tasks
            // Create text boxes for tasks
            List<TextBox> taskTextBoxes = new List<TextBox>();
            for (int i = 0; i < 5; i++) // Only 5 text boxes initially
            {
                TextBox taskTextBox = new TextBox();
                taskTextBox.Tag = "listItem";
                taskTextBox.Width = 100; // Adjust width as needed
                taskTextBox.TextChanged += (s, e) => SaveTodoLists();

                // Handle the KeyDown event to detect Enter key press
                taskTextBox.KeyDown += (s, e) =>
                {
                    if (e.KeyCode == Keys.Enter) // Check for Enter key
                    {
                        CompleteTask(taskTextBox);
                    }
                    else if (e.Control && e.KeyCode == Keys.Enter) // Check for Ctrl + Enter
                    {
                        AddNewItem(taskPanel, taskTextBoxes);
                    }
                    else if (e.Control && e.KeyCode == Keys.Back) // Check for Ctrl + Backspace
                    {
                        DeleteItem(taskPanel, taskTextBoxes);
                    }
                };

                taskPanel.Controls.Add(taskTextBox);
                taskTextBoxes.Add(taskTextBox);
            }

            // Add an event handler for Ctrl + Enter to add a new item
            this.KeyPreview = true;
            this.KeyDown += (s, e) =>
            {
                if (e.Control && e.KeyCode == Keys.Enter)
                {
                    AddNewItem(taskPanel, taskTextBoxes);
                }
            };

            return panel;
        }

        private void CompleteTask(TextBox textBox)
        {
            // Apply strikethrough style to the task textbox
            textBox.Font = new System.Drawing.Font(textBox.Font, System.Drawing.FontStyle.Strikeout);
            textBox.Enabled = false; // Disable the textbox
            SaveTodoLists();
        }

        private void AddNewItem(FlowLayoutPanel taskPanel, List<TextBox> taskTextBoxes)
        {
            if (taskTextBoxes.Count < 10) // Maximum of 10 items
            {
                TextBox newTaskTextBox = new TextBox();
                newTaskTextBox.Tag = "listItem";
                newTaskTextBox.Width = 100; // Adjust width as needed
                newTaskTextBox.TextChanged += (sender, eventArgs) => SaveTodoLists();
                taskPanel.Controls.Add(newTaskTextBox);
                taskTextBoxes.Add(newTaskTextBox);
                newTaskTextBox.Focus();
                SaveTodoLists();

                newTaskTextBox.KeyDown += (s, e) =>
                {
                    if (e.Control && e.KeyCode == Keys.Enter) // Check for Ctrl + Enter
                    {
                        AddNewItem(taskPanel, taskTextBoxes);
                    }
                    else if (e.KeyCode == Keys.Enter) // Check for Enter key
                    {
                        CompleteTask(newTaskTextBox);
                    }
                    else if (e.Control && e.KeyCode == Keys.Back) // Check for Ctrl + Backspace
                    {
                        DeleteItem(taskPanel, taskTextBoxes);
                    }
                };
            }
        }

        private void DeleteItem(FlowLayoutPanel taskPanel, List<TextBox> taskTextBoxes)
        {
            if (taskTextBoxes.Count > 0)
            {
                TextBox lastTaskTextBox = taskTextBoxes.Last();
                taskPanel.Controls.Remove(lastTaskTextBox);
                taskTextBoxes.Remove(lastTaskTextBox);
                SaveTodoLists();
            }
        }

        private void SaveTodoLists()
        {
            List<string> listLines = new List<string>();
            foreach (Panel panel in todoLists)
            {
                List<string> tasks = new List<string>();
                foreach (Control control in panel.Controls)
                {
                    if (control is TextBox textBox)
                    {
                        tasks.Add(textBox.Text);
                    }
                }
                listLines.Add(string.Join(",", tasks));
            }
            File.WriteAllLines(listsFilePath, listLines);
        }
    }
}
