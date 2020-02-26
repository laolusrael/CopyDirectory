using CopyDirectory.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CopyDirectory.Test
{
    [TestClass]
    public class CopierTest
    {


        private string _source;
        private string _destination;
        private ICopier _copier;

        [TestInitialize]
        public void Initialize()
        {
            _source = "";
            _destination = "";

            _copier = new Copier();
        }

        [TestMethod]
        public void ThrowArgumentNull_If_Source_Is_Empty()
        {
            Task.Run(() =>
            {
                Assert.ThrowsException<ArgumentNullException>(async () => await _copier.CopyAsync(_source, _destination));
            });

        }


        [TestMethod]
        public void ThrowArgumentNull_If_Destination_Is_Empty()
        {

            var source = Directory.GetDirectoryRoot(Path.GetRandomFileName());

            _destination = "";

            Task.Run(() =>
            {
                Assert.ThrowsException<ArgumentNullException>(async () => await _copier.CopyAsync(source, _destination));
            });
        }

        [TestMethod]
        public void ThrowDirectoryNotFound_If_SourcePath_Does_Not_Exist()
        {

            var source = "C:\asdfla";
            var copier = new Copier();

            Task.Run(() =>
            {
                Assert.ThrowsException<DirectoryNotFoundException>(async () => await copier.CopyAsync(source, _destination));
            });

        }

    }
}
