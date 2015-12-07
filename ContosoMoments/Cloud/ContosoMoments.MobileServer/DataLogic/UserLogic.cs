﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContosoMoments.Common.Models;
using ContosoMoments.MobileServer.Models;

namespace ContosoMoments.MobileServer.DataLogic
{
    public class InitLogic
    {
        public void initializeFirstLogin(string email)
        {
            var ctx = new MobileServiceContext();
            var user = ctx.Users.Where(x => x.Email.ToLower() == email.ToLower()).FirstOrDefault();
            if (user == null)
            {
                var userid = Guid.NewGuid().ToString();
                var defaultAlbum = new Album
                {
                    Id = Guid.NewGuid().ToString(),
                    IsDefault = true,
                    AlbumName = "Default Album",
                    User = new User
                    {
                        Email = email.ToLower(),
                        Id = userid,
                        IsEnabled = true
                    },
                    UserId= userid
                };
                ctx.Albums.Add(defaultAlbum);
                ctx.SaveChanges();
            }


        }
    }
}
