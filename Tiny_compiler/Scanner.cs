using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
public enum Token_Class
{
    Int , Float , String , Main, Read , Write , Repeat , Until , If , Elseif , Else , Then , Return , End,
    Semicolon, Comma, LParanthesis, RParanthesis, RightBraces, LeftBraces, EqualOp, LessThanOp,
    GreaterThanOp, NotEqualOp, PlusOp, MinusOp, MultiplyOp, DivisionOp, OrOp ,AndOp,Assign,
    Idenifier,Comment,Number,Endl
}
namespace Tiny_compiler
{
    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();

        public Scanner()
        {
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("elseif", Token_Class.Elseif);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("end", Token_Class.End);
            ReservedWords.Add("endl", Token_Class.Endl);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("int", Token_Class.Int);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("main", Token_Class.Main);



            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("{", Token_Class.LeftBraces);
            Operators.Add("}", Token_Class.RightBraces);
            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add(":=", Token_Class.Assign);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("<>", Token_Class.NotEqualOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivisionOp);
            Operators.Add("&&", Token_Class.AndOp);
            Operators.Add("||", Token_Class.OrOp);
        }

        public void StartScanning(string SourceCode)
        {
            // i: Outer loop to check on lexemes.
            for (int i = 0; i < SourceCode.Length; i++)
            {
                // j: Inner loop to check on each character in a single lexeme.
                int j = i;
                char CurrentChar = SourceCode[i];
                char NextCurrentChar = ' ';
                string CurrentLexeme = CurrentChar.ToString();

                if (i + 1 < SourceCode.Length)
                    NextCurrentChar = SourceCode[i + 1];

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n' || CurrentChar == '\t')
                    continue;

                if (char.IsLetter(CurrentChar))
                {
                    // The possible Token Classes that begin with a character are
                    // an Idenifier or a Reserved Word.

                    // (1) Update the CurrentChar and validate its value.
                    j++;
                    if (j < SourceCode.Length)
                        CurrentChar = SourceCode[j];
                    // (2) Iterate to build the rest of the lexeme while satisfying the
                    while (j < SourceCode.Length && (char.IsLetterOrDigit(CurrentChar)))
                    {
                        // conditions on how the Token Classes should be.
                        // (2.1) Append the CurrentChar to CurrentLexeme.
                        CurrentLexeme += CurrentChar;
                        j++;
                        // (2.2) Update the CurrentChar.
                        if (j < SourceCode.Length)
                            CurrentChar = SourceCode[j];
                    }

                }
                else if (CurrentChar == '/')
                {
                    j++;

                    if (j < SourceCode.Length && SourceCode[j] == '*')
                    {
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar;
                        j++;
                        while (j < SourceCode.Length)
                        {
                            CurrentChar = SourceCode[j];
                            CurrentLexeme += CurrentChar;
                            if (CurrentChar == '*')
                            {
                                if ((j + 1) < SourceCode.Length && SourceCode[j + 1] == '/')
                                {
                                    CurrentChar = SourceCode[j + 1];
                                    CurrentLexeme += CurrentChar;
                                    j += 2;
                                    break;
                                }
                            }
                            j++;
                        }

                    }

                }
                else if (char.IsDigit(CurrentChar))
                {
                    j++;
                    if (j < SourceCode.Length)
                        CurrentChar = SourceCode[j];
                    while (j < SourceCode.Length && (char.IsDigit(CurrentChar) || CurrentChar == '.' || char.IsLetter(CurrentChar)))
                    {
                        CurrentLexeme += CurrentChar;
                        j++;
                        if (j < SourceCode.Length)
                            CurrentChar = SourceCode[j];
                    }


                }
                else if (CurrentChar == '"')
                {
                    j++;
                    if (j < SourceCode.Length)
                    {
                        CurrentChar = SourceCode[j];
                    }
                    while (j < SourceCode.Length && !char.Equals(SourceCode[j], '"'))
                    {
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar;
                        j++;
                      
                    }
                    if (j < SourceCode.Length)
                    {
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar;
                        j++;
                    }
                }
                else
                {

                    string opOfTwoChars;
                    string opOfOneChar;
                    opOfOneChar = CurrentChar.ToString();

                    opOfTwoChars = CurrentChar.ToString();
                    opOfTwoChars += NextCurrentChar;


                    if (Operators.ContainsKey(opOfTwoChars))
                    {
                        CurrentLexeme += NextCurrentChar;
                        j += 2;
                    }
                    else if (Operators.ContainsKey(opOfOneChar))
                    {
                        j++;
                    }

                    else
                    {
                        j++;
                        while (j < SourceCode.Length)
                        {
                            CurrentChar = SourceCode[j];

                            opOfTwoChars = CurrentChar.ToString();
                            opOfTwoChars += NextCurrentChar;
                            if (Operators.ContainsKey(opOfTwoChars))
                                break;

                            opOfOneChar = CurrentChar.ToString();
                            if (Operators.ContainsKey(opOfOneChar))
                                break;

                            if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                                break;

                            CurrentLexeme += CurrentChar;
                            j++;


                            if (j + 1 < SourceCode.Length)
                                NextCurrentChar = SourceCode[j + 1];
                        }
                    }


                }
                // (3) Call FindTokenClass on the CurrentLexeme.
                FindTokenClass(CurrentLexeme);
                // (4) Update the outer loop pointer (i) to point on the next lexeme.
                i = j - 1;
            }

            TINY_Compiler.TokenStream = Tokens;
        }

        void FindTokenClass(string Lex)
        {
            Token Tok = new Token();
            Tok.lex = Lex;
            //Is it a reserved word?
            if (ReservedWords.ContainsKey(Lex))
            {
                Tok.token_type = ReservedWords[Lex];
                Tokens.Add(Tok);
            }
            //Is it an identifier?
            else if (isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Idenifier;
                Tokens.Add(Tok);
            }
            //is Number?
            else if (isNumber(Lex))
            {
                Tok.token_type = Token_Class.Number;
                Tokens.Add(Tok);
            }
            //Is it an operator?
            else if(Operators.ContainsKey(Lex))
            {
                Tok.token_type = Operators[Lex];
                Tokens.Add(Tok);
            }
            // Is it comment?
            else if (isComment(Lex))
            {
                Tok.token_type = Token_Class.Comment;
                Tokens.Add(Tok);
            }
            //is String?
            else if (isString(Lex))
            {
                Tok.token_type = Token_Class.String;
                Tokens.Add(Tok);
            }
            //Is it an undefined?
            else
            {
                Errors.Error_List.Add("Scanning Error: " + Lex + "\r\n");
            }

        }

        bool isIdentifier(string lex)
        {
            bool isValid = false;
            // Check if the lex is an identifier or not.
            var rx = new Regex(@"^[a-zA-Z]([a-zA-Z0-9])*$", RegexOptions.Compiled);
            if (rx.IsMatch(lex))
                isValid = true;
            return isValid;
        }
        bool isString(string lex)
        {
            bool isValid = false;
            // Check if the lex is an identifier or not.
            var rx = new Regex("^\"(.|[0-9])*\"$", RegexOptions.Compiled);
            if (rx.IsMatch(lex))
                isValid = true;
            return isValid;
        }
        bool isComment(string lex)
        {
            bool isValid = false;
            
            var rx = new Regex(@"/\*(.|\s|[0-9])*\*/", RegexOptions.Compiled);
            if (rx.IsMatch(lex))
                isValid = true;
            return isValid;
        }
        bool isNumber(string lex)
        {
            bool isValid = true;
            // Check if the lex is a (Number) or not.
            Regex reg = new Regex(@"^\d+(\.\d+)?$");
            isValid = reg.IsMatch(lex);

            return isValid;
        }
    }
}
