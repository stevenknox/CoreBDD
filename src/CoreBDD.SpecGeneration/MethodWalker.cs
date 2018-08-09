using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CoreBDD.SpecGeneration
{
    public class MethodWalker : CSharpSyntaxWalker
    {
        public MethodDeclarationSyntax Node { get; private set; }
        private readonly string _targetMethod;

        public MethodWalker(string targetMethod)
        {
            _targetMethod = targetMethod;
        }

        protected MethodWalker(SyntaxWalkerDepth depth = SyntaxWalkerDepth.Node) : base(depth)
        {
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            if (node.Identifier.Text == _targetMethod)
            {
                Node = node;
            }

            base.VisitMethodDeclaration(node);
        }
    }
}
