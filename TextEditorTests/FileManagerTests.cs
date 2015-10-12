using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextEditor.FileManager;
using System.Windows.Documents;
using System.Text;

namespace TextEditorTests
{
    [TestClass]
    public class FileManagerTests
    {
        TextEditorDocument document;
        TextEditorFileManager fileManager;

        [TestInitialize]
        public void SetUp()
        {
            fileManager = new TextEditorFileManager();
        }

        [TestMethod]
        public void TextEditorDocument_Lines()
        {
            document = new TextEditorDocument("DocumentExample.txt");

            Assert.IsNotNull(document, "Document wasn't created");
            document.Blocks.Add(new Paragraph(new Run("hello")));
            document.Blocks.Add(new Paragraph(new Run("world")));
            Assert.IsTrue(document.LinesCount == 2, "Couldn't add lines to document");
            Assert.AreEqual(document.Lines[0], "hello", "Lines aren't added in right order");
            document.Blocks.Clear();
            Assert.IsTrue(document.LinesCount == 0, "Couldn't clear document's lines");
        }

        [TestMethod]
        [DeploymentItem(@"Resources\DocumentExample.txt", "Resources")]
        public void TextEditorFileManager_OpenFileUsingEncoding()
        {
            this.document = this.fileManager.OpenFileUsingEncoding(@"Resources\DocumentExample.txt", Encoding.Default);
            Assert.IsNotNull(this.document, "FileManager couldn't open document");
        }

        [TestMethod]
        [DeploymentItem(@"Resources\DocumentExample.txt", "Resources")]
        public void TextEditorFileManager_Save_Open()
        {
            document = fileManager.OpenFileUsingEncoding(@"Resources\DocumentExample.txt", Encoding.Default);
            Assert.IsNotNull(this.document, "FileManager couldn't open document");
            document.Blocks.Add(new Paragraph(new Run("Hello")));
            fileManager.SaveDocument(document);
            document = fileManager.OpenFileUsingEncoding(@"Resources\DocumentExample.txt", Encoding.Default);
            Assert.IsTrue(document.Lines[document.LinesCount - 1] == "Hello", "Changes to document haven't been saved by FileManager");
            document.Blocks.Remove(document.Blocks.LastBlock);
            fileManager.SaveDocument(document);
        }
    }
}
