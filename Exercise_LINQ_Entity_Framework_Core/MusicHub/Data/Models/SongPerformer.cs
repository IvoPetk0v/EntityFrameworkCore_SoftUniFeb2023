
namespace MusicHub.Data.Models
{

    public class SongPerformer
    {
        public int SongId { get; set; }

        public int PerformerId { get; set; }

        public Song Song { get; set; } = null!;

        public Performer Performer { get; set; } = null!;
    }
}
