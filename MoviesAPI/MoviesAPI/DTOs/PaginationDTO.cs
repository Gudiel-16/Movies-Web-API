namespace MoviesAPI.DTOs
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;
        public int numberOfRecordsPerPage { get; set; } = 10;
        public int maximumNumberOfRecordsPerPage { get; set; } = 50;
        public int NumberOfRecordsPerPage // es de cuantos valores o datos quiero mi paginacion
        {
            get => numberOfRecordsPerPage;
            set
            {
                // value es el valor que se envia, si este es > 50, entonces retorna 50 (maximumNumberOfRecordsPerPage)
                // si este es <= 50 entonces, por ejemplo 15, entonces este es asignado a numberOfRecordsPerPage
                numberOfRecordsPerPage = (value > maximumNumberOfRecordsPerPage) ? numberOfRecordsPerPage : value;
            }
        }
    }
}
