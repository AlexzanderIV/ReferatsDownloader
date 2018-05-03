namespace ReferatsDownloader.Models
{
    public class Referat: DatabaseEntity
    {
        public string Topic { get; set; }
        public string Text { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
