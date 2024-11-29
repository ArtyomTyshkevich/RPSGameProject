namespace Auth.BLL.Exceptions
{
    public class BookAlreadyRentedException : Exception
    {
        public BookAlreadyRentedException(Guid bookId)
            : base($"Book with ID {bookId} is already rented.") { }
    }
}
