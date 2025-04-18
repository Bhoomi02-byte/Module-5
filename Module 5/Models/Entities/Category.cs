﻿using System.ComponentModel.DataAnnotations;

namespace Module_5.Models.Entities
{


    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }


        public int? AuthorId { get; set; }
        public User? Author { get; set; }
       

        public ICollection<Post>? Posts { get; set; }
       
    }
}

