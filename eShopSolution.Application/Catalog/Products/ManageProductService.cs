﻿using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using eShopSolution.ViewModel.Catalog.Products.Manage;
using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Catalog.Products;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using eShopSolution.Application.Common;

namespace eShopSolution.Application.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        private readonly EShopDataContext _context;
        private readonly IStorageService _storageService;
        public ManageProductService(EShopDataContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public Task<int> AddImage(int productId, List<IFormFile> files)
        {
            throw new NotImplementedException();
        }

        public async Task AddViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            Product product = new Product
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation
                    {
                        Name = request.Name,
                        Description = request.Description,
                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                        SeoTitle = request.SeoTitle,
                        LanguageId = request.LanguageId
                    }
                }
            };

            //save image
            if(request.ThumbnailImgae != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                   new ProductImage
                   {
                       Caption = "ThumbnailImage",
                       DateCreated = DateTime.Now,
                       FileSize = request.ThumbnailImgae.Length,
                       ImagePath = await SaveFile(request.ThumbnailImgae),
                       IsDefault = true,
                       SortOrder = 1
                   }
                };
            }
            _context.Products.Add(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product: {productId}");
            var thumbnailImages = await _context.ProductImages.Where(x => x.ProductId == productId).ToListAsync();
            if(thumbnailImages.Count() > 0)
            {
                foreach (var item in thumbnailImages)
                {
                    await _storageService.DeleteFileAsync(item.ImagePath);
                }
            }
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<PageResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request)
        {
            //1.select Join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        select new { p, pt, pic };

            //2.filter
            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.pt.Name.Contains(request.Keyword.Trim()));

            if(request.CategoryIds.Count > 0)
            {
                query = query.Where(x => request.CategoryIds.Contains(x.pic.CategoryId));
            }
            //3.Paging
            int totalRow = await query.CountAsync();

            int pageNumer = (totalRow % request.PageSize) > 0 ? (totalRow / request.PageSize) + 1 : (totalRow / request.PageSize);

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel() { 
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                }).ToListAsync();

            //3. Select and projection
            var pagedResult = new PageResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data,
                PageNumber = pageNumer
            };

            return pagedResult;
        }

        public Task<List<ProductImageViewModel>> GetListImage(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<int> RemoveImage(int imageId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = _context.Products.Find(request.Id);
            var productTranslations = await _context.ProductTranslations
                .FirstOrDefaultAsync(x => x.ProductId == request.Id && x.LanguageId == request.LanguageId);
            if (product == null || productTranslations == null) throw new EShopException($"Cannot find a product with id: {request.Id}");
            productTranslations.Name = request.Name;
            productTranslations.SeoAlias = request.SeoAlias;
            productTranslations.SeoTitle = request.SeoTitle;
            productTranslations.SeoDescription = request.SeoDescription;
            productTranslations.Details = request.Details;
            productTranslations.Description = request.Description;

            //save image
            if (request.ThumbnailImgae != null)
            {
                var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(x =>x.ProductId == request.Id && x.IsDefault == true);
                if(thumbnailImage != null)
                {
                    await _storageService.DeleteFileAsync(thumbnailImage.ImagePath);
                    thumbnailImage.FileSize = request.ThumbnailImgae.Length;
                    thumbnailImage.ImagePath = await SaveFile(request.ThumbnailImgae);
                    _context.ProductImages.Update(thumbnailImage);
                } 
                else
                {
                    product.ProductImages = new List<ProductImage>()
                    {
                       new ProductImage
                       {
                           Caption = "ThumbnailImage",
                           DateCreated = DateTime.Now,
                           FileSize = request.ThumbnailImgae.Length,
                           ImagePath = await SaveFile(request.ThumbnailImgae),
                           IsDefault = true,
                           SortOrder = 1
                       }
                    };
                }
                
            }

            return await _context.SaveChangesAsync();
        }

        public Task<int> UpdateImage(int imageId, string caption, bool isDefault)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product with id: {productId}");
            product.Price += newPrice;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product with id: {productId}");
            product.Stock += addedQuantity;
            return await _context.SaveChangesAsync() > 0;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
    }
}
