using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextEditor;
using System.Collections.Generic;
using TextEditor.Commands;

namespace TextEditorTests.Commands
{
    [TestClass]
    public class InsertLinesCommandTests
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
        public void InsertLinesCommand_Insert()
        {
            List<string> lines = new List<string>()
            {
                "insert", "some", " ", "lines"
            };
            InsertLinesCommand command = new InsertLinesCommand(lines, 9);
            command.Execute(this.document);
            List<string> expected = new List<string>()
            {
                "hello", "worinsert", "some", " ", "linesld", "", "123"
            };
            string expectedString = string.Join("\n", expected);
            Assert.AreEqual(expectedString, this.document.Text);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);

            command = new InsertLinesCommand(lines, 16);
            command.Execute(this.document);
            expected = new List<string>()
            {
                "hello", "world", "", "123insert", "some", " ", "lines"
            };
            expectedString = string.Join("\n", expected);
            Assert.AreEqual(expectedString, this.document.Text);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);

            command = new InsertLinesCommand(lines, 0);
            command.Execute(this.document);
            expected = new List<string>()
            {
                "insert", "some", " ", "lineshello", "world", "", "123"
            };
            expectedString = string.Join("\n", expected);
            Assert.AreEqual(expectedString, this.document.Text);
            command.Undo();
            Assert.AreEqual(this.initialDocument.Text, this.document.Text);
        }
    }
}
