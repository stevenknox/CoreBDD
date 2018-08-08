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

namespace SimpleBDD
{

    public class SimpleBDDFixture : IDisposable
    {
        public SimpleBDDFixture()
        {

        }

        public void Dispose()
        {
            OutputFeatureSpecs();
        }

        private void OutputFeatureSpecs()
        {
            var assembly = this.GetType().Assembly;
            var featureClasses = GetFeatureClasses(assembly);
            var str = new StringBuilder();

            foreach (var feature in featureClasses)
            {
                FeatureAttribute featureAttr = (FeatureAttribute)feature.GetCustomAttributes((typeof(FeatureAttribute))).First();//should only be one feature attribute

                str.Append($"# This file is auto-generated, any changes made to this file will be lost\r\n\r\n\r\n");

                str.Append($"Feature: {featureAttr.Title}\r\n\t{featureAttr.Description}");

                WriteFeatureScenarios(assembly, str, feature);

                File.WriteAllText($@"..\..\..\Specs\{feature.Name}.spec", str.ToString());
                str.Clear();
            }

        }

        private static void WriteFeatureScenarios(Assembly assembly, StringBuilder str, Type feature)
        {
            foreach (var featureScenarios in assembly.GetTypes().Where(f => f.IsSubclassOf(feature)))
            {
                //can be standard or lean syntax style of tests

                var senarios = featureScenarios.GetCustomAttributes((typeof(ScenarioAttribute)));//currently one senario per class

                if (senarios.Any())
                {
                    //standard style
                    str.Append($"\r\n\r\nScenario: {((ScenarioAttribute)senarios.First()).Title}\r\n\t\t\t");

                    var methods = featureScenarios.GetMethods().Where(m => m.GetCustomAttributes(typeof(BDDAttribute), false).Length > 0).ToArray();
                    var attrs = new List<BDDAttribute>();

                    foreach (IEnumerable<BDDAttribute> m in methods.Select(s => s.GetCustomAttributes((typeof(BDDAttribute)))))
                    {
                        attrs.AddRange(m);
                    }

                    str.Append(string.Join("\r\n\t\t\t", attrs.Select(s => s.GetType().Name + " " + string.Format(s.Spec, s.args))));
                }
                else
                {
                    //lean style
                    var decompiler = new CSharpDecompiler(assembly.Location, new DecompilerSettings());
                    var name = new FullTypeName(featureScenarios.FullName);
                    var src = decompiler.DecompileTypeAsString(name);
                    var refs = new List<MetadataReference>() {
                        MetadataReference.CreateFromFile(typeof(SimpleBDD.InlineSpecAttribute).Assembly.Location),
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
                        var spec = (SpecAttribute)m.GetCustomAttribute(typeof(SpecAttribute));
                        str.Append($"\r\n\r\nScenario: {spec.Spec}\r\n\t\t\t");

                        CompilationUnitSyntax orderNode;

                        var mtdWalker = new MethodWalker(m.Name);
                        mtdWalker.Visit(node);

                        var expWalker = new InvocationWalker("Given");
                        expWalker.Visit(expWalker.Node);
                        //looking for Spec
                        str.Append("\r\n\t\t\t" +  expWalker.Node.ToFullString());
                    }
                }

            }

        }

        static IEnumerable<Type> GetFeatureClasses(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(FeatureAttribute), false).Length > 0)
                {
                    yield return type;
                }
            }
        }
    }

    public class MethodWalker : CSharpSyntaxWalker
    {
        public MethodDeclarationSyntax Node { get; private set; }
        private readonly string _targetMethod;

        public MethodWalker(string targetMethod)
        {
            _targetMethod = targetMethod;
        }
        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            //to-do - need to ensure class etc
            if (node.Identifier.Text == _targetMethod)
            {
                Node = node;
            }

            base.VisitMethodDeclaration(node);
        }
    }

    public class InvocationWalker : CSharpSyntaxWalker
    {
        public InvocationExpressionSyntax Node { get; private set; }

        private readonly string _targetExpression;

        public InvocationWalker(string targetExpression)
        {
            _targetExpression = targetExpression;
        }
        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            if (node.Expression.ToString() == _targetExpression)
            {

                Node = node;
            }

            base.VisitInvocationExpression(node);
        }
    }
}