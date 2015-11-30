using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextEditor.FileManager;
using System.Windows.Documents;
using System.Text;
using TextEditor;

namespace TextEditorTests
{
    [TestClass]
    public class FileManagerTests
    {
        ITextEditorDocument document;
        TextEditorFileManager fileManager;

        [TestInitialize]
        public void SetUp()
        {
            fileManager = new TextEditorFileManager();
        }

        [TestMethod]
        [DeploymentItem(@"Resources\DocumentExample.txt", "Resources")]
        public void TextEditorFileManager_OpenFileUsingEncoding()
        {
            this.document = this.fileManager.OpenFileUsingEncoding(@"Resources\DocumentExample.txt", Encoding.Default);
            Assert.IsNotNull(this.document, "FileManager couldn't open document");
            this.document = this.fileManager.OpenFileUsingEncoding(@"Resources\DocumentExample.txt", Encoding.ASCII);
            Assert.IsNotNull(this.document, "FileManager couldn't open document");
            this.document = this.fileManager.OpenFileUsingEncoding(@"Resources\DocumentExample.txt", Encoding.Unicode);
            Assert.IsNotNull(this.document, "FileManager couldn't open document");
            this.document = this.fileManager.OpenFileUsingEncoding(@"Resources\DocumentExample.txt", Encoding.UTF8);
            Assert.IsNotNull(this.document, "FileManager couldn't open document");
        }

        [TestMethod]
        [DeploymentItem(@"Resources\DocumentExample.txt", "Resources")]
        public void TextEditorFileManager_Save_Open()
        {
            document = fileManager.OpenFileUsingEncoding(@"Resources\DocumentExample.txt", Encoding.Default);
            Assert.IsNotNull(this.document, "FileManager couldn't open document");
            document.Lines.Add("Hello");
            fileManager.SaveDocument(document);
            document = fileManager.OpenFileUsingEncoding(@"Resources\DocumentExample.txt", Encoding.Default);
            Assert.IsTrue(document.Lines[document.Lines.Count - 1] == "Hello", "Changes to document haven't been saved by FileManager");
            document.Lines.RemoveAt(document.Lines.Count - 1);
            fileManager.SaveDocument(document);
        }
    }
}
