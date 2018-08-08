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

namespace SimpleBDD.SpecGeneration
{
    public static class GenerateSpecs
    {
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

                str.Append($"# This file is auto-generated, any changes made to this file will be lost\r\n\r\n\r\n");

                str.Append($"Feature: {featureAttr.Title}\r\n\t{featureAttr.Description}");

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

        private static void GenerateSpecsForInlineSyntax(Assembly assembly, StringBuilder str, Type featureScenarios)
        {
            var decompiler = new CSharpDecompiler(assembly.Location, new DecompilerSettings { ThrowOnAssemblyResolveErrors = false });
            var name = new FullTypeName(featureScenarios.FullName);
            var src = decompiler.DecompileTypeAsString(name);
            var refs = new List<MetadataReference>() {
                        MetadataReference.CreateFromFile(typeof(SimpleBDD.InlineSpec).Assembly.Location),
                    };


            var tree = CSharpSyntaxTree.ParseText(src);
            var node = (CompilationUnitSyntax)tree.GetRoot();
            var model = CSharpCompilation.Create(assembly.GetName().Name)
                                    .AddSyntaxTrees(tree)
                                    .AddReferences(refs)
                                    .GetSemanticModel(tree);


            var methods = featureScenarios.GetMethods().Where(m => m.GetCustomAttributes(typeof(BDDAttribute), false).Length > 0).ToArray();
            var attrs = new List<BDDAttribute>();
            foreach (MethodInfo m in methods)
            {
                var spec = (Spec)m.GetCustomAttribute(typeof(Spec));
                str.Append($"\r\n\r\nScenario: {spec.Spec}");

                var mtdWalker = new MethodWalker(m.Name);
                mtdWalker.Visit(node);

                foreach (ExpressionStatementSyntax exp in mtdWalker.Node.Body.Statements)
                {
                    //this will be a statement in method body
                    if (exp.Expression.Kind() == SyntaxKind.InvocationExpression)
                    {
                        var inv = (InvocationExpressionSyntax)exp.Expression;

                        str.Append($"\r\n\t\t\t{inv.Expression.ToString()} {inv.ArgumentList.Arguments[0].ToString().Trim('"')}");
                    }
                }

            }
        }

        private static void GenerateSpecsForDecoratorBasedSyntax(StringBuilder str, Type featureScenarios, IEnumerable<Attribute> senarios)
        {
            str.Append($"\r\n\r\nScenario: {((Scenario)senarios.First()).Title}\r\n\t\t\t");

            var methods = featureScenarios.GetMethods().Where(m => m.GetCustomAttributes(typeof(BDDAttribute), false).Length > 0).ToArray();
            var attrs = new List<BDDAttribute>();

            foreach (IEnumerable<BDDAttribute> m in methods.Select(s => s.GetCustomAttributes((typeof(BDDAttribute)))))
            {
                attrs.AddRange(m);
            }

            str.Append(string.Join("\r\n\t\t\t", attrs.Select(s => s.GetType().Name + " " + string.Format(s.Spec, s.args))));
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