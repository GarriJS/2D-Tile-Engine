using Engine.Physics.Models;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Controls.Typing.Models
{
    /// <summary>
    /// Represents a text editor.
    /// </summary>
    sealed public class TextEditor
    {
        /// <summary>
        /// Gets the text lines.
        /// </summary>
        required public List<TextLine> TextLines { get; init; }

        /// <summary>
        /// Modifies the text from the keys.
        /// </summary>
        /// <param name="textState">The text edit state.</param>
        /// <param name="textCursorPosition">The text cursor position.</param>
        /// <param name="freshKeys">The fresh keys.</param>
        /// <param name="pressedKeys">The pressed keys.</param>
        /// <returns>The text edit state.</returns>
        public TextEditState ModifyTextLines(TextConstraints textState, TextPosition textCursorPosition, List<Keys> freshKeys, List<ElapsedTimeExtender<Keys>> pressedKeys)
        {
            if (0 == this.TextLines.Count)
                return default;

            var textEditState = new TextEditState
            {
                MaxLineCharacterCount = textState.MaxLineCharacterCount,
                MaxLinesCount = textState.MaxLinesCount,
                TextCursorPosition = textCursorPosition,
                StartAnchor = default,
                EndAnchor = default,
                TextHighlightingState = textState.TextHighlightingState
            };
            var movementResult = this.ProcessTextCursorMovement(textEditState, freshKeys, pressedKeys);
            var deleteResult = this.ProcessTextDelete(movementResult, freshKeys, pressedKeys);
            var newLineResult = this.ProcessNewLine(deleteResult, freshKeys, pressedKeys);
            var newTextResult = this.ProcessNewText(newLineResult, freshKeys, pressedKeys);
            var softBreakResult = this.ProcessSoftTextBreaks(newTextResult); 

            return softBreakResult;
        }

        /// <summary>
        /// Processes the text cursor movement.
        /// </summary>
        /// <param name="textEditState">The text edit state.</param>
        /// <param name="freshKeys">The fresh keys.</param>
        /// <param name="pressedKeys">The pressed keys.</param>
        /// <returns>The text edit state.</returns>
        private TextEditState ProcessTextCursorMovement(TextEditState textEditState, List<Keys> freshKeys, List<ElapsedTimeExtender<Keys>> pressedKeys)
        {
            var leftActive = KeyboardHelper.KeyIsActive(Keys.Left, freshKeys, pressedKeys);
            var rightActive = KeyboardHelper.KeyIsActive(Keys.Right, freshKeys, pressedKeys);
            var upActive = KeyboardHelper.KeyIsActive(Keys.Up, freshKeys, pressedKeys);
            var downActive = KeyboardHelper.KeyIsActive(Keys.Down, freshKeys, pressedKeys);

            if ((false == leftActive) &&
                (false == rightActive) &&
                (false == upActive) &&
                (false == downActive))
            {
                var (noMovementStartAnchor, noMovementEndAnchor) = KeyboardHelper.GetAnchors(textEditState.TextCursorPosition, textEditState.TextHighlightingState.TextAnchor);
                var noMovementResult = new TextEditState
                {
                    MaxLineCharacterCount = textEditState.MaxLineCharacterCount,
                    MaxLinesCount = textEditState.MaxLinesCount,
                    TextCursorPosition = textEditState.TextCursorPosition,
                    StartAnchor = noMovementStartAnchor,
                    EndAnchor = noMovementEndAnchor,
                    TextHighlightingState = textEditState.TextHighlightingState
                };

                return noMovementResult;
            }

            var textCursorPosition = textEditState.TextCursorPosition;
            var highlightKeyActive = KeyboardHelper.AnyKeyIsActive(KeyboardHelper.ShiftKeys, freshKeys, pressedKeys, ignoreTime: true);
            var highlightTextAnchor = textEditState.TextHighlightingState.TextAnchor;

            if (true == highlightKeyActive)
                highlightTextAnchor ??= textCursorPosition;
            else
                highlightTextAnchor = null;

            if ((true == leftActive) &&
                (false == rightActive))
            {
                if ((0 == textCursorPosition.Index) &&
                    (0 != textCursorPosition.Line))
                {
                    textCursorPosition.Line--;
                    textCursorPosition.Index = this.TextLines[textCursorPosition.Line].Text.Length;
                }
                else if (0 != textCursorPosition.Index)
                    textCursorPosition.Index--;
            }
            else if ((true == rightActive) &&
                     (false == leftActive))
            {
                if ((textCursorPosition.Index == this.TextLines[textCursorPosition.Line].Text.Length) &&
                    (textCursorPosition.Line < this.TextLines.Count - 1))
                {
                    textCursorPosition.Line++;
                    textCursorPosition.Index = 0;
                }
                else if (textCursorPosition.Index < this.TextLines[textCursorPosition.Line].Text.Length)
                    textCursorPosition.Index++;
            }
            else if ((true == upActive) &&
                     (false == downActive))
            {
                if (0 != textCursorPosition.Line)
                    textCursorPosition.Line--;

                if (textCursorPosition.Index > this.TextLines[textCursorPosition.Line].Text.Length)
                    textCursorPosition.Index = this.TextLines[textCursorPosition.Line].Text.Length;
            }
            else if ((true == downActive) &&
                     (false == upActive))
            {
                if (textCursorPosition.Line < this.TextLines.Count - 1)
                    textCursorPosition.Line++;

                if (textCursorPosition.Index > this.TextLines[textCursorPosition.Line].Text.Length)
                    textCursorPosition.Index = this.TextLines[textCursorPosition.Line].Text.Length;
            }

            var (startAnchor, endAnchor) = KeyboardHelper.GetAnchors(textEditState.TextCursorPosition, textEditState.TextHighlightingState.TextAnchor);
            var result = new TextEditState
            {
                MaxLineCharacterCount = textEditState.MaxLineCharacterCount,
                MaxLinesCount = textEditState.MaxLinesCount,
                TextCursorPosition = textCursorPosition,
                StartAnchor = startAnchor,
                EndAnchor = endAnchor,
                TextHighlightingState = new TextHighlightingState
                {
                    TextAnchor = highlightTextAnchor,
                    TextHighlightColor = textEditState.TextHighlightingState.TextHighlightColor
                }
            };

            return result;
        }

        /// <summary>
        /// Processes the text delete.
        /// </summary>
        /// <param name="textEditState">The text edit state.</param>
        /// <param name="freshKeys">The fresh keys.</param>
        /// <param name="pressedKeys">The pressed keys.</param>
        /// <returns>The text edit state.</returns>
        private TextEditState ProcessTextDelete(TextEditState textEditState, List<Keys> freshKeys, List<ElapsedTimeExtender<Keys>> pressedKeys)
        {
            var backspaceActive = KeyboardHelper.KeyIsActive(Keys.Back, freshKeys, pressedKeys);
            var deleteActive = KeyboardHelper.KeyIsActive(Keys.Delete, freshKeys, pressedKeys);

            if ((false == backspaceActive) &&
                (false == deleteActive))
                return textEditState;

            var textCursorPosition = textEditState.TextCursorPosition;
            var highlightTextAnchor = textEditState.TextHighlightingState.TextAnchor;

            if (true == textEditState.TextHighlightingState.IsHighlighting)
            {
                this.RemoveHighlightedText(textEditState.StartAnchor, textEditState.EndAnchor);
                highlightTextAnchor = null;
                textCursorPosition = textEditState.StartAnchor;
            }
            else if (true == backspaceActive)
            {
                if (0 != textCursorPosition.Index)
                {
                    this.TextLines[textCursorPosition.Line].Text = this.TextLines[textCursorPosition.Line].Text.Remove(textCursorPosition.Index - 1, 1);
                    textCursorPosition.Index--;
                }
                else if ((0 == textCursorPosition.Index) &&
                         (0 != textCursorPosition.Line))
                {
                    if (false == string.IsNullOrEmpty(this.TextLines[textCursorPosition.Line].Text))
                    {
                        if (textEditState.MaxLineCharacterCount < this.TextLines[textCursorPosition.Line].Text.Length + this.TextLines[textCursorPosition.Line - 1].Text.Length)
                        {
                            this.TextLines[textCursorPosition.Line - 1].Text = this.TextLines[textCursorPosition.Line - 1].Text[..^1];
                        }

                        textCursorPosition.Index = this.TextLines[textCursorPosition.Line - 1].Text.Length;
                    }

                    var lineText = this.TextLines[textCursorPosition.Line].Text;
                    this.TextLines.RemoveAt(textCursorPosition.Line);
                    textCursorPosition.Line--;
                    textCursorPosition.Index = this.TextLines[textCursorPosition.Line].Text.Length;
                    this.AddTextToLine(textEditState, textCursorPosition, lineText, replaceEndingWhitespace: true);
                }
            }
            else if (true == deleteActive)
            {
                if (textCursorPosition.Index != this.TextLines[textCursorPosition.Line].Text.Length)
                    this.TextLines[textCursorPosition.Line].Text = this.TextLines[textCursorPosition.Line].Text.Remove(textCursorPosition.Index, 1);
                else if ((textCursorPosition.Index == this.TextLines[textCursorPosition.Line].Text.Length) &&
                         (textCursorPosition.Line != this.TextLines.Count - 1))
                {
                    this.TextLines[textCursorPosition.Line].Text += this.TextLines[textCursorPosition.Line + 1].Text;
                    this.TextLines.RemoveAt(textCursorPosition.Line + 1);
                }
            }

            var result = new TextEditState
            {
                MaxLineCharacterCount = textEditState.MaxLineCharacterCount,
                MaxLinesCount = textEditState.MaxLinesCount,
                TextCursorPosition = textCursorPosition,
                StartAnchor = textEditState.StartAnchor,
                EndAnchor = textEditState.EndAnchor,
                TextHighlightingState = new TextHighlightingState
                {
                    TextAnchor = highlightTextAnchor,
                    TextHighlightColor = textEditState.TextHighlightingState.TextHighlightColor
                }
            };

            return result;
        }

        /// <summary>
        /// Processes the new line text.
        /// </summary>
        /// <param name="textEditState">The text edit state.</param>
        /// <param name="freshKeys">The fresh keys.</param>
        /// <param name="pressedKeys">The pressed keys.</param>
        /// <returns>The text edit state.</returns>
        private TextEditState ProcessNewLine(TextEditState textEditState, List<Keys> freshKeys, List<ElapsedTimeExtender<Keys>> pressedKeys)
        {
            var newLine = KeyboardHelper.AnyKeyIsActive(KeyboardHelper.NewLineKeys, freshKeys, pressedKeys);

            if (false == newLine)
                return textEditState;

            var textCursorPosition = textEditState.TextCursorPosition;
            var highlightTextAnchor = textEditState.TextHighlightingState.TextAnchor;

            if (true == textEditState.TextHighlightingState.IsHighlighting)
            {
                this.RemoveHighlightedText(textEditState.StartAnchor, textEditState.EndAnchor);
                highlightTextAnchor = null;
                textCursorPosition = textEditState.StartAnchor;
            }

            if ((false == textEditState.MaxLinesCount.HasValue) ||
                (this.TextLines.Count < textEditState.MaxLinesCount) ||
                ((textCursorPosition.Line != this.TextLines.Count - 1) &&
                 (this.TextLines.Skip(textCursorPosition.Line + 1).Any(e  => string.IsNullOrEmpty(e.Text)))))
            {
                var existingRight = this.TextLines[textCursorPosition.Line].Text[textCursorPosition.Index..];
                this.TextLines[textCursorPosition.Line].Text = this.TextLines[textCursorPosition.Line].Text[..textCursorPosition.Index];
                this.TextLines[textCursorPosition.Line].IsManualBreak = true;
                var newTextLine = new TextLine
                {
                    IsManualBreak = true,
                    Text = existingRight
                };
                this.TextLines.Insert(textCursorPosition.Line + 1, newTextLine);
                textCursorPosition.Line++;
                textCursorPosition.Index = 0;
            }

            var result = new TextEditState
            {
                MaxLineCharacterCount = textEditState.MaxLineCharacterCount,
                MaxLinesCount = textEditState.MaxLinesCount,
                TextCursorPosition = textCursorPosition,
                StartAnchor = textEditState.StartAnchor,
                EndAnchor = textEditState.EndAnchor,
                TextHighlightingState = new TextHighlightingState
                {
                    TextAnchor = highlightTextAnchor,
                    TextHighlightColor = textEditState.TextHighlightingState.TextHighlightColor
                }
            };

            return result;
        }

        /// <summary>
        /// Processes the new text.
        /// </summary>
        /// <param name="textEditState">The text edit state.</param>
        /// <param name="freshKeys">The fresh keys.</param>
        /// <param name="pressedKeys">The pressed keys.</param>
        /// <returns>The text edit state.</returns>
        private TextEditState ProcessNewText(TextEditState textEditState, List<Keys> freshKeys, List<ElapsedTimeExtender<Keys>> pressedKeys)
        {
            var textFromKeys = KeyboardHelper.GetTextFromKeys(freshKeys, pressedKeys);

            if (true == string.IsNullOrEmpty(textFromKeys))
                return textEditState;

            var textCursorPosition = textEditState.TextCursorPosition;
            var highlightTextAnchor = textEditState.TextHighlightingState.TextAnchor;

            if ((textCursorPosition.Index == textEditState.MaxLineCharacterCount) &&
                (true == string.IsNullOrEmpty(textFromKeys)))
                return textEditState;

            if (true == textEditState.TextHighlightingState.IsHighlighting)
            {
                this.RemoveHighlightedText(textEditState.StartAnchor, textEditState.EndAnchor);
                highlightTextAnchor = null;
                textCursorPosition = textEditState.StartAnchor;
            }

            textCursorPosition = this.AddTextToLine(textEditState, textCursorPosition, textFromKeys, replaceEndingWhitespace: true);
            var result = new TextEditState
            {
                MaxLineCharacterCount = textEditState.MaxLineCharacterCount,
                MaxLinesCount = textEditState.MaxLinesCount,
                TextCursorPosition = textCursorPosition,
                StartAnchor = textEditState.StartAnchor,
                EndAnchor = textEditState.EndAnchor,
                TextHighlightingState = new TextHighlightingState
                {
                    TextAnchor = highlightTextAnchor,
                    TextHighlightColor = textEditState.TextHighlightingState.TextHighlightColor
                }
            };

            return result;
        }

        /// <summary>
        /// Processes the soft text breaks.
        /// </summary>
        /// <param name="textEditState">The text edit state.</param>
        /// <returns></returns>
        private TextEditState ProcessSoftTextBreaks(TextEditState textEditState)
        {
            if ((false == textEditState.MaxLineCharacterCount.HasValue) ||
                (1 >= this.TextLines.Count))
                return textEditState;

            var currentLine = this.TextLines.FirstOrDefault(e => (false == e.IsManualBreak) &&
                                                                 (textEditState.MaxLineCharacterCount != e.Text.Length));

            if (null == currentLine)
                return textEditState;

            var currentLineIndex = this.TextLines.IndexOf(currentLine);
            var result = textEditState;

            if ((currentLineIndex >= 0) && 
                (currentLineIndex < this.TextLines.Count - 1))
            {
                var nextLine = this.TextLines[currentLineIndex + 1];

                if (currentLine.Text.Length + nextLine.Text.Length <= textEditState.MaxLineCharacterCount)
                {
                    currentLine.Text += nextLine.Text;
                    currentLine.IsManualBreak = nextLine.IsManualBreak;
                    this.TextLines.RemoveAt(currentLineIndex + 1);
                }
                else
                { 
                    var remainingCharSpace = textEditState.MaxLineCharacterCount.Value - currentLine.Text.Length;
                    var carryText = nextLine.Text[..remainingCharSpace];
                    nextLine.Text = nextLine.Text[remainingCharSpace..];
                    currentLine.Text += carryText;
                    currentLine.IsManualBreak = false;
                }

                result = this.ProcessSoftTextBreaks(textEditState);
            }

            return result;
        }

        /// <summary>
        /// Adds the text to the line.
        /// </summary>
        /// <param name="textEditState">The text edit state.</param>
        /// <param name="textCursorPosition">The text cursor position.</param>
        /// <param name="newText">The new text.</param>
        /// <param name="replaceEndingWhitespace">A value indicating whether to replace the ending whitespace.</param>
        private TextPosition AddTextToLine(TextEditState textEditState, TextPosition textCursorPosition, string newText, bool replaceEndingWhitespace)
        {
            var existingLeft = this.TextLines[textCursorPosition.Line].Text[..textCursorPosition.Index];
            var existingRight = this.TextLines[textCursorPosition.Line].Text[textCursorPosition.Index..];

            if (false == textEditState.MaxLineCharacterCount.HasValue)
            {
                this.TextLines[textCursorPosition.Line].Text = existingLeft + newText + existingRight;
                var freeSpaceResult = new TextPosition
                {
                    Index = existingLeft.Length + newText.Length,
                    Line = textCursorPosition.Line
                };

                return freeSpaceResult;
            }

            if ((true == textEditState.MaxLinesCount.HasValue) &&
                (false == this.CharacterSpaceIsLeftForNewText(textCursorPosition, textEditState.MaxLineCharacterCount.Value, textEditState.MaxLinesCount.Value)))
                return textCursorPosition;

            var remainingCharSpace = textEditState.MaxLineCharacterCount.Value - this.TextLines[textCursorPosition.Line].Text.Length;

            if (remainingCharSpace < 0)
                remainingCharSpace = 0;

            if (newText.Length <= remainingCharSpace)
            {
                this.TextLines[textCursorPosition.Line].Text = existingLeft + newText + existingRight;
                var freeSpaceResult = new TextPosition
                {
                    Index = textCursorPosition.Index + newText.Length,
                    Line = textCursorPosition.Line
                };

                return freeSpaceResult;
            }

            if ((true == replaceEndingWhitespace) &&
                (false == string.IsNullOrWhiteSpace(newText)))
            {
                var lineEndWhiteSpaceCount = this.TextLines[textCursorPosition.Line].Text.Length - this.TextLines[textCursorPosition.Line].Text.TrimEnd().Length;

                if (newText.Length <= lineEndWhiteSpaceCount)
                {
                    this.TextLines[textCursorPosition.Line].Text = existingLeft + newText + existingRight;
                    this.TextLines[textCursorPosition.Line].Text = this.TextLines[textCursorPosition.Line].Text[..textEditState.MaxLineCharacterCount.Value];

                    var whiteSpaceResult = new TextPosition
                    {
                        Index = textCursorPosition.Index + newText.Length,
                        Line = textCursorPosition.Line
                    };

                    return whiteSpaceResult;
                }
            }

            this.TextLines[textCursorPosition.Line].Text = existingLeft + newText + existingRight;
            var carryText = this.TextLines[textCursorPosition.Line].Text[textEditState.MaxLineCharacterCount.Value..];
            this.AddTextToLineStart(textEditState, textCursorPosition.Line + 1, carryText);
            this.TextLines[textCursorPosition.Line].Text = this.TextLines[textCursorPosition.Line].Text[..textEditState.MaxLineCharacterCount.Value];

            if (textCursorPosition.Index == textEditState.MaxLineCharacterCount)
            {
                var lineEndResult = new TextPosition
                {
                    Index = carryText.Length,
                    Line = textCursorPosition.Line + 1
                };

                return lineEndResult;
            }

            var sameLineResult = new TextPosition
            {
                Index = textCursorPosition.Index + newText.Length,
                Line = textCursorPosition.Line
            };

            return sameLineResult;
        }

        /// <summary>
        /// Adds text to the start of the line.
        /// </summary>
        /// <param name="textEditState">The text edit state.</param>
        /// <param name="line">The line.</param>
        /// <param name="newText">The new text.</param>
        /// <returns>The text position.</returns>
        private void AddTextToLineStart(TextEditState textEditState, int line, string newText)
        {
            if (this.TextLines.Count - 1 < line)
            {
                var emptyTextLine = new TextLine
                {
                    IsManualBreak = false,
                    Text = string.Empty
                };
                this.TextLines.Add(emptyTextLine);
            }

            if (newText.Length + this.TextLines[line].Text.Length <= textEditState.MaxLineCharacterCount)
            {
                this.TextLines[line].Text = newText + this.TextLines[line].Text;

                return;
            }

            this.TextLines[line].Text = newText + this.TextLines[line].Text;
            var carryText = this.TextLines[line].Text[textEditState.MaxLineCharacterCount.Value..];
            this.AddTextToLineStart(textEditState, line + 1, carryText);
            this.TextLines[line].Text = this.TextLines[line].Text[..textEditState.MaxLineCharacterCount.Value];
        }

        /// <summary>
        /// Gets the character space left for the text position.
        /// </summary>
        /// <param name="textPosition">The text position.</param>
        /// <param name="maxLineCharacterCount">The max line character count.</param>
        /// <param name="maxLinesCount">The max line count.</param>
        /// <returns>The character space left.</returns>
        private bool CharacterSpaceIsLeftForNewText(TextPosition textPosition, int maxLineCharacterCount, int maxLinesCount)
        {
            if (this.TextLines.Count < maxLinesCount)
                return true;

            for (var i = textPosition.Line; i < this.TextLines.Count; i++)
                if (this.TextLines[i].Text.Length < maxLineCharacterCount)
                    return true;

            return false;
        }

        /// <summary>
        /// Removes the highlighted text.
        /// </summary>
        /// <param name="startAnchor">The start anchor.</param>
        /// <param name="endAnchor">The end anchor.</param>
        /// <returns>The new text line.</returns>
        private void RemoveHighlightedText(TextPosition startAnchor, TextPosition endAnchor)
        {
            if (startAnchor.Line == endAnchor.Line)
            {
                var lineText = this.TextLines[startAnchor.Line].Text;
                this.TextLines[startAnchor.Line].Text = lineText[..startAnchor.Index] + lineText[endAnchor.Index..];

                return;
            }

            var prefix = this.TextLines[startAnchor.Line].Text[..startAnchor.Index];
            var suffix = this.TextLines[endAnchor.Line].Text[endAnchor.Index..];
            this.TextLines[startAnchor.Line].Text = prefix + suffix;
            this.TextLines.RemoveRange(startAnchor.Line + 1, endAnchor.Line - startAnchor.Line);
        }
    }
}
