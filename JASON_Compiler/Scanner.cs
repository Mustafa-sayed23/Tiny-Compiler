using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public enum Token_Class
{
    Begin, Call, Declare, End, Do, Else, EndIf, EndUntil, EndWhile, If, Integer,Parameters, Procedure, Program,
    Read, Real, Set, Then, Until, While, Write, Dot, Semicolon, Comma, LParanthesis, RParanthesis, EqualOp, 
    LessThanOp,GreaterThanOp, NotEqualOp, PlusOp, MinusOp, MultiplyOp, DivideOp, Idenifier, Number, Assignment_Op,end,
    And_Op, Or_Op, Float, String, Return, LeftCurlyBracket, RightCurlyBracket, endl, Comment, Repeat, ElseIF, Constant_String, Main_Function
}
namespace JASON_Compiler
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
            ReservedWords.Add("int", Token_Class.Integer);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("elseif", Token_Class.ElseIF);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("endl", Token_Class.endl);
            ReservedWords.Add("end", Token_Class.End);
            ReservedWords.Add("program", Token_Class.Program);
            ReservedWords.Add("main", Token_Class.Main_Function);

            Operators.Add(".", Token_Class.Dot);
            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add(":=", Token_Class.Assignment_Op);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("<>", Token_Class.NotEqualOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add("&&", Token_Class.And_Op);
            Operators.Add("||", Token_Class.Or_Op);
            Operators.Add("{", Token_Class.LeftCurlyBracket);
            Operators.Add("}", Token_Class.RightCurlyBracket);
        }

    public void StartScanning(string SourceCode)
    {
        // i: Outer loop to check on lexemes.
        for (int i = 0; i < SourceCode.Length; i++)
        {
            // j: Inner loop to check on each character in a single lexeme.
            int j = i;
            char CurrentChar = SourceCode[i];
            string CurrentLexeme = CurrentChar.ToString();

            if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n' || CurrentChar == '\t')
                continue;

            if (char.IsLetter(CurrentChar))
            {
                // The possible Token Classes that begin with a character are
                // an Idenifier or a Reserved Word.
                // (1) Update the CurrentChar and validate its value.
                string check_string = "";

                // (2) Iterate to build the rest of the lexeme while satisfying the
                // conditions on how the Token Classes should be.
                // (2.1) Append the CurrentChar to CurrentLexeme.
                // (2.2) Update the CurrentChar.

                while (char.IsLetter(CurrentChar) || char.IsDigit(CurrentChar))
                {
                    check_string += CurrentChar.ToString();
                    j++;

                    if (j >= SourceCode.Length) { break; }
                    CurrentChar = SourceCode[j];
                }
                FindTokenClass(check_string);
                i = j - 1;
                 // (3) Call FindTokenClass on the CurrentLexeme.
                 // (4) Update the outer loop pointer (i) to point on the next lexeme.
            }
            else if (char.IsDigit(CurrentChar))
            {
                //4354mmo
                bool wrong = false;
                string check_string = "";
                while (CurrentChar != ' ' && (CurrentChar == '.' || char.IsDigit(CurrentChar)))
                {
                    check_string += CurrentChar.ToString();
                    j++;

                    if (j >= SourceCode.Length) { break; }
                    CurrentChar = SourceCode[j];
                    if (char.IsLetter(CurrentChar))
                    {
                        wrong = true;
                        break;
                    }
                }
            if (wrong)
            {
                int counter = 0;
                while (CurrentChar != ' ' && j < SourceCode.Length)
                {
                    if (counter != 1)
                    {
                        check_string += CurrentChar.ToString();
                    }
                    counter++;
                    CurrentChar = SourceCode[j];
                    j++;
                }
                if (counter > 1)
                {
                    check_string += CurrentChar.ToString(); 
                }
            }
            FindTokenClass(check_string);
            i = j - 1;
        }

        else if (CurrentChar == '"')
        {
            string check_string = "";
            while (true)
            {
                check_string += CurrentChar.ToString();
                j++;
                if (j >= SourceCode.Length) { break; }
                CurrentChar = SourceCode[j];
                if (CurrentChar == '"')
                {
                     check_string += CurrentChar.ToString();
                     j++;
                     break;
                }
            }
            FindTokenClass(check_string);
            i = j - 1;
            // (3) Call FindTokenClass on the CurrentLexeme.
        }
        else if (CurrentChar == '/' && SourceCode[j + 1] == '*')
        {
                    string check_string = "";
                    while (true)
                    {
                        check_string += CurrentChar.ToString();
                        j++;

                        if (j >= SourceCode.Length) { break; }
                        CurrentChar = SourceCode[j];
                        if (CurrentChar == '*' && SourceCode[j+1]=='/')
                        {
                            check_string += CurrentChar.ToString();
                            j++;

                            CurrentChar = SourceCode[j];
                            check_string += CurrentChar.ToString();
                            j++;


                            break;
                        }
                    }
                    FindTokenClass(check_string);
                    i = j - 1;
                }
                else
                {
                    // (1) Update the CurrentChar and validate its value.
                    string check_string = "";
                    // (2) Iterate to build the rest of the lexeme while satisfying the
                    // conditions on how the Token Classes should be.
                    // (2.1) Append the CurrentChar to CurrentLexeme.
                    // (2.2) Update the CurrentChar.
                    while (CurrentChar != ' ' && !char.IsDigit(CurrentChar) && !char.IsLetter(CurrentChar) && CurrentChar != '\n' && CurrentChar != '\r' && CurrentChar != '\t')
                    {
                        check_string += CurrentChar.ToString();
                        j++;
                        if (CurrentChar == ')' || CurrentChar == '}' || CurrentChar == '{' || CurrentChar == '(' || CurrentChar == '+' || CurrentChar == '-' || CurrentChar == '*' || CurrentChar == '/')
                        {
                            break;
                        }
                        if (j >= SourceCode.Length) { break; }
                        CurrentChar = SourceCode[j];
                    }
                    FindTokenClass(check_string);
                    i = j - 1;
                }
            }
            JASON_Compiler.TokenStream = Tokens;
        }
        void FindTokenClass(string Lex)
        {
            Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;
            //Is it a reserved word?
            if (ReservedWords.ContainsKey(Lex))
            {
                Tok.token_type = ReservedWords[Lex];
                Tokens.Add(Tok);
            }
            //Is it an operator?
            else if (Operators.ContainsKey(Lex))
            {
                Tok.token_type = Operators[Lex];
                Tokens.Add(Tok);

            }
            //Is it an identifier?
            else if (isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Idenifier;
                Tokens.Add(Tok);
            }
            //Is it a Constant?
            else if (isNumber(Lex))
            {
                Tok.token_type = Token_Class.Number;
                Tokens.Add(Tok);
            }

            else if (constant_string(Lex))
            {
                Tok.token_type = Token_Class.Constant_String;
                Tokens.Add(Tok);
            }

            else if (comment(Lex))
            {
                Tok.token_type = Token_Class.Comment;
                Tokens.Add(Tok);
            }

            //Is it an undefined?
            else
            {
                Errors.Error_List.Add(Lex);
            }
        }

        bool isIdentifier(string lex)
        {
            bool isValid = true;
            // Check if the lex is an identifier or not.
            var re = new Regex(@"^[a-zA-Z]+(a-zA-Z|[0-9])*");
            if (re.IsMatch(lex))
            {
                return isValid;
            }
            return false;
        }
        bool isNumber(string lex)
        {
            bool isValid = true;
            // Check if the lex is a constant (Number) or not.
            var re = new Regex(@"^-?[0-9]*(\.[0-9]+)?$");
            if (re.IsMatch(lex))
            {
                return isValid;
            }
            return false;
        }
        bool constant_string(string lex)
        {
            bool isValid = true;
            // Check if the lex is a constant (Number) or not.
            var re = new Regex("^\".*\"$");
            if (re.IsMatch(lex))
            {
                return isValid;
            }
            return false;

        }
        

        bool comment(string lex)
        {
            bool isValid = true;
            // Check if the lex is a constant (Number) or not.
            var re = new Regex(@"/\*.*?\*/");
            if (re.IsMatch(lex))
            {
                return isValid;
            }
            return false;
        }
    }
}