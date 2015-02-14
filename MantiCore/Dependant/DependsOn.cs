using System;
using System.Diagnostics;
using MantiCore.Bundle;

namespace MantiCore.Dependant
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DependsOn : Attribute, IComparable<DependsOn>, IComparable<string>
    {
        public string BundleIdentifier { get; set; }
        public string[] Dependancies { get; private set; }

        public DependsOn(params string[] dependancies)
        {
            Dependancies = dependancies;
        }

        public static DependsOn GetDependancies(Bundle.Bundle bundle)
        {
            return GetDependancies(bundle.GetType());
        }

        public static DependsOn GetDependancies(Type type)
        {
            string bundleId;
            try
            {
                var bundleInfo = (BundleInformation)GetCustomAttribute(type, typeof(BundleInformation));
                bundleId = bundleInfo.BundleIdentifier;
            }
            catch (Exception e)
            {
                Debug.WriteLine("DependsOn: " + e.Message);
                throw new DependancyException("DependsOn attribute cannot be applied when not a Bundle.");
            }

            try
            {
                var attribute = (DependsOn)GetCustomAttribute(type, typeof(DependsOn));
                attribute.BundleIdentifier = bundleId;
                if (attribute.Equals(null)) throw new Exception("Type has no dependancies.");
                return attribute;
            }
            catch (Exception e)
            {
                Debug.WriteLine("DependsOn: " + e.Message);
                return new DependsOn();
            }
        }

        public int CompareTo(DependsOn other)
        {
            return CompareTo(other.BundleIdentifier);
        }

        public int CompareTo(string other)
        {
            return string.CompareOrdinal(BundleIdentifier, other);
        }

        public override string ToString()
        {
            return BundleIdentifier + ": {" + string.Join(", ", Dependancies) + "}";
        }
    }
}
