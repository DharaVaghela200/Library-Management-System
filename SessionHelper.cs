// ============================================
// File: SessionHelper.cs
// ============================================
namespace LibraryManagementSystem
{
    public static class SessionHelper
    {
        public static int UserID { get; set; }
        public static string Username { get; set; }
        public static string Role { get; set; }
        public static int StudentID { get; set; }  // Set if Role == Student

        public static bool IsAdmin => Role == "Admin";
        public static bool IsStudent => Role == "Student";

        public static void Clear()
        {
            UserID = 0;
            Username = null;
            Role = null;
            StudentID = 0;
        }
    }
}
