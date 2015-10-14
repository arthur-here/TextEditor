using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextEditor;

namespace TextEditorTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class TextEditorDocumentTest
    {
        TextEditorDocument document;

        [TestInitialize]
        public void setUp()
        {
            this.document = new TextEditorDocument("example.txt");
            this.document.Lines.Add("hello");
            this.document.Lines.Add("world");
            this.document.Lines.Add("");
            this.document.Lines.Add("123");
        }

        [TestMethod]
        public void TextEditorDocument_AddLines()
        {
            Assert.IsNotNull(document, "Document wasn't created");
            Assert.IsTrue(document.Lines.Count == 4, "Lines weren't added to document");
            Assert.AreEqual(document.Lines[0], "hello", "Lines aren't added in right order");
            document.Lines.Clear();
            Assert.IsTrue(document.Lines.Count == 0, "Couldn't clear document's lines");
        }

        [TestMethod]
        public void TextEditorDocument_LineByCaretIndex()
        {
            Assert.AreEqual(0, document.LineNumberByIndex(0));
            Assert.AreEqual(0, document.LineNumberByIndex(5));
            Assert.AreEqual(1, document.LineNumberByIndex(6));
            Assert.AreEqual(1, document.LineNumberByIndex(11));
            Assert.AreEqual(2, document.LineNumberByIndex(12));
            Assert.AreEqual(3, document.LineNumberByIndex(13));
            Assert.AreEqual(3, document.LineNumberByIndex(16));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TextEditorDocument_LineByCaretIndex_WrongArgument()
        {
            document.LineNumberByIndex(-10);
        }

        [TestMethod]
        public void TextEditorDocument_CaretPositionByIndex()
        {
            Assert.AreEqual(0, this.document.CaretPositionInLineByIndex(0));
            Assert.AreEqual(3, this.document.CaretPositionInLineByIndex(3));
            Assert.AreEqual(5, this.document.CaretPositionInLineByIndex(5));
            Assert.AreEqual(0, this.document.CaretPositionInLineByIndex(6));
            Assert.AreEqual(3, this.document.CaretPositionInLineByIndex(9));
            Assert.AreEqual(5, this.document.CaretPositionInLineByIndex(11));
            Assert.AreEqual(0, this.document.CaretPositionInLineByIndex(12));
            Assert.AreEqual(0, this.document.CaretPositionInLineByIndex(13));
            Assert.AreEqual(3, this.document.CaretPositionInLineByIndex(16));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TextEditorDocument_CaretPositionByIndex_WrongArgument()
        {
            document.CaretPositionInLineByIndex(-10);
        }
    }
}
