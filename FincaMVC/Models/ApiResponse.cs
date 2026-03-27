namespace FincaMVC.Models
{
    public class ApiResponse
    {
     
            public int total { get; set; }
            public int page { get; set; }
            public int pageSize { get; set; }
            public List<Empleado> data { get; set; }
        
    }
}
