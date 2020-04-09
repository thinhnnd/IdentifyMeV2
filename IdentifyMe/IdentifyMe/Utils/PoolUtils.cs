
namespace IdentifyMe.Utils
{
    public static class PoolUtils
    {
        public static string LabelForNetwork(this string network)
        {
            switch (network)
            {
                case "sovrin-staging": return "Sovrin Staging Network";
                case "sovrin-live": return "Sovrin Network";
                case "sovrin-builder": return "Sovrin Builder Network";
                case "bcovrin-test": return "Bcovrin Test Network";
            }
            return "Streetcred Development Network";
        }
    }
}