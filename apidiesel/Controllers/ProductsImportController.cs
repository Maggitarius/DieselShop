using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using apidiesel.Models;

namespace apidiesel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsImportController : ControllerBase
    {
        private readonly dieselContext _context;

        public ProductsImportController(dieselContext context)
        {
            _context = context;
        }

        [HttpGet("template")]
        public IActionResult DownloadTemplate()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {
                using var stream = new MemoryStream();
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.Add("ProductsTemplate");

                    worksheet.Cells[1, 1].Value = "ProductName";
                    worksheet.Cells[1, 2].Value = "Sku";
                    worksheet.Cells[1, 3].Value = "PurchasePrice";
                    worksheet.Cells[1, 4].Value = "SalePrice";
                    worksheet.Cells[1, 5].Value = "CategoryId";
                    worksheet.Cells[1, 6].Value = "ImageUrl";
                    worksheet.Cells[1, 7].Value = "Description";

                    worksheet.Cells[2, 1].Value = "Пример товара";
                    worksheet.Cells[2, 2].Value = "SKU123";
                    worksheet.Cells[2, 3].Value = 100.00m;
                    worksheet.Cells[2, 4].Value = 150.00m;
                    worksheet.Cells[2, 5].Value = 1;
                    worksheet.Cells[2, 6].Value = "https://example.com/image.jpg";
                    worksheet.Cells[2, 7].Value = "Описание примера товара";

                    if (worksheet.Dimension != null)
                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    package.Save();
                }

                stream.Position = 0;
                return File(stream,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "ProductsTemplate.xlsx");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Ошибка генерации Excel");
            }
        }

        [HttpPost("upload")]
        public IActionResult UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Файл не выбран.");

            // Указываем лицензию EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var products = new List<Product>();

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.First();
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var productName = worksheet.Cells[row, 1].Text;
                        var sku = worksheet.Cells[row, 2].Text;
                        var purchasePrice = decimal.TryParse(worksheet.Cells[row, 3].Text, out var pp) ? pp : 0;
                        var salePrice = decimal.TryParse(worksheet.Cells[row, 4].Text, out var sp) ? sp : 0;
                        var categoryId = int.TryParse(worksheet.Cells[row, 5].Text, out var cat) ? cat : 0;
                        var imageUrl = worksheet.Cells[row, 6].Text;
                        var description = worksheet.Cells[row, 7].Text;

                        if (!string.IsNullOrEmpty(productName))
                        {
                            products.Add(new Product
                            {
                                ProductName = productName,
                                Sku = sku,
                                PurchasePrice = purchasePrice,
                                SalePrice = salePrice,
                                CategoryId = categoryId,
                                ImageUrl = imageUrl,
                                Description = description
                            });
                        }
                    }
                }
            }

            if (products.Any())
            {
                _context.Products.AddRange(products);
                _context.SaveChanges();
                return Ok(new { message = $"{products.Count} товаров импортировано." });
            }

            return BadRequest("Нет данных для импорта.");
        }
    }
}
