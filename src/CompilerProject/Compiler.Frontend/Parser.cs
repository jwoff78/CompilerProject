using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Compiler.Core;
using Compiler.Core.Parsing;
using Compiler.Frontend.Ast;
using Compiler.Frontend.Grammer;
using DotNetGraph;
using DotNetGraph.Extensions;
using DotNetGraph.Node;
using DotNetGraph.SubGraph;
using static Compiler.Core.Parsing.TokenType;

namespace Compiler.Frontend
{
    public static class Parser
    {
        private static List<(TokenType, Func<Token, AstNode>)> _typeConverterRules =
            new List<(TokenType, Func<Token, AstNode>)>
            {
                (Number, (x) => new ExprNode {Value = new LiteralValueNode {Raw = x}}),
                (Identifier, (x) => new ExprNode {Value = new IdentifierNode() {Raw = x}}),
            };


        public static DocumentNode Parse(TokenString ts)
        {
            var firstBuf = new List<AstNode>();

            //first we convert the toks into ast
            for (var i = 0; i < ts.Tokens.Count; i++)
            {
                firstBuf.Add(new AstNode {Raw = ts.Tokens[i], ReportToken = ts.Tokens[i]});
            }


            var passCount = Rules.PassRules.Count;

            for (int p = 0; p < passCount; p++)
            {
                if (p == 1)
                {
                    for (var i = 0; i < firstBuf.Count; i++)
                    {
                        var node = firstBuf[i];

                        foreach (var (type, func) in _typeConverterRules)
                        {
                            if (node.Raw?.Type == type)
                            {
                                firstBuf[i] = func(node.Raw);
                                firstBuf[i].ReportToken = node.ReportToken;
                            }
                        }
                    }
                }

                var passRules = Rules.PassRules[p];

                start_over:
                for (var i = 1; i < firstBuf.Count - 1; i++)
                {
                    if (firstBuf[i] == null) continue;


                    foreach (var (objects, func) in passRules)
                    {
                        var flag = true;

                        var args = new List<AstNode>();

                        for (var j = 0; j < objects.Length; j++)
                        {
                            var o = objects[j];
                            if ((o is Type t && firstBuf[i + (j - 1)].GetType().Name == t.Name) ||
                                (o is TokenType tt && firstBuf[i + (j - 1)].Raw?.Type == tt)
                            )
                            {
                                args.Add(firstBuf[i + (j - 1)]);
                            }
                            else
                            {
                                if (j >= firstBuf[i + (j - 1)].ClosestExpected)
                                {
                                    firstBuf[i + (j - 1)].ClosestExpected = j;
                                    firstBuf[i + (j - 1)].Expected = o switch
                                    {
                                        Type type => type.Name,
                                        TokenType toketype => toketype.ToString(),
                                        _ => "Unknown"
                                    };
                                    if (firstBuf[i + (j - 1)].Raw != null)
                                    {
                                        firstBuf[i + (j - 1)].Found = firstBuf[i + (j - 1)].Raw;
                                    }
                                    else
                                    {
                                        firstBuf[i + (j - 1)].Found = firstBuf[i + (j - 1)].ReportToken;
                                    }
                                }

                                flag = false;
                                break;
                            }
                        }

                        if (flag)
                        {
                            var ag = args.ToArray().ToList();

                            for (var index = 0; index < args.Count; index++)
                            {
                                args[index] = args[index].Drain();
                                args[index].Parsed = true;
                            }


                            var res = func(args.ToArray());
                            res.Parsed = true;

                            res.ReportToken = ag[0].ReportToken;

                            firstBuf.Insert(i, res);


                            foreach (var i1 in ag)
                            {
                                firstBuf.Remove(i1);
                            }


                            goto start_over;
                        }
                    }
                }
            }

            var re = new DocumentNode();

            foreach (var node in firstBuf)
            {
                if (node.Raw?.Type == Sof || node.Raw?.Type == Eof) continue;

                re.Statments.Add(node.Drain());
            }

            //now find errors

            var lowestEidx = 1000000;
            var ast = new AstNode();

            foreach (var node in firstBuf)
            {
                if (node.ReportToken?.Type == Eof || node.ReportToken?.Type == Sof) continue;


                if (!node.Parsed && node.ReportToken != null && node.Expected != "")
                {
                    if (node.ClosestExpected < lowestEidx)
                    {
                        lowestEidx = node.ClosestExpected;
                        ast = node;
                    }
                    
                    Logger.Debug(
                        $"Found Un-parsed Node '{node.ReportToken.Raw}' at [L{node.ReportToken.Line}C{node.ReportToken.Col}] of {node.ReportToken.Type} expected {node.Expected} eidx [{node.ClosestExpected}] found {node.Found.Type}");
                }
            }

            Logger.Error(
                $"Found Un-parsed Node '{ast.ReportToken.Raw}' at [L{ast.ReportToken.Line}C{ast.ReportToken.Col}] of {ast.ReportToken.Type} expected {ast.Expected} found {ast.Found.Type}");

            return re;
        }
    }
}