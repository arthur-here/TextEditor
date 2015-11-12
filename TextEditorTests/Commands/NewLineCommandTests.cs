using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextEditor;
using TextEditor.Commands;

namespace TextEditorTests
{
    [TestClass]
    public class NewLineCommandTests
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
        public void NewLineCommand_AddNewLine()
        {
            NewLineCommand command = new NewLineCommand(this.document, 0);
            command.Execute();
            Assert.AreEqual(5, this.document.Lines.Count);
            Assert.AreEqual("", this.document.Lines[0]);
            Assert.AreEqual(this.initialDocument.Lines[0], this.document.Lines[1]);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);

            command = new NewLineCommand(this.document, 2);
            command.Execute();
            Assert.AreEqual(5, this.document.Lines.Count);
            Assert.AreEqual("he", this.document.Lines[0]);
            Assert.AreEqual("llo", this.document.Lines[1]);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);

            command = new NewLineCommand(this.document, 5);
            command.Execute();
            Assert.AreEqual(5, this.document.Lines.Count);
            Assert.AreEqual("hello", this.document.Lines[0]);
            Assert.AreEqual("", this.document.Lines[1]);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);

            command = new NewLineCommand(this.document, 6);
            command.Execute();
            Assert.AreEqual(5, this.document.Lines.Count);
            Assert.AreEqual("", this.document.Lines[1]);
            Assert.AreEqual("world", this.document.Lines[2]);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);

            command = new NewLineCommand(this.document, 11);
            command.Execute();
            Assert.AreEqual(5, this.document.Lines.Count);
            Assert.AreEqual("", this.document.Lines[2]);
            Assert.AreEqual("", this.document.Lines[3]);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);

            command = new NewLineCommand(this.document, this.document.Text.Length);
            command.Execute();
            Assert.AreEqual(5, this.document.Lines.Count);
            Assert.AreEqual("123", this.document.Lines[3]);
            Assert.AreEqual("", this.document.Lines[4]);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NewLineCommand_AddNewLine_WrongCaretIndex()
        {
            NewLineCommand command = new NewLineCommand(this.document, this.document.Text.Length + 1);
            command.Execute();
        }
    }
}
