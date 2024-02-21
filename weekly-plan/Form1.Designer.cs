namespace weekly_plan
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            CreateNewListButton = new Button();
            MainFormLayout = new FlowLayoutPanel();
            TabController = new TabControl();
            ToDoListTab = new TabPage();
            CalendarTab = new TabPage();
            MainFormLayout.SuspendLayout();
            TabController.SuspendLayout();
            ToDoListTab.SuspendLayout();
            SuspendLayout();
            // 
            // CreateNewListButton
            // 
            CreateNewListButton.Location = new Point(3, 3);
            CreateNewListButton.Name = "CreateNewListButton";
            CreateNewListButton.Size = new Size(107, 23);
            CreateNewListButton.TabIndex = 0;
            CreateNewListButton.Text = "Create New List";
            CreateNewListButton.UseVisualStyleBackColor = true;
            CreateNewListButton.Click += CreateNewListButton_Click;
            // 
            // MainFormLayout
            // 
            MainFormLayout.Controls.Add(CreateNewListButton);
            MainFormLayout.Dock = DockStyle.Fill;
            MainFormLayout.Location = new Point(3, 3);
            MainFormLayout.Name = "MainFormLayout";
            MainFormLayout.Size = new Size(928, 431);
            MainFormLayout.TabIndex = 1;
            // 
            // TabController
            // 
            TabController.Controls.Add(ToDoListTab);
            TabController.Controls.Add(CalendarTab);
            TabController.Dock = DockStyle.Fill;
            TabController.Location = new Point(0, 0);
            TabController.Name = "TabController";
            TabController.SelectedIndex = 0;
            TabController.Size = new Size(942, 465);
            TabController.TabIndex = 2;
            // 
            // ToDoListTab
            // 
            ToDoListTab.Controls.Add(MainFormLayout);
            ToDoListTab.Location = new Point(4, 24);
            ToDoListTab.Name = "ToDoListTab";
            ToDoListTab.Padding = new Padding(3);
            ToDoListTab.Size = new Size(934, 437);
            ToDoListTab.TabIndex = 0;
            ToDoListTab.Text = "TDL";
            ToDoListTab.UseVisualStyleBackColor = true;
            // 
            // CalendarTab
            // 
            CalendarTab.Location = new Point(4, 24);
            CalendarTab.Name = "CalendarTab";
            CalendarTab.Padding = new Padding(3);
            CalendarTab.Size = new Size(934, 437);
            CalendarTab.TabIndex = 1;
            CalendarTab.Text = "Calendar";
            CalendarTab.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(942, 465);
            Controls.Add(TabController);
            Name = "Form1";
            Text = "Form1";
            MainFormLayout.ResumeLayout(false);
            TabController.ResumeLayout(false);
            ToDoListTab.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void AddListButton_Click1(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ClearButton_Click1(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private Button CreateNewListButton;
        private FlowLayoutPanel MainFormLayout;
        private TabControl TabController;
        private TabPage ToDoListTab;
        private TabPage CalendarTab;
    }
}
