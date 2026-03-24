using System.Collections.Generic;


namespace MunicipalityApp.Models
{
    public static class IssueCategory
    {
        public static List<string> DefaultCategories()
        {
            return new List<string>
            {
                "Sanitation",
                "Roads & Potholes",
                "Electricity / Utilities",
                "Water Supply",
                "Street Lighting",
                "Public Safety",
                "Other"
            };
        }
    }
}
