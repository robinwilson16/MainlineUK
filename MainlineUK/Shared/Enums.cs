using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MainlineUK.Shared
{
    public static class Enums
    {
        public static string GetDescription(Enum value)
        {
            string Description = value.ToString();
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

            string ReturnValue;

            if((DisplayAttribute)fieldInfo.GetCustomAttribute(typeof(DisplayAttribute)) != null)
            {
                var Attribute = (DisplayAttribute)fieldInfo.GetCustomAttribute(typeof(DisplayAttribute));
                ReturnValue = Attribute.Name;
            }
            else
            {
                ReturnValue = value.ToString();
            }

            return ReturnValue;
        }
    }
}
