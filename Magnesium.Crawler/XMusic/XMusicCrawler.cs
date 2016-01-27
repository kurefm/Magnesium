using Magnesium.DataBase.XMusic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magnesium.Crawler.Web;
using System.Text.RegularExpressions;

namespace Magnesium.Crawler.XMusic
{
    class XMusicCrawler
    {
        private const string URL = "http://www.xiami.com";

        //public static async Task<Album> GetAlbum(long id)
        //{
        //    var html = await Http.Get($"{URL}/album/{id}");

        //    //处理html得到album对象
        //    return new Album(1, "", 1);
        //}

        //public static async Task<Song> GetSong(long id)
        //{
        //    var html = await Http.Get($"{URL}/album/{id}");

        //    //处理html得到song对象

        //    return new Song(1, "", 1, 1);
        //}

        //public static async Task<Artist> GetArtist(long id)
        //{
        //    var html = await Http.Get($"{URL}/album/{id}");

        //    //处理html得到artist对象

        //    return new Artist(1, "");
        //}
    }
}
