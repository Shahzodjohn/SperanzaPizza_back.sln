using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SperanzaPizzaApi.Data;
using SperanzaPizzaApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SperanzaPizzaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParsingExcel2 : ControllerBase
    {
        private readonly dbPizzaContext _context;

        public ParsingExcel2(dbPizzaContext context)
        {
            _context = context;
        }
        [HttpPost("ExcelParser")]
        public async Task ExcelParser(IFormFile file)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    int WorkSheetCount = 1;
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[WorkSheetCount];
                    var rowcount = worksheet.Dimension.Rows;
                    #region AddingCategories
                    //for (int i = 1; ; i++)
                    //{
                    //    var CategoryWorkSheet = package.Workbook.Worksheets[i];
                    //    var CategoryName = CategoryWorkSheet.Name.ToUpper();
                    //    _context.DmProductCategories.Add(new Models.DmProductCategory
                    //    {
                    //        CategoryName = CategoryName
                    //    });
                    //    await _context.SaveChangesAsync();
                    //    if (worksheet.Name == "ARKUSZ15")
                    //        break;
                    //}
                    #endregion
                    #region
                    //for (int i = 3; ; i++)
                    //{
                    //    var size = worksheet.Cells[1, i].Value.ToString();
                    //    _context.DmProductSizes.Add(new Models.DmProductSize
                    //    {
                    //        SizeName = size,

                    //    });
                    //    await _context.SaveChangesAsync();
                    //}
                    #endregion
                    bool IsActive = true;
                    for (int row = 2; IsActive; row++)
                    {
                        var ProductCategory = worksheet.Name;
                        if (worksheet.Name == "Pizza tradycyjna")
                        {
                            var StringProductName = worksheet.Cells[row, 1].Value.ToString();
                            string CountingNumber;
                            string ProductName;
                            CountingNumber = StringProductName.Substring(0, StringProductName.IndexOf('.', StringProductName.IndexOf('.', StringProductName.LastIndexOf('.') + 1) + 1));
                            ProductName = StringProductName.Replace(CountingNumber, "").Substring(1).Trim();
                            var ProductFormulation = worksheet.Cells[row, 2].Value.ToString();
                            var CategoryName = worksheet.Name.ToUpper();
                            var PriceMala = worksheet.Cells[row, 3].Value.ToString().Trim().Contains("zł") ? worksheet.Cells[row, 3].Value.ToString().Trim().Replace("zł", "") : worksheet.Cells[row, 3].Value.ToString().Trim().Replace("z", "").Trim().Replace("ł", "").Trim();
                            var PriceSrednia = worksheet.Cells[row, 4].Value.ToString().Trim().Contains("zł") ? worksheet.Cells[row, 4].Value.ToString().Trim().Replace("zł", "") : worksheet.Cells[row, 4].Value.ToString().Trim().Replace("z", "").Trim().Replace("ł", "").Trim();
                            var PriceDuza = worksheet.Cells[row, 5].Value.ToString().Trim().Contains("zł") ? worksheet.Cells[row, 5].Value.ToString().Trim().Replace("zł", "") : worksheet.Cells[row, 5].Value.ToString().Trim().Replace("z", "").Trim().Replace("ł", "").Trim();
                            var ProductFind = await _context.DmProducts.FirstOrDefaultAsync(x => x.ProductName == ProductName.ToUpper());
                            var FindCategory = await _context.DmProductCategories.FirstOrDefaultAsync(x => x.CategoryName == CategoryName);

                            DmProductSize FindSizeMała;
                            FindSizeMała = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Mała" && x.ProductCategoryId == FindCategory.Id);
                            if (FindSizeMała == null)
                            {
                                _context.DmProductSizes.Add(new Models.DmProductSize
                                {
                                    SizeName = "Mała",
                                    ProductCategoryId = FindCategory.Id
                                });
                                await _context.SaveChangesAsync();
                                FindSizeMała = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Mała" && x.ProductCategoryId == FindCategory.Id);
                            }

                            DmProductSize FindSizeŚrednia;
                            FindSizeŚrednia = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Średnia");
                            if (FindSizeŚrednia == null)
                            {
                                _context.DmProductSizes.Add(new Models.DmProductSize
                                {
                                    SizeName = "Średnia",
                                    ProductCategoryId = FindCategory.Id
                                });
                                await _context.SaveChangesAsync();
                                FindSizeŚrednia = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Średnia" && x.ProductCategoryId == FindCategory.Id);
                            }

                            DmProductSize FindSizeDuża;
                            FindSizeDuża = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Duża");
                            if (FindSizeDuża == null)
                            {
                                _context.DmProductSizes.Add(new Models.DmProductSize
                                {
                                    SizeName = "Duża",
                                    ProductCategoryId = FindCategory.Id
                                });
                                await _context.SaveChangesAsync();
                                FindSizeDuża = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Duża" && x.ProductCategoryId == FindCategory.Id);
                            }

                            if (ProductFind == null)
                            {
                                _context.DmProducts.Add(new Models.DmProduct
                                {
                                    ProductName = ProductName.ToUpper(),
                                    Formulation = ProductFormulation,
                                    CategoryId = FindCategory.Id,
                                    IsActive = true,
                                });
                                await _context.SaveChangesAsync();
                            }

                            ProductFind = await _context.DmProducts.FirstOrDefaultAsync(x => x.ProductName == ProductName);
                            /////////////////////////////////////////////////////////
                            var FindProductPriceMala = await _context.DmProductPrices.FirstOrDefaultAsync(x =>
                            x.ProductId == ProductFind.Id && x.SizeId == FindSizeMała.Id);

                            if (FindProductPriceMala == null)
                            {
                                double PriceMalaDouble;
                                Double.TryParse(PriceMala, out PriceMalaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    SizeId = FindSizeMała.Id,
                                    PriceValue = (decimal)PriceMalaDouble,
                                    CreatedDate = DateTime.Now
                                });
                            }
                            else
                            {
                                double Price;
                                Double.TryParse(PriceMala, out Price);
                                FindProductPriceMala.PriceValue = (decimal)Price;
                            }

                            //////////////////////////////////////////////////////////
                            var FindProductPriceŚrednia = await _context.DmProductPrices.FirstOrDefaultAsync(x =>
                            x.ProductId == ProductFind.Id && x.SizeId == FindSizeŚrednia.Id);
                            if (FindProductPriceŚrednia == null)
                            {
                                double PriceSredniaDouble;
                                Double.TryParse(PriceSrednia, out PriceSredniaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    SizeId = FindSizeŚrednia.Id,
                                    PriceValue = (decimal)PriceSredniaDouble,
                                    CreatedDate = DateTime.Now
                                });
                            }
                            else
                            {
                                double Price;
                                Double.TryParse(PriceSrednia, out Price);
                                FindProductPriceŚrednia.PriceValue = (decimal)Price;
                            }
                            //////////////////////////////////////////////////////////
                            var FindProductPriceDuża = await _context.DmProductPrices.FirstOrDefaultAsync(x =>
                            x.ProductId == ProductFind.Id && x.SizeId == FindSizeDuża.Id);
                            if (FindProductPriceDuża == null)
                            {
                                double PriceDużaDouble;
                                Double.TryParse(PriceDuza, out PriceDużaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    SizeId = FindSizeDuża.Id,
                                    PriceValue = (decimal)PriceDużaDouble,
                                    CreatedDate = DateTime.Now
                                });
                            }
                            else
                            {
                                double Price;
                                Double.TryParse(PriceDuza, out Price);
                                FindProductPriceDuża.PriceValue = (decimal)Price;
                            }
                            await _context.SaveChangesAsync();
                            if (row == 41)
                            {
                                WorkSheetCount++;
                                IsActive = false;
                                string[] XlsxFiles = Directory.GetFiles(@"C:\Users\shahz\Videos", "*.xlsx");

                                for (int fileLength = 0; fileLength < XlsxFiles.Length; fileLength++)
                                {
                                    string filePath = XlsxFiles[fileLength];
                                    byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                                    MemoryStream ms = new MemoryStream(bytes);
                                    await PizzaZKurczakiem(ms);
                                }
                                //"C:\Users\shahz\Videos\Alexandra.xlsx"
                            }
                        }
                    }

                }
            }
        }
        private async Task PizzaZKurczakiem(Stream file)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[2];
                    bool IsActive = true;
                    worksheet = package.Workbook.Worksheets[2];
                    for (int row = 2; IsActive; row++)
                    {
                        var ProductCategory = worksheet.Name;
                        if (worksheet.Name == "Pizza z kurczakiem")
                        {
                            var StringProductName = worksheet.Cells[row, 1].Value.ToString();
                            string CountingNumber;
                            string ProductName;
                            CountingNumber = StringProductName.Substring(0, StringProductName.IndexOf('.', StringProductName.IndexOf('.', StringProductName.LastIndexOf('.') + 1) + 1));
                            ProductName = StringProductName.Replace(CountingNumber, "").Substring(1).Trim();
                            var ProductFormulation = worksheet.Cells[row, 2].Value.ToString();
                            var CategoryName = worksheet.Name.ToUpper();
                            var PriceMala = worksheet.Cells[row, 3].Value.ToString();
                            var PriceSrednia = worksheet.Cells[row, 4].Value.ToString();
                            var PriceDuza = worksheet.Cells[row, 5].Value.ToString();
                            var ProductFind = await _context.DmProducts.FirstOrDefaultAsync(x => x.ProductName == ProductName.ToUpper());
                            var FindCategory = await _context.DmProductCategories.FirstOrDefaultAsync(x => x.CategoryName == CategoryName);
                            DmProductSize FindSizeMała;
                            FindSizeMała = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Mała" && x.ProductCategoryId == FindCategory.Id);
                            if (FindSizeMała == null)
                            {
                                _context.DmProductSizes.Add(new Models.DmProductSize
                                {
                                    SizeName = "Mała",
                                    ProductCategoryId = FindCategory.Id
                                });
                                await _context.SaveChangesAsync();
                                FindSizeMała = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Mała" && x.ProductCategoryId == FindCategory.Id);
                            }

                            DmProductSize FindSizeŚrednia;
                            FindSizeŚrednia = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Średnia");
                            if (FindSizeŚrednia == null)
                            {
                                _context.DmProductSizes.Add(new Models.DmProductSize
                                {
                                    SizeName = "Średnia",
                                    ProductCategoryId = FindCategory.Id
                                });
                                await _context.SaveChangesAsync();
                                FindSizeŚrednia = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Średnia" && x.ProductCategoryId == FindCategory.Id);
                            }

                            DmProductSize FindSizeDuża;
                            FindSizeDuża = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Duża");
                            if (FindSizeDuża == null)
                            {
                                _context.DmProductSizes.Add(new Models.DmProductSize
                                {
                                    SizeName = "Duża",
                                    ProductCategoryId = FindCategory.Id
                                });
                                await _context.SaveChangesAsync();
                                FindSizeDuża = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Duża" && x.ProductCategoryId == FindCategory.Id);
                            }
                            //var FindCategory = await _context.DmProductCategories.FirstOrDefaultAsync(x => x.CategoryName == CategoryName);
                            if (ProductFind == null)
                            {
                                _context.DmProducts.Add(new Models.DmProduct
                                {
                                    ProductName = ProductName.ToUpper(),
                                    Formulation = ProductFormulation,
                                    CategoryId = FindCategory.Id,
                                    IsActive = true,
                                });
                                await _context.SaveChangesAsync();
                            }

                            ProductFind = await _context.DmProducts.FirstOrDefaultAsync(x => x.ProductName == ProductName);
                            /////////////////////////////////////////////////////////
                            var FindProductPriceMala = await _context.DmProductPrices.FirstOrDefaultAsync(x =>
                            x.ProductId == ProductFind.Id && x.SizeId == FindSizeMała.Id);

                            if (FindProductPriceMala == null)
                            {
                                double PriceMalaDouble;
                                Double.TryParse(PriceMala, out PriceMalaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    SizeId = FindSizeMała.Id,
                                    PriceValue = (decimal)PriceMalaDouble,
                                    CreatedDate = DateTime.Now
                                });
                            }
                            else
                            {
                                double Price;
                                Double.TryParse(PriceMala, out Price);
                                FindProductPriceMala.PriceValue = (decimal)Price;
                            }

                            //////////////////////////////////////////////////////////
                            var FindProductPriceŚrednia = await _context.DmProductPrices.FirstOrDefaultAsync(x =>
                            x.ProductId == ProductFind.Id && x.SizeId == FindSizeŚrednia.Id);
                            if (FindProductPriceŚrednia == null)
                            {
                                double PriceSredniaDouble;
                                Double.TryParse(PriceSrednia, out PriceSredniaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    SizeId = FindSizeŚrednia.Id,
                                    PriceValue = (decimal)PriceSredniaDouble,
                                    CreatedDate = DateTime.Now
                                });
                            }
                            else
                            {
                                double Price;
                                Double.TryParse(PriceSrednia, out Price);
                                FindProductPriceŚrednia.PriceValue = (decimal)Price;
                            }
                            //////////////////////////////////////////////////////////
                            var FindProductPriceDuża = await _context.DmProductPrices.FirstOrDefaultAsync(x =>
                            x.ProductId == ProductFind.Id && x.SizeId == FindSizeDuża.Id);
                            if (FindProductPriceDuża == null)
                            {
                                double PriceDużaDouble;
                                Double.TryParse(PriceDuza, out PriceDużaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    SizeId = FindSizeDuża.Id,
                                    PriceValue = (decimal)PriceDużaDouble,
                                    CreatedDate = DateTime.Now
                                });
                            }
                            else
                            {
                                double Price;
                                Double.TryParse(PriceDuza, out Price);
                                FindProductPriceDuża.PriceValue = (decimal)Price;
                            }
                            await _context.SaveChangesAsync();
                            if (row == 9)
                            {
                                string[] XlsxFiles = Directory.GetFiles(@"C:\Users\shahz\Videos", "*.xlsx");

                                for (int fileLength = 0; fileLength < XlsxFiles.Length; fileLength++)
                                {
                                    string filePath = XlsxFiles[fileLength];
                                    byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                                    MemoryStream ms = new MemoryStream(bytes);
                                    await PizzaZowocamiMorza(ms);

                                }

                                IsActive = false;
                            }
                        }
                    }
                }
            }

        }
        private async Task PizzaZowocamiMorza(Stream file)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[3];
                    bool IsActive = true;
                    for (int row = 2; IsActive; row++)
                    {
                        var ProductCategory = worksheet.Name;
                        if (worksheet.Name == "Pizza z owocami morza")
                        {
                            var StringProductName = worksheet.Cells[row, 1].Value.ToString();
                            string CountingNumber;
                            string ProductName;
                            CountingNumber = StringProductName.Substring(0, StringProductName.IndexOf('.', StringProductName.IndexOf('.', StringProductName.LastIndexOf('.') + 1) + 1));
                            ProductName = StringProductName.Replace(CountingNumber, "").Substring(1).Trim();
                            var ProductFormulation = worksheet.Cells[row, 2].Value.ToString();
                            var CategoryName = worksheet.Name.ToUpper();
                            var PriceMala = worksheet.Cells[row, 3].Value.ToString();
                            var PriceSrednia = worksheet.Cells[row, 4].Value.ToString();
                            var PriceDuza = worksheet.Cells[row, 5].Value.ToString();
                            var ProductFind = await _context.DmProducts.FirstOrDefaultAsync(x => x.ProductName == ProductName.ToUpper());
                            var FindCategory = await _context.DmProductCategories.FirstOrDefaultAsync(x => x.CategoryName == CategoryName);
                            DmProductSize FindSizeMała;
                            FindSizeMała = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Mała" && x.ProductCategoryId == FindCategory.Id);
                            if (FindSizeMała == null)
                            {
                                _context.DmProductSizes.Add(new Models.DmProductSize
                                {
                                    SizeName = "Mała",
                                    ProductCategoryId = FindCategory.Id
                                });
                                await _context.SaveChangesAsync();
                                FindSizeMała = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Mała" && x.ProductCategoryId == FindCategory.Id);
                            }

                            DmProductSize FindSizeŚrednia;
                            FindSizeŚrednia = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Średnia");
                            if (FindSizeŚrednia == null)
                            {
                                _context.DmProductSizes.Add(new Models.DmProductSize
                                {
                                    SizeName = "Średnia",
                                    ProductCategoryId = FindCategory.Id
                                });
                                await _context.SaveChangesAsync();
                                FindSizeŚrednia = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Średnia" && x.ProductCategoryId == FindCategory.Id);
                            }

                            DmProductSize FindSizeDuża;
                            FindSizeDuża = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Duża");
                            if (FindSizeDuża == null)
                            {
                                _context.DmProductSizes.Add(new Models.DmProductSize
                                {
                                    SizeName = "Duża",
                                    ProductCategoryId = FindCategory.Id
                                });
                                await _context.SaveChangesAsync();
                                FindSizeDuża = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Duża" && x.ProductCategoryId == FindCategory.Id);
                            }
                            //var FindCategory = await _context.DmProductCategories.FirstOrDefaultAsync(x => x.CategoryName == CategoryName);
                            if (ProductFind == null)
                            {
                                _context.DmProducts.Add(new Models.DmProduct
                                {
                                    ProductName = ProductName.ToUpper(),
                                    Formulation = ProductFormulation,
                                    CategoryId = FindCategory.Id,
                                    IsActive = true,
                                });

                                await _context.SaveChangesAsync();
                            }
                            ProductFind = await _context.DmProducts.FirstOrDefaultAsync(x => x.ProductName == ProductName);
                            /////////////////////////////////////////////////////////
                            var FindProductPriceMala = await _context.DmProductPrices.FirstOrDefaultAsync(x =>
                            x.ProductId == ProductFind.Id && x.SizeId == FindSizeMała.Id);

                            if (FindProductPriceMala == null)
                            {
                                double PriceMalaDouble;
                                Double.TryParse(PriceMala, out PriceMalaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    SizeId = FindSizeMała.Id,
                                    PriceValue = (decimal)PriceMalaDouble,
                                    CreatedDate = DateTime.Now
                                });
                            }
                            else
                            {
                                double Price;
                                Double.TryParse(PriceMala, out Price);
                                FindProductPriceMala.PriceValue = (decimal)Price;
                            }

                            //////////////////////////////////////////////////////////
                            var FindProductPriceŚrednia = await _context.DmProductPrices.FirstOrDefaultAsync(x =>
                            x.ProductId == ProductFind.Id && x.SizeId == FindSizeŚrednia.Id);
                            if (FindProductPriceŚrednia == null)
                            {
                                double PriceSredniaDouble;
                                Double.TryParse(PriceSrednia, out PriceSredniaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    SizeId = FindSizeŚrednia.Id,
                                    PriceValue = (decimal)PriceSredniaDouble,
                                    CreatedDate = DateTime.Now
                                });
                            }
                            else
                            {
                                double Price;
                                Double.TryParse(PriceSrednia, out Price);
                                FindProductPriceŚrednia.PriceValue = (decimal)Price;
                            }
                            //////////////////////////////////////////////////////////
                            var FindProductPriceDuża = await _context.DmProductPrices.FirstOrDefaultAsync(x =>
                            x.ProductId == ProductFind.Id && x.SizeId == FindSizeDuża.Id);
                            if (FindProductPriceDuża == null)
                            {
                                double PriceDużaDouble;
                                Double.TryParse(PriceDuza, out PriceDużaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    SizeId = FindSizeDuża.Id,
                                    PriceValue = (decimal)PriceDużaDouble,
                                    CreatedDate = DateTime.Now
                                });
                            }
                            else
                            {
                                double Price;
                                Double.TryParse(PriceDuza, out Price);
                                FindProductPriceDuża.PriceValue = (decimal)Price;
                            }
                            await _context.SaveChangesAsync();

                            if (row == 8)
                            {
                                string[] XlsxFiles = Directory.GetFiles(@"C:\Users\shahz\Videos", "*.xlsx");

                                for (int fileLength = 0; fileLength < XlsxFiles.Length; fileLength++)
                                {
                                    string filePath = XlsxFiles[fileLength];
                                    byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                                    MemoryStream ms = new MemoryStream(bytes);
                                    await PizzaWegetariańska(ms);

                                }
                                IsActive = false;
                            }
                        }
                    }
                }
            }

        }
        private async Task PizzaWegetariańska(Stream file)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[4];
                    bool IsActive = true;

                    for (int row = 2; IsActive; row++)
                    {
                        var ProductCategory = worksheet.Name;
                        if (worksheet.Name == "Pizza wegetariańska")
                        {
                            var StringProductName = worksheet.Cells[row, 1].Value.ToString();
                            string CountingNumber;
                            string ProductName;
                            CountingNumber = StringProductName.Substring(0, StringProductName.IndexOf('.', StringProductName.IndexOf('.', StringProductName.LastIndexOf('.') + 1) + 1));
                            ProductName = StringProductName.Replace(CountingNumber, "").Substring(1).Trim();
                            var ProductFormulation = worksheet.Cells[row, 2].Value.ToString();
                            var CategoryName = worksheet.Name.ToString();
                            var PriceMala = worksheet.Cells[row, 3].Value.ToString();
                            var PriceSrednia = worksheet.Cells[row, 4].Value.ToString();
                            var PriceDuza = worksheet.Cells[row, 5].Value.ToString();
                            var ProductFind = await _context.DmProducts.FirstOrDefaultAsync(x => x.ProductName == ProductName.ToUpper());
                            var FindCategory = await _context.DmProductCategories.FirstOrDefaultAsync(x => x.CategoryName.Equals(CategoryName));
                            //FindCategory.Id;

                            DmProductSize FindSizeMała;
                            FindSizeMała = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Mała" && x.ProductCategoryId == FindCategory.Id);
                            if (FindSizeMała == null)
                            {
                                _context.DmProductSizes.Add(new Models.DmProductSize
                                {
                                    SizeName = "Mała",
                                    ProductCategoryId = FindCategory.Id
                                });
                                await _context.SaveChangesAsync();
                                FindSizeMała = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Mała" && x.ProductCategoryId == FindCategory.Id);
                            }

                            DmProductSize FindSizeŚrednia;
                            FindSizeŚrednia = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Średnia");
                            if (FindSizeŚrednia == null)
                            {
                                _context.DmProductSizes.Add(new Models.DmProductSize
                                {
                                    SizeName = "Średnia",
                                    ProductCategoryId = FindCategory.Id
                                });
                                await _context.SaveChangesAsync();
                                FindSizeŚrednia = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Średnia" && x.ProductCategoryId == FindCategory.Id);
                            }

                            DmProductSize FindSizeDuża;
                            FindSizeDuża = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Duża");
                            if (FindSizeDuża == null)
                            {
                                _context.DmProductSizes.Add(new Models.DmProductSize
                                {
                                    SizeName = "Duża",
                                    ProductCategoryId = FindCategory.Id
                                });
                                await _context.SaveChangesAsync();
                                FindSizeDuża = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Duża" && x.ProductCategoryId == FindCategory.Id);
                            }
                            //var FindCategory = await _context.DmProductCategories.FirstOrDefaultAsync(x => x.CategoryName == CategoryName);
                            if (ProductFind == null)
                            {
                                _context.DmProducts.Add(new Models.DmProduct
                                {
                                    ProductName = ProductName.ToUpper(),
                                    Formulation = ProductFormulation,
                                    CategoryId = FindCategory.Id,
                                    IsActive = true,
                                });
                                await _context.SaveChangesAsync();
                            }
                            ProductFind = await _context.DmProducts.FirstOrDefaultAsync(x => x.ProductName == ProductName);
                            /////////////////////////////////////////////////////////
                            var FindProductPriceMala = await _context.DmProductPrices.FirstOrDefaultAsync(x =>
                            x.ProductId == ProductFind.Id && x.SizeId == FindSizeMała.Id);

                            if (FindProductPriceMala == null)
                            {
                                double PriceMalaDouble;
                                Double.TryParse(PriceMala, out PriceMalaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    SizeId = FindSizeMała.Id,
                                    PriceValue = (decimal)PriceMalaDouble,
                                    CreatedDate = DateTime.Now
                                });
                            }
                            else
                            {
                                double Price;
                                Double.TryParse(PriceMala, out Price);
                                FindProductPriceMala.PriceValue = (decimal)Price;
                            }

                            //////////////////////////////////////////////////////////
                            var FindProductPriceŚrednia = await _context.DmProductPrices.FirstOrDefaultAsync(x =>
                            x.ProductId == ProductFind.Id && x.SizeId == FindSizeŚrednia.Id);
                            if (FindProductPriceŚrednia == null)
                            {
                                double PriceSredniaDouble;
                                Double.TryParse(PriceSrednia, out PriceSredniaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    SizeId = FindSizeŚrednia.Id,
                                    PriceValue = (decimal)PriceSredniaDouble,
                                    CreatedDate = DateTime.Now
                                });
                            }
                            else
                            {
                                double Price;
                                Double.TryParse(PriceSrednia, out Price);
                                FindProductPriceŚrednia.PriceValue = (decimal)Price;
                            }
                            //////////////////////////////////////////////////////////
                            var FindProductPriceDuża = await _context.DmProductPrices.FirstOrDefaultAsync(x =>
                            x.ProductId == ProductFind.Id && x.SizeId == FindSizeDuża.Id);
                            if (FindProductPriceDuża == null)
                            {
                                double PriceDużaDouble;
                                Double.TryParse(PriceDuza, out PriceDużaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    SizeId = FindSizeDuża.Id,
                                    PriceValue = (decimal)PriceDużaDouble,
                                    CreatedDate = DateTime.Now
                                });
                            }
                            else
                            {
                                double Price;
                                Double.TryParse(PriceDuza, out Price);
                                FindProductPriceDuża.PriceValue = (decimal)Price;
                            }
                            await _context.SaveChangesAsync();
                            if (row == 9)
                            {
                                string[] XlsxFiles = Directory.GetFiles(@"C:\Users\shahz\Videos", "*.xlsx");

                                for (int fileLength = 0; fileLength < XlsxFiles.Length; fileLength++)
                                {
                                    string filePath = XlsxFiles[fileLength];
                                    byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                                    MemoryStream ms = new MemoryStream(bytes);
                                    await KEBABpieca(ms);

                                }
                                IsActive = false;
                            }
                        }
                    }
                }
            }
        }
        private async Task KEBABpieca(Stream file)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[5];
                    bool IsActive = true;

                    for (int row = 3; IsActive; row++)
                    {
                        var ProductCategory = worksheet.Name;
                        if (worksheet.Name == "KEBAB (w chlebku z pieca)")
                        {
                            var StringProductName = worksheet.Cells[row, 1].Value.ToString();
                            string CountingNumber;
                            string ProductName;
                            CountingNumber = StringProductName.Substring(0, StringProductName.IndexOf('.', StringProductName.IndexOf('.', StringProductName.LastIndexOf('.') + 1) + 1));
                            ProductName = StringProductName.Replace(CountingNumber, "").Substring(1).Trim();
                            var ProductFormulation = worksheet.Cells[row, 2].Value.ToString();
                            var CategoryName = worksheet.Name;
                            var PriceMala = worksheet.Cells[row, 3].Value.ToString().Trim();
                            var PriceExtraMala = PriceMala;
                            var PriceTrim = PriceMala.LastIndexOf("(");
                            if (PriceTrim > 0)
                                PriceMala = PriceMala.Substring(0, PriceTrim).Replace("zł", "").Trim();
                            //var PriceSrednia = worksheet.Cells[row, 4].Value.ToString();
                            var PriceDuza = worksheet.Cells[row, 4].Value.ToString();
                            var PriceExtraDuza = PriceDuza;
                            PriceTrim = PriceDuza.LastIndexOf("(");
                            if (PriceTrim > 0)
                                PriceDuza = PriceDuza.Substring(0, PriceTrim).Replace("zł", "").Trim();
                            PriceExtraMala = PriceExtraMala.Replace("13,5 zł (z dodatkowym mięsem ", "").Replace(" zł )", "");
                            PriceExtraDuza = PriceExtraDuza.Replace("17,5 zł (z dodatkowymmięsem ", "").Replace(" zł)", "");
                            var FindCategory = await _context.DmProductCategories.FirstOrDefaultAsync(x => x.CategoryName == CategoryName.ToUpper());

                            var ProductFind = await _context.DmProducts.FirstOrDefaultAsync(x => x.ProductName == ProductName.ToUpper());
                            //var FindCategory = await _context.DmProductCategories.FirstOrDefaultAsync(x => x.CategoryName == CategoryName);
                            DmProductSize FindSizeMała;
                            FindSizeMała = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Mała" && x.ProductCategoryId == FindCategory.Id);
                            if (FindSizeMała == null)
                            {
                                _context.DmProductSizes.Add(new Models.DmProductSize
                                {
                                    SizeName = "Mała",
                                    ProductCategoryId = FindCategory.Id
                                });
                                await _context.SaveChangesAsync();
                                FindSizeMała = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Mała" && x.ProductCategoryId == FindCategory.Id);
                            }

                            DmProductSize FindSizeDuża;
                            FindSizeDuża = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Duża");
                            if (FindSizeDuża == null)
                            {
                                _context.DmProductSizes.Add(new Models.DmProductSize
                                {
                                    SizeName = "Duża",
                                    ProductCategoryId = FindCategory.Id
                                });
                                await _context.SaveChangesAsync();
                                FindSizeDuża = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Duża" && x.ProductCategoryId == FindCategory.Id);
                            }
                            var KebabWithExtraMeatMala = "Kebab z dodatkowym mięsem";
                            var FindKebab = await _context.DmProducts.FirstOrDefaultAsync(x => x.ProductName == KebabWithExtraMeatMala && x.CategoryId == FindCategory.Id);
                            if (FindKebab == null && ProductFind == null)
                            {
                                double PriceMalaDouble;
                                Double.TryParse(PriceExtraMala, out PriceMalaDouble);
                                var Product = _context.DmProducts.Add(new Models.DmProduct
                                {
                                    ProductName = KebabWithExtraMeatMala,
                                    CategoryId = FindCategory.Id,
                                    Formulation = ProductFormulation,
                                    IsActive = true
                                });
                                await _context.SaveChangesAsync();
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = Product.Entity.Id,
                                    SizeId = FindSizeMała.Id,
                                    PriceValue = (decimal)PriceMalaDouble,
                                    CreatedDate = DateTime.Now
                                });

                                Double.TryParse(PriceExtraDuza, out PriceMalaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = Product.Entity.Id,
                                    SizeId = FindSizeDuża.Id,
                                    PriceValue = (decimal)PriceMalaDouble,
                                    CreatedDate = DateTime.Now
                                });

                            }
                            var FIndProductPricekebabMala = await _context.DmProductPrices.FirstOrDefaultAsync(x => x.ProductId == FindKebab.Id);
                            if (FIndProductPricekebabMala == null)
                            {
                                double PriceMalaDouble;
                                Double.TryParse(PriceExtraMala, out PriceMalaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = FindKebab.Id,
                                    SizeId = FindSizeMała.Id,
                                    PriceValue = (decimal)PriceMalaDouble,
                                    CreatedDate = DateTime.Now
                                });

                                Double.TryParse(PriceExtraDuza, out PriceMalaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = FindKebab.Id,
                                    SizeId = FindSizeDuża.Id,
                                    PriceValue = (decimal)PriceMalaDouble,
                                    CreatedDate = DateTime.Now
                                });
                            }

                            if (ProductFind == null)
                            {
                                _context.DmProducts.Add(new Models.DmProduct
                                {
                                    ProductName = ProductName.ToUpper(),
                                    Formulation = ProductFormulation,
                                    CategoryId = FindCategory.Id,
                                    IsActive = true,
                                });
                                await _context.SaveChangesAsync();
                            }
                            ProductFind = await _context.DmProducts.FirstOrDefaultAsync(x => x.ProductName == ProductName);
                            /////////////////////////////////////////////////////////
                            var FindProductPriceMala = await _context.DmProductPrices.FirstOrDefaultAsync(x =>
                            x.ProductId == ProductFind.Id && x.SizeId == FindSizeMała.Id);

                            if (FindProductPriceMala == null)
                            {
                                double PriceMalaDouble;
                                Double.TryParse(PriceMala, out PriceMalaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    SizeId = FindSizeMała.Id,
                                    PriceValue = (decimal)PriceMalaDouble,
                                    CreatedDate = DateTime.Now
                                });
                            }
                            else
                            {
                                double Price;
                                Double.TryParse(PriceMala, out Price);
                                FindProductPriceMala.PriceValue = (decimal)Price;
                            }

                            //////////////////////////////////////////////////////////
                            var FindProductPriceDuża = await _context.DmProductPrices.FirstOrDefaultAsync(x =>
                            x.ProductId == ProductFind.Id && x.SizeId == FindSizeDuża.Id);
                            if (FindProductPriceDuża == null)
                            {
                                double PriceDużaDouble;
                                Double.TryParse(PriceDuza, out PriceDużaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    SizeId = FindSizeDuża.Id,
                                    PriceValue = (decimal)PriceDużaDouble,
                                    CreatedDate = DateTime.Now
                                });
                            }
                            else
                            {
                                double Price;
                                Double.TryParse(PriceDuza, out Price);
                                FindProductPriceDuża.PriceValue = (decimal)Price;
                            }
                            await _context.SaveChangesAsync();
                            if (row == 3)
                            {
                                string[] XlsxFiles = Directory.GetFiles(@"C:\Users\shahz\Videos", "*.xlsx");

                                for (int fileLength = 0; fileLength < XlsxFiles.Length; fileLength++)
                                {
                                    string filePath = XlsxFiles[fileLength];
                                    byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                                    MemoryStream ms = new MemoryStream(bytes);
                                    await PIZZARODZINNA(ms);

                                }
                                IsActive = false;
                            }
                        }
                    }

                }
            }
        }
        private async Task PIZZARODZINNA(Stream file)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[6];
                    bool IsActive = true;
                    for (int row = 2; IsActive; row++)
                    {
                        var ProductCategory = worksheet.Name;
                        if (worksheet.Name == "PIZZA RODZINNA (dla 4-5 osób)")
                        {
                            var StringProductName = worksheet.Cells[row, 1].Value.ToString();
                            string CountingNumber;
                            string ProductName;
                            CountingNumber = StringProductName.Substring(0, StringProductName.IndexOf('.', StringProductName.IndexOf('.', StringProductName.LastIndexOf('.') + 1) + 1));
                            ProductName = StringProductName.Replace(CountingNumber, "").Substring(1).Trim();
                            //var ProductFormulation = worksheet.Cells[row, 2].Value.ToString();
                            var CategoryName = worksheet.Name.ToUpper();

                            var Price = worksheet.Cells[row, 2].Value.ToString().Replace("zł.", "").Trim(); ;
                            var ProductFind = await _context.DmProducts.FirstOrDefaultAsync(x => x.ProductName == ProductName.ToUpper());
                            var FindCategory = await _context.DmProductCategories.FirstOrDefaultAsync(x => x.CategoryName == CategoryName);
                            if (ProductFind == null)
                            {
                                _context.DmProducts.Add(new Models.DmProduct
                                {
                                    ProductName = ProductName.ToUpper(),
                                    Formulation = " ",
                                    CategoryId = FindCategory.Id,
                                    IsActive = true,
                                });
                                await _context.SaveChangesAsync();
                            }
                            var FindProductPrice = await _context.DmProductPrices.FirstOrDefaultAsync(x =>
                            x.ProductId == ProductFind.Id);

                            if (FindProductPrice == null)
                            {
                                double PriceMalaDouble;
                                Double.TryParse(Price, out PriceMalaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    PriceValue = (decimal)PriceMalaDouble,
                                    CreatedDate = DateTime.Now
                                });
                            }
                            else
                            {
                                double PriceMalaDouble;
                                Double.TryParse(Price, out PriceMalaDouble);
                                FindProductPrice.PriceValue = (decimal)PriceMalaDouble;
                            }
                            await _context.SaveChangesAsync();
                            if (row == 2)
                            {
                                string[] XlsxFiles = Directory.GetFiles(@"C:\Users\shahz\Videos", "*.xlsx");

                                for (int fileLength = 0; fileLength < XlsxFiles.Length; fileLength++)
                                {
                                    string filePath = XlsxFiles[fileLength];
                                    byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                                    MemoryStream ms = new MemoryStream(bytes);
                                    await KEBABnatalerzu(ms);
                                }
                                IsActive = false;
                            }
                        }
                    }

                }
            }
        }
        private async Task KEBABnatalerzu(Stream file)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[7];
                    bool IsActive = true;
                    for (int row = 2; IsActive; row++)
                    {
                        var ProductCategory = worksheet.Name;
                        if (worksheet.Name == "KEBAB na talerzu")
                        {
                            var StringProductName = worksheet.Cells[row, 1].Value.ToString();
                            string CountingNumber;
                            string ProductName;
                            CountingNumber = StringProductName.Substring(0, StringProductName.IndexOf('.', StringProductName.IndexOf('.', StringProductName.LastIndexOf('.') + 1) + 1));
                            ProductName = StringProductName.Replace(CountingNumber, "").Substring(1).Trim();

                            var CategoryName = worksheet.Name.ToUpper();

                            var Price = worksheet.Cells[row, 2].Value.ToString();
                            var ProductFind = await _context.DmProducts.FirstOrDefaultAsync(x => x.ProductName == ProductName);
                            var FindCategory = await _context.DmProductCategories.FirstOrDefaultAsync(x => x.CategoryName == CategoryName);
                            if (ProductFind == null)
                            {
                                _context.DmProducts.Add(new Models.DmProduct
                                {
                                    ProductName = ProductName.ToUpper(),
                                    Formulation = " ",
                                    CategoryId = FindCategory.Id,
                                    IsActive = true,
                                });
                                await _context.SaveChangesAsync();
                            }
                            var FindProductPrice = await _context.DmProductPrices.FirstOrDefaultAsync(x =>
                            x.ProductId == ProductFind.Id);

                            if (FindProductPrice == null)
                            {
                                double PriceMalaDouble;
                                Double.TryParse(Price, out PriceMalaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    PriceValue = (decimal)PriceMalaDouble,
                                    CreatedDate = DateTime.Now
                                });
                            }
                            else
                            {
                                double PriceMalaDouble;
                                Double.TryParse(Price, out PriceMalaDouble);
                                FindProductPrice.PriceValue = (decimal)PriceMalaDouble;
                            }
                            await _context.SaveChangesAsync();
                            if (row == 2)
                            {
                                string[] XlsxFiles = Directory.GetFiles(@"C:\Users\shahz\Videos", "*.xlsx");

                                for (int fileLength = 0; fileLength < XlsxFiles.Length; fileLength++)
                                {
                                    string filePath = XlsxFiles[fileLength];
                                    byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                                    MemoryStream ms = new MemoryStream(bytes);
                                    await SAŁATKI(ms);
                                }
                                IsActive = false;
                            }
                        }
                    }

                }
            }

        }
        private async Task SAŁATKI(Stream file)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[8];
                    bool IsActive = true;
                    //worksheet = package.Workbook.Worksheets[8];
                    for (int row = 2; IsActive; row++)
                    {
                        var ProductCategory = worksheet.Name;
                        if (worksheet.Name == "SAŁATKI")
                        {
                            var StringProductName = worksheet.Cells[row, 1].Value.ToString();
                            string CountingNumber;
                            string ProductName;
                            CountingNumber = StringProductName.Substring(0, StringProductName.IndexOf('.', StringProductName.IndexOf('.', StringProductName.LastIndexOf('.') + 1) + 1));
                            ProductName = StringProductName.Replace(CountingNumber, "").Substring(1).Trim();
                            var ProductFormulation = worksheet.Cells[row, 2].Value.ToString();
                            var CategoryName = worksheet.Name.ToUpper();

                            var Price = worksheet.Cells[row, 3].Value.ToString();
                            var ProductFind = await _context.DmProducts.FirstOrDefaultAsync(x => x.ProductName == ProductName.ToUpper());
                            var FindCategory = await _context.DmProductCategories.FirstOrDefaultAsync(x => x.CategoryName == CategoryName);
                            if (ProductFind == null)
                            {
                                _context.DmProducts.Add(new Models.DmProduct
                                {
                                    ProductName = ProductName.ToUpper(),
                                    Formulation = ProductFormulation,
                                    CategoryId = FindCategory.Id,
                                    IsActive = true,
                                });
                                await _context.SaveChangesAsync();
                            }
                            var FindProductPrice = await _context.DmProductPrices.FirstOrDefaultAsync(x =>
                            x.ProductId == ProductFind.Id);

                            if (FindProductPrice == null)
                            {
                                double PriceMalaDouble;
                                Double.TryParse(Price, out PriceMalaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    PriceValue = (decimal)PriceMalaDouble,
                                    CreatedDate = DateTime.Now
                                });
                            }
                            else
                            {
                                double PriceMalaDouble;
                                Double.TryParse(Price, out PriceMalaDouble);
                                FindProductPrice.PriceValue = (decimal)PriceMalaDouble;
                            }
                            await _context.SaveChangesAsync();
                            if (row == 7)
                            {
                                string[] XlsxFiles = Directory.GetFiles(@"C:\Users\shahz\Videos", "*.xlsx");

                                for (int fileLength = 0; fileLength < XlsxFiles.Length; fileLength++)
                                {
                                    string filePath = XlsxFiles[fileLength];
                                    byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                                    MemoryStream ms = new MemoryStream(bytes);
                                    await DODATKIDOPIZZY(ms);
                                }
                                IsActive = false;
                            }
                        }
                    }
                }
            }
        }
        private async Task DODATKIDOPIZZY(Stream file)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[9];
                    bool IsActive = true;

                    for (int row = 2; IsActive; row++)
                    {
                        var ProductCategory = worksheet.Name;
                        if (worksheet.Name == "DODATKI DO PIZZY")
                        {
                            var StringProductName = worksheet.Cells[row, 1].Value.ToString();
                            string CountingNumber;
                            string ProductName;
                            CountingNumber = StringProductName.Substring(0, StringProductName.IndexOf('.', StringProductName.IndexOf('.', StringProductName.LastIndexOf('.') + 1) + 1));
                            ProductName = StringProductName.Replace(CountingNumber, "").Substring(1).Trim();
                            //var ProductFormulation = worksheet.Cells[row, 2].Value.ToString();
                            var CategoryName = worksheet.Name.ToUpper();
                            var PriceMala = worksheet.Cells[row, 2].Value.ToString();
                            var PriceSrednia = worksheet.Cells[row, 3].Value.ToString();
                            var PriceDuza = worksheet.Cells[row, 4].Value.ToString();
                            var ProductFind = await _context.DmProducts.FirstOrDefaultAsync(x => x.ProductName == ProductName.ToUpper());
                            var FindSizeMała = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Mała");
                            var FindSizeŚrednia = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Średnia");
                            var FindSizeDuża = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == "Duża");
                            var FindCategory = await _context.DmProductCategories.FirstOrDefaultAsync(x => x.CategoryName == CategoryName);
                            if (ProductFind == null)
                            {
                                _context.DmProducts.Add(new Models.DmProduct
                                {
                                    ProductName = ProductName.ToUpper(),
                                    Formulation = " ",
                                    CategoryId = FindCategory.Id,
                                    IsActive = true,
                                });
                                await _context.SaveChangesAsync();
                            }
                            ProductFind = await _context.DmProducts.FirstOrDefaultAsync(x => x.ProductName == ProductName);
                            /////////////////////////////////////////////////////////
                            var FindProductPriceMala = await _context.DmProductPrices.FirstOrDefaultAsync(x =>
                            x.ProductId == ProductFind.Id && x.SizeId == FindSizeMała.Id);

                            if (FindProductPriceMala == null)
                            {
                                double PriceMalaDouble;
                                Double.TryParse(PriceMala, out PriceMalaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    SizeId = FindSizeMała.Id,
                                    PriceValue = (decimal)PriceMalaDouble,
                                    CreatedDate = DateTime.Now
                                });
                            }
                            else
                            {
                                double Price;
                                Double.TryParse(PriceMala, out Price);
                                FindProductPriceMala.PriceValue = (decimal)Price;
                            }

                            //////////////////////////////////////////////////////////
                            var FindProductPriceŚrednia = await _context.DmProductPrices.FirstOrDefaultAsync(x =>
                            x.ProductId == ProductFind.Id && x.SizeId == FindSizeŚrednia.Id);
                            if (FindProductPriceŚrednia == null)
                            {
                                double PriceSredniaDouble;
                                Double.TryParse(PriceSrednia, out PriceSredniaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    SizeId = FindSizeŚrednia.Id,
                                    PriceValue = (decimal)PriceSredniaDouble,
                                    CreatedDate = DateTime.Now
                                });
                            }
                            else
                            {
                                double Price;
                                Double.TryParse(PriceSrednia, out Price);
                                FindProductPriceŚrednia.PriceValue = (decimal)Price;
                            }
                            //////////////////////////////////////////////////////////
                            var FindProductPriceDuża = await _context.DmProductPrices.FirstOrDefaultAsync(x =>
                            x.ProductId == ProductFind.Id && x.SizeId == FindSizeDuża.Id);
                            if (FindProductPriceDuża == null)
                            {
                                double PriceDużaDouble;
                                Double.TryParse(PriceDuza, out PriceDużaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    SizeId = FindSizeDuża.Id,
                                    PriceValue = (decimal)PriceDużaDouble,
                                    CreatedDate = DateTime.Now
                                });
                            }
                            else
                            {
                                double Price;
                                Double.TryParse(PriceDuza, out Price);
                                FindProductPriceDuża.PriceValue = (decimal)Price;
                            }
                            await _context.SaveChangesAsync();
                            if (row == 3)
                            {
                                string[] XlsxFiles = Directory.GetFiles(@"C:\Users\shahz\Videos", "*.xlsx");

                                for (int fileLength = 0; fileLength < XlsxFiles.Length; fileLength++)
                                {
                                    string filePath = XlsxFiles[fileLength];
                                    byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                                    MemoryStream ms = new MemoryStream(bytes);
                                    await SOSYDOPIZZY(ms);
                                }
                                IsActive = false;
                            }
                        }
                    }

                }
            }

        }
        private async Task SOSYDOPIZZY(Stream file)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[10];
                    bool IsActive = true;
                    for (int row = 2; IsActive; row++)
                    {
                        var ProductCategory = worksheet.Name;
                        var PriceColumn = worksheet.Cells[row, 2].Value;
                        if (worksheet.Name == "SOSY DO PIZZY" && PriceColumn != null)
                        {
                            var StringProductName = worksheet.Cells[row, 1].Value.ToString();
                            string CountingNumber;
                            string ProductName;
                            if (StringProductName.Contains("."))
                            {
                                CountingNumber = StringProductName.Substring(0, StringProductName.IndexOf('.', StringProductName.IndexOf('.', StringProductName.LastIndexOf('.') + 1) + 1));
                                ProductName = StringProductName.Replace(CountingNumber, "").Substring(1).Trim();
                            }
                            else
                                ProductName = StringProductName;

                            //var ProductFormulation = worksheet.Cells[row, 2].Value.ToString();
                            var CategoryName = worksheet.Name.ToUpper();
                            var Price = worksheet.Cells[row, 2].Value.ToString();
                            var ProductFind = await _context.DmProducts.FirstOrDefaultAsync(x => x.ProductName == ProductName.Trim());
                            var FindCategory = await _context.DmProductCategories.FirstOrDefaultAsync(x => x.CategoryName == CategoryName);
                            if (ProductFind == null)
                            {
                                _context.DmProducts.Add(new Models.DmProduct
                                {
                                    ProductName = ProductName.ToUpper(),
                                    Formulation = " ",
                                    CategoryId = FindCategory.Id,
                                    IsActive = true,
                                });
                                await _context.SaveChangesAsync();
                            }
                            var FindProductPrice = await _context.DmProductPrices.FirstOrDefaultAsync(x =>
                            x.ProductId == ProductFind.Id);

                            if (FindProductPrice == null)
                            {
                                double PriceMalaDouble;
                                Double.TryParse(Price, out PriceMalaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    PriceValue = (decimal)PriceMalaDouble,
                                    CreatedDate = DateTime.Now
                                });
                            }
                            else
                            {
                                double PriceMalaDouble;
                                Double.TryParse(Price, out PriceMalaDouble);
                                FindProductPrice.PriceValue = (decimal)PriceMalaDouble;
                            }
                            await _context.SaveChangesAsync();
                            if (row == 9)
                            {
                                string[] XlsxFiles = Directory.GetFiles(@"C:\Users\shahz\Videos", "*.xlsx");

                                for (int fileLength = 0; fileLength < XlsxFiles.Length; fileLength++)
                                {
                                    string filePath = XlsxFiles[fileLength];
                                    byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                                    MemoryStream ms = new MemoryStream(bytes);
                                    await FRYTKI(ms);
                                }
                                IsActive = false;
                            }
                        }
                    }
                }
            }
        }
        private async Task FRYTKI(Stream file)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[11];
                    bool IsActive = true;
                    for (int row = 2; IsActive; row++)
                    {
                        var ProductCategory = worksheet.Name;
                        var PriceColumn = worksheet.Cells[row, 2].Value;
                        if (worksheet.Name == "FRYTKI" && PriceColumn != null)
                        {
                            var StringProductName = worksheet.Cells[row, 1].Value.ToString();
                            string CountingNumber;
                            string ProductName;
                            if (StringProductName.Contains(".") && !StringProductName.Contains("gr."))
                            {
                                CountingNumber = StringProductName.Substring(0, StringProductName.IndexOf('.', StringProductName.IndexOf('.', StringProductName.LastIndexOf('.') + 1) + 1));
                                ProductName = StringProductName.Replace(CountingNumber, "").Substring(1).Trim();
                            }
                            else
                                ProductName = StringProductName;

                            //var ProductFormulation = worksheet.Cells[row, 2].Value.ToString();
                            var CategoryName = worksheet.Name.ToUpper();
                            var Price = worksheet.Cells[row, 2].Value.ToString();
                            var ProductFind = await _context.DmProducts.FirstOrDefaultAsync(x => x.ProductName == ProductName);
                            var FindCategory = await _context.DmProductCategories.FirstOrDefaultAsync(x => x.CategoryName == CategoryName);
                            if (ProductFind == null)
                            {
                                _context.DmProducts.Add(new Models.DmProduct
                                {
                                    ProductName = ProductName.ToUpper(),
                                    Formulation = " ",
                                    CategoryId = FindCategory.Id,
                                    IsActive = true,
                                });
                                await _context.SaveChangesAsync();
                            }
                            ProductFind = await _context.DmProducts.FirstOrDefaultAsync(x => x.ProductName == ProductName);
                            var FindProductPrice = await _context.DmProductPrices.FirstOrDefaultAsync(x =>
                            x.ProductId == ProductFind.Id);

                            if (FindProductPrice == null)
                            {
                                double PriceMalaDouble;
                                Double.TryParse(Price, out PriceMalaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    PriceValue = (decimal)PriceMalaDouble,
                                    CreatedDate = DateTime.Now
                                });
                            }
                            else
                            {
                                double PriceMalaDouble;
                                Double.TryParse(Price, out PriceMalaDouble);
                                FindProductPrice.PriceValue = (decimal)PriceMalaDouble;
                            }
                            await _context.SaveChangesAsync();
                            if (row == 2)
                            {
                                string[] XlsxFiles = Directory.GetFiles(@"C:\Users\shahz\Videos", "*.xlsx");

                                for (int fileLength = 0; fileLength < XlsxFiles.Length; fileLength++)
                                {
                                    string filePath = XlsxFiles[fileLength];
                                    byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                                    MemoryStream ms = new MemoryStream(bytes);
                                    await HAMBURGER(ms);
                                }
                                IsActive = false;
                            }
                        }
                    }

                }
            }

        }
        private async Task HAMBURGER(Stream file)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[12];
                    bool IsActive = true;
                    for (int row = 2; IsActive; row++)
                    {
                        var ProductCategory = worksheet.Name;
                        var PriceColumn = worksheet.Cells[row, 2].Value;
                        if (worksheet.Name == "HAMBURGER" && PriceColumn != null)
                        {
                            var StringProductName = worksheet.Cells[row, 1].Value.ToString();
                            string CountingNumber;
                            string ProductName;
                            if (StringProductName.Contains(".") && !StringProductName.Contains("gr."))
                            {
                                CountingNumber = StringProductName.Substring(0, StringProductName.IndexOf('.', StringProductName.IndexOf('.', StringProductName.LastIndexOf('.') + 1) + 1));
                                ProductName = StringProductName.Replace(CountingNumber, "").Substring(1).Trim();
                            }
                            else
                                ProductName = StringProductName;

                            var ProductFormulation = worksheet.Cells[row, 2].Value.ToString();
                            var CategoryName = worksheet.Name.ToUpper();
                            var Price = worksheet.Cells[row, 3].Value.ToString();
                            var ProductFind = await _context.DmProducts.FirstOrDefaultAsync(x => x.ProductName == ProductName.ToUpper());
                            var FindCategory = await _context.DmProductCategories.FirstOrDefaultAsync(x => x.CategoryName == CategoryName);
                            if (ProductFind == null)
                            {
                                _context.DmProducts.Add(new Models.DmProduct
                                {
                                    ProductName = ProductName.ToUpper(),
                                    Formulation = ProductFormulation,
                                    CategoryId = FindCategory.Id,
                                    IsActive = true,
                                });
                                await _context.SaveChangesAsync();
                            }
                            ProductFind = await _context.DmProducts.FirstOrDefaultAsync(x => x.ProductName == ProductName.ToUpper());
                            var FindProductPrice = await _context.DmProductPrices.FirstOrDefaultAsync(x =>
                            x.ProductId == ProductFind.Id);

                            if (FindProductPrice == null)
                            {
                                double PriceMalaDouble;
                                Double.TryParse(Price, out PriceMalaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    PriceValue = (decimal)PriceMalaDouble,
                                    CreatedDate = DateTime.Now
                                });
                            }
                            else
                            {
                                double PriceMalaDouble;
                                Double.TryParse(Price, out PriceMalaDouble);
                                FindProductPrice.PriceValue = (decimal)PriceMalaDouble;
                            }
                            await _context.SaveChangesAsync();
                            if (row == 2)
                            {
                                string[] XlsxFiles = Directory.GetFiles(@"C:\Users\shahz\Videos", "*.xlsx");

                                for (int fileLength = 0; fileLength < XlsxFiles.Length; fileLength++)
                                {
                                    string filePath = XlsxFiles[fileLength];
                                    byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                                    MemoryStream ms = new MemoryStream(bytes);
                                    await Napoje(ms);
                                }
                                IsActive = false;
                            }
                        }
                    }
                }
            }

        }
        private async Task Napoje(Stream file)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[13];
                    bool IsActive = true;
                    for (int row = 2; IsActive; row++)
                    {
                        var ProductCategory = worksheet.Name;
                        var PriceColumn = worksheet.Cells[row, 2].Value;
                        if (worksheet.Name == "Napoje" && PriceColumn != null)
                        {
                            var StringProductName = worksheet.Cells[row, 1].Value.ToString();
                            string CountingNumber;
                            string ProductName;
                            if (StringProductName.Contains(".") && !StringProductName.Contains("gr."))
                            {
                                CountingNumber = StringProductName.Substring(0, StringProductName.IndexOf('.', StringProductName.IndexOf('.', StringProductName.LastIndexOf('.') + 1) + 1));
                                ProductName = StringProductName.Replace(CountingNumber, "").Substring(1).Trim();
                            }
                            else
                                ProductName = StringProductName;

                            //var ProductFormulation = worksheet.Cells[row, 2].Value.ToString();
                            var Size = worksheet.Cells[row, 2].Value.ToString().Replace(",", ".");


                            var FindSize = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeValue == Size);
                            if (FindSize == null)
                            {
                                _context.DmProductSizes.Add(new Models.DmProductSize
                                {
                                    SizeName = Size == "0.85 l" ? "Średnia" : "Mała",
                                    SizeValue = Size
                                });
                                await _context.SaveChangesAsync();
                            }
                            var CategoryName = worksheet.Name.ToUpper();
                            var Price = worksheet.Cells[row, 3].Value.ToString().Replace("zł", "").Trim();
                            var ProductFind = await _context.DmProducts.FirstOrDefaultAsync(x => x.ProductName == ProductName.ToUpper());
                            var FindCategory = await _context.DmProductCategories.FirstOrDefaultAsync(x => x.CategoryName == CategoryName);
                            if (ProductFind == null)
                            {
                                _context.DmProducts.Add(new Models.DmProduct
                                {
                                    ProductName = ProductName.ToUpper(),
                                    Formulation = " ",
                                    CategoryId = FindCategory.Id,
                                    IsActive = true,
                                });
                                await _context.SaveChangesAsync();
                            }
                            ProductFind = await _context.DmProducts.FirstOrDefaultAsync(x => x.ProductName == ProductName.ToUpper());
                            var FindProductPrice = await _context.DmProductPrices.FirstOrDefaultAsync(x =>
                            x.ProductId == ProductFind.Id);

                            if (FindProductPrice == null)
                            {
                                double PriceMalaDouble;
                                Double.TryParse(Price, out PriceMalaDouble);
                                _context.DmProductPrices.Add(new Models.DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    PriceValue = (decimal)PriceMalaDouble,
                                    CreatedDate = DateTime.Now
                                });
                            }
                            else
                            {
                                double PriceMalaDouble;
                                Double.TryParse(Price, out PriceMalaDouble);
                                FindProductPrice.PriceValue = (decimal)PriceMalaDouble;
                            }
                            await _context.SaveChangesAsync();
                            if (row == 11)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            //private async Task(Stream file)
            //{
            //    using (var stream = new MemoryStream())
            //    {
            //        await file.CopyToAsync(stream);
            //        using (var package = new ExcelPackage(stream))
            //        {
            //            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //            ExcelWorksheet worksheet = package.Workbook.Worksheets[2];
            //            bool IsActive = true;
            //            
            //        }
            //    }

            //}
        }
    }
}

