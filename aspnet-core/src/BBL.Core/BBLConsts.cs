using BBL.Debugging;

namespace BBL
{
    public class BBLConsts
    {
        public const string LocalizationSourceName = "BBL";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "bc6bc9872bb7490eb3eff78e6cd290ef";
    }
}
