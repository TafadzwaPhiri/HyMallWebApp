using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MunicipalityApp.Models
{
    public class ReportIssueViewModel
    {
        [Required(ErrorMessage = "Please provide a location.")]
        public string Location { get; set; } = "";

        [Required(ErrorMessage = "Please select a category.")]
        public string SelectedCategory { get; set; } = "";

        [Required(ErrorMessage = "Please describe the issue.")]
        public string Description { get; set; } = "";

        // attachments (images/documents)
        public List<IFormFile>? Attachments { get; set; }

        // for rendering dropdown
        public List<string> Categories { get; set; } = new List<string>();
    }
}
