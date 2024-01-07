using ShareData.Debugging;

namespace ShareData
{
    public class ShareDataConsts
    {
        public const string LocalizationSourceName = "ShareData";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = false;

        public struct FormStageAttachmentPath
        {
            public const string UploadPath = "/wwwroot/uploads/formStages/";
            public const string FolderPath = "//uploads//formStages//";
        }
        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "05c7337fc12042889bb45a4e24684e2c";
    }
}
