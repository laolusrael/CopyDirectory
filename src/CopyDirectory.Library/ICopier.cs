using System.ComponentModel;
using System.Threading.Tasks;

namespace CopyDirectory.Library
{
    /// <summary>
    /// Copies files from one directory to another and provides properties to monitor progress
    /// </summary>
    public interface ICopier: INotifyPropertyChanged
    {
        /// <summary>
        /// The Directory that is currently being copied into
        /// </summary>
        string CurrentDestinationDirectory { get; }
        /// <summary>
        /// The file being copied currently
        /// </summary>
        string CurrentFile { get; }
        /// <summary>
        /// The Directory being copied currently
        /// </summary>
        string CurrentSourceDirectory { get; }
        /// <summary>
        /// Readonly string specifying the location where files will be copied to
        /// </summary>
        string DestinationPath { get; }
        /// <summary>
        /// Readonly string specifying the source directory where files will be copied from.
        /// </summary>
        string SourcePath { get; }
        /// <summary>
        /// Copy all files from given location to specified destination
        /// </summary>
        /// <param name="source">Where the files will be copied from</param>
        /// <param name="destination">Where the files will be copied to</param>
        /// <exception cref="ArgumentNullException">Thrown when source or destination is not provided</exception>
        /// <exception cref="ArgumentException">Thrown when source and destination point to the same location</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown when source directory is does not exist</exception>
        Task CopyAsync(string source, string destination);
    }
}