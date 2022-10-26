﻿namespace EorDSU.Models
{
    public class FileModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Year { get; set; }
        public Profile? Profile { get; set; }
        public int FileType { get; set; }
    }
}