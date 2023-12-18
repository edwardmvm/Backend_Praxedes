namespace WebApi.Models
{
    public class FamilyGroup
    {
        public int MemberID { get; set; }
        public int UserID { get; set; }
        public string? Cedula { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Genero { get; set; }
        public string? Parentesco { get; set; }
        // Calcula la edad en base a la fecha de nacimiento
        public int Edad
        {
            get
            {
                if (FechaNacimiento.HasValue)
                {
                    var today = DateTime.Today;
                    var age = today.Year - FechaNacimiento.Value.Year;
                    if (FechaNacimiento.Value.Date > today.AddYears(-age)) age--;
                    return age;
                }
                return 0;
            }
        }
        public bool MenorEdad { get; set; }
        public DateTime? FechaNacimiento { get; set; }

    }
}
