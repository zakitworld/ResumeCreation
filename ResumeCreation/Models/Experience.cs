﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ResumeCreation.Models
{
    public class Experience
    {
        public Experience()
        {
        }

        [Key]
        public int ExperienceId { get; set; }

        [ForeignKey("Applicant")]//very important
        public int ApplicantId { get; set; }
        public virtual Applicant Applicant { get; private set; } //very important 

        public string CompanyName { get; set; }
        public string Designation { get; set; }

        [Range(1,25, ErrorMessage = "Years Must be between 1 and 25")]
        [Required]
        public int YearsWorked { get; set; }

        [NotMapped]
        public bool IsDeleted { get; set; } = false;
    }
}
