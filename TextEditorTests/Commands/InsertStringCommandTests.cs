using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextEditor;
using TextEditor.Commands;

namespace TextEditorTests
{
    [TestClass]
    public class InsertStringCommandTests
    {
        TextEditorDocument document;
        TextEditorDocument initialDocument;

        [TestInitialize]
        public void setUp()
        {
            this.document = new TextEditorDocument("example.txt");
            this.initialDocument = new TextEditorDocument("example.txt");
            this.document.Lines.Add("hello");
            this.document.Lines.Add("world");
            this.document.Lines.Add("");
            this.document.Lines.Add("123");
            this.document.Lines.ForEach(l => this.initialDocument.Lines.Add(l));
        }

        [TestMethod]
        public void InsertStringCommand_InsertCharacter()
        {
            InsertStringCommand command = new InsertStringCommand("a", 0);
            command.Execute(this.document);
            Assert.AreEqual("ahello", this.document.Lines[0]);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);

            command = new InsertStringCommand("a", 5);
            command.Execute(this.document);
            Assert.AreEqual("helloa", this.document.Lines[0]);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);

            command = new InsertStringCommand("a", 6);
            command.Execute(this.document);
            Assert.AreEqual("aworld", this.document.Lines[1]);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);

            command = new InsertStringCommand("a", 11);
            command.Execute(this.document);
            Assert.AreEqual("worlda", this.document.Lines[1]);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);

            command = new InsertStringCommand("a", 12);
            command.Execute(this.document);
            Assert.AreEqual("a", this.document.Lines[2]);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);

            command = new InsertStringCommand("a", 13);
            command.Execute(this.document);
            Assert.AreEqual("a123", this.document.Lines[3]);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);

            command = new InsertStringCommand("a", 16);
            command.Execute(this.document);
            Assert.AreEqual("123a", this.document.Lines[3]);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InsertStringCommand_InsertCharacter_InalidCaretIndex()
        {
            InsertStringCommand command = new InsertStringCommand("a", 17);
            command.Execute(this.document);
        }
    }
}
