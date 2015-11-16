using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextEditor;
using System.Collections.Generic;

namespace TextEditorTests
{
    [TestClass]
    public class SnippetLibraryTests
    {
        SnippetLibrary snippetLibrary;

        [TestInitialize]
        public void setUp()
        {
            this.snippetLibrary = new SnippetLibrary();
            this.snippetLibrary.RemoveAll();
        }

        [TestMethod]
        [DeploymentItem(@"Resources\SnippetLibrary.xml", "UserData")]
        public void SnippetLibrary_Add()
        {
            List<string> s1Content = new List<string>()
            {
                "lorem ipsum dot sit amet,",
                "consectetur adipisicing elit"
            };
            Snippet s1 = new Snippet("lorem", s1Content);
            snippetLibrary.Add(s1);
            this.snippetLibrary = null;
            this.snippetLibrary = new SnippetLibrary();
            Assert.IsNotNull(this.snippetLibrary.GetByName("lorem"));
        }
        
        [TestMethod]
        [DeploymentItem(@"Resources\SnippetLibrary.xml", "UserData")]
        public void SnippetLibrary_Remove()
        {
            List<string> s1Content = new List<string>()
            {
                "lorem ipsum dot sit amet,",
                "consectetur adipisicing elit"
            };
            Snippet s1 = new Snippet("lorem", s1Content);
            List<string> s2Content = new List<string>()
            {
                "if ()",
                "{",
                "",
                "}"
            };
            Snippet s2 = new Snippet("if", s2Content);

            snippetLibrary.Add(s1);
            snippetLibrary.Add(s2);
            this.snippetLibrary = null;
            this.snippetLibrary = new SnippetLibrary();
            this.snippetLibrary.Remove("lorem");
            Assert.IsNull(this.snippetLibrary.GetByName("lorem"));
            Assert.AreEqual(1, this.snippetLibrary.Names.Count);
        }
    }
}
