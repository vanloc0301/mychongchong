namespace SkyMap.Net.Security
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Security.Principal;
    using System;

    public class IsInRoleConditionEvaluator : IConditionEvaluator
    {
        public bool IsValid(object caller, Condition condition, Codon codon)
        {
            bool flag = false;
            SmPrincipal smPrincipal = SecurityUtil.GetSmPrincipal();
            SmIdentity identity = smPrincipal.Identity as SmIdentity;
            if (identity.AdminLevel != AdminLevelType.NotAdmin)
            {
                flag = true;
            }
            if (!flag)
            {
                string str = null;
                if (condition.Properties.Contains("rolename"))
                {
                    str = condition.Properties["rolename"];
                }
                if (string.IsNullOrEmpty(str) && (codon != null))
                {
                    if (codon.Properties.Contains("rolename"))
                    {
                        str = codon.Properties["rolename"];
                    }
                    else if (codon.Properties.Contains("class"))
                    {
                        str = codon.Properties["class"];
                    }
                }
                if (str == "*")
                {
                    return true;
                }
                if (!string.IsNullOrEmpty(str))
                {
                    flag = smPrincipal.IsInRole(str);
                }
            }
            return flag;
        }
    }
}

