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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SperanzaPizzaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsParsing : ControllerBase
    {
        private readonly dbPizzaContext _context;

        public ProductsParsing(dbPizzaContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Parsing(IFormFile file)
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
                    for (int row = 2; /*row < rowcount - 1*/; row++)
                    {
                        if (row == 41)
                        {

                        }
                        bool boolean = true;
                        while (boolean)
                        {

                            if (worksheet.Cells[row, 1].Value == null)
                            {
                                row = 2;
                                WorkSheetCount++;
                                worksheet = package.Workbook.Worksheets[WorkSheetCount];
                                break;
                            }
                            else
                                boolean = false;
                        }

                        if(worksheet.Cells[row, 1].Value.ToString() == "Nazwa")
                        {
                            row++;
                        }
                        var StringProductName = worksheet.Cells[row, 1].Value.ToString();
                        string CountingNumber;
                        string ProductName;
                        //if (StringProductName.Substring(0, StringProductName.IndexOf('.')).Length > 0)
                        //{
                            CountingNumber = StringProductName.Substring(0, StringProductName.IndexOf('.', StringProductName.IndexOf('.', StringProductName.LastIndexOf('.') + 1) + 1));
                            ProductName = StringProductName.Replace(CountingNumber, "").Substring(1).Trim().ToUpper();
                        //}
                        //else
                        //{
                        //    ProductName = StringProductName;
                        //}


                        
                        int RowCount = 1;

                        string PriceMala;// = worksheet.Cells[row, 3].Value.ToString().Replace(",00 zł", "").Trim().Replace(",00 z", "");
                        string PriceSredniya;// = worksheet.Cells[row, 4].Value.ToString().Replace(",00 zł", "").Trim().Replace(",00 z", "");
                        string PriceDuza;//= worksheet.Cells[row, 5].Value.ToString().Replace(",00 zł", "").Trim().Replace(",00 z", "");
                        var SizeIdentification = worksheet.Cells[RowCount, 3].Value;

                        double PriceIdentification;
                        if (SizeIdentification == null && worksheet.Name == "PIZZA RODZINNA (dla 4-5 osób)")
                        {
                            var PriceFind = worksheet.Cells[row, 3 - 1].Value.ToString().Replace(",00 zł.", "").Trim().Replace(",00 z", "").Replace(",00 zł", "");
                            double.TryParse(PriceFind, out PriceIdentification);
                        }
                        else
                            PriceIdentification = 0;
                        if (SizeIdentification == null && worksheet.Name != "PIZZA RODZINNA (dla 4-5 osób)")
                            RowCount++;
                        var SizeMala = worksheet.Cells[RowCount, 3].Value;
                        double Price = 0;
                        if (SizeMala != null)
                        {
                            if (SizeMala.ToString() == "Maly" || SizeMala.ToString() == "Mala")
                            {
                                PriceMala = worksheet.Cells[row, 3].Value.ToString().Replace(",00 zł", "").Trim().Replace(",00 z", "");
                            }
                            else
                                PriceMala = null;

                        }
                        else
                            PriceMala = null;

                        var SizeSrediniya = worksheet.Cells[RowCount, 4].Value;
                        if(SizeSrediniya != null)
                        {
                            if (SizeSrediniya.ToString() == "Srednia")
                            {
                                PriceSredniya = worksheet.Cells[row, 4].Value.ToString().Replace(",00 zł", "").Trim().Replace(",00 z", "");
                            }
                            else
                                PriceSredniya = null;

                        }
                        else
                            PriceSredniya = null;

                        //var SizeDuza = worksheet.Cells[RowCount, 4].Value;
                        int Column = 5;
                        SizeIdentification = worksheet.Cells[RowCount, Column].Value;
                        string SizeDuza;
                        if (SizeIdentification == null && worksheet.Name != "PIZZA RODZINNA (dla 4-5 osób)" && worksheet.Name != "KEBAB na talerzu" && worksheet.Name != "SAŁATKI" && worksheet.Name != "SOSY DO PIZZY")
                            SizeDuza = worksheet.Cells[RowCount, Column = Column - 1].Value.ToString();
                        else if (SizeIdentification != null)
                        {
                            SizeDuza = worksheet.Cells[RowCount, Column].Value.ToString();
                        }
                        else
                            SizeDuza = null;
                        if(worksheet.Name == "KEBAB na talerzu")
                        {
                            var PriceFind = worksheet.Cells[row, 2].Value.ToString().Replace(",00 zł.", "").Trim().Replace(",00 z", "").Replace(",00 zł", "");
                            double.TryParse(PriceFind, out PriceIdentification);
                        }
                         //= worksheet.Cells[RowCount, 5].Value;
                        if(SizeDuza != null)
                        {
                            if (SizeDuza.ToString() == "Duza" || SizeDuza.ToString() == "Duzy")
                            {
                                PriceDuza = worksheet.Cells[row, Column].Value.ToString().Replace(",00 zł", "").Trim().Replace(",00 z", "");
                            }
                            else
                                PriceDuza = null; 
                        }
                        else
                            PriceDuza = null;
                        var FindCategory = await _context.DmProductCategories.FirstOrDefaultAsync(x=>x.CategoryName == worksheet.Name);
                        //var ProductFormulation = worksheet.Cells[row, 2].Value.ToString();
                        DmProduct ProductFind;
                        bool isActive = true;
                        bool categoryActive = true;
                        while (isActive)
                        {
                            string PriceFind;
                            ProductFind = await _context.DmProducts.FirstOrDefaultAsync(x => x.ProductName == ProductName);
                            if (ProductFind == null && FindCategory != null)
                            {
                                var insertProduct = _context.DmProducts.Add(new DmProduct
                                {
                                    ProductName = ProductName,
                                    CategoryId = FindCategory.Id,
                                    Formulation = " ",
                                    IsActive = true
                                });
                                await _context.SaveChangesAsync();
                            }
                            else if (PriceIdentification == 0 && worksheet.Name == "SOSY DO PIZZY")
                            {
                                categoryActive = false;
                                StringProductName = worksheet.Cells[row, 1].Value.ToString();
                                 
                                //if (StringProductName.Substring(0, StringProductName.IndexOf('.')).Length > 0)
                                //{
                                CountingNumber = StringProductName.Substring(0, StringProductName.IndexOf('.', StringProductName.IndexOf('.', StringProductName.LastIndexOf('.') + 1) + 1));
                                ProductName = StringProductName.Replace(CountingNumber, "").Substring(1).Trim().ToUpper();
                                
                                if (worksheet.Cells[row, 2].Value != null)
                                {
                                    PriceFind = worksheet.Cells[row, 2].Value.ToString().Replace(",00 zł.", "").Trim().Replace(",00 z", "").Replace(",00 zł", "");
                                    double.TryParse(PriceFind, out Price);

                                }
                                else
                                {
                                    row++;
                                    PriceFind = null;
                                }
                                if(PriceFind != null)
                                {
                                    categoryActive = true;
                                }
                            }
                            if(categoryActive == true)
                                isActive = false;

                        }
                        
                        if(ProductName == "FRYTKI + KEBAB(30GR.)SOS CZOSNKOWY+ SURÓWKA  DO WYBORU")
                        {
                             var PriceFind = worksheet.Cells[row, 2].Value.ToString().Replace(",00 zł.", "").Trim().Replace(",00 z", "").Replace(",00 zł", "");
                             //PriceFind = Convert.ToDouble(PriceFind, out PriceIdentification);
                             double.TryParse(PriceFind, out PriceIdentification);
                        }
                        ProductFind = await _context.DmProducts.FirstOrDefaultAsync(x => x.ProductName == ProductName);
                        
                        if (worksheet.Name == "DODATKI DO PIZZY")
                        {
                            PriceMala = worksheet.Cells[row, 2].Value.ToString().Replace(",00 zł", "").Trim().Replace(",00 z", "");
                            PriceSredniya = worksheet.Cells[row, 3].Value.ToString().Replace(",00 zł", "").Trim().Replace(",00 z", "");
                            PriceDuza = worksheet.Cells[row, 4].Value.ToString().Replace(",00 zł", "").Trim().Replace(",00 z", "");
                        }
                        var FindproductPrice = await _context.DmProductPrices.FirstOrDefaultAsync(x=>x.ProductId == ProductFind.Id);

                            DmProductPrice FindProductPriceMala;
                        if (ProductFind != null)
                        {
                            var FindSizeMala = await _context.DmProductSizes.FirstOrDefaultAsync(x=>x.SizeName == SizeMala.ToString() && x.ProductCategoryId == FindCategory.Id);
                            if (FindSizeMala != null)
                            {
                                FindProductPriceMala = await _context.DmProductPrices.FirstOrDefaultAsync(x => x.ProductId == ProductFind.Id && x.SizeId == FindSizeMala.Id);
                                //x.SizeId == 4 || x.SizeId == 7 ||
                                //x.SizeId == 10 || x.SizeId == 13 ||
                                //x.SizeId == 16 || x.SizeId == 19 || x.SizeId == 23

                            }
                            else
                                FindProductPriceMala = null;
                            var FindSizeSredniya = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == SizeSrediniya.ToString() && x.ProductCategoryId == FindCategory.Id);
                            DmProductPrice FindProductPriceSredniya;
                            if (FindSizeSredniya != null)
                            {
                                FindProductPriceSredniya = await _context.DmProductPrices.FirstOrDefaultAsync(x => x.ProductId == ProductFind.Id && x.SizeId == FindSizeMala.Id);
                                //x.SizeId == 4 || x.SizeId == 7 ||
                                //x.SizeId == 10 || x.SizeId == 13 ||
                                //x.SizeId == 16 || x.SizeId == 19 || x.SizeId == 23

                            }
                            else
                                FindProductPriceSredniya = null;
                            
                            var FindSizeDuza = await _context.DmProductSizes.FirstOrDefaultAsync(x => x.SizeName == SizeDuza.ToString() && x.ProductCategoryId == FindCategory.Id);
                            DmProductPrice FindProductPriceDuza;
                            if (FindSizeDuza != null)
                            {
                                FindProductPriceDuza = await _context.DmProductPrices.FirstOrDefaultAsync(x => x.ProductId == ProductFind.Id && x.SizeId == FindSizeMala.Id);
                                //x.SizeId == 4 || x.SizeId == 7 ||
                                //x.SizeId == 10 || x.SizeId == 13 ||
                                //x.SizeId == 16 || x.SizeId == 19 || x.SizeId == 23

                            }
                            else
                                FindProductPriceDuza = null;

                            //var FindProductPriceSredniya = await _context.DmProductPrices.FirstOrDefaultAsync(x => x.ProductId == ProductFind.Id && x.SizeId == 2 ||
                            //x.SizeId == 5|| x.SizeId == 8|| x.SizeId == 11 || x.SizeId == 14|| x.SizeId == 17|| x.SizeId == 20|| x.SizeId == 23);

                            //var FindProductPriceDuza = await _context.DmProductPrices.FirstOrDefaultAsync(x => x.ProductId == ProductFind.Id && x.SizeId == 3||
                            //x.SizeId == 6|| x.SizeId == 9|| x.SizeId == 12 || x.SizeId == 15|| x.SizeId == 18|| x.SizeId == 21|| x.SizeId == 24);
                            
                            if (PriceMala == null && PriceSredniya == null && PriceDuza == null)
                            {
                                Price = PriceIdentification;
                                if (FindproductPrice == null)
                                {
                                    
                                    var insertProduct = _context.DmProductPrices.Add(new DmProductPrice
                                    {
                                        ProductId = ProductFind.Id,
                                        //SizeId = null,
                                        CreatedDate = DateTime.Now,
                                        PriceValue = (decimal)Price
                                    });
                                
                                }
                                else
                                {
                                    FindproductPrice.PriceValue = (decimal)Price;

                                }
                                //var insertProduct = _context.DmProductPrices.Add(new DmProductPrice
                                //{
                                //    ProductId = ProductFind.Id,
                                //    //SizeId = null,
                                //    CreatedDate = DateTime.Now,
                                //    PriceValue = Price
                                //});
                            }

                            
                            if (FindProductPriceMala != null && PriceMala != null || PriceSredniya != null || PriceDuza != null)
                            {
                                double.TryParse(PriceMala, out Price);
                                FindProductPriceMala.PriceValue = (decimal)Price;
                            }
                            else if (FindProductPriceMala == null && PriceMala != null)
                            {
                                double.TryParse(PriceMala, out Price);
                                var insertProduct = _context.DmProductPrices.Add(new DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    SizeId = 1,
                                    CreatedDate = DateTime.Now,
                                    PriceValue = (decimal)Price
                                });
                                //var FPP = await _context.DmProductPrices.FirstOrDefaultAsync(x=>x.ProductId == ProductFind.Id);
                                //FPP.PriceValue = Price;
                                //FindProductPriceMala.PriceValue = Price;
                            }
                            //////////////////
                            if (FindProductPriceSredniya != null)
                            {
                                double.TryParse(PriceSredniya, out Price);
                                FindProductPriceSredniya.PriceValue = (decimal)Price;
                            }
                            else if (FindProductPriceSredniya == null && PriceSredniya != null)
                            {
                                double.TryParse(PriceSredniya, out Price);
                                var insertProduct = _context.DmProductPrices.Add(new DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    SizeId = 2,
                                    CreatedDate = DateTime.Now,
                                    PriceValue = (decimal)Price
                                });
                                //var FPP = await _context.DmProductPrices.FirstOrDefaultAsync(x => x.ProductId == ProductFind.Id);
                                //FPP.PriceValue = Price;
                                //FindProductPriceSredniya.PriceValue = Price;
                            }
                            ///////////////
                            if (FindProductPriceDuza != null)
                            {
                                double.TryParse(PriceDuza, out Price);
                                FindProductPriceDuza.PriceValue = (decimal)Price;
                            }
                            else if (FindProductPriceDuza == null && PriceDuza != null)
                            {
                                double.TryParse(PriceDuza, out Price);
                                var insertProduct = _context.DmProductPrices.Add(new DmProductPrice
                                {
                                    ProductId = ProductFind.Id,
                                    SizeId = 3,
                                    CreatedDate = DateTime.Now,
                                    PriceValue = (decimal)Price
                                });
                                //var FPP = await _context.DmProductPrices.FirstOrDefaultAsync(x => x.ProductId == ProductFind.Id);
                                //FPP.PriceValue = Price;
                                // FindProductPriceDuza.PriceValue = Price;
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            var insertProduct = _context.DmProductPrices.Add(new DmProductPrice
                            {
                                ProductId = ProductFind.Id,
                                //SizeId = null,
                                CreatedDate = DateTime.Now,
                                PriceValue = (decimal)Price
                            });
                        }


                    }

                    //}
                    //catch (Exception ex)
                    //{
                    //    ex.Message.ToString();
                    //    WorkSheetCount++;
                    //}


                }

            }
        }
        //[HttpPost("TEST")]
        //public async Task<IActionResult> ReturnAsync(IFormFile file)
        //{
        //    using (var stream = new MemoryStream())
        //    {
        //        await file.CopyToAsync(stream);
        //        using (var package = new ExcelPackage(stream))
        //        {
        //            int WorkSheetCount = 3;
        //            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //            ExcelWorksheet worksheet = package.Workbook.Worksheets[WorkSheetCount];
        //            var rowcount = worksheet.Dimension.Rows;
        //            for (int row = 5; /*row < rowcount - 1*/; row++)
        //            {

        //                var StringProductName = worksheet.Cells[row, 1].Value.ToString();
        //                var CountingNumber = StringProductName.Substring(0, StringProductName.IndexOf('.', StringProductName.IndexOf('.', StringProductName.LastIndexOf('.') + 1) + 1));
        //                var name = StringProductName.Replace(CountingNumber, "").Substring(1);
        //              var s = name.Substring(1);

        //            }


        //        }

        //    }
        //}
    }
}
