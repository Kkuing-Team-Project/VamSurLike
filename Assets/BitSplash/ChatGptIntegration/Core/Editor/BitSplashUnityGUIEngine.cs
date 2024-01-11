using Highlight.Engines;
using Highlight.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using UnityEngine;

namespace BitSplash.AI.GPT
{
    class BitSplashUnityGUIEngine : Engine
    {
        private const string ColorStyleFormat = "<color={0}>{1}</color>";

        static string ToHtmlStringRGB(Color color)
        {
            return $"#{ColorUtility.ToHtmlStringRGB(color)}";
        }
        static StringBuilder builder = new StringBuilder();
        static string EscapeString(string input)
        {
            builder.Clear();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '<')
                    builder.Append("<<i></i>");
                else
                    if (input[i] == '>')
                    builder.Append("<i></i>>");
                else
                    builder.Append(input[i]);
            }
            return builder.ToString();
        }
        protected override string PreHighlight(Definition definition, string input)
        {
            if (definition == null)
            {
                throw new ArgumentNullException("definition");
            }

            return EscapeString(input);
        }

        protected override string PostHighlight(Definition definition, string input)
        {
            if (definition == null)
            {
                throw new ArgumentNullException("definition");
            }

            string color =ToHtmlStringRGB(definition.Style.Colors.ForeColor);
            return String.Format(ColorStyleFormat, color, input);
        }

        protected override string ProcessBlockPatternMatch(Definition definition, BlockPattern pattern, Match match)
        {
            string color = ToHtmlStringRGB(pattern.Style.Colors.ForeColor);
            return String.Format(ColorStyleFormat, color, match.Value);
        }

        protected override string ProcessMarkupPatternMatch(Definition definition, MarkupPattern pattern, Match match)
        {
            if (definition == null)
            {
                throw new ArgumentNullException("definition");
            }
            if (pattern == null)
            {
                throw new ArgumentNullException("pattern");
            }
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            var result = new StringBuilder();

            var patternStyle = ToHtmlStringRGB(pattern.BracketColors.ForeColor);
            result.AppendFormat(ColorStyleFormat, patternStyle, match.Groups["openTag"].Value);

            result.Append(match.Groups["ws1"].Value);

            patternStyle = ToHtmlStringRGB(pattern.Style.Colors.ForeColor);
            result.AppendFormat(ColorStyleFormat, patternStyle, match.Groups["tagName"].Value);

            if (pattern.HighlightAttributes)
            {
                var highlightedAttributes = ProcessMarkupPatternAttributeMatches(definition, pattern, match);
                result.Append(highlightedAttributes);
            }

            result.Append(match.Groups["ws5"].Value);

            patternStyle = ToHtmlStringRGB(pattern.BracketColors.ForeColor);
            result.AppendFormat(ColorStyleFormat, patternStyle, match.Groups["closeTag"].Value);

            return result.ToString();
        }

        protected override string ProcessWordPatternMatch(Definition definition, WordPattern pattern, Match match)
        {
            var patternStyle = ToHtmlStringRGB(pattern.Style.Colors.ForeColor);
            return String.Format(ColorStyleFormat, patternStyle, match.Value);
        }

        private string ProcessMarkupPatternAttributeMatches(Definition definition, MarkupPattern pattern, Match match)
        {
            var result = new StringBuilder();

            for (var i = 0; i < match.Groups["attribute"].Captures.Count; i++)
            {
                result.Append(match.Groups["ws2"].Captures[i].Value);
                var patternStyle =  ToHtmlStringRGB(pattern.AttributeNameColors.ForeColor);
                result.AppendFormat(ColorStyleFormat, patternStyle, match.Groups["attribName"].Captures[i].Value);

                if (String.IsNullOrWhiteSpace(match.Groups["attribValue"].Captures[i].Value))
                {
                    continue;
                }

                patternStyle = ToHtmlStringRGB(pattern.AttributeValueColors.ForeColor);
                result.AppendFormat(ColorStyleFormat, patternStyle, match.Groups["attribValue"].Captures[i].Value);
        }

            return result.ToString();
        }
    }
}
