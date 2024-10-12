namespace SnapshotShare
{
    public partial class SecondaryMonitorForm : Form
    {
        private PictureBox secondaryPictureBox;

        public SecondaryMonitorForm()
        {
            InitializeComponent();

            Screen secondaryScreen = Screen.AllScreens.Length > 1 ? Screen.AllScreens[1] : Screen.PrimaryScreen;

            this.StartPosition = FormStartPosition.Manual;
            this.Location = secondaryScreen.Bounds.Location;
            this.Size = secondaryScreen.Bounds.Size;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.BackColor = Color.Black;

            secondaryPictureBox = new PictureBox();
            secondaryPictureBox.Dock = DockStyle.Fill;
            secondaryPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            this.Controls.Add(secondaryPictureBox);
        }

        public void ShowScreenshot(Image screenshot)
        {
            this.Show();
            secondaryPictureBox.Image = screenshot;
        }

        // Method to clear the screenshot
        public void ClearScreenshot()
        {
            secondaryPictureBox.Image = null;
            this.Hide();
        }

        private void ImageDisplayerForm_Load(object sender, EventArgs e)
        {

        }
    }
}
