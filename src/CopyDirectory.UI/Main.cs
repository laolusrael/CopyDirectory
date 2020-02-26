using CopyDirectory.Library;
using NLog;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace CopyDirectory.UI
{
    public partial class Main : Form
    {
        private ICopier _copierInstance;
        private ILogger _logger;


        public Main(ICopier copier, ILogger logger)
        {
            InitializeComponent();

            
            _copierInstance = copier;
            _logger = logger;


            // Watch for changes in the copier
            _copierInstance.PropertyChanged += _copierInstance_PropertyChanged;
        }

        private void _copierInstance_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Console.WriteLine($"{e.PropertyName} has changed");

            switch (e.PropertyName)
            {

                case nameof(_copierInstance.CurrentDestinationDirectory):
                    lblTo.Text = _copierInstance.CurrentDestinationDirectory;
                    break;
                case nameof(_copierInstance.CurrentSourceDirectory):
                    lblFrom.Text = _copierInstance.CurrentSourceDirectory;
                    break;
                case nameof(_copierInstance.CurrentFile):
                    lblCurrentFile.Text = _copierInstance.CurrentFile;
                    break;
            }
        }

        private void btnSource_Click(object sender, EventArgs e)
        {

            if(sourceFolderDialog.ShowDialog(this) == DialogResult.OK)
            {
                txtSourcePath.Text = sourceFolderDialog.SelectedPath;
            }
            
        }

        private void btnDestination_Click(object sender, EventArgs e)
        {
            if(destinationFolderDialog.ShowDialog(this) == DialogResult.OK)
            {
                txtDestinationPath.Text = destinationFolderDialog.SelectedPath;
            }
        }

        private void _DisableButtons()
        {

            btnStartCopy.Enabled = false;
            btnSource.Enabled = false;
            btnDestination.Enabled = false;

        }
        private void _EnableButtons()
        {
            btnStartCopy.Enabled = true;
            btnSource.Enabled = true;
            btnDestination.Enabled = true;
        }

        private async void btnStartCopy_Click(object sender, EventArgs e)
        {
            _DisableButtons();
            copyProgressBar.Style = ProgressBarStyle.Marquee;

            try
            {
                _logger.Info($"Copying from source: {txtSourcePath.Text} :: destination: {txtDestinationPath.Text}");

                await _copierInstance.CopyAsync(txtSourcePath.Text, txtDestinationPath.Text);

                _logger.Info($"Completed");

                MessageBox.Show(this, "Completed!", "CopyDirectory", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch(Exception ex)
            {
                _logger.Debug(ex);
                MessageBox.Show(this, ex.Message, "CopyDirectory", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            copyProgressBar.Style = ProgressBarStyle.Continuous;

            _EnableButtons();

        }
    }
}
