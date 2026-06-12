using System;
using System.Drawing;
using System.Windows.Forms;

namespace ErrorMaker
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ErrorMakerForm());
        }
    }

    internal sealed class ErrorMakerForm : Form
    {
        private readonly TextBox titleBox;
        private readonly TextBox messageBox;
        private readonly ComboBox typeBox;
        private readonly CheckBox topMostCheck;

        public ErrorMakerForm()
        {
            Text = "ErrorMaker";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(520, 390);
            Size = new Size(560, 430);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            BackColor = Color.FromArgb(245, 246, 248);

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 7,
                Padding = new Padding(18),
            };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            Controls.Add(root);

            var heading = new Label
            {
                Text = "ErrorMaker",
                Dock = DockStyle.Top,
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(31, 41, 55),
                Margin = new Padding(0, 0, 0, 12),
            };
            root.Controls.Add(heading, 0, 0);

            var titleLabel = CreateLabel("Window title");
            root.Controls.Add(titleLabel, 0, 1);

            titleBox = new TextBox
            {
                Dock = DockStyle.Top,
                Text = "ErrorMaker",
                Margin = new Padding(0, 0, 0, 14),
            };
            root.Controls.Add(titleBox, 0, 2);

            var messageLabel = CreateLabel("Message");
            root.Controls.Add(messageLabel, 0, 3);

            messageBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Text = "This is a Windows pop-up made with ErrorMaker.",
                Margin = new Padding(0, 0, 0, 14),
                MinimumSize = new Size(0, 120),
            };
            root.Controls.Add(messageBox, 0, 4);

            var options = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                RowCount = 2,
                Margin = new Padding(0, 0, 0, 16),
            };
            options.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            options.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            options.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            options.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.Controls.Add(options, 0, 5);

            options.Controls.Add(CreateLabel("Pop-up type"), 0, 0);

            typeBox = new ComboBox
            {
                Dock = DockStyle.Top,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new Padding(0, 0, 12, 0),
            };
            typeBox.Items.AddRange(new object[] { "Info", "Error", "Success", "Warning", "None" });
            typeBox.SelectedIndex = 0;
            options.Controls.Add(typeBox, 0, 1);

            topMostCheck = new CheckBox
            {
                Text = "Keep pop-up on top",
                Dock = DockStyle.Top,
                AutoSize = true,
                Checked = true,
                Margin = new Padding(12, 25, 0, 0),
            };
            options.Controls.Add(topMostCheck, 1, 1);

            var actions = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0),
                Margin = new Padding(0),
                WrapContents = false,
            };
            root.Controls.Add(actions, 0, 6);

            var showButton = CreateActionButton("Show Pop-up", 128);
            showButton.Click += ShowPopUp;
            actions.Controls.Add(showButton);

            var clearButton = CreateActionButton("Clear", 92);
            clearButton.Click += delegate
            {
                messageBox.Clear();
                messageBox.Focus();
            };
            actions.Controls.Add(clearButton);
        }

        private static Label CreateLabel(string text)
        {
            return new Label
            {
                Text = text,
                Dock = DockStyle.Top,
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(55, 65, 81),
                Margin = new Padding(0, 0, 0, 6),
            };
        }

        private static Button CreateActionButton(string text, int width)
        {
            return new Button
            {
                Text = text,
                Width = width,
                Height = 34,
                Margin = new Padding(8, 0, 0, 0),
                FlatStyle = FlatStyle.System,
            };
        }

        private void ShowPopUp(object sender, EventArgs e)
        {
            var icon = GetIcon(typeBox.SelectedItem.ToString());
            var title = string.IsNullOrWhiteSpace(titleBox.Text) ? "ErrorMaker" : titleBox.Text.Trim();
            var text = string.IsNullOrWhiteSpace(messageBox.Text)
                ? " "
                : messageBox.Text;

            if (topMostCheck.Checked)
            {
                using (var owner = new Form())
                {
                    owner.ShowInTaskbar = false;
                    owner.StartPosition = FormStartPosition.Manual;
                    owner.Size = new Size(1, 1);
                    owner.Location = new Point(-2000, -2000);
                    owner.TopMost = true;
                    owner.Show();
                    MessageBox.Show(owner, text, title, MessageBoxButtons.OK, icon);
                }
            }
            else
            {
                MessageBox.Show(this, text, title, MessageBoxButtons.OK, icon);
            }
        }

        private static MessageBoxIcon GetIcon(string type)
        {
            switch (type)
            {
                case "Error":
                    return MessageBoxIcon.Error;
                case "Success":
                    return MessageBoxIcon.Information;
                case "Warning":
                    return MessageBoxIcon.Warning;
                case "None":
                    return MessageBoxIcon.None;
                case "Info":
                default:
                    return MessageBoxIcon.Information;
            }
        }
    }
}
