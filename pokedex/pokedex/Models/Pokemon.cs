namespace pokedex.Models
{
    /// <summary>
    /// The object template that hold the data of a single pokemon.
    /// This class is used when converting JSON objects to C# objects.
    /// </summary>
    class Pokemon
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public string Length { get; set; }
        public string Weight { get; set; }
        public string[] Abilities { get; set; }
        public string[] Typing { get; set; }
    }
}