﻿using System;
using System.Linq;
using ContosoMoments.Common;
using ContosoMoments.Common.Models;
using ContosoMoments.Common.Storage;
using ContosoMoments.MobileServer.Models;

namespace ContosoMoments.MobileServer.DataLogic
{
    public class ImageBusinessLogic
    {

      

        public void AddImageToDB(string AlbumId, string UserId, string containerName, string fileName, bool IsMobile)
        {

            var uploadFormat = IsMobile ? "Mobile Image" : "Web Image";
            AddImageToDB(AlbumId, UserId, containerName,  fileName, uploadFormat);
        }

        public void AddImageToDB(string AlbumId, string UserId, string containerName, string fileName, string UploadFormat)
        {
            ContosoStorage cs = new ContosoStorage();
            var ctx = new MobileServiceContext();
            var img = new Image
            {
                Album = ctx.Albums.Where(x => x.Id == AlbumId).FirstOrDefault(),
                User = ctx.Users.Where(x => x.Id == UserId).FirstOrDefault(),
                Id = Guid.NewGuid().ToString(),
                UploadFormat = UploadFormat,
                ContainerName = AppSettings.StorageWebUri + containerName,
                FileName = fileName,
                
              
            };
            ctx.Images.Add(img);
            try
            {
                ctx.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }
    }
}