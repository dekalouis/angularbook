namespace BookApi.Domain.Entities

{
    //the book entity 
    public class Book
    {
        public int Id { get; set; } // primary key
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;

        //relationships foreign key to the user table
        public int UserId { get; set; }
        //navigation property to the user table (req to enfore ownership)
        public User? User { get; set; }

    }
}