using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.TypeSystem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Mono.Cecil;
using Xunit;

namespace CoreBDD.SpecGeneration
{
    public static class GenerateSpecs
    {
        private const string FormatWhitespace = "\r\n\t\t\t";
        private const string SingleLine = "\r\n";
        private const string Tab = "\t";
        public static void OutputFeatureSpecs(string assemblyPath, string specFilePath)
        {
            OutputFeatureSpecs(Assembly.LoadFile(assemblyPath), specFilePath);
        }
        public static void OutputFeatureSpecs(Assembly assembly, string specFilePath)
        {
            var featureClasses = GetFeatureClasses(assembly);
            var str = new StringBuilder();

            foreach (var feature in featureClasses)
            {
                Feature featureAttr = (Feature)feature.GetCustomAttributes((typeof(Feature))).First();//should only be one feature attribute

                str.Append($"# This file is auto-generated, any changes made to this file will be lost{SingleLine}{SingleLine}{SingleLine}");

                str.Append($"Feature: {featureAttr.Title}{SingleLine}{Tab}{featureAttr.Description}");

                WriteFeatureScenarios(assembly, str, feature);

                if(!Directory.Exists(specFilePath))
                    Directory.CreateDirectory(specFilePath);
                    
                File.WriteAllText($"{specFilePath}{feature.Name}.spec", str.ToString());
                str.Clear();
            }

        }

        private static void WriteFeatureScenarios(Assembly assembly, StringBuilder str, Type feature)
        {
            foreach (var featureScenarios in assembly.GetTypes().Where(f => f.IsSubclassOf(feature)))
            {
                //can be standard or lean syntax style of tests

                var senarios = featureScenarios.GetCustomAttributes((typeof(Scenario)));//currently one senario per class

                if (senarios.Any())
                    GenerateSpecsForDecoratorBasedSyntax(str, featureScenarios, senarios);
                else
                    GenerateSpecsForInlineSyntax(assembly, str, featureScenarios);

            }

        }

        //todo - tidy/refactor this method
        private static void GenerateSpecsForInlineSyntax(Assembly assembly, StringBuilder str, Type featureScenarios)
        {
            var decompiler = new CSharpDecompiler(assembly.Location, new DecompilerSettings { ThrowOnAssemblyResolveErrors = false });
            var name = new FullTypeName(featureScenarios.FullName);
            var src = decompiler.DecompileTypeAsString(name);
            var refs = new List<MetadataReference>() {
                        MetadataReference.CreateFromFile(typeof(CoreBDD.InlineSpec).Assembly.Location),
                    };


            var tree = CSharpSyntaxTree.ParseText(src);
            var node = (CompilationUnitSyntax)tree.GetRoot();
            var model = CSharpCompilation.Create(assembly.GetName().Name)
                                    .AddSyntaxTrees(tree)
                                    .AddReferences(refs)
                                    .GetSemanticModel(tree);


            var methods = featureScenarios.GetMethods().Where(m => m.GetCustomAttributes(typeof(BDDAttribute), false).Length > 0).ToList();
            methods.AddRange(featureScenarios.GetMethods().Where(m => m.GetCustomAttributes(typeof(DataDrivenSpec), false).Length > 0));

            var attrs = new List<BDDAttribute>();
            foreach (MethodInfo m in methods.Where(f=> f.GetCustomAttribute(typeof(Spec)) != null))
            {
                var spec = (Spec)m.GetCustomAttribute(typeof(Spec));
                str.Append($"{SingleLine}{SingleLine}Scenario: {spec.Spec}");

                var mtdWalker = new MethodWalker(m.Name);
                mtdWalker.Visit(node);

                foreach (ExpressionStatementSyntax exp in mtdWalker.Node.Body.Statements.Where(f=> f.Kind() == SyntaxKind.ExpressionStatement))
                {
                    //this will be a statement in method body
                    if (exp.Expression.Kind() == SyntaxKind.InvocationExpression && exp.Expression.ToString().IsGherkin())
                    {
                        var inv = (InvocationExpressionSyntax)exp.Expression;

                        str.Append($"{FormatWhitespace}{inv.Expression.ToString().TrimExpression()} {inv.ArgumentList.Arguments[0].ToString().TrimArgument()}");
                    }
                }

            }

            foreach (MethodInfo m in methods.Where(f=> f.GetCustomAttribute(typeof(DataDrivenSpec)) != null))
            {
                var spec = (DataDrivenSpec)m.GetCustomAttribute(typeof(DataDrivenSpec));
                str.Append($"{SingleLine}{SingleLine}Scenario: {spec.Spec}");

                var inlineData = m.GetCustomAttributes(typeof(InlineDataAttribute));

                var mtdWalker = new MethodWalker(m.Name);
                mtdWalker.Visit(node);

                var methodParams = mtdWalker.Node.ParameterList.Parameters.Select(s=> s.Identifier);
                var iData = 0;
                foreach (InlineDataAttribute data in inlineData)
                {
                    if(iData > 0)
                        str.Append(SingleLine);
                    foreach (ExpressionStatementSyntax exp in mtdWalker.Node.Body.Statements.Where(f=> f.Kind() == SyntaxKind.ExpressionStatement))
                    {
                        //todo - this can be tighted up
                        if (exp.Expression.Kind() == SyntaxKind.InvocationExpression && exp.Expression.ToString().IsGherkin())
                        {
                            var inv = (InvocationExpressionSyntax)exp.Expression;

                            var argData = data.GetData(m).First().ToArray();
                            var statement = inv.ArgumentList.Arguments[0].ToString().TrimArgument();
                            var index = 0;
                            foreach (var param in methodParams)
                            {
                                statement = statement.Replace("{" + param + "}", argData[index].ToString());    
                                index++;
                            }
                            
                            str.Append($"{FormatWhitespace}{inv.Expression.ToString().TrimExpression()} {statement}");
                        }
                    }
                    iData++;
                }
            }
        }

        private static void GenerateSpecsForDecoratorBasedSyntax(StringBuilder str, Type featureScenarios, IEnumerable<Attribute> senarios)
        {
            str.Append($"{SingleLine}{SingleLine}Scenario: {((Scenario)senarios.First()).Title}{FormatWhitespace}");

            var methods = featureScenarios.GetMethods().Where(m => m.GetCustomAttributes(typeof(BDDAttribute), false).Length > 0).ToArray();
            var attrs = new List<BDDAttribute>();

            foreach (IEnumerable<BDDAttribute> m in methods.Select(s => s.GetCustomAttributes((typeof(BDDAttribute)))))
            {
                attrs.AddRange(m);
            }

            str.Append(string.Join($"{FormatWhitespace}", attrs.Select(s => s.GetType().Name + " " + string.Format(s.Spec, s.args))));
        }

        static IEnumerable<Type> GetFeatureClasses(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(Feature), false).Length > 0)
                {
                    yield return type;
                }
            }
        }
    }
}