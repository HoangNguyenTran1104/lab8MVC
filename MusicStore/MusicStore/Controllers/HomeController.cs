using MusicStore.EntityContext;
using MusicStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.ViewModels;

namespace MusicStore.Controllers
{
    public class HomeController : Controller
    {
        private String AlbumSessionKey = "AlbumId";
        MusicStoreEntities storeDB = new MusicStoreEntities();
        //
        // GET: /Home/
        //public ActionResult Index(string Searchname, IEnumerable<bool> showEnhancements)
        //{
        //    if (!string.IsNullOrEmpty(Searchname))
        //    {
        //        var albumlist = storeDB.Albums.Where(s => s.Title.Contains(Searchname)).ToList();
        //        return View(albumlist);
        //    }
        //    else if (showEnhancements != null && showEnhancements.Count() == 2)
        //    {
        //        // Get most popular albums
        //        var albums = GetTopSellingAlbums(5);

        //        return View(albums);
        //    }
        //    else
        //    {
        //        var list = storeDB.Albums.ToList();
        //        return View(list);
        //    }
        //}

        public ActionResult Index(string Searchname)
        {
            ViewBag.IsAtHome = true;
            // Get most popular albums
            if (!string.IsNullOrEmpty(Searchname))
            {
                HomeViewModel homeViewModel = new HomeViewModel();
                homeViewModel.ViewedAlbums = GetViewedAlbums(5);
                homeViewModel.RandomAlbums = GetSearchName(Searchname);
                homeViewModel.TopSellingAlbums = GetTopSellingAlbums(5);
                return View(homeViewModel);
            }
            else 
            {
                HomeViewModel homeViewModel = new HomeViewModel();
                homeViewModel.ViewedAlbums = GetViewedAlbums(5);
                homeViewModel.RandomAlbums = GetRandomAlbums(5);
                homeViewModel.TopSellingAlbums = GetTopSellingAlbums(5);
                return View(homeViewModel);
            }
        }

        public List<Album> GetViewedAlbums(int cnt)
        {
            var id = Session[AlbumSessionKey];

            var viewedAlbums = new List<Album>();
            if (id != null)
            {
                var indexString = id.ToString().Split(',');
                for(int i = indexString.Count() - 1; i >= 0; i--)
                {
                    var index = indexString.ElementAt(i);
                    if (cnt == 0) break;
                    viewedAlbums.Add(storeDB.Albums.Find(Convert.ToInt32(index)));
                    cnt--;
                }
            }
            return viewedAlbums;
        }
        public List<Album> GetTopSellingAlbums(int count)
        {
            // Group the order details by album and return
            // the albums with the highest count
            return storeDB.Albums
                .OrderByDescending(a => a.OrderDetails.Count())
                .Take(count)
                .ToList();
        }

        public List<Album> GetRandomAlbums(int count)
        {
            // Group the order details by album and return
            // the albums with the highest count
            return storeDB.Albums.Take(count).ToList();
        }

        public List<Album> GetSearchName(string name)
        {
            return storeDB.Albums.Where(a => a.Title.Contains(name)).ToList();
        }

        public PartialViewResult RightContent()
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.ViewedAlbums = GetViewedAlbums(5);
            homeViewModel.RandomAlbums = GetRandomAlbums(5);
            homeViewModel.TopSellingAlbums = GetTopSellingAlbums(5);
            return PartialView(homeViewModel);
        }

    }
}