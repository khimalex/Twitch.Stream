using System;
using System.Collections.Generic;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils.Conventions;

namespace Twitch.Stream.Attributes
{
   [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
   internal class OptionRequiredAttribute : Attribute, IMemberConvention
   {

      public void Apply(ConventionContext context, MemberInfo member)
      {
         if (member is PropertyInfo pi)
         {
            context.Application.OnParsingComplete(r =>
            {
               Object commandModel = context.ModelAccessor.GetModel();
               if (pi.GetValue(commandModel) is null)
               {
                  Boolean isIEnum = pi.PropertyType.IsAssignableFrom(typeof(IEnumerable<>));
               }
               var p = pi;
               var c = r.SelectedCommand;
            });

         }
         if (member is FieldInfo fi)
         {

         }

      }
   }
}
