using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.DataBase.XMusic
{
    public class Song : XModleBase
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public long XArtistId { get; set; }
        public long XAlbumId { get; set; }
        public int Track { get; set; } = 1;
        public string Lyricist { get; set; }
        public string Composer { get; set; }
        public string Arranger { get; set; }
        public int DiscNumber { get; set; } = 1;

        public Song(int xId, string title, int xArtistId, int xAlbumId) : base(xId)
        {
            Title = title;
            XArtistId = xArtistId;
            XAlbumId = xAlbumId;
        }
    }
}
