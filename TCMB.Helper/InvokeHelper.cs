using System;

namespace TCMB.Helper
{
    public class InvokeHelper
    {
        public static void ObjectInvoke(dynamic className, string objectName, object parameters)
        {
            className.GetType().GetMethod(objectName).Invoke(className, new Object[] {parameters});
        }
    }
}