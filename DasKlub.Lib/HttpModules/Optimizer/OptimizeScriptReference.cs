using System;
using System.Web.UI;
using System.Reflection;

namespace AdShoreLib.AspNetPerformanceOptimizer.ScriptOptimizer
{
    internal class OptimizeScriptReference : ScriptReference
    {
        public OptimizeScriptReference(ScriptReference reference) : base()
        {
            Name = reference.Name;
            Assembly = reference.Assembly;
            Path = reference.Path;
            IgnoreScriptPath = reference.IgnoreScriptPath;
            NotifyScriptLoaded = false;
            ResourceUICultures = reference.ResourceUICultures;
            ScriptMode = reference.ScriptMode;
        }
        public string GetUrl(ScriptManager scriptManager)
        {
            string url = string.Empty;
            if (String.IsNullOrEmpty(Path))
            {
                try
                {
                    PropertyInfo piScriptManager_IControl = scriptManager.GetType().GetProperty("Control", BindingFlags.NonPublic | BindingFlags.Instance);
                    MethodInfo miScriptReference_GetUrl = typeof(ScriptReference).GetMethod("GetUrl", BindingFlags.NonPublic | BindingFlags.Instance);
                    Type typeIControl = Type.GetType(piScriptManager_IControl.PropertyType.AssemblyQualifiedName.ToString(), false, true);
                    //object value = Convert.ChangeType(piScriptManager_IControl.PropertyType, typeIControl);
                    object value = piScriptManager_IControl.GetValue(scriptManager, null);
                    url = (string)miScriptReference_GetUrl.Invoke(this, new object[] { scriptManager, value, false });

                    /*return base.GetUrl(scriptManager, false); //Ajax 3.5*/
                    //MethodInfo miGetScriptResourceUrl = typeof(ScriptManager).GetMethod("GetScriptResourceUrl", BindingFlags.NonPublic | BindingFlags.Instance);
                    //Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
                    //url = (string)miGetScriptResourceUrl.Invoke(scriptManager, new object[] { Name, asm });
                }
                catch (Exception ex) { }
            }
            else
            {
                url = scriptManager.ResolveClientUrl(Path);
            }
            return url;
        }
        public bool IsFromSystemWebExtensions
        {
            get { return false; /*base.IsFromSystemWebExtensions(); //Ajax 3.5*/ }
        }
    }
}
