using System;
using System.Linq;
using Altom.AltUnityDriver;
using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetAllMethodsCommand : AltUnityReflectionMethodsCommand
    {
        AltUnityComponent component;
        AltUnityMethodSelection methodSelection;

        public AltUnityGetAllMethodsCommand(params string[] parameters) : base(parameters, 4)
        {
            this.component = JsonConvert.DeserializeObject<AltUnityComponent>(parameters[2]);
            this.methodSelection = (AltUnityMethodSelection)Enum.Parse(typeof(AltUnityMethodSelection), parameters[3], true);
        }

        public override string Execute()
        {
            LogMessage("getAllMethods");
            System.Type type = GetType(component.componentName, component.assemblyName);
            System.Reflection.MethodInfo[] methodInfos = new System.Reflection.MethodInfo[1];
            switch (methodSelection)
            {
                case AltUnityMethodSelection.CLASSMETHODS:
                    methodInfos = type.GetMethods(System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
                    break;
                case AltUnityMethodSelection.INHERITEDMETHODS:
                    var allMethods = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
                    var classMethods = type.GetMethods(System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
                    methodInfos = allMethods.Except(classMethods).ToArray();
                    break;
                case AltUnityMethodSelection.ALLMETHODS:
                    methodInfos = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
                    break;
            }

            System.Collections.Generic.List<string> listMethods = new System.Collections.Generic.List<string>();

            foreach (var methodInfo in methodInfos)
            {
                listMethods.Add(methodInfo.ToString());
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(listMethods);
        }
    }
}
