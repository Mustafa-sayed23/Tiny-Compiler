using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JASON_Compiler
{
    public class Node
    {
        public List<Node> Children = new List<Node>();
        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }

    public class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public Node root;

        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = new Node("Tiny Code");
            root.Children.Add(Program());
            return root;
        }

        Node Program()
        {
            Node program = new Node("Program");
            program.Children.Add(Equation());
            MessageBox.Show("Success");
            return program;
        }

        Node Main_funtion()
        {
            Node main_fun = new Node("Main_funtion");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Integer == TokenStream[InputPointer].token_type || Token_Class.Float == TokenStream[InputPointer].token_type ||
                    Token_Class.String == TokenStream[InputPointer].token_type && InputPointer + 1 < TokenStream.Count)
                {
                    if (Token_Class.Main_Function == TokenStream[InputPointer + 1].token_type)
                    {

                        main_fun.Children.Add(DataType());
                        main_fun.Children.Add(match(Token_Class.Main_Function));
                        main_fun.Children.Add(match(Token_Class.LParanthesis));
                        main_fun.Children.Add(match(Token_Class.RParanthesis));
                        main_fun.Children.Add(Function_Body());
                        return main_fun;
                    }
                }
            }
            return null;
        }

        Node Function_Body()
        {
            Node func_body = new Node("Function_Body");
            if (InputPointer < TokenStream.Count)
            {
                func_body.Children.Add(match(Token_Class.LeftCurlyBracket));
                func_body.Children.Add(statements());
                func_body.Children.Add(Return_Statement());
                func_body.Children.Add(match(Token_Class.RightCurlyBracket));
                return func_body;
            }
            return null;
        }

        Node statement()
        {
            Node stat = new Node("Statement");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Idenifier == TokenStream[InputPointer].token_type || Token_Class.Integer == TokenStream[InputPointer].token_type
                    || Token_Class.String == TokenStream[InputPointer].token_type || Token_Class.Float == TokenStream[InputPointer].token_type ||
                    Token_Class.Write == TokenStream[InputPointer].token_type || Token_Class.Read == TokenStream[InputPointer].token_type
                    || Token_Class.Return == TokenStream[InputPointer].token_type || Token_Class.Repeat == TokenStream[InputPointer].token_type ||
                    Token_Class.If == TokenStream[InputPointer].token_type)
                {
                    stat.Children.Add(statements());
                    stat.Children.Add(state());
                    return stat;
                }
            }
            return null;
        }

        Node statements()
        {

            if (InputPointer < TokenStream.Count)
            {

                if (Token_Class.Idenifier == TokenStream[InputPointer].token_type)
                {
                    Node s = new Node("Statements");
                    s.Children.Add(Ass_Statement());
                    return s;
                }
                if (Token_Class.Integer == TokenStream[InputPointer].token_type || Token_Class.Float == TokenStream[InputPointer].token_type || Token_Class.String == TokenStream[InputPointer].token_type)
                {
                    Node s = new Node("Statements");
                    s.Children.Add(Decl_Statement());
                    return s;
                }
                if (Token_Class.Write == TokenStream[InputPointer].token_type)
                {
                    Node s = new Node("Statements");
                    s.Children.Add(Write_State());
                    return s;

                }
                if (Token_Class.Read == TokenStream[InputPointer].token_type)
                {
                    Node s = new Node("Statements");
                    s.Children.Add(Read_Statment());
                    return s;
                }

                if (Token_Class.Return == TokenStream[InputPointer].token_type)
                {
                    Node s = new Node("Statements");
                    s.Children.Add(Return_Statement());
                    return s;
                }

                if (Token_Class.Repeat == TokenStream[InputPointer].token_type)
                {
                    Node s = new Node("Statements");
                    s.Children.Add(Repeat_Statement());
                    return s;
                }

                if (Token_Class.If == TokenStream[InputPointer].token_type)
                {
                    Node s = new Node("Statements");
                    s.Children.Add(if_Statement());
                    return s;
                }
            }
            return null;
        }

        Node state()
        {
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Idenifier == TokenStream[InputPointer].token_type || Token_Class.Integer == TokenStream[InputPointer].token_type
                    || Token_Class.String == TokenStream[InputPointer].token_type || Token_Class.Float == TokenStream[InputPointer].token_type ||
                    Token_Class.Write == TokenStream[InputPointer].token_type || Token_Class.Read == TokenStream[InputPointer].token_type
                    || Token_Class.Return == TokenStream[InputPointer].token_type || Token_Class.Repeat == TokenStream[InputPointer].token_type
                    || Token_Class.If == TokenStream[InputPointer].token_type)
                {

                    Node stat = new Node("State");
                    stat.Children.Add(statements());
                    stat.Children.Add(state());
                    return stat;
                }
            }
            return null;
        }

        Node Function_list()
        {
            Node Funct = new Node("Function_list");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Integer == TokenStream[InputPointer].token_type || Token_Class.Float == TokenStream[InputPointer].token_type
                    || Token_Class.String == TokenStream[InputPointer].token_type && InputPointer + 1 < TokenStream.Count)
                {
                    if (Token_Class.Main_Function != TokenStream[InputPointer + 1].token_type)
                    {
                        Funct.Children.Add(Function_Statement());
                        Funct.Children.Add(Function_list());
                        return Funct;
                    }
                }
            }
            return null;
        }

        Node Function_Statement()
        {
            Node funct = new Node("Function_Statement");
            funct.Children.Add(funct_decl());
            funct.Children.Add(Function_Body());
            return funct;
        }

        Node funct_decl()
        {
            Node funct = new Node("Function Declaration");
            funct.Children.Add(DataType());
            funct.Children.Add(match(Token_Class.Idenifier));
            funct.Children.Add(match(Token_Class.LParanthesis));
            funct.Children.Add(Parameter());
            funct.Children.Add(match(Token_Class.RParanthesis));
            return funct;

        }

        Node Parameter()
        {
            Node para = new Node("Parameter");
            if (Token_Class.Integer == TokenStream[InputPointer].token_type || Token_Class.String == TokenStream[InputPointer].token_type ||
                Token_Class.Float == TokenStream[InputPointer].token_type)
            {
                para.Children.Add(DataType());
                para.Children.Add(match(Token_Class.Idenifier));
                para.Children.Add(parameter_List());

                return para;
            }
            return null;
        }

        Node parameter_List()
        {
            Node para = new Node("parameter_List");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Comma == TokenStream[InputPointer].token_type)
                {
                    para.Children.Add(match(Token_Class.Comma));
                    para.Children.Add(DataType());
                    para.Children.Add(match(Token_Class.Idenifier));
                    para.Children.Add(parameter_List());
                }
                else
                    return null;
            }
            return para;
        }

        Node Repeat_Statement()
        {

            if (Token_Class.Repeat == TokenStream[InputPointer].token_type)
            {
                Node re = new Node("Repeat_Statement");
                re.Children.Add(match(Token_Class.Repeat));
                re.Children.Add(statements());
                re.Children.Add(match(Token_Class.Until));
                re.Children.Add(condition_statement());
                return re;
            }
            return null;
        }

        Node condition_statement()
        {
            Node con = new Node("condition_statement");
            con.Children.Add(condition());
            con.Children.Add(condition_list());
            return con;
        }

        Node condition_list()
        {
            if (InputPointer < TokenStream.Count)
            {

                if (Token_Class.And_Op == TokenStream[InputPointer].token_type || Token_Class.Or_Op == TokenStream[InputPointer].token_type)
                {
                    Node con_list = new Node("condition_list");
                    con_list.Children.Add(Boolean_op());
                    con_list.Children.Add(condition());
                    con_list.Children.Add(condition_list());
                    return con_list;
                }
            }
            return null;

        }

        Node Boolean_op()
        {

            if (Token_Class.And_Op == TokenStream[InputPointer].token_type)
            {
                Node bool_op = new Node("Boolean_op");
                bool_op.Children.Add(match(Token_Class.And_Op));
                return bool_op;
            }

            else if (Token_Class.Or_Op == TokenStream[InputPointer].token_type)
            {
                Node bool_op = new Node("Boolean_op");
                bool_op.Children.Add(match(Token_Class.Or_Op));
                return bool_op;
            }
            else
                return null;

        }

        Node condition()
        {

            if (Token_Class.Idenifier == TokenStream[InputPointer].token_type)
            {
                Node con = new Node("Condition");
                con.Children.Add(match(Token_Class.Idenifier));
                con.Children.Add(condition_op());
                con.Children.Add(Term());
                return con;
            }
            return null;
        }

        Node condition_op()
        {

            if (Token_Class.LessThanOp == TokenStream[InputPointer].token_type)
            {
                Node con_op = new Node("condition_op");
                con_op.Children.Add(match(Token_Class.LessThanOp));
                return con_op;
            }
            else if (Token_Class.GreaterThanOp == TokenStream[InputPointer].token_type)
            {
                Node con_op = new Node("condition_op");
                con_op.Children.Add(match(Token_Class.GreaterThanOp));
                return con_op;
            }
            else if (Token_Class.EqualOp == TokenStream[InputPointer].token_type)
            {
                Node con_op = new Node("condition_op");
                con_op.Children.Add(match(Token_Class.EqualOp));
                return con_op;
            }
            else if (Token_Class.NotEqualOp == TokenStream[InputPointer].token_type)
            {
                Node con_op = new Node("condition_op");
                con_op.Children.Add(match(Token_Class.NotEqualOp));
                return con_op;

            }
            else
                return null;

        }

        Node FunctionCall()
        {
            Node fun = new Node("FunctionCall");
            fun.Children.Add(match(Token_Class.Idenifier));
            fun.Children.Add(match(Token_Class.LParanthesis));
            fun.Children.Add(ArgList());
            fun.Children.Add(match(Token_Class.RParanthesis));
            return fun;
        }

        Node ArgList()
        {
            if (Token_Class.Idenifier == TokenStream[InputPointer].token_type)
            {
                Node ar = new Node("ArgList");
                ar.Children.Add(match(Token_Class.Idenifier));
                ar.Children.Add(Arg());
                return ar;
            }
            else if (Token_Class.Number == TokenStream[InputPointer].token_type)
            {
                Node ar = new Node("ArgList");
                ar.Children.Add(match(Token_Class.Number));
                ar.Children.Add(Arg());
                return ar;
            }

            else { return null; }
        }

        Node Arg()
        {

            if (Token_Class.Comma == TokenStream[InputPointer].token_type)
            {
                Node ar = new Node("Arg");
                ar.Children.Add(match(Token_Class.Comma));
                if (Token_Class.Idenifier == TokenStream[InputPointer].token_type)
                    ar.Children.Add(match(Token_Class.Idenifier));
                else if (Token_Class.Number == TokenStream[InputPointer].token_type)
                    ar.Children.Add(match(Token_Class.Number));
                ar.Children.Add(Arg());
                return ar;
            }
            else
                return null;

        }

        Node Term()
        {
            //Token_Class.Number == TokenStream[InputPointer].token_type ||Token_Class.Idenifier == TokenStream[InputPointer].token_type
            if (Token_Class.Number == TokenStream[InputPointer].token_type)
            {
                Node te = new Node("Term");
                te.Children.Add(match(Token_Class.Number));
                return te;
            }
            else if (Token_Class.Idenifier == TokenStream[InputPointer].token_type && InputPointer + 1 < TokenStream.Count && Token_Class.LParanthesis == TokenStream[InputPointer + 1].token_type)
            {

                Node te = new Node("Term");
                te.Children.Add(FunctionCall());
                return te;
            }

            else if (Token_Class.Idenifier == TokenStream[InputPointer].token_type && Token_Class.LParanthesis != TokenStream[InputPointer + 1].token_type)
            {
                Node te = new Node("Term");
                te.Children.Add(match(Token_Class.Idenifier));
                return te;
            }
            return null;
        }

        Node ArthOP()
        {
            //   Token_Class.PlusOp == TokenStream[InputPointer].token_type || Token_Class.MinusOp == TokenStream[InputPointer].token_type || Token_Class.MultiplyOp == TokenStream[InputPointer].token_type || Token_Class.DivideOp == TokenStream[InputPointer].token_type
            if (Token_Class.PlusOp == TokenStream[InputPointer].token_type)
            {
                Node te = new Node("ArthOP");
                te.Children.Add(match(Token_Class.PlusOp));
                return te;
            }
            else if (Token_Class.MinusOp == TokenStream[InputPointer].token_type)
            {
                Node te = new Node("ArthOP");
                te.Children.Add(match(Token_Class.MinusOp));
                return te;
            }
            else if (Token_Class.MultiplyOp == TokenStream[InputPointer].token_type)
            {
                Node te = new Node("ArthOP");
                te.Children.Add(match(Token_Class.MultiplyOp));
                return te;
            }
            else if (Token_Class.DivideOp == TokenStream[InputPointer].token_type)
            {
                Node te = new Node("ArthOP");
                te.Children.Add(match(Token_Class.DivideOp));
                return te;
            }
            return null;
        }

        Node Expression()
        {
            Node ex = new Node("Expression");
            if (InputPointer < TokenStream.Count)
            {

                if (Token_Class.Constant_String == TokenStream[InputPointer].token_type)
                {
                    ex.Children.Add(match(Token_Class.Constant_String));
                }
                else if ((Token_Class.Number == TokenStream[InputPointer].token_type || Token_Class.Idenifier == TokenStream[InputPointer].token_type)&&
                    (Token_Class.PlusOp == TokenStream[InputPointer + 1].token_type || Token_Class.MinusOp == TokenStream[InputPointer + 1].token_type ||
                     Token_Class.MultiplyOp == TokenStream[InputPointer + 1].token_type || Token_Class.DivideOp == TokenStream[InputPointer + 1].token_type))
                {
                    
                        ex.Children.Add(Equation());
                    
                }
                else if (Token_Class.Number == TokenStream[InputPointer].token_type || Token_Class.Idenifier == TokenStream[InputPointer].token_type)
                {
                    
                        ex.Children.Add(Term());

                }
                return ex;
            }
            return null;
        }

        Node Ass_Statement()
        {

            Node Assignment = new Node("Assignment_state_list");
            Assignment.Children.Add(match(Token_Class.Idenifier));
            Assignment.Children.Add(match(Token_Class.Assignment_Op));
            Assignment.Children.Add(Expression());
            Assignment.Children.Add(match(Token_Class.Semicolon));
            return Assignment;
        }

        Node Write_State()
        {
            Node w = new Node("Write_Statement");
            if (Token_Class.Write == TokenStream[InputPointer].token_type)
            {
                w.Children.Add(match(Token_Class.Write));
                if (Token_Class.endl == TokenStream[InputPointer].token_type)
                {
                    w.Children.Add(match(Token_Class.endl));
                }
                else
                    w.Children.Add(Expression());
                w.Children.Add(match(Token_Class.Semicolon));
                return w;
            }
            return null;
        }

        Node Return_Statement()
        {
            Node turn = new Node("Return_Statement");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Return == TokenStream[InputPointer].token_type)
                {
                    turn.Children.Add(match(Token_Class.Return));
                    turn.Children.Add(Expression());
                    turn.Children.Add(match(Token_Class.Semicolon));
                    return turn;
                }
            }
            return null;
        }

        Node DataType()
        {


            if (InputPointer < TokenStream.Count)
            {
                Node Datatype = new Node("DataType");

                if (Token_Class.Integer == TokenStream[InputPointer].token_type)
                {
                    Datatype.Children.Add(match(Token_Class.Integer));
                }
                else if (Token_Class.Float == TokenStream[InputPointer].token_type)
                {
                    Datatype.Children.Add(match(Token_Class.Float));

                }
                else if (Token_Class.String == TokenStream[InputPointer].token_type)
                {
                    Datatype.Children.Add(match(Token_Class.String));

                }
                return Datatype;
            }
            return null;
        }

        Node Decl_Statement()
        {
            Node Data = new Node("Decl_Statement");

            if (Token_Class.Integer == TokenStream[InputPointer].token_type || Token_Class.Float == TokenStream[InputPointer].token_type ||
                Token_Class.String == TokenStream[InputPointer].token_type)
            {
                Data.Children.Add(DataType());
                Data.Children.Add(Identifier_list());
                Data.Children.Add(match(Token_Class.Semicolon));
                return Data;
            }
            return null;
        }

        Node Identifier_list()
        {
            Node identif = new Node("Identifer_list");

            if (Token_Class.Idenifier == TokenStream[InputPointer].token_type && Token_Class.Assignment_Op == TokenStream[InputPointer + 1].token_type &&
                InputPointer + 1 < TokenStream.Count)
            {
                identif.Children.Add(match(Token_Class.Idenifier));
                identif.Children.Add(match(Token_Class.Assignment_Op));
                identif.Children.Add(Expression());
                identif.Children.Add(Ident_lists());
            }
            else if (Token_Class.Idenifier == TokenStream[InputPointer].token_type && Token_Class.Assignment_Op != TokenStream[InputPointer + 1].token_type)
            {
                identif.Children.Add(match(Token_Class.Idenifier));
                identif.Children.Add(Ident_lists());
            }
            return identif;

        }

        Node Ident_lists()
        {
            Node identif = new Node("Ident_lists");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Comma == TokenStream[InputPointer].token_type && Token_Class.Assignment_Op == TokenStream[InputPointer + 2].token_type &&
                    InputPointer + 2 < TokenStream.Count)
                {
                    identif.Children.Add(match(Token_Class.Comma));
                    identif.Children.Add(match(Token_Class.Idenifier));
                    identif.Children.Add(match(Token_Class.Assignment_Op));
                    identif.Children.Add(Expression());
                    identif.Children.Add(Ident_lists());
                    return identif;

                }
                else if (Token_Class.Comma == TokenStream[InputPointer].token_type)
                {
                    identif.Children.Add(match(Token_Class.Comma));
                    identif.Children.Add(match(Token_Class.Idenifier));
                    identif.Children.Add(Ident_lists());
                    return identif;
                }
            }
            return null;
        }

        Node Read_Statment()
        {
            Node Read_Stat = new Node("Read_Statment");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Read == TokenStream[InputPointer].token_type)
                {
                    Read_Stat.Children.Add(match(Token_Class.Read));
                    Read_Stat.Children.Add(match(Token_Class.Idenifier));
                    Read_Stat.Children.Add(match(Token_Class.Semicolon));
                    Read_Stat.Children.Add(Read_Statment());
                    return Read_Stat;
                }
            }
            return null;
        }

        Node if_Statement()
        {
            Node if_state = new Node("if_Statement");
            if (InputPointer < TokenStream.Count)
            {

                if_state.Children.Add(match(Token_Class.If));
                if_state.Children.Add(condition_statement());
                if_state.Children.Add(match(Token_Class.Then));
                if_state.Children.Add(statements());
                if (Token_Class.ElseIF == TokenStream[InputPointer].token_type)
                {
                    if_state.Children.Add(elseif());
                    return if_state;
                }
                if (Token_Class.Else == TokenStream[InputPointer].token_type)
                {
                    if_state.Children.Add(else_statement());
                    return if_state;
                }
                if (Token_Class.End == TokenStream[InputPointer].token_type)
                {
                    if_state.Children.Add(match(Token_Class.End));
                    return if_state;
                }


            }
            return null;
        }

        Node elseif()
        {
            Node if_state = new Node("elseif_Statement");
            if (InputPointer < TokenStream.Count)
            {
                if_state.Children.Add(match(Token_Class.ElseIF));
                if_state.Children.Add(condition_statement());
                if_state.Children.Add(match(Token_Class.Then));
                if_state.Children.Add(statements());
                if (Token_Class.ElseIF == TokenStream[InputPointer].token_type)
                {
                    if_state.Children.Add(elseif());
                }
                else if (Token_Class.Else == TokenStream[InputPointer].token_type)
                {
                    if_state.Children.Add(else_statement());
                }
                else if (Token_Class.End == TokenStream[InputPointer].token_type)
                {
                    if_state.Children.Add(match(Token_Class.End));
                }
                return if_state;
            }
            return null;
        }

        Node else_statement()
        {
            Node if_state = new Node("else_Statement");
            if (InputPointer < TokenStream.Count)
            {
                if_state.Children.Add(match(Token_Class.Else));
                if_state.Children.Add(statements());
                if (Token_Class.End == TokenStream[InputPointer].token_type)
                {
                    if_state.Children.Add(match(Token_Class.End));
                }
                return if_state;
            }
            return null;
        }

        Node Equation()
        {
            Node equ = new Node("Equation");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.LParanthesis == TokenStream[InputPointer].token_type)
                {
                    equ.Children.Add(match(Token_Class.LParanthesis));
                    equ.Children.Add(Term());
                    equ.Children.Add(extraTerm());                     
                    equ.Children.Add(bracket());
                    equ.Children.Add(extraTerm());
                    equ.Children.Add(match(Token_Class.RParanthesis));                   
                }
            }
            if (InputPointer < TokenStream.Count)
            {
                equ.Children.Add(Term());
            }
            equ.Children.Add(extraTerm());            
            equ.Children.Add(bracket());
            equ.Children.Add(extraTerm());            
            return equ;
        }

        Node extraTerm()
        {
            Node extra = new Node("extraTerm");
            if (InputPointer < TokenStream.Count)
            {
                if ((Token_Class.PlusOp == TokenStream[InputPointer].token_type || Token_Class.MinusOp == TokenStream[InputPointer].token_type ||
                    Token_Class.MultiplyOp == TokenStream[InputPointer].token_type || Token_Class.DivideOp == TokenStream[InputPointer].token_type)
                    && Token_Class.LParanthesis != TokenStream[InputPointer + 1].token_type)
                {
                    extra.Children.Add(ArthOP());
                    extra.Children.Add(Term());
                    extra.Children.Add(extraTerm());
                    return extra;
                }
            }
            return null;
        }

        Node bracket()
        {
            Node bra = new Node(" Bracket");
            if (InputPointer < TokenStream.Count)
            {
                if ((Token_Class.PlusOp == TokenStream[InputPointer].token_type || Token_Class.MinusOp == TokenStream[InputPointer].token_type ||
                    Token_Class.MultiplyOp == TokenStream[InputPointer].token_type || Token_Class.DivideOp == TokenStream[InputPointer].token_type)
                    && Token_Class.LParanthesis == TokenStream[InputPointer + 1].token_type)
                {
                    bra.Children.Add(ArthOP());
                    bra.Children.Add(match(Token_Class.LParanthesis));                   
                    bra.Children.Add(Term());
                    bra.Children.Add(extraTerm());                    
                    bra.Children.Add(bracket());
                    bra.Children.Add(extraTerm());
                    bra.Children.Add(match(Token_Class.RParanthesis));
                    bra.Children.Add(bracket());
                    return bra;
                }
            }
            return null;
        }

        public Node match(Token_Class ExpectedToken)
        {

            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Comment == TokenStream[InputPointer].token_type)
                {
                    InputPointer++;
                }


                if (ExpectedToken == TokenStream[InputPointer].token_type)
                {
                    InputPointer++;
                    Node newNode = new Node(ExpectedToken.ToString());

                    return newNode;

                }

                else
                {
                    
                        Errors.Error_List.Add("Parsing Error: Expected "
                            + ExpectedToken.ToString() + " and " +
                            TokenStream[InputPointer].token_type.ToString() +
                            "  found\r\n");
                    
                    InputPointer++;
                    return null;
                }
            }
            return null;

        }

        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }

        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }

    }
}

/*
 Node Equation()
        {
            Node equ = new Node("Equation");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.LParanthesis == TokenStream[InputPointer].token_type)
                {
                    equ.Children.Add(match(Token_Class.LParanthesis));
                    equ.Children.Add(Term());
                    equ.Children.Add(extraTerm());                     
                    equ.Children.Add(match(Token_Class.RParanthesis));                   
                }
            }
            if (InputPointer < TokenStream.Count)
            {
                equ.Children.Add(Term());
            }
            equ.Children.Add(extraTerm());            
            equ.Children.Add(bracket());           
            return equ;
        }

        Node extraTerm()
        {
            Node extra = new Node("extraTerm");
            if (InputPointer < TokenStream.Count)
            {
                if ((Token_Class.PlusOp == TokenStream[InputPointer].token_type || Token_Class.MinusOp == TokenStream[InputPointer].token_type ||
                    Token_Class.MultiplyOp == TokenStream[InputPointer].token_type || Token_Class.DivideOp == TokenStream[InputPointer].token_type)
                    && Token_Class.LParanthesis != TokenStream[InputPointer + 1].token_type)
                {
                    extra.Children.Add(ArthOP());
                    extra.Children.Add(Term());
                    extra.Children.Add(extraTerm());
                    return extra;
                }
            }
            return null;
        }

        Node bracket()
        {
            Node bra = new Node(" Bracket");
            if (InputPointer < TokenStream.Count)
            {
                if ((Token_Class.PlusOp == TokenStream[InputPointer].token_type || Token_Class.MinusOp == TokenStream[InputPointer].token_type ||
                    Token_Class.MultiplyOp == TokenStream[InputPointer].token_type || Token_Class.DivideOp == TokenStream[InputPointer].token_type)
                    && Token_Class.LParanthesis == TokenStream[InputPointer + 1].token_type)
                {
                    bra.Children.Add(ArthOP());
                    bra.Children.Add(match(Token_Class.LParanthesis));                   
                    bra.Children.Add(Term());
                    bra.Children.Add(extraTerm()); 
                    bra.Children.Add(match(Token_Class.RParanthesis));
                    return bra;
                }
            }
            return null;
        }
*/