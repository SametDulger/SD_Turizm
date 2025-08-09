using FluentValidation;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.Application.Validators
{
    public class SaleValidator : AbstractValidator<Sale>
    {
        public SaleValidator()
        {
            RuleFor(x => x.PNRNumber)
                .NotEmpty().WithMessage("PNR numarası boş olamaz")
                .MaximumLength(20).WithMessage("PNR numarası 20 karakterden uzun olamaz")
                .Matches("^[A-Z0-9]+$").WithMessage("PNR numarası sadece büyük harf ve rakam içerebilir");

            RuleFor(x => x.CariCode)
                .NotEmpty().WithMessage("Cari kodu boş olamaz")
                .MaximumLength(50).WithMessage("Cari kodu 50 karakterden uzun olamaz");

            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Müşteri adı boş olamaz")
                .MaximumLength(200).WithMessage("Müşteri adı 200 karakterden uzun olamaz");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Para birimi boş olamaz")
                .Length(3).WithMessage("Para birimi 3 karakter olmalıdır")
                .Matches("^[A-Z]{3}$").WithMessage("Para birimi 3 büyük harf olmalıdır");

            RuleFor(x => x.TotalAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Toplam tutar negatif olamaz");

            RuleFor(x => x.SalePrice)
                .GreaterThanOrEqualTo(0).WithMessage("Satış fiyatı negatif olamaz");

            RuleFor(x => x.PurchasePrice)
                .GreaterThanOrEqualTo(0).WithMessage("Alış fiyatı negatif olamaz");

            RuleFor(x => x.TotalAmountTL)
                .GreaterThanOrEqualTo(0).WithMessage("Toplam tutar TL negatif olamaz");

            RuleFor(x => x.AgencyCode)
                .MaximumLength(50).WithMessage("Acenta kodu 50 karakterden uzun olamaz")
                .When(x => !string.IsNullOrEmpty(x.AgencyCode));

            RuleFor(x => x.PackageCode)
                .MaximumLength(50).WithMessage("Paket kodu 50 karakterden uzun olamaz")
                .When(x => !string.IsNullOrEmpty(x.PackageCode));

            RuleFor(x => x.SellerType)
                .MaximumLength(50).WithMessage("Satıcı tipi 50 karakterden uzun olamaz")
                .When(x => !string.IsNullOrEmpty(x.SellerType));

            RuleFor(x => x.FileCode)
                .MaximumLength(50).WithMessage("Dosya kodu 50 karakterden uzun olamaz")
                .When(x => !string.IsNullOrEmpty(x.FileCode));

            RuleFor(x => x.ProductName)
                .MaximumLength(100).WithMessage("Ürün adı 100 karakterden uzun olamaz")
                .When(x => !string.IsNullOrEmpty(x.ProductName));

            RuleFor(x => x.SaleDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Satış tarihi gelecek bir tarih olamaz");
        }
    }
}
