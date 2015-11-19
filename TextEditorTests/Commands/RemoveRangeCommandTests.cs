using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextEditor;
using TextEditor.Commands;
using System.Collections.Generic;

namespace TextEditorTests
{
    [TestClass]
    public class RemoveRangeCommandTests
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
        public void RemoveRangeCommand_RemoveCharacter()
        {
            RemoveRangeCommand command = new RemoveRangeCommand(0, 1);
            command.Execute(this.document);
            Assert.AreEqual("ello", this.document.Lines[0]);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);

            command = new RemoveRangeCommand(4, 1);
            command.Execute(this.document);
            Assert.AreEqual("hell", this.document.Lines[0]);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);

            command = new RemoveRangeCommand(5, 1);
            command.Execute(this.document);
            Assert.AreEqual("helloworld", this.document.Lines[0]);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);

            command = new RemoveRangeCommand(11, 1);
            command.Execute(this.document);
            Assert.AreEqual(3, this.document.Lines.Count);
            Assert.AreEqual("hello\nworld\n123", this.document.Text);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);

            command = new RemoveRangeCommand(12, 1);
            command.Execute(this.document);
            Assert.AreEqual(3, this.document.Lines.Count);
            Assert.AreEqual("hello\nworld\n123", this.document.Text);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);

            command = new RemoveRangeCommand(this.document.Text.Length, 1);
            command.Execute(this.document);
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RemoveRangeCommand_RemoveCharacter_InvalidCaretIndex()
        {
            RemoveRangeCommand command = new RemoveRangeCommand(this.document.Text.Length + 1, 1);
            command.Execute(this.document);
        }

        [TestMethod]
        public void RemoveRangeCommand_RemoveLines()
        {
            RemoveRangeCommand command = new RemoveRangeCommand(3, 5);
            command.Execute(this.document);
            List<string> expected = new List<string>()
            { "helrld", "", "123" };
            string expectedString = string.Join("\n", expected);
            Assert.AreEqual(expectedString, this.document.Text);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);

            command = new RemoveRangeCommand(0, this.document.Text.Length);
            command.Execute(this.document);
            Assert.AreEqual("", this.document.Text);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);

            command = new RemoveRangeCommand(12, 4);
            command.Execute(this.document);
            expected = new List<string>()
            { "hello", "world", "" };
            expectedString = string.Join("\n", expected);
            Assert.AreEqual(expectedString, this.document.Text);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);

            command = new RemoveRangeCommand(11, 5);
            command.Execute(this.document);
            expected = new List<string>()
            { "hello", "world" };
            expectedString = string.Join("\n", expected);
            Assert.AreEqual(expectedString, this.document.Text);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);
        }
    }
}
