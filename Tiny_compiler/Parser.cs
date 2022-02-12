using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Tiny_compiler
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
        bool return_exist;
        bool arithOpExist;
        public Node StartParsing(List<Token> TokenStream)
        {
            this.return_exist = false;
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = new Node("Program");
            root.Children.Add(Program());
            return root;
        }
        Node Program()
        {
            Node program = new Node("Program");
            program.Children.Add(FunctionStatements());
            MessageBox.Show("Success");
            return program;
        }
        Node FunctionStatements()
        {
            Node functionStatements = new Node("FunctionStatements");
            functionStatements.Children.Add(Function_statement());
            functionStatements.Children.Add(FunctionStatements_());
            return functionStatements;
        }
        Node FunctionStatements_()
        {
            Node functionStatements_ = new Node("FunctionStatements_");
            if (InputPointer < TokenStream.Count)
            {
                functionStatements_.Children.Add(FunctionStatements());
                return functionStatements_;
            }
            return null;
        }
        Node Function_statement()
        {
            Node func_state = new Node("Function_statement");
            func_state.Children.Add(Function_Declaration());
            func_state.Children.Add(Function_Body());
            return func_state;
        }
        Node Function_Declaration()
        {
            Node func_declare = new Node("Function_Declaration");
            func_declare.Children.Add(DataType());
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Main)
                func_declare.Children.Add(match(Token_Class.Main));
            else
                func_declare.Children.Add(match(Token_Class.Idenifier));
            func_declare.Children.Add(match(Token_Class.LParanthesis));
            func_declare.Children.Add(Parameters());
            func_declare.Children.Add(match(Token_Class.RParanthesis));
            return func_declare;
        }
        Node Function_Body()
        {
            Node func_body = new Node("Function_Body");
            func_body.Children.Add(match(Token_Class.LeftBraces));
            func_body.Children.Add(Set_of_statements());
            if (this.return_exist == false)
                func_body.Children.Add(Return_Statement());
            this.return_exist = false;
            func_body.Children.Add(match(Token_Class.RightBraces));
            return func_body;
        }
        Node Parameters()
        {
            Node parameters = new Node("Parameters");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.Int || TokenStream[InputPointer].token_type == Token_Class.Float || TokenStream[InputPointer].token_type == Token_Class.String)
                {
                    parameters.Children.Add(Parameter());
                    parameters.Children.Add(Param());
                    return parameters;
                }
            }
            return null;
        }
        Node Parameter()
        {
            Node parameter = new Node("Parameter");
            parameter.Children.Add(DataType());
            parameter.Children.Add(match(Token_Class.Idenifier));
            return parameter;
        }
        Node Param()
        {
            Node param = new Node("Param");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                param.Children.Add(match(Token_Class.Comma));
                param.Children.Add(Parameters());
                return param;
            }
            return null;
        }
        Node Term()
        {
            Node term = new Node("Term");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Number)
            {
                term.Children.Add(match(Token_Class.Number));
            }
            else
            {
                term.Children.Add(match(Token_Class.Idenifier));
                term.Children.Add(Term_());
            }

            return term;
        }
        Node Term_()
        {
            Node term_ = new Node("Term_");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
            {
                term_.Children.Add(match(Token_Class.LParanthesis));
                term_.Children.Add(Arguments());
                term_.Children.Add(match(Token_Class.RParanthesis));
                return term_;
            }
            else
                return null;
        }
        Node Arguments()
        {
            Node arguments = new Node("Arguments");
            arguments.Children.Add(match(Token_Class.Idenifier));
            arguments.Children.Add(Argument());
            return arguments;
        }
        Node Argument()
        {
            Node argument = new Node("Argument");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                argument.Children.Add(match(Token_Class.Comma));
                argument.Children.Add(match(Token_Class.Idenifier));
                argument.Children.Add(Argument());
                return argument;
            }
            return null;
        }
        Node DataType()
        {
            Node datatype = new Node("DataType");

            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.String)
            {
                datatype.Children.Add(match(Token_Class.String));

            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Float)
            {
                datatype.Children.Add(match(Token_Class.Float));

            }
            else
                datatype.Children.Add(match(Token_Class.Int));

            return datatype;
        }
        Node Read_Statement()
        {
            Node node = new Node("Read_Statement");

            node.Children.Add(match(Token_Class.Read));
            node.Children.Add(match(Token_Class.Idenifier));
            node.Children.Add(match(Token_Class.Semicolon));
            return node;
        }
        Node Return_Statement()
        {
            Node node = new Node("Return_Statement");


            node.Children.Add(match(Token_Class.Return));
            node.Children.Add(Expression());
            node.Children.Add(match(Token_Class.Semicolon));
            return node;

        }
        Node Repeat_Statement()
        {
            Node node = new Node("Repeat_Statement");

            node.Children.Add(match(Token_Class.Repeat));
            node.Children.Add(Set_of_statements());
            node.Children.Add(match(Token_Class.Until));
            node.Children.Add(Condition_Statement());
            return node;
        }
        Node Write_Statement()
        {
            Node node = new Node("Write_Statement");

            node.Children.Add(match(Token_Class.Write));
            node.Children.Add(Write_St());
            node.Children.Add(match(Token_Class.Semicolon));
            return node;
        }
        Node Write_St()
        {
            Node node = new Node("Write_St");

            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Endl)
                node.Children.Add(match(Token_Class.Endl));

            else
                node.Children.Add(Expression());

            return node;
        }
        Node Expression()
        {
            Node expression = new Node("Expression");
            arithOpExist = false;

            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.String)
                expression.Children.Add(match(Token_Class.String));
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
            {
                expression.Children.Add(match(Token_Class.LParanthesis));
                expression.Children.Add(Equation());
                expression.Children.Add(match(Token_Class.RParanthesis));
                expression.Children.Add(Equation_());
                if (!arithOpExist)
                    Errors.Error_List.Add("Arithmatic operator should be found in equation\r\n");
            }
            else
            {
                expression.Children.Add(Term());
                expression.Children.Add(Equation_());
            }

            return expression;
        }
        Node ArithmaticOp()
        {
            Node arithmaticOp = new Node("ArithmaticOp");
            arithOpExist = true;

            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.MinusOp)
                arithmaticOp.Children.Add(match(Token_Class.MinusOp));
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.DivisionOp)
                arithmaticOp.Children.Add(match(Token_Class.DivisionOp));
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.MultiplyOp)
                arithmaticOp.Children.Add(match(Token_Class.MultiplyOp));
            else
                arithmaticOp.Children.Add(match(Token_Class.PlusOp));

            return arithmaticOp;
        }
        Node Equation()
        {
            Node equation = new Node("Equation");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
            {
                equation.Children.Add(match(Token_Class.LParanthesis));
                equation.Children.Add(Equation());
                equation.Children.Add(match(Token_Class.RParanthesis));
                equation.Children.Add(Equation_());
            }
            else
            {
                equation.Children.Add(Term());
                equation.Children.Add(Equation_());
            }
            return equation;
        }
        Node Equation_()
        {
            Node equation_ = new Node("Equation_");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.PlusOp || TokenStream[InputPointer].token_type == Token_Class.MinusOp
                || TokenStream[InputPointer].token_type == Token_Class.DivisionOp || TokenStream[InputPointer].token_type == Token_Class.MultiplyOp)
                {
                    equation_.Children.Add(ArithmaticOp());
                    equation_.Children.Add(Equation());
                    return equation_;
                }
            }
            return null;
        }
        Node Condition_Statement()
        {
            Node condition_statement = new Node("Condition_Statement");
            condition_statement.Children.Add(Condition());
            condition_statement.Children.Add(Con_St());
            return condition_statement;
        }
        Node Condition()
        {
            Node node = new Node("Condition");

            node.Children.Add(match(Token_Class.Idenifier));
            node.Children.Add(Condition_Operator());
            node.Children.Add(Term());
            return node;
        }
        Node Condition_Operator()
        {
            Node node = new Node("Condition_Operator");

            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.LessThanOp)
                node.Children.Add(match(Token_Class.LessThanOp));

            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.GreaterThanOp)
                node.Children.Add(match(Token_Class.GreaterThanOp));

            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.NotEqualOp)
                node.Children.Add(match(Token_Class.NotEqualOp));

            else
                node.Children.Add(match(Token_Class.EqualOp));

            return node;
        }
        Node Con_St()
        {
            Node node = new Node("Con_St");
            if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.AndOp || TokenStream[InputPointer].token_type == Token_Class.OrOp))
            {
                node.Children.Add(Boolean_Operator());
                node.Children.Add(Condition());
                node.Children.Add(Con_St());
                return node;
            }
            return null;
        }
        Node Boolean_Operator()
        {
            Node node = new Node("Boolean_Operator");

            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.OrOp)
                node.Children.Add(match(Token_Class.OrOp));

            else
                node.Children.Add(match(Token_Class.AndOp));

            return node;
        }
        Node Else_Statement()
        {
            Node else_statement = new Node("Else_Statement");

            if (InputPointer < TokenStream.Count && Token_Class.Else == TokenStream[InputPointer].token_type)
            {
                else_statement.Children.Add(match(Token_Class.Else));
                else_statement.Children.Add(Set_of_statements());
                return else_statement;
            }
            return null;
        }
        Node Else_If_Statement()
        {
            Node else_if_statement = new Node("Else_If_Statement");

            if (InputPointer < TokenStream.Count && Token_Class.Elseif == TokenStream[InputPointer].token_type)
            {
                else_if_statement.Children.Add(match(Token_Class.Elseif));
                else_if_statement.Children.Add(Condition_Statement());
                else_if_statement.Children.Add(match(Token_Class.Then));
                else_if_statement.Children.Add(Set_of_statements());
                else_if_statement.Children.Add(Else_If_Statement());
                return else_if_statement;
            }

            return null;
        }
        Node If_Statement()
        {
            Node if_statement = new Node("If_Statement");

            if_statement.Children.Add(match(Token_Class.If));
            if_statement.Children.Add(Condition_Statement());
            if_statement.Children.Add(match(Token_Class.Then));
            if_statement.Children.Add(Set_of_statements());
            if_statement.Children.Add(Else_If_Statement());
            if_statement.Children.Add(Else_Statement());
            if_statement.Children.Add(match(Token_Class.End));

            return if_statement;
        }
        Node Declaration_Statetment()
        {
            Node node = new Node("Declaration_Statetment");
            node.Children.Add(DataType());
            node.Children.Add(Idlist());
            node.Children.Add(match(Token_Class.Semicolon));
            return node;
        }
        Node Idlist()
        {
            Node node = new Node("Idlist");
            node.Children.Add(ID());
            node.Children.Add(Id_list());
            return node;
        }
        Node Id_list()
        {
            Node node = new Node("Id_list");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                node.Children.Add(match(Token_Class.Comma));
                node.Children.Add(ID());
                node.Children.Add(Id_list());
                return node;
            }
            return null;
        }
        Node ID()
        {
            Node node = new Node("ID");
            node.Children.Add(match(Token_Class.Idenifier));
            node.Children.Add(ID_());
            return node;
        }
        Node ID_()
        {
            Node node = new Node("ID_");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Assign)
            {
                node.Children.Add(match(Token_Class.Assign));
                node.Children.Add(Expression());
                return node;
            }
            return null;
        }
        Node Assignment_Function_Call()
        {
            Node node = new Node("Assignment_Function_Call");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Assign)
            {
                node.Children.Add(match(Token_Class.Assign));
                node.Children.Add(Expression());
            }

            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
            {
                node.Children.Add(match(Token_Class.LParanthesis));
                node.Children.Add(Arguments());
                node.Children.Add(match(Token_Class.RParanthesis));
            }
            node.Children.Add(match(Token_Class.Semicolon));
            return node;
        }
        Node Set_of_statements()
        {
            Node set_of_statements = new Node("Set_of_statements");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comment)
            {
                set_of_statements.Children.Add(match(Token_Class.Comment));
                set_of_statements.Children.Add(Set_of_statements());
                return set_of_statements;
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Read)
            {

                set_of_statements.Children.Add(Read_Statement());
                set_of_statements.Children.Add(Set_of_statements());
                return set_of_statements;
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Write)
            {

                set_of_statements.Children.Add(Write_Statement());
                set_of_statements.Children.Add(Set_of_statements());
                return set_of_statements;
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Repeat)
            {

                set_of_statements.Children.Add(Repeat_Statement());
                set_of_statements.Children.Add(Set_of_statements());
                return set_of_statements;
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Return)
            {
                this.return_exist = true;
                set_of_statements.Children.Add(Return_Statement());
                set_of_statements.Children.Add(Set_of_statements());
                return set_of_statements;
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.If)
            {

                set_of_statements.Children.Add(If_Statement());
                set_of_statements.Children.Add(Set_of_statements());
                return set_of_statements;
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                set_of_statements.Children.Add(match(Token_Class.Idenifier));
                set_of_statements.Children.Add(Assignment_Function_Call());
                set_of_statements.Children.Add(Set_of_statements());
                return set_of_statements;
            }
            else if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.Int ||
                TokenStream[InputPointer].token_type == Token_Class.Float || TokenStream[InputPointer].token_type == Token_Class.String))
            {

                set_of_statements.Children.Add(Declaration_Statetment());
                set_of_statements.Children.Add(Set_of_statements());
                return set_of_statements;
            }
            else
                return null;
        }

        public Node match(Token_Class ExpectedToken)
        {

            if (InputPointer < TokenStream.Count)
            {
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
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + "\r\n");
                InputPointer++;
                return null;
            }
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