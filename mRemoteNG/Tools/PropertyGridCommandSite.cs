using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.Tools
{
    public class PropertyGridCommandSite : IMenuCommandService, ISite
    {
        private readonly object TheObject;

        public PropertyGridCommandSite(object @object)
        {
            TheObject = @object;
        }

        public DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerbCollection objectVerbs = new();
                // ReSharper disable VBPossibleMistakenCallToGetType.2
                MethodInfo[] methods = TheObject.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
                // ReSharper restore VBPossibleMistakenCallToGetType.2
                foreach (MethodInfo method in methods)
                {
                    object[] commandAttributes = method.GetCustomAttributes(typeof(CommandAttribute), true);
                    if (commandAttributes.Length == 0)
                    {
                        continue;
                    }

                    CommandAttribute commandAttribute = (CommandAttribute)commandAttributes[0];
                    if (!commandAttribute.Command)
                    {
                        continue;
                    }

                    string displayName = method.Name;
                    object[] displayNameAttributes = method.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                    if (displayNameAttributes.Length != 0)
                    {
                        DisplayNameAttribute displayNameAttribute = (DisplayNameAttribute)displayNameAttributes[0];
                        if (!string.IsNullOrEmpty(displayNameAttribute.DisplayName))
                        {
                            displayName = displayNameAttribute.DisplayName;
                        }
                    }

                    objectVerbs.Add(new DesignerVerb(displayName, new EventHandler(VerbEventHandler)));
                }

                return objectVerbs;
            }
        }

        private void VerbEventHandler(object sender, EventArgs e)
        {
            DesignerVerb verb = sender as DesignerVerb;
            if (verb == null)
            {
                return;
            }

            // ReSharper disable VBPossibleMistakenCallToGetType.2
            MethodInfo[] methods = TheObject.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
            // ReSharper restore VBPossibleMistakenCallToGetType.2
            foreach (MethodInfo method in methods)
            {
                object[] commandAttributes = method.GetCustomAttributes(typeof(CommandAttribute), true);
                if (commandAttributes.Length == 0)
                {
                    continue;
                }

                CommandAttribute commandAttribute = (CommandAttribute)commandAttributes[0];
                if (!commandAttribute.Command)
                {
                    continue;
                }

                string displayName = method.Name;
                object[] displayNameAttributes = method.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                if (displayNameAttributes.Length != 0)
                {
                    DisplayNameAttribute displayNameAttribute = (DisplayNameAttribute)displayNameAttributes[0];
                    if (!string.IsNullOrEmpty(displayNameAttribute.DisplayName))
                    {
                        displayName = displayNameAttribute.DisplayName;
                    }
                }

                if (verb.Text != displayName) continue;
                method.Invoke(TheObject, null);
                return;
            }
        }

        public object GetService(Type serviceType)
        {
            return serviceType == typeof(IMenuCommandService) ? this : null;
        }

        public IComponent Component => throw new NotSupportedException();

        public IContainer Container => null;

        public bool DesignMode => true;

        public string Name
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public void AddCommand(MenuCommand command)
        {
            throw new NotSupportedException();
        }

        public void AddVerb(DesignerVerb verb)
        {
            throw new NotSupportedException();
        }

        public MenuCommand FindCommand(CommandID commandId)
        {
            throw new NotSupportedException();
        }

        public bool GlobalInvoke(CommandID commandId)
        {
            throw new NotSupportedException();
        }

        public void RemoveCommand(MenuCommand command)
        {
            throw new NotSupportedException();
        }

        public void RemoveVerb(DesignerVerb verb)
        {
            throw new NotSupportedException();
        }

        public void ShowContextMenu(CommandID menuId, int x, int y)
        {
            throw new NotSupportedException();
        }
    }

    public class CommandAttribute : Attribute
    {
        public bool Command { get; set; }

        public CommandAttribute(bool isCommand = true)
        {
            Command = isCommand;
        }
    }
}