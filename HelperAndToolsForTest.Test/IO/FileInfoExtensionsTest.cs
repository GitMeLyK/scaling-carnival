﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HelperAndToolsForUT.Helper.Extensions.IOExtensions;

namespace HelperAndToolsForTest.Test.IO
{
    [TestFixture]
    class FileInfoExtensionsTest
    {
        [Test]
        public void FileInfo_ExtensionIO_IsPathQualificableFull_Valid()
        {
            // Arrange
            //      On Windows system check path with qualified full name incluse UNC root c: d: etc..
            List<string> ValidFullquelifiedPaths = new List<String>(new String[] {
                    @"c:\fold\subfold\",            @"d:\fold\subfold\sub\",
                    @"c:\fold\subfold\",            @"d:\fold\subfold\sub\\\\\",                
                    @"x:\\sharedlan\subfold\",      @"y:\\sharedlan\subfold\subfold\sub\",
                    @"x:\\\sharedlan\subfold\",     @"y:\\sharedlan\\\\\subfold\subfold\sub\",
                });
            // ** On presence of multislahs on path example with /////// // FileInfo render ok for normalization!!

            // Act  :: Check if is Full Path Qualified for Scope Directory ::
            foreach (string tpath in ValidFullquelifiedPaths) {
                // Assert
                Assert.IsTrue(new FileInfo(tpath).IsPathQualificableFull(TypePathSupposed.ForFolders, out string errororwarning));
                Assert.IsNull(errororwarning);
            }
        }

        [Test]
        public void FileInfo_ExtensionIO_IsPathQualificableFull_NotValid()
        {
            // Arrange
            //      On Windows system check error on path with qualified full name incluse UNC root c: d: etc..
            List<string> ValidFullquelifiedPaths = new List<String>(new String[] {
                    @"c:\fold\subfold",             @"d:\fold\subfold\\sub",
                    @"x:\\\sharedlan\subfold",      
                });

            // Act  :: Check if is Full Path Qualified for Scope Directory ::
            foreach (string tpath in ValidFullquelifiedPaths)
            {
                // Assert
                Assert.IsTrue(new FileInfo(tpath).IsPathQualificableFull(TypePathSupposed.ForFolders, out string errororwarning));
                Assert.IsNull(errororwarning);
            }

            // Arrange
            //      On Windows system check error on path with qualified full name incluse UNC root c: d: etc..
            List<string> NotValidFullquelifiedPaths = new List<String>(new String[] {
                    @"ac:\fold\subfold\",           @"d\fold\subfold\\sub\",
                    @"fold\subfold\\sub\",          @"ac:\fold\subfold\",
                    @"yz:\\sharedlan\subfold\subfold\sub\",
                });

            // Act  :: Check if is Full Path Qualified for Scope Directory ::
            foreach (string tpath in NotValidFullquelifiedPaths)
            {
                // Assert
                Assert.IsFalse(new FileInfo(tpath).IsPathQualificableFull(TypePathSupposed.ForFolders, out string errororwarning));
                Assert.IsNull(errororwarning);
            }

            string tpath2 = @"\fold\subfold\";
            Assert.IsFalse(new FileInfo(tpath2).IsPathQualificableFull(TypePathSupposed.ForFolders, out string errororwarning2));
            Assert.IsNull(errororwarning2);

        }

        [Test]
        public void FileInfo_ExtensionIO_IsPathQualificableFull_NotValid_ForChars()
        {
            // Arrange
            //      On Windows system check error on path with qualified full name incluse UNC root c: d: etc..
            List<string> ValidFullquelifiedPaths = new List<String>(new String[] {
                    @"d::\fold\subfold\\sub\",
                    @"c::\\\\\fold\subfold\"
                });

            // Act  :: Check if is Full Path Qualified for Scope Directory ::
            foreach (string tpath in ValidFullquelifiedPaths)
            {
                // Assert (not valid
                Assert.IsFalse(new FileInfo(tpath).IsPathQualificableFull( TypePathSupposed.ForFolders, out string errororwarning));
                Assert.IsNotEmpty(errororwarning);
            }

            // Arrange
            //      On Windows system check error on path with qualified full name incluse UNC root c: d: etc..
            List<string> ValidFullquelifiedPaths2 = new List<String>(new String[] {
                    @"c:\fo:ld\subfold\", @"d:\fo|ld\subfold\\sub\"
                });

            // Act  :: Check if is Full Path Qualified for Scope Directory ::
            foreach (string tpath in ValidFullquelifiedPaths2)
            {
                // Assert (Valid for path with error on chars)
                Assert.IsTrue(new FileInfo(tpath).IsPathQualificableFull(TypePathSupposed.ForFolders, out string errororwarning));
                TestContext.WriteLine(errororwarning);
                Assert.IsNotEmpty(errororwarning);
            }


        }


        [Test]
        public void FileInfo_ExtensionIO_WhenUsePathCompleteWith_GetFileNameWithoutExtension()
        {
            // :: Check extension on path with file
            string tFile = new FileInfo("testDir/test.txt").GetFileNameWithoutExtension();
            Assert.AreEqual(tFile, "test");
        }

        [Test]
        public void FileInfo_ExtensionIO_WhenUseWith_GetFileNameWithExtension()
        {
            // :: Check extension on path with file
            string tFile = new FileInfo("test.txt").GetFileNameWithoutExtension();
            Assert.AreEqual(tFile, "test");
        }

        [Test]
        public void FileInfo_ExtensionIO_WhenUseWith_GetFileNameWithoutExtension()
        {
            // :: Check extension on path with file
            string tFile = new FileInfo("test").GetFileNameWithoutExtension();
            Assert.AreEqual(tFile, "test");
        }

        [Test]
        public void FileInfo_ExtensionIO_WhenUseWith_GetFileNameWithoutExtension_Erroneed()
        {
            // :: Check extension on path with file
            string tFile = new FileInfo("test.").GetFileNameWithoutExtension();
            Assert.AreEqual(tFile, "test");
        }

        [Test]
        public void FileInfo_ExtensionIO_WhenUseWith_GetFileNameWithoutExtension_ErroneedDoubleSeparator()
        {
            // :: Check extension on path with file
            string tFile = new FileInfo("test.ciao.test").GetFileNameWithoutExtension();
            Assert.AreEqual(tFile, "test.ciao");    // Is a valid name for file

            tFile = new FileInfo("test.ciao.test.").GetFileNameWithoutExtension();
            Assert.AreEqual(tFile, "test.ciao");    // Is a valid name for file

        }

        [Test]
        public void FileInfo_ExtensionIO_WhenUseWith_GetFileNameWithoutExtension_ErroneedPath()
        {
            // :: Check extension on path with file
            string tFile = new FileInfo("test/").GetFileNameWithoutExtension();
            Assert.AreEqual(tFile, "");

            tFile = new FileInfo("testdir.t/").GetFileNameWithoutExtension();
            Assert.AreEqual(tFile, "");

        }

        [Test]
        public void FileInfo_ExtensionIO_WhenUseWith_GetFileNameWithoutExtension_ErroneedPaths()
        {
            // :: Check extension on path with file
            string tFile = new FileInfo("test/pippo/.").GetFileNameWithoutExtension();
            Assert.AreEqual(tFile, "pippo");    // This is a directory

            tFile = new FileInfo("test/pippo/..").GetFileNameWithoutExtension();
            Assert.AreEqual(tFile, "test");     // This is a directory

        }



    }
}
