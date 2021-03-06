using System;
using System.Collections.Generic;
using Compiler.Frontend.Ast;
using static Compiler.Core.Parsing.TokenType;
namespace Compiler.Frontend.Grammer
{
    public class Rules
    {
  public static List<List<(object[], Func<AstNode[], AstNode>)>> PassRules =
            new List<List<(object[], Func<AstNode[], AstNode>)>>
            {
   new List<(object[], Func<AstNode[], AstNode>)>
   {
(new object[] {Identifier,DoublePoint,Identifier,},
 (objs) => new NameTypePair () {Name = objs[0],Type = objs[2],}),
(new object[] {Identifier,OpenRoundBracket,},
 (objs) => new InvokeNodeName () {Target = objs[0],}),
(new object[] {Identifier,DoubleDouble,},
 (objs) => new ProcedureNode () {Name = objs[0],}),
(new object[] {At,Identifier,},
 (objs) => new AttributeNodeName () {Name = objs[1],}),
},
   new List<(object[], Func<AstNode[], AstNode>)>
   {
(new object[] {Not,typeof(ExprNode),},
  (objs) => new ExprNode   {Value = new UnaryOperator  {Operator = objs[0],Argument = objs[1],}}),
(new object[] {typeof(ExprNode),AndBitWise,typeof(ExprNode),},
  (objs) => new ExprNode   {Value = new AndBitWiseNode  {A = objs[0],B = objs[2],}}),
(new object[] {typeof(ExprNode),AndBool,typeof(ExprNode),},
  (objs) => new ExprNode   {Value = new AndBoolNode  {A = objs[0],B = objs[2],}}),
(new object[] {typeof(ExprNode),Divide,typeof(ExprNode),},
  (objs) => new ExprNode   {Value = new DivisionNode  {A = objs[0],B = objs[2],}}),
(new object[] {typeof(ExprNode),Multiply,typeof(ExprNode),},
  (objs) => new ExprNode   {Value = new MultiplicationNode  {A = objs[0],B = objs[2],}}),
(new object[] {typeof(ExprNode),Multiply,typeof(ExprNode),},
  (objs) => new ExprNode   {Value = new MultiplicationNode  {A = objs[0],B = objs[2],}}),
},
   new List<(object[], Func<AstNode[], AstNode>)>
   {
(new object[] {typeof(ExprNode),Plus,typeof(ExprNode),},
  (objs) => new ExprNode   {Value = new AdditionNode  {A = objs[0],B = objs[2],}}),
(new object[] {typeof(ExprNode),Minus,typeof(ExprNode),},
  (objs) => new ExprNode   {Value = new SubtractionNode  {A = objs[0],B = objs[2],}}),
(new object[] {typeof(ExprNode),OrBool,typeof(ExprNode),},
  (objs) => new ExprNode   {Value = new OrBoolNode  {A = objs[0],B = objs[2],}}),
(new object[] {typeof(ExprNode),EqBool,typeof(ExprNode),},
  (objs) => new ExprNode   {Value = new EqNode  {A = objs[0],B = objs[2],}}),
(new object[] {typeof(ExprNode),LessThan,typeof(ExprNode),},
  (objs) => new ExprNode   {Value = new ComparisonNode  {A = objs[0],Operator = objs[1],B = objs[2],}}),
(new object[] {typeof(ExprNode),GreaterThan,typeof(ExprNode),},
  (objs) => new ExprNode   {Value = new ComparisonNode  {A = objs[0],Operator = objs[1],B = objs[2],}}),
(new object[] {typeof(NameTypePair),Comma,typeof(NameTypePair),},
 (objs) => new ListNode () {A = objs[0],B = objs[2],}),
(new object[] {typeof(ListNode),Comma,typeof(NameTypePair),},
 (objs) => new ListNode () {A = objs[0],B = objs[2],}),
(new object[] {typeof(InvokeListNode),Comma,typeof(ExprNode),},
 (objs) => new InvokeListNode () {A = objs[0],B = objs[2],}),
(new object[] {typeof(ExprNode),Comma,typeof(ExprNode),},
 (objs) => new InvokeListNode () {A = objs[0],B = objs[2],}),
},
   new List<(object[], Func<AstNode[], AstNode>)>
   {
(new object[] {OpenRoundBracket,typeof(ListNode),CloseRoundBracket,},
 (objs) => new ProcedureArgsNode () {Args = objs[1],}),
(new object[] {OpenRoundBracket,typeof(NameTypePair),CloseRoundBracket,},
 (objs) => new ProcedureArgsNode () {Args = objs[1],}),
(new object[] {OpenRoundBracket,CloseRoundBracket,},
 (objs) => new ProcedureArgsNode () {}),
(new object[] {typeof(InvokeNodeName),typeof(InvokeListNode),CloseRoundBracket,SemiColon,},
  (objs) => new StatementNode   {Value = new InvokeNode  {Target = objs[0],Arguments = objs[1],}}),
(new object[] {typeof(InvokeNodeName),typeof(ExprNode),CloseRoundBracket,SemiColon,},
  (objs) => new StatementNode   {Value = new InvokeNode  {Target = objs[0],Arguments = objs[1],}}),
(new object[] {typeof(InvokeNodeName),CloseRoundBracket,SemiColon,},
  (objs) => new StatementNode   {Value = new InvokeNode  {Target = objs[0],}}),
},
   new List<(object[], Func<AstNode[], AstNode>)>
   {
(new object[] {typeof(ExprNode),Eq,typeof(ExprNode),SemiColon,},
  (objs) => new StatementNode   {Value = new AssignmentNode  {Name = objs[0],Value = objs[2],}}),
(new object[] {typeof(ExprNode),Eq,typeof(StatementNode),},
  (objs) => new StatementNode   {Value = new AssignmentNode  {Name = objs[0],Value = objs[2],}}),
(new object[] {typeof(ExprNode),DoubleEq,typeof(ExprNode),SemiColon,},
  (objs) => new StatementNode   {Value = new DeclNode  {Name = objs[0],Value = objs[2],}}),
(new object[] {typeof(ExprNode),DoubleEq,typeof(StatementNode),},
  (objs) => new StatementNode   {Value = new DeclNode  {Name = objs[0],Value = objs[2],}}),
(new object[] {typeof(CodeBlockList),typeof(StatementNode),},
 (objs) => new CodeBlockList () {A = objs[0],B = objs[1],}),
(new object[] {typeof(StatementNode),typeof(CodeBlockList),},
 (objs) => new CodeBlockList () {A = objs[0],B = objs[1],}),
(new object[] {typeof(StatementNode),typeof(StatementNode),},
 (objs) => new CodeBlockList () {A = objs[0],B = objs[1],}),
(new object[] {typeof(CodeBlockList),typeof(CodeBlockList),},
 (objs) => new CodeBlockList () {A = objs[0],B = objs[1],}),
(new object[] {typeof(CodeBlock),typeof(CodeBlock),},
 (objs) => new CodeBlockList () {A = objs[0],B = objs[1],}),
(new object[] {OpenCurlyBracket,typeof(CodeBlockList),CloseCurlyBracket,},
 (objs) => new CodeBlock () {Body = objs[1],}),
(new object[] {OpenCurlyBracket,typeof(StatementNode),CloseCurlyBracket,},
 (objs) => new CodeBlock () {Body = objs[1],}),
(new object[] {OpenCurlyBracket,CloseCurlyBracket,},
 (objs) => new CodeBlock () {}),
(new object[] {If,typeof(ExprNode),typeof(CodeBlock),},
  (objs) => new StatementNode   {Value = new IfNode  {Condition = objs[1],Body = objs[2],}}),
(new object[] {Loop,typeof(ExprNode),typeof(CodeBlock),},
  (objs) => new StatementNode   {Value = new LoopNode  {Condition = objs[1],Body = objs[2],}}),
},
   new List<(object[], Func<AstNode[], AstNode>)>
   {
(new object[] {typeof(ProcedureNode),typeof(ProcedureArgsNode),Arrow,typeof(ExprNode),SemiColon,},
 (objs) => new ProcedureNode () {Name = objs[0],Args = objs[1],ReturnType = objs[3],}),
(new object[] {typeof(ProcedureNode),typeof(ProcedureArgsNode),SemiColon,},
 (objs) => new ProcedureNode () {Name = objs[0],Args = objs[1],}),
(new object[] {OpenCurlyBracket,typeof(CodeBlockList),CloseCurlyBracket,},
 (objs) => new CodeBlock () {Body = objs[1],}),
(new object[] {OpenCurlyBracket,typeof(StatementNode),CloseCurlyBracket,},
 (objs) => new CodeBlock () {Body = objs[1],}),
(new object[] {OpenCurlyBracket,CloseCurlyBracket,},
 (objs) => new CodeBlock () {}),
(new object[] {typeof(ProcedureNode),typeof(ProcedureArgsNode),Arrow,typeof(ExprNode),typeof(CodeBlock),},
 (objs) => new ProcedureNode () {Name = objs[0],Args = objs[1],ReturnType = objs[3],Body = objs[4],}),
(new object[] {typeof(ProcedureNode),typeof(ProcedureArgsNode),typeof(CodeBlock),},
 (objs) => new ProcedureNode () {Name = objs[0],Args = objs[1],Body = objs[2],}),
},
   new List<(object[], Func<AstNode[], AstNode>)>
   {
(new object[] {typeof(AttributeNodeName),typeof(ProcedureNode),},
 (objs) => new AttributeNode () {Name = objs[0],Client = objs[1],}),
}
   };
    }
}
