using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Threading;
using BasicPL.Models;

namespace BasicPL.Core
{
    public class Lexer
    {
        private string _text;
        private Position _pos;
        private char? _currChar;

        public Lexer(string fileName, string text)
        {
            _text = text;
            _pos = new Position(-1, 0, -1, fileName, text);
            Advance();
        }

        private void Advance()
        {
            _pos.Advance(_currChar);
            if (_pos.Index < _text.Length)
                _currChar = _text[_pos.Index];
            else _currChar = null;
        }

        public List<Token> MakeTokens()
        {
            List<Token> tokens = new List<Token>();

            while (_currChar != null)
            {
                switch (_currChar)
                {
                    case ' ':
                        Advance();
                        break;
                    case '\t':
                        Advance();
                        break;
                    case '+':
                        tokens.Add(new Token(TokenType.PLUS, _pos));
                        Advance();
                        break;
                    case '-':
                        tokens.Add(new Token(TokenType.MINUS, _pos));
                        Advance();
                        break;
                    case '*':
                        tokens.Add(new Token(TokenType.MUL, _pos));
                        Advance();
                        break;
                    case '/':
                        tokens.Add(new Token(TokenType.DIV, _pos));
                        Advance();
                        break;
                    case '^':
                        tokens.Add(new Token(TokenType.POW, _pos));
                        Advance();
                        break;
                    case '(':
                        tokens.Add(new Token(TokenType.LPAREN, _pos));
                        Advance();
                        break;
                    case ')':
                        tokens.Add(new Token(TokenType.RPAREN, _pos));
                        Advance();
                        break;
                    case '=':
                        tokens.Add(MakeEquals());
                        //new Token(TokenType.ASSIGN, _pos)
                        //Advance();
                        break;
                    case '<':
                        tokens.Add(MakeLessThan());
                        break;
                    case '>':
                        tokens.Add(MakeGreaterThan());
                        break;
                    case '!':
                        tokens.Add(MakeNotEquals());
                        break;
                    case var digit when int.TryParse(digit.ToString(), out int _):
                        tokens.Add(MakeNumber());
                        break;

                    case var letter when letter is not null && char.IsLetterOrDigit((char)letter):
                        tokens.Add(MakeIdentifier());
                        break;

                    default:
                        Position start = _pos;
                        char ch = (char)_currChar;
                        Advance();
                        Position end = _pos;
                        throw new IllegalCharException(ch, start, end);
                }
            }

            tokens.Add(new Token(TokenType.EOF, _pos));
            return tokens;
        }

        private Token MakeEquals()
        {
            TokenType type = TokenType.ASSIGN;
            var start = _pos;
            Advance();

            if (_currChar == '=')
            {
                type = TokenType.EQ;
                Advance();
            }

            return new Token(type, start, end: _pos);
        }

        private Token MakeNotEquals()
        {
            var start = _pos;
            Advance();

            if (_currChar == '=')
            {
                Advance();
                return new Token(TokenType.NE, start, end: _pos);
            }

            throw new ExpectedCharException("'=' (after '!')", start, _pos);
        }

        private Token MakeGreaterThan()
        {
            TokenType type = TokenType.GT;
            var start = _pos;
            Advance();

            if (_currChar == '=')
            {
                type = TokenType.GTE;
                Advance();
            }

            return new Token(type, start, end: _pos);
        }

        private Token MakeLessThan()
        {
            TokenType type = TokenType.LT;
            var start = _pos;
            Advance();

            if (_currChar == '=')
            {
                type = TokenType.LTE;
                Advance();
            }

            return new Token(type, start, end: _pos);
        }


        private Token MakeIdentifier()
        {
            string identifier = string.Empty;
            Position pos = _pos;

            while (_currChar != null && char.IsLetterOrDigit((char)_currChar))
            {
                identifier += _currChar;
                Advance();
            }

            if (Keywords.IsKeyword(identifier))
                return new Token(TokenType.KEYWORD, pos, identifier, _pos);
            else return new Token(TokenType.IDENTIFIER, pos, identifier, _pos);
        }

        private Token MakeNumber()
        {
            string numberString = string.Empty;
            int dotCount = 0;

            var posStart = _pos;

            while (_currChar != null && _currChar == '.' || int.TryParse(_currChar.ToString(), out _))
            {
                if (_currChar == '.')
                {
                    if (dotCount >= 1) break;
                    dotCount++;
                    numberString += '.';
                }
                else
                {
                    numberString += _currChar;
                }
                Advance();
            }

            if (dotCount == 0)
            {
                return new Token(TokenType.INT, posStart, int.Parse(numberString), _pos);
            }
            else
            {
                return new Token(TokenType.FLOAT, posStart, float.Parse(numberString, CultureInfo.InvariantCulture), _pos);
            }
        }
    }
}
