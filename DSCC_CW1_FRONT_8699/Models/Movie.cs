using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSCC_CW1_FRONT_8699.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int IssueYear { get; set; }
        public string Genre { get; set; }
    }
}