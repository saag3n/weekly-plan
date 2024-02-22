using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace weekly_plan
{
    public partial class Form1 : Form
    {
        // Fields
        private List<Panel> todoLists = new List<Panel>();
        private const string dataDirectory = "data";

        // Constructor
        public Form1()
        {
            InitializeComponent();
            Directory.CreateDirectory(dataDirectory);
            LoadTodoLists();
        }

        // Event Handlers
        private void CreateNewListButton_Click(object sender, EventArgs e)
        {
            Panel newListPanel = CreateTodoListPanel("New List");
            todoLists.Add(newListPanel);
            MainFormLayout.Controls.Add(newListPanel);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (e.Control && e.KeyCode == Keys.D1)
                {
                    AddNewItem(textBox.Parent as FlowLayoutPanel, textBox.Parent.Controls.OfType<TextBox>().ToList());
                    e.SuppressKeyPress = true;
                }
                else if (e.Control && e.KeyCode == Keys.D4)
                {
                    DeleteItem(textBox.Parent?.Parent as Panel);
                    e.SuppressKeyPress = true;
                }
                else if (e.Control && e.KeyCode == Keys.D3)
                {
                    DeleteTextBox(textBox);
                    e.SuppressKeyPress = true;
                }
                else if (e.Control && e.KeyCode == Keys.D2)
                {
                    ToggleCompletion(textBox);
                    e.SuppressKeyPress = true;
                }
            }
        }

        // Methods
        private void LoadTodoLists()
        {
            string[] listFiles = Directory.GetFiles(dataDirectory, "*.txt");
            foreach (string file in listFiles)
            {
                Panel newListPanel = LoadListFromFile(file);
                if (newListPanel != null)
                {
                    todoLists.Add(newListPanel);
                    MainFormLayout.Controls.Add(newListPanel);
                }
            }
        }

        private Panel LoadListFromFile(string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                if (lines.Length >= 2 && int.TryParse(lines[1], out int numItems))
                {
                    Panel panel = CreateTodoListPanel(lines[0]);
                    for (int i = 2; i < lines.Length && i < numItems + 2; i++)
                    {
                        string[] parts = lines[i].Split(',');
                        if (parts.Length >= 2)
                        {
                            CreateNewTextBox(panel.Controls.OfType<FlowLayoutPanel>().FirstOrDefault(), panel.Controls.OfType<TextBox>().ToList(), parts[0], parts[1]);
                        }
                    }
                    return panel;
                }
                else
                {
                    MessageBox.Show("Invalid format for number of items.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading list from file: {ex.Message}");
            }
            return null;
        }

        private Panel CreateTodoListPanel(string listName)
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Top,
                Padding = new Padding(30),
                Height = 320,
                Width = 200
            };

            TextBox listNameTextBox = new TextBox
            {
                Text = listName,
                Tag = "listTitle",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Width = 200
            };
            panel.Controls.Add(listNameTextBox);

            FlowLayoutPanel taskPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight
            };
            panel.Controls.Add(taskPanel);

            List<TextBox> taskTextBoxes = new List<TextBox>();
            for (int i = 0; i < 5; i++)
            {
                CreateNewTextBox(taskPanel, taskTextBoxes);
            }

            Button saveButton = new Button
            {
                Text = "Save (Ctrl + S)",
                Dock = DockStyle.Top
            };
            saveButton.Click += (s, e) => SaveListToFile(panel);
            panel.Controls.Add(saveButton);

            return panel;
        }

        private void CreateNewTextBox(FlowLayoutPanel taskPanel, List<TextBox> taskTextBoxes, string completionState = "0", string itemText = "")
        {
            TextBox taskTextBox = new TextBox
            {
                Tag = "listItem",
                Width = 100
            };
            taskTextBox.TextChanged += (s, e) => SaveListToFile(taskPanel.Parent as Panel);
            taskTextBox.KeyDown += TextBox_KeyDown;

            if (completionState == "1")
            {
                ToggleCompletion(taskTextBox);
            }
            if (!string.IsNullOrEmpty(itemText))
            {
                taskTextBox.Text = itemText;
            }

            taskPanel.Controls.Add(taskTextBox);
            taskTextBoxes.Add(taskTextBox);

            foreach (var textBox in taskTextBoxes)
            {
                textBox.KeyDown -= TextBox_KeyDown;
                textBox.KeyDown += TextBox_KeyDown;
            }
        }

        private void ToggleCompletion(TextBox textBox)
        {
            if (textBox != null && textBox.Parent != null)
            {
                if (textBox.Font.Strikeout)
                {
                    textBox.Font = new Font(textBox.Font, textBox.Font.Style & ~FontStyle.Strikeout);
                    textBox.ForeColor = SystemColors.ControlText;
                }
                else
                {
                    textBox.Font = new Font(textBox.Font, textBox.Font.Style | FontStyle.Strikeout);
                    textBox.ForeColor = Color.Gray;
                }
                SaveListToFile(textBox.Parent as Panel);
            }
        }

        private void DeleteItem(Panel panel)
        {
            if (panel != null && panel.Parent != null)
            {
                var parentControl = panel.Parent;
                parentControl.Controls.Remove(panel);
                todoLists.Remove(panel);
                SaveListToFile(panel.Parent as Panel);
            }
        }

        private void AddNewItem(FlowLayoutPanel taskPanel, List<TextBox> taskTextBoxes)
        {
            if (taskPanel != null && taskTextBoxes != null)
            {
                if (taskTextBoxes.Count < 10)
                {
                    foreach (var textBox in taskTextBoxes)
                    {
                        textBox.KeyDown -= TextBox_KeyDown;
                    }
                    CreateNewTextBox(taskPanel, taskTextBoxes);
                    SaveListToFile(taskPanel.Parent as Panel);
                }
            }
        }

        private void DeleteTextBox(TextBox textBox)
        {
            if (textBox != null && textBox.Parent != null)
            {
                var parentControl = textBox.Parent;
                parentControl.Controls.Remove(textBox);
                SaveListToFile(parentControl.Parent as Panel);
            }
        }

        private void SaveListToFile(Panel panel)
        {
            if (panel != null)
            {
                string listName = panel.Controls.OfType<TextBox>().FirstOrDefault(t => t.Tag?.ToString() == "listTitle")?.Text ?? "Untitled";
                string fileName = $"{listName}.txt";
                string filePath = Path.Combine(dataDirectory, fileName);

                List<string> lines = new List<string>();
                foreach (TextBox textBox in panel.Controls.OfType<TextBox>().Where(tb => tb.Tag?.ToString() == "listItem"))
                {
                    string completionState = textBox.Font.Strikeout ? "1" : "0";
                    lines.Add($"{completionState},{textBox.Text}");
                }

                try
                {
                    File.WriteAllLines(filePath, lines);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving list to file: {ex.Message}");
                }
            }
        }

        private string GetUniqueFileName(string baseFileName)
        {
            string fileName = baseFileName;
            int counter = 1;
            string filePath = Path.Combine(dataDirectory, $"{fileName}.txt");

            while (File.Exists(filePath))
            {
                fileName = $"{baseFileName} ({counter++})";
                filePath = Path.Combine(dataDirectory, $"{fileName}.txt");
            }

            return filePath;
        }
    }
}
