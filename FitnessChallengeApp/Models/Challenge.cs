using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessChallengeApp.Models
{
    public class Challenge
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Period { get; set; }
    public string Difficulty { get; set; }
    public string Instructions { get; set; }
    public ApplicationUser CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}
}
