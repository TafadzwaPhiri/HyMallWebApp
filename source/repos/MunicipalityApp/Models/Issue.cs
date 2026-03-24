using System;
using System.Collections.Generic;

namespace MunicipalityApp.Models
{
    public class Issue
    {
        public int Id { get; set; }
        public string Location { get; set; } = "";
        public string Category { get; set; } = "";
        public string Description { get; set; } = "";
        public List<string> Attachments { get; set; } = new List<string>();
        public string Status { get; set; } = "Submitted";
        public DateTime CreatedAt { get; set; }

        
        public string AssignedTo { get; set; }
    }
}
