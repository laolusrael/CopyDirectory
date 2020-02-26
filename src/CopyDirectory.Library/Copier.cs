using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CopyDirectory.Library
{
    /// <summary>
    /// Copies files from one directory to another and provides properties to monitor progress
    /// </summary>
    public class Copier : ICopier
    {
        private string _source;
        private string _destination;

        private string _currentFile;
        private string _currentSourceDirectory;
        private string _currentDestinationDirectory;

        public Copier()
        {

        }

        /// <summary>
        /// Readonly string specifying the source directory where files will be copied from.
        /// </summary>
        public string SourcePath
        {
            get => _source;
            private set
            {
                _source = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Readonly string specifying the location where files will be copied to
        /// </summary>
        public string DestinationPath
        {
            get => _destination;
            private set
            {
                _destination = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// The file being copied currently
        /// </summary>
        public string CurrentFile
        {
            get => _currentFile;
            private set
            {
                _currentFile = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// The Directory being copied currently
        /// </summary>
        public string CurrentSourceDirectory
        {
            get => _currentSourceDirectory;
            private set
            {
                _currentSourceDirectory = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// The Directory that is currently being copied into
        /// </summary>
        public string CurrentDestinationDirectory
        {
            get => _currentDestinationDirectory;
            private set
            {
                _currentDestinationDirectory = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Changes in the Copier are published here.
        /// Add a handler here to be notified of changes to the properties (DestinationPath, CurrentFile, etc)
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Copy all files from given location to specified destination
        /// </summary>
        /// <param name="source">Where the files will be copied from</param>
        /// <param name="destination">Where the files will be copied to</param>
        /// <exception cref="ArgumentNullException">Thrown when source or destination is not provided</exception>
        /// <exception cref="ArgumentException">Thrown when source and destination point to the same location</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown when source directory is does not exist</exception>
        public async Task CopyAsync(string source, string destination)
        {

            if (string.IsNullOrEmpty(source))
                throw new ArgumentNullException(nameof(source));


            if (Directory.Exists(source) == false)
                throw new DirectoryNotFoundException(source);


            SourcePath = source;
            

            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (source.Equals(destination))
                throw new ArgumentException("source and destination must not point to the same location on disk");


            DestinationPath = destination;

            await _Copy(_source, _destination);

        }

        /// <summary>
        /// Recursively copy files from source path to destination.
        /// </summary>
        /// <param name="source">Location of files to copy</param>
        /// <param name="destination">Where the files will be copied to</param>
        private async Task _Copy(string source, string destination)
        {

            // Create destination if it doesn't exist
            if (Directory.Exists(destination) == false)
            {
                Directory.CreateDirectory(destination);
            }

            // Update current directories beign copied so listeners can be informed
            CurrentSourceDirectory = source;
            CurrentDestinationDirectory = destination;


            // still check if user is trying to copy  a file to a location 
            // because I don't trust the response from Directory.Exists() to not allow a file pass through
            if((File.GetAttributes(source) & FileAttributes.Directory) != FileAttributes.Directory)
            {
                if ((File.GetAttributes(destination) & FileAttributes.Directory) != FileAttributes.Directory)
                    destination = Path.GetDirectoryName(destination);

                await _CopyFileAsync(source, destination);
                return;
            }



            foreach (var entry in Directory.EnumerateFileSystemEntries(source))
            {
                if ((File.GetAttributes(entry) & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    // This is a directory, recursively process it
                    await _Copy(entry, Path.Combine(destination, new DirectoryInfo(entry).Name));
                }
                else
                {
                    // This is a file copy it
                    await _CopyFileAsync(entry, destination);
                }

            }

        }

        private async Task _CopyFileAsync(string filePath, string destinationPath)
        {
            using (FileStream sourceStream = new FileStream(filePath, FileMode.Open))
            {
                using (FileStream destinationStream = new FileStream(Path.Combine(destinationPath, Path.GetFileName(filePath)), FileMode.Create))
                {
                    CurrentFile = Path.GetFileName(filePath); // Update the current file being copied so that listeners can be aware
                    await sourceStream.CopyToAsync(destinationStream);
                }

            }

        }
        /// <summary>
        /// Event dispatcher for property changes
        /// </summary>
        /// <param name="propertyName"></param>
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {

            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
