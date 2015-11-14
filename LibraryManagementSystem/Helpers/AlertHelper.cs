using System.Web.Mvc;

namespace LibraryManagementSystem.Helpers
{
    public static class AlertExtensions
    {
        public static string Alert(this HtmlHelper helper, string text, string type)
        {
            return $"<div class=\"alert alert-{type}\">{text}</div>";
        }
    }
}