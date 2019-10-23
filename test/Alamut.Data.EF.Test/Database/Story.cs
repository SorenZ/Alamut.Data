using System.ComponentModel.DataAnnotations;

namespace Alamut.Data.EF.Test.Database
{
    public class Story
    {
        public int Id { get; set; } 
        
        public int Key { get; set; }

        public string Title { get;set; }
    }
}