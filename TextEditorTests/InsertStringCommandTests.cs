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
            InsertStringCommand command = new InsertStringCommand("a", this.document, 0);
            command.Execute();
            Assert.AreEqual("ahello", this.document.Lines[0]);
            command = new InsertStringCommand("a", this.document, 6);
            command.Execute();
            Assert.AreEqual("ahelloa", this.document.Lines[0]);
            command = new InsertStringCommand("a", this.document, 7);
            command.Execute();
            Assert.AreEqual("ahelloaa", this.document.Lines[0]);
            command = new InsertStringCommand("a", this.document, 9);
            command.Execute();
            Assert.AreEqual("aworld", this.document.Lines[1]);
            command = new InsertStringCommand("a", this.document, 15);
            command.Execute();
            Assert.AreEqual("aworlda", this.document.Lines[1]);
            command = new InsertStringCommand("a", this.document, 16);
            command.Execute();
            Assert.AreEqual("aworldaa", this.document.Lines[1]);
            command = new InsertStringCommand("a", this.document, 18);
            command.Execute();
            Assert.AreEqual("a", this.document.Lines[2]);
            command = new InsertStringCommand("a", this.document, 19);
            command.Execute();
            Assert.AreEqual("aa", this.document.Lines[2]);
            command = new InsertStringCommand("a", this.document, 20);
            command.Execute();
            Assert.AreEqual("aaa", this.document.Lines[2]);
            Assert.AreEqual(4, this.document.Lines.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InsertStringCommand_InsertCharacter_InalidCaretIndex()
        {
            InsertStringCommand command = new InsertStringCommand("a", this.document, 17);
            command.Execute();
        }
    }
}
