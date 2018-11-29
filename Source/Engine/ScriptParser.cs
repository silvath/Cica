using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class ScriptParser
    {
        #region Properties

        #endregion
        #region Constructors
            public ScriptParser() 
            {
            }
        #endregion
        #region Parse
            public bool TryParse(string script, out Package package)
            {
                package = null;
                if (string.IsNullOrEmpty(script))
                    return (false);
                string[] args = script.Split(' ');
                if (args.Length == 0)
                    return (false);
                PackageType? type = GetParsePackageType(args[0]);
                if (!type.HasValue)
                    return (false);
                package = new Package();
                package.Type = type.Value;
                if (args.Length == 1)
                    return (true);
                PackageItem packageItem = null;
                for (int i = 1; i < args.Length; i++) 
                {
                    string arg = args[i];
                    PackageItemType? itemType = GetParsePackageItemType(arg);
                    if (itemType.HasValue)
                    {
                        packageItem = new PackageItem();
                        packageItem.Type = itemType.Value;
                        package.Items.Add(packageItem);
                    }else {
                        if (packageItem == null)
                            return (false);
                        packageItem.Data.Add(arg);
                    }
                }
                return (true);
            }

            private PackageType? GetParsePackageType(string typeText)
            {
                PackageType type;
                if (Enum.TryParse<PackageType>(typeText, true, out type))
                    return (type);
                return (null);
            }

            private PackageItemType? GetParsePackageItemType(string typeText) 
            {
                PackageItemType itemType;
                if (Enum.TryParse<PackageItemType>(typeText, true, out itemType))
                    return (itemType);
                return (null);
            }
        #endregion
    }
}
