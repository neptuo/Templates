﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neptuo.Text.ComponentModel;
using Neptuo.Text.IO;
using Neptuo.Text.Positions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Templates.Parsers.Tokenizers.IO
{
    [TestClass]
    public class ContentDecorator_General : TestBase
    {
        private void AssertSourceContentInfo(ITextSpan contentInfo, int startIndex, int length)
        {
            AssertAreEqual(contentInfo.StartIndex, startIndex);
            AssertAreEqual(contentInfo.Length, length);
        }

        private void AssertSourceRangeLineInfo(IDocumentSpan lineInfo, int columnIndex, int lineIndex, int endColumnIndex, int endLineIndex)
        {
            AssertAreEqual(lineInfo.ColumnIndex, columnIndex);
            AssertAreEqual(lineInfo.LineIndex, lineIndex);
            AssertAreEqual(lineInfo.EndColumnIndex, endColumnIndex);
            AssertAreEqual(lineInfo.EndLineIndex, endLineIndex);
        }

        [TestMethod]
        public void Decorator_BaseSingleLine()
        {
            ContentDecorator contentReader = new ContentDecorator(new StringReader("abc abc"));
            
            contentReader.Next();
            contentReader.Next();

            // 'abc'
            AssertSourceContentInfo(contentReader.CurrentContentInfo(), 0, 3);
            AssertSourceRangeLineInfo(contentReader.CurrentLineInfo(), 0, 0, 3, 0);
            AssertAreEqual(contentReader.CurrentContent(), "abc");
            contentReader.ResetCurrentInfo();

            contentReader.Next();

            // ' '
            AssertSourceContentInfo(contentReader.CurrentContentInfo(), 3, 1);
            AssertSourceRangeLineInfo(contentReader.CurrentLineInfo(), 3, 0, 4, 0);
            AssertAreEqual(contentReader.CurrentContent(), " ");
            contentReader.ResetCurrentInfo();

            contentReader.Next();
            contentReader.Next();
            contentReader.Next();

            // 'abc'
            AssertSourceContentInfo(contentReader.CurrentContentInfo(), 4, 3);
            AssertSourceRangeLineInfo(contentReader.CurrentLineInfo(), 4, 0, 7, 0);
            AssertAreEqual(contentReader.CurrentContent(), "abc");
        }

        [TestMethod]
        public void Decorator_BaseMultiLine()
        {
            ContentDecorator contentReader = new ContentDecorator(new StringReader("abc abc" + '\n' + "abc abc"));

            contentReader.Next(); // 'b'
            contentReader.Next(); // 'c'
            contentReader.Next(); // ' '
            contentReader.Next(); // 'a'
            contentReader.Next(); // 'b'
            contentReader.Next(); // 'c'
            contentReader.Next(); // '\n'
            contentReader.Next(); // 'a'

            // 'abc abc\na'
            AssertSourceContentInfo(contentReader.CurrentContentInfo(), 0, 9);
            AssertSourceRangeLineInfo(contentReader.CurrentLineInfo(), 0, 0, 1, 1);
            AssertAreEqual(contentReader.CurrentContent(), "abc abc" + '\n' + "a");
            contentReader.ResetCurrentInfo();

            contentReader.Next(); // 'b'
            contentReader.Next(); // 'c'
            contentReader.Next(); // ' '
            contentReader.Next(); // 'a'
            contentReader.Next(); // 'b'
            contentReader.Next(); // 'c'

            // 'bc abc'
            AssertSourceContentInfo(contentReader.CurrentContentInfo(), 9, 6);
            AssertSourceRangeLineInfo(contentReader.CurrentLineInfo(), 1, 1, 7, 1);
            AssertAreEqual(contentReader.CurrentContent(), "bc abc");
            contentReader.ResetCurrentInfo();
        }

        [TestMethod]
        public void Decorator_ResetPosition()
        {
            ContentDecorator contentReader = new ContentDecorator(new StringReader("abc abc" + '\n' + "abc abc"));

            // 'a'
            AssertSourceContentInfo(contentReader.CurrentContentInfo(), 0, 1);
            AssertSourceRangeLineInfo(contentReader.CurrentLineInfo(), 0, 0, 1, 0);
            AssertAreEqual(contentReader.CurrentContent(), "a");
            AssertAreEqual(contentReader.Current, 'a');

            contentReader.Next();
            contentReader.Next();

            // 'abc'
            AssertSourceContentInfo(contentReader.CurrentContentInfo(), 0, 3);
            AssertSourceRangeLineInfo(contentReader.CurrentLineInfo(), 0, 0, 3, 0);
            AssertAreEqual(contentReader.CurrentContent(), "abc");

            contentReader.ResetCurrentPosition(2);

            // 'a'
            AssertSourceContentInfo(contentReader.CurrentContentInfo(), 0, 1);
            AssertSourceRangeLineInfo(contentReader.CurrentLineInfo(), 0, 0, 1, 0);
            AssertAreEqual(contentReader.CurrentContent(), "a");
            AssertAreEqual(contentReader.Current, 'a');

            contentReader.Next();
            contentReader.Next();
            contentReader.Next();

            // 'abc '
            AssertSourceContentInfo(contentReader.CurrentContentInfo(), 0, 4);
            AssertSourceRangeLineInfo(contentReader.CurrentLineInfo(), 0, 0, 4, 0);
            AssertAreEqual(contentReader.CurrentContent(), "abc ");
            contentReader.ResetCurrentInfo();

            contentReader.Next(); // 'a'
            contentReader.Next(); // 'b'
            contentReader.Next(); // 'c'
            contentReader.Next(); // '\n'
            contentReader.Next(); // 'a'

            // 'abc\na'
            AssertSourceContentInfo(contentReader.CurrentContentInfo(), 4, 5);
            AssertSourceRangeLineInfo(contentReader.CurrentLineInfo(), 4, 0, 1, 1);
            AssertAreEqual(contentReader.CurrentContent(), "abc" + '\n' + "a");

            contentReader.ResetCurrentPosition(2);

            // abc
            AssertSourceContentInfo(contentReader.CurrentContentInfo(), 4, 3);
            AssertSourceRangeLineInfo(contentReader.CurrentLineInfo(), 4, 0, 7, 0);
            AssertAreEqual(contentReader.CurrentContent(), "abc");
        }

        [TestMethod]
        public void Decorator_ResetPositionToInvalid()
        {
            ContentDecorator contentReader = new ContentDecorator(new StringReader("abc abc" + '\n' + "abc abc"));

            // 'a'
            AssertSourceContentInfo(contentReader.CurrentContentInfo(), 0, 1);
            AssertSourceRangeLineInfo(contentReader.CurrentLineInfo(), 0, 0, 1, 0);
            AssertAreEqual(contentReader.CurrentContent(), "a");
            AssertAreEqual(contentReader.Current, 'a');

            contentReader.Next();
            contentReader.Next();

            AssertAreEqual(contentReader.ResetCurrentPosition(2), true);
            AssertAreEqual(contentReader.Current, 'a');
            AssertAreEqual(contentReader.ResetCurrentPosition(1), true);
            AssertAreEqual(contentReader.Current, ContentReader.EndOfInput);
            AssertAreEqual(contentReader.ResetCurrentPosition(1), false);

        }
    }
}
