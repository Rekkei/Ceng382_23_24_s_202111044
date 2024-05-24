using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessChallengeApp.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int ChallengeId { get; set; }
        public Challenge Challenge { get; set; }
        public string Text { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
