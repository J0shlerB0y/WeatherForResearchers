namespace Domain
{
    public class FilterCityParametr
    {
        public string? City { get; set; }
        public string? Country { get; set; }

        public virtual void ClearBySpaces()
        {
            if (City != null)
            {
                City.Trim();
            }
            if (Country != null)
            {
                Country.Trim();
            }
        }
    }
}
