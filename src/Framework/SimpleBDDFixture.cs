using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

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
                var senarios = featureScenarios.GetCustomAttributes((typeof(ScenarioAttribute)));//currently one senario per class

                if(senarios.Any())
                    str.Append($"\r\n\r\nScenario: {((ScenarioAttribute)senarios.First()).Title}\r\n\t\t\t");


                var methods = featureScenarios.GetMethods().Where(m => m.GetCustomAttributes(typeof(BDDAttribute), false).Length > 0).ToArray();
                var attrs = new List<BDDAttribute>();

                foreach (IEnumerable<BDDAttribute> m in methods.Select(s => s.GetCustomAttributes((typeof(BDDAttribute)))))
                {
                    attrs.AddRange(m);
                }

                str.Append(string.Join("\r\n\t\t\t", attrs.Select(s => s.GetType().Name + " " + string.Format(s.Spec, s.args))));   
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
}