namespace SkyMap.Net.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    public class Codon
    {
        private SkyMap.Net.Core.AddIn addIn;
        private ICondition[] conditions;
        private string name;
        private SkyMap.Net.Core.Properties properties;

        public Codon(SkyMap.Net.Core.AddIn addIn, string name, SkyMap.Net.Core.Properties properties, ICondition[] conditions)
        {
            this.addIn = addIn;
            this.name = name;
            this.properties = properties;
            this.conditions = conditions;
        }

        public object BuildItem(object owner, ArrayList subItems)
        {
            object obj2;
            try
            {
                IDoozer doozer = AddInTree.Doozers[this.Name];
                if ((!doozer.HandleConditions && (this.conditions.Length > 0)) && (this.GetFailedAction(owner) != ConditionFailedAction.Nothing))
                {
                    return null;
                }
                obj2 = doozer.BuildItem(owner, this, subItems);
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Doozer " + this.Name + " not found!");
            }
            return obj2;
        }

        public ConditionFailedAction GetFailedAction(object caller)
        {
            ConditionFailedAction nothing = ConditionFailedAction.Nothing;
            foreach (ICondition condition in this.conditions)
            {
                try
                {
                    if (!condition.IsValid(caller, this))
                    {
                        if (condition.Action == ConditionFailedAction.Disable)
                        {
                            nothing = ConditionFailedAction.Disable;
                        }
                        else
                        {
                            return (nothing = ConditionFailedAction.Exclude);
                        }
                    }
                }
                catch
                {
                    LoggingService.Error("Exception while getting failed action from " + this.addIn.FileName);
                    throw;
                }
            }
            return nothing;
        }

        public ConditionFailedAction GetFailedAction(object caller, ICommand command)
        {
            ConditionFailedAction failedAction = this.GetFailedAction(caller);
            if ((((this.Conditions.Length == 0) && (command != null)) && (command is IMenuCommand)) && (failedAction != ConditionFailedAction.Exclude))
            {
                IMenuCommand command2 = command as IMenuCommand;
                if (!command2.Visible)
                {
                    return ConditionFailedAction.Exclude;
                }
                if (!((failedAction == ConditionFailedAction.Disable) || command2.IsEnabled))
                {
                    failedAction = ConditionFailedAction.Disable;
                }
            }
            return failedAction;
        }

        public override string ToString()
        {
            return string.Format("[Codon: name = {0}, addIn={1}]", this.name, this.addIn.FileName);
        }

        public SkyMap.Net.Core.AddIn AddIn
        {
            get
            {
                return this.addIn;
            }
        }

        public ICondition[] Conditions
        {
            get
            {
                return this.conditions;
            }
        }

        public string Id
        {
            get
            {
                return this.properties["id"];
            }
        }

        public string InsertAfter
        {
            get
            {
                if (!this.properties.Contains("insertafter"))
                {
                    return "";
                }
                return this.properties["insertafter"];
            }
            set
            {
                this.properties["insertafter"] = value;
            }
        }

        public string InsertBefore
        {
            get
            {
                if (!this.properties.Contains("insertbefore"))
                {
                    return "";
                }
                return this.properties["insertbefore"];
            }
            set
            {
                this.properties["insertbefore"] = value;
            }
        }

        public string this[string key]
        {
            get
            {
                return this.properties[key];
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public SkyMap.Net.Core.Properties Properties
        {
            get
            {
                return this.properties;
            }
        }
    }
}

