using System;    
using System.Collections.Generic;    
using System.ComponentModel.DataAnnotations;    
using System.Linq;    
using System.Threading.Tasks; 

namespace UsersAPI.Models    
{    
    public class Rating    
    {  
        public int RatingId { get; set; }
        public int MusicId { get; set; }  
        public int PlaylistId { get; set; }  
        public int ArtistId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Type { get; set; }    
        [Required]    
        public int Score { get; set; }
        public string Comment { get; set; }
    }
}