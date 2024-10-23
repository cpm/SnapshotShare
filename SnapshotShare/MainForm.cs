using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnapshotShare
{
    public partial class MainForm : Form
    {
        private PictureBox pictureBox;
        private Button showButton;
        private Button stopButton;
        private Button refreshButton;
        private FlowLayoutPanel buttonPanel;

        private SecondaryMonitorForm secondaryForm;
        private bool sharing = false;
        private TableLayoutPanel layoutPanel;

        public MainForm()
        {
            InitializeComponent();


            // Always on top for the control window
            this.TopMost = true;
            this.Size = new Size(400, 300);  // Adjust the initial window size as needed
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(100, 100);

            this.SizeGripStyle = SizeGripStyle.Show;
            // Create layout panel for organizing buttons and picture box
            layoutPanel = new TableLayoutPanel();
            layoutPanel.Dock = DockStyle.Fill; // Fill the form
            layoutPanel.RowCount = 2;  // Two rows: one for buttons, one for the picture
            layoutPanel.ColumnCount = 1;  // Single column

            layoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));  // Auto-size the row for buttons
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // Make the second row (picture) fill the remaining space

            buttonPanel = new FlowLayoutPanel();
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.FlowDirection = FlowDirection.LeftToRight;
            buttonPanel.AutoSize = true;

            // Create "Show to Customer" button
            showButton = new Button();
            showButton.Text = "Show to Customer";
            showButton.Dock = DockStyle.Top;
            showButton.Width = 150;
            showButton.Click += new EventHandler(ShowToCustomerClicked);

            // Create "Stop Sharing" button
            stopButton = new Button();
            stopButton.Text = "Stop Sharing";
            stopButton.Dock = DockStyle.Top;
            stopButton.Click += new EventHandler(StopSharingClicked);
            stopButton.Visible = false;  // Initially hidden

            // Create "Refresh" button
            refreshButton = new Button();
            refreshButton.Text = "Refresh";
            refreshButton.Dock = DockStyle.Top;
            refreshButton.Click += new EventHandler(RefreshScreenshotClicked);
            refreshButton.Visible = false;  // Initially hidden

            // Add buttons to the FlowLayoutPanel
            buttonPanel.Controls.Add(showButton);
            buttonPanel.Controls.Add(stopButton);
            buttonPanel.Controls.Add(refreshButton);

            // Create PictureBox for preview
            pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;  // Maintain aspect ratio while filling available space
            pictureBox.Visible = false;

            // Add buttons and picture box to the layout panel
            layoutPanel.Controls.Add(buttonPanel, 0, 0);
            layoutPanel.Controls.Add(pictureBox, 0, 1);

            // Add the layout panel to the form
            this.Controls.Add(layoutPanel);

            // Initialize secondary form for the second monitor
            secondaryForm = new SecondaryMonitorForm();
        }

        private void CaptureAndDisplayScreenshot()
        {
            // Capture screenshot of the main monitor (Primary screen)
            Rectangle screenBounds = Screen.PrimaryScreen.Bounds;
            Bitmap screenshot = new Bitmap(screenBounds.Width, screenBounds.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(screenBounds.Location, Point.Empty, screenBounds.Size);
            }

            // Update PictureBox in the control window
            pictureBox.Image = screenshot;
            pictureBox.Visible = true;

            // Display the screenshot on the secondary monitor's full-screen form
            secondaryForm.ShowScreenshot(screenshot);
        }

        private void ShowToCustomerClicked(object sender, EventArgs e)
        {
            if (sharing)
                return;

            // Hide the control window before taking the screenshot
            this.Hide();
            System.Threading.Thread.Sleep(300);  // Small delay to ensure window is hidden

            // Capture screenshot of the main monitor (Primary screen)
            CaptureAndDisplayScreenshot();

            // Show the control window again
            this.Show();

            // Toggle visibility of buttons
            showButton.Visible = false;
            stopButton.Visible = true;
            refreshButton.Visible = true;

            sharing = true;
        }

        private void RefreshScreenshotClicked(object sender, EventArgs e)
        {
            if (!sharing)
                return;

            // Hide the control window before taking the screenshot
            this.Hide();
            System.Threading.Thread.Sleep(300);  // Small delay to ensure window is hidden

            // Refresh the screenshot (capture again)
            CaptureAndDisplayScreenshot();

            // Show the control window again
            this.Show();
        }

        private void StopSharingClicked(object sender, EventArgs e)
        {
            if (!sharing)
                return;

            // Clear the secondary monitor screenshot
            secondaryForm.ClearScreenshot();

            // Reset the control form to its initial state
            pictureBox.Visible = false;
            pictureBox.Image = null;

            showButton.Visible = true;
            stopButton.Visible = false;
            refreshButton.Visible = false;

            sharing = false;
        }



        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
