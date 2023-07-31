using Business_Logic.Modules.ImageModule.Request;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.ImageModule.Interface
{
    public interface IImageService
    {
        public Task<Image> AddNewImage(CreateImageRequest ImageCreate);
        public Task<Image> UpdateImage(UpdateImageRequest ImageUpdate);
        public Task<ICollection<Image>> GetAll();
        public Task<ICollection<Image>> GetImageByID(Guid id);
        public Task<ICollection<Image>> GetImageByItem(Guid id);

    }
}
